using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using Nest;

namespace EmployeePermissionInfoToElasticsearch
{
    class Program
    {
        static void Main()
        {
            IndexAccounts(GetAllAccounts());
            Console.WriteLine("Enter Any Key to Exit");
            Console.ReadKey();
        }

        private const string LdapCn = "cn";
        private const string LdapDescription = "Description";
        private const string LdapDistinguishedName = "distinguishedName";
        private const string LdapMemberOf = "memberOf";
        private const string LdapAcountName = "sAMAccountName";
        private const string LdapDepartment = "department";


        //private const string LdapOuUsersOuUnitedImagingDcUnitedImagingDcCom = "LDAP://OU=Users,OU=United_Imaging,DC=united-imaging,DC=com";
        private const string LdapDcUnitedImagingDcCom = "LDAP://DC=united-imaging,DC=com";
        private const string LdapOuUnitedImagingDcUnitedImagingDcCom = "LDAP://OU=United_Imaging,DC=united-imaging,DC=com";
        private const string GroupFilter = "(objectClass=group)";
        private const string PersonFilter = "(objectClass=Person)";

        private static IEnumerable<Account> GetAllAccounts()
        {
            var groupEntries = GetEntries(LdapDcUnitedImagingDcCom, GroupFilter);
            var groups = GetGroups(groupEntries);
            var personEntries = GetEntries(LdapOuUnitedImagingDcUnitedImagingDcCom, PersonFilter);
            return GetAccounts(personEntries, groups);
        }

        private static void IndexAccounts(IEnumerable<Account> accounts)
        {
            // Connect Elasticsearch
            var node = new Uri("http://10.6.14.157:9200");
            var elasticUser = "elastic";
            var elasticPwd = "123qwe";
            var setting = new ConnectionSettings(node);
            setting.BasicAuthentication(elasticUser, elasticPwd);
            var elasticSearchClient = new ElasticClient(setting);


            // Index Mapping
            var descriptor = new CreateIndexDescriptor("account")
                .Mappings(ms => ms.Map<Account>(m => m.AutoMap()));
            elasticSearchClient.CreateIndex(descriptor);

            foreach (var account in accounts)
            {
                var respose = elasticSearchClient.Index(account, idx => idx.Index("account"));
                Console.WriteLine("Indexed an account with respose : {0}", respose.Result);
            }
        }

        private static IEnumerable<Account> GetAccounts(SearchResultCollection personEntries, IEnumerable<Group> groups)
        {
            var groupList = groups as IList<Group> ?? groups.ToList();
            foreach (SearchResult personEntry in personEntries)
            {
                var account = new Account
                {
                    Name = GetEntryPropertyValue(personEntry, LdapAcountName),
                    Department = GetEntryPropertyValue(personEntry, LdapDepartment),
                    DistinDistinguishedName = GetEntryPropertyValue(personEntry, LdapDistinguishedName),
                    Description = GetEntryPropertyValue(personEntry, LdapDescription)
                };
                foreach (string memberOf in personEntry.Properties[LdapMemberOf])
                {
                    var group = groupList.FirstOrDefault(g => g.DistinguishedName == memberOf);
                    if (null == group)
                    {
                        Console.WriteLine($"Not found {account.Name} memberOf {memberOf} in groups");
                        continue;
                    }
                    account.Groups.Add(group);
                }

                yield return account;
            }
        }

        private static IEnumerable<Group> GetGroups(SearchResultCollection groupEntries)
        {
            foreach (SearchResult groupEntry in groupEntries)
            {
                yield return new Group
                {
                    Name = GetEntryPropertyValue(groupEntry, LdapCn),
                    Description = GetEntryPropertyValue(groupEntry, LdapDescription),
                    DistinguishedName = GetEntryPropertyValue(groupEntry, LdapDistinguishedName)
                };
            }
        }

        private static string GetEntryPropertyValue(SearchResult entry, string propertyKey)
        {
            var properties = entry.Properties[propertyKey];
            return properties.Count > 0 ? properties[0] as string : string.Empty;
        }

        private static SearchResultCollection GetEntries(string rootPath, string filter = "")
        {
            using (var entry = new DirectoryEntry(rootPath))
            {
                using (var directorySearcher = new DirectorySearcher(entry))
                {
                    directorySearcher.Filter = filter;
                    return directorySearcher.FindAll();
                }
            }
        }

        //[Conditional("DEBUG")]
        //private static void ListEntryProperties(DirectoryEntry entry)
        //{
        //    Console.WriteLine(entry.Path);
        //    foreach (string key in entry.Properties.PropertyNames)
        //    {
        //        Console.WriteLine(key + " = ");
        //        foreach (var obj in entry.Properties[key])
        //        {
        //            Console.WriteLine("\t" + obj);
                    
        //        }
        //    }
        //}
    }
}
