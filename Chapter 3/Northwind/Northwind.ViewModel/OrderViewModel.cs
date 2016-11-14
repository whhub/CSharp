using System.ComponentModel;
using System.Linq;
using Northwind.Data;

namespace Northwind.ViewModel
{
    public class OrderViewModel : Notifier
    {
        public OrderViewModel(Order model)
        {
            _model = model;
            SubscribeToOrderDetailsChanged(_model);
        }

        ~OrderViewModel()
        {
            UnSubscribeToOrderDetailsChanged(_model);
        }

        private void SubscribeToOrderDetailsChanged(Order order)
        {
            order.PropertyChanged += OrderOnPropertyChanged;
            foreach (var orderDetail in order.Order_Details)
            {
                orderDetail.PropertyChanged += OrderOnPropertyChanged;
            }
        }

        private void UnSubscribeToOrderDetailsChanged(Order order)
        {
            order.PropertyChanged -= OrderOnPropertyChanged;
            foreach (var orderDetail in order.Order_Details)
            {
                orderDetail.PropertyChanged -= OrderOnPropertyChanged;
            }
        }

        private void OrderOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            switch (propertyChangedEventArgs.PropertyName)
            {
                case "Freight":
                case "Quantity":
                case "UnitPrice":
                    NotifyPropertyChanged(() => Total);
                    break;
            }
        }

        #region [--Notified Properties--] 

        #region [--Model--]

        private Order _model;

        public Order Model
        {
            get { return _model; }
            set
            {
                if (_model == value) return;
                _model = value;
                NotifyPropertyChanged(() => Model);
                NotifyPropertyChanged(() => Total);
            }
        }

        #endregion [--Model--]

        #region [--Total--]

        public decimal Total
        {
            get { return _model.Order_Details.Sum(o => o.Quantity*o.UnitPrice); }
        }

        #endregion [--Total--]

        #endregion [--Notified Properties--]
    }
}