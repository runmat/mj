using System.Collections.Generic;
using GeneralTools.Models;

namespace SapORM.Models
{
    public class AppModelMappings : ModelMappings
    {

        // ReSharper disable InconsistentNaming
        public static ModelMapping<Z_V_Ueberf_Auftr_Kund_Port.T_AUFTRAEGE, HistoryAuftrag> Z_V_Ueberf_Auftr_Kund_Port_T_AUFTRAEGE_To_HistoryAuftrag
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_V_Ueberf_Auftr_Kund_Port.T_AUFTRAEGE, HistoryAuftrag>(
                                                new Dictionary<string, string> {
                                                    { "AUFNR", "AuftragsNr" },
                                                    { "ERDAT", "AuftragsDatum" },
                                                    { "FAHRTNR", "Fahrt" },
                                                    { "ZZKENN", "Kennzeichen" },
                                                    { "ZzRefnr", "Referenz" },
                                                    { "ZZBEZEI", "Typ" },
                                                    { "VDATU", "UeberfuehrungsDatum" },
                                                    { "wadat_ist", "AbgabeDatum" },
                                                    { "FahrtVon", "FahrtVonOrt" },
                                                    { "FahrtNach", "FahrtNachOrt" },
                                                    { "Gef_Km", "GefahreneKilometer" },
                                                    { "KFTEXT", "Klaerfall" },
                                                    { "EXTENSION2", "Ansprechpartner" },
                                                    { "Telnr_Long", "Telefon" },
                                                    { "Smtp_Addr", "Email" },
                                                    { "Zzfahrg", "FahrgestellNr" },
                                                    { "Kftext", "KlaerFall" },
                                                    { "Name1", "Kundenberater" },
                                                }));
            }
        }

        
        // ReSharper disable InconsistentNaming
        public static ModelMapping<Z_V_Ueberf_Auftr_Kund_Port.T_SELECT, HistoryAuftragFilter> Z_V_Ueberf_Auftr_Kund_Port_T_SELECT_To_HistoryAuftragFilter
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_V_Ueberf_Auftr_Kund_Port.T_SELECT, HistoryAuftragFilter>(
                                                 new Dictionary<string, string> {
                                                     { "Aufnr", "AuftragsNr" },
                                                     { "Erdat", "ErfassungsDatumVon" },
                                                     { "Erdat_Bis", "ErfassungsDatumBis" },
                                                     { "Vdatu", "UeberfuehrungsDatumVon" },
                                                     { "Vdatu_Bis", "UeberfuehrungsDatumBis" },
                                                     { "Zzrefnr", "Referenz" },
                                                     { "Zzkenn", "Kennzeichen" },
                                                     { "Kunnr_Ag", "KundenNr" },
                                                     { "EX_KUNNR", "KundenReferenz" },
                                                     { "Zorgadmin", "AlleOrganisationen" },
                                                     { "Wbstk", "AuftragsArt" },
                                                 }));
            }
        }


        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_DPM_CD_READ_GRUEQUIDAT_02.GT_OUT, EquiGrunddaten> Z_DPM_CD_READ_GRUEQUIDAT_02_GT_OUT_To_GrunddatenEqui
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_CD_READ_GRUEQUIDAT_02.GT_OUT, EquiGrunddaten>(
                    new Dictionary<string, string> {
                        //{ "FIN", "Fahrgestellnummer" },
                        //{ "FIN_10", "Fahrgestellnummer10" },
                        //{ "ERDAT", "Erfassungsdatum" },
                        //{ "LICENSE_NUM", "Kennzeichen" },
                        //{ "TIDNR", "TechnIdentnummer" },
                        //{ "REPLA_DATE", "Erstzulassungsdatum" },
                        //{ "EXPIRY_DATE", "Abmeldedatum" },
                        //{ "BETRIEB", "Betrieb" },
                        //{ "STORT", "Standort" },
                        //{ "STORT_TEXT", "StandortBez" },
                        //{ "ZZCOCKZ", "CocVorhanden" },
                        //{ "ZZEDCOC", "EingangCoc" },
                        //{ "ZZHANDELSNAME", "Handelsname" },
                        //{ "ZIELORT", "Zielort" },
                    }, (s, d) =>
                        {
                            d.Fahrgestellnummer = s.FIN;
                            d.Fahrgestellnummer10 = s.FIN_10;
                            d.Erfassungsdatum = s.ERDAT;
                            d.Kennzeichen = s.LICENSE_NUM;
                            d.TechnIdentnummer = s.TIDNR;
                            d.Erstzulassungsdatum = s.REPLA_DATE;
                            d.Abmeldedatum = s.EXPIRY_DATE;
                            d.Betrieb = s.BETRIEB;
                            d.Standort = s.STORT;
                            d.StandortBez = s.STORT_TEXT;
                            d.CocVorhanden = (s.ZZCOCKZ.NotNullOrEmpty().ToUpper() == "X");
                            d.EingangCoc = s.ZZEDCOC;
                            d.Handelsname = s.ZZHANDELSNAME;
                            d.Zielort = s.ZIELORT;
                        }));
            }
        }
    }
}
