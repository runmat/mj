using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CarDocu.Models;
using GeneralTools.Services;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows.Input;
using WebTools.Services;

namespace CarDocu.Services
{
    public static class DomainService
    {
        public static bool DebugIsAdminEnvironment => Environment.UserName.ToLower().Contains("jenzenm") &&
                                                      Environment.MachineName.ToUpper().Contains("AHW590");

        public static string AppName => AppSettings.AppName;

        public static string AppVersion => $"{Assembly.GetEntryAssembly().GetName().Version.Major}.{Assembly.GetEntryAssembly().GetName().Version.Minor.ToString("00")}";

        public static DateTime JobCancelDate => DateTime.Parse("01.01.2000");

        public static string DomainPath => Repository.AppSettings.DomainPath;

        public static string DomainName => Repository.AppSettings.DomainName;

        private static DomainRepository _repository;
        public static DomainRepository Repository => (_repository ?? (_repository = new DomainRepository()));
        public static bool RepositoryIsInitialized => _repository != null;

        private static DomainThreads _threads;
        public static DomainThreads Threads => (_threads ?? (_threads = new DomainThreads()));

        private static SimpleLogger _logger;
        public static SimpleLogger Logger => (_logger ?? (_logger = new SimpleLogger(Repository.UserErrorLogDirectoryName)));

        private static ObservableCollection<StatusMessage>  _statusMessages = new ObservableCollection<StatusMessage>();
        public static ObservableCollection<StatusMessage> StatusMessages => (_statusMessages ?? (_statusMessages = new ObservableCollection<StatusMessage>()));

        static public bool UserLogon(Func<string> getUserLoginDataFromDialog, bool forceAdminLogon = false)
        {
            if (forceAdminLogon)
            {
                Repository.LogonUser = Repository.AdminUser;
                Repository.LoadRemainingSettings();
                return true;
            }

            string loginData;
            var defaultUser = Repository.GlobalSettings.DomainUsers.FirstOrDefault(u => u.IsDefaultUser);
            var forceLoginPanelKeyPressed = Keyboard.IsKeyDown(Key.F8);
            var forceDomainSelection = Repository.AppSettings.AskForDomainSelectionAtLogin;

            if (defaultUser != null && !forceLoginPanelKeyPressed && !forceDomainSelection)
                loginData = $"{defaultUser.LoginName}~{Repository.GlobalSettings.DomainLocations.First().SapCode}";
            else
                loginData = getUserLoginDataFromDialog();

            if (string.IsNullOrEmpty(loginData) || !loginData.Contains("~"))
                return false;

            var loginDataArray = loginData.Split('~');
            var loginName = loginDataArray[0];

            var logonUser = Repository.GlobalSettings.DomainUsers.FirstOrDefault(user => user.LoginName == loginName);
            if (logonUser == null)
            {
                if (!string.IsNullOrEmpty(loginName))
                    Tools.AlertError($"{AppName}:\r\n\r\nLogin fehlgeschlagen!\r\n\r\nBenutzer '{loginName}' ist in der Domain '{DomainName}' nicht bekannt!");

                return false;
            }
            logonUser.DomainLocation = Repository.GlobalSettings.DomainLocations.First(loc => loc.SapCode == loginDataArray[1]);
            Repository.GlobalSettingsSave();

            Repository.LogonUser = logonUser;
            Repository.LoadRemainingSettings();

            return true;
        }

        public static void LoadGlobalSettings()
        {
            Repository.LoadGlobalSettings();
        }

        /// <summary>
        /// Stellt sicher, dass ein Pfad zu den Domänen bezogenen Einstellungen (Konfig Dateien, etc) verfügbar ist
        /// </summary>
        public static bool ValidDomainSettingsAvailable()
        {
            if (!string.IsNullOrEmpty(DomainPath) && !Directory.Exists(DomainPath))
                FileService.TryDirectoryCreate(DomainPath);

            if (string.IsNullOrEmpty(DomainPath) || !Directory.Exists(DomainPath))
                return false;

            if (string.IsNullOrEmpty(DomainName))
                return false;

            return true;
        }

        public static bool ValidArchivesAvailable()
        {
            if (Repository.GlobalSettings == null)
                return false;

            if (Repository.GlobalSettings.Archives.Any(archive => string.IsNullOrEmpty(archive.Path) && !archive.IsOptional))
                return false;

            return true;
        }

        public static bool SendMail(string to, string subject, string body, IEnumerable<string> filesToAttach = null)
        {
            if (Repository.GlobalSettings?.SmtpSettings == null)
                return false;

            return new SmtpMailService(Repository.GlobalSettings.SmtpSettings).SendMail(to, subject, body, filesToAttach); 
        }

        public static bool CheckOnlineState()
        {
            try
            {
                using (var client = new WebClient())
                    using (client.OpenRead("http://www.google.com"))
                        return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
