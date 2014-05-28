﻿using System.Collections.ObjectModel;
using CKGDatabaseAdminLib.Models.DbModels;
using CkgDomainLogic.General.Contracts;

namespace CKGDatabaseAdminLib.Contracts
{
    public interface IFieldTranslationCopyDataService : ICkgGeneralDataService
    {
        ObservableCollection<ApplicationInfo> Applications { get; }

        ObservableCollection<ApplicationField> FieldTranslations { get; }

        bool IsInEditMode { get; }

        void InitDataContext(string connectionName);

        void BeginEdit(int appId, string appURL);

        void CopyFieldTranslations(string destinationDb);

        void ResetCurrentApp();
    }
}