using System;
using System.Linq;
using Nest;

namespace ExchangeToElasticsearch
{
    [ElasticsearchType(Name="Mail")]
    class Mail
    {
        private const string InnerDomain = "@united-imaging.com";

        public string MessageId { get; set; }
        public string[] From { get; set; }
        public string[] To { get; set; }
        public string[] Cc { get; set; }
        public string[] Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string[] Attachments { get; set; }
        public DateTime SentTime { get; set; }

        //TODO: Access people soft view web service, to get hierarchy
        public bool HasSentToSupervisor { get; set; }

        public bool FromManagership { get; set; }


        public bool HasSentOut
        {
            get
            {
                return ((null != To && To.Any(address => !address.Contains(InnerDomain)))
                       || (null != Cc && Cc.Any(address => !address.Contains(InnerDomain)))
                       || (null != Bcc && Bcc.Any(address => !address.Contains(InnerDomain))))
                       && (null != From && From.All(address => address.Contains(InnerDomain)));
            }
        }

        public bool HasAttachment => null != Attachments && Attachments.Length > 0;
    }
}
