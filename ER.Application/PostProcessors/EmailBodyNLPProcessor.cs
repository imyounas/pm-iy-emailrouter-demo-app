using ER.Application.Interfaces.PostProcessors;
using ER.Application.Models.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER.Application.PostProcessors
{
    public class EmailBodyNLPProcessor : PostProcessHandler<EmailMessageRequest>, IEmailBodyNLPProcessor<EmailMessageRequest>
    {
        public override async Task ProcessItAsync(EmailMessageRequest request)
        {
            // some NLP processing to assign TnC Rating/Status
            request.PostProcessedMetaData.PMTnCRatings = EPMTnCRatings.AllGood;

            await CallNextAsync(request);
        }
    }
}
