using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using Northwind.Application;
using Northwind.Data;

namespace Northwind.ViewModel
{
    public class MainWindowViewModel
    {
        private readonly IUIDataProvider _dataProvider;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public MainWindowViewModel(IUIDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            Tools = new ObservableCollection<ToolViewModel>();
        }

        public void ShowCustomerDetails()
        {
            if (string.IsNullOrEmpty(SelectedCustomerID))
                throw new InvalidOperationException("SelectedCustomerID can't be null");

            var customerDetailsViewModel = GetCustomerDetailsTool(SelectedCustomerID);

            if (customerDetailsViewModel == null)
            {
                customerDetailsViewModel = new CustomerDetailsViewModel(_dataProvider, SelectedCustomerID);
                Tools.Add(customerDetailsViewModel);
            }

            SetCurrentTool(customerDetailsViewModel);
        }

        #region [--Command--]

        private bool IsCustomerSelected()
        {
            return !string.IsNullOrEmpty(SelectedCustomerID);
        }

        private Command _showDetailsCommand;

        public Command ShowDetailsCommand
        {
            get
            {
                return _showDetailsCommand ??
                       (_showDetailsCommand = new Command(p => ShowCustomerDetails(), p => IsCustomerSelected()));
            }
        }

        #endregion

        #region [--Properties--]

        public string Name
        {
            get { return "Northwind"; }
        }

        public string ControlPanelName
        {
            get { return "Control Panel"; }
        }

        private IList<Customer> _customers;
        private string _selectedCustomerID;

        public IList<Customer> Customers
        {
            get
            {
                if (_customers == null) GetCustomers();
                return _customers;
            }
        }

        public ObservableCollection<ToolViewModel> Tools { get; set; }

        public string SelectedCustomerID
        {
            get { return _selectedCustomerID; }
            set
            {
                if(_selectedCustomerID == value) return;
                _selectedCustomerID = value;
                ShowDetailsCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region [--Private Methods--]

        private void GetCustomers()
        {
            _customers = _dataProvider.GetCustomers();

            Tools.Add(new CustomerDetailsViewModel(_dataProvider, "ALFKI"));
        }

        private void SetCurrentTool(ToolViewModel currentTool)
        {
            var collectionView = CollectionViewSource.GetDefaultView(Tools);
            if (collectionView != null)
            {
                if (collectionView.MoveCurrentTo(currentTool) != true)
                {
                    throw new InvalidOperationException("Could not find the current tool");
                }
            }
        }

        private CustomerDetailsViewModel GetCustomerDetailsTool(string customerID)
        {
            return Tools.OfType<CustomerDetailsViewModel>().FirstOrDefault(c => c.Customer.CustomerID == customerID);
        }

        #endregion
    }
}