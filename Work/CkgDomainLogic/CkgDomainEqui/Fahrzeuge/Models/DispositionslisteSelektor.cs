using System;
using System.Collections.Generic;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Fahrzeuge.Models
{
      
    public class DispositionslisteSelektor : Store 
    {

        [LocalizedDisplay(LocalizeConstants.PDINumber)]
        public string PDINummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        public DateRange ZulassungsdatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Today)); } set { PropertyCacheSet(value); } }                              
    }      

}
