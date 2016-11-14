using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Northwind.Data;

namespace Northwind.ViewModel
{
    public class OrdersViewModel
    {
        public OrdersViewModel(IEnumerable<Order> orders)
        {
            Orders = new ObservableCollection<OrderViewModel>(orders.Select(o => new OrderViewModel(o)));
        }

        public ObservableCollection<OrderViewModel> Orders { get; set; }
    }
}