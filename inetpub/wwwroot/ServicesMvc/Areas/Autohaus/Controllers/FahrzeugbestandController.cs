using System.Collections;
using System.Linq;
using System.Web.Mvc;
using CkgDomainLogic.DomainCommon.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Fahrzeugbestand.Contracts;
using CkgDomainLogic.Fahrzeugbestand.Models;
using CkgDomainLogic.Fahrzeugbestand.ViewModels;
using CkgDomainLogic.General.Services;
using DocumentTools.Services;
using GeneralTools.Contracts;
using GeneralTools.Models;
using Telerik.Web.Mvc;

namespace ServicesMvc.Autohaus.Controllers
{
    public class FahrzeugbestandController : CkgDomainController
    {
        public override string DataContextKey { get { return "FahrzeugbestandViewModel"; } }

        public FahrzeugbestandViewModel ViewModel { get { return GetViewModel<FahrzeugbestandViewModel>(); } }

        public override AdressenPflegeViewModel AdressenPflegeViewModel { get { return ViewModel; } }


        public FahrzeugbestandController(IAppSettings appSettings, ILogonContextDataService logonContext, IFahrzeugAkteBestandDataService fahrzeugbestandDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, fahrzeugbestandDataService);
            InitModelStatics();
        }

        [CkgApplication]
        public ActionResult Index(string pid = null, string fid = null)
        {
            ViewModel.DataInit(pid, fid);

            return View(ViewModel);
        }

        void InitModelStatics()
        {
            FahrzeugAkteBestand.GetBestandViewModel = GetViewModel<FahrzeugbestandViewModel>;
            FahrzeugAkteBestandSelektor.GetViewModel = GetViewModel<FahrzeugbestandViewModel>;
        }


        #region Fahrzeug Akte / Bestand

        [HttpPost]
        public ActionResult LoadFahrzeugAkteBestand(FahrzeugAkteBestandSelektor model)
        {
            ViewModel.FahrzeugAkteBestandSelektor = model;

            if (model.TmpSelectionKey.IsNotNullOrEmpty())
            {
                if (model.TmpSelectionType == "HALTER")
                    model.Halter = (model.TmpSelectionKey == "ALLE" ? "" : model.TmpSelectionKey);
                else
                    model.Kaeufer = (model.TmpSelectionKey == "ALLE" ? "" : model.TmpSelectionKey);

                ModelState.Clear();
                model.IsValid = false;
                return PartialView("Partial/FahrzeugAkteBestandSuche", model);
            }

            if (ModelState.IsValid)
                ViewModel.LoadFahrzeuge();

            model.IsValid = ModelState.IsValid;

            return PartialView("Partial/FahrzeugAkteBestandSuche", model);
        }

        [HttpPost]
        public ActionResult ShowFahrzeugAkteBestandDetails(string finIdToLoad)
        {
            ViewModel.TryLoadFahrzeugDetailsUsingFinId(finIdToLoad);

            return PartialView("Partial/FahrzeugAkteBestandDetails", ViewModel.CurrentFahrzeug);
        }

        [HttpPost]
        public ActionResult UpdateFahrzeugDetails(FahrzeugAkteBestand model)
        {
            if (!ModelState.IsValid)
                return PartialView("Partial/FahrzeugAkteBestandDetails", model);

            ViewModel.UpdateFahrzeugDetails(model, model.FinID, ModelState.AddModelError);

            if (ModelState.IsValid)
                ModelState.Clear();
                
            return PartialView("Partial/FahrzeugAkteBestandDetails", ViewModel.CurrentFahrzeug);
        }

        [HttpPost]
        public ActionResult GetTypDaten(string herstellerSchluessel, string typSchluessel, string vvsSchluessel)
        {
            var model = ViewModel.GetTypDaten(herstellerSchluessel, typSchluessel, vvsSchluessel);

            return Json(new
                {
                    success = (model != null),
                    fabrikName = (model ?? new FahrzeugAkteBestand()).FabrikName,
                    handelsName = (model ?? new FahrzeugAkteBestand()).HandelsName
                });
        }

        #region Halter

        [GridAction]
        public ActionResult HalterAdressenAjaxBinding()
        {
            return View(new GridModel(ViewModel.HalterForSelectionFiltered));
        }

        [HttpPost]
        public ActionResult FilterHalterAdressenAuswahlGrid(string filterValue, string filterColumns)
        {
            ViewModel.FilterHalterForSelection(filterValue, filterColumns);
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult HalterAdressenShowGrid()
        {
            ViewModel.DataMarkForRefreshPartnerAdressen();

            return PartialView("Partial/HalterAdressenAuswahlGrid");
        }

        public ActionResult HalterAdressenAuswahlExportFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.HalterForSelectionFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.CarOwner, dt);

