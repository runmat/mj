using System;
using System.Collections.Generic;

namespace SapORM.Contracts
{
    [Serializable]
    public class BapiStructure
    {
        public List<BapiField> ImportParameters { get; set; }

        public List<BapiField> ExportParameters { get; set; }

        public List<BapiField> Parameters { get; set; }

        public Dictionary<string, List<BapiField>> ImportTables { get; set; }

        public Dictionary<string, List<BapiField>> ExportTables { get; set; }

        public Dictionary<string, List<BapiField>> Tables { get; set; }

        public BapiStructure()
        {
            ImportParameters = new List<BapiField>();
            ExportParameters = new List<BapiField>();
            Parameters = new List<BapiField>();
            ImportTables = new Dictionary<string, List<BapiField>>();
            ExportTables = new Dictionary<string, List<BapiField>>();
            Tables = new Dictionary<string, List<BapiField>>();
        }
    }
}
