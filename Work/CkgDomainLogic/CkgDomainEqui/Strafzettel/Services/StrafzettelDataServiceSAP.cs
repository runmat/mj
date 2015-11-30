// ReSharper disable RedundantUsingDirective
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Strafzettel.Contracts;
using CkgDomainLogic.Strafzettel.Models;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Strafzettel.Models.AppModelMappings;
// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.Strafzettel.Services
{
    public class StrafzettelDataServiceSAP : CkgGeneralDataServiceSAP, IStrafzettelDataService
    {
        public StrafzettelDataServiceSAP(ISapDataService sap)
            :base(sap)
        {
        }

        public List<StrafzettelModel> GetStrafzettel(StrafzettelSelektor selector)
        {
            Z_DPM_CD_STRAFZETTEL.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());

            if (selector.Fin.IsNotNullOrEmpty())
                SAP.SetImportParameter("I_FIN17", selector.Fin);
            if (selector.Fin10.IsNotNullOrEmpty())
                SAP.SetImportParameter("I_FIN10", selector.Fin10);

            if (selector.VertragsNr.IsNotNullOrEmpty())
                SAP.SetImportParameter("I_VERTRAGS_NR", selector.VertragsNr);

            if (selector.Kennzeichen.IsNotNullOrEmpty())
                SAP.SetImportParameter("I_KENNZ", selector.Kennzeichen);

            if (selector.EingangsDatumRange.IsSelected)
            {
                SAP.SetImportParameter("I_EINGDAT_VON", selector.EingangsDatumRange.StartDate);
                SAP.SetImportParameter("I_EINGDAT_BIS", selector.EingangsDatumRange.EndDate);
            }

            if (selector.BehoerdeDatumRange.IsSelected)
            {
                SAP.SetImportParameter("I_DATBEHO_VON", selector.BehoerdeDatumRange.StartDate);
                SAP.SetImportParameter("I_DATBEHO_BIS", selector.BehoerdeDatumRange.EndDate);
            }

            if (selector.BehoerdeName.IsNotNullOrEmpty())
                SAP.SetImportParameter("I_NAME1_AMT", selector.BehoerdeName);
            if (selector.BehoerdePlz.IsNotNullOrEmpty())
                SAP.SetImportParameter("I_PLZCODE", selector.BehoerdePlz);

            SAP.Execute();

            var sapItemsEquis = Z_DPM_CD_STRAFZETTEL.GT_OUT.GetExportList(SAP);
            var webItemsEquis = AppModelMappings.Z_DPM_CD_ABM_LIST__ET_ABM_LIST_To_Strafzettel.Copy(sapItemsEquis).ToList();

            return webItemsEquis;
        }
    }
}
