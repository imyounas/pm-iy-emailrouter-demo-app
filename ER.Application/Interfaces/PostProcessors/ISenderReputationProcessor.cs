using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER.Application.Interfaces.PostProcessors
{
    /*
     * Sender Reputation PostProcessor 
    */
    public interface ISenderReputationProcessor<T> : IPostProcessor<T> where T : class
    {
    }
}
