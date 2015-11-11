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
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.Fahrzeugbestand.Contracts;
using CkgDomainLogic.Fahrzeugbestand.Models;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Autohaus.Contracts;
using CkgDomainLogic.Autohaus.Models;
using CkgDomainLogic.DataKonverter.Contracts;
using CkgDomainLogic.Partner.Contracts;
using DocumentTools.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;
using SapORM.Contracts;
using ServicesMvc.Areas.DataKonverter.Models;

using Newtonsoft.Json;

namespace CkgDomainLogic.DataKonverter.ViewModels
{

    public class KroschkeDataKonverterViewModel : CkgBaseViewModel
    {
        [XmlIgnore, ScriptIgnore]
        public IDataKonverterDataService DataKonverterDataService { get { return CacheGet<IDataKonverterDataService>(); } }

        public SourceFile SourceFile { get; set; }
        public DestinationObj DestinationFile { get; set; }

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

            GlobalViewData = new GlobalViewData
            {
            };

            #endregion
        }


        #region File converter

        public string ConvertExcelToCsv(string excelFilename, string csvFilename, char delimeter = ';')
        {
            var tempFolder = GetUploadPathTemp();
            var tmpSourceFile = Path.Combine(tempFolder, excelFilename);
            var tmpDestFile = Path.Combine(tempFolder, csvFilename);

            var errorResult = DocumentTools.Services.SpireXlsFactory.ConvertExcelToCsv(tmpSourceFile, tmpDestFile, delimeter);            

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

        public DestinationObj FillDestinationObj(string filename)
        {
            var folder = GetUploadPathTemp();
            var filenameFull = Path.Combine(folder, filename);
            string xmlContent;

            using (var sr = new StreamReader(filenameFull))
            {
                xmlContent = sr.ReadToEnd();
            }

            var destinationObj = new DestinationObj
            {
                Filename = filename,
                XmlRaw = xmlContent,
                XmlDocument = StringToXmlDoc(xmlContent)
            };

            return destinationObj;
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
        public bool PdfUploadFileSave(string fileName, Func<string, string, string, string> fileSaveAction)
        {
            const string extension = ".pdf";

            //Upload.UploadFileName = fileName;
            var randomfilename = Guid.NewGuid().ToString();

            var nameSaved = fileSaveAction(GetUploadPathTemp(), randomfilename, extension);

            if (string.IsNullOrEmpty(nameSaved))
                return false;

            var tmpFilename = GetUploadPathTemp() + @"\" + nameSaved + extension;

            var bytes = File.ReadAllBytes(tmpFilename);
            //Overview.PdfUploaded = bytes;

            return true;
        }
        #endregion

    }
}