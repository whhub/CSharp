using System;
using System.Linq;
using LumiSoft.Net.Mail;
using LumiSoft.Net.POP3.Client;
using Nest;

namespace ExchangeToElasticsearch
{
    class Program
    {
        static void Main(string[] args)
        {
            var user = "softtester";
            var pwd = "xAcmQ3gg";
            var pop3_server = "smtp.united-imaging.com";
            var pop3_port = 995;

            // Connect Elasticsearch
            var node = new Uri("http://localhost:9200");
            var setting = new ConnectionSettings(node);
            var elasticSearchClient = new ElasticClient(setting);
            
            // Index Mapping
            var descriptor = new CreateIndexDescriptor("mail")
                .Mappings(ms=>ms.Map<Mail>(m=>m.AutoMap()));
            elasticSearchClient.CreateIndex(descriptor);

            using (var pop3 = new POP3_Client())
            {
                pop3.Connect(pop3_server, pop3_port, true);
                pop3.Login(user, pwd);

                var messages = pop3.Messages;
                Console.WriteLine("Message Count: {0}", messages.Count);
                Console.WriteLine("Sample Mail: ");

                var sampleMessage = messages[0];
                var mail = IndexMessage(sampleMessage);
                var respose = elasticSearchClient.Index(mail, idx => idx.Index("mail"));
                Console.WriteLine("Indexed a mail with respose : {0}", respose.Result.ToString());
                Console.WriteLine();

                sampleMessage.MarkForDeletion();
            }

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        private static Mail IndexMessage(POP3_ClientMessage sampleMessage)
        {
            var messageBytes = sampleMessage.MessageToByte();

            var mimeMessage = Mail_Message.ParseFromByte(messageBytes);
            Console.WriteLine("Body: {0}", mimeMessage.BodyText ?? "Content is null");

            var mail = new Mail();
            mail.From = mimeMessage.From.ToArray().Select(m=>m.Address).ToArray();
            mail.To = mimeMessage.To.Mailboxes.Select(m => m.Address).ToArray();
            var cc = mimeMessage.Cc;
            if(null != cc) {mail.Cc = cc.Mailboxes.Select(m => m.Address).ToArray();}
            var bcc = mimeMessage.Bcc;
            if(null != bcc) {mail.Bcc = bcc.Mailboxes.Select(m => m.Address).ToArray();}
            mail.Subject = mimeMessage.Subject;
            mail.Body = mimeMessage.BodyText;
            mail.SentTime = mimeMessage.Date;

            return mail;
        }
    }
}
