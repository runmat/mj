using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using CkgDomainLogic.DomainCommon.Models;
using DocumentTools.Services;
using GeneralTools.Models;

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

        public void Init(string sourceFile, bool firstRowIsCaption, char delimiter, string destinationFileXml, List<DataConnection> dataConnections, List<Processor> processors)
        {
            // Convert Excel to csv if needed...
            SourceFile.FilenameOrig = sourceFile;
            DestinationFile.Filename = destinationFileXml;
            DataConnections = dataConnections;
            Processors = Processors;

            ReadSourceFile();
            ReadDestinationObj();
        }

        public List<DataConnection> GetConnections()
        {
            return DataConnections;
        }

        public void ReadDestinationObj()
        {
            var filename = Path.GetFileName(DestinationFile.Filename);

            var doc = new XmlDocument();
            doc.Load(DestinationFile.Filename);
            DestinationFile.XmlRaw = doc.InnerXml;
            DestinationFile.XmlDocument = doc;
            //DestinationFile.Fields = new List<FieldXml>();
            DestinationFile.Fields = new List<Field>();

            doc.IterateThroughAllNodes(delegate(XmlNode node)
            {
                try
                {
                    var nodeId = node.Attributes["id"].Value;

                    // var newField = new FieldXml
                    var newField = new Field
                    {
                        Guid = "Dest-" + nodeId,
                        Records = new List<string>()
                    };

                    // newField.Records.Add("test");

                    DestinationFile.Fields.Add(newField);
                }
                catch (Exception)
                {
                }
            });

            // return destinationFileObj;
        }

        /// <summary>
        /// Gibt SourceFile-Objekt zurück
        /// </summary>
        /// <returns></returns>
        public void ReadSourceFile()
        {
            var csvObj = CsvReaderFactory.GetCsvObj(SourceFile.FilenameCsv, SourceFile.FirstRowIsCaption, SourceFile.Delimiter);

            var fieldCount = csvObj.FieldCount;
            var headers = csvObj.GetFieldHeaders();
            var fields = new List<Field>();
            
            for (var i = 0; i < headers.Length; i++)
            {
                if (!SourceFile.FirstRowIsCaption)  // Falls keine Überschriften, Spaltennamen selbst erstellen..
                    headers[i] = "Spalte" + i ;

                var newField = new Field
                {
                    Caption = headers[i],
                    FieldType = FieldType.String,  // DefaultType
                    IsUsed = true
                };

                fields.Add(newField);
            }

            // Daten jeder Column zuordnen...
            while (csvObj.ReadNextRecord())
            {
                for (var j = 0; j < fieldCount; j++)
                {
                    var value = csvObj[j];
                    fields[j].Records.Add(value);
                }
            }

            SourceFile.Fields = fields;
        }

        //protected DataItem.DataType GetDataType(IEnumerable<string> values)
        //{
        //    var dataType = DataItem.DataType.String;

        //    return dataType;
        //}

        private static XmlDocument StringToXmlDoc(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            return doc;
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
        
        public string ConvertToCsvIfNeeded(string filenameOrigFull, string filenameCsvFull, char delimeter = ';')
        {
            SourceFile.FilenameOrig = filenameOrigFull;
            SourceFile.FilenameCsv = filenameCsvFull;


            var extension = Path.GetExtension(filenameOrigFull).ToLower();
            if (extension == ".xls" || extension == ".xlsx")
            {
                SpireXlsFactory.ConvertExcelToCsv(filenameOrigFull, filenameCsvFull, delimeter);
                File.Delete(filenameOrigFull);
            }

            return filenameCsvFull;
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

                var test1 = xmlRecord.Root.Element("IMPORT");
                var test2 = xmlRecord.Root;
                var test3 = test2.ElementsBeforeSelf();
                var test4 = test2.ElementsAfterSelf();

                xmlComplete.Root.Add(test3.Elements());

                xmlComplete.Add(xmlRecord);
                
            }

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
                        if (processor != null)
                        {
                            value = processor.Output;
                            field.IsUsed = true;
                        }
                    }
                    else
                    {
                        value = sourceItem.Records[RecordNo - 1];
                        field.IsUsed = true;    
                    }
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