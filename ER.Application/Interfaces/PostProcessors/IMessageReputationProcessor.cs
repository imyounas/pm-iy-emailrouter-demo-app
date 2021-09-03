using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER.Application.Interfaces.PostProcessors
{
    /*
     * Message Reputation PostProcessor 
    */
    public interface IMessageReputationProcessor<T> : IPostProcessor<T> where T : class
    {
    }
}
