using ER.Application.Interfaces.Database;
using ER.Application.Interfaces.PostProcessors;
using ER.Application.Models.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER.Application.PostProcessors
{
    public class UnsubscribedUsersFilterProcessor : PostProcessHandler<EmailMessageRequest>, IUnsubscribedUsersFilterProcessor<EmailMessageRequest>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public UnsubscribedUsersFilterProcessor(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public override async Task ProcessItAsync(EmailMessageRequest request)
        {

            var recepietnts = request.Message.To.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var filteredRecepients = await _applicationDbContext.FilterUnsubscribedAddressesAsync(recepietnts);

            // some dummy logic to calculate how many recepietnts were filtered out and then use it for reputation
            request.PostProcessedMetaData.FilteredRecepients = filteredRecepients;
            request.PostProcessedMetaData.FilteredRecepientsPercentage = (filteredRecepients.Count / recepietnts.Count) * 100.0f;

            if (filteredRecepients.Count > 0)
            {
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
