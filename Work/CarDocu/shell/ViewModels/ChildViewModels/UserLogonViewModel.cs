using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using CarDocu.Models;
using CarDocu.Services;
using GeneralTools.Models;
using WpfTools4.Commands;
using AppDomain = CarDocu.Models.AppDomain;

namespace CarDocu.ViewModels
{
    public class UserLogonViewModel : AppSettingsEditViewModel
    {
        #region Properties

        public string Title { get { return $"{DomainService.AppName}, Login"; } }

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

        public bool DomainSelectionAvailable
        {
            get { return AppSettings.AskForDomainSelectionAtLogin; }
        }

        bool _specifyDomainManually;
        public bool SpecifyDomainManually
        {
            get { return _specifyDomainManually; }
            set
            {
                _specifyDomainManually = value;
                SendPropertyChanged("SpecifyDomainManually");
            }
        }

        string _selectedRecentAppDomain;
        public string SelectedRecentAppDomain
        {
            get { return _selectedRecentAppDomain; }
            set
            {
                _selectedRecentAppDomain = value;
                SendPropertyChanged("SelectedRecentAppDomain");

                SpecifyDomainManually = value.NotNullOrEmpty() == "-";

                var matchingRecentAppDomain = RecentAppDomains.FirstOrDefault(ra => string.Equals(ra.DomainName.NotNullOrEmpty(), value.NotNullOrEmpty(), StringComparison.CurrentCultureIgnoreCase));
                if (matchingRecentAppDomain != null)
                {
                    _manualDomainName = SelectedRecentAppDomain;
                    if (_manualDomainName.NotNullOrEmpty() == "-")
                        _manualDomainName = "";
                    _manualDomainPath = matchingRecentAppDomain.DomainPath;

                    SendPropertyChanged("ManualDomainName");
                    SendPropertyChanged("ManualDomainPath");
                }
            }
        }

        public List<AppDomain> RecentAppDomains
        {
            get { return AppSettings.RecentAppDomains; }
            set
            {
                AppSettings.RecentAppDomains = value;
                SendPropertyChanged("RecentAppDomains");
            }
        }

        string _manualDomainName;
        public string ManualDomainName
        {
            get { return _manualDomainName; }
            set
            {
                _manualDomainName = value;
                SendPropertyChanged("ManualDomainName");

                if (_manualDomainName.NotNullOrEmpty() == "")
                    return;

                var matchingRecentAppDomain = RecentAppDomains.FirstOrDefault(ra => string.Equals(ra.DomainName.NotNullOrEmpty(), value.NotNullOrEmpty(), StringComparison.CurrentCultureIgnoreCase));
                if (matchingRecentAppDomain != null)
                {
                    SelectedRecentAppDomain = matchingRecentAppDomain.DomainName;

                    _manualDomainName = SelectedRecentAppDomain;
                    _manualDomainPath = matchingRecentAppDomain.DomainPath;

                    SendPropertyChanged("ManualDomainName");
                    SendPropertyChanged("ManualDomainPath");
                }
            }
        }

        string _manualDomainPath;
        public string ManualDomainPath
        {
            get { return _manualDomainPath; }
            set
            {
                _manualDomainPath = value;
                SendPropertyChanged("ManualDomainPath");

                var matchingRecentAppDomain = RecentAppDomains.FirstOrDefault(ra => string.Equals(ra.DomainPath.NotNullOrEmpty(), value.NotNullOrEmpty(), StringComparison.CurrentCultureIgnoreCase));
                if (matchingRecentAppDomain != null)
                {
                    SelectedRecentAppDomain = matchingRecentAppDomain.DomainName;

                    _manualDomainName = matchingRecentAppDomain.DomainName;
                    _manualDomainPath = matchingRecentAppDomain.DomainPath;

                    SendPropertyChanged("ManualDomainPath");
                    SendPropertyChanged("ManualDomainName");
                }
            }
        }

        #endregion


        public UserLogonViewModel()
        {
            OkCommand = new DelegateCommand(e => LoginData = $"{UserLoginName}~{DomainLocationCode}", 
                                            e => !string.IsNullOrEmpty(UserLoginName) && !string.IsNullOrEmpty(DomainLocationCode));
        }
    }
}
