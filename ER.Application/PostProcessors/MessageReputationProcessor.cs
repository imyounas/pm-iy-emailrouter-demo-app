using ER.Application.Interfaces.PostProcessors;
using ER.Application.Models.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER.Application.PostProcessors
{
    public class MessageReputationProcessor : PostProcessHandler<EmailMessageRequest>, IMessageReputationProcessor<EmailMessageRequest>
    {

        public override async Task ProcessItAsync(EmailMessageRequest request)
        {
            // some dummy logic to check if its a welcome or promotional email
            if (request.Message.Subject.Contains("Welcome"))
            {
                request.PostProcessedMetaData.MessageReputationScore = 100;
            }
            else
            {
                request.PostProcessedMetaData.MessageReputationScore = 60;
            }

            await CallNextAsync(request);
        }
    }
}
