using System.Collections;
using System.Linq;
using System.Web.Mvc;
using CkgDomainLogic.Charts.Contracts;
using CkgDomainLogic.Charts.Models;
using CkgDomainLogic.Charts.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using GeneralTools.Contracts;
using Telerik.Web.Mvc;

namespace ServicesMvc.Controllers
{
    public class ChartsController : CkgDomainController 
    {
        public override string DataContextKey { get { return GetDataContextKey<ChartsViewModel>(); } }

        public ChartsViewModel ViewModel { get { return GetViewModel<ChartsViewModel>(); } }


        public ChartsController(IAppSettings appSettings, ILogonContextDataService logonContext, IChartsDataService logsDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, logsDataService);
        }


        [CkgApplication]
        public ActionResult Kba()
        {
            ViewModel.DataInit();

            return View(ViewModel.ChartViewModels.First());
        }

        [CkgApplication]
        public ActionResult Index()
        {
            ViewModel.DataInit();

            return View(ViewModel);
        }


        #region Charts

        [HttpPost]
        public ActionResult ChartFilter(ChartDataSelector selector)
        {
            var chartViewModel = ViewModel.GetChartViewModel(selector.ChartID);

            if (ModelState.IsValid)
            {
                chartViewModel.ChartDataSelector.Apply(selector);
            }

            return PartialView("Partial/ChartFilter", chartViewModel.ChartDataSelector);
        }

        [HttpPost]
        public JsonResult ChartShow(string chartID)
        {
            var chartViewModel = ViewModel.GetChartViewModel(chartID);

            chartViewModel.DataMarkForRefresh();

            return Json(new
                {
                    chartViewModel.GroupChartItems,
                    chartViewModel.GroupChartJahrItems,
                    chartViewModel.GroupChartKey1Items,
                    chartViewModel.AdditionalChartItemLists
                });
        }

        #endregion


        #region Details Data

        [CkgApplication]
        public ActionResult Test()
        {
            ViewModel.SetCurrentChartViewModel(TmpGetCurrentChartViewModel().ChartID);
            ViewModel.CurrentChartViewModel.LoadDetailsData(null, null);

            return View(ViewModel.CurrentChartViewModel);
        }

        SingleChartViewModel TmpGetCurrentChartViewModel() { return ViewModel.ChartViewModels.First(); }

        [HttpPost]
        public ActionResult ChartShowDetailsData(string chartID, string group, string subGroup)
        {
            ViewModel.SetCurrentChartViewModel(chartID);
            if (ViewModel.CurrentChartViewModel == null)
                return new EmptyResult();

            ViewModel.CurrentChartViewModel.LoadDetailsData(group, subGroup);

            return PartialView("Partial/ChartDetailsData", ViewModel.CurrentChartViewModel);
        }

        [GridAction]
        public ActionResult DetailsDataAjaxBinding()
        {
            return View(new GridModel(ViewModel.CurrentChartViewModel.DetailsDataFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridChartDetailsData(string filterValue, string filterColumns)
        {
            ViewModel.CurrentChartViewModel.FilterDetailsData(filterValue, filterColumns);

            return new EmptyResult();
        }


        #region Export

        protected override IEnumerable GetGridExportData()
        {
            return ViewModel.CurrentChartViewModel.DetailsDataFiltered;
        }

        #endregion

        #endregion
    }
}
