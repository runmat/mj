using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Finance.Contracts;
using CkgDomainLogic.Finance.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Finance.Models.AppModelMappings;

namespace CkgDomainLogic.Finance.Services
{
    public class FinanceCarporteingaengeOhneEHDataServiceSAP : CkgGeneralDataServiceSAP, IFinanceCarporteingaengeOhneEHDataService
    {
         public List<CarporteingaengeOhneEH> CarporteingaengeOhneEHs { get { return PropertyCacheGet(() => LoadCarporteingaengeOhneEHFromSap()); } }

         public FinanceCarporteingaengeOhneEHDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public void DeleteCarporteingaengeOhneEHToSap(CarporteingaengeOhneEH saveItem)
        {
            Z_M_ABMBEREIT_LAUFAEN.Init(SAP, "KUNNR", LogonContext.KundenNr.ToSapKunnr());

            SAP.SetImportParameter("ZZKENN", saveItem.Kennzeichen);
            SAP.SetImportParameter("ZZFAHRG", saveItem.Fahrgestellnummer);
            SAP.SetImportParameter("KUNPDI", saveItem.BelegNr);

            SAP.Execute();  
        }

        public void MarkForRefresh()
        {
            PropertyCacheClear(this, m => m.CarporteingaengeOhneEHs);
        }

        private List<CarporteingaengeOhneEH>LoadCarporteingaengeOhneEHFromSap()
        {            
            Z_M_ABMBEREIT_LAUFZEIT.Init(SAP, "KUNNR", LogonContext.KundenNr.ToSapKunnr());
            var sapList = Z_M_ABMBEREIT_LAUFZEIT.AUSGABE.GetExportListWithExecute(SAP);

            return AppModelMappings.Z_M_ABMBEREIT_LAUFZEIT_To_CarporteingaengeOhneEH.Copy(sapList).ToList();                                        
        }     
    }
}
