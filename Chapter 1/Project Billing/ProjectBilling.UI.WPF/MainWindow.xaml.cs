using System.Windows;
using ProjectBilling.Application.WPF;
using ProjectBilling.DataAccess;

namespace MVVMProjectBilling
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            _projectsModel = new ProjectsModel(new DataServiceStub());
        }

        private void OnShowProjectsButtonClicked(object sender, RoutedEventArgs e)
        {
            var view = new ProjectsView {DataContext = new ProjectsViewModel(_projectsModel), Owner = this};
            view.Show(); 
        }

        private readonly IProjectsModel _projectsModel;
    }
}
