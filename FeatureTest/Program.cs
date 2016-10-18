using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeatureTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var foo = new Foo2();

            //try
            //{

                foo.Fun(1);
                foo.Fun(-1);
                //foo.Fun(0);

            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("here" + e.Message);
            //}
            foo.Fun2();
            var foo2 = new Foo();
            foo2.Fun2();
        }
    }
}
