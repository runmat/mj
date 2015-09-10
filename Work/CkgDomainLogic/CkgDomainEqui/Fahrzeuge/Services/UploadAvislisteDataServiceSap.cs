using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using SapORM.Contracts;
using SapORM.Models;

namespace CkgDomainLogic.Fahrzeuge.Services
{
    public class UploadAvislisteDataServiceSap : CkgGeneralDataServiceSAP, IUploadAvislisteDataService
    {
        public UploadAvislisteDataServiceSap(ISapDataService sap)
            : base(sap)
        {
        }

        public List<UploadAvisdaten> UploadItems { get; set; }

        public void ValidateAvislisteCsvUpload()
        {
            UploadItems.ForEach(ValidateSingleUploadItem);
        }

        public string SaveAvislisteCsvUpload()
        {
            Z_DPM_IMP_MEL_CARP_01.Init(SAP, "I_AG, I_WEB_USER", LogonContext.KundenNr.ToSapKunnr(), LogonContext.UserName);

            var avisList = AppModelMappings.Z_DPM_IMP_MEL_CARP_01_GT_TAB_From_UploadAvisdaten.CopyBack(UploadItems).ToList();
            SAP.ApplyImport(avisList);

            SAP.Execute();

            if (SAP.ResultCode != 0)
                return SAP.ResultMessage;

            var errList = Z_DPM_IMP_MEL_CARP_01.GT_TAB.GetExportList(SAP);

            foreach (var item in UploadItems)
            {
                var errItem = errList.FirstOrDefault(e => String.Compare(e.CHASSIS_NUM, item.FahrgestellNr, true) == 0);

                if (errItem != null && !String.IsNullOrEmpty(errItem.BEM))
                {
                    item.SaveStatus = errItem.BEM;
                }
                else
                {
                    item.SaveStatus = "OK";
                } 
            }

            return (UploadItems.Any(u => u.SaveStatus != "OK") ? Localize.ErrorsOccuredOnSaving : "");
        }

        private void ValidateSingleUploadItem(UploadAvisdaten item)
        {
            var liste = new List<ValidationResult>();

            item.ValidationOk = Validator.TryValidateObject(item, new ValidationContext(item, null, null), liste, true);

            var ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            item.ValidationErrorsJson = ser.Serialize(liste);
        }
    }
}
