using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using CarDocu.Models;
using CarDocu.Models.Settings;
using CarDocu.Services;
using GeneralTools.Services;
using WpfTools4.Commands;
using WpfTools4.ViewModels;

namespace CarDocu.ViewModels
{
    public class AppSettingsEditViewModel : ViewModelBase
    {
        #region Properties

        public static AppSettings AppSettings
        {
            get { return DomainService.Repository.AppSettings; }
        }

        public static DomainGlobalSettings GlobalSettings
        {
            get { return DomainService.Repository.GlobalSettings; }
        }

        public IEnumerable<Archive> Archives { get { return GlobalSettings == null ? null : GlobalSettings.Archives; } }

        public SmtpSettings SmtpSettings { get { return GlobalSettings == null ? null : GlobalSettings.SmtpSettings; } }

        public SapSettings SapSettings { get { return GlobalSettings == null ? null : GlobalSettings.SapSettings; } }

        public ScanSettings ScanSettings { get { return GlobalSettings == null ? null : GlobalSettings.ScanSettings; } }

        public string UserHintForArchives { get; set; }

        public bool CurrentUserIsMaster { get { return DomainService.Repository.LogonUser.IsMaster; } }

        public ICommand SaveCommand { get; private set; }
        public ICommand SetPathCommand { get; private set; }
        public ICommand OpenScanSettingsCommand { get; private set; }

        private bool _appSettingsPropertyChanged;
        private bool _globalItemsPropertyChanged;

        #endregion


        public AppSettingsEditViewModel(string userHintForArchives = null)
        {
            UserHintForArchives = userHintForArchives;
            AppSettings.PropertyChanged += AppSettingsItemsPropertyChanged;

            if (Archives != null)
                Archives.ToList().ForEach(archive => archive.PropertyChanged += GlobalItemsPropertyChanged);
            if (SmtpSettings != null)
                SmtpSettings.PropertyChanged += GlobalItemsPropertyChanged;
            if (SapSettings != null)
                SapSettings.PropertyChanged += GlobalItemsPropertyChanged;
            if (ScanSettings != null)
                ScanSettings.PropertyChanged += GlobalItemsPropertyChanged;

            SaveCommand = new DelegateCommand(e => Save(true), e => CanSave());
            SetPathCommand = new DelegateCommand(SetPath);
            OpenScanSettingsCommand = new DelegateCommand(OpenScanSettings);
        }

        private void AppSettingsItemsPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            _appSettingsPropertyChanged = true;
        }

        private void GlobalItemsPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            _globalItemsPropertyChanged = true;
        }

        static void OpenScanSettings(object e)
        {
            try { TwainService.Instance.StartScan(null, true); }
            catch { Tools.AlertError("Fehler beim Öffnen der Scanner Einstellungen.\r\n\r\nStellen Sie bitte sicher, dass der Scanner angeschlossen und eingeschaltet ist."); }
        }

        static void SetPath(object e)
        {
            var path = "";
            if (e == null)
            {
                if (GetPathFromDialog(AppSettings.DomainPath, "Pfad zur Konfigurationsablage für diese Domain setzen:", ref path))
                    AppSettings.DomainPath = path;
            }
            else
            {
                var archive = GlobalSettings.Archives.FirstOrDefault(a => a.ID == (string) e);
                if (archive != null)
                    if (GetPathFromDialog(AppSettings.DomainPath, string.Format("Pfad zum {0} setzen:", archive.Name) , ref path))
                        archive.Path = path;
            }
        }

        static bool GetPathFromDialog(string originalPath, string caption, ref string newPath)
        {
            var path = App.GetFolderFromDialog(originalPath, caption);
            if (string.IsNullOrEmpty(path))
                return false;

            newPath = path;
            
            return true;
        }

        public bool CanSave()
        {
            return _globalItemsPropertyChanged || (_appSettingsPropertyChanged && AppSettings.IsValidAtFirstGlance);
        }

        public bool Save(bool confirmWithAppRestart)
        {
            if (_appSettingsPropertyChanged)
            {
                if (!FileService.PathExistsAndWriteEnabled(AppSettings.DomainPath, Tools.AlertCritical, " zur Konfigurationsablage für diese Domain "))
                    return false;

                DomainService.Repository.AppSettingsSave();
                _appSettingsPropertyChanged = false;

                if (confirmWithAppRestart)
                    if (Tools.Confirm("Sie haben den Pfad zur Konfigurationsablage für diese Domain erfolgreich geändert!\r\n\r\nUm die Änderungen wirksam zu machen, starten Sie bitte diese Anwendung neu.\r\n\r\nAnwendung jetzt neu starten?"))
                        App.Restart();
            }

            if (_globalItemsPropertyChanged)
            {
                foreach (var archive in GlobalSettings.Archives.ToList())
                    if (!FileService.PathExistsAndWriteEnabled(archive.Path, Tools.AlertCritical, " für '" + archive.Name + "' "))
                        return false;

                DomainService.Repository.GlobalSettingsSave();
                _globalItemsPropertyChanged = false;
            }

            return true;
        }
    }
}
