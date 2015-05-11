// ReSharper disable RedundantUsingDirective

#region using
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
#endregion
// ReSharper restore RedundantUsingDirective


namespace CkgDomainLogic.Fahrzeuge.ViewModels
{
    public class UnfallmeldungenViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IFahrzeugeDataService DataService { get { return CacheGet<IFahrzeugeDataService>(); } }

        public UnfallmeldungenSelektor UnfallmeldungenSelektor
        {
            get { return PropertyCacheGet(() => new UnfallmeldungenSelektor()); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<Unfallmeldung> Unfallmeldungen
        {
            get { return PropertyCacheGet(() => new List<Unfallmeldung>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<Unfallmeldung> UnfallmeldungenFiltered
        {
            get { return PropertyCacheGet(() => Unfallmeldungen); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<FahrzeugStatus> FahrzeugStatusWerte
        {
            get { return PropertyCacheGet(() => DataService.FahrzeugStatusWerte); }
        }
      
        public void DataInit()
        {           
            DataMarkForRefresh();
            UnfallmeldungenSelektor.MeldeDatumRange.IsSelected = true;
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.UnfallmeldungenFiltered);
        }

        public void Validate(Action<Expression<Func<UnfallmeldungenSelektor, object>>, string> addModelError)
        {            
        }

        public void LoadUnfallmeldungen()
        {
            Unfallmeldungen = DataService.GetUnfallmeldungen(UnfallmeldungenSelektor);

            DataMarkForRefresh();
        }

        public void LoadAllUnfallmeldungen()
        {
            Unfallmeldungen = DataService.GetUnfallmeldungen(new UnfallmeldungenSelektor());

            DataMarkForRefresh();
        }

        public void SelectUnfallmeldung(string vin, bool select, out int allSelectionCount)
        {
            allSelectionCount = 0;
            var fzg = Unfallmeldungen.FirstOrDefault(f => f.Fahrgestellnummer == vin);
            if (fzg == null)
                return;

            fzg.IsSelected = select;
            allSelectionCount = Unfallmeldungen.Count(c => c.IsSelected);
        }

        public void SelectUnfallmeldungen(bool select, Predicate<Unfallmeldung> filter, out int allSelectionCount, out int allCount, out int allFoundCount)
        {
            Unfallmeldungen.Where(f => filter(f)).ToListOrEmptyList().ForEach(f => f.IsSelected = select);

            allSelectionCount = Unfallmeldungen.Count(c => c.IsSelected);
            allCount = Unfallmeldungen.Count();
            allFoundCount = Unfallmeldungen.Count(c => c.IsValidForCancellation);
        }

        public void FilterUnfallmeldungen(string filterValue, string filterProperties)
        {
            UnfallmeldungenFiltered = Unfallmeldungen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }
    }
}
