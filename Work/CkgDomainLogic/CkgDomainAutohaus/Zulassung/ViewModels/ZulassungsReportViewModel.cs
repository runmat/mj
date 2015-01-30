﻿// ReSharper disable ImplicitlyCapturedClosure
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
        [XmlIgnore, ScriptIgnore]
        public IZulassungDataService DataService { get { return CacheGet<IZulassungDataService>(); } }

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

        [XmlIgnore, ScriptIgnore]
        public List<Kunde> Kunden { get { return DataService.Kunden; } }

        [XmlIgnore]
        public static string AuftragsArtOptionen
        {
            get
            {
                return string.Format("1,{0};2,{1};3,{2}", Localize.AllOrders, Localize.FinishedOrders, Localize.OpenOrders);
            }
        }


        public void DataInit()
        {
            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.ItemsFiltered);
        }

        public void Validate(Action<string, string> addModelError)
        {
        }

        private List<ZulassungsReportModel> GetAllItems(ZulassungsReportSelektor selector, Action<string, string> addModelError)
        {
            var items = new List<ZulassungsReportModel>();
            if (selector.KundenNr.IsNotNullOrEmpty())
                items = DataService.GetZulassungsReportItems(selector, Kunden, addModelError);
            else
            {
                foreach (var kunde in Kunden)
                {
                    selector.KundenNr = kunde.KundenNr;
                    items = items.Concat(DataService.GetZulassungsReportItems(selector, Kunden, addModelError)).ToListOrEmptyList();
                }
            }

            return items;
        }

        public void LoadZulassungsReport(Action<string, string> addModelError)
        {
            Items = GetAllItems(Selektor, addModelError);

            DataMarkForRefresh();
        }

        [DashboardItemsLoadMethod("ZulassungenProKundeUndMonat")]
        public ChartItemsPackage NameNotRelevant01()
        {
            var selector = new ZulassungsReportSelektor
            {
                ZulassungsDatumRange = new DateRange(DateRangeType.Last90Days, true)
            };

            var items = GetAllItems(selector, null);


            Func<string, string> stackedKeyFormat = (itemKey => itemKey.Trim('0'));
            Func<DateTime, string> xAxisKeyFormat = (itemKey => itemKey.ToString("yyyyMM"));
            Func<ZulassungsReportModel, DateTime> xAxisKeyModel = (groupKey => groupKey.ZulassungDatum.ToFirstDayOfMonth());

            var stackedGroupValues = items.GroupBy(k => stackedKeyFormat(k.KundenNr)).OrderBy(k => k.Key).Select(k => k.Key);

            return ChartService.GetGroupedAndStackedItemsWithLabels(
                    items, stackedGroupValues,
                    stackedKey => stackedKeyFormat(stackedKey.KundenNr),
                    xAxisKey => xAxisKeyFormat(xAxisKeyModel(xAxisKey)),
                    xAxisList => xAxisList.Insert(0, xAxisKeyFormat(items.Min(xAxisKeyModel).AddMonths(-1)))
                );
        }
        [DashboardItemsLoadMethod("ZulassungenUmsatzProKundeUndMonat")]
        public ChartItemsPackage NameNotRelevant02()
        {
            var selector = new ZulassungsReportSelektor
            {
                ZulassungsDatumRange = new DateRange(DateRangeType.Last90Days, true)
            };

            var items = GetAllItems(selector, null);


            Func<string, string> stackedKeyFormat = (itemKey => itemKey.Trim('0'));
            Func<DateTime, string> xAxisKeyFormat = (itemKey => itemKey.ToString("yyyyMM"));
            Func<ZulassungsReportModel, DateTime> xAxisKeyModel = (groupKey => groupKey.ZulassungDatum.ToFirstDayOfMonth());
            Func<IGrouping<int, ZulassungsReportModel>, int> aggregate = (g => (int) g.Sum(item => item.Preis));

            var stackedGroupValues = items.GroupBy(k => stackedKeyFormat(k.KundenNr)).OrderBy(k => k.Key).Select(k => k.Key);

            return ChartService.GetGroupedAndStackedItemsWithLabels(
                    items, stackedGroupValues,
                    stackedKey => stackedKeyFormat(stackedKey.KundenNr),
                    xAxisKey => xAxisKeyFormat(xAxisKeyModel(xAxisKey)),
                    xAxisList => xAxisList.Insert(0, xAxisKeyFormat(items.Min(xAxisKeyModel).AddMonths(-1))),
                    aggregate
                );
        }

        [DashboardItemsLoadMethod("ZulassungenProKundeUndWoche")]
        public ChartItemsPackage NameNotRelevant03()
        {
            var selector = new ZulassungsReportSelektor
            {
                ZulassungsDatumRange = new DateRange(DateRangeType.Last60Days, true)
            };

            var items = GetAllItems(selector, null);

            Func<string, string> stackedKeyFormat = (itemKey => itemKey.Trim('0'));
            Func<DateTime, string> xAxisKeyFormat = (itemKey => itemKey.FormatYearAndWeek("yy"));
            Func<ZulassungsReportModel, DateTime> xAxisKeyModel = (groupKey => groupKey.ZulassungDatum.ToFirstDayOfWeek());

            var stackedGroupValues = items.GroupBy(k => stackedKeyFormat(k.KundenNr)).OrderBy(k => k.Key).Select(k => k.Key);

            return ChartService.GetGroupedAndStackedItemsWithLabels(
                    items, stackedGroupValues,
                    stackedKey => stackedKeyFormat(stackedKey.KundenNr),
                    xAxisKey => xAxisKeyFormat(xAxisKeyModel(xAxisKey)),
                    xAxisList =>
                        {
                            xAxisList.Insert(0, xAxisKeyFormat(items.Min(xAxisKeyModel).AddDays(-7)));
                            xAxisList.Insert(0, xAxisKeyFormat(items.Min(xAxisKeyModel).AddDays(-14)));
                            xAxisList.Insert(0, xAxisKeyFormat(items.Min(xAxisKeyModel).AddDays(-21)));
                        }
                );
        }

        [DashboardItemsLoadMethod("ZulassungenAlleKunden")]
        public ChartItemsPackage NameNotRelevant04()
        {
            var selector = new ZulassungsReportSelektor
            {
                ZulassungsDatumRange = new DateRange(DateRangeType.Last90Days, true)
            };

            var items = GetAllItems(selector, null);


            Func<DateTime, string> xAxisKeyFormat = (itemKey => itemKey.ToString("yyyyMM"));
            Func<ZulassungsReportModel, DateTime> xAxisKeyModel = (groupKey => groupKey.ZulassungDatum.ToFirstDayOfMonth());

            var stackedGroupValues = new[]{ "all" };

            return ChartService.GetGroupedAndStackedItemsWithLabels(
                    items, stackedGroupValues,
                    stackedKey => "all",
                    xAxisKey => xAxisKeyFormat(xAxisKeyModel(xAxisKey)),
                    xAxisList => xAxisList.Insert(0, xAxisKeyFormat(items.Min(xAxisKeyModel).AddMonths(-1)))
                );
        }

        public void FilterZulassungsReport(string filterValue, string filterProperties)
        {
            ItemsFiltered = Items.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }
    }
}
