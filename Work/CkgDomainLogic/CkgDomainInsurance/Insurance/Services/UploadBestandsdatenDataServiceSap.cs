using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Insurance.Contracts;
using CkgDomainLogic.Insurance.Models;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;

namespace CkgDomainLogic.Insurance.Services
{
    public class UploadBestandsdatenDataServiceSap : CkgGeneralDataServiceSAP, IUploadBestandsdatenDataService
    {
        public UploadBestandsdatenDataServiceSap(ISapDataService sap)
            : base(sap)
        {
        }

        public List<UploadBestandsdatenModel> UploadItems { get; set; }

        public void ValidateBestandsdatenCsvUpload()
        {
            UploadItems.ForEach(ValidateSingleUploadItem);
        }

        public string SaveBestandsdatenCsvUpload()
        {
            Z_DPM_ASSIST_IMP_BESTAND_01.Init(SAP);

            SAP.SetImportParameter("I_KUNNR_AH", LogonContext.KundenNr.ToSapKunnr());
            SAP.SetImportParameter("I_KUNNR_FIL", (LogonContext as ILogonContextDataService).User.Reference.NotNullOrEmpty().ToSapKunnr());
            SAP.SetImportParameter("I_VKORG", (LogonContext as ILogonContextDataService).Customer.AccountingArea);

            var vgList = AppModelMappings.Z_DPM_ASSIST_IMP_BESTAND_01_GT_IN_From_UploadBestandsdatenModel.CopyBack(UploadItems).ToList();
            SAP.ApplyImport(vgList);

            SAP.Execute();

            if (SAP.ResultCode != 0)
                return SAP.ResultMessage;

            var errList = Z_DPM_ASSIST_IMP_BESTAND_01.GT_ERR.GetExportList(SAP);

            foreach (var item in UploadItems)
            {
                if (errList != null && errList.Any(e => Int32.Parse(e.LFDNR) == item.DatensatzNr))
                {
                    item.SaveStatus = Localize.SaveFailed;
                }
                else
                {
                    item.SaveStatus = Localize.OK;
                } 
            }

            return "";
        }

        void ValidateSingleUploadItem(UploadBestandsdatenModel item)
        {
            var liste = new List<ValidationResult>();

            item.IsValid = Validator.TryValidateObject(item, new ValidationContext(item, null, null), liste, true);

            var ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            item.ValidationErrorsJson = ser.Serialize(liste);
        }
    }
}
