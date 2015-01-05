using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.UploadFahrzeugdaten.Contracts;
using CkgDomainLogic.UploadFahrzeugdaten.Models;
using SapORM.Contracts;
using SapORM.Models;

namespace CkgDomainLogic.UploadFahrzeugdaten.Services
{
    public class UploadFahrzeugdatenDataServiceSap : CkgGeneralDataServiceSAP, IUploadFahrzeugdatenDataService
    {
        public UploadFahrzeugdatenDataServiceSap(ISapDataService sap)
            : base(sap)
        {
        }

        public List<UploadFahrzeug> UploadItems { get; set; }

        public void ValidateFahrzeugdatenCsvUpload()
        {
            UploadItems.ForEach(ValidateSingleUploadItem);
        }

        public string SaveFahrzeugdatenCsvUpload()
        {
            Z_AHP_CRE_CHG_FZG_AKT_BEST.Init(SAP, "I_KUNNR, I_USER", LogonContext.KundenNr.ToSapKunnr(), LogonContext.UserName);

            var fzgList = AppModelMappings.Z_AHP_CRE_CHG_FZG_AKT_BEST_GT_WEB_IMP_From_UploadFahrzeug.CopyBack(UploadItems).ToList();
            SAP.ApplyImport(fzgList);

            SAP.Execute();

            if (SAP.ResultCode != 0)
                return SAP.ResultMessage;

            var errList = Z_AHP_CRE_CHG_FZG_AKT_BEST.GT_OUT_ERR.GetExportList(SAP);

            foreach (var item in UploadItems)
            {
                if (errList != null && errList.Any(e => String.Compare(e.FIN, item.FahrgestellNr, true) == 0))
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

        void ValidateSingleUploadItem(UploadFahrzeug item)
        {
            var liste = new List<ValidationResult>();

            item.IsValid = Validator.TryValidateObject(item, new ValidationContext(item, null, null), liste, true);

            var ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            item.ValidationErrorsJson = ser.Serialize(liste);
        }
    }
}
