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

        [DashboardItemsLoadMethod("Zulassungen")]
        public DashboardItemsPackage LoadChartDataZulassungen()
        {
            var selector = new ZulassungsReportSelektor
                {
                    ZulassungsDatumRange = new DateRange(DateRangeType.Last90Days, true)
                };

            var items = GetAllItems(selector, null);

            var xAxisArray = items.GroupBy(kunde => kunde.ZulassungDatum.ToFirstDayOfMonth()).OrderBy(k => k.Key).Select(k => k.Key).ToList();
            xAxisArray.Insert(0, xAxisArray.Min().AddMonths(-1));
            var xAxisLabels = xAxisArray.Select(date => date.ToString("yyyyMM")).ToArray();

            var data = new object[Kunden.Count];
            for (var k = 0; k < Kunden.Count; k++)
            {
                var subArray = items
                    .Where(kunde => kunde.KundenNr == Kunden[k].KundenNr)
                    .GroupBy(group => xAxisArray.IndexOf(group.ZulassungDatum.ToFirstDayOfMonth()))
                    .Select(g => new [] { g.Key, g.Count() })
                    .ToArray();

                data[k] = new 
                {
                    data = subArray,
                    label = Kunden[k].KundenNr.Trim('0')
                };
            }

            return new DashboardItemsPackage
                {
                    data = data,
                    labels = xAxisLabels
                };
        }

        [DashboardItemsLoadMethod("Abmeldungen")]
        public DashboardItemsPackage LoadChartDataAbmeldungen()
        {
            var selector = new ZulassungsReportSelektor
            {
                AuftragsDatumRange = new DateRange(DateRangeType.Last90Days, true)
            };

            var items = DataService.GetZulassungsReportItems(selector, Kunden, null);

            items.ForEach(item =>
                {
                    if (item.ZulassungDatum != null)
                        item.ZulassungDatum = item.ZulassungDatum.GetValueOrDefault().AddYears(-5).AddMonths(-6);
                });

            return null;
        }

        public void FilterZulassungsReport(string filterValue, string filterProperties)
        {
            ItemsFiltered = Items.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }
    }
}
