using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.Equi.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Equi.Models.AppModelMappings;

namespace CkgDomainLogic.Equi.Services
{
    public class BriefbestandDataServiceSAP : CkgGeneralDataServiceSAP, IBriefbestandDataService
    {
        public FahrzeugbriefFilter DatenFilter { get; set; }

        public List<Fahrzeugbrief> FahrzeugbriefeZumVersand { get { return PropertyCacheGet(() => LoadFahrzeugbriefeFromSap(true, false).ToList()); } }

        private List<Fahrzeugbrief> FahrzeugbriefeGesamt { get { return PropertyCacheGet(() => LoadFahrzeugbriefeFromSap(true, true).ToList()); } }

        public List<Fahrzeugbrief> FahrzeugbriefeBestand 
        { 
            get
            {
                if (DatenFilter.SelektionsfilterLagerbestand && DatenFilter.SelektionsfilterTempVersendete)
                {
                    return FahrzeugbriefeGesamt;
                }
                if (DatenFilter.SelektionsfilterLagerbestand)
                {
                    return FahrzeugbriefeGesamt.Where(b => b.AbcKennzeichen != "1").ToList();
                }
                if (DatenFilter.SelektionsfilterTempVersendete)
                {
                    return FahrzeugbriefeGesamt.Where(b => b.AbcKennzeichen == "1").ToList();
                }
                return new List<Fahrzeugbrief>();
            } 
        }

        public BriefbestandDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
            DatenFilter = new FahrzeugbriefFilter();
        }

        public void MarkForRefreshFahrzeugbriefe()
        {
            PropertyCacheClear(this, m => m.FahrzeugbriefeGesamt);
            PropertyCacheClear(this, m => m.FahrzeugbriefeZumVersand);
        }

        private IEnumerable<Fahrzeugbrief> LoadFahrzeugbriefeFromSap(bool mitBestand, bool mitTempVers)
        {
            Z_DPM_BRIEFBESTAND_001.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());

            if (mitBestand)
                SAP.SetImportParameter("I_BESTAND", "X");

            if (mitTempVers)
                SAP.SetImportParameter("I_TEMPVERS", "X");

            var sapList = Z_DPM_BRIEFBESTAND_001.GT_DATEN.GetExportListWithExecute(SAP);

            return AppModelMappings.Z_DPM_BRIEFBESTAND_001_GT_DATEN_To_Fahrzeugbrief.Copy(sapList);
        }
    }
}
