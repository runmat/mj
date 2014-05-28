using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Equi.Models;
using System.Web.Mvc;
using GeneralTools.Models;
// ReSharper disable RedundantUsingDirective
using System.IO;
using GeneralTools.Services;
// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.Equi.ViewModels
{
    public class EquiGrunddatenViewModel : CkgBaseViewModel  
    {
        [XmlIgnore]
        public IEquiGrunddatenDataService DataService { get { return CacheGet<IEquiGrunddatenDataService>(); } }

        public GrunddatenEquiSuchparameter Suchparameter
        {
            get { return PropertyCacheGet(() => new GrunddatenEquiSuchparameter { Standorte = new List<string> {"1601"}}); } set { PropertyCacheSet(value); }
        }

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
            if (Suchparameter.Fahrgestellnummer.IsNotNullOrEmpty())
                Suchparameter.Fahrgestellnummern = Suchparameter.Fahrgestellnummer.SplitSeparators()
                    .Select(teil => new Fahrgestellnummer { FIN = teil }).ToList();

            if (Suchparameter.Fahrgestellnummer10.IsNotNullOrEmpty())
                Suchparameter.Fahrgestellnummern10 = Suchparameter.Fahrgestellnummer10.SplitSeparators()
                    .Select(teil => new Fahrgestellnummer10 { FIN = teil }).ToList();
        }

        /// <summary>
        /// Grund-/Equi-Daten laden
        /// </summary>
        /// <returns></returns>
        public void LoadEquis()
        {
            GrunddatenEquis = DataService.GetEquis(Suchparameter);

            DataMarkForRefresh();

            //XmlService.XmlSerializeToFile(GrunddatenEquis, Path.Combine(AppSettings.DataPath, @"GrunddatenEquis_02.xml"));
        }

        public void FilterEquis(string filterValue, string filterProperties)
        {
            GrunddatenEquisFiltered = GrunddatenEquis.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }
    }
}
