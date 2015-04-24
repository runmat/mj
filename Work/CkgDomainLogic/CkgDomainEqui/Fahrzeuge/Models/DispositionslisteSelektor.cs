using System.Collections.Generic;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;
using System.Linq;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class DispositionslisteSelektor : Store 
    {

        [LocalizedDisplay(LocalizeConstants.PDINumber)]
        public string PDINummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        public DateRange ZulassungsdatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days)); } set { PropertyCacheSet(value); } }                              
    }
}
