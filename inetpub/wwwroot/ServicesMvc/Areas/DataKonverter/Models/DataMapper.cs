using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using SapORM.Models;

namespace ServicesMvc.Areas.DataKonverter.Models
{
    public class DataMapper
    {
        public SourceFile SourceFile { get; set; }                  // Enthält Fields und dazugehörige Value-Listen
        public DestinationFile DestinationFile { get; set; }
        public List<DataConnection> DataConnections { get; set; }
        public List<Processor> Processors { get; set; }
        
        public int RecordNo { get; set; }

        public int RecordCount
        {
            get { return SourceFile.RowCount; }
        }

        public string RecordInfoText
        {
            get { return string.Format("{0}/{1}", RecordNo, RecordCount); } 
        }

        public DataMapper()
        {
            RecordNo = 1;
            
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

        public DataConnection AddConnection(string idSource, string idDest, bool sourceIsProcessor, bool destIsProcessor)
        {

            var newConnection = new DataConnection
            {
                GuidSource = idSource,
                GuidDest = idDest,
                SourceIsProcessor = sourceIsProcessor,
                DestIsProcessor = destIsProcessor
            };

            // Prüfen, ob Verbindung bereits besteht...
            if (DataConnections.Count(x => x.GuidSource == newConnection.GuidSource && x.GuidDest == newConnection.GuidDest) > 0)
            {
                return null;
            }
            
            // Verbindung hinzufügen
            DataConnections.Add(newConnection);

            Debug.WriteLine(DataConnections);

            //// Falls Destination ein Prozessor, dann die dortige Verbindungsliste entsprechend erweitern
            //if (destIsProcessor)
            //{
            //    var destProcessor = Processors.FirstOrDefault(x => x.Guid == new Guid(idDest));

            //    if (destProcessor != null && destProcessor.DataConnectionsIn.FirstOrDefault(x => x.GuidDest == destProcessor.Guid && x.GuidSource == newConnection.GuidSource) == null)
            //    {
            //        // Neue Verbindung zur Liste hinzufügen
            //        destProcessor.DataConnectionsIn.Add(newConnection);                    
            //    }
            //}

            return newConnection;
        }

        public DataConnection RemoveConnection(DataConnection dataConnection)
        {
            return dataConnection;
        }

        public List<DataConnection> GetConnections()
        {
            return DataConnections;
        }

        public string AddProcessor()
        {
            var newProcessor = new Processor();
            Processors.Add(newProcessor);
            
            // return System.Guid.NewGuid().ToString();         
            return newProcessor.Guid;         
        }

        public string RemoveProcessor(string processorId)
        {
            var processor = Processors.FirstOrDefault(x => x.Guid == processorId);
            Processors.Remove(processor);
            return processorId.ToString();
        }

        public Processor GetProcessorResult(Processor processor)
        {
            // Alle eingehenden Connections ermitteln...
            var connectionsIn = DataConnections.Where(x => x.GuidDest == processor.Guid).ToList();

            // Alle ausgehenden Connections ermitteln...
            var connectionsOut = DataConnections.Where(x => x.GuidSource == processor.Guid).ToList();

            // Input-String ermitteln...
            var input = new StringBuilder();

            foreach (var dataConnection in connectionsIn)
            {
                var sourceField = SourceFile.Fields.FirstOrDefault(x => x.Guid == dataConnection.GuidSource);
                if (sourceField != null)
                {
                    var value = sourceField.Records[RecordNo - 1];
                    input.Append(value);
                    input.Append("*#*");    
                }
            }
            if (input.Length > 0)
                input = input.Remove(input.Length-3,3);

            processor.Input = input.ToString();
            
            // Operation durchführen...
            processor.Output = "#" + input;

            return processor;
        }

        public string GetFieldResult(string fieldGuid, string origValue)
        {
            // Alle Connections ermitteln...
            // var connections = DataConnections.Where(x => x.)

            return null;
        }


    }
}