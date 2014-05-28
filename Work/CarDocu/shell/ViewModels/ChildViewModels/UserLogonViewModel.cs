using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using CarDocu.Models;
using CarDocu.Services;
using WpfTools4.Commands;
using WpfTools4.ViewModels;

namespace CarDocu.ViewModels
{
    public class UserLogonViewModel : ViewModelBase
    {
        #region Properties

        public string Title { get { return string.Format("{0}, Login", DomainService.AppName); } }

        public static DomainGlobalSettings GlobalSettings
        {
            get { return DomainService.Repository.GlobalSettings; }
        }

        public List<DomainLocation> DomainLocations { get { return GlobalSettings.DomainLocations; } }

        private string  _domainLocationCode;
        public string DomainLocationCode
        {
            get { return _domainLocationCode; }
            set { _domainLocationCode = value; SendPropertyChanged("DomainLocationCode"); }
        }

        public ICommand OkCommand { get; private set; }

        private string _userLoginName; 
        public string UserLoginName 
        { 
            get { return _userLoginName; }
            set
            {
                _userLoginName = value; 
                SendPropertyChanged("UserLoginName");

                var tryPeekDomainUser = GlobalSettings.DomainUsers.FirstOrDefault(u => u.LoginName == UserLoginName);
                if (tryPeekDomainUser != null && DomainLocations != null && DomainLocations.Any(d => d.SapCode == tryPeekDomainUser.DomainLocationCode))
                    DomainLocationCode = tryPeekDomainUser.DomainLocationCode;
            }
        }

        public string LoginData { get; set; }

        #endregion


        public UserLogonViewModel()
        {
            OkCommand = new DelegateCommand(e => LoginData = string.Format("{0}~{1}", UserLoginName, DomainLocationCode), 
                                            e => !string.IsNullOrEmpty(UserLoginName) && !string.IsNullOrEmpty(DomainLocationCode));
        }
    }
}
