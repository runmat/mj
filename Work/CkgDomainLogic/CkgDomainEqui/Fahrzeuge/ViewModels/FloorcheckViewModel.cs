using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using GeneralTools.Models;
using System.Linq;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeuge.ViewModels
{
    public class FloorcheckViewModel : CkgBaseViewModel
    {

        [XmlIgnore]
        public IFahrzeugeDataService DataService { get { return CacheGet<IFahrzeugeDataService>(); } }

      
        [XmlIgnore]
        public List<Floorcheck> Floorchecks
        {
            get { return PropertyCacheGet(() => new List<Floorcheck>()); }
            private set { PropertyCacheSet(value); }
        }


        static List<FloorcheckHaendler> _floorcheckHaendlers = new List<FloorcheckHaendler>();
        [XmlIgnore]
        public static List<FloorcheckHaendler> FloorcheckHaendlers
        {
            get { return _floorcheckHaendlers; }
            set { _floorcheckHaendlers = value; }
        }


        public static List<SelectItem> FloorcheckHaendlerSelectItems
        {
            get
            {                
                var list = new List<SelectItem>();
               
                foreach (var item in FloorcheckHaendlers.OrderBy(a => a.HaendlerName))                
                    list.Add(new SelectItem(item.HaendlerNummer, item.HaendlerNummer + ", " + item.HaendlerName + ", " + item.HaendlerOrt));                
                return list;
            }
        }

        [XmlIgnore]
        public FloorcheckHaendler FloorcheckHaendler
        {
            get { return PropertyCacheGet(() => new FloorcheckHaendler()); }
            set { PropertyCacheSet(value); }
        }
        

        #region Filter

        [XmlIgnore]
        public List<Floorcheck> FloorchecksFiltered
        {
            get { return PropertyCacheGet(() => Floorchecks); }
            private set { PropertyCacheSet(value); }
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.FloorchecksFiltered);
        }

        public void LoadFloorcheck()
        {            
            Floorchecks = DataService.GetFloorchecks(FloorcheckHaendler.HaendlerNummer);

            DataMarkForRefresh();            
        }

        public void LoadFloorcheckHaendler()
        {
            if (FloorcheckHaendler.HaendlerNummer.IsNullOrEmpty() && FloorcheckHaendler.HaendlerName.IsNullOrEmpty() && FloorcheckHaendler.HaendlerOrt.IsNullOrEmpty())
                FloorcheckHaendler.HaendlerName = "*";

            FloorcheckHaendlers = DataService.GetFloorcheckHaendler(FloorcheckHaendler);

            DataMarkForRefresh();
        }

        public void FilterFloorchecks(string filterValue, string filterProperties)
        {
            FloorchecksFiltered = Floorchecks.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion

    }
}
