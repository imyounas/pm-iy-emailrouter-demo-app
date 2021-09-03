using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER.Application.Interfaces.PostProcessors
{
    /*
     * Email Body NLP PostProcessor 
    */
    public interface IEmailBodyNLPProcessor<T> : IPostProcessor<T> where T : class
    {
    }
}
