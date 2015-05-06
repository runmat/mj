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
    public class Zb2BestandSecurityFleetViewModel : CkgBaseViewModel
    {

        [XmlIgnore]
        public IFahrzeugeDataService DataService { get { return CacheGet<IFahrzeugeDataService>(); } }

        [XmlIgnore]
        public List<Fahrzeughersteller> FahrzeugHersteller
        {
            get { return DataService.GetFahrzeugHersteller().Concat(new List<Fahrzeughersteller>
            {
                new Fahrzeughersteller { HerstellerKey = String.Empty, HerstellerName = Localize.DropdownDefaultOptionAll, ShowAllToken = true}
                                        }).OrderBy(w => w.HerstellerName).ToList();
            } 
        }                  

        public Zb2BestandSecurityFleetSelektor Zb2BestandSecurityFleetSelektor
        {
            get
            {
                return PropertyCacheGet(() => new Zb2BestandSecurityFleetSelektor());                                                   
            }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<Zb2BestandSecurityFleet> Zb2BestandSecurityFleets
        {
            get { return PropertyCacheGet(() => new List<Zb2BestandSecurityFleet>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<Zb2BestandSecurityFleet> Zb2BestandSecurityFleetsFiltered
        {
            get { return PropertyCacheGet(() => Zb2BestandSecurityFleets); }
            private set { PropertyCacheSet(value); }
        }
            
        public void DataInit()
        {          
            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.Zb2BestandSecurityFleetsFiltered);
        }

        public void Validate(Action<Expression<Func<Zb2BestandSecurityFleetSelektor, object>>, string> addModelError)
        {           
        }

        public void LoadZb2BestandSecurityFleet()
        {
            var fleet = DataService.GetZb2BestandSecurityFleet(Zb2BestandSecurityFleetSelektor);  
         
            fleet.ForEach(x => x.Lagerstatus = x.Lagerstatus == string.Empty ? "im Bestand" : x.Lagerstatus == "1" ? "temporär" : "entgültig");

            Zb2BestandSecurityFleets = fleet;
            DataMarkForRefresh();

            //XmlService.XmlSerializeToFile(Zb2BestandSecurityFleets, Path.Combine(AppSettings.DataPath, @"Zb2BestandSecurityFleets.xml"));
        }

        public void FilterZb2BestandSecurityFleet(string filterValue, string filterProperties)
        {
            Zb2BestandSecurityFleetsFiltered = Zb2BestandSecurityFleets.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

       



    }
}
