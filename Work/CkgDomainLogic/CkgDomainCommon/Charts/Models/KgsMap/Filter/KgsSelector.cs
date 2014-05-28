using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using GeneralTools.Services;

namespace CkgDomainLogic.Charts.Models
{
    public class KgsSelector : Store, IValidatableObject 
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }

        private static string GetSqlTableName()
        {
            var tableAttribute = typeof(KbaPlzKgs).GetCustomAttributes(false).OfType<TableAttribute>().FirstOrDefault();
            if (tableAttribute == null)
                return "";

            return tableAttribute.Name;
        }

        private string GetSqlFilterString(string sql)
        {
            //sql += PortalTypes.GetSqlMultiselectCondition(sql, "PortalType");
            //sql += AppIds.GetSqlMultiselectCondition(sql, "AppID");
            //sql += CustomerIds.GetSqlMultiselectCondition(sql, "CustomerID");
            //sql += UserIds.GetSqlMultiselectCondition(sql, "UserID");

            //sql += DatumRange.GetSqlDateRangeCondition(sql, "time_stamp");

            return "";
        }

        public string GetDetailsSqlSelectStatement()
        {
            var sql = "SELECT * FROM " + GetSqlTableName();
            sql += GetSqlFilterString(sql);

            return sql;
        }

        public string GetGroupSqlSelectStatement()
        {
            var sql = "SELECT KGS, SUM(FahrzeugAnzahl) FahrzeugAnzahl FROM " + GetSqlTableName();
            sql += GetSqlFilterString(sql);
            sql += " group by KGS order by KGS";

            return sql;
        }
    }
}
