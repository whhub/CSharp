using System.Windows.Media;
using ProjectBilling.DataAccess;

namespace MvpProjectBilling
{
    class ProjectsPresener
    {
        private readonly IProjectsView _view;
        private readonly IProjectsModel _model;

        public  ProjectsPresener(IProjectsView projectsView, IProjectsModel projectsModel)
        {
            _view = projectsView;
            _view.ProjectUpdated += ViewProjectUpdated;
            _view.SelectionChanged += ViewSelectionChanged;
            _view.DetailsUpdated += ViewDetailsUpdated;

            _model = projectsModel;
            _model.ProjectUpdated += ModelProjectUpdated;

            _view.LoadProjects(_model.GetProjects());
        }

        void ModelProjectUpdated(object sender, ProjectEventArgs e)
        {
            _view.UpdateProject(e.Project);
        }

        void ViewDetailsUpdated(object sender, ProjectEventArgs e)
        {
            SetEstimatedColor(e.Project);
        }

        void ViewSelectionChanged(object sender, System.EventArgs e)
        {
            var selectedId = _view.SelectedProjectId;

            if (selectedId > _view.NoneSelected)
            {
                var project = _model.GetProject(selectedId);
                _view.EnableControls(true);
                _view.UpdateDetails(project);
                //SetEstimatedColor(project);
            }
            else
            {
                _view.EnableControls(false);
            }
        }

        void ViewProjectUpdated(object sender, ProjectEventArgs e)
        {
            _model.UpdateProject(e.Project);
            SetEstimatedColor(e.Project);
        }

        private void SetEstimatedColor(Project project)
        {
            if (project.Id != _view.SelectedProjectId) return;

            if (project.Actual <= 0)
            {
                _view.SetEstimatedColor(null);
            }
            else if (project.Actual > project.Estimate) 
            {
                _view.SetEstimatedColor(Colors.Red);
            }
            else
            {
                _view.SetEstimatedColor(Colors.Green);
            }
        }

    }
}
