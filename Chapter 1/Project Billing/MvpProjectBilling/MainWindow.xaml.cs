using System;
using System.Windows;

namespace MvpProjectBilling
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            _model = new ProjectsModel();
        }

        private void OnShowProjectsButtonClicked(object sender, RoutedEventArgs e)
        {
            var view = new ProjectsView();
            new ProjectsPresener(view, _model);
            view.Owner = this;
            view.Show();
        }

        private IProjectsModel _model;
    }
}
