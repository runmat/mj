using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Logs.Models
{
    public class ErrorLogItemSelector : Store, IValidatableObject 
    {
        [LocalizedDisplay(LocalizeConstants.Date)]
        public DateRange DatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Today, true)); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.Server)]
        public string LogsConnection { get; set; }

        public string AllLogsConnections { get { return "LogsTest,Test;LogsProd,Prod;LogsOnTest,Test ON;LogsOnProd,Prod ON;LogsCkgTest,Test CKG;LogsCkgProd,Prod CKG"; } }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DatumRange.StartDate.HasValue && DatumRange.EndDate.HasValue && DatumRange.StartDate.Value > DatumRange.EndDate.Value)
                yield return new ValidationResult(Localize.DateRangeInvalid);
        }

        public string GetSqlSelectStatement()
        {
            var tableAttribute = typeof(ErrorLogItem).GetCustomAttributes(false).OfType<TableAttribute>().FirstOrDefault();
            if (tableAttribute == null)
                return "";

            string sql = "SELECT * FROM " + tableAttribute.Name;

            sql += DatumRange.GetSqlDateRangeCondition(sql, "timeutc");

            return sql;
        }
    }
}
