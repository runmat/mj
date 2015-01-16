// ReSharper disable InconsistentNaming
using System.Collections.Generic;
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
                            d.Barkunde = s.BARKUNDE.XToBool();
                            d.CpdMitEinzugsermaechtigung = s.XCPDEIN.XToBool();
                            d.Cpdkunde = s.XCPDK.XToBool();
                            d.HausNr = s.HOUSE_NUM1;
                            d.KundenNr = s.KUNNR;
                            d.Name1 = s.NAME1;
                            d.Name2 = s.NAME2;
                            d.OhneUmsatzsteuer = s.OHNEUST.XToBool();
                            d.Ort = s.CITY1;
                            d.Pauschalkunde = s.ZZPAUSCHAL.XToBool();
                            d.Plz = s.POST_CODE1;
                            d.Strasse = s.STREET;
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
                        d.Kennzeichen = s.ZZKENN;
                        d.ZulassungDatum = s.ZZZLDAT;
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
                            var defaultKennzeichenLinkeSeite = Zulassungsdaten.ZulassungskreisToKennzeichenLinkeSeite(s.Zulassungsdaten.Zulassungskreis);

                            d.ZULBELN = s.BelegNr;
                            d.VKORG = s.VkOrg;
                            d.VKBUR = s.VkBur;
                            d.VE_ERNAM = s.Vorerfasser;
                            d.BEB_STATUS = s.VorgangsStatus;

                            // Rechnungsdaten
                            d.KUNNR = s.Rechnungsdaten.KundenNr;

                            // Bank-/Adressdaten
                            d.EINZ_JN = s.BankAdressdaten.Einzugsermaechtigung.BoolToX();
                            d.RECH_JN = s.BankAdressdaten.Rechnung.BoolToX();
                            d.BARZ_JN = s.BankAdressdaten.Bar.BoolToX();
                            d.KOINH = s.BankAdressdaten.Kontoinhaber;
                            d.IBAN = s.BankAdressdaten.Iban;
                            d.SWIFT = s.BankAdressdaten.Swift;
                            d.BANKN = s.BankAdressdaten.KontoNr;
                            d.BANKL = s.BankAdressdaten.Bankleitzahl;

                            // Fahrzeug
                            d.ZZREFNR5 = s.Fahrzeugdaten.AuftragsNr;
                            d.ZZREFNR2 = s.Fahrzeugdaten.FahrgestellNr;
                            d.BRIEFNR = s.Fahrzeugdaten.Zb2Nr;
                            d.FAHRZ_ART = s.Fahrzeugdaten.FahrzeugartId;
                            d.VK_KUERZEL = s.Fahrzeugdaten.VerkaeuferKuerzel;
                            d.ZZREFNR3 = s.Fahrzeugdaten.Kostenstelle;
                            d.ZZREFNR4 = s.Fahrzeugdaten.BestellNr;

                            // Halter
                            d.ZZREFNR1 = s.Halter.NotNullOrEmpty().ToUpper();

                            // Zulassung
                            
                            d.ZZZLDAT = s.Zulassungsdaten.Zulassungsdatum;
                            d.STILL_DAT = s.Zulassungsdaten.Abmeldedatum;

                            d.BLTYP = s.Zulassungsdaten.Belegtyp;
                            d.KREISKZ = s.Zulassungsdaten.Zulassungskreis;
                            d.KREISBEZ = s.Zulassungsdaten.ZulassungskreisBezeichnung;
                            d.ZZEVB = s.Zulassungsdaten.EvbNr;
                            d.RESERVKENN_JN = s.Zulassungsdaten.KennzeichenReserviert.BoolToX();
                            d.WUNSCHKENN_JN = s.Zulassungsdaten.WunschkennzeichenVorhanden.BoolToX();
                            d.RESERVKENN = s.Zulassungsdaten.ReservierungsNr;
                            d.ZZKENN = s.Zulassungsdaten.Kennzeichen.NotNullOr(defaultKennzeichenLinkeSeite);
                            d.WU_KENNZ2 = s.Zulassungsdaten.Wunschkennzeichen2.NotNullOr(defaultKennzeichenLinkeSeite);
                            d.WU_KENNZ3 = s.Zulassungsdaten.Wunschkennzeichen3.NotNullOr(defaultKennzeichenLinkeSeite);

                            // Optionen/Dienstleistungen
                            d.EINKENN_JN = s.OptionenDienstleistungen.NurEinKennzeichen.BoolToX();
                            d.KENNZFORM = s.OptionenDienstleistungen.KennzeichenGroesseText;
                            d.SAISON_KNZ = s.OptionenDienstleistungen.Saisonkennzeichen.BoolToX();
                            d.SAISON_BEG = s.OptionenDienstleistungen.SaisonBeginn;
                            d.SAISON_END = s.OptionenDienstleistungen.SaisonEnde;
                            d.BEMERKUNG = s.OptionenDienstleistungen.Bemerkung;
                            d.KENNZ_VH = s.OptionenDienstleistungen.KennzeichenVorhanden.BoolToX();
                            d.VH_KENNZ_RES = s.OptionenDienstleistungen.VorhandenesKennzeichenReservieren.BoolToX();
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
                            d.PARVW = s.Kennung;
                            d.ZULBELN = s.BelegNr;
                            d.NAME1 = s.Name1;
                            d.NAME2 = s.Name2;
                            d.STREET = s.Strasse;
                            d.PLZ = s.Plz;
                            d.CITY1 = s.Ort;
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

        #endregion
    }
}