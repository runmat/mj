using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Serialization;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using CkgDomainLogic.General.ViewModels;
using DocumentTools.Services;
using GeneralTools.Models;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;


namespace CkgDomainLogic.Fahrzeuge.ViewModels
{
    public class FehlteilEtikettenViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IFehlteilEtikettenDataService DataService { get { return CacheGet<IFehlteilEtikettenDataService>(); } }


        [LocalizedDisplay(LocalizeConstants.DataWithErrorsOccurred)]
        public bool UploadItemsErrorsOccurred { get { return UploadItems.Any(item => item.ValidationErrors.IsNotNullOrEmpty()); } }

        [LocalizedDisplay(LocalizeConstants.ListItemsWithErrorsOnly)]
        public bool UploadItemsShowErrorsOnly { get; set; }

        public List<FehlteilEtikett> UploadItemsFiltered 
        {
            get { return !UploadItemsShowErrorsOnly ? UploadItems : UploadItems.Where(item => item.ValidationErrors.IsNotNullOrEmpty()).ToList(); }
        }

        public List<FehlteilEtikett> UploadItems { get; private set; }

        public string CsvUploadFileName { get; private set; }
        public string CsvUploadServerFileName { get; private set; }

        public string UploadItemsStoredErrorMessage { get; set; }


        public FehlteilEtikettSelektor FehlteilEtikettSelektor
        {
            get { return PropertyCacheGet(() => new FehlteilEtikettSelektor()); }    //    { VIN = "DE87654323" }); }
            set { PropertyCacheSet(value); }
        }

        public FehlteilEtikett FehlteilEtikett
        {
            get { return PropertyCacheGet(() => new FehlteilEtikett { LayoutPosition = 1 }); }
            set { PropertyCacheSet(value); }
        }

        public void DataInit()
        {
            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.UploadItemsFiltered);
            PropertyCacheClear(this, m => m.FehlteilEtikett);
        }


        #region CSV Upload

        public bool CsvUploadFileSave(string fileName, Func<string, bool> fileSaveAction)
        {
            CsvUploadFileName = fileName;
            CsvUploadServerFileName = Path.Combine(AppSettings.TempPath, Guid.NewGuid() + ".csv");

            if (!fileSaveAction(CsvUploadServerFileName))
                return false;

            var list = new ExcelDocumentFactory().ReadToDataTable(CsvUploadServerFileName, CreateFromDataRowWithHeaderAndContentInSeparateColumns<FehlteilEtikett>).ToList();
            var id = 1;
            list.ForEach(item => item.ID = id++);
            FileService.TryFileDelete(CsvUploadServerFileName);
            if (list.None())
                return false;

            UploadItems = list;
            ValidateUploadItems();

            return true;
        }

        static T CreateFromDataRowWithHeaderAndContentInSeparateColumns<T>(DataRow row)
            where T : class, new()
        {
            var item = new T();

            var dt = row.Table;
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            for (var columnIndex = 0; columnIndex < dt.Columns.Count; columnIndex++)
            {
                if (columnIndex == 0)
                {
                    var vinProperty = properties.FirstOrDefault(p => p.Name == "VIN");
                    if (vinProperty != null)
                        vinProperty.SetValue(item, row[0], null);
                    continue;
                }

                // assume the first row is the header row:
                var firstRow = dt.Rows[0];
                var columnHeader = firstRow[columnIndex].ToString();

                var propertyIndex = columnIndex;
                var headerProperty = properties.FirstOrDefault(p => p.Name == string.Format("Header{0}", propertyIndex));
                var contentProperty = properties.FirstOrDefault(p => p.Name == string.Format("Content{0}", propertyIndex));
                if (headerProperty == null || contentProperty == null)
                    continue;

                headerProperty.SetValue(item, columnHeader, null);

                var value = row[columnIndex];
                value = ModelMapping.TryConvertValue(contentProperty, value, "us");
                if (value == DBNull.Value)
                    value = null;
                contentProperty.SetValue(item, value, null);
            }

            return item;
        }

        public string GetHeaderText(int index)
        {
            var item = UploadItems.FirstOrDefault();
            if (item == null)
                return "-";

            var properties = typeof(FehlteilEtikett).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var headerProperty = properties.FirstOrDefault(p => p.Name == string.Format("Header{0}", index));
            if (headerProperty == null)
                return "-";

            return (string)headerProperty.GetValue(item, null);
        }

        public bool HeaderTextAvailable(int index)
        {
            return GetHeaderText(index).IsNotNullOrEmpty();
        }

        public void ValidateUploadItems()
        {
            DataService.ValidateUploadItems(UploadItems);   
        }

        public void SaveUploadItems()
        {
            UploadItemsStoredErrorMessage = DataService.InsertItems(UploadItems);
        }

        #endregion


        #region Edit + Print

        public void LoadFehlteilEtikett()
        {
            var savedLayoutPos = FehlteilEtikett.LayoutPosition;
            FehlteilEtikett = DataService.LoadItem(FehlteilEtikettSelektor.VIN);
            FehlteilEtikett.LayoutPosition = savedLayoutPos;
        }

        public FehlteilEtikett SaveItem(FehlteilEtikett item, Action<string, string> addModelError)
        {
            var savedItem = DataService.SaveItem(item, addModelError);
            DataMarkForRefresh();

            FehlteilEtikett = savedItem;

            return savedItem;
        }

        public void ValidateFehlteilEtikett(FehlteilEtikett model, Action<Expression<Func<FehlteilEtikett, object>>, string> addModelError)
        {
        }

        public void GetEtikettAsPdf(string vin, out string errorMessage, out byte[] pdfBytes)
        {
            DataService.GetEtikettAsPdf(FehlteilEtikett, out errorMessage, out pdfBytes);
        }
        
        #endregion
    }
}
