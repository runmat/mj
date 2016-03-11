// ReSharper disable InconsistentNaming
using System.Collections.Generic;
using CkgDomainLogic.DomainCommon.Models;
using GeneralTools.Models;
using SapORM.Models;

namespace CkgDomainLogic.Fahrzeugbestand.Models
{
    public class AppModelMappings : ModelMappings
    {
        #region Load from Repository

        static public ModelMapping<Z_AHP_READ_FZGBESTAND.GT_WEBOUT, FahrzeugAkteBestand> Z_AHP_READ_FZGBESTAND_GT_WEBOUT_To_FahrzeugAkteBestand
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_AHP_READ_FZGBESTAND.GT_WEBOUT, FahrzeugAkteBestand>(
                    new Dictionary<string, string>(),
                    (s, d) =>
                    {
                        d.FIN = s.FIN;
                        d.FinID = s.FIN_ID;

                        d.FabrikName = s.ZZFABRIKNAME;
                        d.HandelsName = s.ZZHANDELSNAME;
                        d.HerstellerSchluessel = s.ZZHERSTELLER_SCH;
                        d.TypSchluessel = s.ZZTYP_SCHL;
                        d.VvsSchluessel = s.ZZVVS_SCHLUESSEL;
                        d.VvsPruefZiffer = s.ZZTYP_VVS_PRUEF;

                        d.Halter = s.HALTER;
                        d.Kaeufer = s.KAEUFER;
                        d.ZahlerKfzSteuer = s.KONTOINHABER;

                        d.BriefbestandsInfo = s.BRIEFBESTAND;
                        d.BriefLagerort = s.LGORT;
                        d.FahrzeugStandort = s.STANDORT;
                        d.ErstZulassungsgDatum = s.ERSTZULDAT;
                        d.ZulassungsgDatumAktuell = s.AKTZULDAT;
                        d.AbmeldeDatum = s.ABMDAT;
                        d.Kennzeichen = s.KENNZ;
                        d.Briefnummer = s.BRIEFNR;
                        d.CocVorhanden = s.COCVORHANDEN.XToBool();
                        d.Bemerkung = s.BEMERKUNG;

                        d.FahrzeugArt = s.FZGART;
                        d.VerkaufsSparte = s.VKSPARTE;
                        d.FahrzeugNummer = s.FZGNR;
                        d.AuftragsNummer = s.AUFNR;
                        d.FirmenReferenz1 = s.FAREF1;
                        d.FirmenReferenz2 = s.FAREF2;
                        d.KundenReferenz = s.KUNDENREFERENZ;
                    }));
            }
        }

        static public ModelMapping<Z_AHP_READ_TYPDAT_BESTAND.GT_WEB_TYPDATEN, FahrzeugAkteBestand> Z_AHP_READ_TYPDAT_BESTAND_GT_TYPDATEN_To_FahrzeugAkteBestand
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_AHP_READ_TYPDAT_BESTAND.GT_WEB_TYPDATEN, FahrzeugAkteBestand>(
                    new Dictionary<string, string>(),
                    (s, d) =>
                    {
                        d.FIN = s.FIN;
                        d.FinID = s.FIN_ID;
                        d.HerstellerSchluessel = s.ZZHERSTELLER_SCH;
                        d.TypSchluessel = s.ZZTYP_SCHL;
                        d.VvsSchluessel = s.ZZVVS_SCHLUESSEL;
                        d.VvsPruefZiffer = s.ZZTYP_VVS_PRUEF;

                        d.FabrikName = s.ZZFABRIKNAME;
                        d.HandelsName = s.ZZHANDELSNAME;
                    }));
            }
        }

        #endregion

        #region Save to Repository

        static public ModelMapping<Z_AHP_CRE_CHG_FZG_AKT_BEST.GT_WEB_IMP, FahrzeugAkteBestand> Z_AHP_CRE_CHG_FZG_AKT_BEST_GT_WEB_IMP_To_FahrzeugAkteBestand
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_AHP_CRE_CHG_FZG_AKT_BEST.GT_WEB_IMP, FahrzeugAkteBestand>(
                    new Dictionary<string, string>(),
                    null,
                    (s, d) =>
                    {
                        d.FIN_ID = s.FinID;
                        d.FIN = s.FIN;

                        d.ZZFABRIKNAME = s.FabrikName;
                        d.ZZHANDELSNAME = s.HandelsName;
                        d.ZZTYP_SCHL = s.TypSchluessel;
                        d.ZZHERSTELLER_SCH = s.HerstellerSchluessel;
                        d.ZZVVS_SCHLUESSEL = s.VvsSchluessel;
                        d.ZZTYP_VVS_PRUEF = s.VvsPruefZiffer;

                        d.KAEUFER = s.Kaeufer;
                        d.HALTER = s.Halter;
                        d.KONTOINHABER = s.ZahlerKfzSteuer;

                        d.BRIEFBESTAND = s.BriefbestandsInfo;
                        d.LGORT = s.BriefLagerort;
                        d.STANDORT = s.FahrzeugStandort;
                        d.ERSTZULDAT = s.ErstZulassungsgDatum;
                        d.AKTZULDAT = s.ZulassungsgDatumAktuell;
                        d.ABMDAT = s.AbmeldeDatum;
                        d.KENNZ = s.Kennzeichen;
                        d.BRIEFNR = s.Briefnummer;
                        d.COCVORHANDEN = s.CocVorhanden.BoolToX();
                        d.BEMERKUNG = s.Bemerkung;

                        d.FZGART = s.FahrzeugArt;
                        d.VKSPARTE = s.VerkaufsSparte;
                        d.FZGNR = s.FahrzeugNummer;
                        d.AUFNR = s.AuftragsNummer;
                        d.FAREF1 = s.FirmenReferenz1;
                        d.FAREF2 = s.FirmenReferenz2;
                        d.KUNDENREFERENZ = s.KundenReferenz;
                        d.LOEVM = s.Loeschen.BoolToX();
                    }));
            }
        }

        #endregion


        #region Partner Adressen

        static public ModelMapping<Z_AHP_READ_PARTNER.GT_OUT, Adresse> Z_AHP_READ_PARTNER_GT_OUT_To_Adresse
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_AHP_READ_PARTNER.GT_OUT, Adresse>(
                    new Dictionary<string, string>(),
                    (s, d) =>
                    {
                        d.KundenNr = s.KUNNR;
                        d.Kennung = s.PARTART;
                        d.Name1 = s.NAME1;
                        d.Name2 = s.NAME2;
                        d.Strasse = s.STRASSE;
                        d.HausNr = s.HAUSNR;
                        d.PLZ = s.PLZNR;
                        d.Ort = s.ORT;
                        d.Land = s.LAND;
                        d.Telefon = s.TELEFON;
                        d.Fax = s.FAX;
                        d.Bemerkung = s.BEMERKUNG;
                        d.Email = s.EMAIL;
                        d.ReferenzNr = s.REFKUNNR;
                        d.Gewerblich = s.GEWERBE.XToBool();
                        d.EvbNr = s.EVBNR;                      // 20150612 MMA ITA 8127
                        d.Stichtagsabbuchung = s.SEPA_STICHTAG; // 20150612 MMA ITA 8127
                    }));
            }
        }

        private static readonly Dictionary<string, string> PartnerToAdresseDict = new Dictionary<string, string>
            {
                {"KUNNR", "KundenNr"},
                {"PARTART", "Kennung"},
                {"NAME1", "Name1"},
                {"NAME2", "Name2"},
                {"STRASSE", "Strasse"},
                {"HAUSNR", "HausNr"},
                {"PLZNR", "PLZ"},
                {"ORT", "Ort"},
                {"LAND", "Land"},
                {"Telefon", "Telefon"},
                {"FAX", "Fax"},
                {"EMAIL", "Email"},
                {"BEMERKUNG", "Bemerkung"},
                {"REFKUNNR", "ReferenzNr"},

                {"EVBNR", "EvbNr"},
                {"SEPA_STICHTAG", "Stichtagsabbuchung"},

            };

        static public ModelMapping<Z_AHP_CRE_CHG_PARTNER.GT_WEB_IMP, Adresse> Z_AHP_CRE_CHG_PARTNER_GT_WEB_IMP_To_Adresse
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_AHP_CRE_CHG_PARTNER.GT_WEB_IMP, Adresse>(
                    PartnerToAdresseDict,
                    null,
                    (s, d) =>
                        {
                            d.GEWERBE = s.Gewerblich.BoolToX();
                            d.SAVEKDDATEN = true.BoolToX();
                        }));
            }
        }

        static public ModelMapping<Z_AHP_CRE_CHG_PARTNER.GT_OUT, Adresse> Z_AHP_CRE_CHG_PARTNER_GT_OUT_To_Adresse
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_AHP_CRE_CHG_PARTNER.GT_OUT, Adresse>(
                    PartnerToAdresseDict,
                    (s, d) =>
                        {
                        }));
            }
        }

        #endregion
    }
}