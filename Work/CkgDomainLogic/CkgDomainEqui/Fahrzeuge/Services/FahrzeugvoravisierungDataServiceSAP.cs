using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Fahrzeuge.Models.AppModelMappings;

namespace CkgDomainLogic.Fahrzeuge.Services
{
    public class FahrzeugvoravisierungDataServiceSAP : CkgGeneralDataServiceSAP, IFahrzeugvoravisierungDataService
    {
        public FahrzeugvoravisierungDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        // public List<FahrzeugvoravisierungUploadModel> UploadItems { get; set; }
               
        void ValidateSingleUploadItem(FahrzeugvoravisierungUploadModel item)
        {    /* TODO: Remove if not needed */     }

        public string SaveUploadItems(List<FahrzeugvoravisierungUploadModel> uploadItems)
        {
            Z_DPM_IMP_MODELL_ID_01.Init(SAP, "I_AG", LogonContext.KundenNr.ToSapKunnr());
            SAP.SetImportParameter("I_WEB_USER", LogonContext.UserName);
            string email = ((ILogonContextDataService)LogonContext).GetEmailAddressForUser();
            SAP.SetImportParameter("I_WEB_MAIL", email);

            var vgList = AppModelMappings.Z_DPM_IMP_MODELL_ID_01_GT_IN_From_FahrzeugvoravisierungUploadModel.CopyBack(uploadItems).ToList();
            SAP.ApplyImport(vgList);

            SAP.Execute();
           
            if (SAP.ResultCode != 0)
                return SAP.ResultMessage;

            var outList = Z_DPM_IMP_MODELL_ID_01.GT_OUT.GetExportList(SAP);

            foreach (var item in uploadItems)            
                item.Status = outList.Find(o => o.CHASSIS_NUM.ToUpper() == item.Fahrgestellnummer.ToUpper()).BEM;
            
            return null;
        }
    }
}
