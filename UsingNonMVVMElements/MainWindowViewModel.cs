using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace UsingNonMVVMElements
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region [--Properties--]

        #region [--SourcePage--]

        private string _sourcePage = "http://www.baidu.com";

        public string SourcePage
        {
            get { return _sourcePage; }
            set
            {
                if (_sourcePage == value) return;
                _sourcePage = value;
                RaisePropertyChanged(() => SourcePage);
            }
        }

        #endregion [--SourcePage--]

        #region [--UserSuggestedSourcePage--]

        private string _userSuggestedSourcePage;

        public string UserSuggestedSourcePage
        {
            get { return _userSuggestedSourcePage; }
            set
            {
                if (_userSuggestedSourcePage == value) return;
                _userSuggestedSourcePage = value;
                RaisePropertyChanged(() => UserSuggestedSourcePage);
            }
        }

        #endregion [--UserSuggestedSourcePage--]

        #endregion [--Properties--]

        #region [--NavigateUrlCommand--]

        private ICommand _navigateUrlCommand;

        public ICommand NavigateUrlCommand
        {
            get { return _navigateUrlCommand = _navigateUrlCommand ?? new RelayCommand(NavigateUrl); }
        }

        private void NavigateUrl()
        {
            if (Uri.IsWellFormedUriString(_userSuggestedSourcePage, UriKind.Absolute))
                SourcePage = UserSuggestedSourcePage;
        }

        #endregion [--NavigateUrlCommand--]    
    }
}