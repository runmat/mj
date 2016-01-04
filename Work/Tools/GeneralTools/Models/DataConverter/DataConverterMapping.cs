using System.Collections.Generic;

namespace GeneralTools.Models
{
    public class DataConverterMapping
    {
        public List<DataConnection> DataConnections { get; set; }
        public List<Processor> Processors { get; set; }

        public DataConverterMapping()
        {
            DataConnections = new List<DataConnection>();
            Processors = new List<Processor>();
        }
    }
}