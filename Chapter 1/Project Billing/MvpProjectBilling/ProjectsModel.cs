using System;
using System.Collections.Generic;
using System.Linq;
using ProjectBilling.DataAccess;

namespace MvpProjectBilling
{

    public class ProjectEventArgs : EventArgs
    {
        public Project Project { get; set; }

        public ProjectEventArgs(Project args)
        {
            Project = args;
        }
    }

    public interface IProjectsModel
    {
        void UpdateProject(Project project);
        IEnumerable<Project> GetProjects();
        Project GetProject(int id);
        event EventHandler<ProjectEventArgs> ProjectUpdated;
    }

    class ProjectsModel : IProjectsModel
    {

        public ProjectsModel()
        {
            _projects = new DataServiceStub().GetProjects();
        }

        public void UpdateProject(Project project)
        {
            ProjectUpdated(this, new ProjectEventArgs(project));
        }

        public IEnumerable<Project> GetProjects()
        {
            return _projects;
        }

        public Project GetProject(int id)
        {
            return _projects.FirstOrDefault(p => p.Id == id);
        }

        public event EventHandler<ProjectEventArgs> ProjectUpdated = delegate { };

        private readonly IList<Project> _projects;
    }
}
