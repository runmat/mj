using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Autohaus.Contracts;
using CkgDomainLogic.Autohaus.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Autohaus.ViewModels
{
    public class FahrzeugverwaltungViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IFahrzeugverwaltungDataService DataService { get { return CacheGet<IFahrzeugverwaltungDataService>(); } }

        public bool InsertMode { get; set; }

        public void DataInit()
        {
            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.Fahrzeuge);
            PropertyCacheClear(this, m => m.FahrzeugeFiltered);
        }

        [XmlIgnore]
        public List<Fahrzeug> Fahrzeuge
        {
            get { return PropertyCacheGet(() => DataService.FahrzeugeGet()); }
        }

        public Fahrzeug FahrzeugCurrent { get; private set; }

        public Fahrzeug FahrzeugGet(int id)
        {
            var item = Fahrzeuge.FirstOrDefault(c => c.ID == id);

            return item;
        }

        public Fahrzeug FahrzeugCreate()
        {
            var item = new Fahrzeug
            {
                KundenNr = LogonContext.KundenNr,
                AnlageUser = LogonContext.UserName,
                AnlageDatum = DateTime.Now,
            };

            return item;
        }

        public Fahrzeug FahrzeugAdd(Fahrzeug newItem, Action<string, string> addModelError)
        {
            newItem = DataService.FahrzeugAdd(newItem, addModelError);
            Fahrzeuge.Add(newItem);
            DataMarkForRefresh();
            return newItem;
        }

        public void FahrzeugDelete(int id)
        {
            Fahrzeuge.Remove(FahrzeugGet(id));
            DataService.FahrzeugDelete(id);

            DataMarkForRefresh();
        }

        public Fahrzeug FahrzeugSave(Fahrzeug item, Action<string, string> addModelError)
        {
            if (InsertMode)
                return FahrzeugAdd(item, addModelError);

            var savedItem = DataService.FahrzeugSave(item, addModelError);
            DataMarkForRefresh();
            return savedItem;
        }

        public void FahrzeugCurrentDisable()
        {
            LoadFahrzeug(0);
        }

        public void LoadFahrzeug(int fahrzeugID)
        {
            FahrzeugCurrent = Fahrzeuge.FirstOrDefault(fall => fall.ID == fahrzeugID);
        }

        #region Filter

        [XmlIgnore]
        public List<Fahrzeug> FahrzeugeFiltered
        {
            get { return PropertyCacheGet(() => Fahrzeuge); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterFahrzeuge(string filterValue, string filterProperties)
        {
            FahrzeugeFiltered = Fahrzeuge.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
