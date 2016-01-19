using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using CKGDatabaseAdminLib.Contracts;
using CkgDomainLogic.General.Services;
using CKGDatabaseAdminLib.Models;

namespace CKGDatabaseAdminLib.Services
{
    public class LoginUserMessageDataServiceSql : CkgGeneralDataService, ILoginUserMessageDataService
    {
        public ObservableCollection<LoginUserMessage> LoginMessages { get { return _dataContext.LoginUserMessages.Local; } }

        private DatabaseContext _dataContext;

        public LoginUserMessageDataServiceSql(string connectionName)
        {
            InitDataContext(connectionName);
        }

        public void InitDataContext(string connectionName)
        {
            var sectionData = Config.GetAllDbConnections();
            _dataContext = new DatabaseContext(sectionData.Get(connectionName));

            _dataContext.LoginUserMessages.Load();
        }

        public void AddItem()
        {
            var newItem = new LoginUserMessage {Created = DateTime.Now, EditMode = true};
            LoginMessages.Add(newItem);
            _dataContext.SaveChanges();
        }

        public void DeleteItem(int id)
        {
            var item = LoginMessages.First(l => l.ID == id);
            LoginMessages.Remove(item);
            _dataContext.SaveChanges();
        }

        public void BeginEdit(int id)
        {
            SetEditMode(id, true);
        }

        public void CancelEdit(int id)
        {
            var item = LoginMessages.First(l => l.ID == id);
            _dataContext.Entry(item).CurrentValues.SetValues(_dataContext.Entry(item).OriginalValues);
            _dataContext.Entry(item).State = EntityState.Unchanged;
            SetEditMode(id, false);
        }

        public void SaveItem(int id)
        {
            var item = LoginMessages.First(l => l.ID == id);
            item.Created = DateTime.Now;
            _dataContext.SaveChanges();
            SetEditMode(id, false);
        }

        private void SetEditMode(int id, bool editMode)
        {
            var item = LoginMessages.First(l => l.ID == id);
            item.EditMode = editMode;
        }
    }
}