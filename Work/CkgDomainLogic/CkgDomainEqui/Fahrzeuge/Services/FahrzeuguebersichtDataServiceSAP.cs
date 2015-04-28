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
using GeneralTools.Models;
using AppModelMappings = CkgDomainLogic.Fahrzeuge.Models.AppModelMappings;

namespace CkgDomainLogic.Fahrzeuge.Services
{
    public class FahrzeuguebersichtDataServiceSAP : CkgGeneralDataServiceSAP, IFahrzeuguebersichtDataService
    {
        public FahrzeuguebersichtDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }
              
        public List<Fahrzeuguebersicht> GetFahrzeuguebersicht(FahrzeuguebersichtSelektor selector)
        {
            Z_DPM_LIST_POOLS_001.Init(SAP, "I_KUNNR_AG", LogonContext.KundenNr.ToSapKunnr());

            // TODO -> Der Filter scheint nachgeschaltet zu sein!!
            // Combo-Auswahl dito -> fragen

            //if (selector.Fahrgestellnummer.IsNotNullOrEmpty())
            //    SAP.SetImportParameter("CHASSIS_NUM", selector.Fahrgestellnummer);

            //if (selector.Kennzeichen.IsNotNullOrEmpty())
            //    SAP.SetImportParameter("LICENSE_NUM", selector.Kennzeichen);

            //if (selector.Unitnummer.IsNotNullOrEmpty())
            //    SAP.SetImportParameter("ZZREFERENZ1", selector.Unitnummer);

            //if (selector.Auftragsnummer.IsNotNullOrEmpty())
            //    SAP.SetImportParameter("LIZNR", selector.Auftragsnummer);

            //if (selector.BatchId.IsNotNullOrEmpty())
            //    SAP.SetImportParameter("ZUNIT_NR_BIS", selector.BatchId);

            //if (selector.SIPPCode.IsNotNullOrEmpty())
            //    SAP.SetImportParameter("ZZSIPP", selector.SIPPCode);

            //if (selector.ModelID.IsNotNullOrEmpty())
            //    SAP.SetImportParameter("ZZMODELL", selector.ModelID);

            //if (selector.Zb2Nummer.IsNotNullOrEmpty())
            //    SAP.SetImportParameter("TIDNR", selector.Zb2Nummer);

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
                              
            SAP.Execute();

            var sapItemsEquis = Z_DPM_LIST_POOLS_001.GT_WEB.GetExportList(SAP);
            var webItemsEquis = AppModelMappings.Z_DPM_LIST_POOLS_001_GT_WEB_ToFahrzeuguebersicht.Copy(sapItemsEquis).ToList();

            return webItemsEquis;            
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
