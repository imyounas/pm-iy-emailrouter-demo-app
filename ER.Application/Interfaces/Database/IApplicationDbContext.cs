using ER.Application.Models.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER.Application.Interfaces.Database
{
    public interface IApplicationDbContext
    {
        /*
         * Update DB for post processed, passed and failed email requests
         */
        Task<bool> UpdatePostProcessedEmailStatusAsync(EmailMessageRequest emailRequest);

        /*
         * Update DB for pre processed, passed and failed email requests
         */
        Task<bool> UpdatePreProcessedEmailStatusAsync(EmailMessageRequest emailRequest, EmailMessageResponse response);

        /*
         * Check DB for To email addresses who have unsubscribed
         */
        Task<List<string>> FilterUnsubscribedAddressesAsync(List<string> toAddresses);
    }
}
