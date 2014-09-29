// ReSharper disable RedundantUsingDirective

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml.Serialization;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Equi.Models;
using System.Linq;
using GeneralTools.Models;
// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.Equi.ViewModels
{
    public class VersandBeauftragungenViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IBriefbestandDataService DataService { get { return CacheGet<IBriefbestandDataService>(); } }

        public VersandBeauftragungSelektor VersandBeauftragungSelektor
        {
            get { return PropertyCacheGet(() => new VersandBeauftragungSelektor()); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<Fahrzeugbrief> VersandBeauftragungen
        {
            get { return PropertyCacheGet(() => new List<Fahrzeugbrief>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<Fahrzeugbrief> VersandBeauftragungenFiltered
        {
            get { return PropertyCacheGet(() => VersandBeauftragungen); }
            private set { PropertyCacheSet(value); }
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.VersandBeauftragungenFiltered);
            PropertyCacheClear(this, m => m.VersandBeauftragungSelektor);
        }

        public void LoadVersandBeauftragungen(VersandBeauftragungSelektor model)
        {
            VersandBeauftragungen = DataService.GetVersandBeauftragungen(model);
            DataMarkForRefresh();
        }

        public void FilterVersandBeauftragungen(string filterValue, string filterProperties)
        {
            VersandBeauftragungenFiltered = VersandBeauftragungen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public void SelectVersandBeauftragung(string fahrzeugID, bool select, out int allSelectionCount)
        {
            if (fahrzeugID.IsNullOrEmpty())
                VersandBeauftragungen.ForEach(c => c.IsSelected = select);
            else
            {
                allSelectionCount = 0;
                var item = VersandBeauftragungen.FirstOrDefault(f => f.Fahrgestellnummer == fahrzeugID);
                if (item == null)
                    return;
                item.IsSelected = select;
            }

            allSelectionCount = VersandBeauftragungen.Count(c => c.IsSelected);
        }

        public string DeleteSelectedVersandBeauftragungen()
        {
            foreach (var fahrzeugbrief in VersandBeauftragungen.Where(c => c.IsSelected))
            {
                var error = DataService.DeleteVersandBeauftragungen(fahrzeugbrief.Fahrgestellnummer);
                if (error.IsNotNullOrEmpty())
                    return error;
                
                fahrzeugbrief.MarkForDelete = true;
            }

            VersandBeauftragungen.RemoveAll(c => c.MarkForDelete);

            return "";
        }
    }
}
