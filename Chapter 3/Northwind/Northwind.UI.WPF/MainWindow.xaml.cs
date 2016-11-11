using System.Windows;
using Northwind.ViewModel;

namespace Northwind.UI.WPF
{
    /// <summary>
    ///     MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private MainWindowViewModel ViewModel
        {
            get { return (MainWindowViewModel) DataContext; }
        }

        private void Hyperlink_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.ShowCustomerDetails();
        }
    }
}