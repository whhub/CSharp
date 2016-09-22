using System.Collections.Generic;

namespace ProjectBilling.DataAccess
{
    public interface IProject
    {
        int Id { get; set; }
        string Name { get; set; }
        double Estimate { get; set; }
        double Actual { get; set; }
        void Update(IProject project);
    }

    public class Project : IProject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Estimate { get; set; }
        public double Actual { get; set; }

        public void Update(IProject project)
        {
            Name = project.Name;
            Estimate = project.Estimate;
            Actual = project.Actual;
        }
    }


    public interface IDataService
    {
        IList<Project> GetProjects();
    }

    public class DataServiceStub : IDataService
    {
        public IList<Project> GetProjects()
        {
            var projects = new List<Project>
                               {
                                   new Project
                                       {
                                           Id = 0,
                                           Name = "Halloway",
                                           Estimate = 500
                                       },
                                   new Project
                                       {
                                           Id = 1,
                                           Name = "Jones",
                                           Estimate = 1500
                                       },
                                   new Project
                                       {
                                           Id = 2,
                                           Name = "Smith",
                                           Estimate = 2000
                                       }
                               };
            return projects;
        }
    }
}

