using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Xml.Serialization;
using CKGDatabaseAdminLib.Contracts;
using CKGDatabaseAdminLib.Models;
using CKGDatabaseAdminLib.Services;
using WpfTools4.Commands;
using WpfTools4.Services;
using WpfTools4.ViewModels;

namespace CKGDatabaseAdminLib.ViewModels
{
    public class LoginUserMessageViewModel : ViewModelBase
    {
        #region Properties

        public ObservableCollection<LoginUserMessage> LoginMessages { get { return DataService.LoginMessages; } }

        [XmlIgnore]
        private readonly ILoginUserMessageDataService DataService;

        public MainViewModel Parent { get; set; }

        public ICommand CommandManageLoginMessages { get; private set; }
        public ICommand CommandEditLoginMessage { get; private set; }
        public ICommand CommandDeleteLoginMessage { get; private set; }
        public ICommand CommandSaveLoginMessages { get; private set; }
        public ICommand CommandCancelEditLoginMessage { get; private set; }
        public ICommand CommandAddNewLoginMessage { get; private set; }

        #endregion

        public LoginUserMessageViewModel(MainViewModel parentVM)
        {
            Parent = parentVM;

            DataService = new LoginUserMessageDataServiceSql(Parent.ActualDatabase);

            CommandManageLoginMessages = new DelegateCommand(ManageLoginMessages);
            CommandEditLoginMessage = new DelegateCommand(EditLoginMessage);
            CommandDeleteLoginMessage = new DelegateCommand(DeleteLoginMessage);
            CommandSaveLoginMessages = new DelegateCommand(SaveLoginMessages);
            CommandCancelEditLoginMessage = new DelegateCommand(CancelEditLoginMessage);
            CommandAddNewLoginMessage = new DelegateCommand(AddNewLoginMessage);
        }

        #region Commands

        public void ManageLoginMessages(object parameter)
        {
            Parent.ActiveViewModel = this;
        }

        public void EditLoginMessage(object parameter)
        {
            var id = (parameter as int?);
            if (id.HasValue)
            {
                DataService.BeginEdit(id.Value);
            }
        }

        public void DeleteLoginMessage(object parameter)
        {
            if (!Tools.Confirm("Login-Message wirklich löschen?"))
                return;

            var id = (parameter as int?);
            if (id.HasValue)
            {
                DataService.DeleteItem(id.Value);
                Parent.ShowMessage("LoginMessage wurde erfolgreich gelöscht", MessageType.Success);
            }
            else
            {
                Parent.ShowMessage("LoginMessage konnte nicht gelöscht werden", MessageType.Error);
            }
        }

        public void SaveLoginMessages(object parameter)
        {
            var id = (parameter as int?);
            if (id.HasValue)
            {
                DataService.SaveItem(id.Value);
                Parent.ShowMessage("LoginMessage wurde erfolgreich gespeichert", MessageType.Success);
            }
            else
            {
                Parent.ShowMessage("LoginMessage konnte nicht gespeichert werden", MessageType.Error);
            }
        }

        public void CancelEditLoginMessage(object parameter)
        {
            var id = (parameter as int?);
            if (id.HasValue)
            {
                DataService.CancelEdit(id.Value);
            }
        }

        public void AddNewLoginMessage(object parameter)
        {
            DataService.AddItem();
        }

        #endregion

    }
}
