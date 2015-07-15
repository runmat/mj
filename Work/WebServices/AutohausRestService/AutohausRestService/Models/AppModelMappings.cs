using System.Collections.Generic;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;

namespace AutohausRestService.Models
{
    public class AppModelMappings : ModelMappings
    {
        #region FromSap

        static public ModelMapping<Z_AHP_CRE_CHG_PARTNER_FZGDATEN.GT_WEB_PARTNER_OUT, Partner> Z_AHP_CRE_CHG_PARTNER_FZGDATEN_GT_WEB_PARTNER_OUT_To_Partner
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_AHP_CRE_CHG_PARTNER_FZGDATEN.GT_WEB_PARTNER_OUT, Partner>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                        {
                            d.Bemerkung = s.BEMERKUNG;
                            d.Email = s.EMAIL;
                            d.Fax = s.FAX;
                            d.Gewerblich = s.GEWERBE.XToBool();
                            d.HausNr = s.HAUSNR;
                            d.KundenNr = s.KUNNR;
                            d.KundendatenSpeichern = s.SAVEKDDATEN.XToBool();
                            d.Land = s.LAND;
                            d.Name1 = s.NAME1;
                            d.Name2 = s.NAME2;
                            d.Ort = s.ORT;
                            d.Partnerrolle = s.PARTART;
                            d.Plz = s.PLZNR;
                            d.Referenz1 = s.REFKUNNR;
                            d.Referenz2 = s.REFKUNNR2;
                            d.Strasse = s.STRASSE;
                            d.Telefon = s.TELEFON;
                        }));
            }
        }

        static public ModelMapping<Z_AHP_CRE_CHG_PARTNER_FZGDATEN.GT_WEB_FZG_OUT, Fahrzeug> Z_AHP_CRE_CHG_PARTNER_FZGDATEN_GT_WEB_FZG_OUT_To_Fahrzeug
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_AHP_CRE_CHG_PARTNER_FZGDATEN.GT_WEB_FZG_OUT, Fahrzeug>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                        {
                            d.Abmeldedatum = s.ABMDAT;
                            d.AktZulassung = s.AKTZULDAT;
                            d.AuftragsNr = s.AUFNR;
                            d.BriefNr = s.BRIEFNR;
                            d.Briefbestand = s.BRIEFBESTAND;
                            d.CocVorhanden = s.COCVORHANDEN.XToBool();
                            d.Erstzulassung = s.ERSTZULDAT;
                            d.FahrgestellNr = s.FIN;
                            d.FahrzeugID = s.FIN_ID;
                            d.FahrzeugNr = s.FZGNR;
                            d.Fahrzeugart = s.FZGART;
                            d.Firmenreferenz1 = s.FAREF1;
                            d.Firmenreferenz2 = s.FAREF2;
                            d.HerstellerSchluessel = s.ZZHERSTELLER_SCH;
                            d.Kennzeichen = s.KENNZ;
                            d.Kostenstelle = s.KOSTL;
                            d.Lagerort = s.LGORT;
                            d.Standort = s.STANDORT;
                            d.TypSchluessel = s.ZZTYP_SCHL;
                            d.Verkaufssparte = s.VKSPARTE;
                            d.VvsPruefziffer = s.ZZTYP_VVS_PRUEF;
                            d.VvsSchluessel = s.ZZVVS_SCHLUESSEL;
                        }));
            }
        }

        #endregion

        #region ToSap

        static public ModelMapping<Z_AHP_CRE_CHG_PARTNER_FZGDATEN.GT_WEB_PARTNER_IMP, Partner> Z_AHP_CRE_CHG_PARTNER_FZGDATEN_GT_WEB_PARTNER_IMP_From_Partner
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_AHP_CRE_CHG_PARTNER_FZGDATEN.GT_WEB_PARTNER_IMP, Partner>(
                    new Dictionary<string, string>()
                    , null
                    , (s, d) =>
                        {
                            d.BEMERKUNG = s.Bemerkung;
                            d.EMAIL = s.Email;
                            d.FAX = s.Fax;
                            d.GEWERBE = s.Gewerblich.BoolToX();
                            d.HAUSNR = s.HausNr;
                            d.KUNNR = s.KundenNr.ToSapKunnr();
                            d.LAND = s.Land;
                            d.NAME1 = s.Name1;
                            d.NAME2 = s.Name2;
                            d.ORT = s.Ort;
                            d.PARTART = s.Partnerrolle;
                            d.PLZNR = s.Plz;
                            d.REFKUNNR = s.Referenz1;
                            d.REFKUNNR2 = s.Referenz2;
                            d.SAVEKDDATEN = s.KundendatenSpeichern.BoolToX();
                            d.STRASSE = s.Strasse;
                            d.TELEFON = s.Telefon;
                        }));
            }
        }

        static public ModelMapping<Z_AHP_CRE_CHG_PARTNER_FZGDATEN.GT_WEB_FZG_IMP, Fahrzeug> Z_AHP_CRE_CHG_PARTNER_FZGDATEN_GT_WEB_FZG_IMP_From_Fahrzeug
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_AHP_CRE_CHG_PARTNER_FZGDATEN.GT_WEB_FZG_IMP, Fahrzeug>(
                    new Dictionary<string, string>()
                    , null
                    , (s, d) =>
                        {
                            d.ABMDAT = s.Abmeldedatum;
                            d.AKTZULDAT = s.AktZulassung;
                            d.AUFNR = s.AuftragsNr;
                            d.BRIEFBESTAND = s.Briefbestand;
                            d.BRIEFNR = s.BriefNr;
                            d.COCVORHANDEN = s.CocVorhanden.BoolToX();
                            d.ERSTZULDAT = s.Erstzulassung;
                            d.FAREF1 = s.Firmenreferenz1;
                            d.FAREF2 = s.Firmenreferenz2;
                            d.FIN = s.FahrgestellNr;
                            d.FIN_ID = s.FahrzeugID;
                            d.FZGART = s.Fahrzeugart;
                            d.FZGNR = s.FahrzeugNr;
                            d.KENNZ = s.Kennzeichen;
                            d.KOSTL = s.Kostenstelle;
                            d.LGORT = s.Lagerort;
                            d.STANDORT = s.Standort;
                            d.VKSPARTE = s.Verkaufssparte;
                            d.ZZHERSTELLER_SCH = s.HerstellerSchluessel;
                            d.ZZTYP_SCHL = s.TypSchluessel;
                            d.ZZTYP_VVS_PRUEF = s.VvsPruefziffer;
                            d.ZZVVS_SCHLUESSEL = s.VvsSchluessel;
                        }));
            }
        }

        #endregion
    }
}
