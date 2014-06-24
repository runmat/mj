using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.ViewModels;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Autohaus.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Autohaus.ViewModels
{
    public class FahrzeugakteDocsViewModel : CustomerDocumentViewModel
    {
        [XmlIgnore]
        public List<FahrzeugakteDocument> FahrzeugakteDocuments { get { return MakeFahrzeugakteDocumentList(DataService.Documents); } }

        public int FahrzeugID { get; private set; }

        public int NewDocCategoryID { get; set; }
        public string NewDocBemerkung { get; set; }

        #region Dokumente

        public void LoadFahrzeugakteDocs(int id)
        {
            FahrzeugID = id;

            RefreshCategories();
            RefreshDocuments();
        }

        private void RefreshDocuments()
        {
            DataService.ApplicationKey = "Fahrzeugakte";
            DataService.ReferenceKey = FahrzeugID.ToString().PadLeft(10, '0');
            PropertyCacheClear(this, m => m.DocumentsFiltered);
            PropertyCacheClear(this, m => m.FahrzeugakteDocumentsFiltered);  
        }

        public FahrzeugakteDocument GetFahrzeugakteDoc(string id)
        {
            return FahrzeugakteDocuments.Find(s => s.ID.ToString() == id.TrimStart('0'));
        }

        public FahrzeugakteDocument GetNewFahrzeugakteDoc()
        {
            return new FahrzeugakteDocument { CustomerID = LogonContext.Customer.CustomerID, ApplicationKey = "Fahrzeugakte", FahrzeugID = FahrzeugID };
        }

        public void UpdateDocument(FahrzeugakteDocument model, ModelStateDictionary state)
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

                var erg = DataService.SaveDocument(tempDoc);
                if (erg == null)
                {
                    state.AddModelError("", Localize.SaveFailed);
                }

                RefreshDocuments();
            }
        }

        public int CreateDocument(FahrzeugakteDocument model, ModelStateDictionary state)
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
                    AdditionalData = model.AdditionalData,
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

        private List<FahrzeugakteDocument> MakeFahrzeugakteDocumentList(List<CustomerDocument> inList)
        {
            var outList = new List<FahrzeugakteDocument>();

            foreach (var item in inList)
            {
                outList.Add(new FahrzeugakteDocument
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
            var liste = new List<CustomerDocumentCategory>();

            foreach (var cat in Categories)
            {
                if (!FahrzeugakteDocuments.Exists(d => d.CategoryID == cat.ID))
                {
                    liste.Add(cat);
                }
            }

            return liste;
        }

        #region Filter

        [XmlIgnore]
        public List<FahrzeugakteDocument> FahrzeugakteDocumentsFiltered
        {
            get { return PropertyCacheGet(() => FahrzeugakteDocuments); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterFahrzeugakteDocuments(string filterValue, string filterProperties)
        {
            FahrzeugakteDocumentsFiltered = FahrzeugakteDocuments.SearchPropertiesWithOrCondition(filterValue, filterProperties);
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
            DataService.ApplicationKey = "Fahrzeugakte";
            PropertyCacheClear(this, m => m.CategoriesFiltered);
        }

        public CustomerDocumentCategory GetDocCategory(string id)
        {
            return Categories.Find(s => s.ID.ToString() == id.TrimStart('0'));
        }

        public CustomerDocumentCategory GetNewDocCategory()
        {
            return new CustomerDocumentCategory { CustomerID = LogonContext.Customer.CustomerID, ApplicationKey = "Fahrzeugakte" };
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
