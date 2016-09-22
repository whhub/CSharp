namespace DesignPattern.CompositePattern
{
    abstract class Company
    {
        protected string _name;

        public Company(string name)
        {
            _name = name;
        }

        public abstract void Add(Company c);    //增加
        public abstract void Remove(Company c); //移除
        public abstract void Display(int depth);//显示
        public abstract void LineOfDuty();      //履行职责
    }
}
