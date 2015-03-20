// ReSharper disable RedundantUsingDirective
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.WFL.Contracts;
using CkgDomainLogic.WFL.Models;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.WFL.Models.AppModelMappings;
// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.WFL.Services
{
    public class WflDataServiceSAP : CkgGeneralDataServiceSAP, IWflDataService
    {
        public WflDataServiceSAP(ISapDataService sap)
            :base(sap)
        {
        }

        public List<WflAbmeldung> GetAbmeldungen(WflAbmeldungSelektor selector)
        {
            Z_DPM_CD_Strafzettel.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());

            if (selector.VertragsNr.IsNotNullOrEmpty())
                SAP.SetImportParameter("I_VERTRAGS_NR", selector.VertragsNr);

            if (selector.EingangsDatumRange.IsSelected)
            {
                SAP.SetImportParameter("I_EINGDAT_VON", selector.EingangsDatumRange.StartDate);
                SAP.SetImportParameter("I_EINGDAT_BIS", selector.EingangsDatumRange.EndDate);
            }

            SAP.Execute();

            var sapItemsEquis = Z_DPM_CD_Strafzettel.GT_OUT.GetExportList(SAP);
            //var webItemsEquis = AppModelMappings.Z_DPM_CD_ABM_LIST__ET_ABM_LIST_To_Strafzettel.Copy(sapItemsEquis).ToList();

            return null; // webItemsEquis;
        }
    }
}