            return new EmptyResult();
        }

        public ActionResult HalterAdressenAuswahlExportFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.HalterForSelectionFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.CarOwner, dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion

        #region Käufer

        [GridAction]
        public ActionResult KaeuferAdressenAjaxBinding()
        {
            return View(new GridModel(ViewModel.KaeuferForSelectionFiltered));
        }

        [HttpPost]
        public ActionResult FilterKaeuferAdressenAuswahlGrid(string filterValue, string filterColumns)
        {
            ViewModel.FilterKaeuferForSelection(filterValue, filterColumns);
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult KaeuferAdressenShowGrid()
        {
            ViewModel.DataMarkForRefreshPartnerAdressen();

            return PartialView("Partial/KaeuferAdressenAuswahlGrid");
        }

        public ActionResult KaeuferAdressenAuswahlExportFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.KaeuferForSelectionFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.Buyer, dt);

            return new EmptyResult();
        }

        public ActionResult KaeuferAdressenAuswahlExportFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.KaeuferForSelectionFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.Buyer, dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion   

        #region Grid

        [HttpPost]
        public ActionResult ShowFahrzeugAkteBestandGrid()
        {
            return PartialView("Partial/FahrzeugAkteBestandGrid", ViewModel);
        }

        [GridAction]
        public ActionResult FahrzeugAkteBestandAjaxBinding()
        {
            return View(new GridModel(ViewModel.FahrzeugeAkteBestandFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridFahrzeugAkteBestand(string filterValue, string filterColumns)
        {
            ViewModel.FilterFahrzeugeAkteBestand(filterValue, filterColumns);

            return new EmptyResult();
        }

        [HttpPost]
        public JsonResult FahrzeugAuswahlSelectionChanged(string id, bool isChecked)  
        {
            int allSelectionCount, allCount = 0;
            if (id.IsNullOrEmpty())
                ViewModel.SelectFahrzeuge(isChecked, f => true, out allSelectionCount, out allCount);
            else
                ViewModel.SelectFahrzeug(id, isChecked, out allSelectionCount);
            
            return Json(new
            {
                allSelectionCount,
                allCount
            });
        }

        #endregion

        /// <summary>
        /// Für Massenzulassung
        /// </summary>
        /// <returns></returns>
        public ActionResult MultiReg()
        {
            var selectedFahrzeuge = ViewModel.FahrzeugeAkteBestand.Where(x => x.IsSelected).ToList();   // Alle Fahrzeuge zurückgeben, die vom Benutzer selektiert wurden

            TempData["SelectedFahrzeuge"] = selectedFahrzeuge;

            return RedirectToAction("IndexMultiReg", "Zulassung");
        }

        /// <summary>
        /// Für Massenabmeldung
        /// </summary>
        /// <returns></returns>
        public ActionResult MultiCancellation()
        {
            var selectedFahrzeuge = ViewModel.FahrzeugeAkteBestand.Where(x => x.IsSelected).ToList();   // Alle Fahrzeuge zurückgeben, die vom Benutzer selektiert wurden

            TempData["SelectedFahrzeuge"] = selectedFahrzeuge;

            return RedirectToAction("IndexMultiCancellation", "Zulassung");
        }

        [HttpPost]
        public ActionResult DeleteFahrzeuge()
        {
            var errorMessage = ViewModel.DeleteSelectedVehicles();

            if (!string.IsNullOrEmpty(errorMessage))
                return Json(new { success = false, message = errorMessage });

            int allSelectionCount, allCount;
            ViewModel.SelectFahrzeuge(false, f => true, out allSelectionCount, out allCount);

            return Json(new { success = true, allSelectionCount, allCount });
        }

        #region Export

        protected override IEnumerable GetGridExportData()
        {
            return ViewModel.FahrzeugeAkteBestandFiltered;
        }

        #endregion

        #endregion


        #region Partner Adressen

        [CkgApplication]
        public ActionResult K()
        {
            AdressenPflegeViewModel.AdressenDataInit("KAEUFER", LogonContext.KundenNr);

            return View("TestAdressPflege");
        }

        [CkgApplication]
        public ActionResult H()
        {
            AdressenPflegeViewModel.AdressenDataInit("HALTER", LogonContext.KundenNr);

            return View("TestAdressPflege");
        }

        [HttpPost]
        public ActionResult PickPartnerAddress(string partnerKennung)
        {
            AdressenPflegeViewModel.AdressenDataInit(partnerKennung, LogonContext.KundenNr);

            return PartialView("Partial/PartnerAdressenGrid", ViewModel);
        }

        [HttpPost]
        public JsonResult PickPartnerAddressFinished(int id)
        {
            var selectedPartner = ViewModel.PickPartnerAddressFinished(id);

            return Json(new
                {
                    partnerKennung = AdressenPflegeViewModel.AdressenKennung,
                    partnerID = selectedPartner.KundenNr,
                    partnerName = selectedPartner.GetAutoSelectString()
                });
        }

        #endregion
    }
}
