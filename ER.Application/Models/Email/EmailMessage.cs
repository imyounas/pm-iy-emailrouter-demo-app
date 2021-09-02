using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER.Application.Models.Email
{
    public class EmailMessage
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public string Tag { get; set; }
        public string HtmlBody { get; set; }
        public string TextBody { get; set; }
        public string ReplyTo { get; set; }
        public IEnumerable<CustomtHeader> Headers { get; set; }
        public bool TrackOpens { get; set; }
        public string TrackLinks { get; set; }
        public IDictionary<string, object> Metadata { get; set; }
        public IEnumerable<EmailAttachment> Attachments { get; set; }
        public string MessageStream { get; set; }
    }
}
