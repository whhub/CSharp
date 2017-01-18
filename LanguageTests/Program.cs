using System;

namespace LanguageTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var c = new C2();
            Console.WriteLine(c.Count());
        }
    }


    interface I1<T>
    {
        int Count();
    }

    interface I2<T> : I1<T>
    {
         
    }

    class C1<T> : I2<T>
    {
        #region Implementation of I1

        public int Count()
        {
            return 1;
        }

        #endregion
    }

    interface I3
    {
        int Count();
    }

    class C2 : C1<int>, I3
    {
        
    }
}
