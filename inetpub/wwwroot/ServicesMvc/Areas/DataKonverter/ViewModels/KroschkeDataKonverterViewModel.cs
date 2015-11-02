using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
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

namespace CkgDomainLogic.DataKonverter.ViewModels
{

    public class KroschkeDataKonverterViewModel : CkgBaseViewModel
    {
        [XmlIgnore, ScriptIgnore]
        public IDataKonverterDataService DataKonverterDataService { get { return CacheGet<IDataKonverterDataService>(); } }

        #region Wizard
        [XmlIgnore]
        public IDictionary<string, string> Steps
        {
            get
            {
                return PropertyCacheGet(() => new Dictionary<string, string>
                {
                    { "Prozessauswahl", "Prozessauswahl" },         // Localize.Vehicle
                    { "Konfiguration", "Konfiguration" },
                    { "Testimport", "Testimport" },
                    { "Abschluss", Localize.Ready + "!" },
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

        #region File converter

        public string ConvertExcelToCsv(string excelFilename, string csvFilename, string delimeter = ";")
        {
            var tempFolder = GetUploadPathTemp();
            var tmpSourceFile = Path.Combine(tempFolder, excelFilename);
            var tmpDestFile = Path.Combine(tempFolder, csvFilename);
            var convert = DocumentTools.Services.SpireXlsFactory.ConvertExcelToCsv(tmpSourceFile, tmpDestFile, delimeter);

            //using (var sr = new StreamReader(tmpDestFile, Encoding.UTF8))
            //{
            //    // sw.WriteLine("頼もう");
            //}

            // var test = GetFileEncoding(tmpDestFile, Encoding.UTF8);

            var test = CsvReaderFactory.ReadCsv(tmpDestFile, true);

            var sb = new StringBuilder();
            using (var sr = new StreamReader(tmpDestFile, Encoding.Default, true))
            {
                string line;
                // Read and display lines from the file until the end of the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    sb.AppendLine(line);
                }
            }

            return tmpDestFile;
        }

        public string GetUploadPathTemp()
        {
            return HttpContext.Current.Server.MapPath(string.Format(@"{0}", AppSettings.UploadFilePathTemp));
        }

        protected byte[] GetCsvFileContent(string fileName)
        {
            var sb = new StringBuilder();
            using (var sr = new StreamReader(fileName, Encoding.Default, true))
            {
                string line;
                // Read and display lines from the file until the end of the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    sb.AppendLine(line);
                }
            }
            var allines = sb.ToString();

            var utf8 = new UTF8Encoding();

            var preamble = utf8.GetPreamble();

            var data = utf8.GetBytes(allines);

            return data;
        }

        #endregion

    }
}