using System.Collections.Generic;

namespace CkgDomainLogic.DataConverter.Models
{
    public class DataMappingModel
    {
        public List<DataConnection> DataConnections { get; set; }
        public List<Processor> Processors { get; set; }

        public SourceFile SourceFile { get; set; }
        public ProcessStructure DestinationStructure { get; set; }

        public int RecordNo { get; set; }
        public int RecordCount { get { return SourceFile.RowCount; } }
        public string RecordInfoText { get { return string.Format("{0}/{1}", RecordNo, RecordCount); }  }

        public DataMappingModel()
        {
            DataConnections = new List<DataConnection>();
            Processors = new List<Processor>();
            SourceFile = new SourceFile();
            DestinationStructure = new ProcessStructure();

            RecordNo = 1;
        }
    }
}
