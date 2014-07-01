using System.Linq;
using System.Web.Mvc;
using CkgDomainLogic.Autohaus.Models;
using CkgDomainLogic.Autohaus.ViewModels;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Controllers;
using DocumentTools.Services;
using GeneralTools.Models;
using MvcTools.Web;
using Telerik.Web.Mvc;

namespace ServicesMvc.Controllers
{
    public partial class AutohausController  
    {
        public AutohausZulassungViewModel ZulassungViewModel { get { return GetViewModel<AutohausZulassungViewModel>(); } }

        [CkgApplication]
        public ActionResult Zulassung()
        {
            ZulassungViewModel.DataMarkForRefresh();

            return View(ZulassungViewModel);
        }

        [CkgApplication]
        public ActionResult ZulassungWithParams(string fahrgestellNr, string auftragsNr, string zb2Nr)
        {
            ZulassungViewModel.DataMarkForRefresh();
            ZulassungViewModel.SetFreieZulassungsOption(new FreieZulassung { VIN = fahrgestellNr, AuftragsReferenz = auftragsNr, ZBII = zb2Nr });

            return View("Zulassung", ZulassungViewModel);
        }

        [CkgApplication]
        public ActionResult ZulassungFromId(int fahrzeugId)
        {
            ZulassungViewModel.DataMarkForRefresh();
            ZulassungViewModel.SetFreieZulassungsOption(FahrzeugverwaltungViewModel.FahrzeugGet(fahrzeugId));

            return View("Zulassung", ZulassungViewModel);
        }

        #region Freie Zulassungs-Optionen

        [HttpPost]
        public ActionResult FreieZulassungsOptionen()
        {
            return PartialView("Zulassung/FreieZulassungsOptionen", ZulassungViewModel);
        }

        [HttpPost]
        public ActionResult FreieZulassungsOptionenForm(FreieZulassung model)
        {
            if (ModelState.IsValid)
            {
                ZulassungViewModel.SetFreieZulassungsOption(model);

                LogonContext.DataContextPersist(ZulassungViewModel);
            }

            return PartialView("Zulassung/FreieZulassungsOptionenForm", model);
        }

        #endregion

        #region HalterAdresse

        [HttpPost]
        public ActionResult HalterAdresse()
        {
            return PartialView("Zulassung/HalterAdresse", ZulassungViewModel);
        }

        [HttpPost]
        public JsonResult HalterAdresseGetAutoCompleteItems()
        {
            return Json(new { items = ZulassungViewModel.GetAdressenAsAutoCompleteItems("Halter") });
        }

        [HttpPost]
        public ActionResult HalterAdresseForm(Adresse model)
        {
            // Avoid ModelState clearing on saving => because automatic model validation (via data annotations) would be omitted !!!
            // ModelState.Clear();

            if (model.TmpSelectionKey.IsNotNullOrEmpty())
            {
                model = ZulassungViewModel.GetAdresseFromKey("Halter", model.TmpSelectionKey);
                if (model == null)
                    return new EmptyResult();

                ModelState.Clear();
                model.IsValid = false;
                return PartialView("Zulassung/HalterAdresseForm", model);
            }

            if (ModelState.IsValid)
            {
                ZulassungViewModel.SetHalterAdresse(model);

                LogonContext.DataContextPersist(ZulassungViewModel);
            }

            model.IsValid = ModelState.IsValid;

            return PartialView("Zulassung/HalterAdresseForm", model);
        }

        #endregion
        
        #region Zulassungs Daten

        [HttpPost]
        public ActionResult ZulassungsOptionen()
        {
            return PartialView("Zulassung/ZulassungsOptionen", ZulassungViewModel);
        }

        [HttpPost]
        public ActionResult ZulassungsOptionenForm(ZulassungsOptionen model)
        {
            if (ModelState.IsValid)
            {
                ZulassungViewModel.ZulassungsOption = model;

                LogonContext.DataContextPersist(ZulassungViewModel);
            }

            model.IsValid = ModelState.IsValid;

            return PartialView("Zulassung/ZulassungsOptionenForm", model);
        }

        [GridAction]
        public ActionResult VinWunschkennzeichenAjaxSelect()
        {
            return View(new GridModel(ZulassungViewModel.Wunschkennzeichen.WunschkennzeichenList));
        }

        [HttpPost]
        [GridAction]
        public ActionResult VinWunschkennzeichenAjaxUpdate(string id)
        {
            var itemToUpdate = ZulassungViewModel.Wunschkennzeichen.WunschkennzeichenList.FirstOrDefault(p => p.UniqueKey == id);
            var itemCloned = ModelMapping.Copy(itemToUpdate);
            if (TryUpdateModel(itemCloned))
                ModelMapping.Copy(itemCloned, itemToUpdate);

            return View(new GridModel(ZulassungViewModel.Wunschkennzeichen.WunschkennzeichenList));
        }
        
        [HttpPost]
        public ActionResult LoadKfzKreisAusHalterAdresse()
        {
            return Json(new { kfzKreis = ZulassungViewModel.LoadKfzKreisAusHalterAdresse() });
        }

