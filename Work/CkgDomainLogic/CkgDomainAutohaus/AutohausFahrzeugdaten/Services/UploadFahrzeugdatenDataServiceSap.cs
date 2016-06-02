using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.AutohausFahrzeugdaten.Contracts;
using CkgDomainLogic.AutohausFahrzeugdaten.Models;
using SapORM.Contracts;
using SapORM.Models;

namespace CkgDomainLogic.AutohausFahrzeugdaten.Services
{
    public class UploadFahrzeugdatenDataServiceSap : CkgGeneralDataServiceSAP, IUploadFahrzeugdatenDataService
    {
        public UploadFahrzeugdatenDataServiceSap(ISapDataService sap)
            : base(sap)
        {
        }

        public List<UploadFahrzeug> UploadItems { get; set; }

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
    }
}
