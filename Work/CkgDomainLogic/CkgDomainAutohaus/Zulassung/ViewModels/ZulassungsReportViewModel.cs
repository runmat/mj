using System;
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
        //public object ReportSelector { get { return Selektor; } set { Selektor = (ZulassungsReportSelektor)value; } }

        //public IEnumerable ReportItems { get { return Items; } }

        //public void ReportItemsLoad(Action<string, string> addModelError)
        //{
        //    LoadZulassungsReport(addModelError);
        //}

        [XmlIgnore, ScriptIgnore]
        public IZulassungDataService DataService { get { return CacheGet<IZulassungDataService>(); } }

        [DashboardItemSelector]
        public ZulassungsReportSelektor Selektor
        {
            get { return PropertyCacheGet(() => new ZulassungsReportSelektor()); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        [DashboardItems]
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

        [DashboardItemsLoadMethod]
        public void LoadZulassungsReport(Action<string, string> addModelError)
        {
            Items = DataService.GetZulassungsReportItems(Selektor, Kunden, addModelError);

            DataMarkForRefresh();
        }

        public void FilterZulassungsReport(string filterValue, string filterProperties)
        {
            ItemsFiltered = Items.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }
    }
}
