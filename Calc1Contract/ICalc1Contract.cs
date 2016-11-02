using System.AddIn.Contract;
using System.AddIn.Pipeline;

namespace Calc1Contract
{
    // The AddInContractAttribute identifies this pipeline segment a contract.
    [AddInContract]
    public interface ICalc1Contract : IContract
    {
        double Add(double a, double b);
        double Subtract(double a, double b);
        double Multiply(double a, double b);
        double Divide(double a, double b);
    }
}