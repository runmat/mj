// ReSharper disable ImplicitlyCapturedClosure
// ReSharper disable RedundantUsingDirective

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using System.Web.Mvc;

using CkgDomainLogic.AppUserOverview.Contracts;
using CkgDomainLogic.AppUserOverview.Models;

using GeneralTools.Models;
using System.IO;
using CkgDomainLogic.General.Database.Services;
using GeneralTools.Services;
using ServicesMvc.AppUserOverview.Models;

// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.AppUserOverview.ViewModels
{
    [DashboardProviderViewModel]
    public class AppUserOverviewViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IAppUserOverviewDataService DataService { get { return CacheGet<IAppUserOverviewDataService>(); } }

        public AppUserOverviewSelektor AppUserOverviewSelektor
        {
            get { return PropertyCacheGet(() => new AppUserOverviewSelektor()); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<AppUserOverviewResults> AppUserOverviewList
        {
            get { return PropertyCacheGet(() => new List<AppUserOverviewResults>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<AppUserOverviewResults> AppUserOverviewFiltered
        {
            get { return PropertyCacheGet(() => AppUserOverviewList); }
            private set { PropertyCacheSet(value); }
        }

        public void DataInit()
        {
            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.AppUserOverviewFiltered);
        }

        public void Validate(Action<string, string> addModelError)
        {       
        }

        public void LoadAppUserOverview()
        {
            // Datenbeschaffung über DataService...
            AppUserOverviewList = DataService.GetGridData(AppUserOverviewSelektor, LogonContext);

            DataMarkForRefresh();
        }

        public void FilterAppUserOverview(string filterValue, string filterProperties)
        {
            AppUserOverviewFiltered = AppUserOverviewList.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #region Dashboard functionality

        //[DashboardItemsLoadMethod("AppUserOverviewAlleKunden")]
        //public ChartItemsPackage NameNotRelevant07()
        //{
        //    var selector = new AppUserOverviewSelektor
        //    {
        //        // EingangsDatumRange = new DateRange(DateRangeType.Last6Months, true)
        //    };
        //    DashboardSessionSaveCurrentReportSelector(selector);

        //    var items = DataService.GetAppUserOverview(selector);


        //    Func<DateTime, string> xAxisKeyFormat = (itemKey => itemKey.ToString("yyyyMM"));
        //    Func<AppUserOverviewModel, DateTime> xAxisKeyModel = (groupKey => groupKey.EingangsDatum.ToFirstDayOfMonth());

        //    return ChartService.GetBarChartGroupedStackedItemsWithLabels(
        //            items,
        //            xAxisKey => xAxisKeyFormat(xAxisKeyModel(xAxisKey)),
        //            xAxisList => xAxisList.Insert(0, xAxisKeyFormat(items.Min(xAxisKeyModel).AddMonths(-1)))
        //        );
        //}

        #endregion
    }
}
