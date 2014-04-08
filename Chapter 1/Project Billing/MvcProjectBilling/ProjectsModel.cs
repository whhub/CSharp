using System;
using System.Collections.Generic;
using System.Linq;
using ProjectBilling.DataAccess;

namespace MvcProjectBilling
{

    public interface IProjectsModel
    {
        IEnumerable<Project> Projects { get; set; }
        event EventHandler<ProjectEventArgs> ProjectUpdated;
        void UpdateProject(Project project);
    }

    public class ProjectEventArgs : EventArgs
    {
        public Project Project { get; set; }

        public ProjectEventArgs(Project project)
        {
            Project = project;
        }
    }

    public class ProjectsModel : IProjectsModel
    {
        public IEnumerable<Project> Projects { get; set; }
        public event EventHandler<ProjectEventArgs> 
            ProjectUpdated = delegate { };
        public void UpdateProject(Project project)
        {
            var selectedProject = Projects.FirstOrDefault(p => p.Id == project.Id);
            if (selectedProject != null)
            {
                selectedProject.Name = project.Name;
                selectedProject.Estimate = project.Estimate;
                selectedProject.Actual = project.Actual;
                RaiseProjectUpdate(selectedProject);
            }
        }

        public ProjectsModel()
        {
            Projects = new DataServiceStub().GetProjects();
        }

        private void RaiseProjectUpdate(Project project)
        {
            ProjectUpdated(this, new ProjectEventArgs(project));
        }
    }
}
