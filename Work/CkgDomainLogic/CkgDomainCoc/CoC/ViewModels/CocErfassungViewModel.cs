using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Serialization;
using CkgDomainLogic.CoC.Contracts;
using CkgDomainLogic.CoC.Models;
using CkgDomainLogic.CoC.Models.UiModels;
using CkgDomainLogic.General.ViewModels;
using DocumentTools.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;


namespace CkgDomainLogic.CoC.ViewModels
{
    public class CocErfassungViewModel : CkgBaseViewModel, ICocEntityViewModel
    {
        [XmlIgnore]
        public ICocErfassungDataService DataService { get { return CacheGet<ICocErfassungDataService>(); } }


        [LocalizedDisplay(LocalizeConstants.DataWithErrorsOccurred)]
        public bool UploadItemsErrorsOccurred { get { return UploadItems.Any(item => item.ValidationErrors.IsNotNullOrEmpty()); } }

        [LocalizedDisplay(LocalizeConstants.ListItemsWithErrorsOnly)]
        public bool UploadItemsShowErrorsOnly { get; set; }

        public List<CsvUploadEntityDpmCoc> UploadItemsFiltered 
        { 
            get  { return !UploadItemsShowErrorsOnly ? UploadItems : UploadItems.Where(item => item.ValidationErrors.IsNotNullOrEmpty()).ToList(); } 
        }

        public List<CsvUploadEntityDpmCoc> UploadItems { get; private set; }

        public string CsvUploadFileName { get; private set; }
        public string CsvUploadServerFileName { get; private set; }

        public bool UploadItemsSuccessfullyStored { get; set; }

        public bool InsertMode { get; set; }

        public CocEntity CocOrder { get; set; }

        public CocEntity CocTmpOrder { get; set; }

        public CocOrderParameter CocOrderParameter { get; set; }


        #region Repository

        public CocEntity CreateNewCocOrder(CocEntity basedCocTyp, string typ, string variante, string version)
        {
            InsertMode = true;

            CocOrder = new CocEntity
            {
                COC_0_2_TYP = typ,
                COC_0_2_VAR = variante,
                COC_0_2_VERS = version,
            }; 
            
            if (basedCocTyp != null)
                CocOrder = ModelMapping.Copy(basedCocTyp);

            CocOrder.DuplicateVinOccured = false;
            CocOrder.DuplicateVinIgnoreOnSaving = false;

            CocOrderParameter = (CocOrderParameter)LogonContext.DataContextRestore(typeof(CocOrderParameter).GetFullTypeName()) ?? new CocOrderParameter();
            CocOrder.CocOrderHideCocTypenProperties = CocOrderParameter.HideCocTypenProperties;

            return CocOrder.SetOrderStatus(true).SetInsertMode(InsertMode);
        }

        public void AddItem(CocEntity newItem)
        {
            // yet no stored list here in this viewmodel ...
        }

        public CocEntity SaveItem(CocEntity item, Action<string, string> addModelError)
        {
            CocOrderParameter.HideCocTypenProperties = item.CocOrderHideCocTypenProperties;
            LogonContext.DataContextPersist(CocOrderParameter);

            return CocOrder = DataService.SaveCocOrder(item, addModelError);
        }

        public void ValidateModel(CocEntity model, bool insertMode, Action<Expression<Func<CocEntity, object>>, string> addModelError)
        {
            if (model.VIN.IsNullOrEmpty())
                addModelError(m => m.VIN, "Bitte eine gültige VIN angeben.");
            else
            {
                var existingItemsOfThisKey = DataService.CocAuftraege.Where(t => t.VIN.NotNullOrEmpty().ToUpper() == model.VIN.NotNullOrEmpty().ToUpper());
                var primaryKeyViolation = (insertMode && existingItemsOfThisKey.Any() ||
                                           !insertMode && existingItemsOfThisKey.Any(t => t.ID != model.ID));
                var existingItem = existingItemsOfThisKey.FirstOrDefault();

                if (!primaryKeyViolation || existingItem == null)
                    return;

                model.AUFTRAG_DAT = existingItem.AUFTRAG_DAT;
                if (model.DuplicateVinIgnoreOnSaving)
                {
                    if (existingItem.AUFTRAG_DAT == null)
                        // 1. wenn noch kein Auftragsdatum vorhanden ist soll der Datensatz überschrieben werden.
                        // 2. Bei gesetztem Auftragsdatum soll eine Dublette angelegt werden 
                        //    (in diesem Fall bleibt die VORG_NR leer weil nur durch eine leere VORG_NR ein Insert veranlasst wird.)
                        model.VORG_NR = existingItem.VORG_NR;

                    return;
                }

                model.DuplicateVinOccured = true;
                CocOrder = model;

                addModelError(m => m.VIN, "Diese VIN existiert bereits in einem anderen Auftrag.");
            }
        }

        public void DataMarkForRefresh()
        {
            DataService.MarkForRefreshCocOrders();
        }

        #endregion


        #region CSV Upload

        public bool CsvUploadFileSave(string fileName, Func<string, bool> fileSaveAction)
        {
            CsvUploadFileName = fileName;
            CsvUploadServerFileName = Path.Combine(AppSettings.TempPath, Guid.NewGuid() + ".csv");

            if (!fileSaveAction(CsvUploadServerFileName))
                return false;

            var list = new ExcelDocumentFactory().ReadToDataTable<CsvUploadEntityDpmCoc>(CsvUploadServerFileName, null, ',').ToList();
            FileService.TryFileDelete(CsvUploadServerFileName);
            if (list.None())
                return false;

            UploadItems = list;
            ValidateUploadItems();

            return true;
        }

        void ValidateUploadItems()
        {
            DataService.ValidateUploadCocOrders(UploadItems);   
        }

        public void SaveUploadItems()
        {
            UploadItemsSuccessfullyStored = DataService.SaveUploadCocOrdersWithProofReading(UploadItems);
        }

        #endregion
    }
}
