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
        public ChartItemsPackage NameNotRelevant07()
        {
            var selector = new StrafzettelSelektor
            {
                EingangsDatumRange = new DateRange(DateRangeType.Last6Months, true)
            };
            DashboardSessionSaveCurrentReportSelector(selector);

            var items = DataService.GetStrafzettel(selector);


            Func<DateTime, string> xAxisKeyFormat = (itemKey => itemKey.ToString("yyyyMM"));
            Func<StrafzettelModel, DateTime> xAxisKeyModel = (groupKey => groupKey.EingangsDatum.ToFirstDayOfMonth());

            return ChartService.GetBarChartGroupedStackedItemsWithLabels(
                    items,
                    xAxisKey => xAxisKeyFormat(xAxisKeyModel(xAxisKey)),
                    xAxisList => xAxisList.Insert(0, xAxisKeyFormat(items.Min(xAxisKeyModel).AddMonths(-1)))
                );
        }

        #endregion
    }
}
