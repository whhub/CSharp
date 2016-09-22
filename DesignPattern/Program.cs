using System;
using System.ComponentModel;
using DesignPattern.CompositePattern;

namespace DesignPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            //TODO: 叶子结点代码重复, 可以改造
            var root = new ConcreteCompany("北京总公司");
            root.Add(new HRDepartment("总公司人力资源部"));
            root.Add(new FinanceDepartment("总公司财务部"));

            var comp = new ConcreteCompany("上海华东分公司");
            comp.Add(new HRDepartment("华东分公司人力资源部"));
            comp.Add(new FinanceDepartment("华东分公司财务部"));
            root.Add(comp);

            var bsc1 = new ConcreteCompany("南京办事处");
            bsc1.Add(new HRDepartment("南京办事处人力资源部"));
            bsc1.Add(new FinanceDepartment("南京办事处财务部"));
            comp.Add(bsc1);

            var bsc2 = new ConcreteCompany("杭州办事处");
            bsc2.Add(new HRDepartment("杭州办事处人力资源部"));
            bsc2.Add(new FinanceDepartment("杭州办事处财务部"));
            comp.Add(bsc2);


            Console.WriteLine("\n 结构图: ");
            root.Display(1);

            Console.WriteLine("\n 职责: ");
            root.LineOfDuty();


        }
    }





    #region test

    internal class Cloner : ICloneable
    {
        public object Clone()
        {
            throw new NotImplementedException();
        }
    }

    internal class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    #endregion

}
