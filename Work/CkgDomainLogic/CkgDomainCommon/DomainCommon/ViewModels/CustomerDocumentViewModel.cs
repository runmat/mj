using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.ViewModels;
using GeneralTools.Models;

namespace CkgDomainLogic.DomainCommon.ViewModels
{
    public class CustomerDocumentViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public ICustomerDocumentDataService DataService { get { return CacheGet<ICustomerDocumentDataService>(); } }

        [XmlIgnore]
        public List<CustomerDocument> Documents { get { return DataService.Documents; } }

        [XmlIgnore]
        public List<CustomerDocumentCategory> Categories { get { return DataService.Categories; } }

        public CustomerDocument AddItem(CustomerDocument item)
        {
            return DataService.SaveDocument(item);
        }

        public void RemoveItem(int id)
        {
            DataService.DeleteDocument(id);
        }

        #region Filter

        [XmlIgnore]
        private List<CustomerDocument> m_DocumentsFiltered;
        [XmlIgnore]
        public List<CustomerDocument> DocumentsFiltered
        {
            get { return m_DocumentsFiltered ?? Documents; }
            private set { m_DocumentsFiltered = value; }
        }

        public void FilterDocuments(string filterValue, string filterProperties)
        {
            DocumentsFiltered = Documents.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        [XmlIgnore]
        private List<CustomerDocumentCategory> m_CategoriesFiltered;
        [XmlIgnore]
        public List<CustomerDocumentCategory> CategoriesFiltered
        {
            get { return m_CategoriesFiltered ?? Categories; }
            private set { m_CategoriesFiltered = value; }
        }

        public void FilterCategories(string filterValue, string filterProperties)
        {
            CategoriesFiltered = Categories.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
