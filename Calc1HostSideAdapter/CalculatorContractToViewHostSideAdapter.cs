using System.AddIn.Pipeline;
using Calc1Contract;
using Calc1HostView;

namespace Calc1HostSideAdapter
{
    // The HostAdapterAttribute identifies this class as the host-side adapter pipeline segment.
    [HostAdapter]
    public class CalculatorContractToViewHostSideAdapter : ICalculator
    {
        private ContractHandle _handle;
        private readonly ICalc1Contract _contract;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public CalculatorContractToViewHostSideAdapter(ICalc1Contract contract)
        {
            _contract = contract;
            _handle = new ContractHandle(contract);
        }

        #region Implementation of ICalculator

        public double Add(double a, double b)
        {
            return _contract.Add(a, b);
        }

        public double Subtract(double a, double b)
        {
            return _contract.Subtract(a, b);
        }

        public double Multiply(double a, double b)
        {
            return _contract.Multiply(a, b);
        }

        public double Divide(double a, double b)
        {
            return _contract.Divide(a, b);
        }

        #endregion
    }
}