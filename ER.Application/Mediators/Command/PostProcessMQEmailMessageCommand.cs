using ER.Application.Common;
using ER.Application.Interfaces.Database;
using ER.Application.Interfaces.MessageQueue;
using ER.Application.Interfaces.PostProcessors;
using ER.Application.Models.Email;
using ER.Application.PostProcessors;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ER.Application.Mediators.Command
{
    /*
    * MediatR Command to Post Process Injested EMail Messages
   */
    public class PostProcessMQEmailMessageCommand : IRequest<bool>
    {
        public EmailMessageRequest EmailRequestMessage { get; init; }
    }

    public class ProcessMQEmailMessageCommandHandler : IRequestHandler<PostProcessMQEmailMessageCommand, bool>
    {

        private readonly IMQPublisher _mqPublisher;
        private readonly AppSettings _appSettings;
        private readonly IApplicationDbContext _dbContext;

        private readonly UnsubscribedUsersFilterProcessor _unsubUsersFilterProcessor;
        private readonly EmailBodyNLPProcessor _emailBodyNLPProcessor;
        private readonly SenderReputationProcessor _senderReputationProcessor;
        private readonly MessageReputationProcessor _messageReputationProcessor;
        public ProcessMQEmailMessageCommandHandler(IMQPublisher mqPublisher, AppSettings appSettings,
            IApplicationDbContext dbContext,
            IUnsubscribedUsersFilterProcessor<EmailMessageRequest> unsubUsersFilterProcessor,
            IEmailBodyNLPProcessor<EmailMessageRequest> emailBodyNLPProcessor,
            ISenderReputationProcessor<EmailMessageRequest> senderReputationProcessor,
            IMessageReputationProcessor<EmailMessageRequest> messageReputationProcessor)
        {
            _appSettings = appSettings;
            _mqPublisher = mqPublisher;
            _dbContext = dbContext;

            // // TODO: Need to check here , for time being satisfying compiler with explicit conversion
            _unsubUsersFilterProcessor = (UnsubscribedUsersFilterProcessor)unsubUsersFilterProcessor;
            _emailBodyNLPProcessor = (EmailBodyNLPProcessor)emailBodyNLPProcessor;
            _senderReputationProcessor = (SenderReputationProcessor)senderReputationProcessor;
            _messageReputationProcessor = (MessageReputationProcessor)messageReputationProcessor;
        }

        public async Task<bool> Handle(PostProcessMQEmailMessageCommand request, CancellationToken cancellationToken)
        {
            bool response = true;
            bool isEmailEligableToSend = true;

            // chain of responsibility patteren to do the post processing

            _unsubUsersFilterProcessor
                .Next(_senderReputationProcessor)
                .Next(_emailBodyNLPProcessor)
                .Next(_messageReputationProcessor);

            await _unsubUsersFilterProcessor.ProcessItAsync(request.EmailRequestMessage);

            if (request.EmailRequestMessage.PostProcessedMetaData.IsInviolationOfTnC
            || request.EmailRequestMessage.PostProcessedMetaData.MessageReputationScore < 2
            || request.EmailRequestMessage.PostProcessedMetaData.SenderReputationScore < 2)
            {
                isEmailEligableToSend = false;
            }

            if (!isEmailEligableToSend)
            {
                // may be notify via Email to customers with error report
                var failRes = await _mqPublisher.PublishAsync<EmailMessageRequest>(_appSettings.QueueExchange, _appSettings.FailMessageQueue, request.EmailRequestMessage, "");
                response = false;
                return response;
            }


            // persist the result status in DB for client dashboard/report
            await _dbContext.UpdatePostProcessedEmailStatusAsync(request.EmailRequestMessage);

            string queueName = request.EmailRequestMessage.MessageMetaData.Type == EEmailType.Transactional ? _appSettings.TransMessageQueue : _appSettings.BulkMessageQueue;
            var res = await _mqPublisher.PublishAsync<EmailMessageRequest>(_appSettings.QueueExchange, queueName, request.EmailRequestMessage, "");

            // return response to client
            return response;
        }
    }
}
