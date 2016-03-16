using System.Collections.Generic;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Database.Models;

namespace CkgDomainLogic.DomainCommon.Contracts
{
    public interface IInfoCenterDataService : ICkgGeneralDataService
    {
        List<Document> DocumentsForAll { get; }

        List<Document> DocumentsForCurrentCustomer { get; }

        List<Document> DocumentsForCurrentGroup { get; }

        List<UserGroup> UserGroupsOfCurrentCustomer { get; }

        Document SaveDocument(Document document);

        bool SaveDocument(DokumentErstellenBearbeiten dokumentBearbeiten);

        bool DeleteDocument(int documentId);

        List<DocumentType> DocumentTypesForAll { get; }

        List<DocumentType> DocumentTypes { get; }

        DocumentType SaveDocumentType(DocumentType documentType);

        bool DeleteDocumentType(int documentTypeId);

        List<DocumentRight> DocumentRights { get; }
    }
}

