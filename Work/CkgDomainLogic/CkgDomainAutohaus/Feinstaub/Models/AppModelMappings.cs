using System.Collections.Generic;
using GeneralTools.Models;
using SapORM.Models;

namespace CkgDomainLogic.Feinstaub.Models
{
    public class AppModelMappings : ModelMappings
    {
        #region FromSap

        static public ModelMapping<Z_ZLD_AH_FS_STATISTIK.GT_FSP, FeinstaubVergabeInfo> Z_ZLD_AH_FS_STATISTIK_GT_FSP_To_FeinstaubVergabeInfo
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_AH_FS_STATISTIK.GT_FSP, FeinstaubVergabeInfo>(
                    new Dictionary<string, string> {
                        { "KENNZ", "Kennzeichen" },
                        { "SELLDAT", "Erfassungsdatum" },
                        { "PLAKART", "Plakettenart" },
                        { "WEB_USER", "Erfasser" },
                    }));
            }
        }

        #endregion


        #region ToSap

        #endregion
    }
}