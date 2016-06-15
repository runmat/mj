using System;
using System.Collections.Generic;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;

namespace AppZulassungsdienst.lib.Models
{
    public class AppModelMappings : ModelMappings
    {
        #region FromSap

        static private void Map_ZZLD_ADRS_To_ZLDAdressdaten(IZZLD_ADRS s, ZLDAdressdaten d)
        {
            d.Bemerkung = s.BEMERKUNG;
            d.KundenNr = s.KUNNR.NotNullOrEmpty().TrimStart('0');
            d.Loeschkennzeichen = s.LOEKZ.NotNullOrEmpty().Replace('X', 'L');
            d.Name1 = s.LI_NAME1;
            d.Name2 = s.LI_NAME2;
            d.Ort = s.LI_CITY1;
            d.Partnerrolle = s.PARVW;
            d.Plz = s.LI_PLZ;
            d.SapId = s.ZULBELN.NotNullOrEmpty().TrimStart('0');
            d.Strasse = s.LI_STREET;
            d.Land = s.LAND1;
        }

        static private void Map_ZZLD_BAK_To_ZLDKopfdaten(IZZLD_BAK s, ZLDKopfdaten d)
        {
            d.AnzahlKennzeichen = s.KENNZANZ;
            d.AuftragsNr = s.VBELN.NotNullOrEmpty().TrimStart('0');
            d.Barcode = s.BARCODE;
            d.BarzahlungKunde = s.KUNDEBAR_JN.XToBool();
            d.Bearbeitungsstatus = s.BEB_STATUS;
            d.Belegart = s.BLTYP;
            d.Bemerkung = s.BEMERKUNG;
            d.Erfasser = s.ERNAM;
            d.Erfassungsdatum = s.ERDAT;
            d.Fehlertext = s.ERROR_TEXT;
            d.Flieger = s.FLIEGER.XToBool();
            d.FrachtbriefNrHin = s.ZL_RL_FRBNR_HIN;
            d.FrachtbriefNrZurueck = s.ZL_RL_FRBNR_ZUR;
            d.Infotext = s.INFO_TEXT;
            d.Kennzeichen = s.ZZKENN;
            d.KennzeichenReservieren = s.RESERVKENN_JN.XToBool();
            d.Kennzeichenform = s.KENNZFORM;
            d.Kopfstatus = s.KSTATUS;
            d.KreisBezeichnung = s.KREISBEZ;
            d.KundenNr = s.KUNNR.NotNullOrEmpty().TrimStart('0');
            d.Landkreis = s.KREISKZ;
            d.LangtextNr = s.LTEXT_NR;
            d.LieferantenNr = s.ZL_LIFNR.NotNullOrEmpty().TrimStart('0');
            d.Nachbearbeiten = s.NACHBEARBEITEN.XToBool();
            d.MobilUser = s.MOBUSER;
            d.EvbNr = s.ZZEVB;
            d.NurEinKennzeichen = s.EINKENN_JN.XToBool();
            d.PraegelisteErstellt = s.PRALI_PRINT.XToBool();
            d.Referenz1 = s.ZZREFNR1;
            d.Referenz2 = s.ZZREFNR2;
            d.ReserviertesKennzeichen = s.RESERVKENN;
            d.SapId = s.ZULBELN.NotNullOrEmpty().TrimStart('0');
            d.StatusVersandzulassung = s.STATUS;
            d.VersandzulassungBearbeitungsstatus = s.VZB_STATUS;
            d.VersandzulassungDurchfuehrendesVkBur = s.VZD_VKBUR;
            d.VersandzulassungErledigtDatum = s.VZERDAT;
            d.VkBur = s.VKBUR;
            d.VkOrg = s.VKORG;
            d.Vorerfasser = s.VE_ERNAM;
            d.Vorerfassungsdatum = s.VE_ERDAT;
            d.Vorerfassungszeit = s.VE_ERZEIT;
            d.Wunschkennzeichen = s.WUNSCHKENN_JN.XToBool();
            d.Zahlart_Bar = s.BAR_JN.XToBool();
            d.Zahlart_EC = s.EC_JN.XToBool();
            d.Zahlart_Rechnung = s.RE_JN.XToBool();
            d.Zulassungsdatum = s.ZZZLDAT;
        }

        static private void Map_ZZLD_BANK_To_ZLDBankdaten(IZZLD_BANK s, ZLDBankdaten d)
        {
            d.Bankleitzahl = s.BANKL;
            d.Geldinstitut = s.EBPP_ACCNAME;
            d.Einzug = s.EINZ_JN.XToBool();
            d.IBAN = s.IBAN;
            d.KontoNr = s.BANKN;
            d.Kontoinhaber = s.KOINH;
            d.Loeschkennzeichen = s.LOEKZ.NotNullOrEmpty().Replace('X', 'L');
            d.Partnerrolle = s.PARVW;
            d.Rechnung = s.RECH_JN.XToBool();
            d.SWIFT = s.SWIFT;
            d.SapId = s.ZULBELN.NotNullOrEmpty().TrimStart('0');
        }

        static private void Map_ZZLD_POS_2_To_ZLDPosition(IZZLD_POS_2 s, ZLDPosition d)
        {
            d.Differenz = s.DIFF;
            d.GebuehrAmt = s.GEB_AMT;
            d.GebuehrAmtAdd = s.GEB_AMT_ADD;
            d.Gebuehrenpaket = s.GBPAK.XToBool();
            d.Kalkulationsdatum = s.CALCDAT;
            d.Konditionsart = s.KSCHL;
            d.Konditionstabelle = s.KONDTAB;
            d.Loeschkennzeichen = s.LOEKZ.NotNullOrEmpty().Replace('X', 'L');
            d.MaterialName = s.MAKTX;
            d.MaterialNr = s.MATNR.NotNullOrEmpty().TrimStart('0');
            d.Menge = s.MENGE;
            d.NullpreisErlaubt = s.NULLPREIS_OK.XToBool();
            d.PositionsNr = s.ZULPOSNR.NotNullOrEmpty().TrimStart('0');
            d.Preis = s.PREIS;
            d.SapId = s.ZULBELN.NotNullOrEmpty().TrimStart('0');
            d.SdRelevant = s.SD_REL.XToBool();
            d.UebergeordnetePosition = s.UEPOS.NotNullOrEmpty().TrimStart('0');
            d.UrspruenglicherPreis = s.UPREIS;
            d.WebBearbeitungsStatus = s.WEB_STATUS;
            d.WebMaterialart = s.WEBMTART;
        }

        static private void Map_ZZLD_ERROR_To_ZLDFehler(IZZLD_ERROR s, ZLDFehler d)
        {
            d.PositionsNr = s.ZULPOSNR.NotNullOrEmpty().TrimStart('0');
            d.SapId = s.ZULBELN.NotNullOrEmpty().TrimStart('0');
            d.FehlerText = s.ERROR_TEXT;
        }

        //Z_ALL_DEBI_CHECK_TABLES.GT_T005
        //Z_ALL_DEBI_CHECK_TABLES.GT_T016
        //Z_ALL_DEBI_CHECK_TABLES.GT_TPFK
        //Z_FIL_EFA_PLATARTIKEL.GT_PLATART
        //Z_FIL_EFA_PLATSTAMM.GT_PLATSTAMM
        //Z_FIL_EFA_UML_MAT.GT_MAT
        //Z_FIL_EFA_UML_MAT_GROESSE.GT_MAT
        //Z_FIL_EFA_UML_OFF_POS.GT_OFF_UML_POS
        //Z_FIL_EFA_UML_STEP1.GT_BELNR
        //Z_FIL_READ_OFF_BEST_001.GT_OFF_UML
        //Z_M_BAPIRDZ.ITAB
        //Z_M_ZGBS_BEN_ZULASSUNGSUNT.GT_WEB
        //Z_MC_CONNECT.GT_VORGART
        //Z_MC_CONNECT.GT_ROLLE_VGART
        //Z_MC_GET_IN_OUT.GT_IN
        //Z_MC_GET_IN_OUT.GT_OUT
        //Z_ZLD_AH_EX_VSZUL.GT_OUT

