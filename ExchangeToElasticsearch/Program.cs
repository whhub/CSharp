using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using LumiSoft.Net.Mail;
using LumiSoft.Net.MIME;
using LumiSoft.Net.POP3.Client;
using Nest;

namespace ExchangeToElasticsearch
{
    class Program
    {
        private const string SupervisorServiceUrl = @"http://10.6.14.157:8008/EmployeeService.svc/Supervisor?Id=";

        static void Main()
        {
            var user = "softtester";
            var pwd = "xAcmQ3gg";
            const string pop3Server = "smtp.united-imaging.com";
            const int pop3Port = 995;

            // Connect Elasticsearch
            var node = new Uri("http://10.6.14.157:9200");
            var elasticUser = "elastic";
            var elasticPwd = "123qwe";
            var setting = new ConnectionSettings(node);
            setting.BasicAuthentication(elasticUser, elasticPwd);
            var elasticSearchClient = new ElasticClient(setting);
            

            // Index Mapping
            var descriptor = new CreateIndexDescriptor("mail")
                .Mappings(ms=>ms.Map<Mail>(m=>m.AutoMap()));
            elasticSearchClient.CreateIndex(descriptor);

            // Scheduled Mail Collecting
            while (true)
            {
                using (var pop3 = new POP3_Client())
                {
                    pop3.Connect(pop3Server, pop3Port, true);
                    pop3.Login(user, pwd);

                    var messages = pop3.Messages;
                    var messagesCount = messages.Count;
                    Console.WriteLine("Message Count: {0}", messagesCount);

                    for(int i=0; i<messagesCount; i++)
                    {
                        Console.WriteLine("Indexing Mail: {0} of {1}", i, messagesCount);
                        var message = messages[i];
                        var mail = IndexMessage(message);
                        CheckMailToToSupervisor(mail);
                        var respose = elasticSearchClient.Index(mail, idx => idx.Index("mail"));
                        Console.WriteLine("Indexed a mail with respose : {0}", respose.Result.ToString());
                        Console.WriteLine();

                        message.MarkForDeletion();
                    }
                    pop3.Disconnect();
                }

                Thread.Sleep(5 * 60 * 1000);
            }

            //Console.WriteLine("Press any key to continue");
            //Console.ReadKey();
        }

        private static void CheckMailToToSupervisor(Mail mail)
        {
            var from = mail.From.FirstOrDefault();
            if (null == from) return;
            Console.WriteLine($"Mail From {from}");
            var domainIndex = from.IndexOf(Mail.InnerDomain, StringComparison.Ordinal);
            if (-1 == domainIndex) return;

            var id = from.Substring(0, domainIndex);
            Console.WriteLine($"Id: {id}");
            var supervisor = HttpGetSupervisor(id);
            Console.WriteLine($"Supervisior: {supervisor}");

            mail.HasSentToSupervisor = (mail.To != null && mail.To.Any(address => address.Contains(supervisor)))
                                       || (mail.Cc != null && mail.Cc.Any(address => address.Contains(supervisor)))
                                       || (mail.Bcc != null && mail.Bcc.Any(address => address.Contains(supervisor)));
        }

        private static string HttpGetSupervisor(string id)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SupervisorServiceUrl + id);
            request.Method = "GET";
            request.ContentType = "text/json;charset=UTF-8";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream myResponseStream = response.GetResponseStream())
                {
                    if(null == myResponseStream) {return string.Empty;}
                    StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                    string retString = myStreamReader.ReadToEnd();

                    return retString.Substring(1,retString.Length-2); // remove ""
                }
            }
        }

        private static Mail IndexMessage(POP3_ClientMessage sampleMessage)
        {
            var messageBytes = sampleMessage.MessageToByte();

            var mimeMessage = Mail_Message.ParseFromByte(messageBytes);
            var body = mimeMessage.BodyText ?? "Content is null";
            Console.WriteLine("Body: {0}", body);

            var mail = new Mail();
            var from = mimeMessage.From;
            if(null != from) {mail.From = from.ToArray().Select(m=>m.Address).ToArray();}
            var to = mimeMessage.To;
            if(null != to) {mail.To = to.Mailboxes.Select(m => m.Address).ToArray();}
            var cc = mimeMessage.Cc;
            if(null != cc) {mail.Cc = cc.Mailboxes.Select(m => m.Address).ToArray();}
            var bcc = mimeMessage.Bcc;
            if(null != bcc) {mail.Bcc = bcc.Mailboxes.Select(m => m.Address).ToArray();}

            mail.Subject = mimeMessage.Subject;
            mail.SentTime = mimeMessage.Date;            

            var attachments = mimeMessage.Attachments;
            if (null == attachments || 0 == attachments.Length) return mail;


            //获取原件
            var originMail = attachments[0];
            var originMailBody = originMail.Body as MIME_b_MessageRfc822;
            var originMailMessage = originMailBody?.Message;
            if (originMailMessage != null)
            {
                mail.Body = originMailMessage.BodyText;
                mail.MessageId = originMailMessage.MessageID;
            }

            var attachmentCount = attachments.Length - 1;
            if (attachmentCount <= 0) return mail;

            //获取附件
            var attachmentNameList = new List<string>();
            for (int i = 0; i < attachmentCount; i++)
            {
                var attachment = attachments[i + 1];
                var attachmentName = attachment.ContentDescription;
                if (string.IsNullOrEmpty(attachmentName))
                {
                    var attachmentMailBody = attachment.Body as MIME_b_MessageRfc822;
                    var attachmentMailMessage = attachmentMailBody?.Message;
                    if (null != attachmentMailMessage) {attachmentName = attachmentMailMessage.Subject;}
                    if(string.IsNullOrEmpty(attachmentName)) {attachmentName = attachment.Body?.MediaType;}
                }
                attachmentNameList.Add(attachmentName);
            }
            if (attachmentNameList.Count > 0) mail.Attachments = attachmentNameList.ToArray();

            //mail.Body = mimeMessage.BodyText;


            return mail;
        }
    }
}
