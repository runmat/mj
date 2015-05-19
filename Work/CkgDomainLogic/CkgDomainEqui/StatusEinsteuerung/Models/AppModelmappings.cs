using System.Collections.Generic;
using GeneralTools.Contracts;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;

namespace CkgDomainLogic.FzgModelle.Models
{
    public class AppStatusEinsteuerungModelMappings : ModelMappings
    {
        #region Load from Repository

        static public ModelMapping<Z_M_EC_AVM_STATUS_EINSTEUERUNG.GT_WEB, StatusEinsteuerung> Z_M_EC_AVM_STATUS_EINSTEUERUNG_GT_WEB_To_StatusEinsteuerung
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_EC_AVM_STATUS_EINSTEUERUNG.GT_WEB, StatusEinsteuerung>(
                                                 new Dictionary<string, string>(),
                                                 (s, d) =>
                                                     {
                                                         



                                                         // d.Bluetooth = s.BLUETOOTH.XToBool();
                                                     }));
            }
        }

        #endregion


    }
}