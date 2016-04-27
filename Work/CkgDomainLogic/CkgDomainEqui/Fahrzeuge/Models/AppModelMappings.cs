// ReSharper disable InconsistentNaming

using System.Collections.Generic;
using CkgDomainLogic.DomainCommon.Models;
using System.Linq;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;
using SapORM.Models;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class AppModelMappings : ModelMappings
    {
        #region Load from Repository

        static public ModelMapping<Z_DPM_CD_ABM_LIST.ET_ABM_LIST, AbgemeldetesFahrzeug> Z_DPM_CD_ABM_LIST__ET_ABM_LIST_To_AbgemeldetesFahrzeug
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

        static public ModelMapping<Z_DPM_CD_ABM_HIST.ET_ABM_HIST, AbmeldeHistorie> Z_DPM_CD_ABM_HIST__ET_ABM_HIST_To_AbmeldeHistorie
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
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_UF_MELDUNGS_SUCHE.GT_UF, Unfallmeldung>(
                     new Dictionary<string, string> ()
                    , (sap, business) =>
                    {
                        business.AnlageDatum = sap.ERDAT;
                        business.WebUser = sap.ERNAM;
                        business.Kennzeichen = sap.LICENSE_NUM;
                        business.Fahrgestellnummer = sap.CHASSIS_NUM;
                        business.ErstzulassungDatum = sap.ERSTZULDAT;
                        business.KennzeicheneingangsDatum = sap.EG_KENNZ;
                        business.AbmeldeDatum = sap.ABMDT;
                        business.StationsCode = sap.STATION;
                        business.Mahnstufe = sap.MAHNSTUFE;                        
                        business.UnfallNr = sap.UNFALL_NR;
                        business.StornoDatum = sap.STORNODAT;
                        business.StornoText = sap.STORNOBEM;
                    }));
            }
        }

        static public ModelMapping<Z_DPM_UF_EQUI_SUCHE.GT_EQUIS, Unfallmeldung> Z_DPM_UF_EQUI_SUCHE_To_Unfallmeldungen
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_UF_EQUI_SUCHE.GT_EQUIS, Unfallmeldung>(
                     new Dictionary<string, string>()
                    , (sap, business) =>
                    {
                        business.Fahrgestellnummer = sap.CHASSIS_NUM;
                        business.Kennzeichen = sap.LICENSE_NUM;
                        business.BriefNummer = sap.TIDNR;
                        business.UnitNummer = sap.ZZREFERENZ1;
                        business.EquiNr = sap.EQUNR;
                    }));
            }
        }



        static public ModelMapping<Z_M_ABM_ABGEMELDETE_KFZ.AUSGABE, AbgemeldetesFahrzeug> Z_M_Abm_Abgemeldete_Kfz_AUSGABE_ToAbgemeldetesFahrzeug
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_ABM_ABGEMELDETE_KFZ.AUSGABE, AbgemeldetesFahrzeug>(
                    new Dictionary<string, string>()
                    , (sap, business) =>
                    {
                        business.AbmeldeDatum = sap.VDATU; 
                        business.Briefnummer = sap.ZZBRIEF;                         
                        business.Fahrgestellnummer = sap.ZZFAHRG;
                        business.Kennzeichen = sap.ZZKENN;                        
                        business.Durchfuehrung = sap.PICKDAT;
                        business.Versand = sap.ZZTMPDT;

                        var halterName1 = sap.NAME1_ZH;
                        var halterName2 = sap.NAME2_ZH;
                        var halterStrasse = sap.STREET_ZH.FormatIfNotNull("{this}{0}", sap.HOUSE_NUM1_ZH.PrependIfNotNull(" "));
                        var halterPlz = sap.POST_CODE1_ZH;
                        var halterOrt = sap.CITY1_ZH;
                        business.HalterAdresse = string.Format(
                            "{0}{1}{2}{3}{4}",
                            halterName1, halterName2.PrependIfNotNull(", "), 
                            halterStrasse.PrependIfNotNull(", "), halterPlz.PrependIfNotNull(", "), halterOrt.PrependIfNotNull(" "));
                    }));
            }
        }

        static public ModelMapping<Z_M_TH_GET_TREUH_AG.GT_EXP, TreuhandKunde> Z_M_TH_GET_TREUH_AG_GT_EXP_ToTreuhandKunden
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
                        business.Selection = sap.ZSELECT;
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
                        business.Ablehnungsgrund = sap.NICHT_FREIG_GRU;
                        business.Ablehnender = sap.FREIGABEUSER;
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

        static public ModelMapping<Z_DPM_LIST_POOLS_001.GT_WEB, Fahrzeuguebersicht> Z_DPM_LIST_POOLS_001_GT_WEB_ToFahrzeuguebersicht
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_LIST_POOLS_001.GT_WEB, Fahrzeuguebersicht>(
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
                        business.StatusKey = sap.STATUS.ToInt();
                        business.EingangZb2Datum = sap.ERDAT_EQUI;
                        business.EingangFahrzeugDatum = sap.ZZDAT_EIN;
                        business.BereitmeldungDatum = sap.ZZDAT_BER;
                        business.Hersteller = sap.ZZHERST_TEXT;
                        business.BemerkungIntern = sap.BEMERKUNG_INTERN;
                        business.BemerkungExtern = sap.BEMERKUNG_EXTERN;
                        business.BemerkungSperre = sap.ZBEMERKUNG;
                        business.Gesperrt = (sap.ZZAKTSPERRE == "JA");
                        business.MeldungsNr = sap.QMNUM;
                        business.DadPdi = sap.ZZCARPORT;
                        business.Farbcode = sap.ZFARBE;
                        business.Farbname = sap.FARBE_TEXT;
                        business.Anhaengerkupplung = sap.ZAHK.XToBool();
                        business.Navi = sap.ZNAVI.XToBool();
                        business.Winterreifen = sap.ZMS_REIFEN.XToBool();
                        business.SIPPCode = sap.ZZSIPP;
                        business.Auftragsnummer = sap.LIZNR;
                        business.Zb2Nummer = sap.TIDNR;
                        business.BatchId = sap.ZBATCH_ID;
                        business.Fahrzeugtyp = sap.FZGART;

                        business.Farbcode = sap.ZFARBE;
                        business.Farbname = sap.FARBE_TEXT;
                        business.KraftstoffArt = sap.ZZKRAFTSTOFF_TXT;
                        business.ZulassungBereit = sap.ZULBEREIT.XToBool();
                        business.ZulassungsSperre = sap.ZZAKTSPERRE.XToBool();
                        business.AbmeldeDatum = sap.EXPIRY_DATE;
                        business.VersandDatum = sap.ZZTMPDT;

                        business.Lieferant = sap.NAME1_ZP;
                        business.Ort = sap.CITY1_ZP;
                        business.FinanzierungsArt = sap.FIN_ART;
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
                        business.StatusKey = sap.POS_KURZTEXT;
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
                        business.DadPdi = sap.DADPDI;
                    }));
            }
        }

        // Z_M_ECA_TAB_BESTAND
        static public ModelMapping<Z_DPM_CHANGE_ADDR002_001.GT_OUT, Adresse> Z_DPM_CHANGE_ADDR002_001_To_Adresse
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_CHANGE_ADDR002_001.GT_OUT, Adresse>(
                    new Dictionary<string, string>()
                    , (sap, business) =>
                    {
                        business.KundenNr = sap.EX_KUNNR;
                        business.Name1 = sap.NAME1;
                        business.Name2 = sap.NAME2;
                        business.Strasse = sap.STREET;
                        business.HausNr = sap.HOUSE_NUM1;
                        business.PLZ = sap.POST_CODE1;
                        business.Ort = sap.CITY1;

                        business.GetAutoSelectStringCustom = () => string.Format("{0} - {1}, {2} {3}", 
                            business.KundenNr, business.Name1, business.PLZ, business.Ort);
                    }));
            }
        }

        static public ModelMapping<Z_M_EC_AVM_MELDUNGEN_PDI1.GT_WEB, Fzg> Z_M_EC_AVM_MELDUNGEN_PDI1_GT_WEB_ToFzg
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_EC_AVM_MELDUNGEN_PDI1.GT_WEB, Fzg>(
                    new Dictionary<string, string>()
                    , (sap, business) =>
                    {
                        business.InternalID = sap.QMNUM;

                        business.EquiNummer = sap.EQUNR;
                        business.Pdi = sap.KUNPDI;
                        business.DadPdi = sap.DADPDI;
                        business.Fahrgestellnummer = sap.ZZFAHRG;
                        business.Zb2Nummer = sap.ZZBRIEF;
                        business.Zulassungsdatum = sap.REPLA_DATE;
                        business.ModelID = sap.ZZMODELL;
                        business.Modell = sap.ZZBEZEI;
                        business.Hersteller = sap.HERST_T;
                        business.EingangFahrzeugDatum = sap.ZZDAT_EIN;
                        business.Farbcode = sap.ZZFARBE;
                        business.Reifen = sap.ZZREIFEN;
                        business.Navi = sap.ZZNAVI;
                        business.Ahk = sap.ZAHK;
                        business.BemerkungSperre = sap.ZBEMERKUNG;
                        business.BemerkungIntern = sap.BEMERKUNG_INTERN;
                        business.BemerkungExtern = sap.BEMERKUNG_EXTERN;
                        business.AuftragsNummer = sap.ZZREF1;
                        business.SippCode = sap.ZZSIPP1.NotNullOrEmpty() + sap.ZZSIPP2.NotNullOrEmpty() +
                                            sap.ZZSIPP3.NotNullOrEmpty() + sap.ZZSIPP4.NotNullOrEmpty();
                    }));
            }
        }

        static public ModelMapping<Z_M_EC_AVM_ANZ_BEAUFTR_ZUL.GT_WEB, Fzg> Z_M_EC_AVM_ANZ_BEAUFTR_ZUL_GT_WEB_ToFzg
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_EC_AVM_ANZ_BEAUFTR_ZUL.GT_WEB, Fzg>(
                    new Dictionary<string, string>()
                    , (sap, business) =>
                    {
                        business.Pdi = sap.ZZCARPORT;
                        business.Amount = sap.ZANZAHL.ToInt(0);
                    }));
            }
        }

        static public ModelMapping<Z_M_EC_AVM_KENNZ_SERIE.GT_WEB, KennzeichenSerie> Z_M_EC_AVM_KENNZ_SERIE_GT_WEB_ToKennzeichenSerie
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_EC_AVM_KENNZ_SERIE.GT_WEB, KennzeichenSerie>(
                    new Dictionary<string, string>()
                    , (sap, business) =>
                    {
                        business.ID = sap.SONDERSERIE.NotNullOrEmpty();

                        business.Name = string.Format("{0}-{1}{2}", sap.ORTKENNZ, sap.MINLETTER, sap.SONDERSERIE.FormatIfNotNull(" ({this})"));

                        business.Art = sap.ART;
                        business.OrtsKennzeichen = sap.ORTKENNZ;
                        business.BuchstabenTeilMin = sap.MINLETTER;
                        business.BuchstabenTeilMax = sap.MAXLETTER;
                        business.NummernTeilMin = sap.MINNUMBER;
                        business.NummernTeilMax = sap.MAXNUMBER;
                        business.SonderSerie = sap.SONDERSERIE;
                    }));
            }
        }

        static public void Z_M_EC_AVM_MELDUNGEN_PDI1_GT_TXT_ToFzg(IEnumerable<Z_M_EC_AVM_MELDUNGEN_PDI1.GT_TXT> sapItems, IEnumerable<Fzg> businessItems)
        {
            foreach (var businessItem in businessItems)
            {
                var business = businessItem;
                var sap = sapItems.FirstOrDefault(s => s.QMNUM == business.InternalID);
                if (sap == null)
                    continue;

                business.BemerkungSperre = sap.TDLINE;
            }
       }

        static public ModelMapping<Z_DPM_READ_MEL_CARP_01.GT_TAB, CarporterfassungModel> Z_DPM_READ_MEL_CARP_01_GT_TAB_To_CarporterfassungModel
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_MEL_CARP_01.GT_TAB, CarporterfassungModel>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.AuftragsNr = s.AUFNR_AG;
                        d.CarportId = s.CARPORT_ID_AG;
                        d.FahrgestellNr = s.CHASSIS_NUM;
                        d.Kennzeichen = s.LICENSE_NUM;
                        d.BestandsNr = s.MVA_NUMMER;
                        d.CarportName = s.PDINAME;
                        d.Status = s.BEM;
                    }));
            }
        }

        static public ModelMapping<Z_DPM_IMP_CARPORT_MELD_01.GT_WEB, CarporterfassungModel> Z_DPM_IMP_CARPORT_MELD_01_GT_WEB_To_CarporterfassungModel
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_IMP_CARPORT_MELD_01.GT_WEB, CarporterfassungModel>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.AnzahlKennzeichen = s.ANZ_KENNZ_CPL;
                        d.AuftragsNr = s.AUFNR_AG;
                        d.Barcode = s.BARCODE;
                        d.CarportId = s.CARPORT_ID_AG;
                        d.FahrgestellNr = s.CHASSIS_NUM;
                        d.CocVorhanden = CarporterfassungModel.GetMaterialVorhandenOptionWeb(s.COC_VORH);
                        d.DemontageDatum = s.DAT_DEMONT;
                        d.Abgemeldet = s.FZG_ABGEMELDET.XToBool();
                        d.HuAuBerichtVorhanden = CarporterfassungModel.GetMaterialVorhandenOptionWeb(s.HU_AU_BER_VORH);
                        d.KundenNr = s.KUNNR_AG;
                        d.Kennzeichen = s.LICENSE_NUM;
                        d.LieferscheinNr = s.LSNUMMER;
                        d.BestandsNr = s.MVA_NUMMER;
                        d.NaviCdVorhanden = CarporterfassungModel.GetMaterialVorhandenOptionWeb(s.NAVI_CD_VORH);
                        d.ServiceheftVorhanden = CarporterfassungModel.GetMaterialVorhandenOptionWeb(s.SERVICEH_VORH);
                        d.Zb1Vorhanden = CarporterfassungModel.GetMaterialVorhandenOptionWeb(s.ZB1_VORH);
                        d.Zb2Vorhanden = CarporterfassungModel.GetMaterialVorhandenOptionWeb(s.ZB2_VORH);
                        d.ZweitschluesselVorhanden = CarporterfassungModel.GetMaterialVorhandenOptionWeb(s.ZWEITSCHLUE_VORH);
                        d.Status = s.BEM;
                        d.Organisation = s.ORGANISATION;
                    }));
            }
        }

        static public ModelMapping<Z_DPM_READ_AUFTR_006.GT_OUT, CarportInfo> Z_DPM_READ_AUFTR_006_GT_OUT_To_CarportInfo
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_AUFTR_006.GT_OUT, CarportInfo>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.CarportId = s.POS_KURZTEXT;
                        d.KundenNr = s.INTNR;
                        d.Land = s.LAND1;
                        d.Name1 = s.NAME1;
                        d.Name2 = s.NAME2;
                        d.Ort = s.ORT01;
                        d.Plz = s.PSTLZ;
                        d.CarportRegion = s.POS_TEXT;
                        d.StrasseHausnummer = s.STRAS;
                        d.Telefon = s.TELNR;
                    }));
            }
        }

        static public ModelMapping<Z_DPM_READ_CARPORT_MELD_01.GT_WEB, CarporterfassungModel> Z_DPM_READ_CARPORT_MELD_01_GT_WEB_To_CarporterfassungModel
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_CARPORT_MELD_01.GT_WEB, CarporterfassungModel>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        // Barcode wird hier bewusst nicht mitgelesen
                        d.AnzahlKennzeichen = s.ANZ_KENNZ_CPL;
                        d.AuftragsNr = s.AUFTRAGS_NR;
                        d.CarportId = s.CARPORT_ID_AG;
                        d.FahrgestellNr = s.CHASSIS_NUM;
                        d.CocVorhanden = CarporterfassungModel.GetMaterialVorhandenOptionWeb(s.COC_VORH);
                        d.DemontageDatum = s.DAT_DEMONT;
                        d.Abgemeldet = s.FZG_ABGEMELDET.XToBool();
                        d.HuAuBerichtVorhanden = CarporterfassungModel.GetMaterialVorhandenOptionWeb(s.HU_AU_BER_VORH);
                        d.KundenNr = s.KUNNR_AG;
                        d.Kennzeichen = s.LICENSE_NUM;
                        d.BestandsNr = s.MVA_NUMMER;
                        d.NaviCdVorhanden = CarporterfassungModel.GetMaterialVorhandenOptionWeb(s.NAVI_CD_VORH);
                        d.ServiceheftVorhanden = CarporterfassungModel.GetMaterialVorhandenOptionWeb(s.SERVICEH_VORH);
                        d.Zb1Vorhanden = CarporterfassungModel.GetMaterialVorhandenOptionWeb(s.VORLAGE_ZB1_CPL);
                        d.Zb2Vorhanden = CarporterfassungModel.GetMaterialVorhandenOptionWeb(s.ZB2_VORH);
                        d.ZweitschluesselVorhanden = CarporterfassungModel.GetMaterialVorhandenOptionWeb(s.ZWEITSCHLUE_VORH);
                        d.Organisation = s.ORGANISATION;
                        d.UserName = s.WEB_USER;
                    }));
            }
        }

        static public ModelMapping<Z_DPM_INS_CARPORT_NACHLIEF_01.GT_WEB, CarporterfassungModel> Z_DPM_INS_CARPORT_NACHLIEF_01_GT_WEB_To_CarporterfassungModel
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_INS_CARPORT_NACHLIEF_01.GT_WEB, CarporterfassungModel>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.AnzahlKennzeichen = s.ANZ_KENNZ_CPL;
                        d.AuftragsNr = s.AUFTRAGS_NR;
                        d.Barcode = s.BARCODE;
                        d.CarportId = s.CARPORT_ID_AG;
                        d.FahrgestellNr = s.CHASSIS_NUM;
                        d.CocVorhanden = CarporterfassungModel.GetMaterialVorhandenOptionWeb(s.COC_VORH);
                        d.DemontageDatum = s.DAT_DEMONT;
                        d.Abgemeldet = s.FZG_ABGEMELDET.XToBool();
                        d.HuAuBerichtVorhanden = CarporterfassungModel.GetMaterialVorhandenOptionWeb(s.HU_AU_BER_VORH);
                        d.KundenNr = s.KUNNR_AG;
                        d.Kennzeichen = s.LICENSE_NUM;
                        d.LieferscheinNr = s.LSNUMMER;
                        d.BestandsNr = s.MVA_NUMMER;
                        d.NaviCdVorhanden = CarporterfassungModel.GetMaterialVorhandenOptionWeb(s.NAVI_CD_VORH);
                        d.ServiceheftVorhanden = CarporterfassungModel.GetMaterialVorhandenOptionWeb(s.SERVICEH_VORH);
                        d.Zb1Vorhanden = CarporterfassungModel.GetMaterialVorhandenOptionWeb(s.VORLAGE_ZB1_CPL);
                        d.Zb2Vorhanden = CarporterfassungModel.GetMaterialVorhandenOptionWeb(s.ZB2_VORH);
                        d.ZweitschluesselVorhanden = CarporterfassungModel.GetMaterialVorhandenOptionWeb(s.ZWEITSCHLUE_VORH);
                        d.Organisation = s.ORGANISATION;
                        d.Status = s.BEM;
                    }));
            }
        }

        static public ModelMapping<Z_DPM_REM_READ_VERSSPERR_01.GT_OUT, FahrzeugVersand> Z_DPM_REM_READ_VERSSPERR_01_GT_OUT_To_FahrzeugVersand
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_REM_READ_VERSSPERR_01.GT_OUT, FahrzeugVersand>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.AuftragGeber = s.KUNNR_AG;
                        d.Fahrgestellnummer = s.CHASSIS_NUM;
                        d.HaendlerNummer = s.RDEALER;
                        d.ZahlungsArt = s.DZLART;
                        d.Name1 = s.NAME1_ZF;
                        d.Name2 = s.NAME2_ZF;
                        d.Strasse = s.STREET_ZF;
                        d.PLZ = s.POST_CODE1_ZF;
                        d.Ort = s.CITY1_ZF;
                        d.Land = s.LAND_BEZ_ZF;
                        d.Gesperrt = s.VERSANDSPERR.XToBool();
                    }));
            }
        }


        static public ModelMapping<Z_DPM_RETAIL_FLOORCHECK_01.GT_HAENDLER, FloorcheckHaendler> Z_DPM_RETAIL_FLOORCHECK_01_GT_HAENDLER_To_FloorcheckHaendler
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_RETAIL_FLOORCHECK_01.GT_HAENDLER, FloorcheckHaendler>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {                        
                        d.HaendlerNummer = s.KUNNR_BEIM_AG;
                        d.HaendlerOrt = s.CITY1;
                        d.HaendlerName = s.NAME;                       
                    }));
            }
        }

        static public ModelMapping<Z_DPM_RETAIL_FLOORCHECK_01.GT_DATEN, Floorcheck> Z_DPM_RETAIL_FLOORCHECK_01_GT_DATEN_To_Floorcheck
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_RETAIL_FLOORCHECK_01.GT_DATEN, Floorcheck>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.Fahrgestellnummer = s.CHASSIS_NUM;
                        d.Marke = s.ZZHERST_TEXT;
                        d.Handelsname = s.ZZHANDELSNAME;
                        d.Verwendung = s.FAHRZEUGART;
                        d.Farbe = s.ZFARBE_TEXT;
                        d.Versandgrund = s.VERS_GRUND;
                        d.Kilometerstand = s.KM_STAND;
                        d.Kreditnummer = s.PAID.TrimStart('0');
                        d.HaendlerNummer = s.KUNNR_BEIM_AG;
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
                                                 new Dictionary<string, string>(),
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

        static public ModelMapping<Z_DPM_IMP_MEL_CARP_01.GT_TAB, UploadAvisdaten> Z_DPM_IMP_MEL_CARP_01_GT_TAB_From_UploadAvisdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_IMP_MEL_CARP_01.GT_TAB, UploadAvisdaten>(
                    new Dictionary<string, string>()
                    , null
                    , (s, d) =>
                    {
                        d.AUFNR_AG = s.AuftragsNr;
                        d.CARPORT_ID_AG = s.Carport;
                        d.CHASSIS_NUM = s.FahrgestellNr;
                        d.LICENSE_NUM = s.Kennzeichen;
                        d.MVA_NUMMER = s.MvaNr;
                    }
                ));
            }
        }

        static public ModelMapping<Z_DPM_LIST_POOLS_001.GT_WEB, FahrzeuguebersichtSelektor> Z_DPM_LIST_POOLS_001_GT_WEB_From_FahrzeuguebersichtSelektor
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_LIST_POOLS_001.GT_WEB, FahrzeuguebersichtSelektor>(
                        new Dictionary<string, string>()
                        , null
                        , (source, destination) =>
                        {
                            destination.CHASSIS_NUM = source.Fahrgestellnummer;
                            destination.LICENSE_NUM = source.Kennzeichen;
                            destination.TIDNR = source.Zb2Nummer;
                            destination.ZZMODELL = source.ModelID;
                            destination.ZZREFERENZ1 = source.Unitnummer;
                            destination.LIZNR = source.Auftragsnummer;
                            destination.ZBATCH_ID = source.BatchId;
                            destination.ZZSIPP = source.SIPPCode;
                            destination.KUNPDI = source.Pdi;
                        }
                    ));
            }
        }

        static public ModelMapping<Z_DPM_LIST_POOLS_001.GT_WEB, Fahrzeuguebersicht> Z_DPM_LIST_POOLS_001_GT_WEB_From_Fahrzeuguebersicht
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_LIST_POOLS_001.GT_WEB, Fahrzeuguebersicht>(
                        new Dictionary<string, string>()
                        , null
                        , (source, destination) =>
                        {
                            destination.CHASSIS_NUM = source.Fahrgestellnummer;
                            destination.LICENSE_NUM = source.Kennzeichen;
                            destination.TIDNR = source.Zb2Nummer;
                            destination.ZZMODELL = source.ModelID;
                            destination.ZZREFERENZ1 = source.Unitnummer;
                            destination.LIZNR = source.Auftragsnummer;
                            destination.ZBATCH_ID = source.BatchId;
                            destination.ZZSIPP = source.SIPPCode;
                            destination.ZZHERST_TEXT = source.Hersteller;
                            destination.KUNPDI = source.Carport;
                        }
                    ));
            }
        }

        static public ModelMapping<Z_DPM_IMP_CARPORT_MELD_01.GT_WEB, CarporterfassungModel> Z_DPM_IMP_CARPORT_MELD_01_GT_WEB_From_CarporterfassungModel
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_IMP_CARPORT_MELD_01.GT_WEB, CarporterfassungModel>(
                        new Dictionary<string, string>()
                        , null
                        , (s, d) =>
                        {
                            d.ANZ_KENNZ_CPL = s.AnzahlKennzeichen;
                            d.AUFNR_AG = s.AuftragsNr;
                            d.BARCODE = s.Barcode;
                            d.CARPORT_ID_AG = s.CarportId;
                            d.CHASSIS_NUM = s.FahrgestellNr;
                            d.COC_VORH = CarporterfassungModel.GetMaterialVorhandenOptionSap(s.CocVorhanden);
                            d.DAT_DEMONT = s.DemontageDatum;
                            d.FZG_ABGEMELDET = s.Abgemeldet.BoolToX();
                            d.HU_AU_BER_VORH = CarporterfassungModel.GetMaterialVorhandenOptionSap(s.HuAuBerichtVorhanden);
                            d.KUNNR_AG = s.KundenNr;
                            d.LICENSE_NUM = s.Kennzeichen;
                            d.MVA_NUMMER = s.BestandsNr;
                            d.NAVI_CD_VORH = CarporterfassungModel.GetMaterialVorhandenOptionSap(s.NaviCdVorhanden);
                            d.SERVICEH_VORH = CarporterfassungModel.GetMaterialVorhandenOptionSap(s.ServiceheftVorhanden);
                            d.ZB1_VORH = CarporterfassungModel.GetMaterialVorhandenOptionSap(s.Zb1Vorhanden);
                            d.ZB2_VORH = CarporterfassungModel.GetMaterialVorhandenOptionSap(s.Zb2Vorhanden);
                            d.ZWEITSCHLUE_VORH = CarporterfassungModel.GetMaterialVorhandenOptionSap(s.ZweitschluesselVorhanden);
                            d.ORGANISATION = s.Organisation;
                        }
                    ));
            }
        }

        static public ModelMapping<Z_DPM_INS_CARPORT_NACHLIEF_01.GT_WEB, CarporterfassungModel> Z_DPM_INS_CARPORT_NACHLIEF_01_GT_WEB_From_CarporterfassungModel
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_INS_CARPORT_NACHLIEF_01.GT_WEB, CarporterfassungModel>(
                        new Dictionary<string, string>()
                        , null
                        , (s, d) =>
                        {
                            d.ANZ_KENNZ_CPL = s.AnzahlKennzeichen;
                            d.AUFTRAGS_NR = s.AuftragsNr;
                            d.BARCODE = s.Barcode;
                            d.CARPORT_ID_AG = s.CarportId;
                            d.CHASSIS_NUM = s.FahrgestellNr;
                            d.COC_VORH = CarporterfassungModel.GetMaterialVorhandenOptionSap(s.CocVorhanden);
                            d.DAT_DEMONT = s.DemontageDatum;
                            d.FZG_ABGEMELDET = s.Abgemeldet.BoolToX();
                            d.HU_AU_BER_VORH = CarporterfassungModel.GetMaterialVorhandenOptionSap(s.HuAuBerichtVorhanden);
                            d.KUNNR_AG = s.KundenNr;
                            d.LICENSE_NUM = s.Kennzeichen;
                            d.MVA_NUMMER = s.BestandsNr;
                            d.NAVI_CD_VORH = CarporterfassungModel.GetMaterialVorhandenOptionSap(s.NaviCdVorhanden);
                            d.SERVICEH_VORH = CarporterfassungModel.GetMaterialVorhandenOptionSap(s.ServiceheftVorhanden);
                            d.VORLAGE_ZB1_CPL = CarporterfassungModel.GetMaterialVorhandenOptionSap(s.Zb1Vorhanden);
                            d.ZB2_VORH = CarporterfassungModel.GetMaterialVorhandenOptionSap(s.Zb2Vorhanden);
                            d.ZWEITSCHLUE_VORH = CarporterfassungModel.GetMaterialVorhandenOptionSap(s.ZweitschluesselVorhanden);
                            d.ORGANISATION = s.Organisation;
                        }
                    ));
            }
        }

        #endregion
    }
}