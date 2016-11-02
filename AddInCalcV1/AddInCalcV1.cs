using System.AddIn;
using Calc1AddInView;

namespace AddInCalcV1
{
    // The AddInAttribute identifies this pipeline segment as an add-in.
    [AddIn("Calculator AddIn", Version = "1.0.0.0")]
    public class AddInCalcV1 : ICalculator
    {
        #region Implementation of ICalculator

        public double Add(double a, double b)
        {
            return a + b;
        }

        public double Subtract(double a, double b)
        {
            return a - b;
        }

        public double Multiply(double a, double b)
        {
            return a*b;
        }

        public double Divide(double a, double b)
        {
            return a/b;
        }

        #endregion
    }
}