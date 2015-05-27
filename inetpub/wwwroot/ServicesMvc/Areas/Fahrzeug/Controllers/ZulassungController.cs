using System.Collections;
using System.Web.Mvc;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.ViewModels;
using GeneralTools.Contracts;
using GeneralTools.Models;
using Telerik.Web.Mvc;

namespace ServicesMvc.Fahrzeug.Controllers
{
    public class ZulassungController : CkgDomainController 
    {
        public override string DataContextKey { get { return GetDataContextKey<ZulassungViewModel>(); } }

        public ZulassungViewModel ViewModel 
        { 
            get { return GetViewModel<ZulassungViewModel>(); } 
            set { SetViewModel(value); } 
        }

        public ZulassungController(IAppSettings appSettings, ILogonContextDataService logonContext,
            IFahrzeugeDataService fahrzeugeDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, fahrzeugeDataService);
            InitModelStatics();
        }

        [CkgApplication]
        public ActionResult Index()
        {
            ViewModel.DataInit();

            return View(ViewModel);
        }

        static void InitModelStatics()
        {
        }


        #region Fahrzeug Auswahl

        [GridAction]
        public ActionResult FahrzeugAuswahlAjaxBinding()
        {
            var items = ViewModel.FahrzeugeFiltered;

            return View(new GridModel(items));
        }

        [HttpPost]
        public ActionResult FilterGridFahrzeugAuswahl(string filterValue, string filterColumns)
        {
            ViewModel.FilterFahrzeuge(filterValue, filterColumns);

            return new EmptyResult();
        }

        [HttpPost]
        public JsonResult FahrzeugAuswahlSelectionChanged(string vin, bool isChecked)
        {
            int allSelectionCount, allCount = 0;
            if (vin.IsNullOrEmpty())
                ViewModel.SelectFahrzeuge(isChecked, f => true, out allSelectionCount, out allCount);
            else
                ViewModel.SelectFahrzeug(vin, isChecked, out allSelectionCount);

            return Json(new
            {
                allSelectionCount, allCount,
                zulassungenAnzahlPdiTotal = ViewModel.ZulassungenAnzahlPdiTotal,
                zulassungenAnzahlGesamtTotal = ViewModel.ZulassungenAnzahlGesamtTotal,
            });
        }

        [HttpPost]
        public JsonResult OnChangeFilterValues(string type, string value)
        {
            ViewModel.OnChangeFilterValues(type, value);

            return Json(new
            {
                zulassungenAnzahlPdiTotal = ViewModel.ZulassungenAnzahlPdiTotal,
                zulassungenAnzahlGesamtTotal = ViewModel.ZulassungenAnzahlGesamtTotal,
            });
        }

        [HttpPost]
        public JsonResult OnChangePresetValues(string type, string value)
        {
            var errorMessage = ViewModel.OnChangePresetValues(type, ref value);

            return Json(new
            {
                value, errorMessage,
                zulassungenAnzahlPdiTotal = ViewModel.ZulassungenAnzahlPdiTotal,
                zulassungenAnzahlGesamtTotal = ViewModel.ZulassungenAnzahlGesamtTotal,
            });
        }

        #endregion    


        #region Export

        protected override IEnumerable GetGridExportData()
        {
            return ViewModel.FahrzeugeFiltered;
        }

        #endregion
    }
}
