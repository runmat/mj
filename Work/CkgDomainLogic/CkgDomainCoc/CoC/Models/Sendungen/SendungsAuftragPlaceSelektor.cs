using System.Collections.Generic;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.CoC.Models
{
    public class SendungsAuftragPlaceSelektor : Store
    {
        [LocalizedDisplay(LocalizeConstants.DateRange)]
        public DateRange DatumRangeZul
        {
            get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last7Days) { IsSelected = true }); }
            set { PropertyCacheSet(value); }
        }

        public List<SelectItem> Standorte
        {
            get
            {
             return 
            }
            
        }

        public string Fahrzeugstandort { get; set; }

        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string FIN { get; set; }

        [LocalizedDisplay(LocalizeConstants._Kennzeichen)]
        public string Kennzeichen { get; set; }


    }
}