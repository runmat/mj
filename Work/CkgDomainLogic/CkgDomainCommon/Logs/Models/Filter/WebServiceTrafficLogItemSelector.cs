using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Logs.Models
{
    public class WebServiceTrafficLogItemSelector : Store, IValidatableObject 
    {
        [LocalizedDisplay(LocalizeConstants.Date)]
        public DateRange DatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last7Days, true)); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.Type)]
        public string SelectedType { get; set; }

        public string AllTypes { get { return ";Request;Response"; } }

        [Required]
        [LocalizedDisplay(LocalizeConstants.Selection)]
        public string SelectedLogTable { get; set; }

        [LocalizedDisplay(LocalizeConstants.Server)]
        public string LogsConnection { get; set; }

        public string AllLogsConnections { get { return "LogsTest,Test;LogsProd,Prod"; } }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DatumRange.StartDate.HasValue && DatumRange.EndDate.HasValue && DatumRange.StartDate.Value > DatumRange.EndDate.Value)
                yield return new ValidationResult(Localize.DateRangeInvalid);
        }

        public string GetSqlSelectStatement()
        {
            if (String.IsNullOrEmpty(SelectedLogTable))
                return "";

            string sql = "SELECT * FROM " + SelectedLogTable + " ";

            if (!String.IsNullOrEmpty(SelectedType))
                sql += "WHERE Type = '" + SelectedType + "' ";

            sql += DatumRange.GetSqlDateRangeCondition(sql, "time_stamp");

            return sql;
        }
    }
}
