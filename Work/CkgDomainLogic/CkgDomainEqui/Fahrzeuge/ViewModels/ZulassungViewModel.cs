// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Serialization;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using System.Web.Mvc;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using GeneralTools.Models;
using System.IO;
using GeneralTools.Services;
// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.Fahrzeuge.ViewModels
{
    public class ZulassungViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IFahrzeugeDataService DataService { get { return CacheGet<IFahrzeugeDataService>(); } }

        [XmlIgnore]
        public List<AbgemeldetesFahrzeug> Fahrzeuge
        {
            get { return PropertyCacheGet(() => new List<AbgemeldetesFahrzeug>
            {
                new AbgemeldetesFahrzeug { Briefnummer = "4711" },
                new AbgemeldetesFahrzeug { Briefnummer = "4712" },
            });
            }
            protected set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<AbgemeldetesFahrzeug> FahrzeugeFiltered
        {
            get { return PropertyCacheGet(() => Fahrzeuge); }
            protected set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<FahrzeugStatus> FahrzeugStatusWerte
        {
            get { return PropertyCacheGet(() => DataService.FahrzeugStatusWerte); }
        }


        public void DataInit()
        {
            AbgemeldeteFahrzeugeSelektor.AlleFahrzeugStatusWerteStatic = DataService.FahrzeugStatusWerte;
            DataMarkForRefresh();
        }

        public void DataInit(bool preSelection)
        {
            DataInit();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.FahrzeugeFiltered);
        }

        public void Validate(Action<Expression<Func<AbgemeldeteFahrzeugeSelektor, object>>, string> addModelError)
        {
        }

        public void FilterFahrzeuge(string filterValue, string filterProperties)
        {
            FahrzeugeFiltered = Fahrzeuge.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }
    }
}
