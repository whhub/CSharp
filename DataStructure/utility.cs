using System;
using System.Collections.Generic;

namespace DataStructure
{
    class Utility
    {
        public static void DisplayHorizontalSplitter()
        {
            Console.WriteLine("********************************************************************************");
        }

        public static void DisplayInfoWithHorizontalSplitter(object param)
        {
            Console.WriteLine();
            Console.WriteLine("********************************************************************************");
            Console.WriteLine(param.ToString());
            Console.WriteLine("********************************************************************************");
        }

        public static void DisplayCollection<T>(IEnumerable<T> c)
        {
            foreach (var e in c)
            {
                Console.Write(e);
                Console.Write("\t");
            }
        }

        public static void Swap(ref int a, ref int b)
        {
            a = a ^ b;
            b = a ^ b;
            a = a ^ b;
        }
    }
}
