using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeneralTools.Log.Models;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Logs.Models
{
    [Table("SapBapiView")]
    public class SapLogItem
    {
        public string Bapi { get; set; }

        [LocalizedDisplay(LocalizeConstants.Date)]
        [NotMapped]
        public DateTime Datum { get { return time_stamp; } }
        
        [LocalizedDisplay(LocalizeConstants.CustomerNo)]
        [NotMapped]
        [GridResponsiveVisible(GridResponsive.Workstation)]
        public int? CustomerNo { get { return KUNNR; } }

        [LocalizedDisplay(LocalizeConstants.Customer)]
        public string CustomerName { get; set; }

        public double? Dauer { get; set; }

        [GridResponsiveVisible(GridResponsive.Workstation)]
        [LocalizedDisplay(LocalizeConstants.App)]
        public int? AppID { get; set; }

        [LocalizedDisplay(LocalizeConstants.Application)]
        public string AppFriendlyName { get; set; }

        [GridResponsiveVisible(GridResponsive.Workstation)]
        public int? UserID { get; set; }

        [LocalizedDisplay(LocalizeConstants.UserName)]
        public string UserName { get; set; }

        [LocalizedDisplay(LocalizeConstants.Browser)]
        public string Browser { get; set; }

        [NotMapped]
        public string Portal { get { return LogConstants.PortalTypes.TryGetValue(PortalType.GetValueOrDefault()); } }


        [GridRawHtml, NotMapped]
        [LocalizedDisplay(LocalizeConstants.StackContext)]
        public string StackContextHtml { get { return StackContextItemTemplate == null ? "" : StackContextItemTemplate(this.StackContext); } }


        #region Grid Hidden

        [Key]
        [GridHidden]
        public int Id { get; set; }

        [LocalizedDisplay(LocalizeConstants.CustomerID)]
        [GridHidden]
        public int? CustomerID { get; set; }

        [GridHidden]
        public string LogonContext { get; set; }

        [GridHidden]
        public int? PortalType { get; set; }

        [GridHidden]
        public string DataContext { get; set; }

        // ReSharper disable InconsistentNaming
        [GridHidden]
        public DateTime time_stamp { get; set; }

        [GridHidden]
        public int? KUNNR { get; set; }
        // ReSharper restore InconsistentNaming

        [NotMapped]
        [GridHidden]
        public string DauerCssClass { get { return SapBapiDuration.GetSapBapiDuration(Dauer).LevelCssClass; } }

        [GridHidden, NotMapped]
        public StackContext StackContext
        {
            get
            {
                if (StackContextItemTemplate == null || DataContext.IsNullOrEmpty())
                    return new StackContext();

                return XmlService.XmlTryDeserializeFromString<StackContext>(DataContext);
            }
        }

        [GridHidden, NotMapped]
        public static Func<StackContext, string> StackContextItemTemplate { get; set; }

        #endregion

        /*
         * ITA 7295
        Hier das SQL Script was gegen die Prod Datenbank laufen muss.  Es ist bereits gegen DEV gelaufen
        */

        /*

ALTER TABLE SapBapi ADD Browesr VARCHAR(60) AFTER portalType;
DROP VIEW `SapBapiView`;
CREATE 
    ALGORITHM = UNDEFINED 
    DEFINER = `karl2heinz`@`%` 
    SQL SECURITY DEFINER
VIEW `SapBapiView` AS
    select 
        `U`.`Username` AS `UserName`,
        `C`.`Customername` AS `CustomerName`,
        `A`.`AppFriendlyName` AS `AppFriendlyName`,
        `A`.`AppName` AS `AppName`,
        `L`.`Id` AS `Id`,
        `L`.`Anmeldename` AS `Anmeldename`,
        `L`.`Bapi` AS `Bapi`,
        `L`.`ImportParameters` AS `ImportParameters`,
        `L`.`ImportTables` AS `ImportTables`,
        `L`.`DataContext` AS `DataContext`,
        `L`.`LogonContext` AS `LogonContext`,
        `L`.`Status` AS `Status`,
        `L`.`time_stamp` AS `time_stamp`,
        `L`.`dauer` AS `dauer`,
        `L`.`ExportParameters` AS `ExportParameters`,
        `L`.`ExportTables` AS `ExportTables`,
        `L`.`AppId` AS `AppId`,
        `L`.`userID` AS `userID`,
        `L`.`customerID` AS `customerID`,
        `L`.`kunnr` AS `kunnr`,
        `L`.`portalType` AS `portalType`,
        `L`.`Browser` AS `Browser`
    from
        (((`LogsTest`.`SapBapi` `L`
        join `Customer` `C` ON ((`L`.`customerID` = `C`.`CustomerID`)))
        join `WebUser` `U` ON ((`L`.`userID` = `U`.`UserID`)))
        join `ApplicationTranslated` `A` ON ((`L`.`AppId` = `A`.`AppID`)))



        */

    }
}
