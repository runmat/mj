using System.Collections.Generic;
using System.Data;
using CkgDomainLogic.Archive.Models;
using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.Archive.Contracts
{
    public interface IEasyAccessDataService : ICkgGeneralDataService
    {
        // Diese beiden Properties sollten später in die Definition von ICkgGeneralDataService hochgezogen werden
        bool HasErrors { get; }
        string ErrorMessage { get; set; }

        EasyAccessSuchparameter Suchparameter { get; }

        DataTable Documents { get; }

        void ApplySuchparameter(EasyAccessSuchparameter suchparameter);

        void UpdateSuchparameter(string archiveType);

        void MarkForRefreshSuchparameter();

        void MarkForRefreshDocuments();

        string ViewDocument(string docId);

        EasyAccessDetail GetDocumentDetail(string docId);

        List<string> GetDocuments(EasyAccessArchiveDefinition archiveToSearch, string query);

        List<string> GetDocuments(List<EasyAccessArchiveDefinition> archivesToSearch, string query);

        List<string> GetDocuments(string archiveLocation, string archiveName, string archiveIndex, string query);
    }
}
