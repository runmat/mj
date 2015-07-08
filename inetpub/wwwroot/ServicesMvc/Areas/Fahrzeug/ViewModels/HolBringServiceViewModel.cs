using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.Autohaus.Models;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models.HolBringService;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using DocumentTools.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;
using ServicesMvc.Areas.Fahrzeug.Models.HolBringService;

namespace CkgDomainLogic.Fahrzeuge.ViewModels
{
    public class HolBringServiceViewModel : CkgBaseViewModel
    {
        public IHolBringServiceDataService DataService { get { return CacheGet<IHolBringServiceDataService>(); } }

        #region Models der einzelnen Partials
        public Auftraggeber Auftraggeber            // public Auftraggeber Auftraggeber { get; set; }
        {
            get { return PropertyCacheGet(() => Auftraggeber); }
            set { PropertyCacheSet(value); }
        }

        public Abholung Abholung                    // public Abholung Abholung { get; set; }
        {
            get { return PropertyCacheGet(() => Abholung); }
            set { PropertyCacheSet(value); }
        }

        public Anlieferung Anlieferung              // public Anlieferung Anlieferung { get; set; }
        {
            get { return PropertyCacheGet(() => Anlieferung); }
            set { PropertyCacheSet(value); }
        }

        public Upload Upload                        // public Upload Upload { get; set; }
        {
            get { return PropertyCacheGet(() => Upload); }
            set { PropertyCacheSet(value); }
        }

        public Overview Overview { get; set; }
        #endregion

        public GlobalViewData GlobalViewData;   // Model für Nutzung in allen Partials

        #region Wizard
        [XmlIgnore]
        public IDictionary<string, string> Steps
        {
            get
            {
                return PropertyCacheGet(() => new Dictionary<string, string>
                {
                    { "Auftraggeber", Localize.Client },
                    { "Abholung", Localize.Pickup },
                    { "Anlieferung", Localize.DeliveryHolBringService },
                    { "Upload", Localize.Upload },
                    { "Overview", Localize.Overview },
                    { "Ready", Localize.Ready + " !" },
                });
            }
        }

        public string[] StepKeys { get { return PropertyCacheGet(() => Steps.Select(s => s.Key).ToArray()); } }

        public string[] StepFriendlyNames { get { return PropertyCacheGet(() => Steps.Select(s => s.Value).ToArray()); } }

        public string FirstStepPartialViewName
        {
            get { return string.Format("{0}", StepKeys[0]); }
        }
        #endregion

        public byte[] GenerateSapPdf(List<BapiParameterSet> bapiParameterSets)
        {
            byte[] pdfGenerated;

            DataService.GenerateSapPdf(bapiParameterSets, out pdfGenerated);

            return pdfGenerated;
        }

        public void DataInit()
        {
            #region Globale Properties, nutzbar in allen Partials
            GlobalViewData = new GlobalViewData
                {
                    BetriebeSap = DataService.LoadKundenFromSap(),

                    Fahrzeugarten = DataService.GetFahrzeugarten,
                    FeiertageAsString =  DateService.FeiertageAsString,
                    AnsprechpartnerList = DataService.GetAnsprechpartner
                };

            #endregion

            Auftraggeber = new Auftraggeber
                {
                    Auftragsersteller = DataService.GetUsername,
                    AuftragerstellerTel = DataService.GetUserTel,
                };

            Abholung = new Abholung();
            Anlieferung = new Anlieferung();
            Upload = new Upload();

            DataMarkForRefresh();
        }

        public void CopyDefaultValuesToAnlieferung(Abholung model)
        {
            if (!string.IsNullOrEmpty(model.AbholungStrasseHausNr) && string.IsNullOrEmpty(Anlieferung.AnlieferungStrasseHausNr))
                Anlieferung.AnlieferungStrasseHausNr = model.AbholungStrasseHausNr;

            if (!string.IsNullOrEmpty(model.AbholungPlz) && string.IsNullOrEmpty(Anlieferung.AnlieferungPlz))
                Anlieferung.AnlieferungPlz = model.AbholungPlz;

            if (!string.IsNullOrEmpty(model.AbholungOrt) && string.IsNullOrEmpty(Anlieferung.AnlieferungOrt))
                Anlieferung.AnlieferungOrt = model.AbholungOrt;

            if (!string.IsNullOrEmpty(model.AbholungAnsprechpartner) && string.IsNullOrEmpty(Anlieferung.AnlieferungAnsprechpartner))
                Anlieferung.AnlieferungAnsprechpartner = model.AbholungAnsprechpartner;

            if (!string.IsNullOrEmpty(model.AbholungStrasseHausNr) && string.IsNullOrEmpty(Anlieferung.AnlieferungStrasseHausNr))
                Anlieferung.AnlieferungStrasseHausNr = model.AbholungStrasseHausNr;

            if (!string.IsNullOrEmpty(model.AbholungTel) && string.IsNullOrEmpty(Anlieferung.AnlieferungTel))
                Anlieferung.AnlieferungTel = model.AbholungTel;
        }

        public void DataMarkForRefresh()
        {
        }

        public bool PdfUploadFileSave(string fileName, Func<string, string, string, string> fileSaveAction)
        {
            Upload.UploadFileName = fileName;
            var randomfilename = Guid.NewGuid().ToString();
            var extension = ".pdf"; //  (Upload.UploadFileName.NotNullOrEmpty().ToLower().EndsWith(".xls") ? ".xls" : ".csv");
            Upload.UploadServerFileName = Path.Combine(AppSettings.TempPath, randomfilename + extension);

            var nameSaved = fileSaveAction(AppSettings.TempPath, randomfilename, extension);

            if (string.IsNullOrEmpty(nameSaved))
                return false;

            //var list = new ExcelDocumentFactory().ReadToDataTable(Upload.UploadServerFileName, true, "", CreateInstanceFromDatarow, ';', true, true).ToList();
            //FileService.TryFileDelete(Upload.UploadServerFileName);
            //if (list.None())
            //    return false;

            return true;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return null;    
        }
    }
}
