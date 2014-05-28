using System.Collections.ObjectModel;
using CKGDatabaseAdminLib.Models.DbModels;
using CkgDomainLogic.General.Contracts;

namespace CKGDatabaseAdminLib.Contracts
{
    public interface ILoginUserMessageDataService : ICkgGeneralDataService
    {
        ObservableCollection<LoginUserMessage> LoginMessages { get; }

        void BeginEdit(int id);

        void SaveItem(int id);

        void CancelEdit(int id);

        void DeleteItem(int id);

        void AddItem();

        void InitDataContext(string connectionName);
    }
}