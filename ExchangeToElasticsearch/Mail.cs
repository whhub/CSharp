using System;
using System.Linq;
using Nest;

namespace ExchangeToElasticsearch
{
    [ElasticsearchType(Name="Mail")]
    class Mail
    {
        private const string INNER_DOMAIN = "@united-imaging.com";

        public string[] From { get; set; }
        public string[] To { get; set; }
        public string[] Cc { get; set; }
        public string[] Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string[] Attachments { get; set; }
        public DateTime SentTime { get; set; }

        public bool HasSentOut
        {
            get
            {
                return (null != To && To.Any(address => !address.Contains(INNER_DOMAIN)))
                       || (null != Cc && Cc.Any(address => !address.Contains(INNER_DOMAIN)))
                       || (null != Bcc && Bcc.Any(address => !address.Contains(INNER_DOMAIN)));
            }
        }

        public bool HasAttachment => null != Attachments && Attachments.Length > 0;
    }
}
