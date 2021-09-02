using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER.Application.Models.Email
{
    public class EmailMessageRequest
    {
        public EmailMessageRequest()
        {
            MessageMetaData = new MessageMetaData();
            PostProcessedMetaData = new PostProcessedMetaData();
        }

        public EmailMessage Message { get; set; }
        public MessageMetaData MessageMetaData { get; set; }
        public PostProcessedMetaData PostProcessedMetaData { get; set; }
    }
}
