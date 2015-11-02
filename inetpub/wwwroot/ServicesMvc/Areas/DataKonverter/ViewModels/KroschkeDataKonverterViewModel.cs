using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
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

        #region File converter

        public string ConvertExcelToCsv(string excelFilename, string csvFilename, string delimeter = ";")
        {
            // var tempFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\CarDocu\\";
            // var tmpFileName = Path.Combine(tempFolder, resourceName);

            var tempFolder = GetUploadPathTemp();
            var tmpSourceFile = Path.Combine(tempFolder, excelFilename);
            var tmpDestFile = Path.Combine(tempFolder, csvFilename);
            var convert = DocumentTools.Services.SpireXlsFactory.ConvertExcelToCsv(tmpSourceFile, tmpDestFile, delimeter);

            return null;
        }

        public string GetUploadPathTemp()
        {
            return HttpContext.Current.Server.MapPath(string.Format(@"{0}", AppSettings.UploadFilePathTemp));
        }

        #endregion


    }

}