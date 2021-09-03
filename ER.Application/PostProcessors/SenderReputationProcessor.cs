using ER.Application.Interfaces.PostProcessors;
using ER.Application.Models.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER.Application.PostProcessors
{
    public class SenderReputationProcessor : PostProcessHandler<EmailMessageRequest>, ISenderReputationProcessor<EmailMessageRequest>
    {
        public override async Task ProcessItAsync(EmailMessageRequest request)
        {

            if (request.PostProcessedMetaData.FilteredRecepientsPercentage > 0)
            {
                // some dummy logic to calculate Sender Reputation Score
                request.PostProcessedMetaData.SenderReputationScore = request.PostProcessedMetaData.FilteredRecepientsPercentage;

                await CallNextAsync(request);
            }
            else
            {
                // do not process remaning chain , if any
                return;
            }
        }
    }
}
