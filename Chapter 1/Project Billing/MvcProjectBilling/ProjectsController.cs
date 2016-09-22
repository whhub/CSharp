using System;
using ProjectBilling.DataAccess;

namespace MvcProjectBilling
{
    public interface IProjectsController
    {
        void ShowProjectsView(MainWindow owner);
        void Update(Project project);
    }

    public class ProjectsController : IProjectsController
    {

        private readonly IProjectsModel _model;

        public ProjectsController(IProjectsModel projectsModel)
        {
            if (projectsModel == null)
            {
                throw new ArgumentException("projectsModel");
            }
            _model = projectsModel;
        }

        public void ShowProjectsView(MainWindow owner)
        {
            var view = new ProjectsView(this, _model) {Owner = owner};
            view.Show();
        }

        public void Update(Project project)
        {
            _model.UpdateProject(project);
        }
    }
}
