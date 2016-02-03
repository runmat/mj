using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.Database.Services;
using CkgDomainLogic.General.Services;

namespace CkgDomainLogic.DomainCommon.Services
{
    public class InfoCenterDataService : CkgGeneralDataService, IInfoCenterDataService
    {
        private DomainDbContext CreateDbContext()
        {
            return new DomainDbContext(ConfigurationManager.AppSettings["Connectionstring"], LogonContext.UserName);
        }

        public List<Document> DocumentsForAll { get { return CreateDbContext().DocumentsForAll; } }

        public List<Document> DocumentsForCurrentCustomer { get { return CreateDbContext().DocumentsForCustomer; } }

        public List<Document> DocumentsForCurrentGroup { get { return CreateDbContext().DocumentsForGroup; } }

        public List<UserGroup> UserGroupsOfCurrentCustomer { get { return CreateDbContext().UserGroupsOfCurrentCustomer; } }

        public Document SaveDocument(Document document)
        {
            return CreateDbContext().SaveDocument(document);
        }

        public bool DeleteDocument(int documentId)
        {
            return CreateDbContext().DeleteDocument(documentId);
        }

        public bool SaveDocument(DokumentErstellenBearbeiten dokumentBearbeiten)
        {
            return CreateDbContext().SaveDocument(dokumentBearbeiten.ID, dokumentBearbeiten.DocTypeID, dokumentBearbeiten.SelectedWebGroups, dokumentBearbeiten.Tags);
        }

        public List<DocumentType> DocumentTypesForAll { get { return CreateDbContext().DocumentTypesForAll; } }

        public List<DocumentType> DocumentTypes { get { return CreateDbContext().DocumentTypesForCustomer; } }

        public DocumentType SaveDocumentType(DocumentType documentType)
        {
            return CreateDbContext().SaveDocumentType(documentType);
        }

        public bool DeleteDocumentType(int documentTypeId)
        {
            return CreateDbContext().DeleteDocumentType(documentTypeId);
        }

        public List<DocumentRight> DocumentRights { get { return CreateDbContext().DocumentRights.ToList(); } }
    }
}
