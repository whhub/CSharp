using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.ManagedDataAccess.Client;

namespace SqlConnection
{
    public class EmployeeService
    {
        private static object _obj = new object();
        private static List<Employee> _allEmployees = new List<Employee>(); //从Oracle获取数据的
        private static DateTime? _lastCachedTime; //存的是访问Oracel的时间

        IEnumerable<Employee> GetAllEmployees()
        {
            if (EmployeeInfoCachedToday())
            {
                return _allEmployees;
            }

            lock (_obj)
            {
                if (EmployeeInfoCachedToday())
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
                        var sql = "select * from PS_UIH_IFIS_VW";
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

        private bool EmployeeInfoCachedToday()
        {
            return null != _lastCachedTime && _lastCachedTime.Value.Date == DateTime.Now.Date;
        }


        public string GetString()
        {
            return "foo";
        }

        public IEnumerable<string> GetSubordinates(string id)
        {
            GetAllEmployees();

            List<string> subOrdinates = new List<string>();
            Queue<string> idQueue = new Queue<string>();
            idQueue.Enqueue(id);

            while (idQueue.Count > 0)
            {
                var currentId = idQueue.Dequeue();
                Console.WriteLine($"Under {currentId}");

                var directleSubordinates = _allEmployees.Where(e => e.SuperiorId == currentId).Select(e => e.Id).Except(subOrdinates).ToArray();
                subOrdinates.AddRange(directleSubordinates);
                foreach (var directleSubordinate in directleSubordinates)
                {
                    idQueue.Enqueue(directleSubordinate);
                    Console.WriteLine(directleSubordinate);
                }
            }

            return subOrdinates;
        }

        public string Subordinates(string id)
        {
            return string.Join(" OR ", GetSubordinates(id));
        }
    }
}
