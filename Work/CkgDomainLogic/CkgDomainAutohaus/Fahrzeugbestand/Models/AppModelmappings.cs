// ReSharper disable InconsistentNaming
// ReSharper disable RedundantUsingDirective
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

                        // Fahrzeug Akte
                        d.FabrikName = s.ZZFABRIKNAME;
                        d.HandelsName = s.ZZHANDELSNAME;
                        d.HerstellerSchluessel = s.ZZHERSTELLER_SCH;
                        d.TypSchluessel = s.ZZTYP_SCHL;
                        d.VvsSchluessel = s.ZZVVS_SCHLUESSEL;
                        d.VvsPruefZiffer = s.ZZTYP_VVS_PRUEF;

                        // Fahrzeug Bestand
                        d.Halter = s.HALTER;
                        d.Kaeufer = s.KAEUFER;

                        d.BriefbestandsInfo = s.BRIEFBESTAND;
                        d.BriefLagerort = s.LGORT;
                        d.FahrzeugStandort = s.STANDORT;
                        d.ErstZulassungsgDatum = s.ERSTZULDAT;
                        d.ZulassungsgDatumAktuell = s.AKTZULDAT;
                        d.AbmeldeDatum = s.ABMDAT;
                        d.Kennzeichen = s.KENNZ;
                        d.Briefnummer = s.BRIEFNR;
                        d.CocVorhanden = (s.COCVORHANDEN.NotNullOrEmpty().ToUpper() == "X");
                        d.Bemerkung = s.BEMERKUNG;
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

                        // Fahrzeug Akte
                        d.ZZFABRIKNAME = s.FabrikName;
                        d.ZZHANDELSNAME = s.HandelsName;
                        d.ZZTYP_SCHL = s.TypSchluessel;
                        d.ZZHERSTELLER_SCH = s.HerstellerSchluessel;
                        d.ZZVVS_SCHLUESSEL = s.VvsSchluessel;
                        d.ZZTYP_VVS_PRUEF = s.VvsPruefZiffer;

                        // Fahrzeug Bestand
                        d.KAEUFER = s.Kaeufer;
                        d.HALTER = s.Halter;

                        d.BRIEFBESTAND = s.BriefbestandsInfo;
                        d.LGORT = s.BriefLagerort;
                        d.STANDORT = s.FahrzeugStandort;
                        d.ERSTZULDAT = s.ErstZulassungsgDatum;
                        d.AKTZULDAT = s.ZulassungsgDatumAktuell;
                        d.ABMDAT = s.AbmeldeDatum;
                        d.KENNZ = s.Kennzeichen;
                        d.BRIEFNR = s.Briefnummer;
                        d.COCVORHANDEN = (s.CocVorhanden ? "X" : "");
                        d.BEMERKUNG = s.Bemerkung;
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
                        d.Email = s.EMAIL;
                        d.Telefon = s.TELEFON;
                        d.ReferenzNr = s.REFKUNNR;
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
                {"EMAIL", "Email"},
                {"Telefon", "Telefon"},
                {"REFKUNNR", "ReferenzNr"},
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
                            d.GEWERBE = (s.Gewerblich ? "X" : "");
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
                            d.Gewerblich = (s.GEWERBE.NotNullOrEmpty().ToUpper() == "X");
                        }));
            }
        }

        #endregion
    }
}