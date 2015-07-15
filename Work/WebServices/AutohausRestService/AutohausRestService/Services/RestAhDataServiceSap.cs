using System.Collections.Generic;
using System.Linq;
using AutohausRestService.Contracts;
using AutohausRestService.Models;
using CkgDomainLogic.General.Database.Models;
using SapORM.Contracts;
using SapORM.Models;

namespace AutohausRestService.Services
{
    public class RestAhDataServiceSap : IRestAhDataService
    {
        public string SaveDatensatz(User user, Customer kunde, Datensatz daten, out List<Partner> partnerList, out List<Fahrzeug> fzgList)
        {
            partnerList = null;
            fzgList = null;
            var erg = "";
            var objS = new S();

            Z_AHP_CRE_CHG_PARTNER_FZGDATEN.Init(objS.AP);

            objS.AP.SetImportParameter("I_KUNNR", kunde.KUNNR.ToSapKunnr());
            objS.AP.SetImportParameter("I_USER", user.Username);

            if (daten.Partnerdaten != null)
            {
                var pImportList = AppModelMappings.Z_AHP_CRE_CHG_PARTNER_FZGDATEN_GT_WEB_PARTNER_IMP_From_Partner.CopyBack(new List<Partner> { daten.Partnerdaten }).ToList();
                objS.AP.ApplyImport(pImportList);
            }

            if (daten.Fahrzeuge.Count > 0)
            {
                var fzgImportList = AppModelMappings.Z_AHP_CRE_CHG_PARTNER_FZGDATEN_GT_WEB_FZG_IMP_From_Fahrzeug.CopyBack(daten.Fahrzeuge).ToList();
                objS.AP.ApplyImport(fzgImportList);
            }

            objS.AP.Execute();

            if (objS.AP.ResultCode == 0)
            {
                var pExportList = Z_AHP_CRE_CHG_PARTNER_FZGDATEN.GT_WEB_PARTNER_OUT.GetExportList(objS.AP);
                partnerList = AppModelMappings.Z_AHP_CRE_CHG_PARTNER_FZGDATEN_GT_WEB_PARTNER_OUT_To_Partner.Copy(pExportList).ToList();

                var fzgExportList = Z_AHP_CRE_CHG_PARTNER_FZGDATEN.GT_WEB_FZG_OUT.GetExportList(objS.AP);
                fzgList = AppModelMappings.Z_AHP_CRE_CHG_PARTNER_FZGDATEN_GT_WEB_FZG_OUT_To_Fahrzeug.Copy(fzgExportList).ToList();
            }
            else
            {
                erg = objS.AP.ResultMessage;
            }

            return erg;
        }
    }
}