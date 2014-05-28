using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using GeneralTools.Services;

namespace CkgDomainLogic.General.Services
{
    public static class DbContextExtensions
    {
        public static void GetDynamicObjects(this DbContext dbContext, string sqlStatement, out Type dynamicType, out IEnumerable<dynamic> dynamicObjects)
        {
            dynamicType = null;
            dynamicObjects = null;

            DataTable tb;
            using (var sc = new SqlConnection(dbContext.Database.Connection.ConnectionString))
            {
                sc.Open();
                var cmd = new SqlCommand(sqlStatement, sc);
                var sr = cmd.ExecuteReader(CommandBehavior.SchemaOnly);
                tb = sr.GetSchemaTable();
                sc.Close();
            }
            if (tb == null)
                return;

            var builder = DynamicTypeBuilder.CreateTypeBuilder("Dynamic Type Assembly", "Dynamic Type Module", "Dynamic Type from Sql: " + sqlStatement);
            foreach (DataRow row in tb.Rows)
            {
                var name = (string)row["ColumnName"];
                var type = (Type)row["DataType"];
                DynamicTypeBuilder.CreateAutoImplementedProperty(builder, name, type);
            }

            dynamicType = builder.CreateType();
            dynamicObjects = dbContext.Database.SqlQuery(dynamicType, sqlStatement).Cast<dynamic>();            
        }
    }
}
