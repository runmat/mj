using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Fahrzeuge.Models.AppModelMappings;

namespace CkgDomainLogic.Fahrzeuge.Services
{
    public class FahrzeuguebersichtDataServiceSAP : CkgGeneralDataServiceSAP, IFahrzeuguebersichtDataService
    {
        public FahrzeuguebersichtDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }
              
        public List<Fahrzeuguebersicht> GetFahrzeuguebersicht(FahrzeuguebersichtSelektor selector, List<Fahrzeuguebersicht> uploadItems = null)
        {
            Z_DPM_LIST_POOLS_001.Init(SAP, "I_KUNNR_AG", LogonContext.KundenNr.ToSapKunnr());

            if (uploadItems != null)
            {
                var selItems = AppModelMappings.Z_DPM_LIST_POOLS_001_GT_WEB_From_Fahrzeuguebersicht.CopyBack(uploadItems);
                SAP.ApplyImport(selItems);
            }
            else
            {
                if (!String.IsNullOrEmpty(selector.Herstellerkennung))
                    SAP.SetImportParameter("I_HERST_T", selector.Herstellerkennung);

                if (selector.EingangZb2DatumRange.IsSelected)
                {
                    SAP.SetImportParameter("I_ERDAT_EQUI_VON", selector.EingangZb2DatumRange.StartDate);
                    SAP.SetImportParameter("I_ERDAT_EQUI_BIS", selector.EingangZb2DatumRange.EndDate);
                }

                if (selector.EingangFahrzeugDatumRange.IsSelected)
                {
                    SAP.SetImportParameter("I_ZZDAT_EIN_VON", selector.EingangFahrzeugDatumRange.StartDate);
                    SAP.SetImportParameter("I_ZZDAT_EIN_BIS", selector.EingangFahrzeugDatumRange.EndDate);
                }

                if (selector.BereitmeldungDatumRange.IsSelected)
                {
                    SAP.SetImportParameter("I_ZZDAT_BER_VON", selector.BereitmeldungDatumRange.StartDate);
                    SAP.SetImportParameter("I_ZZDAT_BER_BIS", selector.BereitmeldungDatumRange.EndDate);
                }

                if (selector.ZulassungDatumRange.IsSelected)
                {
                    SAP.SetImportParameter("I_REPLA_DATE_VON", selector.ZulassungDatumRange.StartDate);
                    SAP.SetImportParameter("I_REPLA_DATE_BIS", selector.ZulassungDatumRange.EndDate);
                }

                int i;
                if (Int32.TryParse(selector.Statuskennung.NotNullOrEmpty(), out i) && i <= 700)
                    SAP.SetImportParameter("I_SELECT", "X");

                var selItem = AppModelMappings.Z_DPM_LIST_POOLS_001_GT_WEB_From_FahrzeuguebersichtSelektor.CopyBack(selector);
                SAP.ApplyImport(new List<Z_DPM_LIST_POOLS_001.GT_WEB> { selItem });
            }
            
            SAP.Execute();

            return AppModelMappings.Z_DPM_LIST_POOLS_001_GT_WEB_ToFahrzeuguebersicht.Copy(Z_DPM_LIST_POOLS_001.GT_WEB.GetExportList(SAP)).ToList();
        }
        
        public List<FahrzeuguebersichtPDI> GetPDIStandorte()
        {            
            Z_DPM_LIST_PDI_001.Init(SAP, "I_KUNNR_AG", LogonContext.KundenNr.ToSapKunnr());

            SAP.Execute();

            var sapItemsEquis = Z_DPM_LIST_PDI_001.GT_WEB.GetExportList(SAP);
            var webItemsEquis = AppModelMappings.Z_DPM_LIST_PDI_001_GT_WEB_ToFahrzeuguebersichtPDI.Copy(sapItemsEquis).ToList();

            return webItemsEquis;                      
        }

        public List<FahrzeuguebersichtStatus> GetFahrzeugStatus()
        {                        
            Z_DPM_READ_AUFTR_006.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());
            SAP.SetImportParameter("I_KENNUNG", "STATUS");

            SAP.Execute();

            var sapItemsEquis = Z_DPM_READ_AUFTR_006.GT_OUT.GetExportList(SAP);
            var webItemsEquis = AppModelMappings.Z_DPM_READ_AUFTR_006_GT_OUT_ToFahrzeuguebersichtStatus.Copy(sapItemsEquis).ToList();

            return webItemsEquis;          
        }
    }
}
