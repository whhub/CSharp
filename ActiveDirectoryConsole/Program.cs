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
            GetALLOU();
            Console.WriteLine("Enter Any Key to Exit");
            Console.ReadKey();
        }

        private static void GetALLOU()
        {
            var entry = new DirectoryEntry("LDAP://OU=United_Imaging,DC=united-imaging,DC=com");
            var directorySearcher = new DirectorySearcher(entry);
            directorySearcher.Filter = ("(objectClass=organizationalUnit)");

            foreach (SearchResult resEnt in directorySearcher.FindAll())
            {
                Console.Write(resEnt.GetDirectoryEntry().Name.ToString());
            }

            // 根据名字访问属性
            //foreach (string key in entry.Properties.PropertyNames)
            //{
            //    Console.WriteLine(key + " = ");
            //    foreach (var obj in entry.Properties[key])
            //    {
            //        Console.WriteLine("\t" + obj);
            //    }
            //}
        }
    }
}
