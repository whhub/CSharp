using System.Collections.Generic;
using System.Linq;
using Northwind.Data;
using System.Data.Objects;

namespace Northwind.ViewModel
{
    public class MainWindowViewModel
    {
        private IList<Customer> _customers;

        public IList<Customer> Customers
        {
            get
            {
                if (_customers == null) GetCustomers();
                return _customers;
            }
        }

        private void GetCustomers()
        {
            _customers = new NORTHWNDEntities().Customers.ToList();
        }
    }
    
}
