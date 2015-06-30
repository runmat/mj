using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models.HolBringService;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using DocumentTools.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Fahrzeuge.ViewModels
{
    public class HolBringServiceViewModel : CkgBaseViewModel
    {
        public Auftraggeber Auftraggeber { get; set; }
        public Abholung Abholung { get; set; }
        public Anlieferung Anlieferung { get; set; }
        public Upload Upload { get; set; }

        [XmlIgnore]
        public IHolBringServiceDataService DataService { get { return CacheGet<IHolBringServiceDataService>(); } }

        public List<Domaenenfestwert> Fahrzeugarten { get; set; }
        public List<DropDownTimeItem> DropDownHours { get; set; }
        public List<DropDownTimeItem> DropDownMinutes { get; set; }
        public List<Domaenenfestwert> AbholungUhrzeitStundenList { get; set; }

        // CkgDomainLogic.General.Contracts.ILogonContextDataService

        public string Username { get; set; }                // Antragsteller
        public List<string> Betriebe { get; set; }
        public List<string> Ansprechpartner { get; set; }

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
                    { "Übersicht", Localize.Overview },
                    { "Fertig", Localize.Ready + " !" },
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

        public class DropDownTimeItem
        {
            [SelectListKey]
            public string ID { get; set; }

            [SelectListText]
            public string Name { get; set; }
        }
       
        public void DataInit()
        {
            Auftraggeber = new Auftraggeber
                {
                    Auftragsersteller = DataService.GetUsername,
                    AuftragerstellerTel = DataService.GetUserTel,
                    Betrieb = "Betrieb",
                    Ansprechpartner = "Ansprechpartner",
                    AnsprechpartnerTel = "AnsprechpartnerTel"
                    
                };

            Abholung = new Abholung();
            Anlieferung = new Anlieferung();
            Upload = new Upload();
            Fahrzeugarten = DataService.GetFahrzeugarten;

            var selectableHours = new List<DropDownTimeItem>
                {
                    new DropDownTimeItem {ID = "Stunden", Name = "Stunden"}
                };
            for (var i = 5; i < 22; i++)
            {
                selectableHours.Add(new DropDownTimeItem { ID = i.ToString(), Name = i.ToString() });
                
            }
            DropDownHours = selectableHours;
            DropDownHours = selectableHours;

            var selectableMinutes = new List<DropDownTimeItem>
                {
                    new DropDownTimeItem {ID = "Minuten", Name = "Minuten"},
                    new DropDownTimeItem {ID = "00", Name = "00"},
                    new DropDownTimeItem {ID = "15", Name = "15"},
                    new DropDownTimeItem {ID = "30", Name = "30"},
                    new DropDownTimeItem {ID = "45", Name = "45"}
                };
            DropDownMinutes = selectableMinutes;
            DropDownMinutes = selectableMinutes;

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
            // var extension = (Upload.UploadFileName.NotNullOrEmpty().ToLower().EndsWith(".xls") ? ".xls" : ".csv");
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

        [XmlIgnore]
        public string FeiertageAsString { get { return DateService.FeiertageAsString; } }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return null;    // throw new NotImplementedException();
        }
    }
}
