using ER.Application.Common;
using ER.Application.Interfaces.Cache;
using ER.Application.Interfaces.Database;
using ER.Application.Interfaces.MessageQueue;
using ER.Application.Models.Email;
using ER.Application.Validators.Email;
using FluentValidation.Results;
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
     * MediatR Command to Injest incomming EMail Messages to Queue
    */
    public class IngestEmailMessageCommand : IRequest<EmailMessageResponse>
    {
        public EmailMessageRequest EmailRequestMessage { get; init; }
    }

    public class IngestEmailMessageCommandHandler : IRequestHandler<IngestEmailMessageCommand, EmailMessageResponse>
    {

        private readonly IMQPublisher _mqPublisher;
        private readonly ICacheManager _cacheManager;
        private readonly IApplicationDbContext _dbContext;
        private readonly AppSettings _appSettings;

        public IngestEmailMessageCommandHandler(IMQPublisher mqPublisher, AppSettings appSettings, ICacheManager cacheManager, IApplicationDbContext dbContext)
        {
            _appSettings = appSettings;
            _mqPublisher = mqPublisher;
            _cacheManager = cacheManager;
            _dbContext = dbContext;
        }

        public async Task<EmailMessageResponse> Handle(IngestEmailMessageCommand request, CancellationToken cancellationToken)
        {
            EmailMessageResponse response = new EmailMessageResponse(request.EmailRequestMessage.Message.To);

            if (string.IsNullOrEmpty(request.EmailRequestMessage.Message.MessageStream)
                ||(string.Equals(request.EmailRequestMessage.Message.MessageStream, Constants.TRANS_STREAM, StringComparison.OrdinalIgnoreCase)))
            {
                request.EmailRequestMessage.MessageMetaData.Type = EEmailType.Transactional;
            }
            else
            {
                request.EmailRequestMessage.MessageMetaData.Type = EEmailType.Bulk;
            }

            // using fluent validation for headers & model
            ValidationResult modelResults = new EmailMessagetValidator().Validate(request.EmailRequestMessage.Message);
            ValidationResult headersResults = new EmailRequestHeaderValidator().Validate(request.EmailRequestMessage.MessageMetaData.RequestHTTPHeaders);

            if (!modelResults.IsValid || !headersResults.IsValid)
            {
                response.IsSuccessful = false;
                foreach (var failure in modelResults.Errors)
                {
                    response.ErrorCode = failure.ErrorCode;
                    response.Message = Common.Constants.ErrorCodeMessages[failure.ErrorCode];
                    response.IsSuccessful = false;
                    // we can break on first error or keep going to add all errors in response ?
                    break;
                }

                // save preprocessing data
                await _dbContext.UpdatePreProcessedEmailStatusAsync(request.EmailRequestMessage, response);

                // send the failure response to client after initial validations
                return response;
            }

            //we have already validated that x-server - token exists
           var clientId = _cacheManager.ValidateTokenAndSendClientId(request.EmailRequestMessage.MessageMetaData.RequestHTTPHeaders[Common.Constants.PM_SERVER_TOKEN]);
            var isVerifiedSender = _cacheManager.VerifySender(clientId, request.EmailRequestMessage.Message.From);

            if (!isVerifiedSender)
            {
                response.ErrorCode = Common.Constants.UNVERIFIED_SENDER;
                response.Message = Common.Constants.ErrorCodeMessages[Common.Constants.UNVERIFIED_SENDER];
                response.IsSuccessful = false;

                // save preprocessing data
                await _dbContext.UpdatePreProcessedEmailStatusAsync(request.EmailRequestMessage, response);

                // send the failure response to client after initial validations
                return response;
            }

            //ingest message to queue for post processing
            var isInjested = await _mqPublisher.PublishAsync<EmailMessageRequest>(_appSettings.QueueExchange, _appSettings.MessageInjestQueue, request.EmailRequestMessage, "");

            // send the success response to client after initial validations
            return response;
        }
    }
}
