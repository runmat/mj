using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SapORM.Models;

namespace SoapRuecklaeuferschnittstelle
{
    public static class RuecklaeuferschnittstelleUtils
    {
        internal const string TimestampDateFormat = "yyyy-MM-ddTHH:mm:ss.fffzzz";
        internal const string DateFormat = "yyyy-MM-dd";

        internal const string ErrorAuthentification = "11";

        internal const string StammdatenNichtAngegeben = "02";

        internal static Ruecklaeuferschnittstelle RuecklaeuferschnittstelleFromSap(IEnumerable<Z_DPM_IMP_DAT_RUECKL_01.GT_DAT> gtdatExportList, List<Z_DPM_IMP_DAT_RUECKL_01.GT_REP> gtReps, List<Z_DPM_IMP_DAT_RUECKL_01.GT_TRANSP> gtTransps)
        {

            #region Zusammenbauen des Rückgabewertes

            #region Stammdaten
            var stammdaten = gtdatExportList.First();

            Ruecklaeuferschnittstelle toReturn = new Ruecklaeuferschnittstelle
                {
                    vorgangsid = long.Parse(stammdaten.VORGANGS_ID),
                    fehlercode = stammdaten.FEHLERCODE
                };

            if (toReturn.fehlercode != "0")
            {
                {
                    return toReturn;
                }
            }

            toReturn.zuletzt_aktualisiert = DateTime.ParseExact(stammdaten.STD_ZULETZT_AKTUALISIERT, TimestampDateFormat, null);

            var toreturnStammdaten = new RuecklaeuferschnittstelleStammdaten();

            toreturnStammdaten.Rueckgabeoption = stammdaten.RUECKGAB_OPTION;
            toreturnStammdaten.Kundennummer = stammdaten.KUNNR_HLA;
            toreturnStammdaten.Kundenname = stammdaten.KUNNAME_HLA;
            toreturnStammdaten.Nutzer = stammdaten.NUTZER;
            toreturnStammdaten.Vertragsnummer = stammdaten.VERTRAGSNR_HLA;
            toreturnStammdaten.Kennzeichen = stammdaten.LICENSE_NUM;
            toreturnStammdaten.Fahrgestellnummer = stammdaten.CHASSIS_NUM;
            toreturnStammdaten.Erstzulassung = SapDatumToDatetime(stammdaten.ERST_ZUL_DAT);
            toreturnStammdaten.Hersteller = stammdaten.HERST;
            toreturnStammdaten.Serie = stammdaten.SERIE;
            toreturnStammdaten.Treibstoffart = stammdaten.KRAFTSTOFF;
            toreturnStammdaten.Aufbauart = stammdaten.AUFBAU;
            toreturnStammdaten.Leistung = stammdaten.LEISTUNG;
            toreturnStammdaten.Winterreifen = stammdaten.WINTERREIFEN;

            toReturn.Stammdaten = toreturnStammdaten;
            
            #endregion

            #region Abholauftrag zurückschreiben

            // Nur wenn der Abholaufrag angegeben ist auffüllen für die Rückgabe
            if (string.IsNullOrEmpty(stammdaten.ABH_ZULETZT_AKTUALISIERT) == false)
            {
                toReturn.Abholung = new RuecklaeuferschnittstelleAbholung();
                toReturn.Abholung.Abholauftrag = new RuecklaeuferschnittstelleAbholungAbholauftrag();

                // Abholauftrag
                toReturn.Abholung.Abholauftrag.Transportart = stammdaten.TRANSPORTART;
                toReturn.Abholung.Abholauftrag.Datum_von = SapDatumToDatetime(stammdaten.WLIEFDAT_VON);
                toReturn.Abholung.Abholauftrag.Datum_bis = SapDatumToDatetime(stammdaten.WLIEFDAT_BIS);
                toReturn.Abholung.Abholauftrag.Bemerkung = stammdaten.BEM;
                toReturn.Abholung.Abholauftrag.zuletzt_aktualisiert = DateTime.ParseExact(stammdaten.ABH_ZULETZT_AKTUALISIERT,
                                                                                          TimestampDateFormat, null);
                toReturn.Abholung.Abholauftrag.Eigenanlieferung = TrueFalseToBool(stammdaten.EIG_ANL);
                toReturn.Abholung.Abholauftrag.Gutachten_erstellen = TrueFalseToBool(stammdaten.GUTA_ERSTELLEN);

                // Abholort
                toReturn.Abholung.Abholauftrag.Adressen = new RuecklaeuferschnittstelleAbholungAbholauftragAdressen();
                toReturn.Abholung.Abholauftrag.Adressen.Abholort =
                    new RuecklaeuferschnittstelleAbholungAbholauftragAdressenAbholort();
                toReturn.Abholung.Abholauftrag.Adressen.Abholort.Firma = stammdaten.NAME_ABH;
                toReturn.Abholung.Abholauftrag.Adressen.Abholort.Ansprechpartner = stammdaten.NAME_2_ABH;
                toReturn.Abholung.Abholauftrag.Adressen.Abholort.Strasse = stammdaten.STREET_ABH;
                toReturn.Abholung.Abholauftrag.Adressen.Abholort.Postleitzahl = stammdaten.POSTL_CODE_ABH;
                toReturn.Abholung.Abholauftrag.Adressen.Abholort.Ort = stammdaten.CITY_ABH;
                toReturn.Abholung.Abholauftrag.Adressen.Abholort.Ansprechpartner = stammdaten.NAME_2_ABH;
                toReturn.Abholung.Abholauftrag.Adressen.Abholort.Telefon = stammdaten.TELEPHONE_ABH;
                toReturn.Abholung.Abholauftrag.Adressen.Abholort.Email = stammdaten.SMTP_ADDR_ABH;

                // Zielort
                toReturn.Abholung.Abholauftrag.Adressen.Zielort =
                    new RuecklaeuferschnittstelleAbholungAbholauftragAdressenZielort();
                toReturn.Abholung.Abholauftrag.Adressen.Zielort.Firma = stammdaten.NAME_ZI;
                toReturn.Abholung.Abholauftrag.Adressen.Zielort.Ansprechpartner = stammdaten.NAME_2_ZI;
                toReturn.Abholung.Abholauftrag.Adressen.Zielort.Strasse = stammdaten.STREET_ZI;
                toReturn.Abholung.Abholauftrag.Adressen.Zielort.Postleitzahl = stammdaten.POSTL_CODE_ZI;
                toReturn.Abholung.Abholauftrag.Adressen.Zielort.Ort = stammdaten.CITY_ZI;
                toReturn.Abholung.Abholauftrag.Adressen.Zielort.Ansprechpartner = stammdaten.NAME_2_ZI;
                toReturn.Abholung.Abholauftrag.Adressen.Zielort.Telefon = stammdaten.TELEPHONE_ZI;
                toReturn.Abholung.Abholauftrag.Adressen.Zielort.Email = stammdaten.SMTP_ADDR_ZI;

                // Abholtermin
                if (string.IsNullOrEmpty(stammdaten.ABHT_ZULETZT_AKTUALISIERT) == false)
                {
                    toReturn.Abholung.Abholtermin_bestaetigt = new RuecklaeuferschnittstelleAbholungAbholtermin_bestaetigt();
                    toReturn.Abholung.Abholtermin_bestaetigt.zuletzt_aktualisiert =
                        DateTime.ParseExact(stammdaten.ABHT_ZULETZT_AKTUALISIERT, TimestampDateFormat, null);
                    toReturn.Abholung.Abholtermin_bestaetigt.Bestaetigter_Abholtermin =
                        SapDatumToDatetime(stammdaten.BEST_ABH_TERMIN);
                }

                if (string.IsNullOrEmpty(stammdaten.PROT_ZULETZT_AKTUALISIERT) == false)
                {
                    toReturn.Abholung.Eingang_Zielort = new RuecklaeuferschnittstelleAbholungEingang_Zielort();
                    toReturn.Abholung.Eingang_Zielort.zuletzt_aktualisiert = DateTime.ParseExact(stammdaten.PROT_ZULETZT_AKTUALISIERT, TimestampDateFormat, null);
                    toReturn.Abholung.Eingang_Zielort.Datum = SapDatumToDatetime(stammdaten.EING_DAT_PROT);
                    toReturn.Abholung.Eingang_Zielort.Firma = stammdaten.NAME_ZI;
                    toReturn.Abholung.Eingang_Zielort.Ansprechpartner = stammdaten.NAME_2_ZI;
                    toReturn.Abholung.Eingang_Zielort.Strasse = stammdaten.STREET_ZI;
                    toReturn.Abholung.Eingang_Zielort.Postleitzahl = stammdaten.POSTL_CODE_ZI;
                    toReturn.Abholung.Eingang_Zielort.Ort = stammdaten.CITY_ZI;
                    toReturn.Abholung.Eingang_Zielort.Ansprechpartner = stammdaten.NAME_2_ZI;
                    toReturn.Abholung.Eingang_Zielort.Telefon = stammdaten.TELEPHONE_ZI;
                    toReturn.Abholung.Eingang_Zielort.Email = stammdaten.SMTP_ADDR_ZI;
                    toReturn.Abholung.Eingang_Zielort.Protokoll_Abholung = stammdaten.PROT_EING_NAMEO;
                    toReturn.Abholung.Eingang_Zielort.Protokoll_Zielort = stammdaten.PROT_EING_NAMED;
                }
            }

            #endregion

            #region Aufbereitung und Reparaturen zurückshreiben

            if (gtReps.Any())
            {
                RuecklaeuferschnittstelleAufbereitung aufbereitung = new RuecklaeuferschnittstelleAufbereitung();
                RuecklaeuferschnittstelleAufbereitungAufbereitungsauftrag aufbereitungsauftrag =
                    new RuecklaeuferschnittstelleAufbereitungAufbereitungsauftrag();

                aufbereitungsauftrag.zuletzt_aktualisiert = DateTime.ParseExact(stammdaten.AUFB_ZULETZT_AKTUALISIERT, TimestampDateFormat, null);
                aufbereitungsauftrag.Datum = DateTime.ParseExact(stammdaten.EING_AUFBER_AUFTR, DateFormat, null);
                aufbereitungsauftrag.Bemerkung = stammdaten.BEM_AUFB;

                aufbereitung.Aufbereitungsauftrag = aufbereitungsauftrag;

                List<RuecklaeuferschnittstelleAufbereitungAufbereitungsauftragPosition> reparaturList =
                    new List<RuecklaeuferschnittstelleAufbereitungAufbereitungsauftragPosition>();

                foreach (var gtRep in gtReps)
                {
                    RuecklaeuferschnittstelleAufbereitungAufbereitungsauftragPosition reparatur = new RuecklaeuferschnittstelleAufbereitungAufbereitungsauftragPosition();

                    reparatur.Bezeichnung = gtRep.BEZEICHNUNG;
                    reparatur.Code = gtRep.CODE;
                    reparatur.Massnahme = gtRep.MASSNAHME;
                    reparatur.Reparaturkosten = decimal.Parse(gtRep.REP_KOSTEN);
                    reparatur.Typ = gtRep.TYP;

                    reparaturList.Add(reparatur);
                }

                aufbereitung.Aufbereitungsauftrag.Reparaturen = reparaturList.ToArray();

                if (string.IsNullOrEmpty(stammdaten.AUFBER_FERTIG) == false)
                {
                    RuecklaeuferschnittstelleAufbereitungAufbereitung_erfolgt aufbereitungErfolgt = new RuecklaeuferschnittstelleAufbereitungAufbereitung_erfolgt();
                    aufbereitungErfolgt.Aufbereitet_am = SapDatumToDatetime(stammdaten.AUFBER_FERTIG);
                    aufbereitungErfolgt.zuletzt_aktualisiert = DateTime.ParseExact(stammdaten.AUBF_ZULETZT_AKTUALISIERT, TimestampDateFormat, null);

                    aufbereitung.Aufbereitung_erfolgt = aufbereitungErfolgt;
                }

                toReturn.Aufbereitung = aufbereitung;
            }

            #endregion

            #region Verwertung

            if (string.IsNullOrEmpty(stammdaten.VERW_ZULETZT_AKTUALISIERT) == false)
            {
                toReturn.Verwertung = new RuecklaeuferschnittstelleVerwertung();
                toReturn.Verwertung.Verwertungsentscheidung = new RuecklaeuferschnittstelleVerwertungVerwertungsentscheidung();

                toReturn.Verwertung.Verwertungsentscheidung.zuletzt_aktualisiert = DateTime.ParseExact(stammdaten.VERW_ZULETZT_AKTUALISIERT, TimestampDateFormat, null);
                toReturn.Verwertung.Verwertungsentscheidung.Entscheidung = stammdaten.ENTSCHEIDUNG;
                toReturn.Verwertung.Verwertungsentscheidung.Verkaufskanal = stammdaten.VERKAUFSKANAL;

                if (string.IsNullOrEmpty(stammdaten.EING_ZULETZT_AKTUALISIERT) == false)
                {
                    RuecklaeuferschnittstelleVerwertungFahrzeug_eingestellt fahrzeugEingestellt = new RuecklaeuferschnittstelleVerwertungFahrzeug_eingestellt();
                    fahrzeugEingestellt.Angebotsnummer = stammdaten.ANGEBOTSNR;
                    fahrzeugEingestellt.link = stammdaten.LINK;
                    fahrzeugEingestellt.zuletzt_aktualisiert = DateTime.ParseExact(stammdaten.EING_ZULETZT_AKTUALISIERT, TimestampDateFormat, null);

                    toReturn.Verwertung.Fahrzeug_eingestellt = fahrzeugEingestellt;                    
                }
            }

            #endregion

            #region Transport

            if (gtTransps.Any())
            {
                List<RuecklaeuferschnittstelleTransport> transports = new List<RuecklaeuferschnittstelleTransport>();
                foreach (var transport in gtTransps)
                {
                    RuecklaeuferschnittstelleTransport toReturnTransport = new RuecklaeuferschnittstelleTransport
                        {
                            Transportauftrag = new RuecklaeuferschnittstelleTransportTransportauftrag()
                        };

                    toReturnTransport.Transportauftrag.zuletzt_aktualisiert = DateTime.ParseExact(transport.ZULETZT_AKTUALISIERT, TimestampDateFormat, null);
                    toReturnTransport.Transportauftrag.Transportart = transport.TRANSPORTART;
                    toReturnTransport.Transportauftrag.Datum_von = SapDatumToDatetime(transport.WLIEFDAT_VON);
                    toReturnTransport.Transportauftrag.Datum_bis = SapDatumToDatetime(transport.WLIEFDAT_BIS);
                    toReturnTransport.Transportauftrag.Bemerkung = transport.BEM;

                    toReturnTransport.Transportauftrag.Adressen = new RuecklaeuferschnittstelleTransportTransportauftragAdressen();
                    toReturnTransport.Transportauftrag.Adressen.Abholort = new RuecklaeuferschnittstelleTransportTransportauftragAdressenAbholort();
                    toReturnTransport.Transportauftrag.Adressen.Zielort = new RuecklaeuferschnittstelleTransportTransportauftragAdressenZielort();

                    toReturnTransport.Transportauftrag.Adressen.Abholort.Firma = transport.NAME_ABH;
                    toReturnTransport.Transportauftrag.Adressen.Abholort.Postleitzahl = transport.POSTL_CODE_ABH;
                    toReturnTransport.Transportauftrag.Adressen.Abholort.Ort = transport.CITY_ABH;
                    toReturnTransport.Transportauftrag.Adressen.Abholort.Ansprechpartner = transport.NAME_2_ABH;
                    toReturnTransport.Transportauftrag.Adressen.Abholort.Telefon = transport.TELEPHONE_ABH;
                    toReturnTransport.Transportauftrag.Adressen.Abholort.Strasse = transport.STREET_ABH;
                    toReturnTransport.Transportauftrag.Adressen.Abholort.Email = transport.SMTP_ADDR_ABH;

                    toReturnTransport.Transportauftrag.Adressen.Zielort.Firma = transport.NAME_ZI;
                    toReturnTransport.Transportauftrag.Adressen.Zielort.Postleitzahl = transport.POSTL_CODE_ZI;
                    toReturnTransport.Transportauftrag.Adressen.Zielort.Ort = transport.CITY_ZI;
                    toReturnTransport.Transportauftrag.Adressen.Zielort.Ansprechpartner = transport.NAME_2_ZI;
                    toReturnTransport.Transportauftrag.Adressen.Zielort.Telefon = transport.TELEPHONE_ZI;
                    toReturnTransport.Transportauftrag.Adressen.Zielort.Strasse = transport.STREET_ZI;
                    toReturnTransport.Transportauftrag.Adressen.Zielort.Email = transport.SMTP_ADDR_ZI;
                    
                    if (string.IsNullOrEmpty(transport.BEST_ABH_TERMIN))
                    {
                        RuecklaeuferschnittstelleTransportAbholtermin_bestaetigt abholterminBestaetigt = new RuecklaeuferschnittstelleTransportAbholtermin_bestaetigt();
                        abholterminBestaetigt.Bestaetigter_Abholtermin = SapDatumToDatetime(transport.BEST_ABH_TERMIN);
                        abholterminBestaetigt.zuletzt_aktualisiert = DateTime.ParseExact(transport.ABHT_ZULETZT_AKTUALISIERT, TimestampDateFormat, null);

                        toReturnTransport.Abholtermin_bestaetigt = abholterminBestaetigt;
                    }

                    if (string.IsNullOrEmpty(transport.EIZO_ZULETZT_AKTUALISIERT))
                    {
                        RuecklaeuferschnittstelleTransportEingang_Zielort transportEingangZielort = new RuecklaeuferschnittstelleTransportEingang_Zielort();
                        transportEingangZielort.zuletzt_aktualisiert = DateTime.ParseExact(transport.EIZO_ZULETZT_AKTUALISIERT, TimestampDateFormat, null);
                        transportEingangZielort.Datum = SapDatumToDatetime(transport.DAT_EING_ZO);
                        transportEingangZielort.Firma = transport.NAME_ZI;
                        transportEingangZielort.Postleitzahl = transport.POSTL_CODE_ZI;
                        transportEingangZielort.Ort = transport.CITY_ZI;
                        transportEingangZielort.Ansprechpartner = transport.NAME_2_ZI;
                        transportEingangZielort.Telefon = transport.TELEPHONE_ZI;
                        transportEingangZielort.Strasse = transport.STREET_ZI;
                        transportEingangZielort.Email = transport.SMTP_ADDR_ZI;
                        transportEingangZielort.Protokoll_Abholung = transport.PROT_EING_NAMEO;
                        transportEingangZielort.Protokoll_Zielort = transport.PROT_EING_NAMED;

                        toReturnTransport.Eingang_Zielort = transportEingangZielort;
                    }

                    transports.Add(toReturnTransport);
                }

                toReturn.Transport = transports.ToArray();
            }

            #endregion

            #region Rueckgabeprotokoll

            if (string.IsNullOrEmpty(stammdaten.EING_DAT_PROT) == false)
            {
                var rueckgabeprotokoll = new RuecklaeuferschnittstelleRueckgabeprotokoll();
                rueckgabeprotokoll.zuletzt_aktualisiert = DateTime.ParseExact(stammdaten.PROT_ZULETZT_AKTUALISIERT, TimestampDateFormat, null);
                rueckgabeprotokoll.Eingangsdatum = SapDatumToDatetime(stammdaten.EING_DAT_PROT); 
                rueckgabeprotokoll.Ruecknahmedatum = SapDatumToDatetime(stammdaten.RUECKNAHMEDATUM); 

                rueckgabeprotokoll.Ruecknahmezeit = stammdaten.RUECKNAHMEZEIT; 

                // Wir nehmen die Daten aus den Stammdaten da die entsprechenden Felder aus SAP leer kommen
                // richtiger wäre es aber wenn SAP diese Felder füllen würde
                // rueckgabeprotokoll.Ruecknahmeort = stammdaten.RUECKNAHMEORT; 
                // rueckgabeprotokoll.Standort = stammdaten.STANDORT;
                rueckgabeprotokoll.Ruecknahmeort = stammdaten.CITY_ABH; 
                rueckgabeprotokoll.Standort = stammdaten.CITY_ZI;

                rueckgabeprotokoll.KMstand_Ruecknahme = stammdaten.KM_BEI_UEBERNAHM; 
                rueckgabeprotokoll.KMstand_nachTransfer = stammdaten.KM_BEI_UEBERG;
                rueckgabeprotokoll.ZB1 = SapNumericToBool(stammdaten.ZB1_SCHGEIN); 
                rueckgabeprotokoll.Serviceheft = SapNumericToBool(stammdaten.SERVHEFT_BORDBU); 
                rueckgabeprotokoll.RadioCodeKarte = SapNumericToBool(stammdaten.RADIO_CODCARD_NR); 
                rueckgabeprotokoll.NaviCD_DVD = SapNumericToBool(stammdaten.ORIG_NAVI_DVD_CD); 
                rueckgabeprotokoll.Bordwerkzeug = SapNumericToBool(stammdaten.BORDWERKZEUG); 
                rueckgabeprotokoll.Reserverad = SapNumericToBool(stammdaten.RESERVERAD); 
                rueckgabeprotokoll.ReifenReparaturSet = SapNumericToBool(stammdaten.REIF_REP_SET); 
                rueckgabeprotokoll.Laderaumabdeckung = SapNumericToBool(stammdaten.LADERAUMABDECK); 
                rueckgabeprotokoll.Ruecknahme_unter_Vorbehalt_Grund = stammdaten.RUECKN_VORBEH; 
                rueckgabeprotokoll.Ruecknahmeschaeden_innen = SapNumericToBool(stammdaten.RUE_SCHAED_I); 
                rueckgabeprotokoll.Ruecknahmeschaeden_aussen = SapNumericToBool(stammdaten.RUE_SCHAED_A); 
                rueckgabeprotokoll.Bemerkung = stammdaten.BEMERKUNG;
                rueckgabeprotokoll.Protokoll_Abholung = stammdaten.PROT_EING_NAMEO;

                toReturn.Rueckgabeprotokoll = rueckgabeprotokoll;
            }

            #endregion

            #region Stilllegung

            if (string.IsNullOrEmpty(stammdaten.ABMELDEDATUM) == false)
            {
                toReturn.Stilllegung_erfolgt = new RuecklaeuferschnittstelleStilllegung_erfolgt();
                toReturn.Stilllegung_erfolgt.Stilllegungstermin = SapDatumToDatetime(stammdaten.ABMELDEDATUM);
                toReturn.Stilllegung_erfolgt.zuletzt_aktualisiert = DateTime.ParseExact(stammdaten.STIL_ZULETZT_AKTUALISIERT, TimestampDateFormat, null);

                //var stilllegungerfolgt = new RuecklaeuferschnittstelleStilllegung_erfolgt();
                //stilllegungerfolgt.Stilllegungstermin = SapDatumToDatetime(stammdaten.ABMELDEDATUM);
                //stilllegungerfolgt.zuletzt_aktualisiert = DateTime.ParseExact(stammdaten.STIL_ZULETZT_AKTUALISIERT, TimestampDateFormat, null);
            }

            #endregion

            #region ProzessEnde

            if (string.IsNullOrEmpty(stammdaten.VABG_ZULETZT_AKTUALISIERT) == false)
            {
                toReturn.Prozessende = new RuecklaeuferschnittstelleProzessende();
                toReturn.Prozessende.vorgang_abgeschlossen = stammdaten.VORG_ABGESCHL;
                toReturn.Prozessende.zuletzt_aktualisiert = DateTime.ParseExact(stammdaten.VABG_ZULETZT_AKTUALISIERT,
                                                                                TimestampDateFormat, null);
            }

            #endregion

            #endregion

            return toReturn;
        }

        internal static bool SapNumericToBool(string value)
        {
            if (value == "1")
            {
                return true;
            }

            if (value == "0")
            {
                return false;
            }

            // Arno und ich wissen im Moment nicht was alles im Feld vorhanden sein kkann
            return false;
        }

        internal static string BoolToTrueFalse(bool value)
        {
            return value ? "true" : "false";
        }

        internal static bool TrueFalseToBool(string value)
        {
            if (value.ToUpper() == "true".ToUpper())
            {
                return true;
            }

            return false;
        }

        internal static bool Authenticate(string anmeldename, string passwort)
        {
            return "lease_motion" == anmeldename && "58Rt%!dR" == passwort;
        }

        #region Umwandlungsfunktionen für Datetime

        internal static string DatumDateTimeToSap(DateTime date)
        {
            if (date == DateTime.MinValue)
            {
                return string.Empty;
            }

            return date.ToString(DateFormat);
        }

        internal static DateTime SapDatumToDatetime(string date)
        {
            if (string.IsNullOrEmpty(date))
            {
                return DateTime.MinValue;
            }

            return DateTime.ParseExact(date, DateFormat, null);
        }

        #endregion

    }
}