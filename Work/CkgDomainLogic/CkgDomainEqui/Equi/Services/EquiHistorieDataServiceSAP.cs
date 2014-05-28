using System.Linq;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.Equi.Models;
using CkgDomainLogic.General.Services;
using SapORM.Contracts;
using SapORM.Models;

namespace CkgDomainLogic.Equi.Services
{
    public class EquiHistorieDataServiceSAP : CkgGeneralDataServiceSAP, IEquiHistorieDataService
    {
        public EquiHistorieDataServiceSAP(ISapDataService sap)
            :base(sap)
        {
        }

        public EquiHistorie GetEquiHistorie(string fahrgestellnummer)
        {
            EquiHistorie hist = null;

            SAP.Init("Z_M_BRIEFLEBENSLAUF_001", "I_KUNNR", LogonContext.KundenNr.PadLeft(10, '0'));

            SAP.SetImportParameter("I_ZZFAHRG", fahrgestellnummer);

            SAP.Execute();

            // GT_WEB
            var sapItemsHist = Z_M_BRIEFLEBENSLAUF_001.GT_WEB.GetExportList(SAP);
            var webItemsHist = AppModelMappings.Z_M_BRIEFLEBENSLAUF_001_GT_WEB_To_EquiHistorie.Copy(sapItemsHist).OrderBy(w => w.Fahrgestellnummer).ToList();

            // GT_QMEL
            var sapItemsMeld = Z_M_BRIEFLEBENSLAUF_001.GT_QMEL.GetExportList(SAP);
            var webItemsMeld = AppModelMappings.Z_M_BRIEFLEBENSLAUF_001_GT_QMEL_To_EquiMeldungsdaten.Copy(sapItemsMeld).OrderBy(w => w.Meldungsnummer).ToList();
            
            if (webItemsHist.Count > 0)
            {
                hist = webItemsHist[0];
                hist.Meldungsdaten = webItemsMeld;
            }

            return hist ?? new EquiHistorie();
        }
    }
}
