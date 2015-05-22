// ReSharper disable RedundantUsingDirective

using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Fahrzeuge.Models.AppModelMappings;

// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.Fahrzeuge.Services
{
    public class FahrzeugeDataServiceSAP : CkgGeneralDataServiceSAP, IFahrzeugeDataService
    {
        public FahrzeugeDataServiceSAP(ISapDataService sap)
            :base(sap)
        {
        }

        public List<AbgemeldetesFahrzeug> GetAbgemeldeteFahrzeuge(AbgemeldeteFahrzeugeSelektor selector)
        {
            Z_DPM_CD_ABM_LIST.Init(SAP, "I_AG", LogonContext.KundenNr.ToSapKunnr());

            if (selector.NurKlaerfaelle)
                SAP.SetImportParameter("I_STATUS_NUR_KF", "X");

            if (selector.AbmeldeDatumRange.IsSelected)
            {
                SAP.SetImportParameter("I_DAT_ABM_AUFTR_AB", selector.AbmeldeDatumRange.StartDate);
                SAP.SetImportParameter("I_DAT_ABM_AUFTR_BIS", selector.AbmeldeDatumRange.EndDate);
            }

            if (selector.GueltigkeitsEndeDatum.IsSelected)
            {
                SAP.SetImportParameter("I_EXPIRY_DATE_AB", selector.GueltigkeitsEndeDatum.StartDate);
                SAP.SetImportParameter("I_EXPIRY_DATE_BIS", selector.GueltigkeitsEndeDatum.EndDate);
            }

            if (selector.Fin.IsNotNullOrEmpty())
                SAP.SetImportParameter("I_FIN", selector.Fin);
            if (selector.Fin10.IsNotNullOrEmpty())
                SAP.SetImportParameter("I_FIN_10", selector.Fin10);

            if (selector.FahrzeugStatusWerte.AnyAndNotNull()) // && !selector.NurKlaerfaelle
            {
                var statusList = AppModelMappings.Z_DPM_CD_ABM_LIST__IT_STATUS_To_FahrzeugStatus.CopyBack(selector.FahrzeugStatusWerte.Select(e => new FahrzeugStatus{ ID = e }).ToList()).ToList();
                SAP.ApplyImport(statusList);
            }

            if (selector.Zielort.IsNotNullOrEmpty())
                SAP.SetImportParameter("I_ZIELORT", selector.Zielort);
            if (selector.Betrieb.IsNotNullOrEmpty())
                SAP.SetImportParameter("I_BETRIEB", selector.Betrieb);
            if (selector.Kostenstelle.IsNotNullOrEmpty())
                SAP.SetImportParameter("I_KOSTST", selector.Kostenstelle);

            if (selector.Abteilung.IsNotNullOrEmpty())
                SAP.SetImportParameter("I_ABTEILUNG", selector.Abteilung);
            if (selector.AbteilungsLeiter.IsNotNullOrEmpty())
                SAP.SetImportParameter("I_ABT_LEITER_NAME", selector.AbteilungsLeiter);
            //if (selector.AbteilungsLeiterVorname.IsNotNullOrEmpty())
            //    SAP.SetImportParameter("I_ABT_LEITER_VNAME", selector.AbteilungsLeiterVorname);

            SAP.Execute();

            var sapItemsEquis = Z_DPM_CD_ABM_LIST.ET_ABM_LIST.GetExportList(SAP);
            var webItemsEquis = AppModelMappings.Z_DPM_CD_ABM_LIST__ET_ABM_LIST_To_AbgemeldetesFahrzeug.Copy(sapItemsEquis).ToList();

            return webItemsEquis;
        }

        public List<AbmeldeHistorie> GetAbmeldeHistorien(string fin)
        {
            Z_DPM_CD_ABM_HIST.Init(SAP, "I_FIN", fin);
           
            SAP.Execute();

            var sapItemsEquis = Z_DPM_CD_ABM_HIST.ET_ABM_HIST.GetExportList(SAP);
            var webItemsEquis = AppModelMappings.Z_DPM_CD_ABM_HIST__ET_ABM_HIST_To_AbmeldeHistorie.Copy(sapItemsEquis).ToList();

            return webItemsEquis;
        }


        public List<Zb2BestandSecurityFleet> GetZb2BestandSecurityFleet(Zb2BestandSecurityFleetSelektor selector)
        {
            Z_M_ECA_TAB_BESTAND.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());

            if (selector.Herstellerkennung.IsNotNullOrEmpty())
                SAP.SetImportParameter("I_HERST", selector.Herstellerkennung);

            SAP.Execute();

            var sapItemsEquis = Z_M_ECA_TAB_BESTAND.GT_WEB.GetExportList(SAP);
            var webItemsEquis = AppModelMappings.Z_M_ECA_TAB_BESTAND_To_Zb2BestandSecurityFleet.Copy(sapItemsEquis).ToList();

            return webItemsEquis;
        }

        public List<Fahrzeughersteller> GetFahrzeugHersteller()
        {
            Z_M_HERSTELLERGROUP.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());
          
            SAP.Execute();

            var sapItemsEquis = Z_M_HERSTELLERGROUP.T_HERST.GetExportList(SAP);
            var webItemsEquis = AppModelMappings.Z_M_HERSTELLERGROUP_To_Fahrzeughersteller.Copy(sapItemsEquis).ToList();

            return webItemsEquis;
        }
        
        public List<AbgemeldetesFahrzeug> GetAbgemeldeteFahrzeuge2(AbgemeldeteFahrzeugeSelektor selector)
        {                                  
            Z_M_Abm_Abgemeldete_Kfz.Init(SAP, "KUNNR", LogonContext.KundenNr.ToSapKunnr());

            if (selector.AbmeldeDatumRange.IsSelected)
            {
                SAP.SetImportParameter("PICKDATAB", selector.AbmeldeDatumRange.StartDate);
                SAP.SetImportParameter("PICKDATBI", selector.AbmeldeDatumRange.EndDate);
            }

            SAP.Execute();

            var sapItemsEquis = Z_M_Abm_Abgemeldete_Kfz.AUSGABE.GetExportList(SAP); 
            var webItemsEquis = AppModelMappings.Z_M_Abm_Abgemeldete_Kfz_AUSGABE_ToAbgemeldetesFahrzeug.Copy(sapItemsEquis).ToList();

            return webItemsEquis;
        }

        public List<Treuhandbestand> GetTreuhandbestandFromSap()
        {                      
            Z_M_TH_BESTAND.Init(SAP, "I_AG", LogonContext.KundenNr.ToSapKunnr());
            SAP.SetImportParameter("I_EQTYP", "B");
            SAP.Execute();

            var sapItemsEquis = Z_M_TH_BESTAND.GT_BESTAND.GetExportList(SAP);
            var webItemsEquis = AppModelMappings.Z_M_TH_BESTAND__GET_BESTAND_LIST_To_Treuhandbestand.Copy(sapItemsEquis).ToList();

            return webItemsEquis;
        }   

        public List<Unfallmeldung> GetUnfallmeldungen(UnfallmeldungenSelektor selector)
        {
            Z_DPM_UF_MELDUNGS_SUCHE.Init(SAP, "I_AG", LogonContext.KundenNr.ToSapKunnr());
            SAP.SetImportParameter("I_MIT_ABM", "X");

            if (selector.MeldeDatumRange.IsSelected)
            {
                SAP.SetImportParameter("I_ABMDT_VON", selector.MeldeDatumRange.StartDate);
                SAP.SetImportParameter("I_ABMDT_BIS", selector.MeldeDatumRange.EndDate);
            }

            if (selector.StillegungsDatumRange.IsSelected)
            {
                SAP.SetImportParameter("I_ERDAT_VON", selector.StillegungsDatumRange.StartDate);
                SAP.SetImportParameter("I_ERDAT_BIS", selector.StillegungsDatumRange.EndDate);
            }

            SAP.Execute();

            var sapItemsEquis = Z_DPM_UF_MELDUNGS_SUCHE.GT_UF.GetExportList(SAP);
            var webItemsEquis = AppModelMappings.Z_DPM_UF_MELDUNGS_SUCHE_To_Unfallmeldungen.Copy(sapItemsEquis).ToList();
            return webItemsEquis;
        }

        public List<Fahrzeug> GetFahrzeugeForZulassung()
        {
            Z_M_EC_AVM_MELDUNGEN_PDI1.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());
            SAP.SetImportParameter("I_VKORG", "1510");
            SAP.Execute();

            var sapItemsEquis = Z_M_EC_AVM_MELDUNGEN_PDI1.GT_WEB.GetExportList(SAP);
            var webItemsEquis = AppModelMappings.Z_M_EC_AVM_MELDUNGEN_PDI1_GT_WEB_ToFahrzeug.Copy(sapItemsEquis).ToList();

            return webItemsEquis;
        }
    }
}
