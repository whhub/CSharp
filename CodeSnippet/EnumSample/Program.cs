using System;

namespace EnumSample
{

        enum FooEnum
        {
            A = 1,
            B = 2,
            C = 3
        }

    class Program
    {


        static void Main(string[] args)
        {
            foreach (var value in Enum.GetValues(typeof(FooEnum)))
            {
                Console.WriteLine(value);
            }
        }
    }
}