        #endregion

        #region Dienstleistungen

        [HttpPost]
        public ActionResult Dienstleistungen()
        {
            ZulassungViewModel.ZulassungsDienstleistungen.InitDienstleistungen();

            return PartialView("Zulassung/Dienstleistungen", ZulassungViewModel);
        }

        [HttpPost]
        public ActionResult DienstleistungenForm(ZulassungsDienstleistungen model)
        {
            if (ModelState.IsValid)
            {
                ZulassungViewModel.SaveZulassungsDienstleistungen(model);

                LogonContext.DataContextPersist(ZulassungViewModel);
            }

            return PartialView("Zulassung/DienstleistungenForm", ZulassungViewModel.ZulassungsDienstleistungen);
        }

        #endregion

        #region Versicherungsdaten

        [HttpPost]
        public JsonResult VersicherungsAdresseGetAutoCompleteItems()
        {
            return Json(new { items = ZulassungViewModel.VersicherungsAdressenAsAutoCompleteItems });
        }

        #endregion

        #region Summary + Receipt

        [HttpPost]
        public ActionResult Summary()
        {
            return PartialView("Zulassung/Summary", ZulassungViewModel.CreateSummaryModel(false, GetAdressenSelectionLink));
        }

        public FileContentResult SummaryAsPdf()
        {
            var summaryHtml = this.RenderPartialViewToString("Zulassung/Partial/SummaryPdf", ZulassungViewModel.CreateSummaryModel(true, GetAdressenSelectionLink));

            var logoPath = AppSettings.LogoPath.IsNotNullOrEmpty() ? Server.MapPath(AppSettings.LogoPath) : "";
            var summaryPdfBytes = PdfDocumentFactory.HtmlToPdf(summaryHtml, logoPath, AppSettings.LogoPdfPosX, AppSettings.LogoPdfPosY);

            return new FileContentResult(summaryPdfBytes, "application/pdf") { FileDownloadName = "Übersicht.pdf" };
        }

        public ActionResult SummaryAsHtml()
        {
            return View("Zulassung/Partial/SummaryPdf", ZulassungViewModel.CreateSummaryModel(true, GetAdressenSelectionLink));
        }

        [HttpPost]
        public ActionResult Receipt()
        {
            LogonContext.DataContextPersist(ZulassungViewModel);
            ZulassungViewModel.Save();

            return PartialView("Zulassung/Receipt", ZulassungViewModel);
        }

        #endregion
   
        #region General Address Types
        
        #region HalterAdressen

        [GridAction]
        public ActionResult HalterAdressenAjaxBinding()
        {
            var items = ZulassungViewModel.HalterAdressenFiltered;
            return View(new GridModel(items));
        }
        [HttpPost]
        public ActionResult FilterHalterAdressenAuswahlGrid(string filterValue, string filterColumns)
        {
            ZulassungViewModel.FilterHalterAdressen(filterValue, filterColumns);
            return new EmptyResult();
        }
        [HttpPost]
        public ActionResult HalterAdressenShowGrid()
        {
            return AdressenShowGrid("Halter");
        }
        public ActionResult HalterAdressenAuswahlExportFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ZulassungViewModel.HalterAdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("HalterAdressen", dt);

            return new EmptyResult();
        }
        public ActionResult HalterAdressenAuswahlExportFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ZulassungViewModel.HalterAdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("HalterAdressen", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion

        #region ReguliererAdressen

        [GridAction]
        public ActionResult ReguliererAdressenAjaxBinding()
        {
            var items = ZulassungViewModel.ReguliererAdressenFiltered;
            return View(new GridModel(items));
        }
        [HttpPost]
        public ActionResult FilterReguliererAdressenAuswahlGrid(string filterValue, string filterColumns)
        {
            ZulassungViewModel.FilterReguliererAdressen(filterValue, filterColumns);
            return new EmptyResult();
        }
        [HttpPost]
        public ActionResult ReguliererAdressenShowGrid()
        {
            return AdressenShowGrid("Regulierer");
        }
        public ActionResult ReguliererAdressenAuswahlExportFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ZulassungViewModel.ReguliererAdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("ReguliererAdressen", dt);

            return new EmptyResult();
        }
        public ActionResult ReguliererAdressenAuswahlExportFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ZulassungViewModel.ReguliererAdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("ReguliererAdressen", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion

        #region RechnungsEmpfaengerAdressen

        [GridAction]
        public ActionResult RechnungsEmpfaengerAdressenAjaxBinding()
        {
            var items = ZulassungViewModel.RechnungsEmpfaengerAdressenFiltered;
            return View(new GridModel(items));
        }
        [HttpPost]
        public ActionResult FilterRechnungsEmpfaengerAdressenAuswahlGrid(string filterValue, string filterColumns)
        {
            ZulassungViewModel.FilterRechnungsEmpfaengerAdressen(filterValue, filterColumns);
            return new EmptyResult();
        }
        [HttpPost]
        public ActionResult RechnungsEmpfaengerAdressenShowGrid()
        {
            return AdressenShowGrid("RechnungsEmpfaenger");
        }
        public ActionResult RechnungsEmpfaengerAdressenAuswahlExportFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ZulassungViewModel.RechnungsEmpfaengerAdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("RechnungsEmpfaengerAdressen", dt);

            return new EmptyResult();
        }
        public ActionResult RechnungsEmpfaengerAdressenAuswahlExportFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ZulassungViewModel.RechnungsEmpfaengerAdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("RechnungsEmpfaengerAdressen", dt, landscapeOrientation: true);

