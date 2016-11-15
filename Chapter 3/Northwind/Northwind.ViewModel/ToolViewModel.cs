using System.Windows.Input;
using Northwind.Application;
using Northwind.Data;

namespace Northwind.ViewModel
{
    public class ToolViewModel
    {
        private readonly IToolManager _toolManager;

        public ToolViewModel(IToolManager toolManager)
        {
            _toolManager = toolManager;
        }

        public string DisplayName { get; set; }

        #region [--CloseCommand--] 

        private ICommand _closeCommand;

        public ICommand CloseCommand
        {
            get { return _closeCommand = _closeCommand ?? new Command(p => Close()); }
        }

        private void Close()
        {
            _toolManager.CloseTool(this);
        }

        #endregion [--CloseCommand--]     
    }

    public class CustomerDetailsViewModel : ToolViewModel
    {
        private OrdersViewModel _orders;

        public CustomerDetailsViewModel(IUIDataProvider dataProvider, string customerID) : base(TODO)
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