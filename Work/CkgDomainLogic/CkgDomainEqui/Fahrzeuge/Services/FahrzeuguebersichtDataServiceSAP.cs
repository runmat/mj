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
    public class FahrzeuguebersichtDataServiceSAP : CkgGeneralDataServiceSAP, IFahrzeuguebersichtDataService
    {
        public FahrzeuguebersichtDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }
              
        public List<Fahrzeuguebersicht> GetFahrzeuguebersicht(FahrzeuguebersichtSelektor selector)
        {
            Z_DPM_LIST_POOLS_001.Init(SAP, "I_KUNNR_AG", LogonContext.KundenNr.ToSapKunnr());
                                                      
            SAP.Execute();

            var sapItemsEquis = Z_DPM_LIST_POOLS_001.GT_WEB.GetExportList(SAP);
            var webItemsEquis = AppModelMappings.Z_DPM_LIST_POOLS_001_GT_WEB_ToFahrzeuguebersicht.Copy(sapItemsEquis).ToList();

            return webItemsEquis;            
        }
        
        public List<FahrzeuguebersichtPDI> GetPDIStandorte()
        {            
            Z_DPM_LIST_PDI_001.Init(SAP, "I_KUNNR_AG", LogonContext.KundenNr.ToSapKunnr());

            var sapItemsEquis = Z_DPM_LIST_PDI_001.GT_WEB.GetExportList(SAP);
            var webItemsEquis = AppModelMappings.Z_DPM_LIST_PDI_001_GT_WEB_ToFahrzeuguebersichtPDI.Copy(sapItemsEquis).ToList();

            return webItemsEquis;                      
        }

        public List<FahrzeuguebersichtStatus> GetFahrzeugStatus()
        {                        
            Z_DPM_READ_AUFTR_006.Init(SAP, "I_KUNNR_AG", LogonContext.KundenNr.ToSapKunnr());
            SAP.SetImportParameter("I_KENNUNG", "STATUS");

            var sapItemsEquis = Z_DPM_READ_AUFTR_006.GT_OUT.GetExportList(SAP);
            var webItemsEquis = AppModelMappings.Z_DPM_READ_AUFTR_006_GT_OUT_ToFahrzeuguebersichtStatus.Copy(sapItemsEquis).ToList();

            return webItemsEquis;          

        }
    }
}
