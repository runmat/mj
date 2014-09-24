using System.Data;

namespace CkgDomainLogic.Logs.Models
{
    public class SapCallContext
    {
        public DataTable ImportParameters { get; set; }

        public DataTable[] ImportTables { get; set; }

        public DataTable ExportParameters { get; set; }

        public ExportTable[] ExportTables { get; set; }
    }

    public class ExportTable
    {
        public string TableName { get; set; }
        public string RowCount { get; set; }
    }
}
