using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.Equi.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Equi.ViewModels
{
    public class ErweiterterBriefbestandViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IErweiterterBriefbestandDataService DataService { get { return CacheGet<IErweiterterBriefbestandDataService>(); } }

        [XmlIgnore]
        public List<FahrzeugbriefErweitert> Fahrzeugbriefe { get { return DataService.Fahrzeugbriefe; } }

        private FahrzeugbriefErweitert _selectedItem;

        public void LoadFahrzeugbriefe(FahrzeugbriefSuchparameter suchparameter)
        {
            DataService.Suchparameter = suchparameter;
            DataService.MarkForRefreshFahrzeugbriefe();
            PropertyCacheClear(this, m => m.FahrzeugbriefeFiltered);
        }


        public FahrzeugbriefErweitert GetItem(string fin)
        {
            _selectedItem = Fahrzeugbriefe.FirstOrDefault(m => m.Fahrgestellnummer == fin);
            return _selectedItem;
        }

        public string SaveItem(FahrzeugbriefErweitert model)
        {
            var error = DataService.SaveSperrvermerk(model);

            if (error.IsNullOrEmptyOrNullString() && _selectedItem != null && model.Fahrgestellnummer == _selectedItem.Fahrgestellnummer)
            {
                _selectedItem.Referenz1 = model.Referenz1;
                if (model.Referenz1.IsNotNullOrEmpty())
                    _selectedItem.Referenz2 = model.Referenz2;
                else
                    _selectedItem.Referenz2 = "";
            }

            return error;
        }

        #region Filter

        [XmlIgnore]
        public List<FahrzeugbriefErweitert> FahrzeugbriefeFiltered
        {
            get { return PropertyCacheGet(() => Fahrzeugbriefe); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterFahrzeugbriefe(string filterValue, string filterProperties)
        {
            FahrzeugbriefeFiltered = Fahrzeugbriefe.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
