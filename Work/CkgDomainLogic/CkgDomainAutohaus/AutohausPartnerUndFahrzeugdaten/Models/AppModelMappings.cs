using System.Collections.Generic;
using GeneralTools.Models;
using SapORM.Models;
using SapORM.Contracts;

namespace CkgDomainLogic.AutohausPartnerUndFahrzeugdaten.Models
{
    public class AppModelMappings : ModelMappings
    {
        #region FromSap

        #endregion


        #region ToSap

        static public ModelMapping<Z_AHP_CRE_CHG_PARTNER.GT_WEB_IMP, Partnerdaten> Z_AHP_CRE_CHG_PARTNER_GT_WEB_IMP_From_Partnerdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_AHP_CRE_CHG_PARTNER.GT_WEB_IMP, Partnerdaten>(
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

        static public ModelMapping<Z_AHP_CRE_CHG_FZG_AKT_BEST.GT_WEB_IMP, Fahrzeugdaten> Z_AHP_CRE_CHG_FZG_AKT_BEST_GT_WEB_IMP_From_Fahrzeugdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_AHP_CRE_CHG_FZG_AKT_BEST.GT_WEB_IMP, Fahrzeugdaten>(
                    new Dictionary<string, string>()
                    , null
                    , (s, d) =>
                    {
                        if (s.Abmeldedatum.ToNullableDateTime() != null)
                            d.ABMDAT = s.Abmeldedatum.ToNullableDateTime();

                        if (s.AktZulassung.ToNullableDateTime() != null)
                            d.AKTZULDAT = s.AktZulassung.ToNullableDateTime();

                        d.AUFNR = s.AuftragsNr;
                        d.BRIEFBESTAND = s.Briefbestand;
                        d.BRIEFNR = s.BriefNr;
                        d.COCVORHANDEN = s.CocVorhanden.BoolToX();

                        if (s.Erstzulassung.ToNullableDateTime() != null)
                            d.ERSTZULDAT = s.Erstzulassung.ToNullableDateTime();

                        d.FAREF1 = s.Firmenreferenz1;
                        d.FAREF2 = s.Firmenreferenz2;
                        d.FIN = s.FahrgestellNr;
                        d.FIN_ID = s.FahrzeugID;
                        d.FZGART = s.Fahrzeugart;
                        d.FZGNR = s.FahrzeugNr;
                        d.HALTER = s.Halter;
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