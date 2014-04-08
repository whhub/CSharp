using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace ProjectBilling.Application.WPF
{
    public class Notifier : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate {};

        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void NotifyPropertyChanged<T>(Expression<Func<T>> expression)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(
                ((MemberExpression)expression.Body).Member.Name));
        }
    }
}
