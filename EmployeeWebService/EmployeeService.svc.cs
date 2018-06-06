using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Oracle.ManagedDataAccess.Client;

namespace EmployeeWebService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "EmployeeService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select EmployeeService.svc or EmployeeService.svc.cs at the Solution Explorer and start debugging.
    public class EmployeeService : IEmployeeService
    {
        public string GetString()
        {
            return "foo";
        }

        public IEnumerable<string> GetSubordinates(string id)
        {
            string constr = @"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.6.204.13) (PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=HR92PRD)));User Id=ifpsis; Password=Asdf5623";

            IList<string> subOrdinates = new List<string>();
            Queue<string> idQueue = new Queue<string>();
            idQueue.Enqueue(id);

            var con = new OracleConnection(constr);
            try
            {
                con.Open();
                Console.WriteLine("成功连接数据库");
                while (idQueue.Count>0)
                {
                    var currentId = idQueue.Dequeue();
                    var com = con.CreateCommand();
                    var sql = string.Format("select OPRID from PS_UIH_IFIS_VW WHERE OPRID2 = '{0}'",currentId);
                    Console.WriteLine(sql);
                    com.CommandText = sql;
                    var oracleDataReader = com.ExecuteReader();
                    while (oracleDataReader.Read())
                    {
                        var subordinateId = oracleDataReader.GetString(0);
                        Console.Write(subordinateId);
                        if (!subOrdinates.Contains(subordinateId))
                        {
                            Console.WriteLine(" found");
                            subOrdinates.Add(subordinateId);
                            idQueue.Enqueue(subordinateId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            finally
            {
                con.Close();
            }
            return subOrdinates;
        }
    }
}
