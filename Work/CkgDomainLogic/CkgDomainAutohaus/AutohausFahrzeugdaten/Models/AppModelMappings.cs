using System.Collections.Generic;
using GeneralTools.Models;
using SapORM.Models;

namespace CkgDomainLogic.AutohausFahrzeugdaten.Models
{
    public class AppModelMappings : ModelMappings
    {
        #region FromSap

        #endregion


        #region ToSap

        static public ModelMapping<Z_AHP_CRE_CHG_FZG_AKT_BEST.GT_WEB_IMP, UploadFahrzeug> Z_AHP_CRE_CHG_FZG_AKT_BEST_GT_WEB_IMP_From_UploadFahrzeug
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_AHP_CRE_CHG_FZG_AKT_BEST.GT_WEB_IMP, UploadFahrzeug>(
                    new Dictionary<string, string>()
                    , null
                    , (s, d) =>
                    {
                        if (s.Abmeldedatum.ToNullableDateTime() != null)
                        {
                            d.ABMDAT = s.Abmeldedatum.ToNullableDateTime();
                        }
                        if (s.Zulassungsdatum.ToNullableDateTime() != null)
                        {
                            d.AKTZULDAT = s.Zulassungsdatum.ToNullableDateTime();
                        }
                        d.AUFNR = s.AuftragsNr;
                        if (s.Erstzulassung.ToNullableDateTime() != null)
                        {
                            d.ERSTZULDAT = s.Erstzulassung.ToNullableDateTime();
                        }
                        d.FAREF1 = s.Referenz1;
                        d.FAREF2 = s.Referenz2;
                        d.FIN = s.FahrgestellNr;
                        d.FZGART = s.Fahrzeugart;
                        d.FZGNR = s.FahrzeugNr;
                        d.KENNZ = s.Kennzeichen;
                        d.VKSPARTE = s.Verkaufssparte;
                        d.ZZHERSTELLER_SCH = s.HerstellerSchluessel;
                        d.ZZTYP_SCHL = s.TypSchluessel;
                        d.ZZTYP_VVS_PRUEF = s.VvsPruefziffer;
                        d.ZZVVS_SCHLUESSEL = s.VvsSchluessel;
                    }
                ));
            }
        }

        #endregion
    }
}