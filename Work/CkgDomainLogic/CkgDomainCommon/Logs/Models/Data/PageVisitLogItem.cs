using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeneralTools.Log.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Logs.Models
{
    [Table("PageVisitView")]
    public class PageVisitLogItem
    {
        [LocalizedDisplay(LocalizeConstants.Date)]
        [NotMapped]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yy HH:mm}")]
        public DateTime Datum { get { return time_stamp; } }
        
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

        [GridResponsiveVisible(GridResponsive.Workstation)]
        public int? UserID { get; set; }

        [LocalizedDisplay(LocalizeConstants.UserName)]
        public string UserName { get; set; }

        [NotMapped]
        public string Portal { get { return LogConstants.PortalTypes.TryGetValue(PortalType.GetValueOrDefault()); } }


        #region Grid Hidden

        [Key]
        [GridHidden]
        public int Id { get; set; }

        [LocalizedDisplay(LocalizeConstants.CustomerID)]
        [GridHidden]
        public int? CustomerID { get; set; }

        [GridHidden]
        public int? PortalType { get; set; }

        // ReSharper disable InconsistentNaming
        [GridHidden]
        public DateTime time_stamp { get; set; }

        [GridHidden]
        public int? KUNNR { get; set; }
        // ReSharper restore InconsistentNaming

        #endregion
    }
}
