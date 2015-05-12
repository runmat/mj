using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Fahrzeuge.Models.AppModelMappings;

namespace CkgDomainLogic.Fahrzeuge.Services
{
    public class FahrzeugSperrenVerschiebenDataServiceSAP : CkgGeneralDataServiceSAP, IFahrzeugSperrenVerschiebenDataService
    {
        public FahrzeugSperrenVerschiebenDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public List<Domaenenfestwert> GetFarben()
        {
            var sapList = Z_DPM_DOMAENENFESTWERTE.GT_WEB.GetExportListWithInitExecute(SAP, "DOMNAME, DDLANGUAGE", "ZFARBE", "DE");

            return DomainCommon.Models.AppModelMappings.Z_DPM_DOMAENENFESTWERTE_GT_WEB_To_Domaenenfestwert.Copy(sapList).ToList();
        }

        public List<Fahrzeuguebersicht> GetFahrzeuge()
        {
            Z_DPM_LIST_POOLS_001.Init(SAP, "I_KUNNR_AG, I_SELECT", LogonContext.KundenNr.ToSapKunnr(), "X");
                                      
            SAP.Execute();

            return AppModelMappings.Z_DPM_LIST_POOLS_001_GT_WEB_ToFahrzeuguebersicht.Copy(Z_DPM_LIST_POOLS_001.GT_WEB.GetExportList(SAP)).ToList();            
        }
    }
}
