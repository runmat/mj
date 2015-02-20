// ReSharper disable InconsistentNaming
// ReSharper disable RedundantUsingDirective

using System;
using System.Collections.Generic;
using CkgDomainLogic.Insurance.Models;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;

namespace CkgDomainLogic.Insurance.Models
{
    public class AppModelMappings : ModelMappings
    {
        /// <summary>
        /// Events
        /// </summary>
        static public ModelMapping<Z_DPM_TAB_ZEVENT_KONFIG_01.GT_EVENT, VersEvent> Z_DPM_TAB_ZEVENT_KONFIG_01_GT_EVENT_To_VersEvent
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_TAB_ZEVENT_KONFIG_01.GT_EVENT, VersEvent>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.ID = s.EVENT.ToInt();

                        d.EventName = s.EVENT_NAME;
                        d.Region = s.REGION;
                        d.Kategorie = s.KATEGORIE;
                        d.FahrzeugVolumen = s.FAHRZEUGVOLUMEN.ToInt();
                        d.Beschreibung = s.BESCHREIBUNG;
                        d.StartDatum = s.STARTDATUM.GetValueOrDefault();
                        d.EndDatum = s.ENDDATUM.GetValueOrDefault();

                        d.AnlageDatum = s.ANLAGEDATUM.GetValueOrDefault();
                        d.AnlageUser = s.ANLAGEUSER;
                        d.LoeschDatum = s.LOESCHDATUM;
                        d.LoeschUser = s.LOESCHUSER;
                    }
                    , (s, d) =>
                    {
                        d.EVENT = s.ID.ToString().PadLeft0(10);

                        d.EVENT_NAME = s.EventName;
                        d.REGION = s.Region;
                        d.KATEGORIE = s.Kategorie;
                        d.FAHRZEUGVOLUMEN = s.FahrzeugVolumen.ToString().PadLeft0(6);
                        d.BESCHREIBUNG = s.Beschreibung;
                        d.STARTDATUM = s.StartDatum;
                        d.ENDDATUM = s.EndDatum;

                        d.ANLAGEDATUM = s.AnlageDatum;
                        d.ANLAGEUSER = s.AnlageUser;
                        d.LOESCHDATUM = s.LoeschDatum;
                        d.LOESCHUSER = s.LoeschUser;
                    }
                ));
            }
        }

        /// <summary>
        /// Orte
        /// </summary>
        static public ModelMapping<Z_DPM_TAB_ZEVENT_KONFIG_02.GT_ORT, VersEventOrt> Z_DPM_TAB_ZEVENT_KONFIG_02_GT_ORT_To_VersEventOrt
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_TAB_ZEVENT_KONFIG_02.GT_ORT, VersEventOrt>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.ID = s.EVENT_ORT.ToInt();
                        d.VersEventID = s.EVENT.ToInt();

                        d.OrtName = s.NAME1;
                        d.OrtName2 = s.NAME2;
                        d.Strasse = s.STREET;
                        d.HausNr = s.HOUSE_NUM1;
                        d.PLZ = s.POST_CODE2;
                        d.Ort = s.CITY1;
                        d.Land = s.LAND;
                        d.StartZeitMoFr = HhMmFromSap(s.STARTZEITMOFR);
                        d.EndZeitMoFr = HhMmFromSap(s.ENDZEITMOFR);
                        d.StartZeitSa = HhMmFromSap(s.STARTZEITSA);
                        d.EndZeitSa = HhMmFromSap(s.ENDZEITSA);

                        d.LoeschDatum = s.LOESCHDATUM;
                        d.LoeschUser = s.LOESCHUSER;
                    }
                    , (s, d) =>
                    {
                        d.EVENT_ORT = s.ID.ToString().PadLeft0(10);
                        d.EVENT = s.VersEventID.ToString().PadLeft0(10);
                        
                        d.NAME1 = s.OrtName;
                        d.NAME2 = s.OrtName2;
                        d.STREET = s.Strasse;
                        d.HOUSE_NUM1 = s.HausNr;
                        d.POST_CODE2 = s.PLZ;
                        d.CITY1 = s.Ort;
                        d.LAND = s.Land;
                        d.STARTZEITMOFR = HhMmToSap(s.StartZeitMoFr);
                        d.ENDZEITMOFR = HhMmToSap(s.EndZeitMoFr);
                        d.STARTZEITSA = HhMmToSap(s.StartZeitSa);
                        d.ENDZEITSA = HhMmToSap(s.EndZeitSa);
                        
                        d.LOESCHDATUM = s.LoeschDatum;
                        d.LOESCHUSER = s.LoeschUser;
                    }
                ));
            }
        }

        /// <summary>
        /// Boxen
        /// </summary>
        static public ModelMapping<Z_DPM_TAB_ZEVENT_KONFIG_03.GT_BOX, VersEventOrtBox> Z_DPM_TAB_ZEVENT_KONFIG_03_GT_BOX_To_VersEventOrtBox
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_TAB_ZEVENT_KONFIG_03.GT_BOX, VersEventOrtBox>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.ID = s.EVENT_ORTBOX.ToInt();
                        d.VersOrtID = s.EVENT_ORT.ToInt();
                        //d.VersEventID = s.EVENT.ToInt();

                        d.BoxNr = s.BOXNR.ToInt();
                        d.BoxName = s.BOXNAME;
                        d.BoxArt = s.BOXART;
                        d.VersicherungID = s.VERSICH_ID;
                        d.TechnikerName = s.TECHNIKERNAME;
                        d.TaktungMinuten = s.TAKTUNGMINUTEN.ToInt();

                        d.LoeschDatum = s.LOESCHDATUM;
                        d.LoeschUser = s.LOESCHUSER;
                    }
                    , (s, d) =>
                    {
                        d.EVENT_ORTBOX = s.ID.ToString().PadLeft0(10);
                        d.EVENT_ORT = s.VersOrtID.ToString().PadLeft0(10);
                        //d.EVENT = s.VersEventID.ToString().PadLeft0(10);

                        d.BOXNR = s.BoxNr.ToString().PadLeft0(10);
                        d.BOXNAME = s.BoxName;
                        d.BOXART = s.BoxArt;
                        d.VERSICH_ID = s.VersicherungID;
                        d.TECHNIKERNAME = s.TechnikerName;
                        d.TAKTUNGMINUTEN = s.TaktungMinuten.ToString().PadLeft0(6);
                        
                        d.LOESCHDATUM = s.LoeschDatum;
                        d.LOESCHUSER = s.LoeschUser;
                    }
                ));
            }
        }

        /// <summary>
        /// Termine pro Schadenfall
        /// </summary>
        static public ModelMapping<Z_DPM_TAB_ZEVENT_TERMIN_01.GT_TERMIN, TerminSchadenfall> Z_DPM_TAB_ZEVENT_TERMIN_01_GT_TERMIN_To_TerminSchadenfall
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_TAB_ZEVENT_TERMIN_01.GT_TERMIN, TerminSchadenfall>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.ID = s.EVENT_TERMIN.ToInt();
                        d.VersBoxID = s.EVENT_ORTBOX.ToInt();
                        d.VersOrtID = s.EVENT_ORT.ToInt();
                        
                        d.VersSchadenfallID = s.EVENT_SCHADEN.ToInt();

                        d.Datum = s.DATUM_VON.GetValueOrDefault();
                        d.ZeitVon = HhMmFromSap(s.ZEIT_VON);
                        d.ZeitBis = HhMmFromSap(s.ZEIT_BIS);
                        d.Bemerkung = s.TEXT;

                        d.AnlageDatum = s.ANLAGEDATUM.GetValueOrDefault();
                        d.AnlageUser = s.ANLAGEUSER;
                        d.LoeschDatum = s.LOESCHDATUM;
                        d.LoeschUser = s.LOESCHUSER;
                    }
                    , (s, d) =>
                    {
                        d.EVENT_TERMIN = s.ID.ToString().PadLeft0(10);
                        d.EVENT_ORTBOX = s.VersBoxID.ToString().PadLeft0(10);
                        d.EVENT_ORT = s.VersOrtID.ToString().PadLeft0(10);
                        
                        d.EVENT_SCHADEN = s.VersSchadenfallID.ToString().PadLeft0(10);

                        d.DATUM_VON = s.Datum;
                        d.ZEIT_VON = HhMmToSap(s.ZeitVon);
                        d.ZEIT_BIS = HhMmToSap(s.ZeitBis);
                        d.TEXT = s.Bemerkung;

                        d.ANLAGEDATUM = s.AnlageDatum;
                        d.ANLAGEUSER = s.AnlageUser;
                        d.LOESCHDATUM = s.LoeschDatum;
                        d.LOESCHUSER = s.LoeschUser;
                    }
                ));
            }
        }

        /// <summary>
        /// Schadenfälle
        /// </summary>
        static public ModelMapping<Z_DPM_TAB_ZEVENT_SCHADEN_01.GT_SCHADEN, Schadenfall> Z_DPM_TAB_ZEVENT_SCHADEN_01_GT_SCHADEN_To_Schadenfall
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_TAB_ZEVENT_SCHADEN_01.GT_SCHADEN, Schadenfall>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.ID = s.EVENT_SCHADEN.ToInt();
                        d.EventID = s.EVENT.ToInt();

                        d.Kennzeichen = s.LICENSE_NUM;
                        d.Nachname = s.NAME_LAST;
                        d.Vorname = s.NAME_FIRST;
                        d.Email = s.SMTP_ADDR;
                        d.TelefonNr = s.TELNR_LONG;
                        d.FzgHersteller = s.FZGMARKE;
                        d.FzgModell = s.FZGTYP;
                        d.VersicherungID = s.VERSICH_ID;
                        d.SelbstbeteiligungsHoehe = s.SBHOEHE;
                        d.Sammelbesichtigung = (s.SAMMELBES.NotNullOrEmpty() == "X");
                        d.Referenznummer = s.REFNR;

                        d.AnlageDatum = s.ANLAGEDATUM.GetValueOrDefault();
                        d.AnlageUser = s.ANLAGEUSER;
                        d.LoeschDatum = s.LOESCHDATUM;
                        d.LoeschUser = s.LOESCHUSER;
                    }
                    , (s, d) =>
                    {
                        d.EVENT_SCHADEN = s.ID.ToString().PadLeft0(10);
                        d.EVENT = s.EventID.ToString().PadLeft0(10);

                        d.LICENSE_NUM = s.Kennzeichen;
                        d.NAME_LAST = s.Nachname;
                        d.NAME_FIRST = s.Vorname;
                        d.SMTP_ADDR = s.Email;
                        d.TELNR_LONG = s.TelefonNr;
                        d.FZGMARKE = s.FzgHersteller;
                        d.FZGTYP = s.FzgModell;
                        d.VERSICH_ID = s.VersicherungID;
                        d.SBHOEHE = s.SelbstbeteiligungsHoehe;
                        d.SAMMELBES = (s.Sammelbesichtigung ? "X" : "");
                        d.REFNR = s.Referenznummer;

                        d.ANLAGEDATUM = s.AnlageDatum;
                        d.ANLAGEUSER = s.AnlageUser;
                        d.LOESCHDATUM = s.LoeschDatum;
                        d.LOESCHUSER = s.LoeschUser;
                    }
                ));
            }
        }

        /// <summary>
        /// Schaden Status (read)
        /// </summary>
        static public ModelMapping<Z_DPM_EVENT_READ_SCHAD_STAT_01.GT_STATUS, SchadenfallStatus> Z_DPM_EVENT_READ_SCHAD_STAT_01_GT_STATUS_To_SchadenfallStatus
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_EVENT_READ_SCHAD_STAT_01.GT_STATUS, SchadenfallStatus>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.VersSchadenfallID = s.EVENT_SCHADEN.ToInt();
                        d.Sort = s.REIHENFOLGE.ToInt();
                        d.StatusArtID = s.STATUSART.ToInt();
                        d.Datum = s.DATUM;
                        d.Zeit = HhMmFromSap(s.UZEIT);
                        d.User = s.BENUTZER;
                        d.Kommentar = s.TEXT;
                        d.StatusFarbe = s.FARBE_STATUS;
                    }
                ));
            }
        }
        /// <summary>
        /// Schaden Status (save)
        /// </summary>
        static public ModelMapping<Z_DPM_EVENT_SET_SCHAD_STAT_01.GT_STATUS, SchadenfallStatus> Z_DPM_EVENT_SET_SCHAD_STAT_01_GT_STATUS_To_SchadenfallStatus
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_EVENT_SET_SCHAD_STAT_01.GT_STATUS, SchadenfallStatus>(
                    new Dictionary<string, string>()
                    , null
                    , (s, d) =>
                    {
                        d.EVENT_SCHADEN = s.VersSchadenfallID.ToString().PadLeft0(10);
                        d.STATUSART = s.StatusArtID.ToString().PadLeft0(10);
                        d.DATUM = s.Datum;
                        d.UZEIT = HhMmToSap(s.Zeit);
                        d.BENUTZER = s.User;
                        d.TEXT = s.Kommentar;
                    }          
                ));
            }
        }

        /// <summary>
        /// Schaden Status Art
        /// </summary>
        static public ModelMapping<Z_DPM_EVENT_READ_SCHAD_STAT_01.GT_STATART, SchadenfallStatusArt> Z_DPM_EVENT_READ_SCHAD_STAT_01_GT_STATART_To_SchadenfallStatusArt
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_EVENT_READ_SCHAD_STAT_01.GT_STATART, SchadenfallStatusArt>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.ArtID = s.STATUSART.ToInt();
                        d.ProzessNr = s.PROZESSNR;
                        d.Name = s.NAME;
                        d.Sort = s.REIHENFOLGE.ToInt();
                        d.Optional = s.OPTIONAL;
                        d.Bezeichnung = s.TEXT;
                        d.ArtFarbe = s.FARBE_STATUS;
                    }
                ));
            }
        }

        /// <summary>
        /// Bestandsdaten
        /// </summary>
        static public ModelMapping<Z_DPM_ASSIST_READ_BESTAND_01.GT_OUT, BestandsdatenModel> Z_DPM_ASSIST_READ_BESTAND_01_GT_OUT_To_BestandsdatenModel
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_ASSIST_READ_BESTAND_01.GT_OUT, BestandsdatenModel>(
                    new Dictionary<string, string>()
                    , (source, destination) =>
                    {
                        destination.Eingangsdatum = source.EINGANG;
                        destination.Name1 = source.NAME1;
                        destination.Strasse = source.STREET;
                        destination.PLZ = source.PSTLZ;
                        destination.Ort = source.ORT;
                        destination.VuNummer = source.VUNUMMER;
                        destination.VersicherungsscheinNr = source.VERSNUMMER;
                        destination.Vertragsbeginn = source.VERTRAGSBEGINN;
                        destination.Vertragsende = source.VERTRAGSABLAUF;
                        destination.Vertragsstatus = source.VERTRAGSSTATUS;
                        destination.Bedingungen = source.BEDINGUNGEN;
                        destination.Produkttyp = source.PRODUKTTYP;
                        destination.AnzahlRisiken = source.ANZ_RISIKEN;
                        destination.Kennzeichen = source.KENNZEICHEN;
                        destination.Hersteller = source.HERSTNAME;
                        destination.VIN = source.FIN;
                        destination.Erstzulassung = source.ERSTZULDAT;
                        destination.Mehrfahrzeugklausel = source.MEHRFZKLAUSEL;
                        destination.Wagniskennziffer = source.WAGNISZIFFER;
                        destination.Geltungsbereich = source.GELTUNGSBEREICH;
                        destination.Krankenversicherung = source.KVVERSICHERUNG;
                        destination.EuroVertrag = source.EURO_VTRG;
                        destination.WerkstattName = source.WKS_NAME;
                        destination.WerkstattStrasse = source.WKS_STRASSE;
                        destination.WerkstattPLZ = source.WKS_PLZ;
                        destination.WerkstattOrt = source.WKS_ORT;
                        destination.WerkstattOeffnungszeiten = source.WKS_ZEIT;
                    }
                ));
            }
        }

        /// <summary>
        /// Upload Bestandsdaten
        /// </summary>
        static public ModelMapping<Z_DPM_ASSIST_IMP_BESTAND_01.GT_IN, UploadBestandsdatenModel> Z_DPM_ASSIST_IMP_BESTAND_01_GT_IN_From_UploadBestandsdatenModel
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_ASSIST_IMP_BESTAND_01.GT_IN, UploadBestandsdatenModel>(
                    new Dictionary<string, string>()
                    , null
                    , (source, destination) =>
                        {
                            destination.LFDNR = source.DatensatzNr.ToString().PadLeft(3, '0');
                            destination.ANRED = source.Anrede;
                            destination.NAME1 = source.Name1;
                            destination.NAME2 = source.Name2;
                            destination.NAME3 = source.Name3;
                            destination.TITEL = source.Titel;
                            destination.LANDT = source.Land;
                            destination.PSTLZ = source.PLZ;
                            destination.ORT = source.Ort;
                            destination.STREET = source.Strasse;
                            if (source.Geburtsdatum.ToNullableDateTime() != null)
                            {
                                destination.GEBURTSDATUM = source.Geburtsdatum.ToNullableDateTime();
                            }
                            destination.STAATSANGE1 = source.Staatsangehoerigkeit1;
                            destination.STAATSANGE2 = source.Staatsangehoerigkeit2;
                            destination.GESCHLECHT = source.Geschlecht;
                            destination.KONTONUMMER = source.Kontonummer;
                            destination.BLZ = source.Bankleitzahl;
                            destination.INSTITUTSNAME = source.Kreditinstitut;
                            destination.LAND2 = source.Land2;
                            destination.ABWKONTOINHABER = source.AbwKontoinhaber;
                            destination.KOMMTYP1 = source.Kommunikationstyp1;
                            destination.KOMMSNO1 = source.Kommunikationsnummer1;
                            destination.KOMMTYP2 = source.Kommunikationstyp2;
                            destination.KOMMSNO2 = source.Kommunikationsnummer2;
                            destination.VUNUMMER = source.VuNummer;
                            destination.VERSNUMMER = source.VersicherungsscheinNr;
                            destination.VERMITTLER = source.Vermittler;
                            destination.ABGRP = source.Arbeitsgruppe;
                            if (source.Vertragsbeginn.ToNullableDateTime() != null)
                            {
                                destination.VERTRAGSBEGINN = source.Vertragsbeginn.ToNullableDateTime();
                            }
                            if (source.Vertragsende.ToNullableDateTime() != null)
                            {
                                destination.VERTRAGSABLAUF = source.Vertragsende.ToNullableDateTime();
                            }
                            destination.VERTRAGSSTATUS = source.Vertragsstatus;
                            destination.BEDINGUNGEN = source.Bedingungen;
                            destination.PRODUKTTYP = source.Produkttyp;
                            destination.DECKUNGSART = source.Deckungsart;
                            destination.ANZ_RISIKEN = source.AnzahlRisiken;
                            destination.KENNZEICHEN = source.Kennzeichen;
                            destination.HERSTNAME = source.Hersteller;
                            destination.FIN = source.VIN;
                            destination.MEHRFZKLAUSEL = source.Mehrfahrzeugklausel;
                            if (source.Erstzulassung.ToNullableDateTime() != null)
                            {
                                destination.ERSTZULDAT = source.Erstzulassung.ToNullableDateTime();
                            }
                            destination.WAGNISZIFFER = source.Wagniskennziffer;
                            destination.GELTUNGSBEREICH = source.Geltungsbereich;
                            destination.KVVERSICHERUNG = source.Krankenversicherung;
                            destination.EURO_VTRG = source.EuroVertrag;
                            destination.SBT_VK = source.SelbstbeteiligungVKinWE;
                            destination.WAERS1 = source.Waehrungsschluessel1;
                            destination.SBT_TK = source.SelbstbeteiligungTKinWE;
                            destination.WAERS2 = source.Waehrungsschluessel2;
                            destination.KASKO = source.Kasko;
                            destination.GETRIEBE = source.Getriebe;
                            destination.WKS_NAME = source.WerkstattName;
                            destination.WKS_PLZ = source.WerkstattPLZ;
                            destination.WKS_ORT = source.WerkstattOrt;
                            destination.WKS_STRASSE = source.WerkstattStrasse;
                            destination.WKS_ZEIT = source.WerkstattOeffnungszeiten;
                        }
                ));
            }
        }

        /// <summary>
        /// Daten für Vertragsverlängerung
        /// </summary>
        static public ModelMapping<Z_DPM_ASSIST_READ_VTRAGVERL_01.GT_OUT, VertragsverlaengerungModel> Z_DPM_ASSIST_READ_VTRAGVERL_01_GT_OUT_To_VertragsverlaengerungModel
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_ASSIST_READ_VTRAGVERL_01.GT_OUT, VertragsverlaengerungModel>(
                    new Dictionary<string, string>()
                    , (source, destination) =>
                    {
                        destination.ID = source.ID;
                        destination.VIN = source.FIN;
                        destination.Kennzeichen = source.KENNZEICHEN;
                        destination.Hersteller = source.HERSTNAME;
                        destination.Bedingungen = source.BEDINGUNGEN;
                        destination.Produkttyp = source.PRODUKTBEZ;
                        destination.Vertragsbeginn = source.VERTRAGSBEGINN;
                        destination.Vertragsende = source.VERTRAGSABLAUF;
                    }
                ));
            }
        }

        /// <summary>
        /// Daten für Vertragsverlängerung
        /// </summary>
        static public ModelMapping<Z_DPM_ASSIST_CHG_VTRAGVERL_01.GT_IN, VertragsverlaengerungModel> Z_DPM_ASSIST_CHG_VTRAGVERL_01_GT_IN_From_VertragsverlaengerungModel
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_ASSIST_CHG_VTRAGVERL_01.GT_IN, VertragsverlaengerungModel>(
                    new Dictionary<string, string>()
                    , null
                    , (source, destination) =>
                    {
                        destination.ID = source.ID;
                        destination.VERTRAGSABLAUF = source.Vertragsende;
                    }
                ));
            }
        }

        #region intern

        public static string HhMmToSap(string s)
        {
            return s.IsNullOrEmpty() ? null : s.Replace(":", "") + "00";
        }

        public static string HhMmFromSap(string s)
        {
            return s.IsNullOrEmpty() ? null : string.Format("{0}:{1}", s.SubstringTry(0, 2), s.SubstringTry(2, 2));
        }

        #endregion
    }
}