using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using ProjectBilling.DataAccess;

namespace ProjectBilling.Monolithic
{
    sealed class ProjectsView : Window
    {
        public ProjectsView()
        {
            Title = "Project";
            Width = 250;
            MinWidth = 250;
            Height = 180;
            MinHeight = 180;

            LoadProjects();

            AddControlsToWindow();

            _updateButton.Click += OnUpdateButtonClicked;
        }

        private void OnProjectsListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;

            //if there is a selected item
            if (comboBox != null && comboBox.SelectedIndex > -1)
            {
                UpdateDetails();
            }
            else
            {
                DisableDetails();
            }
        }

        private void DisableDetails()
        {
            _estimateTextBox.IsEnabled = false;
            _actualTextBox.IsEnabled = false;
            _updateButton.IsEnabled = false;
        }

        private void UpdateDetails()
        {
            var seletedProject = _projectsComboBox.SelectedItem as Project;

            _estimateTextBox.IsEnabled = true;
            if (seletedProject != null)
                _estimateTextBox.Text = seletedProject.Estimate.ToString(CultureInfo.InvariantCulture);
            _actualTextBox.IsEnabled = true;
            if (seletedProject != null)
            {
                _actualTextBox.Text = Math.Abs(seletedProject.Actual - 0) < 1e-6 ? "" :
                                                                                                                    seletedProject.Actual.ToString(CultureInfo.InvariantCulture);

                SetEstimateColor(seletedProject);
            }
            _updateButton.IsEnabled = true;
        }

        private void OnUpdateButtonClicked(object sender, RoutedEventArgs e)
        {
            var selectedProject = _projectsComboBox.SelectedItem as Project;
            if (selectedProject != null)
            {
                selectedProject.Estimate = double.Parse(_estimateTextBox.Text);
                if (!string.IsNullOrEmpty(_actualTextBox.Text))
                    selectedProject.Actual = double.Parse(_actualTextBox.Text);
                SetEstimateColor(selectedProject);
            }
        }

        private void SetEstimateColor(Project selectedProject)
        {
            if (selectedProject.Actual < 1e-6)
            {
                _estimateTextBox.Foreground = _actualTextBox.Foreground;
            } 
            else if (selectedProject.Actual <= selectedProject.Estimate)
            {
                _estimateTextBox.Foreground = Brushes.Green;
            }
            else
            {
                _estimateTextBox.Foreground = Brushes.Red;
            }

        }

        private void AddControlsToWindow()
        {
            var grid = new UniformGrid {Columns = 2};
            grid.Children.Add(new Label {Content = "Projects:"});
            grid.Children.Add(_projectsComboBox);
            grid.Children.Add(new Label {Content = "Estimated Cost:"});
            grid.Children.Add(_estimateTextBox);
            grid.Children.Add(new Label {Content = "Actual Cost:"});
            grid.Children.Add(_actualTextBox);
            grid.Children.Add(_updateButton);

            Content = grid;
        }

        //private Grid GetGrid()
        //{
        //    var grid = new Grid();
        //    grid.ColumnDefinitions.Add(new ColumnDefinition());
        //    grid.ColumnDefinitions.Add(new ColumnDefinition());
        //    grid.RowDefinitions.Add(new RowDefinition());
        //    grid.RowDefinitions.Add(new RowDefinition());
        //    grid.RowDefinitions.Add(new RowDefinition());
        //    grid.RowDefinitions.Add(new RowDefinition());

        //    return grid;
        //}

        private void LoadProjects()
        {
            foreach (var project in new DataServiceStub().GetProjects())
            {
                _projectsComboBox.Items.Add(project);  
            }
            _projectsComboBox.DisplayMemberPath = "Name";
            _projectsComboBox.SelectionChanged += OnProjectsListBoxSelectionChanged;
        }

        [STAThread]
        static void Main()
        {
            var mainWindow = new ProjectsView();
            new Application().Run(mainWindow);
        }

        private static readonly Thickness GeneralMargin = new Thickness(5);
        private readonly ComboBox _projectsComboBox = new ComboBox {Margin = GeneralMargin};
        private readonly TextBox _estimateTextBox = new TextBox {IsEnabled = false, Margin = GeneralMargin};
        private readonly TextBox _actualTextBox = new TextBox {IsEnabled = false, Margin = GeneralMargin};
        private readonly Button _updateButton = new Button {IsEnabled = false, Content = "Update", Margin = GeneralMargin};


    }


}
