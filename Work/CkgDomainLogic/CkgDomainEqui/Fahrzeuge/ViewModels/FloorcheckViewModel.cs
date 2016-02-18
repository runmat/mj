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
        public bool ResetSelection { get; set; }

        public void Init()
        {
            if(FloorcheckHaendlers != null)
                FloorcheckHaendlers.Clear();
            if (FloorcheckHaendlerSelectItems != null)
                FloorcheckHaendlerSelectItems.Clear();

            FloorcheckHaendler = new FloorcheckHaendler();
        }

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
        public FloorcheckHaendler FloorcheckHaendler { get; set; }
        

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
            Floorchecks = DataService.GetFloorchecks(FloorcheckHaendler.SelectedHaendlerNummer);

            DataMarkForRefresh();            
        }

        public void LoadFloorcheckHaendler()
        {
            if (ResetSelection)
            {
                FloorcheckHaendler.SelectedHaendlerNummer = null;
                ResetSelection = false;
            }

            if (FloorcheckHaendler.SelectedHaendlerNummer.IsNullOrEmpty() && FloorcheckHaendler.HaendlerNummer.IsNullOrEmpty() && FloorcheckHaendler.HaendlerName.IsNullOrEmpty() && FloorcheckHaendler.HaendlerOrt.IsNullOrEmpty())
                FloorcheckHaendler.HaendlerName = "*";

            // Auswahl via Freitext überschreibt select
            if (FloorcheckHaendler.HaendlerNummer.IsNotNullOrEmpty() || FloorcheckHaendler.HaendlerName.IsNotNullOrEmpty() || FloorcheckHaendler.HaendlerOrt.IsNotNullOrEmpty())
                FloorcheckHaendler.SelectedHaendlerNummer = null;

            // Auswahl via select überschreibt freitext
            if (FloorcheckHaendler.SelectedHaendlerNummer.IsNotNullOrEmpty())
            {
                FloorcheckHaendler.HaendlerNummer = FloorcheckHaendler.SelectedHaendlerNummer;
                FloorcheckHaendler.HaendlerName = "";
                FloorcheckHaendler.HaendlerOrt = "";
            }

            FloorcheckHaendlers = DataService.GetFloorcheckHaendler(FloorcheckHaendler);

            if (FloorcheckHaendlers.Count == 1)
            {
                FloorcheckHaendler.SelectedHaendlerNummer = FloorcheckHaendlers.First().HaendlerNummer;
                LoadFloorcheck();
            }
            else
                Floorchecks.Clear();

            DataMarkForRefresh();
        }

        public void FilterFloorchecks(string filterValue, string filterProperties)
        {
            FloorchecksFiltered = Floorchecks.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion

    }
}
