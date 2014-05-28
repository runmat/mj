using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CarDocu.Models;
using CarDocu.Services;
using WpfTools4.Commands;
using WpfTools4.ViewModels;

namespace CarDocu.ViewModels
{
    public class BackstageViewModel : ViewModelBase
    {
        public ICommand FormsPilotUserInsertCommand { get; private set; }
        public ICommand AppExitCommand { get; private set; }

        public BackstageViewModel()
        {
            AppExitCommand = new DelegateCommand(u => Application.Current.MainWindow.Close());

            RefreshUserUIHintEntities();
        }

        private ObservableCollection<UserUIHintEntity> _userUIHintEntities; 
        public ObservableCollection<UserUIHintEntity> UserUIHintEntities 
        { 
            get { return _userUIHintEntities; }
            set { _userUIHintEntities = value; SendPropertyChanged("UserUIHintEntities"); }
        }

        public void RefreshUserUIHintEntities()
        {
            var list = DomainService.Repository.UserSettings.UIHelpHints
                .Select(k => new UserUIHintEntity
                {
                    Key = k.Key,
                    Title = k.Value,
                    IsConfirmed = true
                }).ToList();

            list.ForEach(ui => ui.PropertyChanged += UserUIHintEntityPropertyChanged);
            UserUIHintEntities = new ObservableCollection<UserUIHintEntity>(list);
        }

        static void UserUIHintEntityPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var uiHint = (UserUIHintEntity) sender;

            if (e.PropertyName != "IsConfirmed")
                return;

            if (uiHint.IsConfirmed)
                DomainService.Repository.UserSettings.UIHelpHintConfirm(uiHint.Key, uiHint.Title);
            else
                DomainService.Repository.UserSettings.UIHelpHintRemove(uiHint.Key);
        }
    }
}
