using GalaSoft.MvvmLight;

namespace EIMPrint
{
    public class CoverViewModel : ViewModelBase
    {
        #region [--Name--]

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value) return;
                _name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        #endregion [--Name--]

        #region [--Company--]

        private string _company;

        public string Company
        {
            get { return _company; }
            set
            {
                if (_company == value) return;
                _company = value;
                RaisePropertyChanged(() => Company);
            }
        }

        #endregion [--Company--]


    }
}
