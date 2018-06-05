using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeHierarchyWebServiceTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var webservice = new EmployeeHierarchyWebService.EmployeeHierarchyService();
            Console.WriteLine(webservice.HasManagership(""));
            Console.ReadLine();
        }
    }
}
