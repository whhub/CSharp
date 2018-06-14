using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExchangeToElasticsearch.Tests
{
    [TestClass]
    public class MailTests
    {
        #region [---HasSentOut Tests---]

        [TestMethod]
        public void If_From_Outer_MailBox_Then_not_send_out()
        {
            var mail = new Mail {From = new[] {"xxx"}};
            
            Assert.IsFalse(mail.HasSentOut);
        }

        [TestMethod]
        public void If_From_inner_mailBox_And_To_outer_mailbox_Then_is_send_out()
        {
            // Arrange
            var mail = new Mail {From = new[] {Mail.InnerDomain}, To = new []{"xxx"} };
            // Act

            // Assert
            Assert.IsTrue(mail.HasSentOut);
        }

        [TestMethod]
        public void If_From_inner_mailBox_And_Cc_outer_mailbox_Then_is_send_out()
        {
            // Arrange
            var mail = new Mail { From = new[] { Mail.InnerDomain }, Cc = new[] { "xxx" } };
            // Act

            // Assert
            Assert.IsTrue(mail.HasSentOut);
        }

        [TestMethod]
        public void If_From_inner_mailBox_And_Bcc_outer_mailbox_Then_is_send_out()
        {
            // Arrange
            var mail = new Mail { From = new[] { Mail.InnerDomain }, Bcc = new[] { "xxx" } };
            // Act

            // Assert
            Assert.IsTrue(mail.HasSentOut);
        }
        #endregion

    }
}
