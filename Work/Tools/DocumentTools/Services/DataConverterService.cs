using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using GeneralTools.Models;

namespace DocumentTools.Services
{
    public class DataConverterService
    {
        public DataConverterMapping DataMapping { get; private set; }

        public SourceFile SourceFile { get; set; }                  // Enthält Fields und dazugehörige Value-Listen
        public DestinationFile DestinationFile { get; set; }

        public string XmlOutput { get; set; }

        public int RecordNo { get; set; }
        public int RecordCount
        {
            get { return SourceFile.RowCount; }
        }
        public string RecordInfoText
        {
            get { return string.Format("{0}/{1}", RecordNo, RecordCount); } 
        }

        public DataConverterService()
        {
            DataMapping = new DataConverterMapping();

            RecordNo = 1;
            
            SourceFile = new SourceFile();
            DestinationFile = new DestinationFile();
        }

        public void Init(string sourceFile, bool firstRowIsCaption, char delimiter, string destinationFileXml, List<DataConnection> dataConnections, List<Processor> processors)
        {
            // Convert Excel to csv if needed...
            SourceFile.FilenameOrig = sourceFile;
            DestinationFile.Filename = destinationFileXml;
            DataMapping.DataConnections = dataConnections;
            DataMapping.Processors = processors;

            ReadSourceFile();
            ReadDestinationObj();
        }

        public List<DataConnection> GetConnections()
        {
            return DataMapping.DataConnections;
        }

        public void ReadDestinationObj()
        {
            var doc = new XmlDocument();
            doc.Load(DestinationFile.Filename);
            DestinationFile.XmlRaw = doc.InnerXml;
            DestinationFile.XmlDocument = doc;
            DestinationFile.Fields = new List<Field>();

            doc.IterateThroughAllNodes(delegate(XmlNode node)
            {
                try
                {
                    var nodeId = node.Attributes["id"].Value;

                    var newField = new Field
                    {
                        Guid = "Dest-" + nodeId,
                        Records = new List<string>()
                    };

                    DestinationFile.Fields.Add(newField);
                }
                catch (Exception)
                {
                }
            });
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

        public void AddProcessor()
        {
            var newProcessorNumber = DataMapping.Processors.Count + 1;
            DataMapping.Processors.Add(new Processor { Number = newProcessorNumber, Title = string.Format("Processor {0}", newProcessorNumber) });       
        }

        //public string RemoveProcessor(string processorId)
        //{
        //    var processor = Processors.FirstOrDefault(x => x.Guid == processorId);
        //    Processors.Remove(processor);
        //    return processorId;
        //}

        public Processor GetProcessorResult(Processor processor)
        {
            // Alle eingehenden Connections ermitteln...
            var connectionsIn = DataMapping.DataConnections.Where(x => x.GuidDest == "prozin-" + processor.Guid).ToList();

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

            var extension = Path.GetExtension(filenameOrigFull).ToLowerAndNotEmpty();
            if (extension == ".xls" || extension == ".xlsx")
            {
                SpireXlsFactory.ConvertExcelToCsv(filenameOrigFull, filenameCsvFull, delimeter);
                File.Delete(filenameOrigFull);
            }

            return filenameCsvFull;
        }

        /// <summary>
        /// Speichert ein XML-Dokument mit allen vorhandenen Datensätzen in einer Datei, falls xmlOutputFilename gesetzt.
        /// </summary>
        /// <param name="xmlOutputFilename"></param>
        /// <returns>XML-Dokument als String</returns>
        public string ExportToXml(string xmlOutputFilename = null)
        {
            var xmlComplete = new XDocument();
            xmlComplete.Add(new XElement("IMPORTS"));

            // In SourceFiele.Fields[] sind alle Felder der Quelldatei und jeweils alle Original-Datensätze
            var records = SourceFile.Fields[0].Records;

            // Alle Datensätze durchlaufen
            for (var recordNo = 1; recordNo < records.Count + 1; recordNo++)
            {
                // XML-Document für einen Datensatz zum späteren Füllen holen
                var xmlSingleRecord = (XmlDocument)DestinationFile.XmlDocument.Clone();
                var xmlRecord = XDocument.Parse(xmlSingleRecord.InnerXml);

                RecordNo = recordNo;

                // Alle Ergebnisse des aktuellen Datensatzes ermitteln und nur Zielfelder zurückgeben, die nicht leer sind
                RecalcProcessors();
                var destFields = RecalcDestFields().Where(x => !string.IsNullOrEmpty(x.Text));

                foreach (var field in destFields)
                {
                    var fieldId = field.Key;
                    var value = field.Text;             // Wert des Feldes ermitteln...

                    // In XML-Dokument das passende Feld suchen
                    var fieldIdToFind = fieldId.Substring(5);   // ***refactor me*** Im Model wird der OriginalId (gem. XML-Datei) immer "Dest-" hinzugefügt. Damit Eintrag gefunden werden kann, hier entfernen.
                    var element = xmlRecord.Descendants().FirstOrDefault(x => (string)x.Attribute("id") == fieldIdToFind);
                    if (element != null) {
                        element.Value = value;
                    }
                }
                
                var contentToAdd = xmlRecord.DescendantNodes().FirstOrDefault();
                var firstDescendant = xmlComplete.Descendants().FirstOrDefault();
                if (firstDescendant != null)
                    firstDescendant.Add(contentToAdd);
            }

            xmlComplete.Save(@"C:\tmp\TestOutputComplete.xml");
            if (xmlOutputFilename != null)
            {
                xmlComplete.Save(xmlOutputFilename);
            }
            
            return xmlComplete.ToString();
        }

        // Alle DatenRecords der Quellfelder (z.B. zur "Live-Anzeige" im UI) ermitteln...
        public List<SelectItem> RecalcSourceFields()
        {
            var sourceFieldList = new List<SelectItem>();
            foreach (var field in SourceFile.Fields)
            {
                sourceFieldList.Add(new SelectItem(field.Guid, field.Records[RecordNo - 1]));
            }
            return sourceFieldList;
        }

        public List<Processor> RecalcProcessors()
        {
            // Alle Prozessoren zur späteren Ausgabe aktualisieren...
            var processorList = new List<Processor>();
            foreach (var processor in DataMapping.Processors)
            {
                var processorResult = GetProcessorResult(processor);
                processorList.Add(processorResult);
            }

            return processorList;
        }

        public List<SelectItem> RecalcDestFields()
        {
            var destFieldList = new List<SelectItem>();
            foreach (var field in DestinationFile.Fields)
            {
                var value = "";
                field.IsUsed = false;

                // Prüfen, ob Connection vorliegt...
                var connection = DataMapping.DataConnections.FirstOrDefault(x => x.GuidDest == field.Guid);
                if (connection != null)
                {
                    var sourceItem = SourceFile.Fields.FirstOrDefault(x => x.Guid == connection.GuidSource);    // Wenn Sourcefeld kein Prozessor ist
                    if (sourceItem == null)
                    {
                        var processor = DataMapping.Processors.FirstOrDefault(x => x.Guid == connection.GuidSource.Replace("prozout-", ""));
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

                destFieldList.Add(new SelectItem(field.Guid, value));
            }
            return destFieldList;
        }
    }
}
