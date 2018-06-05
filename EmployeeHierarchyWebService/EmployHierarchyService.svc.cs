using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace EmployeeHierarchyWebService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "EmployeeHierarchyService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select EmployeeHierarchyService.svc or EmployeeHierarchyService.svc.cs at the Solution Explorer and start debugging.
    public class EmployeeHierarchyService : IEmployHierarchy
    {
        public bool HasManagership(string employee)
        {
            return 0 == new Random(DateTime.Now.Millisecond).Next() % 10;
        }

        public bool IsSupervisor(string fromEmployee, string toEmployee)
        {
            return 0 == new Random(DateTime.Now.Millisecond).Next() % 10;
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
