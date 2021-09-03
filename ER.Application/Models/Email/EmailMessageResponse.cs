using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER.Application.Models.Email
{
    public class EmailMessageResponse
    {
        public EmailMessageResponse(string to)
        {
            To = to;
            MessageId = Guid.NewGuid().ToString();
            SubmittedAt = DateTime.UtcNow;
            IsSuccessful = true;
        }

        public string To { get; set; }
        public bool IsSuccessful { get; set; }
        public DateTime SubmittedAt { get; set; }
        public string MessageId { get; set; }
        public string ErrorCode { get; set; }
        public string Message { get; set; }
    }
}
