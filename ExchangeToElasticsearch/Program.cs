using System;
using System.Linq;
using LumiSoft.Net.Mail;
using LumiSoft.Net.POP3.Client;

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

            using (var pop3 = new POP3_Client())
            {
                pop3.Connect(pop3_server, pop3_port, true);
                pop3.Login(user, pwd);

                var messages = pop3.Messages;
                Console.WriteLine("Message Count: {0}", messages.Count);
                Console.WriteLine("Sample Mail: ");

                var sampleMessage = messages[0];
                IndexMessage(sampleMessage);

                sampleMessage.MarkForDeletion();
            }

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        private static void IndexMessage(POP3_ClientMessage sampleMessage)
        {
            var messageBytes = sampleMessage.MessageToByte();

            var mimeMessage = Mail_Message.ParseFromByte(messageBytes);
            Console.WriteLine("Sender : {0}", mimeMessage.From);
            var receiver = String.Join(";", mimeMessage.To);
            Console.WriteLine("Receiver: {0}", receiver);
            Console.WriteLine("Body: {0}", mimeMessage.BodyText ?? "Content is null");
        }
    }
}
