using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.Models.DataModels;
using CkgDomainLogic.General.ViewModels;
using GeneralTools.Models;

namespace CkgDomainLogic.DomainCommon.ViewModels
{
    public class DokumentViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IInfoCenterDataService DataService { get { return CacheGet<IInfoCenterDataService>(); } }

        [XmlIgnore]
        public List<Dokument> Dokuments 
        { 
            get
            {
                if (IsAdministrator)
                {
                    return DataService.DokumentsForCurrentCustomer;
                }

                return DataService.DokumentsForCurrentGroup;
            } 
        }

        [XmlIgnore]
        public List<DocumentType> DocumentTypes { get { return DataService.DocumentTypes; } }

        [XmlIgnore]
        public List<Dokument> DokumentsForCurrentGroup { get { return DataService.DokumentsForCurrentGroup; } }

        public DocumentErstellenBearbeiten NewDocumentProperties { get; set; }

        public bool InsertMode { get; set; }

        #region Repository

        public Dokument GetItem(int id)
        {
            return Dokuments.FirstOrDefault(c => c.DocumentID == id);
        }

        public Dokument NewItem()
        {
            // Hier kann z.B. die CustomerID gesetzt werden
            return new Dokument();
        }

        public Dokument SaveItem(Dokument item, Action<string, string> addModelError)
        {
            return DataService.SaveDocument(item, addModelError);
        }

        public void RemoveItem(int id)
        {
            DataService.DeleteDocument(GetItem(id));
        }

        #endregion

        #region View

        public string AjaxBinding { get; set; }

        public bool IsAdministrator { get; set; }

        #endregion

        #region Filter

        [XmlIgnore]
        private List<Dokument> m_DokumentsFiltered;
        [XmlIgnore]
        public List<Dokument> DokumentsFiltered
        {
            get { return m_DokumentsFiltered ?? Dokuments; }
            private set { m_DokumentsFiltered = value; }
        }

        public void FilterDokuments(string filterValue, string filterProperties)
        {
            DokumentsFiltered = Dokuments.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        [XmlIgnore]
        private List<DocumentType> m_DocumentTypesFiltered;
        [XmlIgnore]
        public List<DocumentType> DocumentTypesFiltered
        {
            get { return m_DocumentTypesFiltered ?? DocumentTypes; }
            private set { m_DocumentTypesFiltered = value; }
        }

        public void FilterDocumentTypes(string filterValue, string filterProperties)
        {
            DocumentTypesFiltered = DocumentTypes.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
