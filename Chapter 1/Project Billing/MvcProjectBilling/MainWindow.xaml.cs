using System;
using System.Windows;

namespace MvcProjectBilling
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            _controller = new ProjectsController(new ProjectsModel());
        }

        private void OnShowProjectsButtonClicked(object sender, RoutedEventArgs e)
        {
            _controller.ShowProjectsView(this);
        }

        private readonly IProjectsController _controller;

        private void OnGcCollectionButtonClicked(object sender, RoutedEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
