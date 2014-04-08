using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ProjectBilling.DataAccess;

namespace ProjectBilling.RAD
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource projectViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("projectViewSource")));
            // 通过设置 CollectionViewSource.Source 属性加载数据:
            // projectViewSource.Source = [一般数据源]

            projectViewSource.Source = new DataServiceStub().GetProjects();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var selectedProject = nameComboBox.SelectedItem as Project;
            if (selectedProject != null)
            {
                selectedProject.Estimate = double.Parse(estimateTextBox.Text);
                if (!string.IsNullOrEmpty(actualTextBox.Text))
                    selectedProject.Actual = double.Parse(actualTextBox.Text);
                SetEstimateColor(selectedProject);
            }
        }

        private void SetEstimateColor(Project selectedProject)
        {
            if (selectedProject.Actual < 1e-6)
            {
                estimateTextBox.Foreground = actualTextBox.Foreground;
            }
            else if (selectedProject.Actual <= selectedProject.Estimate)
            {
                estimateTextBox.Foreground = Brushes.Green;
            }
            else
            {
                estimateTextBox.Foreground = Brushes.Red;
            }

        }

        private void nameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;

            //if there is a selected item
            if (comboBox != null && comboBox.SelectedIndex > -1)
            {
                var selectedProject = comboBox.SelectedItem as Project;

                SetEstimateColor(selectedProject);
                button1.IsEnabled = true;
            }
            else
            {
                estimateTextBox.IsEnabled = false;
                actualTextBox.IsEnabled = false;
                button1.IsEnabled = false;
            }
        }
    }
}
