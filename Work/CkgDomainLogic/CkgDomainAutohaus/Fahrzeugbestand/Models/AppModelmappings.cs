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

                            // Fahrzeug Akte
                            d.FabrikName = s.ZZFABRIKNAME;
                            d.HandelsName= s.ZZHANDELSNAME;
                            d.HerstellerSchluessel= s.ZZHERSTELLER_SCH;
                            d.TypSchluessel= s.ZZTYP_SCHL;
                            d.VvsSchluessel= s.ZZVVS_SCHLUESSEL;
                            d.VvsPruefZiffer= s.ZZTYP_VVS_PRUEF;

                            // Fahrzeug Bestand
                            d.Halter = s.HALTER;
                            d.Kaeufer = s.KAEUFER;
                        }));
            }
        }

        #endregion
   }
}