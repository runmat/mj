using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Leasing.Contracts;
using CkgDomainLogic.Leasing.Models;
using CkgDomainLogic.Leasing.Models.UIModels;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Leasing.Models.AppModelMappings;

namespace CkgDomainLogic.Leasing.Services
{
    public class LeasingEndgueltigerVersandDataServiceSAP : CkgGeneralDataServiceSAP, ILeasingEndgueltigerVersandDataService
    {

        public LeasingEndgueltigerVersandDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public List<EndgueltigerVersandModel> GetTempVersandInfos(EndgueltigerVersandSuchParameter selector)
        {
            Z_DPM_TEMP_VERSENDUNGEN_01.Init(SAP);

            Z_DPM_TEMP_VERSENDUNGEN_01.SetImportParameter_I_AG(SAP, LogonContext.KundenNr.ToSapKunnr());


            if (selector.Fahrgestellnummer.IsNotNullOrEmpty())
                Z_DPM_TEMP_VERSENDUNGEN_01.SetImportParameter_I_CHASSIS_NUM(SAP, selector.Fahrgestellnummer);

            if (selector.Kennzeichen.IsNotNullOrEmpty())
                Z_DPM_TEMP_VERSENDUNGEN_01.SetImportParameter_I_LICENSE_NUM(SAP, selector.Kennzeichen);

            if (selector.Vertragsnummer.IsNotNullOrEmpty())
                Z_DPM_TEMP_VERSENDUNGEN_01.SetImportParameter_I_LIZNR(SAP, selector.Vertragsnummer);

            if (selector.Zeitraum.StartDate != null)
                Z_DPM_TEMP_VERSENDUNGEN_01.SetImportParameter_I_ZZTMPDT_VON(SAP, selector.Zeitraum.StartDate);

            if (selector.Zeitraum.EndDate != null)
                Z_DPM_TEMP_VERSENDUNGEN_01.SetImportParameter_I_ZZTMPDT_VON(SAP, selector.Zeitraum.EndDate);

            SAP.Execute();

            var sapItemsEquis = Z_DPM_TEMP_VERSENDUNGEN_01.GT_OUT.GetExportList(SAP);
            var webItemsEquis = AppModelMappings.Z_DPM_TEMP_VERSENDUNGEN_01_GT_OUT_To_EndgueltigerVersandModel.Copy(sapItemsEquis).ToList();

            return webItemsEquis;

        }

        public string Save(IEnumerable<EndgueltigerVersandModel> endgueltigeVersandInfos)
        {
            var erg = "";
            Z_DPM_TEMP_END_SPERR_01.Init(SAP);

            Z_DPM_TEMP_END_SPERR_01.SetImportParameter_I_AG(SAP, LogonContext.KundenNr.ToSapKunnr());

            var infos =
                AppModelMappings.Z_DPM_TEMP_END_SPERR_01_GT_FZG_To_EndgueltigerVersandModel.CopyBack(endgueltigeVersandInfos);

            SAP.ApplyImport(infos);

            SAP.Execute();

            var result = Z_DPM_TEMP_END_SPERR_01.GT_FZG.GetExportList(SAP);


            var ergList = Z_DPM_TEMP_END_SPERR_01.GT_FZG.GetExportList(SAP);
            if ((ergList.Count > 0) && (!String.IsNullOrEmpty(ergList[0].BEM)))
            {
                erg = ergList[0].BEM;
            }

            return erg;
        }
    }
}
