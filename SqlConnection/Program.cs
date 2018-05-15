using System;
using System.Data.SqlClient;

namespace SqlConnection
{
    class Program
    {
        static void Main(string[] args)
        {
            //string constr = "server=10.6.14.158;initial catalog=VRVEIS;user id=vrvsync;pwd=Uih123456;Integrated Security=true";
            string constr = "server=10.6.201.1;initial catalog=VRVEIS;user id=vrvsync;pwd=Uih123456";
            //string constr = "server=10.6.14.158;initial catalog=VRVEIS;user id=vrvsync;pwd=Uih123456";
            //string constr = "server=10.6.14.158;database=VRVEIS;user id=vrvsync;pwd=Uih123456";
            var sql = "select count(*) from dbo.PMoveableDiskEvent";
            var con = new System.Data.SqlClient.SqlConnection(constr);
            var com = new SqlCommand(sql, con);

            try
            {
                con.Open();
                Console.WriteLine("成功连接数据库");
                var x = com.ExecuteScalar();
                Console.WriteLine("成功读取{0}条记录", x);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Enter a key to exit");
            Console.ReadKey();
        }
    }
}
