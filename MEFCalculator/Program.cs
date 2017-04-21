using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using RapidMEF.Diagnostics;

namespace MEFCalculator
{
    internal class Program
    {
        private CompositionContainer _container;
        [Import] public ICalculator Calculator;

        public Program()
        {
            CreateCompositionContainer();

            ComposeParts();

            new CompositionContainerVisualizer(_container).Show();
        }

        private static void Main()
        {
            var p = new Program(); //Composition is performed in the constructor
            Console.WriteLine("Enter Command: ");
            while (true)
            {
                Console.WriteLine(p.Calculator.Calculate(Console.ReadLine()));
            }
        }

        #region [--Private Methods--]

        //Fill the imports of this object
        private void ComposeParts()
        {
            try
            {
                _container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                Console.WriteLine(compositionException);
            }
        }

        private void CreateCompositionContainer()
        {
            //An aggregate catalog that combines multiple catalogs
            var catalog = new AggregateCatalog();
            //Adds all the parts found in the same assembly as the Program class
            catalog.Catalogs.Add(new AssemblyCatalog(typeof (Program).Assembly));

            //Create the CompositionContainer with the parts in the catalog
            _container = new CompositionContainer(catalog);
        }

        #endregion [--Private Methods--]
    }

    public interface IOperation //此处没有Public，编译依然成功，但是运行会在Container.ComposeParts的时候异常
    {
        int Operate(int left, int right);
    }

    public interface IOperationData //此处没有Public，编译依然成功，但是运行会在Container.ComposeParts的时候异常
    {
        char Symbol { get; }
    }

    internal interface ICalculator
    {
        string Calculate(string input);
    }

    [Export(typeof (ICalculator))]
    internal class SimpleCalculator : ICalculator
    {
        [ImportMany] private IEnumerable<Lazy<IOperation, IOperationData>> _operations;

        #region Implementation of ICalculator

        public string Calculate(string input)
        {
            int left, right;
            char oper;
            var fn = FindFirstNonDigit(input);
            if (fn < 0) return "Could not parse command.";

            try
            {
                //separate out the operands
                left = int.Parse(input.Substring(0, fn));
                right = int.Parse(input.Substring(fn + 1));
            }
            catch (Exception)
            {
                return "Could not parse command.";
            }

            oper = input[fn];

            foreach (var operation in _operations)
            {
                if (operation.Metadata.Symbol.Equals(oper))
                    return operation.Value.Operate(left, right).ToString();
            }

            return "Operation Not Found!";
        }

        private int FindFirstNonDigit(string input)
        {
            for (var i = 0; i < input.Length; i++)
            {
                if (!char.IsDigit(input[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        #endregion
    }

    [Export(typeof (IOperation))]
    [ExportMetadata("Symbol", '+')]
    internal class Add : IOperation
    {
        #region Implementation of IOperation

        public int Operate(int left, int right)
        {
            return left + right;
        }

        #endregion
    }

    [Export(typeof (IOperation))]
    [ExportMetadata("Symbol", '-')]
    internal class Subtract : IOperation
    {
        #region Implementation of IOperation

        public int Operate(int left, int right)
        {
            return left - right;
        }

        #endregion
    }
}