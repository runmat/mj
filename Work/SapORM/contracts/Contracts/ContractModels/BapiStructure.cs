using System.Collections.Generic;
using System.Data;

namespace SapORM.Contracts
{
    public class BapiStructure
    {
        public List<BapiField> ImportParameters { get; set; }

        public List<BapiField> ExportParameters { get; set; }

        public List<BapiField> Parameters { get; set; }

        public List<DataTable> ImportTables { get; set; }

        public List<DataTable> ExportTables { get; set; }

        public List<DataTable> Tables { get; set; }

        public BapiStructure()
        {
            ImportParameters = new List<BapiField>();
            ExportParameters = new List<BapiField>();
            Parameters = new List<BapiField>();
            ImportTables = new List<DataTable>();
            ExportTables = new List<DataTable>();
            Tables = new List<DataTable>();
        }
    }
}
