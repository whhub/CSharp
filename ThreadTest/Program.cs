using System;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadTest
{
    class Program
    {
        static readonly object LockObject = new object();

        static void Main()
        {
            var task1 = Task.Factory.StartNew(ParameterizedThreadMethod, 1);
            Task.Factory.StartNew(ParameterizedThreadMethod, 2);
            //task1.Start();
            //task2.Start();

            Console.WriteLine(DateTime.Now.Millisecond);    //778
            lock (LockObject)
            {
                
            }
            Console.WriteLine(DateTime.Now.Millisecond);    //793,  lock time 15ms (debug version)

            Task.WaitAll(task1);
            Console.WriteLine("主线程代码运行结束");
        }

        private static void ParameterizedThreadMethod(object threadId)
        {
            int interval = (int) threadId;
            for (int i = 0; i < 10/interval; i++)
            {
                Console.WriteLine("[Thread{0}][第{1}次]{2}", threadId, i, DateTime.Now);
                Thread.Sleep(interval*1000);
            }
        }

        
    }
}
