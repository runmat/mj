// ReSharper disable InconsistentNaming
// ReSharper disable RedundantUsingDirective
using System.Collections.Generic;
using GeneralTools.Models;
using SapORM.Models;

namespace CkgDomainLogic.FzgModelle.Models
{
    public class AppModelMappings : ModelMappings
    {
        #region Load from Repository

        static public ModelMapping<Z_DPM_READ_MODELID_TAB.GT_OUT, ModellId> Z_DPM_READ_MODELID_TAB__GT_OUT_To_ModellId
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_MODELID_TAB.GT_OUT, ModellId>(
                                                 new Dictionary<string, string>(),
                                                 (s, d) =>
                                                     {
                                                         d.ID = s.ZZMODELL;
                                                         d.Bezeichnung = s.ZZBEZEI;
                                                     }));
            }
        }

        #endregion


        #region Save to Repository

        #endregion
    }
}