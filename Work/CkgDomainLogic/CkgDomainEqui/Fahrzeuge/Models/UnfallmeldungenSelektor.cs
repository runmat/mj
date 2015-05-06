using System.Collections.Generic;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;
using System.Linq;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class UnfallmeldungenSelektor : Store
    {

        [LocalizedDisplay(LocalizeConstants.ReportingDate)]
        public DateRange MeldeDatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days)); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.DeRegistrationDate)]
        public DateRange StillegungsDatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days)); } set { PropertyCacheSet(value); } }

    }
}
