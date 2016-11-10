using System.Collections.Generic;
using System.Linq;
using Northwind.Data;

namespace Northwind.Application
{
    public interface IUIDataProvider
    {
        IList<Customer> GetCustomers();
    }

    public class UIDataProvider : IUIDataProvider
    {
        #region Implementation of IUIDataProvider

        public IList<Customer> GetCustomers()
        {
            return new NORTHWNDEntities().Customers.ToList();
        }

        #endregion
    }
}