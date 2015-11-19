using System.Collections.Generic;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;
using SapORM.Models;


namespace CkgDomainInternal.Verbandbuch.Models
{
    class AppModelMappings : ModelMappings
    {
        static public ModelMapping<Z_VB_EXPORT_FAELLE.GT_VERBANDBUCH, VerbandbuchModel> Z_VB_EXPORT_FAELLE_TO_WEB
        {
            get { return EnsureSingleton(() => new ModelMapping<Z_VB_EXPORT_FAELLE.GT_VERBANDBUCH, VerbandbuchModel>(
                                                 new Dictionary<string, string>(),

                                                 (s, d) =>
                                                 {
                                                     d.AccidentNo = s.ID;
                                                     d.TimeOfAccident = s.ZEIT_UNF;
                                                     d.DateOfAccident = s.DATUM_UNF;
                                                     d.TimeOfFirstAid = s.ZEIT_HILF;
                                                     d.DateOfFirstAid = s.DATUM_HILF;
                                                     d.Vkbur = s.VKBUR;
                                                     d.CourseOfAccident = s.HERGANG;
                                                     d.FirstAidMeasures = s.ART_HILF;
                                                     d.FirstResponder = s.NAME_HELFER;
                                                     d.InjuredPerson = s.NAME_VERL;
                                                     d.Injury = s.ART_VERL;
                                                     d.WitnessName = s.NAME_ZEUG;
                                                     d.PlaceOfAccident = s.ORT;
                                                 }));
            }

        }
    }
}
