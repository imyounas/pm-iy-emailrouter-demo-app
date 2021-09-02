using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER.Application.Models.Email
{
    public class MessageMetaData
    {
        public Guid ClientId { get; set; }
        public string Version { get; set; }
        public EEmailType Type { get; set; }
        public List<string> VerifiedSenders { get; set; }
        public Dictionary<string, string> RequestHTTPHeaders { get; set; }

    }
}
