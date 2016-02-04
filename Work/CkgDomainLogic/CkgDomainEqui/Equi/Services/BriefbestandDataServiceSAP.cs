using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.Equi.Models;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Equi.Models.AppModelMappings;

namespace CkgDomainLogic.Equi.Services
{
    public class BriefbestandDataServiceSAP : CkgGeneralDataServiceSAP, IBriefbestandDataService
    {
        #region Briefbestand

        public FahrzeugbriefFilter DatenFilter { get; set; }

        public BriefversandModus FahrzeugbriefeZumVersandModus { get; set; }
        
        public List<Fahrzeugbrief> FahrzeugbriefeZumVersand
        {
            get { return PropertyCacheGet(() => LoadFahrzeugbriefeFromSap(true, true, FahrzeugbriefeZumVersandModus).ToList()); }
        }

        List<Fahrzeugbrief> FahrzeugbriefeGesamt
        {
            get { return PropertyCacheGet(() => LoadFahrzeugbriefeFromSap(true, true).ToList()); }
        }

        public List<Fahrzeugbrief> FahrzeugbriefeBestand
        {
            get
            {
                if (DatenFilter.SelektionsfilterLagerbestand && DatenFilter.SelektionsfilterTempVersendete)
                    return FahrzeugbriefeGesamt;

                if (DatenFilter.SelektionsfilterLagerbestand)
                    return FahrzeugbriefeGesamt.Where(b => b.AbcKennzeichen != "1").ToList();

                if (DatenFilter.SelektionsfilterTempVersendete)
                    return FahrzeugbriefeGesamt.Where(b => b.AbcKennzeichen == "1").ToList();

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

        private IEnumerable<Fahrzeugbrief> LoadFahrzeugbriefeFromSap(bool mitBestand, bool mitTempVers, BriefversandModus versandModus = BriefversandModus.Brief)
        {
            Z_DPM_BRIEFBESTAND_001.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());

            if (mitBestand)
                SAP.SetImportParameter("I_BESTAND", "X");

            if (mitTempVers)
                SAP.SetImportParameter("I_TEMPVERS", "X");

            var ref1 = GetStringUserReferenceValueByReferenceType(ReferenzfeldtypString.ZZREFERENZ1);
            if (!String.IsNullOrEmpty(ref1))
                SAP.SetImportParameter("I_ZZREFERENZ1", ref1);

            string sapVersandKennzeichen = null;
            if (versandModus == BriefversandModus.Brief || versandModus == BriefversandModus.BriefMitSchluessel)
                sapVersandKennzeichen = "B";
            if (versandModus == BriefversandModus.Schluessel)
                sapVersandKennzeichen = "T";
            SAP.SetImportParameter("I_EQTYP", sapVersandKennzeichen);

            var sapList = Z_DPM_BRIEFBESTAND_001.GT_DATEN.GetExportListWithExecute(SAP);

            return AppModelMappings.Z_DPM_BRIEFBESTAND_001_GT_DATEN_To_Fahrzeugbrief.Copy(sapList);
        }

        #endregion


        #region VersandBeauftragungen

        public List<Fahrzeugbrief> GetVersandBeauftragungen(VersandBeauftragungSelektor model)
        {
            var briefe = AppModelMappings.Z_M_VERSAUFTR_FEHLERHAFTE_GT_WEB_To_Fahrzeugbrief.Copy(GetSapVersandBeauftragungen(model));

            if (model.FilterVersandBeauftragungsTyp == "OnlyZBII")
                briefe = briefe.Where(brief => brief.BriefVersand);

            if (model.FilterVersandBeauftragungsTyp == "OnlyKeys")
                briefe = briefe.Where(brief => brief.SchluesselVersand);

            return briefe.ToList();
        }

        private IEnumerable<Z_M_VERSAUFTR_FEHLERHAFTE.GT_WEB> GetSapVersandBeauftragungen(VersandBeauftragungSelektor model)
        {
            Z_M_VERSAUFTR_FEHLERHAFTE.Init(SAP);

            SAP.SetImportParameter("I_KUNNR", LogonContext.KundenNr.ToSapKunnr());

            if (model.FilterAuchNeue)
                SAP.SetImportParameter("I_BEAUFTR", "X");

            if (model.FilterVersandVorbereitungDAD)
                SAP.SetImportParameter("FLAG_VERS", "X");

            if (model.FilterFreigabeTreugeberOffen)
                SAP.SetImportParameter("FLAG_VERS_SPERR", "X");

            SAP.Execute();

            return Z_M_VERSAUFTR_FEHLERHAFTE.GT_WEB.GetExportList(SAP);
        }

        public string DeleteVersandBeauftragungen(string fin, string kennzeichen)
        {
            var error = SAP.ExecuteAndCatchErrors(

                // exception safe SAP action:
                () =>
                {
                    Z_M_VERSAUFTR_FEHLERHAFTE_DEL.Init(SAP);
                    SAP.SetImportParameter("I_KUNNR", LogonContext.KundenNr.ToSapKunnr());
                    SAP.SetImportParameter("I_LICENSE_NUM", kennzeichen);
                    SAP.SetImportParameter("I_CHASSIS_NUM", fin);
                    SAP.SetImportParameter("I_ZZBRFVERS", "1");
                    SAP.SetImportParameter("I_ZZSCHLVERS", "0");
                    SAP.SetImportParameter("I_IDNRK", "");
                    SAP.SetImportParameter("I_ZANF_NR", "");
                    SAP.Execute();
                },

                // SAP custom error handling:
                () =>
                {
                    var sapResult = SAP.ResultMessage;
                    if (SAP.ResultMessage.IsNotNullOrEmpty())
                        return sapResult;

                    return "";
                });

            return error;
        }

        #endregion


        #region ZBII Ein- und Ausgaenge

        public List<Fahrzeugbrief> GetEinAusgaenge(EinAusgangSelektor model)
        {
            return AppModelMappings.Z_DAD_DATEN_EINAUS_REPORT_002_EINNEU_To_Fahrzeugbrief.Copy(GetSapEinAusgaenge(model)).ToList();
        }

        private IEnumerable<Z_DAD_DATEN_EINAUS_REPORT_002.EINNEU> GetSapEinAusgaenge(EinAusgangSelektor model)
        {
            Z_DAD_DATEN_EINAUS_REPORT_002.Init(SAP);

            SAP.SetImportParameter("KUNNR", LogonContext.KundenNr.ToSapKunnr());

            SAP.SetImportParameter("ACTION", model.FilterEinAusgangsTyp == "Inputs" ? "NEU" : "AUS");

            SAP.SetImportParameter("ABCKZ", "A");
            if (model.FilterEinAusgangsTyp == "Outputs")
                SAP.SetImportParameter("ABCKZ", model.FilterAusgangsTyp == "Final" ? "2" : model.FilterAusgangsTyp == "Temporary" ? "1" : "A");

            if (model.DatumRange.IsSelected)
            {
                SAP.SetImportParameter("DATANF", model.DatumRange.StartDate);
                SAP.SetImportParameter("DATEND", model.DatumRange.EndDate);
            }

            SAP.Execute();

            return Z_DAD_DATEN_EINAUS_REPORT_002.EINNEU.GetExportList(SAP);
        }

        #endregion


        #region Stuecklisten

        public IEnumerable<StuecklistenKomponente> GetStuecklistenKomponenten(IEnumerable<string> fahrgestellnummern)
        {
            Z_DPM_READ_EQUI_STL_01.Init(SAP, "I_AG, I_EQTYP", LogonContext.KundenNr.ToSapKunnr(), "B");

            var importList = fahrgestellnummern.Select(f => new Z_DPM_READ_EQUI_STL_01.GT_IN {CHASSIS_NUM = f}).ToListOrEmptyList();
            SAP.ApplyImport(importList);

            SAP.Execute();

            var sapList = Z_DPM_READ_EQUI_STL_01.GT_OUT.GetExportList(SAP);
            var list = AppModelMappings.Z_DPM_READ_EQUI_STL_01_GT_OUT_To_StuecklistenKomponente.Copy(sapList);

            return list;
        }

        public List<Stuecklisten> GetStuecklisten(string equinummer)
        {
            Z_DPM_READ_EQUI_STL_02.Init(SAP, "I_EQUNR",  equinummer);
                        
            SAP.Execute();

            var sapList = Z_DPM_READ_EQUI_STL_02.GT_OUT.GetExportList(SAP);
            var list = AppModelMappings.Z_DPM_READ_EQUI_STL_02_GT_OUT_To_StuecklistenKomponente.Copy(sapList).ToList();

            return list;
        }

        #endregion


        #region Fahrzeugbriefe

        public IEnumerable<Fahrzeugbrief> GetFahrzeugBriefe(Fahrzeugbrief fahrzeug)
        {
            Z_DPM_UNANGEF_ALLG_01.Init(SAP, "I_KUNNR_AG, I_EQTYP", LogonContext.KundenNr.ToSapKunnr(), "B");

            var importList = AppModelMappings.MapFahrzeugbriefeImportToSAP.CopyBack(new List<Fahrzeugbrief> { fahrzeug });
            SAP.ApplyImport(importList);

            SAP.Execute();

            var listAbrufbar = AppModelMappings.MapFahrzeugbriefeAbrufbarFromSAP.Copy(Z_DPM_UNANGEF_ALLG_01.GT_ABRUFBAR.GetExportList(SAP));
            var listFehlerhaft = AppModelMappings.MapFahrzeugbriefeFehlerhaftFromSAP.Copy(Z_DPM_UNANGEF_ALLG_01.GT_FEHLER.GetExportList(SAP));
            var list = listAbrufbar.Concat(listFehlerhaft);

            return list;
        }
       
        #endregion
    }
}
