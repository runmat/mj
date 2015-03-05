using System.Collections.Generic;
using GeneralTools.Models;
using SapORM.Models;

namespace CkgDomainLogic.Zanf.Models
{
    public class AppModelMappings : ModelMappings
    {
        #region FromSap

        static public ModelMapping<Z_ZANF_READ_KLAERF_01.GT_DATEN, ZulassungsAnforderung> Z_ZANF_READ_KLAERF_01_GT_DATEN_To_ZulassungsAnforderung
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZANF_READ_KLAERF_01.GT_DATEN, ZulassungsAnforderung>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                        {
                            d.AnforderungsNr = s.ORDERID;
                            d.Anlagedatum = s.ERDAT;
                            d.AuftragsNr = s.VBELN;
                            d.Ausfuehrungsdatum = s.ADATUM;
                            d.FahrgestellNr = s.ZZFAHRG;
                            d.HauptpositionsNr = s.HPPOS;
                            d.Klaerfall = s.KLAERF.XToBool();
                            d.KundenreferenzNr = s.ZZREFNR;
                        }));
            }
        }

        static public ModelMapping<Z_DPM_READ_ZULDOK_01.GT_DATEN, ZulassungsUnterlagen> Z_DPM_READ_ZULDOK_01_GT_DATEN_To_ZulassungsUnterlagen
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_ZULDOK_01.GT_DATEN, ZulassungsUnterlagen>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                        {
                            d.Aenderungsdatum = s.AENDAT;
                            d.Barcode = s.BARCODE;
                            d.Bemerkung = s.BEMERKUNG;
                            d.BeschaffungsdatumHandelsregisterGewerbeanmeldung = s.DAT_GW_ANMELD;
                            d.BestandOriginalVollmacht = s.BEST_ORIG_VM;
                            d.Bevollmaechtigter1 = s.BEVOLLM1;
                            d.Bevollmaechtigter2 = s.BEVOLLM2;
                            d.Bevollmaechtigter3 = s.BEVOLLM3;
                            d.DokumentId = s.OBJECT_ID;
                            d.EinzugsermaechtigungFahrzeugbezogen = s.EZERM_KFZBEZOGEN.XToBool();
                            d.EinzugsermaechtigungFehlt = s.KZ_EZERM.XToBool();
                            d.EinzugsermaechtigungVorhanden = s.EINZUG.XToBool();
                            d.ErfasstVon = s.ERNAM;
                            d.Erfassungsdatum = s.ERDAT;
                            d.EvbFehlt = s.KZ_EVBNR.XToBool();
                            d.EvbGueltigBis = s.EVB_BIS;
                            d.EvbGueltigVon = s.EVB_VON;
                            d.EvbNr = s.EVB_NUM;
                            d.ExterneKundenNr = s.ZKUNNR_EXT;
                            d.FeinstaubplaketteImmerBeauftragen = s.KZ_FSPLAK.XToBool();
                            d.GeaendertVon = s.AENAM;
                            d.GewerbeanmeldungFehlt = s.KZ_GEWERBE.XToBool();
                            d.GewerbeanmeldungGueltigBis = s.GA_BIS;
                            d.GewerbeanmeldungGueltigVon = s.GA_VON;
                            d.GewerbeanmeldungVorhanden = s.GEWERBE.XToBool();
                            d.GueltigBis1 = s.GUELTIG_BIS1;
                            d.GueltigBis2 = s.GUELTIG_BIS2;
                            d.GueltigBis3 = s.GUELTIG_BIS3;
                            d.HalterId = s.HALTER;
                            d.HalterName = s.NAME1_HALTER;
                            d.HalterOrt = s.ORT01_HALTER;
                            d.HandelsregisterFehlt = s.KZ_HANDREG.XToBool();
                            d.HandelsregisterGueltigBis = s.HREGDAT_BIS;
                            d.HandelsregisterGueltigVon = s.HREGDAT_VON;
                            d.HandelsregisterVorhanden = s.REGISTER.XToBool();
                            d.KennzeichenverstaerkerVersenden = s.KZ_KZVERST.XToBool();
                            d.PersonalausweisFehlt = s.PERSO_FEHLT.XToBool();
                            d.PersonalausweisVorhanden = s.PERSO.XToBool();
                            d.SchluesselNrZls = s.SCHLNR_ZLS;
                            d.Sonstiges = s.SONST_SH;
                            d.Standort = s.STANDORT;
                            d.VollmachtFehlt = s.VOLLMACHT_FEHLT.XToBool();
                            d.VollmachtGueltigBis = s.VOLLMACHT_BIS;
                            d.VollmachtGueltigVon = s.VOLLMACHT_VON;
                            d.VollmachtVorhanden = s.VOLLM.XToBool();
                            d.Vollstaendig = s.VOLLST.XToBool();
                            d.Webuser = s.WEB_USER;
                            d.Wunschkennzeichen = s.WKENNZ;
                            d.Zulassungskreis = s.ZULKREIS;
                            d.ZulassungsunterlagenBeiZls = s.KZ_ZLS.XToBool();
                        }));
            }
        }

        #endregion


        #region ToSap

        static public ModelMapping<Z_DPM_SAVE_ZULDOK_01.GT_DATEN, ZulassungsUnterlagen> Z_DPM_SAVE_ZULDOK_01_GT_DATEN_From_ZulassungsUnterlagen
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_SAVE_ZULDOK_01.GT_DATEN, ZulassungsUnterlagen>(
                    new Dictionary<string, string>()
                    , null
                    , (s, d) =>
                        {
                            d.DAT_LOE = s.Loeschdatum;
                            d.EVB_BIS = s.EvbGueltigBis;
                            d.EVB_NUM = s.EvbNr;
                            d.EVB_VON = s.EvbGueltigVon;
                            d.HALTER = s.HalterId;
                            d.STANDORT = s.Standort;
                        }));
            }
        }

        #endregion
    }
}