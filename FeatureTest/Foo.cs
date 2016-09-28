using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace FeatureTest
{
    class Foo : SecondaryObjectWithAspects
    {
        private bool _fooProperty;

        public bool Fun(int a)
        {
            if(a==0)
                throw new Exception("invalid parameter");
            Console.WriteLine("Fun Parameter {0}", a);
            return a > 0;
        }

        public void Fun2()
        {
            Thread.Sleep(2000);
            FooProperty = true;
        }

        public bool FooProperty
        {
            get { return _fooProperty; }
            set { _fooProperty = value; }
        }
    }
}
