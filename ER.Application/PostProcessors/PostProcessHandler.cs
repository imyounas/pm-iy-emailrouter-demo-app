using ER.Application.Interfaces.PostProcessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER.Application.PostProcessors
{
    public abstract class PostProcessHandler<T> : IPostProcessor<T> where T : class
    {
        protected IPostProcessor<T> NextProcessor { get; set; }
        public IPostProcessor<T> Next(IPostProcessor<T> nextProcessor)
        {
            NextProcessor = nextProcessor;
            // to link the next processor in chain
            return nextProcessor;
        }

        public abstract Task ProcessItAsync(T request);

        public async Task CallNextAsync(T request)
        {
            if (NextProcessor != null)
            {
                await NextProcessor.ProcessItAsync(request);
            }
        }
    }
}
