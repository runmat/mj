using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.DataKonverter.Contracts;
using DocumentTools.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using ServicesMvc.Areas.DataKonverter.Models;

namespace CkgDomainLogic.DataKonverter.ViewModels
{

    public class KroschkeDataKonverterViewModel : CkgBaseViewModel
    {
        [XmlIgnore, ScriptIgnore]
        public IDataKonverterDataService DataKonverterDataService { get { return CacheGet<IDataKonverterDataService>(); } }

        public DataMapper DataMapper { get; set; }

        public GlobalViewData GlobalViewData;   // Model für Nutzung in allen Partials

        #region Wizard
        [XmlIgnore]
        public IDictionary<string, string> Steps
        {
            get
            {
                return PropertyCacheGet(() => new Dictionary<string, string>
                {
                    { "Prozessauswahl", "Prozessauswahl" },         // Localize.Vehicle
                    { "Admin/Konfiguration", "Konfiguration" },
                    //{ "Admin/Testimport", "Testimport" },
                    { "Admin/Abschluss", Localize.Ready + "!" },
                });
            }
        }

        public string[] StepKeys { get { return PropertyCacheGet(() => Steps.Select(s => s.Key).ToArray()); } }

        public string[] StepFriendlyNames { get { return PropertyCacheGet(() => Steps.Select(s => s.Value).ToArray()); } }

        public string FirstStepPartialViewName
        {
            get { return string.Format("{0}", StepKeys[1]); }
        }

        #endregion

        public void DataInit()
        {
            #region Globale Properties, nutzbar in allen Partials

            DataMapper = new DataMapper();

            GlobalViewData = new GlobalViewData
            {
                
            };

            //var fileNameFull = GetUploadPathTemp() + @"\KroschkeOn2.xml";
            // DataMapper.DestinationFile = DataMapper.ReadDestinationObj(fileNameFull);
            // DataMapper.DestinationFile = DataMapper.ReadDestinationObj();
            Prozessauswahl = new WizardProzessauswahl();

            #endregion
        }

        #region Wizard-ViewModels

        public WizardProzessauswahl Prozessauswahl { get; set; }
        public WizardKonfiguration Konfiguration { get; set; }

        public class WizardProzessauswahl
        {
            public SourceFile SourceFile { get; set; }

            [LocalizedDisplay("Prozess")]                   // LocalizeConstants.Customer
            public string SelectedProcess { get; set; }

            [SelectListText]            
            public List<string> ProcessList
            {
                get { return new List<string>
                        {
                            "Zulassung",
                            "Abmeldung",
                            "Test"
                        };
                }
            }
        }

        public class WizardKonfiguration
        {
            [LocalizedDisplay("Datumsformat")] 
            public string GlobalDateTransformation { get; set; }
        }

        #endregion

        #region File converter

        //public string ConvertExcelToCsv(string excelFilename, string csvFilename, char delimeter = ';')
        //{
        //    var tempFolder = GetUploadPathTemp();
        //    var tmpSourceFile = Path.Combine(tempFolder, excelFilename);
        //    var tmpDestFile = Path.Combine(tempFolder, csvFilename);

        //    tmpDestFile = SpireXlsFactory.ConvertExcelToCsv(tmpSourceFile, tmpDestFile, delimeter);            
            
        //    return tmpDestFile;
        //}

        public string GetUploadPathTemp()
        {
            return HttpContext.Current.Server.MapPath(string.Format(@"{0}", AppSettings.UploadFilePathTemp));
        }

        #endregion

        #region XML/XSD-Handling

        #endregion

        public string SetProcessorSettings(string processorId, Operation processorType, string processorPara1, string processorPara2)
        {
            var processor = DataMapper.Processors.FirstOrDefault(x => x.Guid == processorId);
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
            var extension = Path.GetExtension(fileName).ToLower();

            var randomfilename = Guid.NewGuid().ToString();

            var nameSaved = fileSaveAction(GetUploadPathTemp(), randomfilename, extension);

            if (string.IsNullOrEmpty(nameSaved))
                return false;

            var tmpFilenameOrig = GetUploadPathTemp() + @"\" + nameSaved + extension;
            var tmpFilenameCsv = GetUploadPathTemp() + @"\" + nameSaved + ".csv";

            DataMapper.ConvertToCsvIfNeeded(tmpFilenameOrig, tmpFilenameCsv);

            DataMapper.SourceFile.FilenameOrig = fileName;
            DataMapper.SourceFile.FilenameCsv = tmpFilenameCsv;

            return true;
        }
        #endregion

        #region ConnectionHandling


        #endregion

        #region XML-Output

        public string GetDestinationDiv(XmlDocument destXmlDocument)
        {
            var content = "";
            var test1 = destXmlDocument.ChildNodes;
            var test2 = destXmlDocument.ChildNodes[1].ChildNodes;
            var test3 = destXmlDocument.FirstChild;
            var test4 = destXmlDocument.ChildNodes;

            // TraverseNodes(doc.ChildNodes, ref content);
            var sb = new StringBuilder();
            TraverseNodes(test2, ref sb);

            return sb.ToString();
        }
        // private static void TraverseNodes(XmlNodeList nodes, ref string content)
        private static void TraverseNodes(XmlNodeList nodes, ref StringBuilder sb)
        {
            // var sb = new StringBuilder();
            foreach (XmlNode node in nodes)
            {
                if (node.NodeType == XmlNodeType.XmlDeclaration)
                {
                    continue;
                }

                string id = null;

                if (node.Attributes != null)
                {
                    if (node.Attributes["id"] != null)
                        id = node.Attributes["id"].Value;
                }

                if (node.HasChildNodes)
                {
                    // sb.Append("<div class='nodetitle'>" + node.ParentNode.Name + " > " + node.FirstChild.Name + "</div>");
                    sb.Append("<div class='nodetitle'>" + node.Name + "</div>");
                }
                else
                {
                    sb.Append("<div class='w ept' id='Dest-" + id + "'>");  
                    sb.Append("<div class='ep'></div>");
                    sb.Append("<div class='field'>" + node.Name + "</div>");
                    sb.Append("<span class='data'></span></div>");
                }

                // content += sb.ToString();

                if (node.HasChildNodes)
                {
                    TraverseNodes(node.ChildNodes, ref sb);
                }

                // content += sb.ToString();
            }

            // content += sb.ToString();
        }

        public string CreateXmlContent()
        {
            var xmlContent = DataMapper.ExportToXml();
            return xmlContent;
        }

        #endregion
    }
}