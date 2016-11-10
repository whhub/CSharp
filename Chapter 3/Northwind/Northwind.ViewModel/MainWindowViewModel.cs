using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            Tools = new ObservableCollection<ToolViewModel>
            {
                new AToolViewModel(),
                new BToolViewModel()
            };
        }

        private void GetCustomers()
        {
            _customers = _dataProvider.GetCustomers();
        }

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

        public IList<Customer> Customers
        {
            get
            {
                if (_customers == null) GetCustomers();
                return _customers;
            }
        }

        public ObservableCollection<ToolViewModel> Tools { get; set; }

        #endregion
    }
}