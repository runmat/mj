using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;
using GeneralTools.Log.Models;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;
using MvcTools.Web;

namespace CkgDomainLogic.Logs.Models
{
    public class SapLogItemSelector : Store, IValidatableObject 
    {
        [LocalizedDisplay(LocalizeConstants.Date)]
        public DateRange DatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days)); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.LongRunner)]
        public bool ShowMaxDauerProBapiAndKunde { get; set; }

        [LocalizedDisplay(LocalizeConstants.UserName)]
        public List<string> UserIds { get; set; }

        public static SelectList AllUsers { get; set; }
        
        
        [LocalizedDisplay(LocalizeConstants.Customer)]
        public List<int> CustomerIds { get; set; }

        public static SelectList AllCustomers { get; set; }


        [LocalizedDisplay(LocalizeConstants.Application)]
        public List<int> AppIds { get; set; }

        public static SelectList AllApplications { get; set; }


        [LocalizedDisplay(LocalizeConstants.Portal)]
        public List<int> PortalTypes { get; set; }

        public SelectList AllPortalTypes { get { return LogConstants.PortalTypes.ToSelectList(); } }


        [LocalizedDisplay(LocalizeConstants.OnlyDurationAtLeast)]
        public int BapiDurationMinimumLevel { get; set; }

        public SelectList AllSapBapiDurationLevels { get { return SapBapiDuration.Presets.ToSelectList(); } }


        [LocalizedDisplay(LocalizeConstants.Server)]
        public string LogsConnection { get; set; }

        public string AllLogsConnections { get { return "LogsTest,Test;LogsProd,Prod"; } }


        public bool SubmitWithNoDataQuerying { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //if (!DatumRange.IsSelected)
            //    yield return
            //        new ValidationResult(Localize.DateRangeInvalid);

            return new List<ValidationResult>();
        }

        public string GetSqlSelectStatement()
        {
            var tableAttribute = typeof(SapLogItem).GetCustomAttributes(false).OfType<TableAttribute>().FirstOrDefault();
            if (tableAttribute == null)
                return "";

            string sql;
            if (ShowMaxDauerProBapiAndKunde)
                sql = @"SELECT * FROM SapBapiView inner join (select Bapi as d_bapi, customerID as d_customerID, DATE(time_stamp) as ds_timestamp, max(dauer) as d_max_dauer from SapBapi group by Bapi, customerID, DAY(time_stamp) ) as ds on Bapi = ds.d_Bapi and customerID = ds.d_customerID and DATE(time_stamp) = DATE(ds_timestamp) and dauer = ds.d_max_dauer ";
            else
                sql = "SELECT * FROM " + tableAttribute.Name;

            sql += PortalTypes.GetSqlMultiselectCondition(sql, "PortalType");
            sql += AppIds.GetSqlMultiselectCondition(sql, "AppID");
            sql += CustomerIds.GetSqlMultiselectCondition(sql, "CustomerID");
            sql += UserIds.GetSqlMultiselectCondition(sql, "UserID");

            sql += DatumRange.GetSqlDateRangeCondition(sql, "time_stamp");

            if (BapiDurationMinimumLevel > 0)
                sql += sql.GetSqlKeyWordWhereAnd() + " dauer >= " + SapBapiDuration.GetSapBapiDuration(BapiDurationMinimumLevel).ThresholdUpFromSeconds;
            
            return sql;
        }
    }
}
