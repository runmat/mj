using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using GeneralTools.Models;

namespace CkgDomainLogic.DomainCommon.ViewModels
{
    public class DocumentViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IInfoCenterDataService DataService { get { return CacheGet<IInfoCenterDataService>(); } }

        [XmlIgnore]
        public List<Document> Documents 
        { 
            get
            {
                if (GeneralMode)
                    return DataService.DocumentsForAll;

                if (AdminMode)
                    return DataService.DocumentsForCurrentCustomer;

                return DataService.DocumentsForCurrentGroup;
            } 
        }

        [XmlIgnore]
        public List<DocumentType> DocumentTypes
        {
            get
            {
                if (GeneralMode)
                    return DataService.DocumentTypesForAll;

                return DataService.DocumentTypes;
            }
        }

        [XmlIgnore]
        public List<Document> DocumentsForCurrentGroup { get { return DataService.DocumentsForCurrentGroup; } }

        [XmlIgnore]
        public List<DocumentRight> DocumentRights { get { return DataService.DocumentRights; } }

        [XmlIgnore]
        public List<UserGroup> UserGroupsOfCurrentCustomer { get { return DataService.UserGroupsOfCurrentCustomer; } }

        public DokumentErstellenBearbeiten NewDocumentProperties { get { return PropertyCacheGet(() => new DokumentErstellenBearbeiten { DocTypeID = 1 }); } }

        public bool GeneralMode { get; set; }

        public bool AdminMode { get; set; }

        public bool OnMode { get; set; }

        public string ApplicationTitle
        {
            get
            {
                if (GeneralMode)
                    return (OnMode ? Localize.Document_KroschkeDokumentencenter : Localize.Document_Dokumentencenter);

                return (OnMode ? Localize.Document_KroschkeInfocenter : Localize.Document_Infocenter);
            }
        }

        public void DataInit(bool generalMode, bool adminMode, bool onMode)
        {
            GeneralMode = generalMode;
            AdminMode = adminMode;
            OnMode = onMode;

            DataMarkForRefresh(true);
        }

        private void DataMarkForRefresh(bool initial = false)
        {
            PropertyCacheClear(this, m => m.DocumentsFiltered);
            PropertyCacheClear(this, m => m.DocumentTypesFiltered);

            if (initial)    
                PropertyCacheClear(this, m => m.NewDocumentProperties);
        }

        public DokumentErstellenBearbeiten GetDocumentModel(int id)
        {
            var dokument = Documents.Single(x => x.DocumentID == id);
            var dokumentGruppen = DocumentRights.Where(d => d.DocumentID == id).Select(r => r.GroupID.ToString());

            return new DokumentErstellenBearbeiten
            {
                ID = id,
                Name = dokument.FileName,
                DocTypeID = dokument.DocTypeID,
                Tags = dokument.Tags,
                SelectedWebGroups = dokumentGruppen.ToList()
            };
        }

        public bool SaveDocument(string fileName, string fileExtension, int fileSize)
        {
            var now = DateTime.Now;

            var dokument = DataService.SaveDocument(new Document
                {
                    DocTypeID = NewDocumentProperties.DocTypeID,
                    FileName = fileName,
                    FileSize = fileSize,
                    LastEdited = now,
                    Uploaded = now,
                    CustomerID = (GeneralMode ? 1 : LogonContext.CustomerID),
                    FileType = fileExtension,
                    Tags = NewDocumentProperties.Tags
                }
            );

            NewDocumentProperties.ID = dokument.DocumentID;
            NewDocumentProperties.Name = dokument.FileName;

            DataMarkForRefresh();

            return DataService.SaveDocument(NewDocumentProperties);
        }

        public bool SaveDocument(DokumentErstellenBearbeiten item)
        {
            DataMarkForRefresh();

            return DataService.SaveDocument(item);
        }

        public bool DeleteDocument(int id)
        {
            DataMarkForRefresh();

            return DataService.DeleteDocument(id);
        }

        public DocumentType GetDocumentType(int id)
        {
            if (id == -1)
                return new DocumentType { CustomerID = (GeneralMode ? 1 : LogonContext.CustomerID) };

            return DocumentTypes.FirstOrDefault(t => t.DocumentTypeID == id);
        }

        public DocumentType SaveDocumentType(DocumentType item)
        {
            DataMarkForRefresh();

            return DataService.SaveDocumentType(item);
        }

        public bool DeleteDocumentType(int id)
        {
            DataMarkForRefresh();

            return DataService.DeleteDocumentType(id);
        }

        public int NumberOfDocumentsUsingDocType(int docTypeId)
        {
            if (GeneralMode)
                return DataService.DocumentsForAll.Count(d => d.DocTypeID == docTypeId);

            return DataService.DocumentsForCurrentCustomer.Count(d => d.DocTypeID == docTypeId);
        }

        #region Filter

        [XmlIgnore]
        public List<Document> DocumentsFiltered
        {
            get { return PropertyCacheGet(() => Documents); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterDocuments(string filterValue, string filterProperties)
        {
            DocumentsFiltered = Documents.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        [XmlIgnore]
        public List<DocumentType> DocumentTypesFiltered
        {
            get { return PropertyCacheGet(() => DocumentTypes); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterDocumentTypes(string filterValue, string filterProperties)
        {
            DocumentTypesFiltered = DocumentTypes.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
