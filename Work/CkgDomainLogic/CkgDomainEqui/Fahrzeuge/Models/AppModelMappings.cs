using System.Collections.Generic;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;
using SapORM.Models;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class AppModelMappings : ModelMappings
    {
        #region Load from Repository

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_DPM_CD_ABM_LIST.ET_ABM_LIST, AbgemeldetesFahrzeug> Z_DPM_CD_ABM_LIST__ET_ABM_LIST_To_AbgemeldetesFahrzeug
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_CD_ABM_LIST.ET_ABM_LIST, AbgemeldetesFahrzeug>(
                    new Dictionary<string, string> ()
                    ,(sap, business) =>
                        {
                            business.Status = sap.STATUS;
                            business.StatusBezeichnung = sap.STATUS_BEZ;
                            business.FIN = sap.FIN;
                            business.RueckgabeDatum = sap.DAT_ABM_REP;
                            business.Standort = sap.ORT_ABM_REP;
                            business.Art = sap.TODO_TXT;
                            business.Abteilung = sap.ABTEILUNG;
                            business.Kilometer = sap.KM;
                            business.Betriebsnummer = sap.BETRIEB;
                            business.Bemerkung = sap.BEMERKUNG_TXT;
                            business.Modell = sap.MODELL;
                            business.FIN10 = sap.FIN_10;
                            business.AbmeldeAuftragDatum = sap.DAT_ABM_AUFTR;
                            business.AbmeldeDatum = sap.EXPIRY_DATE;
                            business.Kostenstelle = sap.KOSTST;
                            business.Zielort = sap.ZIELORT;
                            business.AbteilungsLeiter = sap.ABT_LEITER_VNAME.IsNullOrEmpty() ? sap.ABT_LEITER_NAME : string.Format("{0}, {1}", sap.ABT_LEITER_NAME, sap.ABT_LEITER_VNAME);
                        }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_DPM_CD_ABM_HIST.ET_ABM_HIST, AbmeldeHistorie> Z_DPM_CD_ABM_HIST__ET_ABM_HIST_To_AbmeldeHistorie
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_CD_ABM_HIST.ET_ABM_HIST, AbmeldeHistorie>(
                    new Dictionary<string, string> {
                        //{ "FIN", "FIN" },
                        { "CDKFN", "AbmeldeVorgangNr" },
                        { "ERDAT", "ErfassungsDatum" },
                        { "ERNAM", "ErfassungsUser" },
                        { "DATBI", "GueltigkeitsEndeDatum" },
                        { "TODO_TXT", "GeplanteAktionen" },
                        { "BEMERKUNG_TXT", "Bemerkung" },
                    }));
            }
        }

        static public ModelMapping<Z_M_HERSTELLERGROUP.T_HERST, Fahrzeuge.Models.Hersteller> Z_M_HERSTELLERGROUP_T_HERST_To_Hersteller
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_HERSTELLERGROUP.T_HERST, Fahrzeuge.Models.Hersteller>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.HerstellerName = s.HERST_T;
                        d.HerstellerSchluessel = s.HERST_GROUP;
                    }));
            }
        }

        static public ModelMapping<Z_M_EC_AVM_ZULAUF.GT_WEB, Fahrzeugzulauf> Z_M_EC_AVM_ZULAUF_GT_WEB_To_Fahrzeugzulauf
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_EC_AVM_ZULAUF.GT_WEB, Fahrzeugzulauf>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                        {
                            d.AuftragsNr = s.LIZNR;
                            d.EingangPdi = s.ZZDAT_EIN;
                            d.EingangZb2 = s.ERDAT_EQUI;
                            d.FahrgestellNr = s.CHASSIS_NUM;
                            d.Hersteller = s.ZMAKE;
                            d.Modell = s.ZMOD_DESCR;
                            d.ModellId = s.ZMODEL_ID;
                            d.UnitNr = s.ZUNIT_NR;
                            d.UnitNrPruefziffer = s.ZPZ_UNIT;
                            d.ZulaufDatumDatum = s.ZVERGDAT;
                            d.ZulaufDatumUhrzeit = s.ZVERGZEIT;
                        }));
            }
        }



        // Z_M_ECA_TAB_BESTAND
        static public ModelMapping<Z_M_ECA_TAB_BESTAND.GT_WEB, Zb2BestandSecurityFleet> Z_M_ECA_TAB_BESTAND_To_Zb2BestandSecurityFleet
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_ECA_TAB_BESTAND.GT_WEB, Zb2BestandSecurityFleet>(
                    new Dictionary<string, string>()
                    , (sap, business) =>
                    {
                        business.Kennzeichen = sap.LICENSE_NUM;
                        business.Fahrgestellnummer = sap.CHASSIS_NUM;
                        business.EingangZb2 = sap.DATAB;
                        business.Zulassungsdatum = sap.REPLA_DATE;
                        business.Bemerkung = sap.ZBEMERKUNG;
                        business.Hersteller = sap.HERST_T;
                        business.Modellbezeichnung = sap.ZZBEZEI;
                        business.Lagerstatus = sap.ABCKZ;                                                                                        
                    }));
            }
        }

        static public ModelMapping<Z_M_HERSTELLERGROUP.T_HERST, Fahrzeughersteller> Z_M_HERSTELLERGROUP_To_Fahrzeughersteller
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_HERSTELLERGROUP.T_HERST, Fahrzeughersteller>(
                    new Dictionary<string, string>()
                    , (sap, business) =>
                    {
                        business.HerstellerKey = sap.HERST_GROUP;
                        business.HerstellerName = sap.HERST_T;                        
                    }));
            }
        }


        static public ModelMapping<Z_M_TH_BESTAND.GT_BESTAND, Treuhandbestand> Z_M_TH_BESTAND__GET_BESTAND_LIST_To_Treuhandbestand
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_TH_BESTAND.GT_BESTAND, Treuhandbestand>(
                    new Dictionary<string, string>()
                    , (sap, business) =>
                    {
                        business.Kennzeichen = sap.LICENSE_NUM;
                        business.Fahrgestellnummer = sap.CHASSIS_NUM;
                        business.NameAG = sap.AG_NAME1;
                        business.NameTG = sap.TG_NAME1;                       
                        business.Zb2Nummer = sap.TIDNR;
                        business.Vertragsnummer = sap.LIZNR;
                        business.Versandstatus = sap.ABCTX;                       
                        business.Versandadresse = string.Concat(sap.NAME1.AppendIfNotNull(", "), sap.STREET.AppendIfNotNull(", "), sap.POST_CODE1.AppendIfNotNull(", "), sap.CITY1);                       
                        business.Referenz = sap.ZZREFERENZ2;                        
                    }));
            }
        }


        static public ModelMapping<Z_DPM_UF_MELDUNGS_SUCHE.GT_UF, Unfallmeldung> Z_DPM_UF_MELDUNGS_SUCHE_To_Unfallmeldungen
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_UF_MELDUNGS_SUCHE.GT_UF, Unfallmeldung>(
                     new Dictionary<string, string> ()
                    ,(sap, business) => {
                        business.Anlagedatum = sap.ERDAT;
                        business.WebUser = sap.ERNAM;
                        business.Kennzeichen = sap.LICENSE_NUM;
                        business.Fahrgestellnummer = sap.CHASSIS_NUM;
                        business.Erstzulassung = sap.ERSTZULDAT;
                        business.Kennzeicheneingang = sap.EG_KENNZ;
                        business.Abmeldung = sap.ABMDT;
                        business.StationsCode = sap.STATION;
                        business.Mahnstufe = sap.MAHNSTUFE;                        
                    }));
            }
        }



        static public ModelMapping<Z_M_Abm_Abgemeldete_Kfz.AUSGABE, AbgemeldetesFahrzeug> Z_M_Abm_Abgemeldete_Kfz_AUSGABE_ToAbgemeldetesFahrzeug
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_Abm_Abgemeldete_Kfz.AUSGABE, AbgemeldetesFahrzeug>(
                    new Dictionary<string, string>()
                    , (sap, business) =>
                    {
                        business.AbmeldeDatum = sap.VDATU; 
                        business.Briefnummer = sap.ZZBRIEF;                         
                        business.Fahrgestellnummer = sap.ZZFAHRG;
                        business.Kennzeichen = sap.ZZKENN;                        
                        business.Durchfuehrung = sap.PICKDAT;
                        business.Versand = sap.ZZTMPDT;
                    }));
            }
        }

        static public ModelMapping<Z_M_TH_GET_TREUH_AG.GT_EXP, TreuhandKunde> Z_M_TH_GET_TREUH_AG_GT_EXP_ToTreuhandKunden
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_TH_GET_TREUH_AG.GT_EXP, TreuhandKunde>(
                    new Dictionary<string, string>()
                    , (sap, business) =>
                    {
                        business.TGName = sap.NAME1_TG;
                        business.AGName = sap.NAME1_AG;
                        business.AGNummer = sap.AG;
                        business.TGNummer = sap.TREU;
                       
                    }));
            }
        }

        static public ModelMapping<Z_M_TH_GET_TREUH_AG.GT_EXP, TreuhandKunde> Z_M_TH_GET_TREUH_AG_GT_EXP_ToTreuhandKundenAG
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_TH_GET_TREUH_AG.GT_EXP, TreuhandKunde>(
                    new Dictionary<string, string>()
                    , (sap, business) =>
                    {
                        business.TGName = sap.NAME1_AG;
                        business.AGName = sap.NAME1_TG;
                        business.AGNummer = sap.TREU;
                        business.TGNummer = sap.AG;
                    }));
            }
        }

        static public ModelMapping<Z_DPM_READ_VERSAND_SPERR_001.GT_OUT, Treuhandbestand> Z_DPM_READ_VERSAND_SPERR_001_GT_OUT_ToTreuhandfreigabe    
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_VERSAND_SPERR_001.GT_OUT, Treuhandbestand>(
                    new Dictionary<string, string>()
                    , (sap, business) =>
                    {
                        business.Kennzeichen = sap.LICENSE_NUM;
                        business.Fahrgestellnummer = sap.CHASSIS_NUM;
                        business.NameAG = sap.NAME1_AG;
                        business.AGNummer = sap.ZZKUNNR_AG;
                        business.NameTG = sap.NAME1_TG;
                        business.TGNummer = sap.KUNNR_TG;
                        business.Zb2Nummer = sap.TIDNR;
                        business.Vertragsnummer = sap.LIZNR;                        
                        business.Versandadresse = string.Concat(sap.NAME2_ZS.AppendIfNotNull(", "), sap.STRASSE_ZS.AppendIfNotNull(", "), sap.PLZ_ZS.AppendIfNotNull(", "), sap.ORT_ZS);
                        business.Ersteller = sap.ERNAM;
                        business.Belegnummer = sap.BELNR;
                        business.Referenz = sap.ZZREFERENZ2;     
                    }));
            }
        }

        static public ModelMapping<Z_M_TH_GET_TREUH_AG.GT_EXP, TreuhandKunde> Z_M_TH_GET_TREUH_AG_GT_EXP_ToTreuhandKundenAGServices
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_TH_GET_TREUH_AG.GT_EXP, TreuhandKunde>(
                    new Dictionary<string, string>()
                    , (sap, business) =>
                    {
                        business.TGName = sap.NAME1_AG;
                        business.AGName = sap.NAME1_TG;
                        business.AGNummer = sap.TREU;
                        business.TGNummer = sap.AG;
                        business.IsServicesAGMapping = true;
                    }));
            }
        }


        static public ModelMapping<Z_M_EC_AVM_ZULASSUNGEN.GT_WEB, Dispositionsliste> Z_M_EC_AVM_ZULASSUNGEN_GT_WEB_ToDispositionsliste        
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_EC_AVM_ZULASSUNGEN.GT_WEB, Dispositionsliste>(
                    new Dictionary<string, string>()
                    , (sap, business) =>
                    {
                        business.Zulassungsdatum = sap.REPLA_DATE;
                        business.PDINummer = sap.ZZCARPORT;
                        business.PDIBezeichnung = sap.ZCARPORT_NAME1;
                        business.ModellCode = sap.ZMODEL_ID;
                        business.Modellbezeichnung = sap.ZMOD_DESCR;
                        business.Hersteller = sap.ZMAKE;
                        int result = 0;
                        int.TryParse(sap.ZANZAHL, out result);
                        business.Anzahl = result;
                        business.KennzeichenVon = sap.LICENSE_NUM_VON;
                        business.KennzeichenBis = sap.LICENSE_NUM_BIS;                                             
                    }));
            }
        }


        static public ModelMapping<Z_M_EC_AVM_STATUS_ZUL.GT_WEB, ZulaufEinsteuerung> Z_M_EC_AVM_STATUS_ZUL_GT_WEB_ToZulaufEinsteuerung
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_EC_AVM_STATUS_ZUL.GT_WEB, ZulaufEinsteuerung>(
                    new Dictionary<string, string>()
                    , (sap, business) =>
                    {
                        business.Hersteller = sap.ZKLTXT;
                        business.Fahrzeugzulauf = sap.FZG_EING_GES;
                        business.Zulassungen = sap.ZUL_GES;
                        business.ZBIIOhneFzgPKW = sap.BR_O_FZG_PKW;
                        business.ZBIIOhneFzgLKW = sap.BR_O_FZG_LKW;                       
                    }));
            }
        }

        static public ModelMapping<Z_DPM_LIST_POOLS_001.GT_WEB, Fahrzeug> Z_DPM_LIST_POOLS_001_GT_WEB_ToFahrzeuguebersicht
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_LIST_POOLS_001.GT_WEB, Fahrzeug>(
                    new Dictionary<string, string>()
                    , (sap, business) =>
                    {
                        business.Carport = sap.KUNPDI;
                        business.Carportname = sap.KUNPDI_TXT;
                        business.Fahrgestellnummer = sap.CHASSIS_NUM;
                        business.Kennzeichen = sap.LICENSE_NUM;
                        business.Zulassungsdatum = sap.REPLA_DATE;
                        business.Unitnummer = sap.ZZREFERENZ1;
                        business.ModelID = sap.ZZMODELL;
                        business.Modell = sap.ZZBEZEI;
                        business.Status = sap.STATUS_TEXT;
                        business.EingangZb2Datum = sap.ERDAT_EQUI;
                        business.EingangFahrzeugDatum = sap.ZZDAT_EIN;
                        business.BereitmeldungDatum = sap.ZZDAT_BER;
                        business.Hersteller = sap.ZZHERST_TEXT;
                    }));
            }
        }

        static public ModelMapping<Z_DPM_READ_AUFTR_006.GT_OUT, FahrzeuguebersichtStatus> Z_DPM_READ_AUFTR_006_GT_OUT_ToFahrzeuguebersichtStatus
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_AUFTR_006.GT_OUT, FahrzeuguebersichtStatus>(
                    new Dictionary<string, string>()
                    , (sap, business) =>
                    {
                        business.StatusKey = sap.POS_TEXT;
                        business.StatusText = sap.POS_TEXT;                        
                    }));
            }
        }

        static public ModelMapping<Z_DPM_LIST_PDI_001.GT_WEB, FahrzeuguebersichtPDI> Z_DPM_LIST_PDI_001_GT_WEB_ToFahrzeuguebersichtPDI
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_LIST_PDI_001.GT_WEB, FahrzeuguebersichtPDI>(
                    new Dictionary<string, string>()
                    , (sap, business) =>
                    {
                        business.PDIKey = sap.KUNPDI;
                        business.PDIText = sap.PDIWEB;
                    }));
            }
        }

        #endregion

        

        #region Save to Repository

        // ReSharper disable InconsistentNaming
        public static ModelMapping<Z_DPM_CD_ABM_LIST.IT_STATUS, FahrzeugStatus> Z_DPM_CD_ABM_LIST__IT_STATUS_To_FahrzeugStatus
            // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_CD_ABM_LIST.IT_STATUS, FahrzeugStatus>(
                                                 new Dictionary<string, string>
                                                     {
                                                         {"STATUS", "ID"},
                                                     },
                                                     null,  // Init Copy
                                                     (business, sap) =>   // Init Copy Back
                                                         {
                                                             sap.STATUS = business.ID.NotNullOrEmpty().TrimStart('0').PadLeft(2, '0');
                                                         }));
            }
        }


        // ReSharper disable InconsistentNaming
        public static ModelMapping<Z_DPM_IMP_FEHLTEILETIK_01.GT_DATEN, FehlteilEtikett> Z_DPM_IMP_FEHLTEILETIK_01__GT_DATEN_To_FehlteilEtikett
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_IMP_FEHLTEILETIK_01.GT_DATEN, FehlteilEtikett>(
                                                 new Dictionary<string, string>
                                                     {
                                                         {"CHASSIS_NUM", "VIN"},

                                                         {"INHALT_1", "Content1"},
                                                         {"UEBERSCHRIFT_1", "Header1"},
                                                         
                                                         {"INHALT_2", "Content2"},
                                                         {"UEBERSCHRIFT_2", "Header2"},
                                                         
                                                         {"INHALT_3", "Content3"},
                                                         {"UEBERSCHRIFT_3", "Header3"},
                                                         
                                                         {"INHALT_4", "Content4"},
                                                         {"UEBERSCHRIFT_4", "Header4"},
                                                         
                                                         {"INHALT_5", "Content5"},
                                                         {"UEBERSCHRIFT_5", "Header5"},
                                                         
                                                         {"INHALT_6", "Content6"},
                                                         {"UEBERSCHRIFT_6", "Header6"},
                                                         
                                                         {"INHALT_7", "Content7"},
                                                         {"UEBERSCHRIFT_7", "Header7"},
                                                         
                                                         {"INHALT_8", "Content8"},
                                                         {"UEBERSCHRIFT_8", "Header8"},
                                                         
                                                         {"INHALT_9", "Content9"},
                                                         {"UEBERSCHRIFT_9", "Header9"},
                                                         
                                                         {"INHALT_10", "Content10"},
                                                         {"UEBERSCHRIFT_10", "Header10"},
                                                     },
                                                     null,  // Init Copy
                                                     (business, sap) =>   // Init Copy Back
                                                         {
                                                             sap.CHASSIS_NUM = business.VIN.NotNullOrEmpty().ToUpper();
                                                         }));
            }
        }

        // ReSharper disable InconsistentNaming
        public static ModelMapping<Z_DPM_DRUCK_FEHLTEILETIK.GT_ETIKETT, FehlteilEtikett> Z_DPM_DRUCK_FEHLTEILETIK_GT_ETIKETT_To_FehlteilEtikett
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_DRUCK_FEHLTEILETIK.GT_ETIKETT, FehlteilEtikett>(
                                                 new Dictionary<string, string>
                                                     {
                                                         {"CHASSIS_NUM", "VIN"},
                                                         {"LICENSE_NUM", "Kennzeichen"},

                                                         {"INHALT_1", "Content1"},
                                                         {"UEBERSCHRIFT_1", "Header1"},
                                                         
                                                         {"INHALT_2", "Content2"},
                                                         {"UEBERSCHRIFT_2", "Header2"},
                                                         
                                                         {"INHALT_3", "Content3"},
                                                         {"UEBERSCHRIFT_3", "Header3"},
                                                         
                                                         {"INHALT_4", "Content4"},
                                                         {"UEBERSCHRIFT_4", "Header4"},
                                                         
                                                         {"INHALT_5", "Content5"},
                                                         {"UEBERSCHRIFT_5", "Header5"},
                                                         
                                                         {"INHALT_6", "Content6"},
                                                         {"UEBERSCHRIFT_6", "Header6"},
                                                         
                                                         {"INHALT_7", "Content7"},
                                                         {"UEBERSCHRIFT_7", "Header7"},
                                                         
                                                         {"INHALT_8", "Content8"},
                                                         {"UEBERSCHRIFT_8", "Header8"},
                                                         
                                                         {"INHALT_9", "Content9"},
                                                         {"UEBERSCHRIFT_9", "Header9"},
                                                         
                                                         {"INHALT_10", "Content10"},
                                                         {"UEBERSCHRIFT_10", "Header10"},
                                                     },
                                                     null,  // Init Copy
                                                     (business, sap) =>   // Init Copy Back
                                                     {
                                                         sap.CHASSIS_NUM = business.VIN.NotNullOrEmpty().ToUpper();
                                                     }));
            }
        }

        /// <summary>
        /// Upload Fahrzeugeinsteuerung
        /// </summary>
        static public ModelMapping<Z_DPM_UPLOAD_GRUDAT_TIP_01.GT_IN, FahrzeugeinsteuerungUploadModel> Z_DPM_UPLOAD_GRUDAT_TIP_01_GT_IN_From_FahrzeugeinsteuerungUploadModel
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_UPLOAD_GRUDAT_TIP_01.GT_IN, FahrzeugeinsteuerungUploadModel>(
                    new Dictionary<string, string>()
                    , null
                    , (source, destination) =>
                    {
                        destination.CHASSIS_NUM = source.Fahrgestellnummer;
                        destination.ZZREFERENZ1 = source.Flottennummer;
                    }
                ));
            }
        }
       

        /// <summary>
        /// Validiere Treuhandverwaltung
        /// </summary>
        static public ModelMapping<Z_DPM_CHECK_TH_CODE.GT_IN, TreuhandverwaltungCsvUpload> Z_DPM_CHECK_TH_CODE_GT_IN_From_TreuhandverwaltungCsvUpload
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_CHECK_TH_CODE.GT_IN, TreuhandverwaltungCsvUpload>(
                    new Dictionary<string, string>()
                    , null
                    , (source, destination) =>
                    {
                        destination.EQUI_KEY = source.Fahrgestellnummer;
                        destination.ERDAT = source.Datum;
                        destination.SPERRDAT = source.Sperrdatum;
                        destination.ERNAM = source.Sachbearbeiter;
                        destination.ZZREFERENZ2 = source.Referenznummer;
                    }
                ));
            }
        }

        /// <summary>
        /// Speichere Treuhandverwaltung Ent-/sperrung
        /// </summary>
        static public ModelMapping<Z_M_TH_INS_VORGANG.GT_IN, TreuhandverwaltungCsvUpload> Z_M_TH_INS_VORGANG_GT_IN_From_TreuhandverwaltungCsvUpload
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_TH_INS_VORGANG.GT_IN, TreuhandverwaltungCsvUpload>(
                    new Dictionary<string, string>()
                    , null
                    , (source, destination) =>
                    {
                        destination.AG = source.AGNummer;
                        destination.EQUI_KEY = source.Fahrgestellnummer;
                        destination.ERNAM = source.Sachbearbeiter;
                        destination.ERDAT = source.Datum;                        
                        destination.SPERRDAT = source.Sperrdatum >= source.Datum ? source.Sperrdatum : source.Datum;
                        destination.TREUH_VGA =  source.IsSperren ? "S" : "F";
                        destination.SUBRC = 0;                        
                        destination.MESSAGE = string.Empty;
                        // VERTR_beginn / Ends? -> s. Legacy                        
                        destination.ZZREFERENZ2 = source.Referenznummer;
                    }
                ));
            }
        }


        static public ModelMapping<Z_DPM_FREIG_VERSAND_SPERR_001.GT_WEB, Treuhandbestand> Z_DPM_FREIG_VERSAND_SPERR_001_GT_WEB_From_Treuhandbestand
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_FREIG_VERSAND_SPERR_001.GT_WEB, Treuhandbestand>(
                    new Dictionary<string, string>()
                    , null
                    , (source, destination) =>
                    {                       
                        destination.BELNR = source.Belegnummer;                       
                        destination.SPERRSTATUS = source.IsActionFreigeben ? "F" : "A";
                        destination.NICHT_FREIG_GRU = !source.IsActionFreigeben ? source.Ablehnungsgrund + "" : "";                      
                        destination.BEM = "";
                    }
                ));
            }
        }

      

        static public ModelMapping<Z_DPM_IMP_MODELL_ID_01.GT_IN, FahrzeugvoravisierungUploadModel> Z_DPM_IMP_MODELL_ID_01_GT_IN_From_FahrzeugvoravisierungUploadModel
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_IMP_MODELL_ID_01.GT_IN, FahrzeugvoravisierungUploadModel>(
                        new Dictionary<string, string>()
                        , null
                        , (source, destination) =>
                        {
                            destination.CHASSIS_NUM = source.Fahrgestellnummer;
                            destination.ZAUFTRAGS_NR = source.Auftragsnummer;
                            destination.ZMODELL = source.ModelID;
                            destination.LICENSE_NUM = source.Kennzeichen;
                        }
                    ));
            }
        }

        #endregion
    }
}