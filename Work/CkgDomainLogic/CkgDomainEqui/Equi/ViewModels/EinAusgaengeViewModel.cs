// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Equi.Models;
using System.Linq;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;
// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.Equi.ViewModels
{
    [DashboardProviderViewModel]
    public class EinAusgaengeViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IBriefbestandDataService DataService { get { return CacheGet<IBriefbestandDataService>(); } }

        public EinAusgangSelektor EinAusgangSelektor
        {
            get { return PropertyCacheGet(() => new EinAusgangSelektor()); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<Fahrzeugbrief> EinAusgaenge
        {
            get { return PropertyCacheGet(() => new List<Fahrzeugbrief>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<Fahrzeugbrief> EinAusgaengeFiltered
        {
            get { return PropertyCacheGet(() => EinAusgaenge); }
            private set { PropertyCacheSet(value); }
        }

        public void DataMarkForRefresh(bool refreshSelektor = true)
        {
            PropertyCacheClear(this, m => m.EinAusgaengeFiltered);

            if (refreshSelektor)
                PropertyCacheClear(this, m => m.EinAusgangSelektor);
        }

        public void LoadEinAusgaenge(EinAusgangSelektor model, Action<string, string> addModelError)
        {
            EinAusgaenge = DataService.GetEinAusgaenge(model);

            if (EinAusgaenge.None())
                addModelError("", Localize.NoDataFound);

            DataMarkForRefresh(false);
        }

        public void FilterEinAusgaenge(string filterValue, string filterProperties)
        {
            EinAusgaengeFiltered = EinAusgaenge.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }


        #region Dashboard functionality

        [DashboardItemsLoadMethod("ZBIIEingaengeLetzte6Monate")]
        public ChartItemsPackage NameNotRelevant01()
        {
            return GetChartForZBIIEingaenge(new DateRange(DateRangeType.Last6Months, true));
        }

        [DashboardItemsLoadMethod("ZBIIEingaengeDiesesJahrQuartal1")]
        public ChartItemsPackage NameNotRelevant02()
        {
            var currentYear = DateTime.Today.Year;
            return GetChartForZBIIEingaenge(new DateRange(new DateTime(currentYear, 1, 1), new DateTime(currentYear, 3, 31), true));
        }

        [DashboardItemsLoadMethod("ZBIIEingaengeDiesesJahr")]
        public ChartItemsPackage NameNotRelevant03()
        {
            return GetChartForZBIIEingaenge(new DateRange(DateRangeType.CurrentYear, true));
        }

        [DashboardItemsLoadMethod("ZBIIEingaengeLetztesJahr")]
        public ChartItemsPackage NameNotRelevant04()
        {
            return GetChartForZBIIEingaenge(new DateRange(DateRangeType.LastYear, true));
        }

        private ChartItemsPackage GetChartForZBIIEingaenge(DateRange dateRange)
        {
            var selector = new EinAusgangSelektor
            {
                FilterEinAusgangsTyp = "Inputs",
                DatumRange = dateRange
            };
            DashboardSessionSaveCurrentReportSelector(selector);

            var items = DataService.GetEinAusgaenge(selector).OrderBy(s => s.Eingangsdatum).ToListOrEmptyList();


            Func<DateTime, string> xAxisKeyFormat = (itemKey => itemKey.ToString("yyyyMM"));
            Func<Fahrzeugbrief, DateTime> xAxisKeyModel = (groupKey => groupKey.Eingangsdatum.ToFirstDayOfMonth());

            return ChartService.GetBarChartGroupedStackedItemsWithLabels(
                    items,
                    xAxisKey => xAxisKeyFormat(xAxisKeyModel(xAxisKey)),
                    xAxisList => xAxisList.Insert(0, xAxisKeyFormat(items.Min(xAxisKeyModel).AddMonths(-1)))
                );
        }

        #endregion
    }
}
