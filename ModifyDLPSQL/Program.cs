using System;
using System.IO;

namespace ModifyDLPSQL
{
    class Program
    {
        private const string SqlFileName = "dlp_decryption_of_privileges.sql";
        static void Main()
        {
            // get date
            var time = DateTime.Now;
            var monthString = time.ToString("yyyyMM");

            // open file & write string
            using (var file = new StreamWriter(SqlFileName, false))
            {
                file.WriteLine($"SELECT * FROM dbo.FileLog2{monthString}");
                file.WriteLine();
                file.WriteLine("WHERE");
                file.WriteLine($"\toperationTime >= :sql_last_value and active = '特权解密'");
            }
        }
    }
}
