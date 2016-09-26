using System;
using ProjectBilling.DataAccess;

namespace ProjectBilling.Application.WPF
{
    public interface IProjectViewModel : IProject
    {
        Status EstimateStatus { get; set; }
    }

    public enum Status
    {
        None,
        Good,
        Bad
    }


    class ProjectViewModel : Notifier, IProjectViewModel
    {

        private int _id;

        private string _name;

        private double _estimate;

        private double _actual;

        private Status _estimateStatus;

        #region Implementation of IProject


        public int Id
        {
            get { return _id; }
            set
            {
                if (_id == value) return;
                _id = value;
                NotifyPropertyChanged(() => Id);
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value) return;
                _name = value;
                NotifyPropertyChanged(() => Name);
            }
        }

        public double Estimate
        {
            get { return _estimate; }
            set
            {
                _estimate = value;
                NotifyPropertyChanged(() => Estimate);
            }
        }

        public double Actual
        {
            get { return _actual; }
            set
            {
                _actual = value;
                UpdateEstimateStatus();
                NotifyPropertyChanged(() => Actual);
            }
        }

        public void Update(IProject project)
        {
            Id = project.Id;
            Name = project.Name;
            Estimate = project.Estimate;
            Actual = project.Actual;
        }



        #endregion

        #region Implementation of IProjectViewModel



        public Status EstimateStatus
        {
            get { return _estimateStatus; }
            set
            {
                if (_estimateStatus == value) return;
                _estimateStatus = value;
                NotifyPropertyChanged(() => EstimateStatus);
            }
        }

        #endregion

        public ProjectViewModel()
        {
            
        }

        public ProjectViewModel(IProject project)
        {
            if (project == null) return;
            Update(project);
        }

        private void UpdateEstimateStatus()
        {
            if (Math.Abs(Actual) < 1e-6)
            {
                EstimateStatus = Status.None;
            }
            else if(Actual <= Estimate)
            {
                EstimateStatus = Status.Good;
            }
            else
            {
                EstimateStatus = Status.Bad;
            }
        }

    }
}
