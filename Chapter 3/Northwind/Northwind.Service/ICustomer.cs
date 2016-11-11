using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Northwind.Data;

namespace Northwind.Service
{
    [ServiceContract]
    public interface ICustomerService
    {
        [OperationContract]
        IList<Customer> GetCustomers();

        [OperationContract]
        Customer GetCustomer(string customerID);
    }

    public class CustomerService : ICustomerService
    {
        private readonly NORTHWNDEntities _northwndEntities = new NORTHWNDEntities();

        #region Implementation of ICustomerService

        public IList<Customer> GetCustomers()
        {
            return _northwndEntities.Customers
                .Select(
                    c => new Customer
                    {
                        CustomerID = c.CustomerID,
                        CompanyName = c.CompanyName
                    }).ToList();
        }

        public Customer GetCustomer(string customerID)
        {
            var customer = _northwndEntities.Customers.Single(c => c.CustomerID == customerID);
            return new Customer
            {
                CustomerID = customer.CustomerID,
                CompanyName = customer.CompanyName,
                ContractName = customer.ContactName,
                Address = customer.Address,
                City = customer.City,
                Country = customer.Country,
                Region = customer.Region,
                PostalCode = customer.PostalCode,
                Phone = customer.Phone
            };
        }

        #endregion
    }
}