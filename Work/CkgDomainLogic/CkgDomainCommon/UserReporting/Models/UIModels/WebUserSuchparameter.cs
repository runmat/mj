using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using GeneralTools.Log.Models.MultiPlatform;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.UserReporting.Models
{
    public class WebUserSuchparameter : Store
    {
        [LocalizedDisplay(LocalizeConstants.Customer)]
        public int CustomerId { get; set; }

        [XmlIgnore]
        public static List<MpCustomer> AllCustomers { get; set; }

        [XmlIgnore]
        public MpCustomer Customer { get { return AllCustomers.FirstOrDefault(c => c.CustomerID == CustomerId) ?? new MpCustomer(); } }

        [LocalizedDisplay(LocalizeConstants.Selection)]
        public string UserSelection { get; set; }

        [XmlIgnore]
        public string UserSelectionOptions
        {
            get
            {
                return String.Format("A,{0};N,{1};G,{2}",
                    Localize.SelectionActiveUsers,
                    Localize.SelectionNewUsers,
                    Localize.SelectionDeletedUsers);
            }
        }

        [LocalizedDisplay(LocalizeConstants.Date)]
        public DateRange DatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.LastMonth, true)); } set { PropertyCacheSet(value); } }
    }
}
