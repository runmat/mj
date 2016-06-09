using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using CarDocu.Services;
using GeneralTools.Models;

namespace CarDocu.Models
{
    public class AppSettings : AppDomain
    {
        List<AppDomain> _recentAppDomains = new List<AppDomain>();
        public List<AppDomain> RecentAppDomains
        {
            get { return _recentAppDomains; }
            set
            {
                _recentAppDomains = value;
                SendPropertyChanged("RecentAppDomains");
            }
        }

        private bool _askForDomainSelectionAtLogin;

        public bool AskForDomainSelectionAtLogin
        {
            get { return _askForDomainSelectionAtLogin; }
            set
            {
                _askForDomainSelectionAtLogin = value;
                SendPropertyChanged("AskForDomainSelectionAtLogin");
            }
        }

        private bool _onlineStatusAutoCheckDisabled;

        public bool OnlineStatusAutoCheckDisabled
        {
            get { return _onlineStatusAutoCheckDisabled; }
            set
            {
                _onlineStatusAutoCheckDisabled = value;
                SendPropertyChanged("OnlineStatusAutoCheckDisabled");
            }
        }

        private bool _globalDeleteAndBackupFileAfterDelivery;

        public bool GlobalDeleteAndBackupFileAfterDelivery
        {
            get { return _globalDeleteAndBackupFileAfterDelivery; }
            set
            {
                _globalDeleteAndBackupFileAfterDelivery = value;
                if (value && DomainService.RepositoryIsInitialized && DomainService.Repository.GlobalSettings.BackupArchive.Path.IsNullOrEmpty())
                {
                    _globalDeleteAndBackupFileAfterDelivery = false;
                    Tools.Alert("Diese Option kann erst aktiviert werden, wenn unter den Domain Einstellungen ein Backup Pfad hinterlegt wurde!");
                    return;
                }

                SendPropertyChanged("GlobalDeleteAndBackupFileAfterDelivery");

                if (DomainService.RepositoryIsInitialized)
                    DomainService.Repository.EnterpriseSettings.DocumentTypes.ForEach(dt => dt.SendPropertyChangedGlobalSettings());
            }
        }

        [XmlIgnore]
        public bool IsValidAtFirstGlance => !string.IsNullOrEmpty(DomainName) && !string.IsNullOrEmpty(DomainPath);

        [XmlIgnore]
        public static string AppCompanyName => ((AssemblyCompanyAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCompanyAttribute), false)).Company;

        [XmlIgnore]
        public static string AppSettingsDirectoryName => AppCompanyName + @"\CarDocu_Scan";

        [XmlIgnore]
        public static string AppName => "CKG Scan Client";

        [XmlIgnore]
        public static string AppVersion => $"{Assembly.GetEntryAssembly().GetName().Version.Major}.{Assembly.GetEntryAssembly().GetName().Version.Minor}";

        public void Init()
        {
            RecentAppDomains = RecentAppDomains.OrderBy(ra => ra.DomainName).ToListOrEmptyList();

            if (RecentAppDomains.None() && RecentAppDomains.None(ra => ra.DomainName.NotNullOrEmpty() == ""))
                RecentAppDomains.Insert(0, new AppDomain());

            if (RecentAppDomains.None(ra => ra.DomainName.NotNullOrEmpty() == "-"))
                RecentAppDomains.Add(new AppDomain { DomainName = "-" });
        }
    }
}
