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

namespace CkgDomainLogic.Fahrzeuge.ViewModels
{
    public class FahrzeuguebersichtViewModel : CkgBaseViewModel
    {

        [XmlIgnore]
        public IFahrzeuguebersichtDataService DataService { get { return CacheGet<IFahrzeuguebersichtDataService>(); } }

        [XmlIgnore]
        public IFahrzeugeDataService DataServiceHersteller { get { return CacheGet<IFahrzeugeDataService>(); } }       

        [XmlIgnore]
        public List<Fahrzeughersteller> FahrzeugHersteller
        {
            get
            {
                return DataServiceHersteller.GetFahrzeugHersteller().Concat(new List<Fahrzeughersteller>
            {
                new Fahrzeughersteller { HerstellerKey = String.Empty, HerstellerName = Localize.DropdownDefaultOptionAll, ShowAllToken = true}
                                        }).OrderBy(w => w.HerstellerName).ToList();
            }
        }

        [XmlIgnore]
        public List<FahrzeuguebersichtPDI> PDIStandorte
        {
            get
            {
                return DataService.GetPDIStandorte().Concat(new List<FahrzeuguebersichtPDI>
            {
                new FahrzeuguebersichtPDI { PDIKey = String.Empty, PDIText = Localize.DropdownDefaultOptionAll }
                                        }).OrderBy(w => w.PDIText).ToList();
            }
        }
       
        [XmlIgnore]
        public List<FahrzeuguebersichtStatus> FahrzeugStatus
        {
            get
            {
                return DataService.GetFahrzeugStatus().Concat(new List<FahrzeuguebersichtStatus>
            {
                new FahrzeuguebersichtStatus { StatusKey = String.Empty, StatusText = Localize.DropdownDefaultOptionAll }
                                        }).OrderBy(w => w.StatusText).ToList();
            }
        }
                           
        [XmlIgnore]
        public List<Fahrzeuguebersicht> Fahrzeuguebersichts
        {
            get { return PropertyCacheGet(() => new List<Fahrzeuguebersicht>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<Fahrzeuguebersicht> FahrzeuguebersichtsFiltered
        {
            get { return PropertyCacheGet(() => Fahrzeuguebersichts); }
            private set { PropertyCacheSet(value); }
        }


        public FahrzeuguebersichtSelektor FahrzeuguebersichtSelektor
        {
            get { return PropertyCacheGet(() => new FahrzeuguebersichtSelektor()); }
            set { PropertyCacheSet(value); }
        }

        public void Init()
        {           
        }

        public void DataInit()
        {          
            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.FahrzeuguebersichtsFiltered);
        }

       
        public void LoadFahrzeuguebersicht()
        {
            Fahrzeuguebersichts = DataService.GetFahrzeuguebersicht(FahrzeuguebersichtSelektor); 
            DataMarkForRefresh();

            //XmlService.XmlSerializeToFile(Fahrzeuguebersichts, Path.Combine(AppSettings.DataPath, @"Fahrzeuguebersichts.xml"));
        }

        public void FilterFahrzeuguebersicht(string filterValue, string filterProperties)
        {
            FahrzeuguebersichtsFiltered = Fahrzeuguebersichts.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }
       
    }
}
