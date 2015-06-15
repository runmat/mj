using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.ViewModels;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Insurance.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Insurance.ViewModels
{
    public class SchadenakteDocsViewModel : CustomerDocumentViewModel
    {
        [XmlIgnore]
        public List<SchadenakteDocument> SchadenakteDocuments { get { return MakeSchadenakteDocumentList(DataService.Documents); } }

        [XmlIgnore]
        public List<CustomerDocumentCategory> CategoriesWithoutZero { get { return DataService.Categories.Where(c => c.ID > 0).ToListOrEmptyList(); } }

        public int SchadenfallID { get; private set; }

        public int NewDocCategoryID { get; set; }
        public string NewDocDienstleister { get; set; }

        #region Dokumente

        public void LoadSchadenakteDocs(int id)
        {
            SchadenfallID = id;

            RefreshCategories();
            RefreshDocuments();
        }

        private void RefreshDocuments()
        {
            DataService.ApplicationKey = "Schadenakte";
            DataService.ReferenceKey = SchadenfallID.ToString().PadLeft(10, '0');
            PropertyCacheClear(this, m => m.DocumentsFiltered);
            PropertyCacheClear(this, m => m.SchadenakteDocumentsFiltered);  
        }

        public SchadenakteDocument GetSchadenakteDoc(string id)
        {
            return SchadenakteDocuments.Find(s => s.ID.ToString() == id.TrimStart('0'));
        }

        public SchadenakteDocument GetNewSchadenakteDoc()
        {
            return new SchadenakteDocument { CustomerID = LogonContext.Customer.CustomerID, ApplicationKey = "Schadenakte", SchadenfallID = SchadenfallID };
        }

        public void UpdateDocument(SchadenakteDocument model, ModelStateDictionary state)
        {
            var dublette = Documents.Find(d => d.CategoryID == model.CategoryID && d.ID != model.ID);
            if (dublette != null)
            {
                state.AddModelError("", Localize.DocumentForCategoryAlreadyExists);
            }
            else
            {
                var tempDoc = Documents.Find(d => d.ID == model.ID);

                tempDoc.CategoryID = model.CategoryID;
                tempDoc.AdditionalData = model.Dienstleister;

                var erg = DataService.SaveDocument(tempDoc);
                if (erg == null)
                {
                    state.AddModelError("", Localize.SaveFailed);
                }

                RefreshDocuments();
            }
        }

        public int CreateDocument(SchadenakteDocument model, ModelStateDictionary state)
        {
            var retInt = 0;

            var dublette = Documents.Find(d => d.CategoryID == model.CategoryID);
            if (dublette != null)
            {
                state.AddModelError("", Localize.DocumentForCategoryAlreadyExists);
            }
            else
            {
                var tempDoc = new CustomerDocument
                {
                    AdditionalData = model.Dienstleister,
                    ApplicationKey = model.ApplicationKey,
                    CategoryID = model.CategoryID,
                    CustomerID = model.CustomerID,
                    FileName = model.FileName,
                    FileType = model.FileType,
                    ID = model.ID,
                    ReferenceField = model.ReferenceField,
                    Uploaded = model.Uploaded
                };
                var erg = DataService.SaveDocument(tempDoc);
                if (erg == null)
                {
                    state.AddModelError("", Localize.SaveFailed);
                }
                else
                {
                    retInt = erg.ID;
                }
                RefreshDocuments();
            }

            return retInt;
        }

        public int DeleteDocument(string id)
        {
            var erg = 0;
            int tempInt;
            if (Int32.TryParse(id, out tempInt))
            {
                erg = DataService.DeleteDocument(tempInt);
                RefreshDocuments();
            }

            return erg;
        }

        private List<SchadenakteDocument> MakeSchadenakteDocumentList(List<CustomerDocument> inList)
        {
            var outList = new List<SchadenakteDocument>();

            foreach (var item in inList)
            {
                outList.Add(new SchadenakteDocument
                    {
                        ID = item.ID,
                        ReferenceField = item.ReferenceField,
                        FileType = item.FileType,
                        FileName = item.FileName,
                        CategoryID = item.CategoryID,
                        Category = item.Category,
                        Uploaded = item.Uploaded,
                        AdditionalData = item.AdditionalData,
                        CustomerID = item.CustomerID,
                        ApplicationKey = item.ApplicationKey
                    });
            }

            return outList;
        }

        public List<CustomerDocumentCategory> GetCategoriesWithoutDocuments()
        {
            var docList = SchadenakteDocuments;

            return CategoriesWithoutZero.Where(c => docList.None(d => d.CategoryID == c.ID)).ToList();
        }

        #region Filter

        [XmlIgnore]
        public List<SchadenakteDocument> SchadenakteDocumentsFiltered
        {
            get { return PropertyCacheGet(() => SchadenakteDocuments); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterSchadenakteDocuments(string filterValue, string filterProperties)
        {
            SchadenakteDocumentsFiltered = SchadenakteDocuments.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion

        #endregion

        #region Kategorien

        public void LoadDocCategories()
        {
            RefreshCategories();
        }

        private void RefreshCategories()
        {
            DataService.ApplicationKey = "Schadenakte";
            PropertyCacheClear(this, m => m.CategoriesFiltered);
        }

        public CustomerDocumentCategory GetDocCategory(string id)
        {
            return Categories.Find(s => s.ID.ToString() == id.TrimStart('0'));
        }

        public CustomerDocumentCategory GetNewDocCategory()
        {
            return new CustomerDocumentCategory { CustomerID = LogonContext.Customer.CustomerID, ApplicationKey = "Schadenakte" };
        }

        public void SaveCategory(CustomerDocumentCategory model, ModelStateDictionary state)
        {
            var dublette = Categories.Find(s => s.CategoryName == model.CategoryName && s.ID != model.ID);
            if (dublette != null)
            {
                state.AddModelError("", Localize.CategoryWithSameNameAlreadyExists);
            }
            else
            {
                var erg = DataService.SaveCategory(model);
                if (erg == null)
                {
                    state.AddModelError("", Localize.SaveFailed);
                }
                RefreshCategories();
            }
        }

        public int DeleteCategory(string id)
        {
            var erg = 0;
            int tempInt;
            if (Int32.TryParse(id, out tempInt))
            {
                erg = DataService.DeleteCategory(tempInt);
                RefreshCategories();
            }

            return erg;
        }

        #endregion
    }
}
