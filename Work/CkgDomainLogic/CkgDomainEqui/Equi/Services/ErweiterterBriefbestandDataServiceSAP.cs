using System;
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
    public class ErweiterterBriefbestandDataServiceSAP : CkgGeneralDataServiceSAP, IErweiterterBriefbestandDataService
    {
        public FahrzeugbriefSuchparameter Suchparameter { get; set; }

        public List<FahrzeugbriefErweitert> Fahrzeugbriefe { get { return PropertyCacheGet(() => LoadFahrzeugbriefeFromSap().ToList()); } }

        public ErweiterterBriefbestandDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
            Suchparameter = new FahrzeugbriefSuchparameter{ Selektion = "alle" };
        }

        public void MarkForRefreshFahrzeugbriefe()
        {
            PropertyCacheClear(this, m => m.Fahrzeugbriefe);
        }

        private IEnumerable<FahrzeugbriefErweitert> LoadFahrzeugbriefeFromSap()
        {
            Z_DPM_BRIEFBESTAND_002.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());

            if (Suchparameter.Selektion == "alle" || Suchparameter.Selektion == "bestand")
                SAP.SetImportParameter("I_BESTAND", "X");

            if (Suchparameter.Selektion == "alle" || Suchparameter.Selektion == "tempvers")
                SAP.SetImportParameter("I_TEMPVERS", "X");

            var sapList = Z_DPM_BRIEFBESTAND_002.GT_DATEN.GetExportListWithExecute(SAP);

            return AppModelMappings.Z_DPM_BRIEFBESTAND_002_GT_DATEN_To_FahrzeugbriefErweitert.Copy(sapList);
        }

        public string SaveSperrvermerk(FahrzeugbriefErweitert model)
        {
            string error = "";

            try
            {
                Z_DPM_SET_EQUI_REFERENZ.Init(SAP, "IMP_AG", LogonContext.KundenNr.ToSapKunnr());
                Z_DPM_SET_EQUI_REFERENZ.SetImportParameter_IMP_FIN(SAP, model.Fahrgestellnummer);
                Z_DPM_SET_EQUI_REFERENZ.SetImportParameter_IMP_REFERENZ1(SAP, model.Referenz1);
                if (!string.IsNullOrEmpty(model.Referenz1))
                    Z_DPM_SET_EQUI_REFERENZ.SetImportParameter_IMP_REFERENZ2(SAP, model.Referenz2);
                else
                    Z_DPM_SET_EQUI_REFERENZ.SetImportParameter_IMP_REFERENZ2(SAP, "");


                SAP.Execute();
            }
            catch (Exception e)
            {
                error = "SAP error: " + e.Message;
            }
            return error;
        }
    }
}
