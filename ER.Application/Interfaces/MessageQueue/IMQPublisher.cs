using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER.Application.Interfaces.MessageQueue
{
    public interface IMQPublisher
    {
        /*
        * Publish Message to Queue
        */
        Task<bool> PublishAsync<T>(string exchangeName, string queue, T message, string routeKey);
    }
}
