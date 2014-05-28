using System.Collections.ObjectModel;
using CKGDatabaseAdminLib.Models.DbModels;
using CkgDomainLogic.General.Contracts;

namespace CKGDatabaseAdminLib.Contracts
{
    public interface IApplicationCopyDataService : ICkgGeneralDataService
    {
        ObservableCollection<ApplicationInfo> Applications { get; }

        ObservableCollection<ApplicationInfo> ChildApplications { get; }

        ObservableCollection<ApplicationField> FieldTranslations { get; }

        ObservableCollection<ColumnTranslation> ColumnTranslations { get; }

        bool IsInEditMode { get; }

        void InitDataContext(string connectionName);

        void BeginEdit(int appId, string appURL);

        int? CopyApplication(string destinationDb, bool blnChildApplications, bool blnFieldTranslations, bool blnColumnTranslations);

        void ResetCurrentApp();
    }
}