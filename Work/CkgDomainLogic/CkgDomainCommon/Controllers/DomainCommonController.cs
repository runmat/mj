using System.Web.Mvc;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.DomainCommon.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.ViewModels;
using DocumentTools.Services;
using GeneralTools.Contracts;
using GeneralTools.Services;
using MvcTools.Web;
using Telerik.Web.Mvc;
using GeneralTools.Models;

namespace ServicesMvc.Controllers
{
    /// <summary>
    /// Standard Adressenpflege Dialog 
    /// </summary>
    public class DomainCommonController : CkgDomainController
    {
        private readonly int _portalType = 3;   // 1 = Portal, 2 = Services, 3 = ServicesMvc

        public override string DataContextKey { get { return GetDataContextKey<AdressenPflegeViewModel>(); } }

        public AdressenPflegeViewModel AdressenPflegeViewModel { get { return GetViewModel<AdressenPflegeViewModel>(); } }

        public CkgCommonViewModel CkgCommonViewModel { get { return GetViewModel<CkgCommonViewModel>(); } }

        
        public DomainCommonController(IAppSettings appSettings, ILogonContextDataService logonContext, IAdressenDataService adressenDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(AdressenPflegeViewModel, appSettings, logonContext, adressenDataService);
            InitViewModel(CkgCommonViewModel, appSettings, logonContext);
        }


        public ActionResult LogPageVisit(string appID)
        {
            if (appID.IsNullOrEmpty() || LogonContext == null || LogonContext.User == null || LogonContext.Customer == null)
                return new EmptyResult();

            var logService = new LogService();
            logService.LogPageVisit(appID.ToInt(), LogonContext.User.UserID, LogonContext.Customer.CustomerID, LogonContext.Customer.KUNNR.ToInt(), _portalType);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult AppFavoritesEditModeSwitch()
        {
            LogonContext.AppFavoritesEditMode = !LogonContext.AppFavoritesEditMode;

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult AppFavoritesEditSwitchOneFavorite(int appID)
        {
            return Json(new { isFavorite = LogonContext.AppFavoritesEditSwitchOneFavorite(appID) });
        }

        [HttpPost]
        public ActionResult AppFavoriteButtonsCoreRefresh()
        {
            return PartialView("AppFavorites/AppFavoriteButtonsCore", LogonContext);
        }

        [CkgApplication]
        public ActionResult Index()
        {
            if (LogonContext != null && LogonContext.Customer != null && LogonContext.Customer.MvcSelectionUrl.IsNotNullOrEmpty())
                return RedirectPermanent(LogonContext.Customer.MvcSelectionUrl);

            return View(CkgCommonViewModel);
        }

        [CkgApplication]
        public ActionResult Search()
        {
            return RedirectToAction("Index");
        }

        [CkgApplication]
        public ActionResult Kontakt()
        {
            return View(CkgCommonViewModel);
        }

        [CkgApplication]
        public ActionResult UserMessages()
        {
            return View(CkgCommonViewModel);
        }

        [CkgApplication]
        public ActionResult Impressum()
        {
            return View(CkgCommonViewModel);
        }


        #region Adressen Pflege

        [CkgApplication]
        public ActionResult AdressenPflege(string kennung, string kdnr)
        {
            AdressenPflegeViewModel.DataInit(kennung ?? "VERSANDADRESSE", kdnr ?? LogonContext.KundenNr);

            return View(AdressenPflegeViewModel);
        }

        [GridAction]
        public ActionResult AdressenAjaxBinding()
        {
            var items = AdressenPflegeViewModel.AdressenFiltered; 

            return View(new GridModel(items));
        }

        [HttpPost]
        public ActionResult FilterGridAdresse(string filterValue, string filterColumns)
        {
            AdressenPflegeViewModel.FilterVersandAdressen(filterValue, filterColumns);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult EditAddress(int id)
        {
            AdressenPflegeViewModel.InsertMode = false;
            ModelState.Clear();
            return PartialView("AdressenPflege/AdressenDetailsForm", AdressenPflegeViewModel.GetItem(id).SetInsertMode(AdressenPflegeViewModel.InsertMode));
        }

        [HttpPost]
        public ActionResult NewAddress()
        {
            AdressenPflegeViewModel.InsertMode = true;
            ModelState.Clear();
            return PartialView("AdressenPflege/AdressenDetailsForm", AdressenPflegeViewModel.NewItem().SetInsertMode(AdressenPflegeViewModel.InsertMode));
        }

        [HttpPost]
        public ActionResult DeleteAddress(int id)
        {
            AdressenPflegeViewModel.RemoveItem(id);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult AddressDetailsFormSave(Adresse model)
        {
            // Avoid ModelState clearing on saving 
            // => because automatic model validation (via data annotations) would be omitted !!!
            // ModelState.Clear();

            var viewModel = AdressenPflegeViewModel;

            viewModel.ValidateModel(model, viewModel.InsertMode, ModelState.AddModelError);

            if (ModelState.IsValid)
            {
                if (viewModel.InsertMode)
                    viewModel.AddItem(model);

                model = viewModel.SaveItem(model, ModelState.AddModelError);
            }

            model.IsValid = ModelState.IsValid;
            model.InsertModeTmp = viewModel.InsertMode;

            return PartialView("AdressenPflege/AdressenDetailsForm", model);
        }

        public ActionResult ExportFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = AdressenPflegeViewModel.AdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Adressen", dt);

            return new EmptyResult();
        }

        public ActionResult ExportFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = AdressenPflegeViewModel.AdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Adressen", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion
    }
}
