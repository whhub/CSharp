using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;

namespace EmployeeWebService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "EmployeeService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select EmployeeService.svc or EmployeeService.svc.cs at the Solution Explorer and start debugging.
    public class EmployeeService : IEmployeeService
    {
        private static object _obj = new object();
        private static readonly List<Employee> _allEmployees = new List<Employee>(); 
        private static DateTime? _lastCachedTime; 

        IEnumerable<Employee> GetAllEmployees()
        {
            if (EmployeeInfoHasCachedToday())
            {
                return _allEmployees;
            }

            lock (_obj)
            {
                if (EmployeeInfoHasCachedToday())
                {
                    return _allEmployees;
                }

                //TODO: write in config file
                string constr = @"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.6.204.13) (PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=HR92PRD)));User Id=ifpsis; Password=Asdf5623";
                using (var con = new OracleConnection(constr))
                {
                    var employees = new List<Employee>();
                    con.Open();
                    Console.WriteLine("成功连接数据库");
                    using (var com = con.CreateCommand())
                    {
                        var sql = "SELECT DISTINCT * FROM PS_UIH_IFIS_VW WHERE OPRID IS NOT NULL";
                        Console.WriteLine(sql);
                        com.CommandText = sql;

                        var reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            int i = 0;
                            var employee = new Employee
                            {
                                Id = reader.Get<string>(i++),
                                Name = reader.Get<string>(i++),
                                Bu = reader.Get<string>(i++),
                                Department = reader.Get<string>(i++),
                                SuperiorId = reader.Get<string>(i)
                            };

                            Console.WriteLine(employee);
                            employees.Add(employee);
                        }
                        _allEmployees.Clear();
                        _allEmployees.AddRange(employees);
                        _lastCachedTime = DateTime.Now;
                    }
                }

            }
            return _allEmployees;
        }

        private bool EmployeeInfoHasCachedToday()
        {
            return null != _lastCachedTime && _lastCachedTime.Value.Date == DateTime.Now.Date;
        }


        public string GetString()
        {
            return "foo";
        }

        public IEnumerable<string> GetSubordinates(string id)
        {
            if(id == Admin) {return new string[]{AllUsers};}

            GetAllEmployees();

            if (id == Devops) id = GetSupervisor(id);

            List<string> subOrdinates = new List<string>();
            Queue<string> idQueue = new Queue<string>();
            idQueue.Enqueue(id);

            while (idQueue.Count > 0)
            {
                var currentId = idQueue.Dequeue();
                Console.WriteLine($"Under {currentId}");

                var directeSubordinates = _allEmployees.Where(e => e.SuperiorId == currentId).Select(e => e.Id).Except(subOrdinates).ToArray();
                subOrdinates.AddRange(directeSubordinates);
                foreach (var directeSubordinate in directeSubordinates)
                {
                    idQueue.Enqueue(directeSubordinate);
                    Console.WriteLine(directeSubordinate);
                }
            }

            return subOrdinates;
        }

        public string Subordinates(string id)
        {
            return string.Join(" OR ", GetSubordinates(id));
        }

        public string CacheEmployeeInfo()
        {
            GetAllEmployees();
            
            return $"Employee Info Cached ? {_lastCachedTime??DateTime.MinValue}";
        }

        public string GetSupervisor(string id)
        {
            GetAllEmployees();

            var employee = _allEmployees.FirstOrDefault(e => e.Id == id);
            return  null == employee ? string.Empty : employee.SuperiorId;
        }

        public string GetStaff()
        {
            GetAllEmployees();
            return JsonConvert.SerializeObject(_allEmployees);
        }


        private const string Admin = "admin";
        private const string Devops = "xiaobin.ling";
        private const string AllUsers = "*";
    }
}
