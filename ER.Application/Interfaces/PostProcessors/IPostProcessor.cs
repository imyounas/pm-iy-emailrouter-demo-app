using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER.Application.Interfaces.PostProcessors
{
    /*
     * Here I was thinkking to go with Visitor Patteren instead of Chain Of Responsibility.
     * But then I decided to go with CoR , as we can mroe elegently chain/plug our new logic/processor and break it anytime when we come across such case
     * And we can also change the order/sequence in which these processors will be executed
     * */
    /*
     * Processor of Chain of Responsibility
    */
    public interface IPostProcessor<T> where T : class
    {
        IPostProcessor<T> Next(IPostProcessor<T> nextProcessor);

        Task ProcessItAsync(T request);
    }
}
