using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER.Application.Common
{
    public record AppSettings
    {
        public string QueueExchange { get; init; }
        public string QueueHost { get; set; }
        public string QueueUserName { get; set; }
        public string QueuePassword { get; set; }
        public int QueuePort { get; set; }
        public string MessageInjestQueue { get; init; }
        public string ProcessTransQueue { get; init; }
        public string ProcessBatchQueue { get; init; }
    }
}
