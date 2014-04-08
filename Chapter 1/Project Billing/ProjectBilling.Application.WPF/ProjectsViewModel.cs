using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using ProjectBilling.DataAccess;

namespace ProjectBilling.Application.WPF
{

    public interface IProjectsViewModel : INotifyPropertyChanged
    {
        IProjectViewModel SelectedProject { get; set; }
        void UpdateProject();
    }

    public class ProjectsViewModel : Notifier, IProjectsViewModel
    {

        public ProjectsViewModel(IProjectsModel projectsModel)
        {
            _model = projectsModel;
            _model.ProjectUpdated += ModelProjectUpdated;
            _updateCommand = new UpdateCommand(this);
        }

        public const string SelectedProjectPropertyName = "SelectedProject";
        
        public ObservableCollection<Project> Projects { get { return _model.Projects; } }

        #region Observed Properties

        public int? SelectedValue
        {
            set
            {
                if (value == null) return;
                var project = GetProject((int) value);
                if (SelectedProject == null)
                {
                    SelectedProject = new ProjectViewModel(project);
                }
                else
                {
                    SelectedProject.Update(project);
                }
                DetailsEstimateStatus = SelectedProject.EstimateStatus;
            }
        }

        public Status DetailsEstimateStatus
        {
            get { return _detailsEstimateStatus; }
            set
            {
                if (_detailsEstimateStatus == value) return;
                _detailsEstimateStatus = value;
                NotifyPropertyChanged(() => DetailsEstimateStatus);
            }
        }

        public bool DetailsEnabled
        {
            get { return _detailsEnabled; }
            set
            {
                if (_detailsEnabled == value) return;
                _detailsEnabled = value;
                NotifyPropertyChanged(() => DetailsEnabled);
            }
        }

        #endregion
        #region Commands

        public ICommand UpdateCommand
        {
            get { return _updateCommand; }
        }

        #endregion
        #region Event Handler

        private void ModelProjectUpdated(object sender, ProjectEventArgs e)
        {
            GetProject(e.Project.Id).Update(e.Project); //??why update model in view model?
            if (SelectedProject == null || e.Project.Id != SelectedProject.Id) return;
            SelectedProject.Update(e.Project);
            DetailsEstimateStatus = SelectedProject.EstimateStatus;
        }

        #endregion
        #region Implementation of IProjectsViewModel

        public IProjectViewModel SelectedProject
        {
            get { return _selectedProject; } 
            set
            {
                if (value == null)
                {
                    _selectedProject = null;
                    DetailsEnabled = false;
                }
                else
                {
                    if (_selectedProject == null)
                    {
                        _selectedProject = new ProjectViewModel(value);
                    }
                    _selectedProject.Update(value);
                    DetailsEstimateStatus = _selectedProject.EstimateStatus;
                    DetailsEnabled = true;
                    NotifyPropertyChanged(SelectedProjectPropertyName);
                }
            }
        }

        public void UpdateProject()
        {
            DetailsEstimateStatus = SelectedProject.EstimateStatus;
            _model.UpdateProject(SelectedProject);
        }

        #endregion
        #region Helper Functions

        private Project GetProject(int id)
        {
            return Projects.FirstOrDefault(p => p.Id == id);
        }

        #endregion
        #region Fields

        private readonly IProjectsModel _model;
        private IProjectViewModel _selectedProject;
        private Status _detailsEstimateStatus = Status.None;
        private bool _detailsEnabled;
        private readonly ICommand _updateCommand;

        #endregion

    }

    public class UpdateCommand : ICommand
    {
        public UpdateCommand(ProjectsViewModel projectsViewModel)
        {
            _vm = projectsViewModel;
            _vm.PropertyChanged += VmPropertyChanged;
        }

        private void VmPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName==ProjectsViewModel.SelectedProjectPropertyName)
            {
                CanExecuteChanged(this, new EventArgs());
            }
        }

        #region Implementation of ICommand

        public void Execute(object parameter)
        {
            _vm.UpdateProject();
        }

        public bool CanExecute(object parameter)
        {
            if (_vm.SelectedProject == null) return false;
            return _vm.SelectedProject.Id > NoneSelected;
        }

        public event EventHandler CanExecuteChanged = delegate { };

        #endregion


        #region Fields

        private const int NoneSelected = -1;
        private IProjectsViewModel _vm;

        #endregion
    }
}
