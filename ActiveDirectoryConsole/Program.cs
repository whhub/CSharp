using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveDirectoryConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            GetAllAccounts();
            Console.WriteLine("Enter Any Key to Exit");
            Console.ReadKey();
        }

        private static readonly IList<Account> _accounts = new List<Account>();
        private static readonly IList<Group> _groups = new List<Group>();
        private const string InternetAccessPermissionGroupRegex = "WG_*";
        private const string VpnPermissionGroupRegex = "SVN-*";
        private const string MailboxPermissionGroup = "AuthorizedMailbox";
        private const string UsbPermissionGroup = "EnabledUSB_R&W";

        private const string LDAP_CN = "cn";
        private const string LDAP_Description = "Description";
        private const string LDAP_DistinguishedName = "distinguishedName";
        private const string LDAP_Member = "member";
        private const string LDAP_AcountName = "sAMAccountName";


        private const string LdapOuUsersOuUnitedImagingDcUnitedImagingDcCom = "LDAP://OU=Users,OU=United_Imaging,DC=united-imaging,DC=com";


        private static void GetAllAccounts()
        {
            TraverseAdGroup(InternetAccessPermissionGroupRegex);
            TraverseAdGroup(VpnPermissionGroupRegex);
            TraverseAdGroup(MailboxPermissionGroup);
            TraverseAdGroup(UsbPermissionGroup);
        }

        private static void TraverseAdGroup(string cnRegex)
        {
            using (var entry = new DirectoryEntry(LdapOuUsersOuUnitedImagingDcUnitedImagingDcCom))
            {
                using (var directorySearcher = new DirectorySearcher(entry))
                {
                    directorySearcher.Filter = $"(&(objectClass=group)(CN={cnRegex}))";

                    foreach (SearchResult resEnt in directorySearcher.FindAll())
                    {
                        var groupEntry = resEnt.GetDirectoryEntry();
                        ListEntryProperties(groupEntry);
                        TraverseGroupMember(groupEntry);
                    }
                }
            }
        }

        private static void TraverseGroupMember(DirectoryEntry groupEntry)
        {
            var groupName = groupEntry.Properties[LDAP_CN][0] as string;
            var groupDescription = groupEntry.Properties[LDAP_Description][0] as string;
            var componentName = groupEntry.Properties[LDAP_DistinguishedName][0] as string;
            _groups.Add(new Group {Name = groupName, Description = groupDescription, DistinguishedName = componentName});

            foreach (var member in groupEntry.Properties[LDAP_Member])
            {
                var memberPath = $"LDAP://{member}";
                using (var memberEntry = new DirectoryEntry(memberPath))
                {
                    ListEntryProperties(memberEntry);
                    var memberDistinguishedName = memberEntry.Properties[LDAP_DistinguishedName][0] as string;
                    var account = _accounts.FirstOrDefault(a => a.DistinDistinguishedName == memberDistinguishedName);
                    account = account ?? new Account {DistinDistinguishedName = memberDistinguishedName};
                    var descriptionCollection = memberEntry.Properties[LDAP_Description];
                    account.Description = descriptionCollection.Count>0 ? descriptionCollection[0] as string : string.Empty;
                    account.Name = memberEntry.Properties[LDAP_AcountName][0] as string;
                }

            }
        }

        [Conditional("DEBUG")]
        private static void ListEntryProperties(DirectoryEntry entry)
        {
            Console.WriteLine(entry.Path);
            foreach (string key in entry.Properties.PropertyNames)
            {
                Console.WriteLine(key + " = ");
                foreach (var obj in entry.Properties[key])
                {
                    Console.WriteLine("\t" + obj);
                    
                }
            }
        }
    }
}
