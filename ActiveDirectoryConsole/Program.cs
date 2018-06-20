using System;
using System.Collections.Generic;
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

        private readonly IList<Account> _accounts = new List<Account>();
        private const string InternetAccessPermissionGroupRegex = "WG_*";
        private const string VpnPermissionGroupRegex = "SVN-*";
        private const string MailboxPermissionGroup = "AuthorizedMailbox";
        private const string UsbPermissionGroup = "EnabledUSB_R&W";


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
                        var directoryEntry = resEnt.GetDirectoryEntry();
                        Console.WriteLine(resEnt.Path);
                        ListEntryProperties(directoryEntry);
                    }
                }
            }
        }

        private static void ListEntryProperties(DirectoryEntry entry)
        {
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
