using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.DataConverter.Contracts;
using CkgDomainLogic.DataConverter.Models;
using CkgDomainLogic.General.Database.Models;
using DocumentTools.Services;
using GeneralTools.Models;
using MvcTools.Data;
using Newtonsoft.Json;

namespace CkgDomainLogic.DataConverter.ViewModels
{
    [SuppressMessage("ReSharper", "ConvertClosureToMethodGroup")]
    public class DataConverterViewModel : CkgBaseViewModel
    {
        [XmlIgnore, ScriptIgnore]
        public IDataConverterDataService DataConverterDataService { get { return CacheGet<IDataConverterDataService>(); } }

        [XmlIgnore]
        public List<Customer> Kunden
        {
            get
            {
                return PropertyCacheGet(() => DataConverterDataService.GetCustomers()
                    .InsertAtTop(new Customer { CustomerID = 0, Customername = Localize.DropdownDefaultOptionPleaseChoose }).OrderBy(k => k.Customername).ToListOrEmptyList());
            }
        }

        [XmlIgnore]
        public List<string> Prozesse
        {
            get
            {
                return PropertyCacheGet(() => DataConverterDataService.GetProcessStructureNames()
                    .InsertAtTop("").OrderBy(p => p).ToListOrEmptyList());
            }
        }

        public DataMappingSelektor MappingSelektor
        {
            get { return PropertyCacheGet(() => new DataMappingSelektor { CustomerId = (CanUserSelectCustomer ? 0 : LogonContext.User.CustomerID) }); }
            set { PropertyCacheSet(value); }
        }

        public NewDataMappingSelektor NewMappingSelektor
        {
            get { return PropertyCacheGet(() => new NewDataMappingSelektor { CustomerId = (CanUserSelectCustomer ? 0 : LogonContext.User.CustomerID) }); }
            set { PropertyCacheSet(value); }
        }

