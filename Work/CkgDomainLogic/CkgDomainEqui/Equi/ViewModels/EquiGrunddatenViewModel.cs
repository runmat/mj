using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Equi.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Equi.ViewModels
{
    public class EquiGrunddatenViewModel : CkgBaseViewModel  
    {
        [XmlIgnore]
        public IEquiGrunddatenDataService DataService { get { return CacheGet<IEquiGrunddatenDataService>(); } }

        public EquiGrunddatenSelektor Selektor
        {
            get { return PropertyCacheGet(() => new EquiGrunddatenSelektor { Standorte = new List<string> {"1601"}}); } set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<SelectItem> Zielorte { get { return PropertyCacheGet(() => DataService.GetZielorte()); } }

        [XmlIgnore]
        public List<SelectItem> Standorte { get { return PropertyCacheGet(() => DataService.GetStandorte()); } }

        [XmlIgnore]
        public List<SelectItem> Betriebsnummern { get { return PropertyCacheGet(() => DataService.GetBetriebsnummern()); } }

        [XmlIgnore]
        public List<EquiGrunddaten> GrunddatenEquis
        {
            get { return PropertyCacheGet(() => new List<EquiGrunddaten>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<EquiGrunddaten> GrunddatenEquisFiltered
        {
            get { return PropertyCacheGet(() => GrunddatenEquis); }
            private set { PropertyCacheSet(value); }
        }

        public void DataInit()
        {
            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.GrunddatenEquisFiltered);
        }

        /// <summary>
        /// Eingabewerte einlesen, setzt ModelState entsprechend
        /// </summary>
        public void CheckInput(Action<string, string> addModelError)
        {
            if (Selektor.Fahrgestellnummer.IsNotNullOrEmpty())
                Selektor.Fahrgestellnummern = Selektor.Fahrgestellnummer.SplitSeparators()
                    .Select(teil => new Fahrgestellnummer { FIN = teil }).ToList();

            if (Selektor.Fahrgestellnummer10.IsNotNullOrEmpty())
                Selektor.Fahrgestellnummern10 = Selektor.Fahrgestellnummer10.SplitSeparators()
                    .Select(teil => new Fahrgestellnummer10 { FIN = teil }).ToList();
        }

        /// <summary>
        /// Grund-/Equi-Daten laden
        /// </summary>
        /// <returns></returns>
        public void LoadEquis()
        {
            GrunddatenEquis = DataService.GetEquis(Selektor);

            DataMarkForRefresh();
        }

        public void FilterEquis(string filterValue, string filterProperties)
        {
            GrunddatenEquisFiltered = GrunddatenEquis.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }
    }
}
