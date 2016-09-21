using System;
using System.Collections.ObjectModel;
using System.Linq;
using ProjectBilling.DataAccess;

namespace ProjectBilling.Application.WPF
{

    public interface IProjectsModel
    {
        ObservableCollection<Project> Projects { get; set; }
        event EventHandler<ProjectEventArgs> ProjectUpdated;
        void UpdateProject(IProject updatedProject);
    }

    public class ProjectEventArgs : EventArgs
    {
        public IProject Project { get; set; }

        public ProjectEventArgs(IProject project)
        {
            Project = project;
        }
    }

    public class ProjectsModel : IProjectsModel
    {
        #region Implementation of IProjectsModel

        public ObservableCollection<Project> Projects { get; set; }
        public event EventHandler<ProjectEventArgs> ProjectUpdated = delegate { };
        public void UpdateProject(IProject updatedProject)
        {
            GetProject(updatedProject.Id).Update(updatedProject);
            ProjectUpdated(this, new ProjectEventArgs(updatedProject));
        }

        private Project GetProject(int id)
        {
            return Projects.FirstOrDefault(p => p.Id == id);
        }

        #endregion

        public ProjectsModel(IDataService dataService)
        {
            Projects = new ObservableCollection<Project>();
            foreach (var project in dataService.GetProjects())
            {
                Projects.Add(project);
            }
        }
    }
}
