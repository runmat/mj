using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml.Serialization;
using CkgDomainLogic.Autohaus.Models;
using CkgDomainLogic.Feinstaub.Contracts;
using CkgDomainLogic.Feinstaub.Models;
using CkgDomainLogic.General.ViewModels;

namespace CkgDomainLogic.Feinstaub.ViewModels
{
    public class AutohausFeinstaubReportViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IAutohausFeinstaubReportDataService DataService { get { return CacheGet<IAutohausFeinstaubReportDataService>(); } }

        [XmlIgnore]
        public List<Kundenstammdaten> Kundenstamm { get { return DataService.Kundenstamm; } }

        [XmlIgnore]
        public List<FeinstaubVergabeInfo> VergabeInfos { get { return DataService.VergabeInfos; } }

        public void LoadStammdaten(ModelStateDictionary state)
        {
            DataService.MarkForRefreshKundenstamm();

            CheckStammdaten(state);
        }

        public void LoadVergabeInfos(FeinstaubSuchparameter suchparameter, ModelStateDictionary state)
        {
            CheckStammdaten(state);
            if (state.IsValid)
            {
                DataService.Suchparameter = suchparameter;
                DataService.MarkForRefreshVergabeInfos();
                if (VergabeInfos.Count == 0)
                {
                    state.AddModelError("", "Keine Daten gefunden");
                }
            }
        }

        private void CheckStammdaten(ModelStateDictionary state)
        {
            if (Kundenstamm == null || Kundenstamm.Count == 0)
            {
                state.AddModelError("", "Fehler: Kundenstammdaten nicht gepflegt! Bitte wenden Sie sich an unsere Service-Rufnummer 04102/804-170.");
            }
            else if (Kundenstamm.Count > 1)
            {
                state.AddModelError("", "Fehler: Kundenstammdaten nicht eindeutig! Bitte wenden Sie sich an unsere Service-Rufnummer 04102/804-170.");
            }
        }
    }
}
