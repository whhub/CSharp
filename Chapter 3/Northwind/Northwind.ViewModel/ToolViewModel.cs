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
        public Customer Customer { get; set; }

        public CustomerDetailsViewModel(IUIDataProvider dataProvider, string customerID)
        {
            Customer = dataProvider.GetCustomer(customerID);

            if(Customer != null)
                DisplayName = Customer.CompanyName;
        }
    }
}