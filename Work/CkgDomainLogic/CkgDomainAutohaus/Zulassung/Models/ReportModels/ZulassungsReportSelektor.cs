using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Autohaus.Models
{
    public class ZulassungsReportSelektor : Store
    {
        [Required]
        [LocalizedDisplay(LocalizeConstants.Customer)]
        public string KundenNr { get; set; }

        [XmlIgnore]
        static public List<Kunde> KundenList { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen
        {
            get { return PropertyCacheGet(() => "").NotNullOrEmpty().ToUpper(); }
            set { PropertyCacheSet(value.NotNullOrEmpty().ToUpper()); }
        }

        [LocalizedDisplay(LocalizeConstants.DispatchType)]
        public string AuftragsArt
        {
            get { return PropertyCacheGet(() => "1"); }
            set { PropertyCacheSet(value); }
        }

        [LocalizedDisplay(LocalizeConstants.OrderDate)]
        public DateRange AuftragsDatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days)); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        public DateRange ZulassungsDatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last3Months, true)); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.Reference1)]
        public string Referenz1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Reference2)]
        public string Referenz2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Reference3)]
        public string Referenz3 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Reference4)]
        public string Referenz4 { get; set; }
    }
}
