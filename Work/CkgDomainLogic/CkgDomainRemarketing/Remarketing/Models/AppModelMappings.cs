using System.Collections.Generic;
using GeneralTools.Models;
using SapORM.Models;

namespace CkgDomainLogic.Remarketing.Models
{
    public class AppModelMappings : ModelMappings
    {
        #region FromSap

        static public ModelMapping<Z_DPM_READ_AUFTR6_001.GT_WEB, Vermieter> Z_DPM_READ_AUFTR6_001_GT_WEB_To_Vermieter
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_AUFTR6_001.GT_WEB, Vermieter>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.VermieterId = s.POS_KURZTEXT;
                        d.VermieterName = s.POS_TEXT;
                    }
                ));
            }
        }

        static public ModelMapping<Z_DPM_READ_REM_002.GT_WEB, FehlendeDaten> Z_DPM_READ_REM_002_GT_WEB_To_FehlendeDaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_REM_002.GT_WEB, FehlendeDaten>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.VermieterId = s.AVNR;
                        d.VermieterName = s.TEXT_AVNR;
                        d.FahrgestellNr = s.FAHRGNR;
                        d.Kennzeichen = s.KENNZ;
                        d.Zb2Eingang = s.EGZB2DAT;
                        d.CarportEingang = s.HCEINGDAT;
                        d.EquiNr = s.EQUNR;
                        d.Rechnungsuebermittlung = s.UEBERM_RE;
                        d.Eingangsdatum = s.INDATUM;
                        d.Stilllegungsdatum = s.STILLDAT;
                        d.DatumHcUebergabeTuevSued = s.DAT_UEB_HC_TUEVSUED;
                        d.DatumErstellungBelastungsanzeige = s.ERDAT_BELAS;
                    }
                ));
            }
        }

        static public ModelMapping<Z_DPM_READ_HC_NR_01.GT_WEB, Hereinnahmecenter> Z_DPM_READ_HC_NR_01_GT_WEB_To_Hereinnahmecenter
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_HC_NR_01.GT_WEB, Hereinnahmecenter>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.HereinnahmecenterId = s.POS_KURZTEXT;
                        d.HereinnahmecenterName = s.POS_TEXT;
                    }
                ));
            }
        }

        static public ModelMapping<Z_DPM_REM_BELASTUNGSANZEIGE_02.GT_OUT, Belastungsanzeige> Z_DPM_REM_BELASTUNGSANZEIGE_02_GT_OUT_To_Belastungsanzeige
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_REM_BELASTUNGSANZEIGE_02.GT_OUT, Belastungsanzeige>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.FahrgestellNr = s.FAHRGNR;
                        d.Kennzeichen = s.KENNZ;
                        d.InventarNr = s.INVENTAR;
                        d.AutoVermieter = s.AVNAME;
                        d.Hereinnahmecenter = s.HCNAME;
                        d.HcEingang = s.HCEINGDAT;
                        d.Beauftragungsdatum = s.GUTAUFTRAGDAT;
                        d.Summe = s.SUMME;
                        d.AnzahlGutachten = s.AZGUT;
                        d.Status = s.STATU;
                        d.StatusText = s.DDTEXT;
                        d.Gutachtendatum = s.GUTADAT;
                        d.MinderwertAv = s.MINWERT_AV;
                        d.AnzahlReparaturKalkulationen = s.REPKALK.NotNullOrEmpty().ToInt(0);
                        d.DatumReparaturKalkulation = s.REPKALKDAT;
                    }
                ));
            }
        }

        static public ModelMapping<Z_DPM_REM_BELASTUNGSA_IBUTT_02.GT_OUT, Gutachten> Z_DPM_REM_BELASTUNGSA_IBUTT_02_GT_OUT_To_Gutachten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_REM_BELASTUNGSA_IBUTT_02.GT_OUT, Gutachten>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.FahrgestellNr = s.FAHRGNR;
                        d.LfdNr = s.LFDNR;
                        d.Eingangsdatum = s.INDATUM;
                        d.Gutachter = s.GUTA;
                        d.GutachtenId = s.GUTAID;
                        d.Gutachtendatum = s.GUTADAT;
                        d.Schadenskennzeichen = s.SCHADKZ;
                        d.Schadensbetrag = s.SCHADBETR;
                        d.SchadensbetragAv = s.SCHADBETR_AV;
                        d.Reparaturkennzeichen = s.REPKZ;
                        d.Aufbereitungsbetrag = s.AUFBETR;
                        d.AufbereitungsbetragAv = s.AUFBETR_AV;
                        d.Wertminderungsbetrag = s.WRTMBETR;
                        d.WertminderungsbetragAv = s.WRTMBETR_AV;
                        d.Fehlteilbetrag = s.FEHLTBETR;
                        d.FehlteilbetragAv = s.FEHLTBETR_AV;
                        d.Restwert = s.RESTWERT;
                        d.MaengelwertAv = s.MAEWERT_AV;
                        d.OptWertAv = s.OPTWRTAV;
                        d.MinderwertAv = s.MINWERT_AV;
                        d.BeschaedigungenAv = s.BESCHAED_AV;
                        d.VorschaedenRepariertAv = s.VORSCHAED_REP_AV;
                        d.SchaedenUnrepariertAv = s.SCHAED_UNREP_AV;
                        d.VorschaedenWertminderung = s.VORSCHAED_WERTMIND;
                        d.SchaedenUnrepariertWertminderung = s.SCHAED_UNREP_WMIND;
                    }
                ));
            }
        }

        #endregion


        #region ToSap

        static public ModelMapping<Z_DPM_REM_UPD_STATNEU_BELA_01.GT_DAT, Belastungsanzeige> Z_DPM_REM_UPD_STATNEU_BELA_01_GT_DAT_From_Belastungsanzeige
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_REM_UPD_STATNEU_BELA_01.GT_DAT, Belastungsanzeige>(
                    new Dictionary<string, string>()
                    , null
                    , (s, d) =>
                    {
                        d.FAHRGNR = s.FahrgestellNr;
                    }
                ));
            }
        }

        static public ModelMapping<Z_DPM_REM_UPD_STAT_BELAST_02.GT_IN, Belastungsanzeige> Z_DPM_REM_UPD_STAT_BELAST_02_GT_IN_From_Belastungsanzeige
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_REM_UPD_STAT_BELAST_02.GT_IN, Belastungsanzeige>(
                    new Dictionary<string, string>()
                    , null
                    , (s, d) =>
                    {
                        d.FAHRGNR = s.FahrgestellNr;
                        d.KENNZ = s.Kennzeichen;
                        d.STATU = s.Status;
                        d.BLOCKTEXT = s.BlockadeText;
                    }
                ));
            }
        }

        #endregion
    }
}