        static public ModelMapping<Z_ZLD_AH_VZ_SAVE2.GT_EX_ERRORS, ZLDFehler> Z_ZLD_AH_VZ_SAVE2_GT_EX_ERRORS_To_ZLDFehler
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_AH_VZ_SAVE2.GT_EX_ERRORS, ZLDFehler>(
                    new Dictionary<string, string>()
                    , Map_ZZLD_ERROR_To_ZLDFehler));
            }
        }

        //Z_ZLD_CJ2_CALC_MWST.GT_POS
        //Z_ZLD_CJ2_GET_ALL_DOCS.GT_DOCS
        //Z_ZLD_CJ2_GET_ALL_DOCS.GT_DOCS_P
        //Z_ZLD_CJ2_GET_ALL_DOCS.GT_TRANSACTIONS
        //Z_ZLD_CJ2_GET_TRANSACTIONS.GT_TRANSACTIONS
        //Z_ZLD_DOMAENEN_WERTE.GT_WERTE
        //Z_ZLD_EXPORT_AUSWERTUNG_1.GT_LISTE1

        static public ModelMapping<Z_ZLD_EXPORT_FILIAL_ADRESSE.ES_FIL_ADRS, Filialadresse> Z_ZLD_EXPORT_FILIAL_ADRESSE_ES_FIL_ADRS_To_Filialadresse
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_EXPORT_FILIAL_ADRESSE.ES_FIL_ADRS, Filialadresse>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.FaxDurchwahl = s.FAX_EXTENS;
                        d.FaxVorwahl = s.FAX_NUMBER;
                        d.HausNr = s.HOUSE_NUM1;
                        d.Name1 = s.NAME1;
                        d.Name2 = s.NAME2;
                        d.Ort = s.CITY1;
                        d.Plz = s.POST_CODE1;
                        d.Strasse = s.STREET;
                        d.TelefonDurchwahl = s.TEL_EXTENS;
                        d.TelefonVorwahl = s.TEL_NUMBER;
                        d.VkBur = s.VKBUR;
                        d.VkOrg = s.VKORG;
                    }));
            }
        }

        //Z_ZLD_EXPORT_INFOPOOL.GT_EX_ZUSTLIEF

        static public ModelMapping<Z_ZLD_EXPORT_KUNDE_MAT.GT_EX_KUNDE, Kundenstammdaten> Z_ZLD_EXPORT_KUNDE_MAT_GT_EX_KUNDE_To_Kundenstammdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_EXPORT_KUNDE_MAT.GT_EX_KUNDE, Kundenstammdaten>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.Bar = s.BARKUNDE.XToBool();
                        d.Cpd = s.XCPDK.XToBool();
                        d.CpdMitEinzug = s.XCPDEIN.XToBool();
                        d.HausNr = s.HOUSE_NUM1;
                        d.Inaktiv = s.INAKTIV.XToBool();
                        d.KundenNr = s.KUNNR.NotNullOrEmpty().TrimStart('0');
                        d.KundenNrLbv = s.KUNNR_LF.NotNullOrEmpty().TrimStart('0');
                        d.Land = s.COUNTRY;
                        d.Landkreis = s.KREISKZ_DIREKT;
                        d.Name1 = s.NAME1;
                        d.Namenserweiterung = s.EXTENSION1;
                        d.Name2 = s.NAME2;
                        d.OhneUst = s.OHNEUST.XToBool();
                        d.Ort = s.CITY1;
                        d.Pauschal = s.ZZPAUSCHAL.XToBool();
                        d.Plz = s.POST_CODE1;
                        d.Sofortabrechung = s.SOFORT_ABR.XToBool();
                        d.Strasse = s.STREET;
                        d.VkBur = s.VKBUR;
                        d.VkOrg = s.VKORG;
                    }));
            }
        }

        static public ModelMapping<Z_ZLD_EXPORT_KUNDE_MAT.GT_EX_MATERIAL, Materialstammdaten> Z_ZLD_EXPORT_KUNDE_MAT_GT_EX_MATERIAL_To_Materialstammdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_EXPORT_KUNDE_MAT.GT_EX_MATERIAL, Materialstammdaten>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.Gebuehrenpflichtig = s.ZZGEBPFLICHT.XToBool();
                        d.GebuehrenMaterialNr = s.GEBMAT.NotNullOrEmpty().TrimStart('0');
                        d.GebuehrenMaterialName = s.GMAKTX;
                        d.GebuehrenMitUstMaterialNr = s.GBAUST.NotNullOrEmpty().TrimStart('0');
                        d.GebuehrenMitUstMaterialName = s.GUMAKTX;
                        d.Inaktiv = s.INAKTIV.XToBool();
                        d.Kennzeichenrelevant = s.KENNZREL.XToBool();
                        d.KennzeichenMaterialNr = s.KENNZMAT.NotNullOrEmpty().TrimStart('0');
                        d.MaterialName = s.MAKTX;
                        d.MaterialNr = s.MATNR.NotNullOrEmpty().TrimStart('0');
                        d.MengeErlaubt = s.MENGE_ERL.XToBool();
                        d.NullpreisErlaubt = s.NULLPREIS_OK.XToBool();
                        d.VkBur = s.VKBUR;
                        d.VkOrg = s.VKORG;
                    }));
            }
        }

        //Z_ZLD_EXPORT_LS.GT_FILENAME

        static public ModelMapping<Z_ZLD_EXPORT_NACHERF2.GT_EX_ADRS, ZLDAdressdaten> Z_ZLD_EXPORT_NACHERF2_GT_EX_ADRS_To_ZLDAdressdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_EXPORT_NACHERF2.GT_EX_ADRS, ZLDAdressdaten>(
                    new Dictionary<string, string>()
                    , Map_ZZLD_ADRS_To_ZLDAdressdaten));
            }
        }

        static public ModelMapping<Z_ZLD_EXPORT_NACHERF2.GT_EX_BAK, ZLDKopfdaten> Z_ZLD_EXPORT_NACHERF2_GT_EX_BAK_To_ZLDKopfdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_EXPORT_NACHERF2.GT_EX_BAK, ZLDKopfdaten>(
                    new Dictionary<string, string>()
                    , Map_ZZLD_BAK_To_ZLDKopfdaten));
            }
        }

        static public ModelMapping<Z_ZLD_EXPORT_NACHERF2.GT_EX_BANK, ZLDBankdaten> Z_ZLD_EXPORT_NACHERF2_GT_EX_BANK_To_ZLDBankdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_EXPORT_NACHERF2.GT_EX_BANK, ZLDBankdaten>(
                    new Dictionary<string, string>()
                    , Map_ZZLD_BANK_To_ZLDBankdaten));
            }
        }

        static public ModelMapping<Z_ZLD_EXPORT_NACHERF2.GT_EX_KUNDE, Kundenname> Z_ZLD_EXPORT_NACHERF2_GT_EX_KUNDE_To_Kundenname
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_EXPORT_NACHERF2.GT_EX_KUNDE, Kundenname>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.KundenNr = s.KUNNR.NotNullOrEmpty().TrimStart('0');
                        d.Name1 = s.NAME1;
                        d.Namenserweiterung = s.EXTENSION1;
                    }));
            }
        }

        static public ModelMapping<Z_ZLD_EXPORT_NACHERF2.GT_EX_POS, ZLDPosition> Z_ZLD_EXPORT_NACHERF2_GT_EX_POS_To_ZLDPosition
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_EXPORT_NACHERF2.GT_EX_POS, ZLDPosition>(
                    new Dictionary<string, string>()
                    , Map_ZZLD_POS_2_To_ZLDPosition));
            }
        }

        //Z_ZLD_EXPORT_NEW_DEBI.GT_KUNDEN
        //Z_ZLD_EXPORT_PRALI.GT_BELEG

        static public ModelMapping<Z_ZLD_EXPORT_SOFORT_ABRECH2.GT_EX_ADRS, ZLDAdressdaten> Z_ZLD_EXPORT_SOFORT_ABRECH2_GT_EX_ADRS_To_ZLDAdressdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_EXPORT_SOFORT_ABRECH2.GT_EX_ADRS, ZLDAdressdaten>(
                    new Dictionary<string, string>()
                    , Map_ZZLD_ADRS_To_ZLDAdressdaten));
            }
        }

        static public ModelMapping<Z_ZLD_EXPORT_SOFORT_ABRECH2.GT_EX_BAK, ZLDKopfdaten> Z_ZLD_EXPORT_SOFORT_ABRECH2_GT_EX_BAK_To_ZLDKopfdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_EXPORT_SOFORT_ABRECH2.GT_EX_BAK, ZLDKopfdaten>(
                    new Dictionary<string, string>()
                    , Map_ZZLD_BAK_To_ZLDKopfdaten));
            }
        }

        static public ModelMapping<Z_ZLD_EXPORT_SOFORT_ABRECH2.GT_EX_BANK, ZLDBankdaten> Z_ZLD_EXPORT_SOFORT_ABRECH2_GT_EX_BANK_To_ZLDBankdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_EXPORT_SOFORT_ABRECH2.GT_EX_BANK, ZLDBankdaten>(
                    new Dictionary<string, string>()
                    , Map_ZZLD_BANK_To_ZLDBankdaten));
            }
        }

        static public ModelMapping<Z_ZLD_EXPORT_SOFORT_ABRECH2.GT_EX_KUNDE, Kundenname> Z_ZLD_EXPORT_SOFORT_ABRECH2_GT_EX_KUNDE_To_Kundenname
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_EXPORT_SOFORT_ABRECH2.GT_EX_KUNDE, Kundenname>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.KundenNr = s.KUNNR.NotNullOrEmpty().TrimStart('0');
                        d.Name1 = s.NAME1;
                        d.Namenserweiterung = s.EXTENSION1;
                    }));
            }
        }

        static public ModelMapping<Z_ZLD_EXPORT_SOFORT_ABRECH2.GT_EX_POS, ZLDPosition> Z_ZLD_EXPORT_SOFORT_ABRECH2_GT_EX_POS_To_ZLDPosition
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_EXPORT_SOFORT_ABRECH2.GT_EX_POS, ZLDPosition>(
                    new Dictionary<string, string>()
                    , Map_ZZLD_POS_2_To_ZLDPosition));
            }
        }

        //Z_ZLD_EXPORT_TAGLI.ES_FIL_ADRS
        //Z_ZLD_EXPORT_TAGLI.GT_TAGLI_BEM
        //Z_ZLD_EXPORT_TAGLI.GT_TAGLI_K
        //Z_ZLD_EXPORT_TAGLI.GT_TAGLI_P
        //Z_ZLD_EXPORT_VZOZUERL.GT_EX_ZUERL

        static public ModelMapping<Z_ZLD_EXPORT_ZULSTEL.GT_EX_ZULSTELL, Stva> Z_ZLD_EXPORT_ZULSTEL_GT_EX_ZULSTELL_To_Stva
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_EXPORT_ZULSTEL.GT_EX_ZULSTELL, Stva>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.KreisBezeichnung = s.KREISBEZ;
                        d.Landkreis = s.KREISKZ;
                        d.Url = s.URL;
                    }));
            }
        }

        static public ModelMapping<Z_ZLD_EXPORT_ZULSTEL.GT_SONDER, SonderStva> Z_ZLD_EXPORT_ZULSTEL_GT_SONDER_To_SonderStva
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_EXPORT_ZULSTEL.GT_SONDER, SonderStva>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.KfzKreiskennzeichen = s.ZKFZKZ;
                        d.Landkreis = s.KREISKZ;
                    }));
            }
        }

        //Z_ZLD_FIND_DAD_SD_ORDER.E_VBAK
        //Z_ZLD_GET_DAD_SD_ORDER.GS_DAD_ORDER
        //Z_ZLD_GET_DAD_SD_ORDER.GT_MAT

        static public ModelMapping<Z_ZLD_GET_GRUPPE.GT_GRUPPE, GruppeTour> Z_ZLD_GET_GRUPPE_GT_GRUPPE_To_GruppeTour
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_GET_GRUPPE.GT_GRUPPE, GruppeTour>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.Anlagedatum = s.ERDAT;
                        d.Gruppe = s.GRUPPE.NotNullOrEmpty().TrimStart('0');
                        d.GruppenArt = s.GRUPART;
                        d.GruppenName = s.BEZEI;
                        d.VkBur = s.VKBUR;
                    }));
            }
        }

        static public ModelMapping<Z_ZLD_GET_GRUPPE_KDZU.GT_KDZU, Kundenadresse> Z_ZLD_GET_GRUPPE_KDZU_GT_KDZU_To_Kundenadresse
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_GET_GRUPPE_KDZU.GT_KDZU, Kundenadresse>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.Anlagedatum = s.ERDAT;
                        d.HausNr = s.HOUSE_NUM1;
                        d.KundenNr = s.KUNNR.NotNullOrEmpty().TrimStart('0');
                        d.Name1 = s.NAME1;
                        d.Name2 = s.NAME2;
                        d.Namenserweiterung = s.EXTENSION1;
                        d.Ort = s.CITY1;
                        d.Plz = s.POST_CODE1;
                        d.Strasse = s.STREET;
                    }));
            }
        }
        
        static public ModelMapping<Z_ZLD_GET_ORDER2.GT_EX_ADRS, ZLDAdressdaten> Z_ZLD_GET_ORDER2_GT_EX_ADRS_To_ZLDAdressdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_GET_ORDER2.GT_EX_ADRS, ZLDAdressdaten>(
                    new Dictionary<string, string>()
                    , Map_ZZLD_ADRS_To_ZLDAdressdaten));
            }
        }

        static public ModelMapping<Z_ZLD_GET_ORDER2.GS_EX_BAK, ZLDKopfdaten> Z_ZLD_GET_ORDER2_GS_EX_BAK_To_ZLDKopfdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_GET_ORDER2.GS_EX_BAK, ZLDKopfdaten>(
                    new Dictionary<string, string>()
                    , Map_ZZLD_BAK_To_ZLDKopfdaten));
            }
        }

        static public ModelMapping<Z_ZLD_GET_ORDER2.GT_EX_BANK, ZLDBankdaten> Z_ZLD_GET_ORDER2_GT_EX_BANK_To_ZLDBankdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_GET_ORDER2.GT_EX_BANK, ZLDBankdaten>(
                    new Dictionary<string, string>()
                    , Map_ZZLD_BANK_To_ZLDBankdaten));
            }
        }

        static public ModelMapping<Z_ZLD_GET_ORDER2.GT_EX_POS, ZLDPosition> Z_ZLD_GET_ORDER2_GT_EX_POS_To_ZLDPosition
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_GET_ORDER2.GT_EX_POS, ZLDPosition>(
                    new Dictionary<string, string>()
                    , Map_ZZLD_POS_2_To_ZLDPosition));
            }
        }

        //Z_ZLD_IMP_KOMPER.GT_BARQ

        static public ModelMapping<Z_ZLD_IMP_KOMPER2.GT_EX_ERRORS, ZLDFehler> Z_ZLD_IMP_KOMPER2_GT_EX_ERRORS_To_ZLDFehler
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_IMP_KOMPER2.GT_EX_ERRORS, ZLDFehler>(
                    new Dictionary<string, string>()
                    , Map_ZZLD_ERROR_To_ZLDFehler));
            }
        }

        //Z_ZLD_IMP_NACHERF.GT_BARQ

        static public ModelMapping<Z_ZLD_IMP_NACHERF2.GT_EX_ERRORS, ZLDFehler> Z_ZLD_IMP_NACHERF2_GT_EX_ERRORS_To_ZLDFehler
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_IMP_NACHERF2.GT_EX_ERRORS, ZLDFehler>(
                    new Dictionary<string, string>()
                    , Map_ZZLD_ERROR_To_ZLDFehler));
            }
        }

        static public ModelMapping<Z_ZLD_IMP_NACHERF_DZLD2.GT_EX_ERRORS, ZLDFehler> Z_ZLD_IMP_NACHERF_DZLD2_GT_EX_ERRORS_To_ZLDFehler
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_IMP_NACHERF_DZLD2.GT_EX_ERRORS, ZLDFehler>(
                    new Dictionary<string, string>()
                    , Map_ZZLD_ERROR_To_ZLDFehler));
            }
        }

        static public ModelMapping<Z_ZLD_IMPORT_ERFASSUNG2.GT_EX_ERRORS, ZLDFehler> Z_ZLD_IMPORT_ERFASSUNG2_GT_EX_ERRORS_To_ZLDFehler
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_IMPORT_ERFASSUNG2.GT_EX_ERRORS, ZLDFehler>(
                    new Dictionary<string, string>()
                    , Map_ZZLD_ERROR_To_ZLDFehler));
            }
        }

        static public ModelMapping<Z_ZLD_IMPORT_SOFORT_ABRECH2.GT_EX_ERRORS, ZLDFehler> Z_ZLD_IMPORT_SOFORT_ABRECH2_GT_EX_ERRORS_To_ZLDFehler
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_IMPORT_SOFORT_ABRECH2.GT_EX_ERRORS, ZLDFehler>(
                    new Dictionary<string, string>()
                    , Map_ZZLD_ERROR_To_ZLDFehler));
            }
        }

        static public ModelMapping<Z_ZLD_MOB_DISPO_GET_VG.GT_VGANZ, AmtDispos> Z_ZLD_MOB_DISPO_GET_VG_GT_VGANZ_To_AmtDispos
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_MOB_DISPO_GET_VG.GT_VGANZ, AmtDispos>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.AnzahlVorgaenge = s.VG_ANZ;
                        d.KreisBezeichnung = s.KREISBEZ;
                        d.Amt = s.AMT;
                        d.MobileUserId = s.MOBUSER;
                        d.MobileUserName = s.NAME;
                        d.MobilAktiv = s.MOB_AKTIV.XToBool();
                        d.NoMobilAktiv = s.NO_MOB_AKTIV.XToBool();
                        d.GebuehrAmt = s.GEB_AMT;
                        d.Hinweis = s.HINWEIS;
                        d.Vorschuss = s.VORSCHUSS.XToBool();
                        d.VorschussBetrag = s.VORSCHUSS_BETRAG;
                        d.WaehrungsSchluessel = s.WAERS;
                    }));
            }
        }

        static public ModelMapping<Z_ZLD_MOB_EXPORT_ANGENOMMENE.GT_EX_ADRS, ZLDAdressdaten> Z_ZLD_MOB_EXPORT_ANGENOMMENE_GT_EX_ADRS_To_ZLDAdressdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_MOB_EXPORT_ANGENOMMENE.GT_EX_ADRS, ZLDAdressdaten>(
                    new Dictionary<string, string>()
                    , Map_ZZLD_ADRS_To_ZLDAdressdaten));
            }
        }

        static public ModelMapping<Z_ZLD_MOB_EXPORT_ANGENOMMENE.GT_EX_BAK, ZLDKopfdaten> Z_ZLD_MOB_EXPORT_ANGENOMMENE_GT_EX_BAK_To_ZLDKopfdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_MOB_EXPORT_ANGENOMMENE.GT_EX_BAK, ZLDKopfdaten>(
                    new Dictionary<string, string>()
                    , Map_ZZLD_BAK_To_ZLDKopfdaten));
            }
        }

        static public ModelMapping<Z_ZLD_MOB_EXPORT_ANGENOMMENE.GT_EX_BANK, ZLDBankdaten> Z_ZLD_MOB_EXPORT_ANGENOMMENE_GT_EX_BANK_To_ZLDBankdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_MOB_EXPORT_ANGENOMMENE.GT_EX_BANK, ZLDBankdaten>(
                    new Dictionary<string, string>()
                    , Map_ZZLD_BANK_To_ZLDBankdaten));
            }
        }

        static public ModelMapping<Z_ZLD_MOB_EXPORT_ANGENOMMENE.GT_EX_KUNDE, Kundenname> Z_ZLD_MOB_EXPORT_ANGENOMMENE_GT_EX_KUNDE_To_Kundenname
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_MOB_EXPORT_ANGENOMMENE.GT_EX_KUNDE, Kundenname>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.KundenNr = s.KUNNR.NotNullOrEmpty().TrimStart('0');
                        d.Name1 = s.NAME1;
                        d.Namenserweiterung = s.EXTENSION1;
                    }));
            }
        }

        static public ModelMapping<Z_ZLD_MOB_EXPORT_ANGENOMMENE.GT_EX_POS, ZLDPosition> Z_ZLD_MOB_EXPORT_ANGENOMMENE_GT_EX_POS_To_ZLDPosition
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_MOB_EXPORT_ANGENOMMENE.GT_EX_POS, ZLDPosition>(
                    new Dictionary<string, string>()
                    , Map_ZZLD_POS_2_To_ZLDPosition));
            }
        }

        static public ModelMapping<Z_ZLD_MOB_GET_USER.GT_USER, MobileUser> Z_ZLD_MOB_GET_USER_GT_USER_To_MobileUser
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_MOB_GET_USER.GT_USER, MobileUser>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.UserId = s.MOBUSER;
                        d.UserName = s.NAME;
                    }));
            }
        }

        //Z_ZLD_MOB_GET_VG_FOR_UPD.GT_VG_STAT

        static public ModelMapping<Z_ZLD_PREISFINDUNG2.GT_POS, ZLDPosition> Z_ZLD_PREISFINDUNG2_GT_POS_To_ZLDPosition
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_PREISFINDUNG2.GT_POS, ZLDPosition>(
                    new Dictionary<string, string>()
                    , Map_ZZLD_POS_2_To_ZLDPosition));
            }
        }

        static public ModelMapping<Z_ZLD_SAVE_DATA2.GT_EX_ERRORS, ZLDFehler> Z_ZLD_SAVE_DATA2_GT_EX_ERRORS_To_ZLDFehler
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_SAVE_DATA2.GT_EX_ERRORS, ZLDFehler>(
                    new Dictionary<string, string>()
                    , Map_ZZLD_ERROR_To_ZLDFehler));
            }
        }

        static public ModelMapping<Z_ZLD_STO_GET_ORDER2.GT_ADRS, ZLDAdressdaten> Z_ZLD_STO_GET_ORDER2_GT_ADRS_To_ZLDAdressdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_STO_GET_ORDER2.GT_ADRS, ZLDAdressdaten>(
                    new Dictionary<string, string>()
                    , Map_ZZLD_ADRS_To_ZLDAdressdaten));
            }
        }

        static public ModelMapping<Z_ZLD_STO_GET_ORDER2.ES_BAK, ZLDKopfdaten> Z_ZLD_STO_GET_ORDER2_ES_BAK_To_ZLDKopfdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_STO_GET_ORDER2.ES_BAK, ZLDKopfdaten>(
                    new Dictionary<string, string>()
                    , Map_ZZLD_BAK_To_ZLDKopfdaten));
            }
        }

        static public ModelMapping<Z_ZLD_STO_GET_ORDER2.ES_BANK, ZLDBankdaten> Z_ZLD_STO_GET_ORDER2_ES_BANK_To_ZLDBankdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_STO_GET_ORDER2.ES_BANK, ZLDBankdaten>(
                    new Dictionary<string, string>()
                    , Map_ZZLD_BANK_To_ZLDBankdaten));
            }
        }

        static public ModelMapping<Z_ZLD_STO_GET_ORDER2.GT_POS, ZLDPosition> Z_ZLD_STO_GET_ORDER2_GT_POS_To_ZLDPosition
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_STO_GET_ORDER2.GT_POS, ZLDPosition>(
                    new Dictionary<string, string>()
                    , Map_ZZLD_POS_2_To_ZLDPosition));
            }
        }

        //Z_ZLD_STO_STORNO_LISTE.GT_LISTE
        //Z_ZLD_STO_STORNOGRUENDE.GT_GRUENDE

        static public ModelMapping<Z_ZLD_EXPORT_AH_WARENKORB.GT_BAK, NochNichtAbgesendeterVorgang> Z_ZLD_EXPORT_AH_WARENKORB_GT_BAK_To_NochNichtAbgesendeterVorgang
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_EXPORT_AH_WARENKORB.GT_BAK, NochNichtAbgesendeterVorgang>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.Bemerkung = s.BEMERKUNG;
                        d.Kennzeichen = s.ZZKENN;
                        d.KundenNr = s.KUNNR.NotNullOrEmpty().TrimStart('0');
                        d.MaterialName = s.MAKTX;
                        d.Name1 = s.NAME1;
                        d.Referenz1 = s.ZZREFNR1;
                        d.Referenz2 = s.ZZREFNR2;
                        d.SapId = s.ZULBELN.NotNullOrEmpty().TrimStart('0');
                        d.Zulassungsdatum = s.ZZZLDAT;
                    }));
            }
        }

        static public ModelMapping<Z_ZLD_IMPORT_AH_WARENKORB.GT_BAK, NochNichtAbgesendeterVorgang> Z_ZLD_IMPORT_AH_WARENKORB_GT_BAK_To_NochNichtAbgesendeterVorgang
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_IMPORT_AH_WARENKORB.GT_BAK, NochNichtAbgesendeterVorgang>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.FehlerText = s.MESSAGE;
                        d.SapId = s.ZULBELN.NotNullOrEmpty().TrimStart('0');
                    }));
            }
        }

        static public ModelMapping<Z_ZLD_AH_2015_ETIKETT_SEL.ET_BAK, Kennzeichenetikett> Z_ZLD_AH_2015_ETIKETT_SEL_ET_BAK_To_Kennzeichenetikett
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_AH_2015_ETIKETT_SEL.ET_BAK, Kennzeichenetikett>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.Fahrzeugtyp = s.FZGTYP;
                        d.Farbe = s.FARBE;
                        d.Kennzeichen = s.ZZKENN;
                        d.KundenName = s.NAME;
                        d.KundenNr = s.KUNNR.NotNullOrEmpty().TrimStart('0');
                        d.Referenz1 = s.ZZREFNR1;
                        d.Referenz2 = s.ZZREFNR2;
                        d.SapId = s.ZULBELN.NotNullOrEmpty().TrimStart('0');
                        d.Zulassungsdatum = s.ZZZLDAT;
                    }));
            }
        }

        #endregion


        #region ToSap

        static private void Map_ZLDAdressdaten_To_ZZLD_ADRS(ZLDAdressdaten s, IZZLD_ADRS d)
        {
            d.BEMERKUNG = s.Bemerkung;
            d.KUNNR = s.KundenNr.NotNullOrEmpty().ToSapKunnr();
            d.LI_CITY1 = s.Ort;
            d.LI_NAME1 = s.Name1;
            d.LI_NAME2 = s.Name2;
            d.LI_PLZ = s.Plz;
            d.LI_STREET = s.Strasse;
            d.LOEKZ = s.Loeschkennzeichen.NotNullOrEmpty().Replace('L', 'X');
            d.PARVW = s.Partnerrolle;
            d.ZULBELN = (String.IsNullOrEmpty(s.SapId) ? "" : s.SapId.PadLeft0(10));
            d.LAND1 = s.Land;
        }

        static private void Map_ZLDKopfdaten_To_ZZLD_BAK(ZLDKopfdaten s, IZZLD_BAK d)
        {
            d.BARCODE = s.Barcode;
            d.BAR_JN = s.Zahlart_Bar.BoolToX();
            d.BEB_STATUS = s.Bearbeitungsstatus;
            d.BEMERKUNG = s.Bemerkung;
            d.BLTYP = s.Belegart;
            d.EC_JN = s.Zahlart_EC.BoolToX();
            d.EINKENN_JN = s.NurEinKennzeichen.BoolToX();
            d.ERDAT = s.Erfassungsdatum;
            d.ERNAM = s.Erfasser;
            d.ERROR_TEXT = s.Fehlertext;
            d.FLIEGER = s.Flieger.BoolToX();
            d.INFO_TEXT = s.Infotext;
            d.KENNZANZ = s.AnzahlKennzeichen;
            d.KENNZFORM = s.Kennzeichenform;
            d.KREISBEZ = s.KreisBezeichnung;
            d.KREISKZ = s.Landkreis;
            d.KSTATUS = s.Kopfstatus;
            d.KUNDEBAR_JN = s.BarzahlungKunde.BoolToX();
            d.KUNNR = s.KundenNr.NotNullOrEmpty().ToSapKunnr();
            d.LOEKZ = s.Loeschkennzeichen.NotNullOrEmpty().Replace('L', 'X');
            d.LTEXT_NR = s.LangtextNr;
            d.NACHBEARBEITEN = s.Nachbearbeiten.BoolToX();
            d.MOBUSER = s.MobilUser;
            d.ZZEVB = s.EvbNr;
            d.PRALI_PRINT = s.PraegelisteErstellt.BoolToX();
            d.RESERVKENN = s.ReserviertesKennzeichen;
            d.RESERVKENN_JN = s.KennzeichenReservieren.BoolToX();
            d.RE_JN = s.Zahlart_Rechnung.BoolToX();
            d.STATUS = s.StatusVersandzulassung;
            d.VBELN = (String.IsNullOrEmpty(s.AuftragsNr) ? "" : s.AuftragsNr.PadLeft0(10));
            d.VE_ERDAT = s.Vorerfassungsdatum;
            d.VE_ERZEIT = s.Vorerfassungszeit;
            d.VE_ERNAM = s.Vorerfasser;
            d.VKBUR = s.VkBur;
            d.VKORG = s.VkOrg;
            d.VZB_STATUS = s.VersandzulassungBearbeitungsstatus;
            d.VZD_VKBUR = s.VersandzulassungDurchfuehrendesVkBur;
            d.VZERDAT = s.VersandzulassungErledigtDatum;
            d.WUNSCHKENN_JN = s.Wunschkennzeichen.BoolToX();
            d.ZL_LIFNR = (String.IsNullOrEmpty(s.LieferantenNr) ? "" : s.LieferantenNr.ToSapKunnr());
            d.ZL_RL_FRBNR_HIN = s.FrachtbriefNrHin;
            d.ZL_RL_FRBNR_ZUR = s.FrachtbriefNrZurueck;
            d.ZULBELN = (String.IsNullOrEmpty(s.SapId) ? "" : s.SapId.PadLeft0(10));
            d.ZZKENN = s.Kennzeichen;
            d.ZZREFNR1 = s.Referenz1;
            d.ZZREFNR2 = s.Referenz2;
            d.ZZZLDAT = s.Zulassungsdatum;
        }

        static private void Map_ZLDBankdaten_To_ZZLD_BANK(ZLDBankdaten s, IZZLD_BANK d)
        {
            d.BANKL = s.Bankleitzahl;
            d.BANKN = s.KontoNr;
            d.EBPP_ACCNAME = s.Geldinstitut;
            d.EINZ_JN = s.Einzug.BoolToX();
            d.IBAN = s.IBAN;
            d.KOINH = s.Kontoinhaber;
            d.LOEKZ = s.Loeschkennzeichen.NotNullOrEmpty().Replace('L', 'X');
            d.PARVW = s.Partnerrolle;
            d.RECH_JN = s.Rechnung.BoolToX();
            d.SWIFT = s.SWIFT;
            d.ZULBELN = (String.IsNullOrEmpty(s.SapId) ? "" : s.SapId.PadLeft0(10));
        }

        static private void Map_ZLDPositionVorerfassung_To_ZZLD_POS(ZLDPositionVorerfassung s, IZZLD_POS d)
        {
            d.MAKTX = s.MaterialName;
            d.MATNR = (String.IsNullOrEmpty(s.MaterialNr) ? "" : s.MaterialNr.PadLeft0(18));
            d.MENGE = s.Menge;
            d.UEPOS = (String.IsNullOrEmpty(s.UebergeordnetePosition) ? "" : s.UebergeordnetePosition.PadLeft0(6));
            d.WEBMTART = s.WebMaterialart;
            d.ZULBELN = s.SapId.NotNullOrEmpty().ToSapKunnr();
            d.ZULPOSNR = (String.IsNullOrEmpty(s.PositionsNr) ? "" : s.PositionsNr.PadLeft0(6));
            d.NULLPREIS_OK = s.NullpreisErlaubt.BoolToX();
        }

        static private void Map_ZLDPosition_To_ZZLD_POS_2(ZLDPosition s, IZZLD_POS_2 d)
        {
            d.CALCDAT = s.Kalkulationsdatum;
            d.DIFF = s.Differenz;
            d.GBPAK = s.Gebuehrenpaket.BoolToX();
            d.GEB_AMT = s.GebuehrAmt;
            d.GEB_AMT_ADD = s.GebuehrAmtAdd;
            d.KONDTAB = s.Konditionstabelle;
            d.KSCHL = s.Konditionsart;
            d.LOEKZ = s.Loeschkennzeichen.NotNullOrEmpty().Replace('L', 'X');
            d.MAKTX = s.MaterialName;
            d.MATNR = (String.IsNullOrEmpty(s.MaterialNr) ? "" : s.MaterialNr.PadLeft0(18));
            d.MENGE = s.Menge;
            d.NULLPREIS_OK = s.NullpreisErlaubt.BoolToX();
            d.PREIS = s.Preis;
            d.SD_REL = s.SdRelevant.BoolToX();
            d.UEPOS = (String.IsNullOrEmpty(s.UebergeordnetePosition) ? "" : s.UebergeordnetePosition.PadLeft0(6));
            d.UPREIS = s.UrspruenglicherPreis;
            d.WEB_STATUS = s.WebBearbeitungsStatus;
            d.WEBMTART = s.WebMaterialart;
            d.ZULBELN = s.SapId.NotNullOrEmpty().ToSapKunnr();
            d.ZULPOSNR = (String.IsNullOrEmpty(s.PositionsNr) ? "" : s.PositionsNr.PadLeft0(6));
        }

        //Z_ALL_DEBI_VORERFASSUNG_WEB.GS_IN
        //Z_FIL_EFA_PO_CREATE.GT_POS
        //Z_FIL_EFA_UML_STEP1.GT_MAT
        //Z_FIL_EFA_UML_STEP2.GT_OFF_UML_POS

        static public ModelMapping<Z_ZLD_AH_VZ_SAVE2.GT_ADRS, ZLDAdressdaten> Z_ZLD_AH_VZ_SAVE2_GT_ADRS_From_ZLDAdressdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_AH_VZ_SAVE2.GT_ADRS, ZLDAdressdaten>(
                    new Dictionary<string, string>()
                    , null
                    , Map_ZLDAdressdaten_To_ZZLD_ADRS));
            }
        }

        static public ModelMapping<Z_ZLD_AH_VZ_SAVE2.GT_BAK, ZLDKopfdaten> Z_ZLD_AH_VZ_SAVE2_GT_BAK_From_ZLDKopfdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_AH_VZ_SAVE2.GT_BAK, ZLDKopfdaten>(
                    new Dictionary<string, string>()
                    , null
                    , Map_ZLDKopfdaten_To_ZZLD_BAK));
            }
        }

        static public ModelMapping<Z_ZLD_AH_VZ_SAVE2.GT_BANK, ZLDBankdaten> Z_ZLD_AH_VZ_SAVE2_GT_BANK_From_ZLDBankdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_AH_VZ_SAVE2.GT_BANK, ZLDBankdaten>(
                    new Dictionary<string, string>()
                    , null
                    , Map_ZLDBankdaten_To_ZZLD_BANK));
            }
        }

        static public ModelMapping<Z_ZLD_AH_VZ_SAVE2.GT_POS, ZLDPosition> Z_ZLD_AH_VZ_SAVE2_GT_POS_From_ZLDPosition
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_AH_VZ_SAVE2.GT_POS, ZLDPosition>(
                    new Dictionary<string, string>()
                    , null
                    , Map_ZLDPosition_To_ZZLD_POS_2));
            }
        }

        static public ModelMapping<Z_ZLD_BARABHEBUNG.IS_BARABHEBUNG, Barabhebung> Z_ZLD_BARABHEBUNG_IS_BARABHEBUNG_From_Barabhebung
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_BARABHEBUNG.IS_BARABHEBUNG, Barabhebung>(
                    new Dictionary<string, string>()
                    , null
                    , (s, d) =>
                    {
                        d.BETRAG = s.Betrag.ToNullableDecimal();
                        d.DATUM = s.Datum;
                        d.EC_KARTE_NR = s.EcKarteNr;
                        d.NAME = s.Name;
                        d.ORT = s.Ort;
                        d.UZEIT = s.Uhrzeit.NotNullOrEmpty().Replace(":", "").PadRight(6, '0');
                        d.VKBUR = s.VkBur;
                        d.WAERS = s.Waehrung;
                    }));
            }
        }

        //Z_ZLD_CHANGE_VZOZUERL.GT_IMP_VZOZUERL
        //Z_ZLD_CJ2_CALC_MWST.IS_DOCS_K
        //Z_ZLD_CJ2_CALC_MWST.GT_POS
        //Z_ZLD_CJ2_SAVE_DOC.IS_DOCS_K
        //Z_ZLD_CJ2_SAVE_DOC.GT_DOCS_P

        static public ModelMapping<Z_ZLD_IMP_KOMPER2.GT_IMP_ADRS, ZLDAdressdaten> Z_ZLD_IMP_KOMPER2_GT_IMP_ADRS_From_ZLDAdressdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_IMP_KOMPER2.GT_IMP_ADRS, ZLDAdressdaten>(
                    new Dictionary<string, string>()
                    , null
                    , Map_ZLDAdressdaten_To_ZZLD_ADRS));
            }
        }

        static public ModelMapping<Z_ZLD_IMP_KOMPER2.GT_IMP_BAK, ZLDKopfdaten> Z_ZLD_IMP_KOMPER2_GT_IMP_BAK_From_ZLDKopfdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_IMP_KOMPER2.GT_IMP_BAK, ZLDKopfdaten>(
                    new Dictionary<string, string>()
                    , null
                    , Map_ZLDKopfdaten_To_ZZLD_BAK));
            }
        }

        static public ModelMapping<Z_ZLD_IMP_KOMPER2.GT_IMP_BANK, ZLDBankdaten> Z_ZLD_IMP_KOMPER2_GT_IMP_BANK_From_ZLDBankdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_IMP_KOMPER2.GT_IMP_BANK, ZLDBankdaten>(
                    new Dictionary<string, string>()
                    , null
                    , Map_ZLDBankdaten_To_ZZLD_BANK));
            }
        }

        static public ModelMapping<Z_ZLD_IMP_KOMPER2.GT_IMP_POS, ZLDPosition> Z_ZLD_IMP_KOMPER2_GT_IMP_POS_From_ZLDPosition
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_IMP_KOMPER2.GT_IMP_POS, ZLDPosition>(
                    new Dictionary<string, string>()
                    , null
                    , Map_ZLDPosition_To_ZZLD_POS_2));
            }
        }

        static public ModelMapping<Z_ZLD_IMP_NACHERF2.GT_IMP_ADRS, ZLDAdressdaten> Z_ZLD_IMP_NACHERF2_GT_IMP_ADRS_From_ZLDAdressdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_IMP_NACHERF2.GT_IMP_ADRS, ZLDAdressdaten>(
                    new Dictionary<string, string>()
                    , null
                    , Map_ZLDAdressdaten_To_ZZLD_ADRS));
            }
        }

        static public ModelMapping<Z_ZLD_IMP_NACHERF2.GT_IMP_BAK, ZLDKopfdaten> Z_ZLD_IMP_NACHERF2_GT_IMP_BAK_From_ZLDKopfdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_IMP_NACHERF2.GT_IMP_BAK, ZLDKopfdaten>(
                    new Dictionary<string, string>()
                    , null
                    , Map_ZLDKopfdaten_To_ZZLD_BAK));
            }
        }

        static public ModelMapping<Z_ZLD_IMP_NACHERF2.GT_IMP_BANK, ZLDBankdaten> Z_ZLD_IMP_NACHERF2_GT_IMP_BANK_From_ZLDBankdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_IMP_NACHERF2.GT_IMP_BANK, ZLDBankdaten>(
                    new Dictionary<string, string>()
                    , null
                    , Map_ZLDBankdaten_To_ZZLD_BANK));
            }
        }

        static public ModelMapping<Z_ZLD_IMP_NACHERF2.GT_IMP_POS, ZLDPosition> Z_ZLD_IMP_NACHERF2_GT_IMP_POS_From_ZLDPosition
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_IMP_NACHERF2.GT_IMP_POS, ZLDPosition>(
                    new Dictionary<string, string>()
                    , null
                    , Map_ZLDPosition_To_ZZLD_POS_2));
            }
        }

        static public ModelMapping<Z_ZLD_IMP_NACHERF_DZLD2.GT_IMP_BAK, ZLDKopfdaten> Z_ZLD_IMP_NACHERF_DZLD2_GT_IMP_BAK_From_ZLDKopfdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_IMP_NACHERF_DZLD2.GT_IMP_BAK, ZLDKopfdaten>(
                    new Dictionary<string, string>()
                    , null
                    , Map_ZLDKopfdaten_To_ZZLD_BAK));
            }
        }

        static public ModelMapping<Z_ZLD_IMP_NACHERF_DZLD2.GT_IMP_POS, ZLDPosition> Z_ZLD_IMP_NACHERF_DZLD2_GT_IMP_POS_From_ZLDPosition
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_IMP_NACHERF_DZLD2.GT_IMP_POS, ZLDPosition>(
                    new Dictionary<string, string>()
                    , null
                    , Map_ZLDPosition_To_ZZLD_POS_2));
            }
        }

        static public ModelMapping<Z_ZLD_IMPORT_ERFASSUNG2.GT_IMP_ADRS, ZLDAdressdaten> Z_ZLD_IMPORT_ERFASSUNG2_GT_IMP_ADRS_From_ZLDAdressdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_IMPORT_ERFASSUNG2.GT_IMP_ADRS, ZLDAdressdaten>(
                    new Dictionary<string, string>()
                    , null
                    , Map_ZLDAdressdaten_To_ZZLD_ADRS));
            }
        }

        static public ModelMapping<Z_ZLD_IMPORT_ERFASSUNG2.GT_IMP_BAK, ZLDKopfdaten> Z_ZLD_IMPORT_ERFASSUNG2_GT_IMP_BAK_From_ZLDKopfdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_IMPORT_ERFASSUNG2.GT_IMP_BAK, ZLDKopfdaten>(
                    new Dictionary<string, string>()
                    , null
                    , Map_ZLDKopfdaten_To_ZZLD_BAK));
            }
        }

        static public ModelMapping<Z_ZLD_IMPORT_ERFASSUNG2.GT_IMP_BANK, ZLDBankdaten> Z_ZLD_IMPORT_ERFASSUNG2_GT_IMP_BANK_From_ZLDBankdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_IMPORT_ERFASSUNG2.GT_IMP_BANK, ZLDBankdaten>(
                    new Dictionary<string, string>()
                    , null
                    , Map_ZLDBankdaten_To_ZZLD_BANK));
            }
        }

        static public ModelMapping<Z_ZLD_IMPORT_ERFASSUNG2.GT_IMP_POS, ZLDPositionVorerfassung> Z_ZLD_IMPORT_ERFASSUNG2_GT_IMP_POS_From_ZLDPositionVorerfassung
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_IMPORT_ERFASSUNG2.GT_IMP_POS, ZLDPositionVorerfassung>(
                    new Dictionary<string, string>()
                    , null
                    , Map_ZLDPositionVorerfassung_To_ZZLD_POS));
            }
        }

        static public ModelMapping<Z_ZLD_IMPORT_SOFORT_ABRECH2.GT_IMP_ADRS, ZLDAdressdaten> Z_ZLD_IMPORT_SOFORT_ABRECH2_GT_IMP_ADRS_From_ZLDAdressdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_IMPORT_SOFORT_ABRECH2.GT_IMP_ADRS, ZLDAdressdaten>(
                    new Dictionary<string, string>()
                    , null
                    , Map_ZLDAdressdaten_To_ZZLD_ADRS));
            }
        }

        static public ModelMapping<Z_ZLD_IMPORT_SOFORT_ABRECH2.GT_IMP_BAK, ZLDKopfdaten> Z_ZLD_IMPORT_SOFORT_ABRECH2_GT_IMP_BAK_From_ZLDKopfdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_IMPORT_SOFORT_ABRECH2.GT_IMP_BAK, ZLDKopfdaten>(
                    new Dictionary<string, string>()
                    , null
                    , Map_ZLDKopfdaten_To_ZZLD_BAK));
            }
        }

        static public ModelMapping<Z_ZLD_IMPORT_SOFORT_ABRECH2.GT_IMP_BANK, ZLDBankdaten> Z_ZLD_IMPORT_SOFORT_ABRECH2_GT_IMP_BANK_From_ZLDBankdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_IMPORT_SOFORT_ABRECH2.GT_IMP_BANK, ZLDBankdaten>(
                    new Dictionary<string, string>()
                    , null
                    , Map_ZLDBankdaten_To_ZZLD_BANK));
            }
        }

        static public ModelMapping<Z_ZLD_IMPORT_SOFORT_ABRECH2.GT_IMP_POS, ZLDPosition> Z_ZLD_IMPORT_SOFORT_ABRECH2_GT_IMP_POS_From_ZLDPosition
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_IMPORT_SOFORT_ABRECH2.GT_IMP_POS, ZLDPosition>(
                    new Dictionary<string, string>()
                    , null
                    , Map_ZLDPosition_To_ZZLD_POS_2));
            }
        }

        static public ModelMapping<Z_ZLD_IMPORT_SOFORT_ABRECH2.GT_IMP_WEBUSER_DATEN, Userdaten> Z_ZLD_IMPORT_SOFORT_ABRECH2_GT_IMP_WEBUSER_DATEN_From_Userdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_IMPORT_SOFORT_ABRECH2.GT_IMP_WEBUSER_DATEN, Userdaten>(
                    new Dictionary<string, string>()
                    , null
                    , (s, d) =>
                        {
                            d.ERNAM = s.UserName;
                            d.VNAME = s.Vorname;
                            d.NNAME = s.Nachname;
                        }));
            }
        }

        //Z_ZLD_IMPORT_ZULUNT.GS_WEB

        static public ModelMapping<Z_ZLD_MOB_DISPO_SET_USER.GT_VGANZ, AmtDispos> Z_ZLD_MOB_DISPO_SET_USER_GT_VGANZ_From_AmtDispos
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_MOB_DISPO_SET_USER.GT_VGANZ, AmtDispos>(
                    new Dictionary<string, string>()
                    , null
                    , (s, d) =>
                    {
                        d.AMT = s.Amt;
                        d.KREISBEZ = s.KreisBezeichnung;
                        d.MOBUSER = s.MobileUserId;
                        d.NAME = s.MobileUserName;
                        d.VG_ANZ = s.AnzahlVorgaenge;
                        d.MOB_AKTIV = s.MobilAktiv.BoolToX();
                        d.NO_MOB_AKTIV = s.NoMobilAktiv.BoolToX();
                        d.GEB_AMT = s.GebuehrAmt;
                        d.HINWEIS = s.Hinweis;
                        d.VORSCHUSS = s.Vorschuss.BoolToX();
                        d.VORSCHUSS_BETRAG = s.VorschussBetrag;
                        d.WAERS = s.WaehrungsSchluessel;
                    }));
            }
        }

        static public ModelMapping<Z_ZLD_PREISFINDUNG2.GT_BAK, ZLDKopfdaten> Z_ZLD_PREISFINDUNG2_GT_BAK_From_ZLDKopfdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_PREISFINDUNG2.GT_BAK, ZLDKopfdaten>(
                    new Dictionary<string, string>()
                    , null
                    , Map_ZLDKopfdaten_To_ZZLD_BAK));
            }
        }

        static public ModelMapping<Z_ZLD_PREISFINDUNG2.GT_POS, ZLDPosition> Z_ZLD_PREISFINDUNG2_GT_POS_From_ZLDPosition
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_PREISFINDUNG2.GT_POS, ZLDPosition>(
                    new Dictionary<string, string>()
                    , null
                    , Map_ZLDPosition_To_ZZLD_POS_2));
            }
        }

        static public ModelMapping<Z_ZLD_SAVE_DATA2.GT_IMP_ADRS, ZLDAdressdaten> Z_ZLD_SAVE_DATA2_GT_IMP_ADRS_From_ZLDAdressdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_SAVE_DATA2.GT_IMP_ADRS, ZLDAdressdaten>(
                    new Dictionary<string, string>()
                    , null
                    , Map_ZLDAdressdaten_To_ZZLD_ADRS));
            }
        }

        static public ModelMapping<Z_ZLD_SAVE_DATA2.GT_IMP_BAK, ZLDKopfdaten> Z_ZLD_SAVE_DATA2_GT_IMP_BAK_From_ZLDKopfdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_SAVE_DATA2.GT_IMP_BAK, ZLDKopfdaten>(
                    new Dictionary<string, string>()
                    , null
                    , Map_ZLDKopfdaten_To_ZZLD_BAK));
            }
        }

        static public ModelMapping<Z_ZLD_SAVE_DATA2.GT_IMP_BANK, ZLDBankdaten> Z_ZLD_SAVE_DATA2_GT_IMP_BANK_From_ZLDBankdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_SAVE_DATA2.GT_IMP_BANK, ZLDBankdaten>(
                    new Dictionary<string, string>()
                    , null
                    , Map_ZLDBankdaten_To_ZZLD_BANK));
            }
        }

        static public ModelMapping<Z_ZLD_SAVE_DATA2.GT_IMP_POS, ZLDPosition> Z_ZLD_SAVE_DATA2_GT_IMP_POS_From_ZLDPosition
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_SAVE_DATA2.GT_IMP_POS, ZLDPosition>(
                    new Dictionary<string, string>()
                    , null
                    , Map_ZLDPosition_To_ZZLD_POS_2));
            }
        }

        //Z_ZLD_SAVE_TAGGLEICHE_MELDUNG.IS_TG_MELDUNG
        //Z_ZLD_SETNEW_DEBI_ERL.GT_KUNDEN

        static public ModelMapping<Z_ZLD_IMPORT_AH_WARENKORB.GT_BAK, NochNichtAbgesendeterVorgang> Z_ZLD_IMPORT_AH_WARENKORB_GT_BAK_From_NochNichtAbgesendeterVorgang
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_IMPORT_AH_WARENKORB.GT_BAK, NochNichtAbgesendeterVorgang>(
                    new Dictionary<string, string>()
                    , null
                    , (s, d) =>
                    {
                        d.ZULBELN = (String.IsNullOrEmpty(s.SapId) ? "" : s.SapId.PadLeft0(10));
                    }));
            }
        }

        static public ModelMapping<Z_ZLD_AH_2015_ETIKETT_DRU.IT_BELN, Kennzeichenetikett> Z_ZLD_AH_2015_ETIKETT_DRU_IT_BELN_From_Kennzeichenetikett
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_AH_2015_ETIKETT_DRU.IT_BELN, Kennzeichenetikett>(
                    new Dictionary<string, string>()
                    , null
                    , (s, d) =>
                    {
                        d.ZULBELN = (String.IsNullOrEmpty(s.SapId) ? "" : s.SapId.PadLeft0(10));
                    }));
            }
        }

        #endregion
    }
}