        public MappedUploadMappingSelectionModel MappingSelectionModel
        {
            get { return PropertyCacheGet(() => new MappedUploadMappingSelectionModel { MappingId = (DataMappingsForCustomer.Count == 1 ? DataMappingsForCustomer.First().Id : 0) }); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<DataConverterMappingInfo> DataMappings
        {
            get { return PropertyCacheGet(() => new List<DataConverterMappingInfo>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<DataConverterMappingInfo> DataMappingsFiltered
        {
            get { return PropertyCacheGet(() => DataMappings); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<DataConverterMappingInfo> DataMappingsForCustomer { get { return PropertyCacheGet(() => LoadDataMappingsForCustomer()); } }

        public DataMappingModel MappingModel { get; set; }

        public int MappingId { get; set; }

        public int MappingCustomerId { get; set; }

        public string MappingName { get; set; }

        public bool CanUserSelectCustomer { get { return LogonContext.HighestAdminLevel > AdminLevel.Customer; } }

        public void DataConverterInit()
        {
            MappingModel = new DataMappingModel();

            DataMarkForRefresh(true);
        }

        private void DataMarkForRefresh(bool refreshStammdaten = false)
        {
            PropertyCacheClear(this, m => m.DataMappingsFiltered);
            PropertyCacheClear(this, m => m.DataMappingsForCustomer);

            if (refreshStammdaten)
            {
                PropertyCacheClear(this, m => m.Kunden);
                PropertyCacheClear(this, m => m.Prozesse);
            }
        }

        public void ClearNewMappingSelektor()
        {
            if (CanUserSelectCustomer)
                NewMappingSelektor.CustomerId = 0;

            NewMappingSelektor.ProzessName = null;
            NewMappingSelektor.MappingName = null;
        }

        public void LoadDataMappings(ModelStateDictionary state)
        {
            DataMappings = DataConverterDataService.GetDataMappingInfos(MappingSelektor);

            DataMarkForRefresh();
        }

        public List<DataConverterMappingInfo> LoadDataMappingsForCustomer()
        {
            return DataConverterDataService.GetDataMappingInfos(new DataMappingSelektor { CustomerId = LogonContext.User.CustomerID, ProzessName = "" });
        }

        public void FilterDataMappings(string filterValue, string filterProperties)
        {
            DataMappingsFiltered = DataMappings.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public void InitDataMapper(int mappingId)
        {
            MappingSelektor = new DataMappingSelektor { CustomerId = LogonContext.User.CustomerID };

            LoadDataMappings(new ModelStateDictionary());

            InitMapping(mappingId);
        }

        public void InitMapping(int mappingId)
        {
            MappingId = mappingId;

            if (mappingId == 0)
            {
                ReadDestinationStructureFromJson(DataConverterDataService.GetProcessStructure(NewMappingSelektor.ProzessName));
                ReadSourceFile();
                MappingCustomerId = NewMappingSelektor.CustomerId;
                MappingName = NewMappingSelektor.MappingName;
            }
            else
            {
                var selectedMapping = DataMappings.FirstOrDefault(m => m.Id == mappingId);

                var mappingData = (selectedMapping != null ? DataConverterDataService.GetDataMapping(selectedMapping.Id) : null);
                if (mappingData != null)
                {
                    MappingId = mappingData.Id;
                    MappingCustomerId = mappingData.CustomerId;
                    MappingName = mappingData.Title;
                    MappingModel = JsonConvert.DeserializeObject<DataMappingModel>(mappingData.Mapping);
                }
            }
        }

        public string GetUploadPathTemp()
        {
            return HttpContext.Current.Server.MapPath(string.Format(@"{0}", AppSettings.UploadFilePathTemp));
        }

        public string SetProcessorSettings(string processorId, Operation processorType, string processorPara1, string processorPara2)
        {
            var processor = MappingModel.Processors.FirstOrDefault(x => x.Guid == processorId);
            if (processor == null)
                return null;

            try
            {
                processor.Operation = processorType;
                processor.OperationPara1 = processorPara1;
                processor.OperationPara2 = processorPara2;

                return null;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        #region Upload source file

        public bool UploadFileSave(string fileName, Func<string, string, string, string> fileSaveAction)
        {
            var extension = Path.GetExtension(fileName).ToLowerAndNotEmpty();

            var randomfilename = Guid.NewGuid().ToString();

            var nameSaved = fileSaveAction(GetUploadPathTemp(), randomfilename, extension);

            if (string.IsNullOrEmpty(nameSaved))
                return false;

            var tmpFilenameOrig = GetUploadPathTemp() + @"\" + nameSaved + extension;
            var tmpFilenameCsv = GetUploadPathTemp() + @"\" + nameSaved + ".csv";

            ConvertToCsvIfNeeded(tmpFilenameOrig, tmpFilenameCsv);

            MappingModel.SourceFile.FilenameOrig = fileName;
            MappingModel.SourceFile.FilenameCsv = tmpFilenameCsv;

            return true;
        }

        #endregion

        #region Visual Output

        public string GetDestinationDiv()
        {
            var sb = new StringBuilder();
            
            var lastGroup = "";
            var groupStillOpen = false;

            foreach (var item in MappingModel.DestinationStructure.Fields)
            {
                if (string.IsNullOrEmpty(item.Feldgruppe))
                {
                    if (groupStillOpen)
                        AddFieldgroupEndHtml(ref sb);

                    AddFieldHtml(item, ref sb);
                }
                else if (item.Feldgruppe == lastGroup)
                {
                    AddFieldHtml(item, ref sb);
                }
                else
                {
                    if (groupStillOpen)
                        AddFieldgroupEndHtml(ref sb);

                    AddFieldgroupStartHtml(item.Feldgruppe, ref sb);
                    AddFieldHtml(item, ref sb);

                    lastGroup = item.Feldgruppe;
                    groupStillOpen = true;
                }
            }

            if (groupStillOpen)
                AddFieldgroupEndHtml(ref sb);

            return sb.ToString();
        }

        private static void AddFieldHtml(Field item, ref StringBuilder sb)
        {
            sb.Append("<div class='w ept' id='" + item.Id + "'>");
            sb.Append("<div class='ep'></div>");
            sb.Append("<div class='field'>" + item.Bezeichnung + "</div>");
            sb.Append("<span class='data'></span></div>");
        }

        private static void AddFieldgroupStartHtml(string fieldgroupName, ref StringBuilder sb)
        {
            sb.Append("<div class='nodetitle-div'>");
            sb.Append("<div class='nodetitle'>" + fieldgroupName + "<button class='btn btn-xs white float-right' onclick='ShowHideLines(this);'><i class='updown icon-angle-up'></i></button></div>");
        }

        private static void AddFieldgroupEndHtml(ref StringBuilder sb)
        {
            sb.Append("</div>");
        }

        public void AddProcessor()
        {
            var newProcessorNumber = MappingModel.Processors.Count + 1;
            MappingModel.Processors.Add(new Processor { Number = newProcessorNumber, Title = string.Format("Processor {0}", newProcessorNumber) });
        }

        public Processor GetProcessorResult(Processor processor)
        {
            // Alle eingehenden Connections ermitteln...
            var connectionsIn = MappingModel.DataConnections.Where(x => x.GuidDest == "prozin-" + processor.Guid).ToList();

            // Input-String ermitteln...
            var input = new StringBuilder();
            foreach (var dataConnection in connectionsIn)
            {
                var sourceField = MappingModel.SourceFile.Fields.FirstOrDefault(x => x.Id == dataConnection.GuidSource);
                if (sourceField != null)
                {
                    var value = sourceField.Records[MappingModel.RecordNo - 1];
                    input.Append(value);
                    input.Append("|");
                }
            }
            if (input.Length > 0)
                input = input.Remove(input.Length - 1, 1);

            processor.Input = input.ToString();

            return processor;
        }

        public List<SelectItem> RecalcSourceFields()
        {
            var sourceFieldList = new List<SelectItem>();
            foreach (var field in MappingModel.SourceFile.Fields)
            {
                sourceFieldList.Add(new SelectItem(field.Id, field.Records[MappingModel.RecordNo - 1]));
            }
            return sourceFieldList;
        }

        public List<SelectItem> RecalcDestFields()
        {
            var destFieldList = new List<SelectItem>();
            foreach (var field in MappingModel.DestinationStructure.Fields)
            {
                var value = "";

                // Prüfen, ob Connection vorliegt...
                var connection = MappingModel.DataConnections.FirstOrDefault(x => x.GuidDest == field.Id);
                if (connection != null)
                {
                    var sourceItem = MappingModel.SourceFile.Fields.FirstOrDefault(x => x.Id == connection.GuidSource);    // Wenn Sourcefeld kein Prozessor ist
                    if (sourceItem == null)
                    {
                        var processor = MappingModel.Processors.FirstOrDefault(x => x.Guid == connection.GuidSource.Replace("prozout-", ""));
                        if (processor != null)
                        {
                            value = processor.Output;
                        }
                    }
                    else
                    {
                        value = sourceItem.Records[MappingModel.RecordNo - 1];
                    }
                }

                destFieldList.Add(new SelectItem(field.Id, value));
            }

            return destFieldList;
        }

        public List<Processor> RecalcProcessors()
        {
            // Alle Prozessoren zur späteren Ausgabe aktualisieren...
            var processorList = new List<Processor>();
            foreach (var processor in MappingModel.Processors)
            {
                var processorResult = GetProcessorResult(processor);
                processorList.Add(processorResult);
            }

            return processorList;
        }

        #endregion

        public void ReadDestinationStructureFromJson(DataConverterProcessStructure destStructure)
        {
            MappingModel.DestinationStructure.StructureName = destStructure.ProcessName;
            MappingModel.DestinationStructure.Fields.Clear();

            var structureItems = JsonConvert.DeserializeObject<DataConverterStructure>(destStructure.DestinationStructure);

            foreach (var gruppe in structureItems.Keys)
            {
                foreach (var feld in structureItems[gruppe])
                {
                    MappingModel.DestinationStructure.Fields.Add(new Field(feld, gruppe));
                }
            }
        }

        private void ReadSourceFile()
        {
            if (string.IsNullOrEmpty(MappingModel.SourceFile.FilenameCsv))
            {
                MappingModel.SourceFile.Fields = new List<Field>();
                return;
            }

            var csvObj = CsvReaderFactory.GetCsvObj(MappingModel.SourceFile.FilenameCsv, MappingModel.SourceFile.FirstRowIsCaption, MappingModel.SourceFile.Delimiter);

            var fieldCount = csvObj.FieldCount;
            var headers = csvObj.GetFieldHeaders();
            var fields = new List<Field>();

            for (var i = 0; i < headers.Length; i++)
            {
                if (!MappingModel.SourceFile.FirstRowIsCaption)  // Falls keine Überschriften, Spaltennamen selbst erstellen..
                    headers[i] = "Spalte" + i;

                fields.Add(new Field(headers[i]));
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

            MappingModel.SourceFile.Fields = fields;
            MappingModel.RecordNo = 0;
        }

        public bool SaveMapping()
        {
            bool success;

            // in SQL nur die erste Datenzeile mitspeichern
            MappingModel.SourceFile.Fields.ForEach(f =>
            {
                if (f.Records.Any())
                    f.Records = new List<string> { f.Records[0] };
            });

            var SaveObject = new DataConverterMappingData
            {
                Id = MappingId,
                CustomerId = MappingCustomerId,
                Title = MappingName,
                Process = MappingModel.DestinationStructure.StructureName,
                Mapping = JsonConvert.SerializeObject(MappingModel)
            };

            success = DataConverterDataService.SaveDataMapping(SaveObject);

            if (MappingId == 0)
            {
                if (success)
                {
                    var kunde = Kunden.FirstOrDefault(k => k.CustomerID == SaveObject.CustomerId);

                    DataMappings.Add(new DataConverterMappingInfo
                    {
                        Id = SaveObject.Id,
                        Title = SaveObject.Title,
                        CustomerId = SaveObject.CustomerId,
                        Customername = (kunde != null ? kunde.Customername : ""),
                        KUNNR = (kunde != null ? kunde.KUNNR : ""),
                        Process = SaveObject.Process
                    });

                    DataMarkForRefresh();
                }
                else
                {
                    ReadSourceFile();
                }
            }

            return success;
        }

        public void DeleteMapping(int mappingId)
        {
            var success = DataConverterDataService.DeleteDataMapping(mappingId);

            if (success)
            {
                DataMappings.RemoveAll(m => m.Id == mappingId);
                DataMarkForRefresh();
            }
        }

        /// <summary>
        /// XML-Struktur mit allen vorhandenen Datensätzen generieren, falls xmlOutputFilename gesetzt -> in XML-Datei speichern.
        /// </summary>
        /// <param name="xmlOutputFilename"></param>
        /// <returns>XML-Struktur</returns>
        public XElement GenerateXmlResultStructure(string xmlOutputFilename = null)
        {
            var xmlResult = new XElement("EXPORTS");

            for (var recordNo = 1; recordNo <= MappingModel.RecordCount; recordNo++)
            {
                var xmlElement = new XElement("EXPORT");

                MappingModel.RecordNo = recordNo;

                // Alle Ergebnisse des aktuellen Datensatzes ermitteln und nur Zielfelder zurückgeben, die nicht leer sind
                RecalcProcessors();
                var destFields = RecalcDestFields().Where(x => !string.IsNullOrEmpty(x.Text));

                foreach (var field in MappingModel.DestinationStructure.Fields)
                {
                    var destField = destFields.FirstOrDefault(f => f.Key == field.Id);
                    if (destField != null)
                        xmlElement.Add(new XElement(field.Bezeichnung, destField.Text));
                    else
                        xmlElement.Add(new XElement(field.Bezeichnung));
                }

                xmlResult.Add(xmlElement);
            }

            if (xmlOutputFilename != null)
            {
                var xmlDoc = new XDocument(xmlResult);
                xmlDoc.Save(xmlOutputFilename);
            }

            return xmlResult;
        }

        /// <summary>
        /// Liste von dynamischen Objekten mit allen vorhandenen Datensätzen generieren
        /// </summary>
        /// <returns></returns>
        public List<dynamic> GenerateResultStructure()
        {
            var liste = new List<dynamic>();

            for (var recordNo = 1; recordNo <= MappingModel.RecordCount; recordNo++)
            {
                var item = new ExpandoObject();

                MappingModel.RecordNo = recordNo;

                RecalcProcessors();
                var destFields = RecalcDestFields().Where(x => !string.IsNullOrEmpty(x.Text));

                foreach (var field in MappingModel.DestinationStructure.Fields)
                {
                    var destField = destFields.FirstOrDefault(f => f.Key == field.Id);

                    ((IDictionary<string, object>) item).Add(field.Bezeichnung, (destField != null ? destField.Text : null));
                }

                liste.Add(item);
            }

            return liste;
        }

        public string ConvertToCsvIfNeeded(string filenameOrigFull, string filenameCsvFull, char delimeter = ';')
        {
            MappingModel.SourceFile.FilenameOrig = filenameOrigFull;
            MappingModel.SourceFile.FilenameCsv = filenameCsvFull;

            var extension = Path.GetExtension(filenameOrigFull).ToLowerAndNotEmpty();
            if (extension == ".xls" || extension == ".xlsx")
            {
                SpireXlsFactory.ConvertExcelToCsv(filenameOrigFull, filenameCsvFull, delimeter);
                File.Delete(filenameOrigFull);
            }

            return filenameCsvFull;
        }

        /// <summary>
        /// Mapping der Daten in eine Liste von anonymen Objekten (vorgelagertes DataInit und InitDataMapper erforderlich!)
        /// </summary>
        /// <param name="inputData">Liste von Objekten, deren Properties in der Reihenfolge sein müssen wie in der Upload-Exceldatei</param>
        /// <param name="state"></param>
        /// <returns>Liste der Ergebnisobjekte</returns>
        public List<object> MapData(IEnumerable<dynamic> inputData, ModelStateDictionary state = null)
        {
            SetSourceFileData(inputData, state);

            return GenerateResultStructure();
        }

        /// <summary>
        /// Mapping der Daten in eine Liste von Objekten des angegebenen Typs T (vorgelagertes DataInit und InitDataMapper erforderlich!)
        /// </summary>
        /// <param name="inputData">Liste von Objekten, deren Properties in der Reihenfolge sein müssen wie in der Upload-Exceldatei</param>
        /// <param name="state"></param>
        /// <returns>Liste der Ergebnisobjekte vom Typ T</returns>
        public List<T> MapData<T>(IEnumerable<dynamic> inputData, ModelStateDictionary state = null)
            where T : class, new()
        {
            SetSourceFileData(inputData, state);

            return DynamicObjectConverter.MapDynamicObjectListToDestinationObjectList<T>(GenerateResultStructure());
        }

        private void SetSourceFileData(IEnumerable<dynamic> inputData, ModelStateDictionary state = null)
        {
            MappingModel.SourceFile.Fields.ForEach(f => f.Records.Clear());

            if (inputData.Any())
            {
                var propertyNames = ((IDictionary<string, object>)inputData.First()).Keys.ToList();

                if (propertyNames.Count < MappingModel.SourceFile.Fields.Count)
                {
                    if (state != null)
                        state.AddModelError(string.Empty, string.Format("{0} ({1})", Localize.MappingFailed, Localize.InsufficientNumberOfImportFields));

                    return;
                }

                foreach (var item in inputData)
                {
                    for (var i = 0; i < MappingModel.SourceFile.Fields.Count; i++)
                    {
                        MappingModel.SourceFile.Fields[i].Records.Add(((IDictionary<string, object>)item)[propertyNames[i]].ToString().NotNullOrEmpty());
                    }
                }
            }
        }
    }
}
