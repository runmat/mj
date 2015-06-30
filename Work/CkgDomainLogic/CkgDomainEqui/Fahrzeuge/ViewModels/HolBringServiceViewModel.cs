using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models.HolBringService;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
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
        public string UploadFile { get; set; }

        [XmlIgnore]
        public IHolBringServiceDataService DataService { get { return CacheGet<IHolBringServiceDataService>(); } }

        public List<Domaenenfestwert> Fahrzeugarten { get; set; }
        public List<DropDownTimeItem> DropDownHours { get; set; }
        public List<DropDownTimeItem> DropDownMinutes { get; set; }
        public List<Domaenenfestwert> AbholungUhrzeitStundenList { get; set; }

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
            // [LocalizedDisplay(LocalizeConstants.Code)]
            [SelectListKey]
            public string ID { get; set; }

            // [LocalizedDisplay(LocalizeConstants.Country)]
            [SelectListText]
            public string Name { get; set; }
        }

        //public class DropDownMinute
        //{
        //    // [LocalizedDisplay(LocalizeConstants.Code)]
        //    [SelectListKey]
        //    public string ID { get; set; }

        //    // [LocalizedDisplay(LocalizeConstants.Country)]
        //    [SelectListText]
        //    public string Name { get; set; }
        //}

        //[LocalizedDisplay(LocalizeConstants.Date)]
        //[Required]
        //public DateTime? PickupDate { get; set; }

        public void DataInit()
        {
            Auftraggeber = new Auftraggeber();
            Abholung = new Abholung();
            Anlieferung = new Anlieferung();
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

        public void DataMarkForRefresh()
        {
            //PropertyCacheClear(this, m => m.Fahrzeuge);
            //PropertyCacheClear(this, m => m.FahrzeugeFiltered);
            //PropertyCacheClear(this, m => m.FahrzeugeGroupByModel);
            //PropertyCacheClear(this, m => m.FahrzeugeGroupByModelId);

            //SelectedZulassungsDatum = null;
            //SelectedKennzeichenSerie = null;
            //SelectedPdi = null;
            //SelectedModel = null;
            //SelectedModelId = null;
            //ZulassungenForPdiAndDate = new List<Fzg>();

            //DataMarkForRefreshFahrzeugeSummary();
        }

        [XmlIgnore]
        public string FeiertageAsString { get { return DateService.FeiertageAsString; } }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // throw new NotImplementedException();
            return null;
        }
    }
}
