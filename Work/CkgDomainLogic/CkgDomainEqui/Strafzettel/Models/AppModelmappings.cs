using System.Collections.Generic;
// ReSharper disable RedundantUsingDirective
using CkgDomainLogic.General.Models;
// ReSharper restore RedundantUsingDirective
using GeneralTools.Models;
using SapORM.Models;

namespace CkgDomainLogic.Strafzettel.Models
{
    public class AppModelMappings : ModelMappings
    {
        #region Load from Repository

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_DPM_CD_STRAFZETTEL.GT_OUT, StrafzettelModel> Z_DPM_CD_ABM_LIST__ET_ABM_LIST_To_Strafzettel
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_CD_STRAFZETTEL.GT_OUT, StrafzettelModel>(
                                                 new Dictionary<string, string>(),
                                                 //{
                                                 //    { "VERTRAGS_NR", "VertragsNr" },
                                                 //    { "AKTENZEICHEN", "Aktenzeichen" },
                                                 //    { "LICENSE_NUM", "Kennzeichen" },

                                                 //    { "EIGDA", "EingangsDatum" },
                                                 //    { "DATUM_BEHOERDE", "BehoerdeDatum" },
                        
                                                 //    { "NAME1_AMT", "BehoerdeName" },
                                                 //    { "POST_CODE1_AMT", "BehoerdePlz" },
                                                 //}));
                                                 (s, d) =>
                                                     {
                                                         d.VertragsNr = s.VERTRAGS_NR;
                                                         d.Aktenzeichen = s.AKTENZEICHEN;
                                                         d.Kennzeichen = s.LICENSE_NUM;
                                                         d.EingangsDatum = s.EIGDA;
                                                         d.BehoerdeDatum = s.DATUM_BEHOERDE;
                                                         d.BehoerdeName = s.NAME1_AMT;
                                                         d.BehoerdePlz = s.POST_CODE1_AMT;
                                                     }));
            }
        }

        #endregion


        #region Save to Repository

        

        #endregion
    }
}