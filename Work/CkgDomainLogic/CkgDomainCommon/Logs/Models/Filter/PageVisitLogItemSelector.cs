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


        [LocalizedDisplay(LocalizeConstants.Customer)]
        public List<int> CustomerIds { get; set; }

        public static SelectList AllCustomers { get; set; }


        [LocalizedDisplay(LocalizeConstants.Application)]
        public List<int> AppIds { get; set; }

        public static SelectList AllApplications { get; set; }


        [LocalizedDisplay(LocalizeConstants.Server)]
        public string LogsConnection { get; set; }

        public string AllLogsConnections { get { return "LogsTest,Test;LogsProd,Prod;LogsOnTest,Test ON;LogsOnProd,Prod ON"; } }

        [LocalizedDisplay(LocalizeConstants.OnlyUnusedApplications)]
        public bool OnlyUnusedApplications { get; set; }

        [LocalizedDisplay(LocalizeConstants.OnlyUnusedCustomerApplications)]
        public bool OnlyUnusedCustomerApplications { get; set; }

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
            var tableAttribute = typeof(PageVisitLogItem).GetCustomAttributes(false).OfType<TableAttribute>().FirstOrDefault();
            if (tableAttribute == null)
                return "";

            var sql = "SELECT * FROM " + tableAttribute.Name;

            sql += CustomerIds.GetSqlMultiselectCondition(sql, "CustomerID");
            sql += AppIds.GetSqlMultiselectCondition(sql, "AppID");
            sql += DatumRange.GetSqlDateRangeCondition(sql, "Datum");

            return sql;
        }

        public string GetCustomerApplicationsSqlSelectStatement()
        {
            var sql = "SELECT * FROM CustomerApplicationsView";

            sql += CustomerIds.GetSqlMultiselectCondition(sql, "CustomerID");
            sql += AppIds.GetSqlMultiselectCondition(sql, "AppID");

            return sql;
        }
    }
}
