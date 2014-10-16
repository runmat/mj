using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Logs.Models
{
    [Table("PageVisitsPerCustomerPerDayView")]
    public class PageVisitLogItem
    {
        [LocalizedDisplay(LocalizeConstants.CustomerNo)]
        [NotMapped]
        [GridResponsiveVisible(GridResponsive.Workstation)]
        public int? CustomerNo { get { return KUNNR; } }

        [LocalizedDisplay(LocalizeConstants.Customer)]
        public string CustomerName { get; set; }

        [GridResponsiveVisible(GridResponsive.Workstation)]
        [LocalizedDisplay(LocalizeConstants.App)]
        public int? AppID { get; set; }

        [LocalizedDisplay(LocalizeConstants.Application)]
        public string AppFriendlyName { get; set; }

        [LocalizedDisplay(LocalizeConstants.Hits)]
        public int Hits { get; set; }


        #region Grid Hidden

        [Key]
        [GridHidden]
        public int Id { get; set; }

        [LocalizedDisplay(LocalizeConstants.CustomerID)]
        [GridHidden]
        public int? CustomerID { get; set; }

        [LocalizedDisplay(LocalizeConstants.Date)]
        [GridHidden]
        public DateTime Datum { get; set; }

        // ReSharper disable InconsistentNaming
        [GridHidden]
        public int? KUNNR { get; set; }
        // ReSharper restore InconsistentNaming

        #endregion
    }
}
