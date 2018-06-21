using System;
using System.Collections.Generic;
using System.Linq;
using Nest;

namespace EmployeePermissionInfoToElasticsearch
{
    [ElasticsearchType(Name = "Account")]
    public class Account
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Department { get; set; }
        public string DistinDistinguishedName { get; set; }

        public IList<string> InternetAccessPermissions => GetPermissions(InternetAccessPermissionGroup);
        public IList<string> InternetAccessPermissionGroups => GetPermissionGroups(InternetAccessPermissionGroup);
        public IList<string> VpnPermissions => GetPermissions(VpnPermissionGroup);
        public IList<string> VpnPermissionGroups => GetPermissionGroups(VpnPermissionGroup);
        public IList<string> MailPermissions => GetPermissions(MailboxPermissionGroup);
        public IList<string> MailPermissionGroups => GetPermissionGroups(MailboxPermissionGroup);
        public IList<string> UsbPermissions => GetPermissions(UsbPermissionGroup);
        public IList<string> UsbPermissionGroupss => GetPermissionGroups(UsbPermissionGroup);

        public IList<Group> Groups { get; } = new List<Group>();


        private IList<string> GetPermissions(string groupNamePattern)
        {
            return Groups.Where(g => g.Name.IndexOf(groupNamePattern, StringComparison.Ordinal) == 0).Select(g => g.Description)
                .ToList();
        }

        private IList<string> GetPermissionGroups(string groupNamePattern)
        {
            return Groups.Where(g => g.Name.IndexOf(groupNamePattern, StringComparison.Ordinal) == 0).Select(g => g.Name)
                .ToList();
        }

        private const string InternetAccessPermissionGroup = "WG_";
        private const string VpnPermissionGroup = "SVN-";
        private const string MailboxPermissionGroup = "AuthorizedMailbox";
        private const string UsbPermissionGroup = "EnabledUSB_R&W";
    }
}