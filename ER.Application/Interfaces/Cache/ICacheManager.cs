using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER.Application.Interfaces.Cache
{
    public interface ICacheManager
    {
        /*
         * this will validate incoming server token and return client Id against it
         */
        Guid ValidateTokenAndSendClientId(string serverToken);

        /*
         * this will validate that given sender is a verified sender against a given clientId
         */
        bool VerifySender(Guid clientId, string sender);

    }
}
