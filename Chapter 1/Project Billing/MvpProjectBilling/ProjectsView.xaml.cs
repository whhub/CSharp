using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ProjectBilling.DataAccess;

namespace MvpProjectBilling
{
    /// <summary>
    /// ProjectsView.xaml 的交互逻辑
    /// </summary>
    public partial class ProjectsView : IProjectsView
    {
        public ProjectsView()
        {
            InitializeComponent();
            SelectedProjectId = NoneSelected;
        }

        #region Implementation of IProjectsView

        public int NoneSelected { get { return -1; }  }
        public int SelectedProjectId { get; private set; }
        public void UpdateProject(Project project)
        {
            var projects = ProjectsComboBox.ItemsSource as IList<Project>;
            
            if(projects == null) return;

            var projectToUpdate = projects.FirstOrDefault(p => p.Id == project.Id);

            if(projectToUpdate == null) return;

            projectToUpdate.Estimate = project.Estimate;
            projectToUpdate.Actual = project.Actual;
            if (SelectedProjectId == project.Id)
            {
                UpdateDetails(project);
            }
        }

        /// <summary>
        /// Allow for loading a collection of Projects as the ItemSource of ProjecsComboBox
        /// </summary>
        /// <param name="projects"></param>
        public void LoadProjects(IEnumerable<Project> projects)
        {
            ProjectsComboBox.ItemsSource = projects;
            ProjectsComboBox.DisplayMemberPath = "Name";
            ProjectsComboBox.SelectedValuePath = "Id";
        }

        public void UpdateDetails(Project project)
        {
            EstimatedTextBox.Text = project.Estimate.ToString(CultureInfo.InvariantCulture);
            ActualTextBox.Text = project.Actual.ToString(CultureInfo.InvariantCulture);
            DetailsUpdated(this, new ProjectEventArgs(project));
        }

        public void EnableControls(bool isEnabled)
        {
            EstimatedTextBox.IsEnabled = isEnabled;
            ActualTextBox.IsEnabled = isEnabled;
            UpdateButton.IsEnabled = isEnabled;
        }

        public void SetEstimatedColor(Color? color)
        {
            EstimatedTextBox.Foreground =
                (color == null)
                    ? ActualTextBox.Foreground
                    : new SolidColorBrush((Color) color);
        }

        public event EventHandler<ProjectEventArgs> ProjectUpdated = delegate { };
        public event EventHandler<ProjectEventArgs> DetailsUpdated = delegate { };
        public event EventHandler SelectionChanged = delegate { };

        #endregion

        #region Event Handlers

        private void OnProjectsComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedProjectId =
                ProjectsComboBox.SelectedValue == null
                    ? NoneSelected
                    : int.Parse(ProjectsComboBox.SelectedValue.ToString());
            SelectionChanged(this, new EventArgs());
        }

        private void OnUpdateButtonClicked(object sender, RoutedEventArgs e)
        {
            var project = new Project
                              {
                                  Estimate = GetDouble(EstimatedTextBox.Text),
                                  Actual = GetDouble(ActualTextBox.Text),
                                  Id = int.Parse(ProjectsComboBox.SelectedValue.ToString())
                              };
            ProjectUpdated(this, new ProjectEventArgs(project));
        }

        #endregion

        #region Helper Functions

        private double GetDouble(string text)
        {
            return string.IsNullOrEmpty(text)
                       ? 0
                       : double.Parse(text);
        }

        #endregion

    }

    public interface IProjectsView
    {
        int NoneSelected { get; }
        int SelectedProjectId { get; }
        void UpdateProject(Project project);
        void LoadProjects(IEnumerable<Project> projects);
        void UpdateDetails(Project project);
        void EnableControls(bool isEnabled);
        void SetEstimatedColor(Color? color);
        event EventHandler<ProjectEventArgs> ProjectUpdated;
        event EventHandler<ProjectEventArgs> DetailsUpdated;
        event EventHandler SelectionChanged;
    }
}
