namespace UnitTestExample
{
    public class DependentClass
    {
        private readonly IDependency _dependency;

        public DependentClass(IDependency dependency)
        {
            _dependency = dependency;
        }

        public int GetValue(string s)
        {
            return _dependency.GetValue(s);
        }

        public void CallMeFirst()
        {
            _dependency.CallMeFirst();
        }

        public void CallMeLast()
        {
            _dependency.CallMeLast();
        }

        public int CallMeTwice(string s)
        {
            return _dependency.CallMeTwice(s);
        }
    }
}
