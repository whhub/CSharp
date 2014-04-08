using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ProjectBilling.DataAccess;

namespace MvcProjectBilling
{
	/// <summary>
	/// ProjectsView.xaml 的交互逻辑
	/// </summary>
	public partial class ProjectsView
	{
		public ProjectsView(
			IProjectsController projectsController,
			IProjectsModel projectsModel
			)
		{
			InitializeComponent();
			
			// 在此点之下插入创建对象所需的代码。
			_controller = projectsController;
			_model = projectsModel;
			_model.ProjectUpdated += OnModelProjectUpdated;

			ProjectsComboBox.ItemsSource = _model.Projects;
			ProjectsComboBox.DisplayMemberPath = "Name";
			ProjectsComboBox.SelectedValuePath = "Id";
		}

        ~ProjectsView()
        {
            MessageBox.Show("ProjectsView collected");
        }

	    #region Event Handlers

        /// <summary>
        /// use model data (project) to update details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
	    private void OnModelProjectUpdated(object sender, ProjectEventArgs e)
	    {
	        int selectedProjectId = GetSelectedProjectId();
	        if (selectedProjectId <= NoneSelected) return;
	        if (selectedProjectId == e.Project.Id) UpdateDetails(e.Project);
	    }

        /// <summary>
        /// Just use UI data (select project) to update details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
	    private void OnProjectsComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
	    {
	        var project = GetSelectedProject();
            if (project == null) return;

	        EstimatedTextBox.Text = project.Estimate.ToString(CultureInfo.InvariantCulture);
	        EstimatedTextBox.IsEnabled = true;
            ActualTextBox.Text = project.Actual.ToString(CultureInfo.InvariantCulture);
	        ActualTextBox.IsEnabled = true;

	        UpdateEstimatedColor();
	    }

	    private void OnUpdateButtonClicked(object sender, RoutedEventArgs e)
	    {
	        var project = new Project
	                          {
	                              Id = (int) ProjectsComboBox.SelectedValue,
	                              Name = ProjectsComboBox.Text,
	                              Estimate = GetDouble(EstimatedTextBox.Text),
	                              Actual = GetDouble(ActualTextBox.Text)
	                          };
            _controller.Update(project);
	    }

        private void OnProjectsViewWindowClosed(object sender, EventArgs e)
        {
            _model.ProjectUpdated -= OnModelProjectUpdated;
        }


	    #endregion


	    #region Helper Functions

        /// <summary>
        /// UI detail update according to project
        /// </summary>
        /// <param name="project"></param>
	    private void UpdateDetails(Project project)
	    {
	        EstimatedTextBox.Text = project.Estimate.ToString(CultureInfo.InvariantCulture);
	        ActualTextBox.Text = project.Actual.ToString(CultureInfo.InvariantCulture);
            UpdateEstimatedColor();
	    }

	    private int GetSelectedProjectId()
	    {
	        var project = ProjectsComboBox.SelectedItem as Project;
	        return project == null ? NoneSelected : project.Id;
	    }

	    private Project GetSelectedProject()
	    {
	        return ProjectsComboBox.SelectedItem as Project;
	    }

        /// <summary>
        /// set foreground of estimated text box according to actual - estimated
        /// </summary>
	    private void UpdateEstimatedColor()
	    {
	        var actual = GetDouble(ActualTextBox.Text);
	        var estimated = GetDouble(EstimatedTextBox.Text);
	        if (Math.Abs(actual - 0) < 1e-6)
	        {
	            EstimatedTextBox.Foreground = ActualTextBox.Foreground;
	        }
            else if (actual > estimated)
	        {
	            EstimatedTextBox.Foreground = Brushes.Red;
	        }
	        else
            {
                EstimatedTextBox.Foreground = Brushes.Green;
            }
	    }


	    private double GetDouble(string text)
	    {
	        return string.IsNullOrEmpty(text)
	                   ? 0
	                   : double.Parse(text);
	    }

	    #endregion



		private readonly IProjectsModel _model;
		private readonly IProjectsController _controller;
		private const int NoneSelected = -1;

	}
}