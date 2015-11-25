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
                    { "Admin/Testimport", "Testimport" },
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
            
            //var csvFilename = ConvertExcelToCsv("Testfile.xlsx", Guid.NewGuid() + "-Testfile.csv");
            //var destFilename = "";
            // DataMapper.SourceFile = DataKonverterDataService.FillSourceFile(csvFilename, true);

            DataMapper.DestinationFile = FillDestinationObj("KroschkeOn2.xml");
            Prozessauswahl = new WizardProzessauswahl();

            #endregion
        }

        #region Wizard-ViewModels

        public WizardProzessauswahl Prozessauswahl { get; set; }

        public class WizardProzessauswahl
        {
            public SourceFile SourceFile { get; set; }

            [LocalizedDisplay("Prozess")]  // LocalizeConstants.Customer
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

        #endregion

        #region File converter

        public string ConvertExcelToCsv(string excelFilename, string csvFilename, char delimeter = ';')
        {
            var tempFolder = GetUploadPathTemp();
            var tmpSourceFile = Path.Combine(tempFolder, excelFilename);
            var tmpDestFile = Path.Combine(tempFolder, csvFilename);

            tmpDestFile = SpireXlsFactory.ConvertExcelToCsv(tmpSourceFile, tmpDestFile, delimeter);            
            // tmpDestFile = @"C:\dev\inetpub\wwwroot\ServicesMvc\App_Data\FileUpload\Temp\Testfile3.csv";

            return tmpDestFile;
        }

        public string GetUploadPathTemp()
        {
            return HttpContext.Current.Server.MapPath(string.Format(@"{0}", AppSettings.UploadFilePathTemp));
        }

        //protected byte[] GetCsvFileContent(string fileName)
        //{
        //    var sb = new StringBuilder();
        //    using (var sr = new StreamReader(fileName, Encoding.Default, true))
        //    {
        //        string line;
        //        // Read and display lines from the file until the end of the file is reached.
        //        while ((line = sr.ReadLine()) != null)
        //        {
        //            sb.AppendLine(line);
        //        }
        //    }
        //    var allines = sb.ToString();
        //    var utf8 = new UTF8Encoding();
        //    var preamble = utf8.GetPreamble();
        //    var data = utf8.GetBytes(allines);
        //    return data;
        //}

        #endregion

        //public string GetSourceFile(string csvFilename)
        //{
        //    var result = DataKonverterDataService.FillSourceFile(csvFilename, true);
        //    return null;
        //}

        #region XML/XSD-Handling

        public DestinationFile FillDestinationObj(string filename)
        {
            var folder = GetUploadPathTemp();
            var filenameFull = Path.Combine(folder, filename);
            string xmlContent;

            using (var sr = new StreamReader(filenameFull))
            {
                xmlContent = sr.ReadToEnd();
            }

            var destinationFileObj = new DestinationFile
            {
                Filename = filename,
                XmlRaw = xmlContent,
                XmlDocument = StringToXmlDoc(xmlContent),
                Fields = new List<Field>()
            };

            var doc = new XmlDocument();
            doc.Load(filenameFull);

            doc.IterateThroughAllNodes( delegate(XmlNode node)
            {
                try
                {
                    var nodeId = node.Attributes["id"].Value;

                    var newField = new Field
                    {
                        Guid = "Dest-" + nodeId,
                        Records = new List<string>()
                    };
                    newField.Records.Add("test");

                    destinationFileObj.Fields.Add(newField);
                }
                catch (Exception)
                {
                }

            });

            return destinationFileObj;
        }

        private XmlDocument StringToXmlDoc(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            return doc;
        }

        private string XsdToJson(string xsd)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xsd);
            var json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        #endregion

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

            var bytes = File.ReadAllBytes(tmpFilenameOrig);
            // Overview.PdfUploaded = bytes;

            if (extension == ".xls" || extension == ".xlsx")
            {
                tmpFilenameCsv = ConvertExcelToCsv(tmpFilenameOrig, tmpFilenameCsv);
                File.Delete(tmpFilenameOrig);
            }

            DataMapper.SourceFile = DataKonverterDataService.FillSourceFile(tmpFilenameCsv, true);

            return true;
        }
        #endregion

        #region ConnectionHandling


        #endregion

    }
}