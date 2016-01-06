using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.DataConverter.Contracts;
using CkgDomainLogic.DataConverter.Models;
using CkgDomainLogic.General.Database.Models;
using DocumentTools.Services;
using GeneralTools.Models;

namespace CkgDomainLogic.DataConverter.ViewModels
{
    public class DataConverterViewModel : CkgBaseViewModel
    {
        [XmlIgnore, ScriptIgnore]
        public IDataConverterDataService DataService { get { return CacheGet<IDataConverterDataService>(); } }

        [XmlIgnore]
        public List<Customer> Kunden
        {
            get
            {
                return PropertyCacheGet(() => DataService.GetCustomers()
                    .InsertAtTop(new Customer { CustomerID = 0, Customername = Localize.DropdownDefaultOptionPleaseChoose }).ToListOrEmptyList());
            }
        }

        [XmlIgnore]
        public List<string> Prozesse
        {
            get
            {
                return PropertyCacheGet(() => DataService.GetProcessStructureNames()
                    .InsertAtTop("").ToListOrEmptyList());
            }
        }

        public DataMappingSelektor Selektor
        {
            get { return PropertyCacheGet(() => new DataMappingSelektor { CustomerId = (CanUserSelectCustomer ? 0 : LogonContext.Customer.CustomerID) }); }
            set { PropertyCacheSet(value); }
        }

        public NewDataMappingSelektor NewMappingSelektor
        {
            get { return PropertyCacheGet(() => new NewDataMappingSelektor { CustomerId = (CanUserSelectCustomer ? 0 : LogonContext.Customer.CustomerID) }); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<DataConverterDataMapping> DataMappings
        {
            get { return PropertyCacheGet(() => new List<DataConverterDataMapping>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<DataConverterDataMapping> DataMappingsFiltered
        {
            get { return PropertyCacheGet(() => DataMappings); }
            private set { PropertyCacheSet(value); }
        }

        public DataConverterService DataConverter { get; set; }

        public bool CanUserSelectCustomer { get { return LogonContext.HighestAdminLevel > AdminLevel.Customer; } }

        public void DataInit()
        {
            DataConverter = new DataConverterService();

            DataMarkForRefresh(true);
        }

        private void DataMarkForRefresh(bool refreshStammdaten = false)
        {
            PropertyCacheClear(this, m => m.DataMappingsFiltered);

            if (refreshStammdaten)
            {
                PropertyCacheClear(this, m => m.Kunden);
                PropertyCacheClear(this, m => m.Prozesse);
            }
        }

        public void LoadDataMappings(ModelStateDictionary state)
        {
            DataMappings = DataService.GetDataMappings(Selektor);

            DataMarkForRefresh();
        }

        public void FilterDataMappings(string filterValue, string filterProperties)
        {
            DataMappingsFiltered = DataMappings.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public void InitMapping(int mappingId)
        {
            DataConverter.InitXml(@"C:\tmp\KroschkeOn2.xml");  // ###removeme### Prozessdatei bis jetzt noch fest verdrahtet

            //var selectedMapping = DataMappings.FirstOrDefault(m => m.Id == mappingId);
            //DataConverter.InitJson(selectedMapping != null ? selectedMapping.Mapping : null);
        }

        public string GetUploadPathTemp()
        {
            return HttpContext.Current.Server.MapPath(string.Format(@"{0}", AppSettings.UploadFilePathTemp));
        }

        public string SetProcessorSettings(string processorId, Operation processorType, string processorPara1, string processorPara2)
        {
            var processor = DataConverter.DataMapping.Processors.FirstOrDefault(x => x.Guid == processorId);
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

            DataConverter.ConvertToCsvIfNeeded(tmpFilenameOrig, tmpFilenameCsv);

            DataConverter.SourceFile.FilenameOrig = fileName;
            DataConverter.SourceFile.FilenameCsv = tmpFilenameCsv;

            return true;
        }

        #endregion

        #region Visual XML Output

        public string GetDestinationDiv(XmlDocument destXmlDocument)
        {
            var nodeList = destXmlDocument.ChildNodes[1].ChildNodes;
            
            var sb = new StringBuilder();
            TraverseNodes(nodeList, ref sb);

            return sb.ToString();
        }

        private static void TraverseNodes(XmlNodeList nodes, ref StringBuilder sb)
        {
            foreach (XmlNode node in nodes)
            {
                if (node.NodeType == XmlNodeType.XmlDeclaration)
                    continue;

                string id = null;

                if (node.Attributes != null)
                {
                    if (node.Attributes["id"] != null)
                        id = node.Attributes["id"].Value;
                }

                if (node.HasChildNodes)
                {
                    sb.Append("<div class='nodetitle-div'>");
                    sb.Append("<div class='nodetitle'>" + node.Name + "<button class='btn btn-xs white float-right' onclick='ShowHideLines(this);'><i class='updown icon-angle-up'></i></button></div>");

                    TraverseNodes(node.ChildNodes, ref sb);

                    sb.Append("</div>");
                }
                else
                {
                    sb.Append("<div class='w ept' id='Dest-" + id + "'>");
                    sb.Append("<div class='ep'></div>");
                    sb.Append("<div class='field'>" + node.Name + "</div>");
                    sb.Append("<span class='data'></span></div>");
                }
            }
        }

        #endregion

        public string GenerateXmlResultStructure()
        {
            return DataConverter.ExportToXmlString();
        }

        public void SaveMapping(ModelStateDictionary state)
        {
            
        }
    }
}