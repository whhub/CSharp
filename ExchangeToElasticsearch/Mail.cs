using System;
using Nest;

namespace ExchangeToElasticsearch
{
    [ElasticsearchType(Name="Mail")]
    class Mail
    {
        public string From { get; set; }
        public string[] To { get; set; }
        public string[] Cc { get; set; }
        public string[] Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string[] Attachments { get; set; }
        public DateTime SentTime { get; set; }
        public bool HasSentOut { get; set; }
        public bool HasAttachment { get; set; }
    }
}
