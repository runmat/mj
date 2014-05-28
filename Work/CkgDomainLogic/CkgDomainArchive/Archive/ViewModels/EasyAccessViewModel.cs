using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Archive.Contracts;
using CkgDomainLogic.Archive.Models;

namespace CkgDomainLogic.Archive.ViewModels
{
    public class EasyAccessViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IEasyAccessDataService DataService { get { return CacheGet<IEasyAccessDataService>(); } }

        [XmlIgnore]
        public DataTable Documents { get { return DataService.Documents; } }

        public EasyAccessSuchparameter GetSuchparameter(Action<string, string> addModelError)
        {
            EasyAccessSuchparameter suchparameter = DataService.Suchparameter;
            if (DataService.HasErrors)
            {
                addModelError("", DataService.ErrorMessage);
            }
            return suchparameter;
        }

        public DataTable GetDocuments(Action<string, string> addModelError)
        {
            DataTable documents = DataService.Documents;
            if (DataService.HasErrors)
            {
                addModelError("", DataService.ErrorMessage);
            }
            return documents;
        }

        public void LoadArchives()
        {
            DataService.MarkForRefreshSuchparameter();
        }

        public void ApplyArchiveTypeAndLoadArchives(string arcType)
        {
            DataService.UpdateSuchparameter(arcType);
        }

        public void ApplySelectionAndLoadDocuments(EasyAccessSuchparameter suchparameter)
        {
            DataService.ApplySuchparameter(suchparameter);
            LoadDocuments();
        }

        public void LoadDocuments()
        {
            DataService.MarkForRefreshDocuments();
            PropertyCacheClear(this, m => m.DocumentsFiltered);
        }

        public string ViewDocument(string docId)
        {
            return DataService.ViewDocument(docId).Replace('\\', '/');
        }

        public EasyAccessDetail GetDocumentDetail(string docId)
        {
            return DataService.GetDocumentDetail(docId);
        }

        #region Filter

        [XmlIgnore]
        public DataTable DocumentsFiltered
        {
            get { return PropertyCacheGet(() => Documents); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterDocuments(string filterValue, string filterProperties)
        {
            List<string> hiddenCols = new List<string> { "DOC_LOCATION", "DOC_ARCHIVE", "DOC_ID", "DOC_VERSION", "BILDER", "LINK" };

            // Tabellenstruktur (ohne Daten) klonen
            DocumentsFiltered = Documents.Clone();

            string filterText = "";

            if (!String.IsNullOrEmpty(filterValue))
            {
                foreach (DataColumn col in Documents.Columns)
                {
                    // nur in den Spalten suchen, die auch im View angezeigt werden
                    if (!hiddenCols.Contains(col.ColumnName.ToUpper()))
                    {
                        filterText += col.ColumnName + " LIKE '%" + filterValue.Trim() + "%' OR ";
                    }
                }
                if (filterText.EndsWith("OR "))
                {
                    filterText = filterText.Substring(0, filterText.Length - 3);
                }
            }

            foreach (DataRow dRow in Documents.Select(filterText))
            {
                DataRow newRow = DocumentsFiltered.NewRow();
                newRow.ItemArray = dRow.ItemArray;
                DocumentsFiltered.Rows.Add(newRow);
            }
        }

        #endregion
    }
}
