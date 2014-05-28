using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Insurance.Contracts;
using CkgDomainLogic.Insurance.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Insurance.ViewModels
{
    public class SchadenverwaltungViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public ISchadenverwaltungDataService DataService { get { return CacheGet<ISchadenverwaltungDataService>(); } }

        [XmlIgnore]
        public List<Schadenfall> Schadenfaelle { get { return DataService.Schadenfaelle; } }

        [XmlIgnore]
        public List<VersEvent> Events { get { return DataService.Events; } }

        [XmlIgnore]
        public List<Versicherung> Versicherungen { get { return DataService.Versicherungen; } }

        public void LoadSchadenfaelle()
        {
            DataService.MarkForRefreshSchadenfaelle();
            DataService.MarkForRefreshEvents();
            DataService.MarkForRefreshVersicherungen();
            PropertyCacheClear(this, m => m.SchadenfaelleFiltered);
        }

        public Schadenfall GetSchadenfall(int id)
        {
            return Schadenfaelle.Find(s => s.ID == id);
        }

        public Schadenfall GetNewSchadenfall()
        {
            return new Schadenfall();
        }

        public void SaveChanges(Schadenfall model, ModelStateDictionary state)
        {
            var dublette = Schadenfaelle.Find(s => s.EventID == model.EventID && s.Kennzeichen.ToUpper() == model.Kennzeichen.ToUpper() && s.ID != model.ID);
            if (dublette != null)
            {
                state.AddModelError("", Localize.DamageCaseAlreadyExists);
            }
            else
            {
                var erg = DataService.SaveSchadenfall(model);
                if (!String.IsNullOrEmpty(erg))
                {
                    state.AddModelError("", erg);
                }
                LoadSchadenfaelle();
            }           
        }

        public string DeleteSchadenfall(string id)
        {
            var erg = DataService.DeleteSchadenfall(id);
            LoadSchadenfaelle();

            return erg;
        }

        #region Filter

        [XmlIgnore]
        public List<Schadenfall> SchadenfaelleFiltered
        {
            get { return PropertyCacheGet(() => Schadenfaelle); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterSchadenfaelle(string filterValue, string filterProperties)
        {
            SchadenfaelleFiltered = Schadenfaelle.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
