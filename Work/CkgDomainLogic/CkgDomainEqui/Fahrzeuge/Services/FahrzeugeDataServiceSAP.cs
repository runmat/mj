using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Fahrzeuge.Models.AppModelMappings;

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
            Z_M_ABM_ABGEMELDETE_KFZ.Init(SAP, "KUNNR", LogonContext.KundenNr.ToSapKunnr());

            if (selector.AbmeldeDatumRange.IsSelected)
            {
                SAP.SetImportParameter("PICKDATAB", selector.AbmeldeDatumRange.StartDate);
                SAP.SetImportParameter("PICKDATBI", selector.AbmeldeDatumRange.EndDate);
            }

            SAP.Execute();

            var sapItemsEquis = Z_M_ABM_ABGEMELDETE_KFZ.AUSGABE.GetExportList(SAP); 
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

            if (selector.NurMitAbmeldungen)
                SAP.SetImportParameter("I_MIT_ABM", "X");

            if (selector.NurOhneAbmeldungen)
                SAP.SetImportParameter("I_OHNE_ABM", "X");

            SAP.SetImportParameter("I_STORNO", "");

            if (selector.MeldeDatumRange.IsSelected)
            {
                SAP.SetImportParameter("I_ERDAT_VON", selector.MeldeDatumRange.StartDate);
                SAP.SetImportParameter("I_ERDAT_BIS", selector.MeldeDatumRange.EndDate);
            }

            if (selector.StillegungsDatumRange.IsSelected)
            {
                SAP.SetImportParameter("I_ABMDT_VON", selector.StillegungsDatumRange.StartDate);
                SAP.SetImportParameter("I_ABMDT_BIS", selector.StillegungsDatumRange.EndDate);
            }

            SAP.Execute();

            var sapItemsEquis = Z_DPM_UF_MELDUNGS_SUCHE.GT_UF.GetExportList(SAP);
            var webItemsEquis = AppModelMappings.Z_DPM_UF_MELDUNGS_SUCHE_To_Unfallmeldungen.Copy(sapItemsEquis).ToList();
            return webItemsEquis;
        }

        public void UnfallmeldungenCancel(List<Unfallmeldung> list, string cancelText, out int cancelCount, out string errorMessage)
        {
            cancelCount = 0;
            errorMessage = "";

            foreach (var item in list)
            {
                var unfallmeldung = item;
                var errorMessageOneItem = SAP.ExecuteAndCatchErrors(

                    // exception safe SAP action:
                    () =>
                    {
                        Z_DPM_UF_STORNO.Init(SAP, "I_AG", LogonContext.KundenNr.ToSapKunnr());
                        SAP.SetImportParameter("I_UNFALL_NR", unfallmeldung.UnfallNr);
                        SAP.SetImportParameter("I_STORNONAM", LogonContext.UserName);
                        SAP.SetImportParameter("I_STORNOBEM", cancelText);
                        SAP.Execute();
                    },

                    // SAP custom error handling:
                    () =>
                    {
                        var sapResult = SAP.ResultMessage;
                        if (SAP.ResultCode != 0 && SAP.ResultMessage.IsNotNullOrEmpty())
                            return sapResult;

                        return "";
                    }
                );

                if (errorMessageOneItem.IsNullOrEmpty())
                    cancelCount++;
                else
                    errorMessage += (errorMessage.IsNullOrEmpty() ? "" : "; ") + string.Format("{0} {1}: {2}", Localize.VIN, unfallmeldung.Fahrgestellnummer, errorMessageOneItem);
            }
        }

        public void MeldungCreateTryLoadEqui(ref Unfallmeldung model, out string errorMessage)
        {
            var unfallmeldung = model;

            errorMessage = SAP.ExecuteAndCatchErrors(
                // exception safe SAP action:
                () =>
                {
                    Z_DPM_UF_EQUI_SUCHE.Init(SAP, "I_AG", LogonContext.KundenNr.ToSapKunnr());
                    SAP.SetImportParameter("I_LICENSE_NUM", unfallmeldung.Kennzeichen);
                    SAP.SetImportParameter("I_CHASSIS_NUM", unfallmeldung.Fahrgestellnummer);
                    SAP.SetImportParameter("I_TIDNR", unfallmeldung.BriefNummer);
                    SAP.SetImportParameter("I_ZZREFERENZ1", unfallmeldung.UnitNummer);
                    SAP.SetImportParameter("I_VORG_ART", unfallmeldung.MeldungTyp);
                    SAP.Execute();
                },

                // SAP custom error handling:
                () =>
                {
                    var sapResult = SAP.ResultMessage;
                    if (SAP.ResultCode != 0 && SAP.ResultMessage.IsNotNullOrEmpty())
                        return sapResult;

                    return "";
                }
            );

            if (!errorMessage.IsNullOrEmpty())
                return;

            var sapItemsEquis = Z_DPM_UF_EQUI_SUCHE.GT_EQUIS.GetExportList(SAP);
            if (sapItemsEquis.None())
                return;

            var meldungTyp = model.MeldungTyp;
            model = AppModelMappings.Z_DPM_UF_EQUI_SUCHE_To_Unfallmeldungen.Copy(sapItemsEquis).First();
            model.MeldungTyp = meldungTyp;
        }

        public void MeldungCreate(Unfallmeldung unfallmeldung, out string errorMessage)
        {
            errorMessage = SAP.ExecuteAndCatchErrors(
                // exception safe SAP action:
                () =>
                {
                    Z_DPM_UF_CREATE.Init(SAP, "I_AG", LogonContext.KundenNr.ToSapKunnr());
                    SAP.SetImportParameter("I_ERNAM", LogonContext.UserName);
                    SAP.SetImportParameter("I_EQUNR", unfallmeldung.EquiNr);
                    SAP.SetImportParameter("I_STATION", unfallmeldung.StationsCode);
                    SAP.SetImportParameter("I_STANDORT", unfallmeldung.Standort);
                    SAP.SetImportParameter("I_VORG_ART", unfallmeldung.MeldungTyp);
                    SAP.Execute();
                },

                // SAP custom error handling:
                () =>
                {
                    var sapResult = SAP.ResultMessage;
                    if (SAP.ResultCode != 0 && SAP.ResultMessage.IsNotNullOrEmpty())
                        return sapResult;

                    return "";
                }
            );
        }

        public List<Adresse> GetStationCodes()
        {
            Z_DPM_CHANGE_ADDR002_001.Init(SAP, "I_KUNNR_AG", LogonContext.KundenNr.ToSapKunnr());
            SAP.SetImportParameter("I_WEBUSER", LogonContext.UserName);
            SAP.SetImportParameter("I_TYPE", "1");
            SAP.SetImportParameter("I_ADDRTYP", "ZSTO");
            SAP.Execute();

            var sapItemsEquis = Z_DPM_CHANGE_ADDR002_001.GT_OUT.GetExportList(SAP);
            var webItemsEquis = AppModelMappings.Z_DPM_CHANGE_ADDR002_001_To_Adresse.Copy(sapItemsEquis).ToList();

            return webItemsEquis;
        }

        public List<Domaenenfestwert> GetFarben()
        {
            var sapList = Z_DPM_DOMAENENFESTWERTE.GT_WEB.GetExportListWithInitExecute(SAP, "DOMNAME, DDLANGUAGE", "ZFARBE", "DE");

            return DomainCommon.Models.AppModelMappings.Z_DPM_DOMAENENFESTWERTE_GT_WEB_To_Domaenenfestwert.Copy(sapList).ToList();
        }

        public List<Fzg> GetFahrzeugeForZulassung()
        {
            Z_M_EC_AVM_MELDUNGEN_PDI1.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());
            SAP.SetImportParameter("I_VKORG", "1510");
            SAP.SetImportParameter("I_PHASE", "B");
            SAP.Execute();

            var sapItemsData = Z_M_EC_AVM_MELDUNGEN_PDI1.GT_WEB.GetExportList(SAP);
            sapItemsData = sapItemsData.Where(s => s.ZZAKTSPERRE.NotNullOrEmpty().ToUpper() != "X").ToListOrEmptyList();
            var webItems = AppModelMappings.Z_M_EC_AVM_MELDUNGEN_PDI1_GT_WEB_ToFzg.Copy(sapItemsData).ToList();

            var sapItemsText = Z_M_EC_AVM_MELDUNGEN_PDI1.GT_TXT.GetExportList(SAP);
            AppModelMappings.Z_M_EC_AVM_MELDUNGEN_PDI1_GT_TXT_ToFzg(sapItemsText, webItems);

            return webItems;
        }

        public List<KennzeichenSerie> GetKennzeichenSerie()
        {
            Z_M_EC_AVM_KENNZ_SERIE.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());
            SAP.SetImportParameter("I_ART", "0");
            SAP.Execute();

            var sapItemsData = Z_M_EC_AVM_KENNZ_SERIE.GT_WEB.GetExportList(SAP);
            var webItems = AppModelMappings.Z_M_EC_AVM_KENNZ_SERIE_GT_WEB_ToKennzeichenSerie.Copy(sapItemsData.OrderBy(s => s.SONDERSERIE)).ToList();

            return webItems;
        }

        public List<Fzg> GetZulassungenAnzahlForPdiAndDate(DateTime date, out string errorMessage)
        {
            var webItems = new List<Fzg>();

            errorMessage = SAP.ExecuteAndCatchErrors(

                // exception safe SAP action:
                () =>
                {
                    Z_M_EC_AVM_ANZ_BEAUFTR_ZUL.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());
                    SAP.SetImportParameter("I_ZULDAT", date);
                    SAP.Execute();

                    var sapItemsData = Z_M_EC_AVM_ANZ_BEAUFTR_ZUL.GT_WEB.GetExportList(SAP);
                    webItems = AppModelMappings.Z_M_EC_AVM_ANZ_BEAUFTR_ZUL_GT_WEB_ToFzg.Copy(sapItemsData).ToList();
                }
                ,

                // SAP custom error handling:
                () =>
                {
                    var sapResult = SAP.ResultMessage;
                    if (SAP.ResultMessage.IsNotNullOrEmpty())
                        return sapResult;

                    return "";
                });

            return webItems;
        }

        public string ZulassungSave(List<Fzg> fahrzeuge, DateTime zulassungsDatum, string kennzeichenSerie)
        {
            var sperreErrorMessage = ZulassungFahrzeugeSperren(fahrzeuge);
            if (sperreErrorMessage.IsNotNullOrEmpty())
                return sperreErrorMessage.PrependIfNotNull("Fehler bei der Fahrzeug-Sperre: ");

            var zulassenErrorMessage = ZulassungFahrzeugeZulassen(fahrzeuge, zulassungsDatum, kennzeichenSerie);

            var invalidItems = fahrzeuge.Where(f => !f.IsValid);
            if (invalidItems.Any())
                return string.Format(
                    "{0}{1} Fahrzeuge konnten nicht zugelassen werden, siehe Details in unten stehender Fahrzeugtabelle.", 
                        zulassenErrorMessage.FormatIfNotNull("Fehler bei der Fahrzeug-Zulassung: {this}. "),
                        invalidItems.Count());

            return zulassenErrorMessage;
        }

        public List<FloorcheckHaendler> GetFloorcheckHaendler(FloorcheckHaendler haendler)
        {            
            Z_DPM_RETAIL_FLOORCHECK_01.Init(SAP, "I_KUNNR_AG", LogonContext.KundenNr.ToSapKunnr());

            if (!String.IsNullOrEmpty(haendler.HaendlerNummer))
                SAP.SetImportParameter("I_HAENDLER", haendler.HaendlerNummer);

            if (!String.IsNullOrEmpty(haendler.HaendlerName))
                SAP.SetImportParameter("I_NAME", haendler.HaendlerName);

            if (!String.IsNullOrEmpty(haendler.HaendlerOrt))
                SAP.SetImportParameter("I_ORT", haendler.HaendlerOrt);

            SAP.Execute();

            var sapItemsData = Z_DPM_RETAIL_FLOORCHECK_01.GT_HAENDLER.GetExportList(SAP);
            var webItems = AppModelMappings.Z_DPM_RETAIL_FLOORCHECK_01_GT_HAENDLER_To_FloorcheckHaendler.Copy(sapItemsData).ToList();

            return webItems;          
        }

        public List<Floorcheck> GetFloorchecks(string haendlerNo)
        {
            Z_DPM_RETAIL_FLOORCHECK_01.Init(SAP, "I_KUNNR_AG", LogonContext.KundenNr.ToSapKunnr());
           
            SAP.SetImportParameter("I_HAENDLER", haendlerNo);
           
            SAP.Execute();

            var sapItemsData = Z_DPM_RETAIL_FLOORCHECK_01.GT_DATEN.GetExportList(SAP);
            var webItems = AppModelMappings.Z_DPM_RETAIL_FLOORCHECK_01_GT_DATEN_To_Floorcheck.Copy(sapItemsData).ToList();

            return webItems;           
        }

        string ZulassungFahrzeugeSperren(List<Fzg> fahrzeuge)
        {
            var errorMessage = SAP.ExecuteAndCatchErrors(

                    // exception safe SAP action:
                    () =>
                    {
                        var list = Z_M_WARENKORB_SPERRE_001.GT_IN.GetImportList(SAP);
                        foreach (var f in fahrzeuge)
                            list.Add(new Z_M_WARENKORB_SPERRE_001.GT_IN { EQUNR = f.EquiNummer });

                        Z_M_WARENKORB_SPERRE_001.Init(SAP);
                        SAP.ApplyImport(list);
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

            return errorMessage;
        }

        string ZulassungFahrzeugeZulassen(List<Fzg> fahrzeuge, DateTime zulassungsDatum, string kennzeichenSerie)
        {
            var errorMessage = SAP.ExecuteAndCatchErrors(

                    // exception safe SAP action:
                    () =>
                    {
                        var list = Z_MASSENZULASSUNG.INTERNTAB.GetImportList(SAP);

                        foreach (var f in fahrzeuge)
                            list.Add(new Z_MASSENZULASSUNG.INTERNTAB
                            {
                                I_KUNNR_AG = LogonContext.KundenNr.ToSapKunnr(),
                                I_ZZFAHRG = f.Fahrgestellnummer,
                                I_EDATU = zulassungsDatum.ToString("dd.MM.yyyy"),
                                I_KUNNR_ZV = "1917".ToSapKunnr(),
                                I_ZZKENNZ = string.Empty,
                                I_KUNNR_ZH = "100010607".ToSapKunnr(),
                                I_KUNNR_ZA = string.Empty,
                                I_ZZSONDER = FormatKennzeichenSerie(kennzeichenSerie),
                                I_KBANR = "0200000",
                                I_ZZCARPORT = f.DadPdi,
                            });

                        Z_MASSENZULASSUNG.Init(SAP);
                        SAP.ApplyImport(list);
                        SAP.Execute();

                        var retCode = SAP.GetExportParameter("RETURN");
                        // ReSharper disable once UnusedVariable
                        var anzahlZugelassen = SAP.GetExportParameter("ANZAHL");
                        var exportList = Z_MASSENZULASSUNG.OUTPUT.GetExportList(SAP);

                        foreach (var f in fahrzeuge)
                        {
                            var savedItem = exportList.FirstOrDefault(e => e.ID == f.Fahrgestellnummer);

                            f.IsValid = (retCode.NotNullOrEmpty().ToUpper() == "OK" && (savedItem == null || savedItem.MESSAGE.IsNullOrEmpty()));
                            f.ValidationMessage = (f.IsValid ? Localize.OK : (savedItem != null ? savedItem.MESSAGE : "").PrependIfNotNull(String.Format("{0}, ", Localize.Error)));
                        }
                    },

                    // SAP custom error handling:
                    () =>
                    {
                        var sapResult = SAP.ResultMessage;
                        if (SAP.ResultMessage.IsNotNullOrEmpty())
                            return sapResult;

                        return "";
                    });

            return errorMessage;
        }

        static string FormatKennzeichenSerie(string kennzeichenSerie)
        {
            if (kennzeichenSerie.IsNullOrEmpty())
                return "";

            var indexKlammer = kennzeichenSerie.IndexOf("(", StringComparison.InvariantCulture);
            if (indexKlammer <= 0)
                return kennzeichenSerie;

            return kennzeichenSerie.Substring(0, indexKlammer).Trim();
        }

       
    }
}
