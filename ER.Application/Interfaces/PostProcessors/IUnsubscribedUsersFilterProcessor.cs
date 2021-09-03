using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER.Application.Interfaces.PostProcessors
{
    /*
     * Unsubscribed Users Filter PostProcessor 
    */
    public interface IUnsubscribedUsersFilterProcessor<T> : IPostProcessor<T> where T : class
    {
    }
}
