using System.Collections.Generic;
using Nest;

namespace ActiveDirectoryConsole
{
    [ElasticsearchType(Name = "Account")]
    public class Account
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Department { get; set; }
        public string DistinDistinguishedName { get; set; }

        public IList<Group> InternetAccessPermissions { get; } = new List<Group>();
        public IList<Group> VpnPermissions { get; } = new List<Group>();
        public IList<Group> MailPermissions { get; } = new List<Group>();
        public IList<Group> UsbPermissions { get; } = new List<Group>();

        public IList<Group> Groups = new List<Group>();
    }
}