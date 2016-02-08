using System.Collections.Generic;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;
using SapORM.Models;

namespace CkgDomainLogic.Autohaus.Models
{
    public partial class AppModelMappings
    {
        #region FromSap

        static public ModelMapping<Z_DPM_READ_ZDAD_AUFTR_006.GT_WEB, Hersteller> Z_DPM_READ_ZDAD_AUFTR_006_GT_WEB_To_Hersteller
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_ZDAD_AUFTR_006.GT_WEB, Hersteller>(
                     new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.Code = s.POS_KURZTEXT;
                        d.Name = s.POS_KURZTEXT;
                    }));
            }
        }

        #endregion


        #region ToSap

        #endregion
    }
}