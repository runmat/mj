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

            if (refreshSelektor && !DashboardCurrentReportSelectorAvailable)
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

        //
        // ZBII Eingänge
        //

        [DashboardItemsLoadMethod("ZBIIEingaengeDiesesJahr")]
        public ChartItemsPackage NameNotRelevant03()
        {
            return GetMonthChartForZBII(new DateRange(DateRangeType.CurrentYear, true), "Inputs");
        }

        [DashboardItemsLoadMethod("ZBIIEingaengeLetztesJahr")]
        public ChartItemsPackage NameNotRelevant04()
        {
            return GetMonthChartForZBII(new DateRange(DateRangeType.LastYear, true), "Inputs");
        }

        [DashboardItemsLoadMethod("ZBIIEingaengeDiesesJahrNachHersteller")]
        public ChartItemsPackage NameNotRelevant13()
        {
            return GetVehicleChartForZBII(new DateRange(DateRangeType.CurrentYear, true), "Inputs");
        }

        [DashboardItemsLoadMethod("ZBIIEingaengeLetztesJahrNachHersteller")]
        public ChartItemsPackage NameNotRelevant14()
        {
            return GetVehicleChartForZBII(new DateRange(DateRangeType.LastYear, true), "Inputs");
        }

        [DashboardItemsLoadMethod("ZBIIAusgaengeDiesesJahr")]
        public ChartItemsPackage NameNotRelevant06()
        {
            return GetMonthChartForZBII(new DateRange(DateRangeType.CurrentYear, true), "Outputs");
        }


        //
        // ZBII Ausgänge
        //

        [DashboardItemsLoadMethod("ZBIIAusgaengeLetztesJahr")]
        public ChartItemsPackage NameNotRelevant07()
        {
            return GetMonthChartForZBII(new DateRange(DateRangeType.LastYear, true), "Outputs");
        }

        [DashboardItemsLoadMethod("ZBIIAusgaengeDiesesJahrNachHersteller")]
        public ChartItemsPackage NameNotRelevant16()
        {
            return GetVehicleChartForZBII(new DateRange(DateRangeType.CurrentYear, true), "Outputs");
        }

        [DashboardItemsLoadMethod("ZBIIAusgaengeLetztesJahrNachHersteller")]
        public ChartItemsPackage NameNotRelevant17()
        {
            return GetVehicleChartForZBII(new DateRange(DateRangeType.LastYear, true), "Outputs");
        }

        private ChartItemsPackage GetMonthChartForZBII(DateRange dateRange, string einAusgangsTyp)
        {
            var eingang = einAusgangsTyp == "Inputs";

            var selector = new EinAusgangSelektor
            {
                FilterEinAusgangsTyp = einAusgangsTyp,
                DatumRange = dateRange
            };
            DashboardSessionSaveCurrentReportSelector(selector);

            Func<Fahrzeugbrief, DateTime?> getDateProperty = (item => eingang ? item.Eingangsdatum : item.Versanddatum);
            Func<DateTime, string> xAxisKeyFormat = (itemKey => itemKey.ToString("MMM"));
            Func<Fahrzeugbrief, DateTime> xAxisKeyModel = (groupKey => getDateProperty(groupKey).ToFirstDayOfMonth());

            var items = DataService.GetEinAusgaenge(selector).OrderBy(s => getDateProperty(s)).ToListOrEmptyList();

            return ChartService.GetBarChartGroupedStackedItemsWithLabels(
                    items,
                    xAxisKey => xAxisKeyFormat(xAxisKeyModel(xAxisKey))
                );
        }

        private ChartItemsPackage GetVehicleChartForZBII(DateRange dateRange, string einAusgangsTyp)
        {
            var selector = new EinAusgangSelektor
            {
                FilterEinAusgangsTyp = einAusgangsTyp,
                DatumRange = dateRange
            };
            DashboardSessionSaveCurrentReportSelector(selector);

            Func<string, string> xAxisKeyFormat = (itemKey => itemKey);
            Func<Fahrzeugbrief, string> xAxisKeyModel = (groupKey =>
            {
                var herst = groupKey.FahrzeugHersteller.NotNullOrEmpty().ToUpper();

                if (herst.Contains("BAYER.MOT."))
                    return " BMW ";
                if (herst.Contains("VOLKSWAGEN"))
                    return " VW ";
                if (herst.Contains("VOLVO"))
                    return " VOLVO ";
                if (herst.Contains("AUDI"))
                    return " AUDI ";
                if (herst.Contains("FORD"))
                    return " FORD ";
                if (herst.Contains("OPEL"))
                    return " OPEL ";
                if (herst.Contains("DAIMLER"))
                    return " Daimler ";

                return "Sonstige";
            });

            var items = DataService.GetEinAusgaenge(selector).OrderBy(xAxisKeyModel).ToListOrEmptyList();

            return ChartService.GetBarChartGroupedStackedItemsWithLabels(
                    items,
                    xAxisKey => xAxisKeyFormat(xAxisKeyModel(xAxisKey))
                );
        }

        #endregion
    }
}
