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
    public class PageVisitLogItemSelector : Store, IValidatableObject 
    {
        [LocalizedDisplay(LocalizeConstants.Date)]
        public DateRange DatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days)); } set { PropertyCacheSet(value); } }

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

            var sql = "SELECT * FROM " + tableAttribute.Name;

            sql += PortalTypes.GetSqlMultiselectCondition(sql, "PortalType");
            sql += AppIds.GetSqlMultiselectCondition(sql, "AppID");
            sql += CustomerIds.GetSqlMultiselectCondition(sql, "CustomerID");
            sql += UserIds.GetSqlMultiselectCondition(sql, "UserID");

            sql += DatumRange.GetSqlDateRangeCondition(sql, "time_stamp");

            return sql;
        }
    }
}
