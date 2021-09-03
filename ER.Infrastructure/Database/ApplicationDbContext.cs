using ER.Application.Interfaces.Database;
using ER.Application.Models.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER.Infrastructure.Database
{
    public class ApplicationDbContext : IApplicationDbContext
    {
        public async Task<bool> UpdatePostProcessedEmailStatusAsync(EmailMessageRequest emailRequest)
        {
            return true;
        }

        public async Task<bool> UpdatePreProcessedEmailStatusAsync(EmailMessageRequest emailRequest, EmailMessageResponse response)
        {
            return true;
        }

        public async Task<List<string>> FilterUnsubscribedAddressesAsync(List<string> toAddresses)
        {
            return toAddresses;
        }
    }
}
