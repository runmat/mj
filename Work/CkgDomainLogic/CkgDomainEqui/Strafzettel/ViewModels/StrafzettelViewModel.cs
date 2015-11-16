// ReSharper disable ImplicitlyCapturedClosure
// ReSharper disable RedundantUsingDirective

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Strafzettel.Contracts;
using CkgDomainLogic.Strafzettel.Models;
using GeneralTools.Models;
// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.Strafzettel.ViewModels
{
    [DashboardProviderViewModel]
    public class StrafzettelViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IStrafzettelDataService DataService { get { return CacheGet<IStrafzettelDataService>(); } }

        public StrafzettelSelektor StrafzettelSelektor
        {
            get { return PropertyCacheGet(() => new StrafzettelSelektor()); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<StrafzettelModel> Strafzettel
        {
            get { return PropertyCacheGet(() => new List<StrafzettelModel>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<StrafzettelModel> StrafzettelFiltered
        {
            get { return PropertyCacheGet(() => Strafzettel); }
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
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.StrafzettelFiltered);
        }

        public void Validate(Action<string, string> addModelError)
        {
        }

        public void LoadStrafzettel()
        {
            Strafzettel = DataService.GetStrafzettel(StrafzettelSelektor);

            DataMarkForRefresh();

            //XmlService.XmlSerializeToFile(Strafzettel, Path.Combine(AppSettings.DataPath, @"Strafzettel.xml"));
        }

        public void FilterStrafzettel(string filterValue, string filterProperties)
        {
            StrafzettelFiltered = Strafzettel.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }


        #region Dashboard functionality

        [DashboardItemsLoadMethod("StrafzettelAlleKunden")]
        public ChartItemsPackage NameNotRelevant01()
        {
            var selector = new StrafzettelSelektor
            {
                EingangsDatumRange = new DateRange(DateRangeType.Last6Months, true)
            };
            DashboardSessionSaveCurrentReportSelector(selector);

            var items = DataService.GetStrafzettel(selector)
                                    .OrderBy(s => s.EingangsDatum).ToListOrEmptyList();


            Func<DateTime, string> xAxisKeyFormat = (itemKey => itemKey.ToString("yyyyMM"));
            Func<StrafzettelModel, DateTime> xAxisKeyModel = (groupKey => groupKey.EingangsDatum.ToFirstDayOfMonth());

            return ChartService.GetBarChartGroupedStackedItemsWithLabels(
                    items,
                    xAxisKey => xAxisKeyFormat(xAxisKeyModel(xAxisKey)),
                    xAxisList => xAxisList.Insert(0, xAxisKeyFormat(items.Min(xAxisKeyModel).AddMonths(-1)))
                );
        }

        [DashboardItemsLoadMethod("StrafzettelNachKennzeichenPieDiesesJahr")]
        public ChartItemsPackage NameNotRelevant02()
        {
            return StrafzettelNachKennzeichen(new DateRange(DateRangeType.CurrentYear, true));
        }

        [DashboardItemsLoadMethod("StrafzettelNachKennzeichenPieLetztesJahr")]
        public ChartItemsPackage NameNotRelevant03()
        {
            return StrafzettelNachKennzeichen(new DateRange(DateRangeType.LastYear, true));
        }

        [DashboardItemsLoadMethod("StrafzettelNachKennzeichenPieJanuar2014")]
        public ChartItemsPackage NameNotRelevant04() 
        {
            return StrafzettelNachKennzeichen(new DateRange(new DateTime(2014, 01, 20), new DateTime(2014, 01, 21), true));
        }

        private ChartItemsPackage StrafzettelNachKennzeichen(DateRange dateRange)
        {
            var selector = new StrafzettelSelektor { EingangsDatumRange = dateRange };
            DashboardSessionSaveCurrentReportSelector(selector);

            Func<StrafzettelModel, string> xAxisKeyModel = (groupKey =>
            {
                var kennzeichenLinks = groupKey.Kennzeichen.NotNullOrEmpty().Split('-')[0].ToUpper();
                switch (kennzeichenLinks)
                {
                    case "HN":
                        return "Heilbronn";
                    case "IN":
                        return "Ingolstadt";
                }
                return "Sonstige"; 
            });

            var items = DataService.GetStrafzettel(selector).OrderBy(xAxisKeyModel).ToList();

            return ChartService.GetPieChartGroupedItemsWithLabels(
                    items,
                    xAxisKey => xAxisKeyModel(xAxisKey)
                );
        }

        #endregion
    }
}
