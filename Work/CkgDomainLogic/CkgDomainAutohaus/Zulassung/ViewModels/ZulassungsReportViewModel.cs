// ReSharper disable ImplicitlyCapturedClosure
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Autohaus.Contracts;
using CkgDomainLogic.Autohaus.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Autohaus.ViewModels
{
    [DashboardProviderViewModel]
    public class ZulassungsReportViewModel : CkgBaseViewModel
    {
        #region Main functionality

        [XmlIgnore, ScriptIgnore]
        public IZulassungDataService DataService
        {
            get { return CacheGet<IZulassungDataService>(); }
        }

        public ZulassungsReportSelektor Selektor
        {
            get { return PropertyCacheGet(() => new ZulassungsReportSelektor()); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<ZulassungsReportModel> Items
        {
            get { return PropertyCacheGet(() => new List<ZulassungsReportModel>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<ZulassungsReportModel> ItemsFiltered
        {
            get { return PropertyCacheGet(() => Items); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<FahrzeugStatus> FahrzeugStatusWerte
        {
            get { return PropertyCacheGet(() => DataService.FahrzeugStatusWerte); }
        }

        [XmlIgnore]
        public List<Kunde> Kunden { get { return PropertyCacheGet(() => DataService.Kunden); } }

        [XmlIgnore]
        public static string AuftragsArtOptionen
        {
            get
            {
                return string.Format("1,{0};2,{1};3,{2}", Localize.AllOrders, Localize.FinishedOrders,
                                     Localize.OpenOrders);
            }
        }


        public void DataInit()
        {
            DataMarkForRefresh(true);
        }

        public void DataMarkForRefresh(bool refreshStammdaten = false)
        {
            PropertyCacheClear(this, m => m.ItemsFiltered);

            if (refreshStammdaten)
            {
                PropertyCacheClear(this, m => m.Kunden);
            }
        }

        public void Validate(Action<string, string> addModelError)
        {
        }

        private List<ZulassungsReportModel> GetAllItems(ZulassungsReportSelektor selector, Action<string, string> addModelError)
        {
            return DataService.GetZulassungsReportItems(selector, Kunden, addModelError);
        }

        public void LoadZulassungsReport(Action<string, string> addModelError)
        {
            Items = GetAllItems(Selektor, addModelError);

            DataMarkForRefresh();
        }

        public void FilterZulassungsReport(string filterValue, string filterProperties)
        {
            ItemsFiltered = Items.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion


        #region Dashboard functionality

        [DashboardItemsLoadMethod("ZulassungenUmsatzProKundeUndMonat")]
        public ChartItemsPackage NameNotRelevant02()
        {
            var selector = new ZulassungsReportSelektor
            {
                ZulassungsDatumRange = new DateRange(DateRangeType.CurrentYear, true)
            };
            DashboardSessionSaveCurrentReportSelector(selector);

            var items = GetAllItems(selector, null);


            Func<ZulassungsReportModel, string> stackedKey = (item => item.KundenNr.Trim('0'));
            Func<DateTime, string> xAxisKeyFormat = (itemKey => itemKey.ToString("MMM"));
            Func<ZulassungsReportModel, DateTime> xAxisKeyModel = (groupKey => groupKey.ZulassungDatum.ToFirstDayOfMonth());
            Func<IGrouping<int, ZulassungsReportModel>, int> aggregate = (g => (int)g.Sum(item => item.Preis.GetValueOrDefault()));

            return ChartService.GetBarChartGroupedStackedItemsWithLabels(
                    items, 
                    xAxisKey => xAxisKeyFormat(xAxisKeyModel(xAxisKey)),
                    xAxisList => xAxisList.Insert(0, xAxisKeyFormat(items.Min(xAxisKeyModel).AddMonths(-1))),
                    stackedKey,
                    aggregate
                );
        }

        [DashboardItemsLoadMethod("ZulassungenProKundeUndMonat")]
        public ChartItemsPackage NameNotRelevant01()
        {
            var selector = new ZulassungsReportSelektor
            {
                ZulassungsDatumRange = new DateRange(DateRangeType.CurrentYear, true)
            };
            DashboardSessionSaveCurrentReportSelector(selector);

            var items = GetAllItems(selector, null);


            Func<ZulassungsReportModel, string> stackedKey = (item => item.KundenNr.Trim('0'));
            Func<DateTime, string> xAxisKeyFormat = (itemKey => itemKey.ToString("MMM"));
            Func<ZulassungsReportModel, DateTime> xAxisKeyModel = (groupKey => groupKey.ZulassungDatum.ToFirstDayOfMonth());

            return ChartService.GetBarChartGroupedStackedItemsWithLabels(
                    items, 
                    xAxisKey => xAxisKeyFormat(xAxisKeyModel(xAxisKey)),
                    xAxisList => xAxisList.Insert(0, xAxisKeyFormat(items.Min(xAxisKeyModel).AddMonths(-1))),
                    stackedKey
                );
        }

        [DashboardItemsLoadMethod("ZulassungenProKundeGesamtPie")]
        public ChartItemsPackage NameNotRelevant05()
        {
            var selector = new ZulassungsReportSelektor
            {
                ZulassungsDatumRange = new DateRange(DateRangeType.Last90Days, true)
            };
            DashboardSessionSaveCurrentReportSelector(selector);

            var items = GetAllItems(selector, null);


            Func<ZulassungsReportModel, string> xAxisKeyModel = (groupKey => groupKey.KundenNr.Trim('0'));

            return ChartService.GetPieChartGroupedItemsWithLabels(
                    items,
                    xAxisKeyModel
                );
        }

        [DashboardItemsLoadMethod("ZulassungenProMonatGesamtPie")]
        public ChartItemsPackage NameNotRelevant07()
        {
            var selector = new ZulassungsReportSelektor
            {
                ZulassungsDatumRange = new DateRange(DateRangeType.Last90Days, true)
            };
            DashboardSessionSaveCurrentReportSelector(selector);

            var items = GetAllItems(selector, null);


            Func<DateTime, string> xAxisKeyFormat = (itemKey => itemKey.ToString("MMM"));
            Func<ZulassungsReportModel, DateTime> xAxisKeyModel = (groupKey => groupKey.ZulassungDatum.ToFirstDayOfMonth());

            return ChartService.GetPieChartGroupedItemsWithLabels(
                    items,
                    xAxisKey => xAxisKeyFormat(xAxisKeyModel(xAxisKey))
                );
        }

        [DashboardItemsLoadMethod("ZulassungenProMaterialLetzte30TagePie")]
        public ChartItemsPackage NameNotRelevant08()
        {
            var selector = new ZulassungsReportSelektor
            {
                ZulassungsDatumRange = new DateRange(DateRangeType.Last30Days, true),
                NurHauptDienstleistungen = true
            };
            DashboardSessionSaveCurrentReportSelector(selector);

            var items = GetAllItems(selector, null);

            Func<ZulassungsReportModel, string> getTitleFunc = (groupKey => groupKey.MaterialKurztext.NotNullOrEmpty().Crop(30));
            Func<ZulassungsReportModel, string> xAxisKeyModel = (groupKey => getTitleFunc(groupKey));

            double countTotal = items.Count;
            if (countTotal > 0)
            {
                var percentThreshold = 1.0;
                var validMaterialTexts = items.GroupBy(xAxisKeyModel).Where(g => g.Count(i => true) / countTotal*100.0 > percentThreshold).Select(g => g.Key);

                xAxisKeyModel = (groupKey => validMaterialTexts.Contains(getTitleFunc(groupKey)) ? getTitleFunc(groupKey) : " Sonstige");
            }
            return ChartService.GetPieChartGroupedItemsWithLabels(
                    items,
                    xAxisKeyModel
                );
        }

        [DashboardItemsLoadMethod("ZulassungenProKundeGesamtBar")]
        public ChartItemsPackage NameNotRelevant06()
        {
            var selector = new ZulassungsReportSelektor
            {
                ZulassungsDatumRange = new DateRange(DateRangeType.Last90Days, true)
            };
            DashboardSessionSaveCurrentReportSelector(selector);

            var items = GetAllItems(selector, null);


            Func<ZulassungsReportModel, string> xAxisKeyModel = (groupKey => groupKey.KundenNr.Trim('0'));

            return ChartService.GetBarChartGroupedStackedItemsWithLabels(
                    items,
                    xAxisKeyModel,
                    xAxisList => xAxisList.Insert(0, "")
                );
        }

        [DashboardItemsLoadMethod("ZulassungenProKundeUndWoche")]
        public ChartItemsPackage NameNotRelevant03()
        {
            var selector = new ZulassungsReportSelektor
                {
                    ZulassungsDatumRange = new DateRange(DateRangeType.Last30Days, true)
                };
            DashboardSessionSaveCurrentReportSelector(selector);

            var items = GetAllItems(selector, null);


            Func<ZulassungsReportModel, string> stackedKey = (item => item.KundenNr.Trim('0'));
            Func<DateTime, string> xAxisKeyFormat = (itemKey => itemKey.FormatYearAndWeek("yy"));
            Func<ZulassungsReportModel, DateTime> xAxisKeyModel = (groupKey => groupKey.ZulassungDatum.ToFirstDayOfWeek());

            return ChartService.GetBarChartGroupedStackedItemsWithLabels(
                    items, 
                    xAxisKey => xAxisKeyFormat(xAxisKeyModel(xAxisKey)),
                    xAxisList =>
                        {
                            xAxisList.Insert(0, xAxisKeyFormat(items.Min(xAxisKeyModel).AddDays(-7)));
                            xAxisList.Insert(0, xAxisKeyFormat(items.Min(xAxisKeyModel).AddDays(-14)));
                        },
                    stackedKey
                );
        }

        [DashboardItemsLoadMethod("ZulassungenAlleKunden2015")]
        public ChartItemsPackage NameNotRelevant04()
        {
            var selector = new ZulassungsReportSelektor
                {
                    ZulassungsDatumRange = new DateRange(new DateTime(2015, 01, 01), new DateTime(2015, 12, 31), true)
                };
            DashboardSessionSaveCurrentReportSelector(selector);

            var items = GetAllItems(selector, null);
            items = items.OrderBy(item => item.ZulassungDatum).ToListOrEmptyList();

            Func<DateTime, string> xAxisKeyFormat = (itemKey => itemKey.ToString("MMM"));
            Func<ZulassungsReportModel, DateTime> xAxisKeyModel = (groupKey => groupKey.ZulassungDatum.ToFirstDayOfMonth());

            return ChartService.GetBarChartGroupedStackedItemsWithLabels(
                    items, 
                    xAxisKey => xAxisKeyFormat(xAxisKeyModel(xAxisKey))
                    //, xAxisList => xAxisList.Insert(0, xAxisKeyFormat(items.Min(xAxisKeyModel).AddMonths(-1)))
                );
        }

        #endregion
    }
}
