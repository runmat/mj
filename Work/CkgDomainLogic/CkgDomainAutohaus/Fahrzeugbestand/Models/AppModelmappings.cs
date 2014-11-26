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

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_AHP_READ_FZGBESTAND.GT_WEBOUT, FahrzeugAkteBestand> Z_AHP_READ_FZGBESTAND_GT_WEBOUT_To_FahrzeugAkteBestand
        // ReSharper restore InconsistentNaming
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
                            d.Halter = s.HALTER;
                            d.Kaeufer = s.KAEUFER;
                        }));
            }
        }

        #endregion
   }
}