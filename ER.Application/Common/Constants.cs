using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER.Application.Common
{
    public static class Constants
    {
        public const int MAX_RETAINED_MQ_CONNECTIONS = 5;
        
        public const string TRANS_STREAM = "outbound";

        public const string INVALID_FROM = "E101";
        public const string UNVERIFIED_FROM = "E102";
        public const string INVALID_TO = "E103";
        public const string INVALID_CC = "E104";
        public const string INVALID_BCC = "E105";
        public const string MAX_TO = "E106";
        public const string MAX_CC = "E107";
        public const string MAX_BCC = "E108";
        public const string INVALID_HTML_BODY = "E109";
        public const string INVALID_TEXT_BODY = "E110";
        public const string INVALID_HTML_OR_TEXT_BODY = "E111";
        public const string INVALID_HTTP_HEADERS = "E112";
        public const string UNVERIFIED_SENDER = "E113";

        public const string CONTENT_TYPE_HEADER = "Content-Type";
        public const string ACCEPT_HEADER = "Accept";
        public const string PM_SERVER_TOKEN = "X-Postmark-Server-Token";

        public static readonly Dictionary<string, string> ErrorCodeMessages;
        static Constants()
        {
            ErrorCodeMessages = new Dictionary<string, string>();
            ErrorCodeMessages.Add(INVALID_FROM, "Invalid From Address");
            ErrorCodeMessages.Add(UNVERIFIED_FROM, "Unverified Sender");
            ErrorCodeMessages.Add(INVALID_TO, "Invalid To Address");
            ErrorCodeMessages.Add(INVALID_CC, "Invalid CC Address");
            ErrorCodeMessages.Add(INVALID_BCC, "Invalid BCC Address");
            ErrorCodeMessages.Add(MAX_TO, "To Addresses can't be more than 50");
            ErrorCodeMessages.Add(MAX_CC, "Cc Addresses can't be more than 50");
            ErrorCodeMessages.Add(MAX_BCC, "Bcc Addresses can't be more than 50");
            ErrorCodeMessages.Add(INVALID_HTML_BODY, "Invalid HTML Body");
            ErrorCodeMessages.Add(INVALID_TEXT_BODY, "Invalid Text Body");
            ErrorCodeMessages.Add(INVALID_HTML_OR_TEXT_BODY, "Missing valied Text or HTML Body");
            ErrorCodeMessages.Add(INVALID_HTTP_HEADERS, "Invalid HTTP Headers");
            ErrorCodeMessages.Add(UNVERIFIED_SENDER, "Sender is not verified");
        }
    }
}
