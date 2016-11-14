using Northwind.Application;
using Northwind.Data;

namespace Northwind.ViewModel
{
    public class ToolViewModel
    {
        public string DisplayName { get; set; }
    }

    public class CustomerDetailsViewModel : ToolViewModel
    {
        private OrdersViewModel _orders;

        public CustomerDetailsViewModel(IUIDataProvider dataProvider, string customerID)
        {
            Customer = dataProvider.GetCustomer(customerID);

            if (Customer != null)
                DisplayName = Customer.CompanyName;
        }

        public Customer Customer { get; set; }

        public OrdersViewModel Orders
        {
            get
            {
                if (Customer == null) return null;
                return _orders ?? (_orders = new OrdersViewModel(Customer.Orders));
            }
        }
    }
}