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

        #endregion


        #region ToSap

        #endregion
    }
}