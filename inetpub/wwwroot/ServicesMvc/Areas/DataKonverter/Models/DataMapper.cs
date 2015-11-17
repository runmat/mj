using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ServicesMvc.Areas.DataKonverter.Models
{
    public class DataMapper
    {
        public SourceFile SourceFile { get; set; }                  // Enthält Fields und dazugehörige Value-Listen
        public DestinationFile DestinationFile { get; set; }
        public List<DataConnection> DataConnections { get; set; }
        public List<Processor> Processors { get; set; }

        public DataMapper()
        {
            SourceFile = new SourceFile();
            DestinationFile = new DestinationFile();
            DataConnections = new List<DataConnection>();
            Processors = new List<Processor>();
        }

        public DataConnection AddConnection(DataConnection dataConnection)
        {
            // Prüfen, ob Verbindung bereits besteht...
            if (DataConnections.Count(x => x.GuidSource == dataConnection.GuidSource && x.GuidDest == dataConnection.GuidDest) > 0)
            {
                return null;
            }
            
            // Verbindung hinzufügen
            // var newConnection = new DataConnection();
            DataConnections.Add(dataConnection);

            return dataConnection;
        }

        public DataConnection RemoveConnection(DataConnection dataConnection)
        {
            return dataConnection;
        }

        public List<DataConnection> GetConnections()
        {
            return DataConnections;
        }

        public Guid AddProcessor()
        {
            var newProcessor = new Processor();
            Processors.Add(newProcessor);
            return newProcessor.Guid;           
        }

        public string RemoveProcessor(Guid processorId)
        {
            var processor = Processors.FirstOrDefault(x => x.Guid == processorId);
            Processors.Remove(processor);
            return processorId.ToString();
        }

        public Processor GetProcessorResult(Processor processor, int recordNo)   // string fieldGuid, string origValue
        {
            // Alle eingehenden Connections ermitteln...
            var connectionsIn = DataConnections.Where(x => x.GuidDest == processor.Guid);

            // Alle ausgehenden Connections ermitteln...
            var connectionsOut = DataConnections.Where(x => x.GuidSource == processor.Guid);

            // Input-String ermitteln...
            var input = new StringBuilder();

            foreach (var dataConnection in connectionsIn)
            {
                var sourceField = SourceFile.Fields.FirstOrDefault(x => x.Guid == dataConnection.GuidSource);
                if (sourceField != null)
                {
                    var value = sourceField.Records[recordNo];
                    input.Append(value);
                    input.Append("*#*");    
                }
            }

            return null;
        }

        public string GetFieldResult(string fieldGuid, string origValue)
        {
            // Alle Connections ermitteln...
            // var connections = DataConnections.Where(x => x.)

            return null;
        }
        
    }
}