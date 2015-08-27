// ReSharper disable InconsistentNaming

using System;
using System.Collections.Generic;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using SapORM.Models;

namespace CkgDomainLogic.Autohaus.Models
{
    public partial class AppModelMappings
    {
        #region FromSap

        static public ModelMapping<Z_ZLD_AH_KUNDEN_ZUR_HIERARCHIE.GT_DEB, Kunde> Z_ZLD_AH_KUNDEN_ZUR_HIERARCHIE_GT_DEB_To_Kunde
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_AH_KUNDEN_ZUR_HIERARCHIE.GT_DEB, Kunde>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                        {
                            d.Adresse.HausNr = s.HOUSE_NUM1;
                            d.Adresse.Name1 = s.NAME1;
                            d.Adresse.Name2 = s.NAME2;
                            d.Adresse.Ort = s.CITY1;
                            d.Adresse.PLZ = s.POST_CODE1;
                            d.Adresse.Strasse = s.STREET;
                            d.Barkunde = s.BARKUNDE.XToBool();
                            d.CpdMitEinzugsermaechtigung = s.XCPDEIN.XToBool();
                            d.Cpdkunde = s.XCPDK.XToBool();
                            d.KundenNr = s.KUNNR;
                            d.OhneUmsatzsteuer = s.OHNEUST.XToBool();
                            d.Pauschalkunde = s.ZZPAUSCHAL.XToBool();
                            d.VkBur = s.VKBUR;
                            d.VkOrg = s.VKORG;
                        }));
            }
        }

        static public ModelMapping<Z_ZLD_AH_MATERIAL.GT_MAT, Material> Z_ZLD_AH_MATERIAL_GT_MAT_To_Material
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_AH_MATERIAL.GT_MAT, Material>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                        {
                            d.Belegtyp = s.BLTYP;
                            d.MaterialText = s.MAKTX;
                            d.MaterialNr = s.MATNR;
                            d.IstAbmeldung = s.ABMELDUNG.XToBool();
                            d.IstVersand = s.VERSAND.XToBool();
                            d.Auf48hVersandPruefen = s.Z48H_VERSAND.XToBool();
                            d.ZulassungAmFolgetagNichtMoeglich = s.NO_NEXT_DAY.XToBool();
                        }));
            }
        }

        static public ModelMapping<Z_ZLD_AH_ZULST_BY_PLZ.T_ZULST, Zulassungsstelle> Z_ZLD_AH_ZULST_BY_PLZ_T_ZULST_To_Zulassungsstelle
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_AH_ZULST_BY_PLZ.T_ZULST, Zulassungsstelle>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                        {
                            d.KbaNr = s.KBANR;
                            d.Zulassungskreis = s.ZKFZKZ;
                            d.ZulassungsKennzeichen = s.KREISKZ;
                        }));
            }
        }

        static public ModelMapping<Z_DPM_READ_LV_001.GT_OUT_DL, Zusatzdienstleistung> Z_DPM_READ_LV_001_GT_OUT_DL_To_Zusatzdienstleistung
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_LV_001.GT_OUT_DL, Zusatzdienstleistung>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                        {
                            d.ID = s.ASNUM;
                            d.MaterialNr = s.EAN11;
                            d.Name = s.ASKTX;
                            d.IstGewaehlt = s.VW_AG.IsNotNullOrEmpty();
                            d.Menge = "1";
                        }));
            }
        }

        static public ModelMapping<Z_ZLD_AH_IMPORT_ERFASSUNG1.GT_FILENAME, PdfFormular> Z_ZLD_AH_IMPORT_ERFASSUNG1_GT_FILENAME_To_PdfFormular
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_AH_IMPORT_ERFASSUNG1.GT_FILENAME, PdfFormular>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.Belegnummer = s.ZULBELN;
                        d.Typ = s.FORMART;
                        d.Label = s.NAME;
                        d.DateiPfad = s.FILENAME;
                    }));
            }
        }

        static public ModelMapping<Z_ZLD_AH_ZULLISTE.GT_OUT, ZulassungsReportModel> Z_ZLD_AH_ZULLISTE_GT_OUT_To_ZulassungsReportModel
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_AH_ZULLISTE.GT_OUT, ZulassungsReportModel>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.KundenNr = s.KUNNR;
                        d.Kennzeichen = s.ZZKENN;
                        d.ZulassungDatum = s.ZZZLDAT;
                        d.BelegNummer = s.ZULBELN;
                        d.PositionsNummer = s.ZULPOSNR;
                        d.ErfassungsDatum = s.VE_ERDAT;
                        d.ErfassungsUser = s.VE_ERNAM;
                        d.ZulassungsKreis = s.KREISKZ;
                        d.MaterialNr = s.MATNR;
                        d.KundenReferenz = s.KUNDEN_REF;
                        d.EvbNummmer = s.ZZEVB;
                        d.VertriebsBelegnummer = s.VBELN;
                        d.VkUser = s.VK_KUERZEL;
                        d.KundenNotiz = s.KUNDEN_REF;
                        d.MaterialKurztext = s.MAKTX;
                        d.FeinstaubAmt = s.FEINSTAUBAMT;
                        d.DokumentName = s.AH_DOKNAME.NotNullOrEmpty().Trim(' ');
                        d.Referenz1 = s.ZZREFNR1;
                        d.Referenz1 = s.ZZREFNR2;
                        d.Referenz1 = s.ZZREFNR3;
                        d.Referenz1 = s.ZZREFNR4;

                        SetStatus(s, d);

                        var resWunsch = s.RESWUNSCH.NotNullOrEmpty().ToUpper();
                        d.KennzeichenMerkmal = (resWunsch == "R" ? Localize.Reserved : (resWunsch == "W" ? Localize.PersonalisedNumberPlate : ""));
                    }));
            }
        }

        static public ModelMapping<Z_ZLD_AH_AUSGABE_ZULFORMS.GT_FILENAME, PdfFormular> Z_ZLD_AH_AUSGABE_ZULFORMS_GT_FILENAME_To_PdfFormular
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_AH_AUSGABE_ZULFORMS.GT_FILENAME, PdfFormular>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.Belegnummer = s.ZULBELN;
                        d.Typ = s.FORMART;
                        d.Label = s.NAME;
                        d.DateiPfad = s.FILENAME;
                    }));
            }
        }

        static public ModelMapping<Z_ZLD_EXPORT_ZULSTEL.GT_EX_ZULSTELL, Zulassungskreis> Z_ZLD_EXPORT_ZULSTEL_GT_EX_ZULSTELL_To_Zulassungskreis
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_EXPORT_ZULSTEL.GT_EX_ZULSTELL, Zulassungskreis>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.KreisKz = s.KREISKZ;
                        d.KreisBez = s.KREISBEZ;
                    }));
            }
        }

        static void SetStatus(Z_ZLD_AH_ZULLISTE.GT_OUT s, ZulassungsReportModel d)
        {
            d.StatusAsText = d.Status = s.BEB_STATUS.NotNullOrEmpty().ToUpper();

            switch (d.Status)
            {
                case "4":
                    d.Status = "DIS";
                    break;
                case "5":
                    d.Status = "GO";
                    break;
                case "7":
                    d.Status = "AR";
                    break;
            }

            switch (d.Status)
            {
                case "A":
                    d.StatusAsText = Localize.Adopted;
                    break;
                case "AR":
                    d.StatusAsText = Localize.Invoiced;
                    break;
                case "D":
                    d.StatusAsText = Localize.Accomplished;
                    break;
                case "DIS":
                    d.StatusAsText = Localize.Planned;
                    break;
                case "O":
                    d.StatusAsText = Localize.Open;
                    break;
                case "F":
                    d.StatusAsText = Localize.Failed;
                    break;
                case "L":
                    d.StatusAsText = Localize.Deleted;
                    break;
                case "GO":
                    d.StatusAsText = Localize.InWork;
                    break;
            }
        }

        static public ModelMapping<Z_ZLD_EXPORT_INFOPOOL.GT_EX_ZUSTLIEF, Adresse> Z_ZLD_EXPORT_INFOPOOL_GT_EX_ZUSTLIEF_To_Adresse
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_EXPORT_INFOPOOL.GT_EX_ZUSTLIEF, Adresse>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.KundenNr = s.LIFNR;
                        d.Name1 = s.NAME1;
                        d.Name2 = s.NAME2;
                        d.Strasse = s.STREET;
                        d.HausNr = s.HOUSE_NUM1;
                        d.PLZ = s.POST_CODE1;
                        d.Ort = s.CITY1;
                    }));
            }
        }

        #endregion


        #region ToSap

        static public ModelMapping<Z_ZLD_AH_IMPORT_ERFASSUNG1.GT_BAK_IN, Vorgang> Z_ZLD_AH_IMPORT_ERFASSUNG1_GT_BAK_IN_From_Vorgang
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_AH_IMPORT_ERFASSUNG1.GT_BAK_IN, Vorgang>(
                    new Dictionary<string, string>()
                    , null
                    , (s, d) =>
                        {
                            d.ZULBELN = s.BelegNr;
                            d.VKORG = s.VkOrg;
                            d.VKBUR = s.VkBur;
                            d.VE_ERNAM = s.Vorerfasser;
                            d.BEB_STATUS = s.VorgangsStatus;

                            // Rechnungsdaten
                            d.KUNNR = s.Rechnungsdaten.KundenNr;

                            // Bank-/Adressdaten
                            d.EINZ_JN = s.BankAdressdaten.Bankdaten.Einzugsermaechtigung.BoolToX();
                            d.RECH_JN = s.BankAdressdaten.Bankdaten.Rechnung.BoolToX();
                            d.BARZ_JN = s.BankAdressdaten.Bankdaten.Bar.BoolToX();
                            d.KOINH = s.BankAdressdaten.Bankdaten.Kontoinhaber;
                            d.IBAN = s.BankAdressdaten.Bankdaten.Iban;
                            d.SWIFT = s.BankAdressdaten.Bankdaten.Swift;
                            d.BANKN = s.BankAdressdaten.Bankdaten.KontoNr;
                            d.BANKL = s.BankAdressdaten.Bankdaten.Bankleitzahl;
                            d.EBPP_ACCNAME = s.BankAdressdaten.Bankdaten.Geldinstitut;

                            // Fahrzeug
                            d.ZZREFNR5 = s.Fahrzeugdaten.AuftragsNr;
                            d.ZZREFNR2 = s.Fahrzeugdaten.FahrgestellNr;
                            d.BRIEFNR = s.Fahrzeugdaten.Zb2Nr;
                            d.FAHRZ_ART = s.Fahrzeugdaten.FahrzeugartId;
                            d.VK_KUERZEL = s.Fahrzeugdaten.VerkaeuferKuerzel;
                            d.ZZREFNR3 = s.Fahrzeugdaten.Kostenstelle;
                            d.ZZREFNR4 = s.Fahrzeugdaten.BestellNr;

                            // Halter
                            d.ZZREFNR1 = s.HalterName.NotNullOrEmpty().ToUpper();

                            // Zulassung
                            d.ZZZLDAT = (s.Zulassungsdaten.ModusAbmeldung ? s.Zulassungsdaten.Abmeldedatum : s.Zulassungsdaten.Zulassungsdatum);
                            d.STILL_DAT = null;

                            d.BLTYP = s.Zulassungsdaten.Belegtyp;
                            d.KREISKZ = (s.Zulassungsdaten.ModusAbmeldung ? null : s.Zulassungsdaten.Zulassungskreis);
                            d.KREISBEZ = (s.Zulassungsdaten.ModusAbmeldung ? null : s.Zulassungsdaten.ZulassungskreisBezeichnung);
                            d.ZZEVB = s.Zulassungsdaten.EvbNr;

                            d.VH_KENNZ_RES = s.Zulassungsdaten.VorhandenesKennzeichenReservieren.BoolToX();
                            d.RESERVKENN_JN = s.Zulassungsdaten.KennzeichenReserviert.BoolToX();
                            d.WUNSCHKENN_JN = s.Zulassungsdaten.WunschkennzeichenVorhanden.BoolToX();
                            d.RESERVKENN = s.Zulassungsdaten.ReservierungsNr;

                            Func<string, string> formatKennzeichen = (kennzeichen => kennzeichen.NotNullOr(Zulassungsdaten.ZulassungsKennzeichenLinkeSeite(kennzeichen)));

                            d.ZZKENN = formatKennzeichen(s.Zulassungsdaten.Kennzeichen);
                            d.WU_KENNZ2 = formatKennzeichen(s.Zulassungsdaten.Wunschkennzeichen2);
                            d.WU_KENNZ3 = formatKennzeichen(s.Zulassungsdaten.Wunschkennzeichen3);

                            // Versandzulassung
                            d.ZL_LIFNR = (s.Zulassungsdaten.ModusVersandzulassung ? s.VersandAdresse.Adresse.KundenNr : "");

                            // Optionen/Dienstleistungen
                            d.EINKENN_JN = s.OptionenDienstleistungen.NurEinKennzeichen.BoolToX();
                            d.KENNZFORM = s.OptionenDienstleistungen.KennzeichenGroesseText;
                            d.SAISON_KNZ = s.OptionenDienstleistungen.Saisonkennzeichen.BoolToX();
                            d.SAISON_BEG = s.OptionenDienstleistungen.SaisonBeginn;
                            d.SAISON_END = s.OptionenDienstleistungen.SaisonEnde;
                            d.BEMERKUNG = s.OptionenDienstleistungen.Bemerkung;
                            d.KENNZ_VH = s.OptionenDienstleistungen.KennzeichenVorhanden.BoolToX();
                            d.HALTE_DAUER = s.OptionenDienstleistungen.HaltedauerBis;
                            d.ALT_KENNZ = s.OptionenDienstleistungen.AltesKennzeichen; 
                        }));
            }
        }

        static public ModelMapping<Z_ZLD_AH_IMPORT_ERFASSUNG1.GT_ADRS_IN, Adressdaten> Z_ZLD_AH_IMPORT_ERFASSUNG1_GT_ADRS_IN_From_Adressdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_AH_IMPORT_ERFASSUNG1.GT_ADRS_IN, Adressdaten>(
                    new Dictionary<string, string>()
                    , null
                    , (s, d) =>
                        {
                            d.KUNNR = s.Adresse.KundenNr;
                            d.PARVW = s.Partnerrolle;
                            d.ZULBELN = s.BelegNr;
                            d.NAME1 = s.Adresse.Name1;
                            d.NAME2 = s.Adresse.Name2;
                            d.STREET = s.Adresse.Strasse;
                            d.PLZ = s.Adresse.PLZ;
                            d.CITY1 = s.Adresse.Ort;
                            d.BEMERKUNG = s.Bemerkung;
                        }));
            }
        }

        static public ModelMapping<Z_ZLD_AH_IMPORT_ERFASSUNG1.GT_POS_IN, Zusatzdienstleistung> Z_ZLD_AH_IMPORT_ERFASSUNG1_GT_POS_IN_From_Zusatzdienstleistung
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_AH_IMPORT_ERFASSUNG1.GT_POS_IN, Zusatzdienstleistung>(
                    new Dictionary<string, string>()
                    , null
                    , (s, d) =>
                    {
                        d.ZULBELN = s.BelegNr;
                        d.LFDNR = s.PositionsNr;
                        d.MATNR = s.MaterialNr;
                        d.MENGE = s.Menge;
                    }));
            }
        }

        static public ModelMapping<Z_ZLD_AH_IMPORT_ERFASSUNG1.GT_BANK_IN, Bankdaten> Z_ZLD_AH_IMPORT_ERFASSUNG1_GT_BANK_IN_From_Bankdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_AH_IMPORT_ERFASSUNG1.GT_BANK_IN, Bankdaten>(
                    new Dictionary<string, string>()
                    , null
                    , (s, d) =>
                    {
                        d.PARVW = s.Partnerrolle;
                        d.ZULBELN = s.BelegNr;
                        d.EINZ_JN = s.Einzugsermaechtigung.BoolToX();
                        d.RECH_JN = s.Rechnung.BoolToX();
                        d.KOINH = s.Kontoinhaber;
                        d.IBAN = s.Iban;
                        d.SWIFT = s.Swift;
                        d.BANKN = s.KontoNr;
                        d.BANKL = s.Bankleitzahl;
                        d.EBPP_ACCNAME = s.Geldinstitut;
                    }));
            }
        }

        #endregion
    }
}