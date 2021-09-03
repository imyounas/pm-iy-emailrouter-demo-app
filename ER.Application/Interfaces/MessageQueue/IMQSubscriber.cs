using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER.Application.Interfaces.MessageQueue
{
    public interface IMQSubscriber
    {
        void SubscribeAsync<T>(Func<T, Task<bool>> callback);
    }
}
