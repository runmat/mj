using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using CkgDomainLogic.DomainCommon.Models;
using GeneralTools.Models;
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
            get { return String.Format("{0}/{1}", RecordNo, RecordCount); } 
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

            return newConnection;
        }

        public string RemoveConnection(string idSource, string idDest)
        {
            idDest = idDest.Replace("prozin-","");
            var connection = DataConnections.FirstOrDefault(x => x.GuidSource == idSource && x.GuidDest == idDest);
            if (connection == null)
                return null;

            DataConnections.Remove(connection);            
            return null;
        }

        public List<DataConnection> GetConnections()
        {
            return DataConnections;
        }

        public Processor AddProcessor()
        {
            var newProcessor = new Processor();
            Processors.Add(newProcessor);
            newProcessor.Number = Processors.Count;
            newProcessor.Title = "Processor " + newProcessor.Number;
            return newProcessor;         
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
            var connectionsIn = DataConnections.Where(x => x.GuidDest == "prozin-" + processor.Guid).ToList();

            // Input-String ermitteln...
            var input = new StringBuilder();

            foreach (var dataConnection in connectionsIn)
            {
                var sourceField = SourceFile.Fields.FirstOrDefault(x => x.Guid == dataConnection.GuidSource);
                if (sourceField != null)
                {
                    var value = sourceField.Records[RecordNo - 1];
                    input.Append(value);
                    input.Append("|");    
                }
            }
            if (input.Length > 0)
                input = input.Remove(input.Length-1,1);

            processor.Input = input.ToString();

            return processor;
        }

        #region TEST
        public static XmlDocument ToXmlDocument(XDocument xdoc)
        {
            var xmlDocument = new XmlDocument();
            using (var reader = xdoc.CreateReader())
            {
                xmlDocument.Load(reader);
            }
            return xmlDocument;
        }
        public static XDocument ToXDocument(XmlDocument xmlDoc)
        {
            using (var reader = new XmlNodeReader(xmlDoc))
            {
                reader.MoveToContent();
                return XDocument.Load(reader);
            }
        }
        #endregion

        public string ExportToXml()
        {
            var doc = new XmlDocument();
            var newElem = doc.CreateNode("element", "pages", "");  

            var xmlFileContent = new XmlDocument();

            var xmlSingleRecord = (XmlDocument)DestinationFile.XmlDocument.Clone();

            var destFields = RecalcDestFields();

            // var xmlComplete = new XDocument();
            // ar xmlComplete = new XmlDocument();
            xmlFileContent.CreateNode("element", "pages", "");

            var xmlComplete = new XDocument();

            // Alle vorhandenen Datensätze aus Sourcefile durchlaufen...
            foreach (var record in SourceFile.Fields[0].Records)
            {
                var xmlRecord = XDocument.Parse(xmlSingleRecord.InnerXml);

                // Alle Felder durchlaufen, die über einen Inhalt verfügen...
                foreach (var field in DestinationFile.Fields.Where(x => x.IsUsed))
                {
                    var id = field.Guid.Replace("Dest-", "");

                    // Element in Template-XmlDokument finden...
                    var found = xmlRecord.Descendants().Attributes("id").Any(attribute => attribute.Value.Contains("Passwort"));
                    if (found)
                    {
                        var element = xmlRecord.Descendants().FirstOrDefault(x => (string)x.Attribute("id") == id);
                        var content = destFields.FirstOrDefault(x => x.Beschreibung == "Dest-" + id);
                        // element.Value = field.Records[0];
                        if (content != null)
                        {
                            element.Value = content.Wert;
                        }
                    }
                }

                //var elementToInsert = xmlRecord.Descendants().Attributes("id").Any(attribute => attribute.Value.Contains("Passwort"));

                //var bigDoc = new XmlDocument();
                //bigDoc.LoadXml("<Data></Data>");
                //var targetNode = bigDoc.FirstChild;

                //XmlDocument doc1 = (XmlDocument)DestinationFile.XmlDocument.Clone();
                //var doc2 = ToXmlDocument(xmlRecord);
                //XmlNode copiedNode = doc2.ImportNode(doc1.SelectSingleNode("/IMPORT"), true);
                //doc2.DocumentElement.AppendChild(copiedNode);

                ////var newElement = xmlFileContent.CreateNode("element", "pages", "");
                ////newElement.InnerText = "100";
                ////var root = xmlFileContent.DocumentElement;
                ////root.AppendChild(newElement);

                //// var newNode = xmlComplete.Add(new object());
                
                //// xdocument habe ich
                //// xmlnode brauche ich für AppendChild

                //var found2 = xmlRecord.Descendants("IMPORT").FirstOrDefault(); // .Attributes("id").Any(attribute => attribute.Value.Contains("Passwort"));

                //xmlComplete.AppendChild(found2.FirstNode);

                var test1 = xmlRecord.Root.Element("IMPORT");
                var test2 = xmlRecord.Root;
                var test3 = test2.ElementsBeforeSelf();
                var test4 = test2.ElementsAfterSelf();
                // var test5 = test2.Element("").Elements();
                // var toAdd = xmlRecord.Root.Element("IMPORT").Elements();
                
                // xmlComplete.Root.Add(xmlRecord.Root.Element("IMPORT").Elements());
                xmlComplete.Root.Add(test3.Elements());

                xmlComplete.Add(xmlRecord);

                // xmlComplete.Add(xmlRecord);
                
            }

            // xmlRecord.Save(@"C:\tmp\TestOutput.xml");
            xmlComplete.Save(@"C:\tmp\TestOutput.xml");

            return "OK";
        }

        // Alle DatenRecords der Quellfelder ermitteln...
        public List<Domaenenfestwert> RecalcSourceFields()
        {
            var sourceFieldList = new List<Domaenenfestwert>();
            foreach (var field in SourceFile.Fields)
            {
                sourceFieldList.Add(new Domaenenfestwert
                {
                    Wert = field.Guid,
                    Beschreibung = field.Records[RecordNo - 1]
                });
            }
            return sourceFieldList;
        }

        public List<Processor> RecalcProcessors()
        {
            // Alle Prozessoren zur späteren Ausgabe aktualisieren...
            var processorList = new List<Processor>();
            foreach (var processor in Processors)
            {
                var processorResult = GetProcessorResult(processor);
                processorList.Add(processorResult);
            }

            return processorList;
        }

        public List<Domaenenfestwert> RecalcDestFields()
        {
            var destFieldList = new List<Domaenenfestwert>();
            foreach (var field in DestinationFile.Fields)
            {
                var value = "";
                field.IsUsed = false;

                // Prüfen, ob Connection vorliegt...
                var connection = DataConnections.FirstOrDefault(x => x.GuidDest == field.Guid);
                if (connection != null)
                {
                    var sourceItem = SourceFile.Fields.FirstOrDefault(x => x.Guid == connection.GuidSource);    // Wenn Sourcefeld kein Prozessor ist
                    if (sourceItem == null)
                    {
                        var processor = Processors.FirstOrDefault(x => x.Guid == connection.GuidSource.Replace("prozout-", ""));
                        value = processor.Output;
                        field.IsUsed = true;
                    }
                    else
                    {
                        value = sourceItem.Records[RecordNo - 1];
                        field.IsUsed = true;    
                    }

                    //if (connection.SourceIsProcessor)
                    //{
                    //    var processor = Processors.FirstOrDefault(x => x.Guid == connection.GuidSource.Replace("prozout-", ""));
                    //    value = processor.Output;
                    //    field.IsUsed = true;
                    //}
                    //else
                    //{
                    //    var sourceItem = SourceFile.Fields.FirstOrDefault(x => x.Guid == connection.GuidSource);
                    //    if (sourceItem != null)
                    //    {
                    //        value = sourceItem.Records[RecordNo - 1];
                    //    }
                    //    field.IsUsed = true;
                    //}
                }

                destFieldList.Add(new Domaenenfestwert
                {
                    Wert = value,
                    Beschreibung = field.Guid
                });
            }
            return destFieldList;
        }
    }
}