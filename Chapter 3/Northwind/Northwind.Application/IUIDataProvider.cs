using System.Collections.Generic;
using System.Linq;
using Northwind.Data;

namespace Northwind.Application
{
    public interface IUIDataProvider
    {
        IList<Customer> GetCustomers();
        Customer GetCustomer(string customerID);
    }

    public class UIDataProvider : IUIDataProvider
    {
        private readonly NORTHWNDEntities _northwndEntities = new NORTHWNDEntities();

        #region Implementation of IUIDataProvider

        public IList<Customer> GetCustomers()
        {
            return _northwndEntities.Customers.ToList();
        }

        public Customer GetCustomer(string customerID)
        {
            return _northwndEntities.Customers.SingleOrDefault(c => c.CustomerID == customerID);
        }

        #endregion
    }
}