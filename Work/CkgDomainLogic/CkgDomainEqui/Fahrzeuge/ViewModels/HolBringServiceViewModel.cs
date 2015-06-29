using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
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
        public IFahrzeugeDataService DataService { get { return CacheGet<IFahrzeugeDataService>(); } }

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

        [LocalizedDisplay(LocalizeConstants.Date)]
        [Required]
        public DateTime? PickupDate { get; set; }

        public void DataInit()
        {
            Auftraggeber = new Auftraggeber();
            Abholung = new Abholung();
            Anlieferung = new Anlieferung();
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