            return new EmptyResult();
        }
        #endregion

        #region VersandScheinSchilderAdressen

        [GridAction]
        public ActionResult VersandScheinSchilderAdressenAjaxBinding()
        {
            var items = ZulassungViewModel.VersandScheinSchilderAdressenFiltered;
            return View(new GridModel(items));
        }
        [HttpPost]
        public ActionResult FilterVersandScheinSchilderAdressenAuswahlGrid(string filterValue, string filterColumns)
        {
            ZulassungViewModel.FilterVersandScheinSchilderAdressen(filterValue, filterColumns);
            return new EmptyResult();
        }
        [HttpPost]
        public ActionResult VersandScheinSchilderAdressenShowGrid()
        {
            return AdressenShowGrid("VersandScheinSchilder");
        }
        public ActionResult VersandScheinSchilderAdressenAuswahlExportFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ZulassungViewModel.VersandScheinSchilderAdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("VersandScheinSchilderAdressen", dt);

            return new EmptyResult();
        }
        public ActionResult VersandScheinSchilderAdressenAuswahlExportFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ZulassungViewModel.VersandScheinSchilderAdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("VersandScheinSchilderAdressen", dt, landscapeOrientation: true);

            return new EmptyResult();
        }
        #endregion

        #region VersandZb2CocAdressen

        [GridAction]
        public ActionResult VersandZb2CocAdressenAjaxBinding()
        {
            var items = ZulassungViewModel.VersandZb2CocAdressenFiltered;
            return View(new GridModel(items));
        }
        [HttpPost]
        public ActionResult FilterVersandZb2CocAdressenAuswahlGrid(string filterValue, string filterColumns)
        {
            ZulassungViewModel.FilterVersandZb2CocAdressen(filterValue, filterColumns);
            return new EmptyResult();
        }
        [HttpPost]
        public ActionResult VersandZb2CocAdressenShowGrid()
        {
            return AdressenShowGrid("VersandZb2Coc");
        }
        public ActionResult VersandZb2CocAdressenAuswahlExportFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ZulassungViewModel.VersandZb2CocAdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("VersandZb2CocAdressen", dt);

            return new EmptyResult();
        }
        public ActionResult VersandZb2CocAdressenAuswahlExportFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ZulassungViewModel.VersandZb2CocAdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("VersandZb2CocAdressen", dt, landscapeOrientation: true);

            return new EmptyResult();
        }
        
        #endregion

        #endregion

        #region General Address Helpers

        [HttpPost]
        public ActionResult AdressenShowGrid(string adressenTyp)
        {
            ZulassungViewModel.DataMarkForRefreshAdressenFiltered();

            ViewBag.AdressType = adressenTyp;
            return PartialView("Zulassung/Partial/AdressenAuswahlGrid");
        }

        [HttpPost]
        public ActionResult UpdateAddressAndGetSummary(int updateAddressid, string addressType)
        {
            if (updateAddressid != -1)
                ZulassungViewModel.SummaryUpdateAddressFromGrid(updateAddressid, addressType);

            return PartialView("Zulassung/Summary", ZulassungViewModel.CreateSummaryModel(false, GetAdressenSelectionLink));
        }

        string GetAdressenSelectionLink(string addressType)
        {
            var adressEditAvailable = ZulassungViewModel.SummaryAdressEditAvailable(addressType);
            var adressSelectionAvailable = ZulassungViewModel.SummaryAdressSelectionAvailable(addressType);

            return this.RenderPartialViewToString("Zulassung/Partial/SummaryAddressSelectionLink", new dynamic[]
                {
                    addressType, 
                    adressSelectionAvailable,
                    adressEditAvailable
                });
        }

        [HttpPost]
        public ActionResult Adresse(string addressType)
        {
            var address = ZulassungViewModel.GetAdresseFromType(addressType);
            if (address == null)
                return new EmptyResult();

            ViewBag.AdressType = addressType;
            return PartialView("Zulassung/Partial/AdresseOverlay", address);
        }

        [HttpPost]
        public ActionResult AdresseForm(Adresse model, string addressType)
        {
            // Avoid ModelState clearing on saving => because automatic model validation (via data annotations) would be omitted !!!
            // ModelState.Clear();

            if (ModelState.IsValid)
            {
                ZulassungViewModel.SetAdresse(model);

                LogonContext.DataContextPersist(ZulassungViewModel);
            }

            model.IsValid = ModelState.IsValid;

            return PartialView("Zulassung/Partial/AdresseOverlayForm", model);
        }

        #endregion     
    }
}
