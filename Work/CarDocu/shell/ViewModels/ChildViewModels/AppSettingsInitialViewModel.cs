using System.Windows;
using System.Windows.Input;
using CarDocu.Services;
using WpfTools4.Commands;

namespace CarDocu.ViewModels
{
    public class AppSettingsInitialViewModel 
    {
        #region Properties

        public string Title => $"Start-Einstellungen {DomainService.AppName}, Version {DomainService.AppVersion}";

        private readonly string _userHintForArchives;

        private AppSettingsEditViewModel _appSettingsEditViewModel;
        public AppSettingsEditViewModel AppSettingsEditViewModel => (_appSettingsEditViewModel ?? (_appSettingsEditViewModel = new AppSettingsEditViewModel(_userHintForArchives)));

        public ICommand OkCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public bool Success { get; set; }

        private readonly Window _window;

        #endregion


        public AppSettingsInitialViewModel(Window window, string userHintForArchives = null)
        {
            _userHintForArchives = userHintForArchives;
            _window = window;
            OkCommand = new DelegateCommand(e => Ok(), e => CanOk());
            CancelCommand = new DelegateCommand(e => Cancel());
        }

        static bool CanOk()
        {
            return AppSettingsEditViewModel.AppSettings.IsValidAtFirstGlance;
        }

        void Ok()
        {
            if (!AppSettingsEditViewModel.Save(false))
                return;

            Success = true;

            _window.Close();
        }

        void Cancel()
        {
            _window.Close();
        }
    }
}
