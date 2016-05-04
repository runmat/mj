using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Leasing.Models.UIModels
{
    public class AbweichWiedereingangSelektor : Store
    {
        [LocalizedDisplay(LocalizeConstants.DateRange)]
        public DateRange SelectionRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days, true)); } set { PropertyCacheSet(value); } }

    }
}
