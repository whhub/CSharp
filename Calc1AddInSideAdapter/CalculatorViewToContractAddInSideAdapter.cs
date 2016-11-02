using System.AddIn.Pipeline;
using Calc1AddInView;
using Calc1Contract;

namespace Calc1AddInSideAdapter
{
    // The AddInAdapterAttribute identifies this class as the add-in-side adapter pipeline segment.
    [AddInAdapter]
    public class CalculatorViewToContractAddInSideAdapter : ContractBase, ICalc1Contract
    {
        private readonly ICalculator _view;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.AddIn.Pipeline.ContractBase" /> class.
        /// </summary>
        public CalculatorViewToContractAddInSideAdapter(ICalculator view)
        {
            _view = view;
        }

        #region Implementation of ICalc1Contract

        public virtual double Add(double a, double b)
        {
            return _view.Add(a, b);
        }

        public virtual double Subtract(double a, double b)
        {
            return _view.Subtract(a, b);
        }

        public virtual double Multiply(double a, double b)
        {
            return _view.Multiply(a, b);
        }

        public virtual double Divide(double a, double b)
        {
            return _view.Divide(a, b);
        }

        #endregion
    }
}