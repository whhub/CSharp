using System;
using System.Collections.Generic;

namespace DesignPattern.CompositePattern
{
    class ConcreteCompany : Company
    {
        public ConcreteCompany(string name) : base(name)
        {
        }

        public override void Add(Company c)
        {
            _children.Add(c);
        }

        public override void Remove(Company c)
        {
            _children.Remove(c);
        }

        public override void Display(int depth)
        {
            Console.WriteLine(new string('-', depth) + _name);

            _children.ForEach(c=>c.Display(depth+2));
        }

        public override void LineOfDuty()
        {
            _children.ForEach(c=>c.LineOfDuty());
        }


        private readonly List<Company> _children = new List<Company>();
    }
}
