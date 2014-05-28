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
    public class AbgemeldeteFahrzeugeViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IFahrzeugeDataService DataService { get { return CacheGet<IFahrzeugeDataService>(); } }

        public AbgemeldeteFahrzeugeSelektor AbgemeldeteFahrzeugeSelektor
        {
            get 
            { 
                return PropertyCacheGet(() => new AbgemeldeteFahrzeugeSelektor
                {
                    NurKlaerfaelle = true, 
                    //Fin = "WAUZZZ8R0EA002995"
                }); 
            }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<AbgemeldetesFahrzeug> Fahrzeuge
        {
            get { return PropertyCacheGet(() => new List<AbgemeldetesFahrzeug>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<AbgemeldetesFahrzeug> FahrzeugeFiltered
        {
            get { return PropertyCacheGet(() => Fahrzeuge); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<FahrzeugStatus> FahrzeugStatusWerte
        {
            get { return PropertyCacheGet(() => DataService.FahrzeugStatusWerte); }
        }

        [XmlIgnore]
        public List<AbmeldeHistorie> AbmeldeHistorien { get; private set; }


        public void DataInit()
        {
            AbgemeldeteFahrzeugeSelektor.AlleFahrzeugStatusWerteStatic = DataService.FahrzeugStatusWerte;

            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.FahrzeugeFiltered);
        }

        public void Validate(Action<Expression<Func<AbgemeldeteFahrzeugeSelektor, object>>, string> addModelError)
        {
            //var selector = AbgemeldeteFahrzeugeSelektor;
            //if (!selector.NurKlaerfaelle && !selector.FahrzeugStatusWerte.AnyAndNotNull())
            //{
            //    addModelError(m => m.NurKlaerfaelle, Localize.PleaseProvideVehicleStatusInfo);
            //    addModelError(m => m.FahrzeugStatusWerte, Localize.PleaseProvideVehicleStatusInfo);
            //}
        }

        public void LoadAbgemeldeteFahrzeuge()
        {
            Fahrzeuge = DataService.GetAbgemeldeteFahrzeuge(AbgemeldeteFahrzeugeSelektor);

            DataMarkForRefresh();

            //XmlService.XmlSerializeToFile(Fahrzeuge, Path.Combine(AppSettings.DataPath, @"Fahrzeuge.xml"));
        }

        public void FilterFahrzeuge(string filterValue, string filterProperties)
        {
            FahrzeugeFiltered = Fahrzeuge.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }
    
        public void LoadHistorie(string fin)
        {
            AbmeldeHistorien = DataService.GetAbmeldeHistorien(fin);
        }
    }
}
