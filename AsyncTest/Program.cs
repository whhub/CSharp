using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncTest
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 4; i++)
            {
                TestAsync();
            }
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        private static async Task TestAsync()
        {
            Console.WriteLine("Test()开始， Thread Id: {0}\r\n", Thread.CurrentThread.ManagedThreadId);
            var name = GetNameAsync();
            var res = await name;

            Console.WriteLine("await GetName1: {0}, 得到结果进行其它操作", res);
            Console.WriteLine("Test() 结束.\r\n");
        }

        private static async Task<string> GetNameAsync()
        {
            Console.WriteLine("GetName()开始， thread Id is: {0}", Thread.CurrentThread.ManagedThreadId);
            return await Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("'GetName' Thread Id: {0}", Thread.CurrentThread.ManagedThreadId);
                return "Jesse";
            });
        }
    }
}
