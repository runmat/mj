// ReSharper disable RedundantUsingDirective

using System.Linq;
using System.Web.Mvc;
using CkgDomainLogic.CoC.Contracts;
using CkgDomainLogic.CoC.Models;
using CkgDomainLogic.CoC.ViewModels;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Models;
using DocumentTools.Services;
using GeneralTools.Contracts;
using GeneralTools.Models;
using MvcTools.Web;
using Telerik.Web.Mvc;
// ReSharper restore RedundantUsingDirective

namespace ServicesMvc.Controllers
{
    public partial class CocBeauftragungController  
    {
        [CkgApplication]
        public ActionResult Zulassung(string vins)
        {
            ViewModel.DataMarkForRefresh(vins);
            ViewModel.SetCocBeauftragungMode(CocBeauftragungMode.Zulassung);

            return View("Beauftragung", ViewModel);
        }


        [CkgApplication]
        public ActionResult ZulassungFrei()
        {
            ViewModel.DataMarkForRefresh(null);
            ViewModel.SetCocBeauftragungMode(CocBeauftragungMode.FreieZulassung);

            return View("Beauftragung", ViewModel);
        }

        public ActionResult ZulassungFreiMitVorbelegung(string vin, string auftragsNr)
        {
            ViewModel.DataMarkForRefresh(null);
            ViewModel.SetFreieZulassungsOption(new FreieZulassung{ VIN = vin, AuftragsReferenz = auftragsNr });
            ViewModel.SetCocBeauftragungMode(CocBeauftragungMode.FreieZulassung);

            return View("Beauftragung", ViewModel);
        }


        #region Freie Zulassungs-Optionen

        [HttpPost]
        public ActionResult FreieZulassungsOptionen()
        {
            return PartialView(ViewModel);
        }

        [HttpPost]
        public ActionResult FreieZulassungsOptionenForm(FreieZulassung model)
        {
            if (ModelState.IsValid)
            {
                ViewModel.SetFreieZulassungsOption(model);

                LogonContext.DataContextPersist(ViewModel);
            }

            return PartialView(model);
        }

        #endregion


        #region HalterAdresse

        [HttpPost]
        public ActionResult HalterAdresse()
        {
            return PartialView(ViewModel);
        }

        [HttpPost]
        public JsonResult HalterAdresseGetAutoCompleteItems()
        {
            return Json(new { items = ViewModel.GetAdressenAsAutoCompleteItems("Halter") });
        }

        [HttpPost]
        public ActionResult HalterAdresseForm(Adresse model)
        {
            // Avoid ModelState clearing on saving => because automatic model validation (via data annotations) would be omitted !!!
            // ModelState.Clear();

            if (model.TmpSelectionKey.IsNotNullOrEmpty())
            {
                model = ViewModel.GetAdresseFromKey("Halter", model.TmpSelectionKey);
                if (model == null)
                    return new EmptyResult();

                ModelState.Clear();
                model.IsValid = false;
                return PartialView(model);
            }

            if (ModelState.IsValid)
            {
                ViewModel.SetHalterAdresse(model);

                LogonContext.DataContextPersist(ViewModel);
            }

            model.IsValid = ModelState.IsValid;

            return PartialView(model);
        }

        #endregion
        
        #region Zulassungs Daten

        [HttpPost]
        public ActionResult ZulassungsOptionen()
        {
            return PartialView(ViewModel);
        }

        [HttpPost]
        public ActionResult ZulassungsOptionenForm(ZulassungsOptionen model)
        {
            if (ModelState.IsValid)
            {
                ViewModel.ZulassungsOption = model;

                LogonContext.DataContextPersist(ViewModel);
            }

            model.IsValid = ModelState.IsValid;

            return PartialView(model);
        }

        [GridAction]
        public ActionResult VinWunschkennzeichenAjaxSelect()
        {
            return View(new GridModel(ViewModel.Wunschkennzeichen.WunschkennzeichenList));
        }

        [HttpPost]
        [GridAction]
        public ActionResult VinWunschkennzeichenAjaxUpdate(string id)
        {
            var itemToUpdate = ViewModel.Wunschkennzeichen.WunschkennzeichenList.FirstOrDefault(p => p.UniqueKey == id);
            var itemCloned = ModelMapping.Copy(itemToUpdate);
            if (TryUpdateModel(itemCloned))
                ModelMapping.Copy(itemCloned, itemToUpdate);

            return View(new GridModel(ViewModel.Wunschkennzeichen.WunschkennzeichenList));
        }
        
        [HttpPost]
        public ActionResult LoadKfzKreisAusHalterAdresse()
        {
            return Json(new { kfzKreis = ViewModel.LoadKfzKreisAusHalterAdresse() });
        }

        #endregion

        #region Wunschkennzeichen

        //[HttpPost]
        //public ActionResult WunschkennzeichenOptionen()
        //{
        //    return PartialView(ViewModel);
        //}

        //[HttpPost]
        //public ActionResult WunschkennzeichenForm(VinWunschkennzeichen[] model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        ViewModel.Wunschkennzeichen.WunschkennzeichenList = model.ToListOrEmptyList();

        //        LogonContext.DataContextPersist(ViewModel);
        //    }

        //    return PartialView(model);
        //}

        #endregion

        #region Dienstleistungen

        [HttpPost]
        public ActionResult Dienstleistungen()
        {
            ViewModel.ZulassungsDienstleistungen.InitDienstleistungen();

            return PartialView(ViewModel);
        }

        [HttpPost]
        public ActionResult DienstleistungenForm(ZulassungsDienstleistungen model)
        {
            if (ModelState.IsValid)
            {
                ViewModel.SaveZulassungsDienstleistungen(model);

                LogonContext.DataContextPersist(ViewModel);
            }

            return PartialView(ViewModel.ZulassungsDienstleistungen);
        }

        #endregion

        #region Versicherungsdaten

        [HttpPost]
        public ActionResult Versicherungsdaten()
        {
            return PartialView(ViewModel);
        }

        [HttpPost]
        public ActionResult VersicherungsdatenForm(Versicherungsdaten model)
        {
            if (ModelState.IsValid)
            {
                ViewModel.SaveVersicherungsdaten(model);

                LogonContext.DataContextPersist(ViewModel);
            }

            return PartialView(ViewModel.Versicherungsdaten);
        }

        [HttpPost]
        public JsonResult VersicherungsAdresseGetAutoCompleteItems()
        {
            return Json(new { items = ViewModel.VersicherungsAdressenAsAutoCompleteItems });
        }

        #endregion

        
        #region General Address Types
        
        #region HalterAdressen

        [GridAction]
        public ActionResult HalterAdressenAjaxBinding()
        {
            var items = ViewModel.HalterAdressenFiltered;
            return View(new GridModel(items));
        }
        [HttpPost]
        public ActionResult FilterHalterAdressenAuswahlGrid(string filterValue, string filterColumns)
        {
            ViewModel.FilterHalterAdressen(filterValue, filterColumns);
            return new EmptyResult();
        }
        [HttpPost]
        public ActionResult HalterAdressenShowGrid()
        {
            return AdressenShowGrid("Halter");
        }
        public ActionResult HalterAdressenAuswahlExportFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.HalterAdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("HalterAdressen", dt);

            return new EmptyResult();
        }
        public ActionResult HalterAdressenAuswahlExportFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.HalterAdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("HalterAdressen", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion

        #region ReguliererAdressen

        [GridAction]
        public ActionResult ReguliererAdressenAjaxBinding()
        {
            var items = ViewModel.ReguliererAdressenFiltered;
            return View(new GridModel(items));
        }
        [HttpPost]
        public ActionResult FilterReguliererAdressenAuswahlGrid(string filterValue, string filterColumns)
        {
            ViewModel.FilterReguliererAdressen(filterValue, filterColumns);
            return new EmptyResult();
        }
        [HttpPost]
        public ActionResult ReguliererAdressenShowGrid()
        {
            return AdressenShowGrid("Regulierer");
        }
        public ActionResult ReguliererAdressenAuswahlExportFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.ReguliererAdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("ReguliererAdressen", dt);

            return new EmptyResult();
        }
        public ActionResult ReguliererAdressenAuswahlExportFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.ReguliererAdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("ReguliererAdressen", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion

        #region RechnungsEmpfaengerAdressen

        [GridAction]
        public ActionResult RechnungsEmpfaengerAdressenAjaxBinding()
        {
            var items = ViewModel.RechnungsEmpfaengerAdressenFiltered;
            return View(new GridModel(items));
        }
        [HttpPost]
        public ActionResult FilterRechnungsEmpfaengerAdressenAuswahlGrid(string filterValue, string filterColumns)
        {
            ViewModel.FilterRechnungsEmpfaengerAdressen(filterValue, filterColumns);
            return new EmptyResult();
        }
        [HttpPost]
        public ActionResult RechnungsEmpfaengerAdressenShowGrid()
        {
            return AdressenShowGrid("RechnungsEmpfaenger");
        }
        public ActionResult RechnungsEmpfaengerAdressenAuswahlExportFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.RechnungsEmpfaengerAdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("RechnungsEmpfaengerAdressen", dt);

            return new EmptyResult();
        }
        public ActionResult RechnungsEmpfaengerAdressenAuswahlExportFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.RechnungsEmpfaengerAdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("RechnungsEmpfaengerAdressen", dt, landscapeOrientation: true);

            return new EmptyResult();
        }
        #endregion

        #region VersandScheinSchilderAdressen

        [GridAction]
        public ActionResult VersandScheinSchilderAdressenAjaxBinding()
        {
            var items = ViewModel.VersandScheinSchilderAdressenFiltered;
            return View(new GridModel(items));
        }
        [HttpPost]
        public ActionResult FilterVersandScheinSchilderAdressenAuswahlGrid(string filterValue, string filterColumns)
        {
            ViewModel.FilterVersandScheinSchilderAdressen(filterValue, filterColumns);
            return new EmptyResult();
        }
        [HttpPost]
        public ActionResult VersandScheinSchilderAdressenShowGrid()
        {
            return AdressenShowGrid("VersandScheinSchilder");
        }
        public ActionResult VersandScheinSchilderAdressenAuswahlExportFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.VersandScheinSchilderAdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("VersandScheinSchilderAdressen", dt);

            return new EmptyResult();
        }
        public ActionResult VersandScheinSchilderAdressenAuswahlExportFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.VersandScheinSchilderAdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("VersandScheinSchilderAdressen", dt, landscapeOrientation: true);

            return new EmptyResult();
        }
        #endregion

        #region VersandZb2CocAdressen

        [GridAction]
        public ActionResult VersandZb2CocAdressenAjaxBinding()
        {
            var items = ViewModel.VersandZb2CocAdressenFiltered;
            return View(new GridModel(items));
        }
        [HttpPost]
        public ActionResult FilterVersandZb2CocAdressenAuswahlGrid(string filterValue, string filterColumns)
        {
            ViewModel.FilterVersandZb2CocAdressen(filterValue, filterColumns);
            return new EmptyResult();
        }
        [HttpPost]
        public ActionResult VersandZb2CocAdressenShowGrid()
        {
            return AdressenShowGrid("VersandZb2Coc");
        }
        public ActionResult VersandZb2CocAdressenAuswahlExportFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.VersandZb2CocAdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("VersandZb2CocAdressen", dt);

            return new EmptyResult();
        }
        public ActionResult VersandZb2CocAdressenAuswahlExportFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.VersandZb2CocAdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("VersandZb2CocAdressen", dt, landscapeOrientation: true);

            return new EmptyResult();
        }
        
        #endregion

        #endregion


        #region General Address Helpers

        [HttpPost]
        public ActionResult AdressenShowGrid(string adressenTyp)
        {
            ViewModel.DataMarkForRefreshAdressenFiltered();

            ViewBag.AdressType = adressenTyp;
            return PartialView("Partial/AdressenAuswahlGrid");
        }

        [HttpPost]
        public ActionResult UpdateAddressAndGetSummary(int updateAddressid, string addressType)
        {
            if (updateAddressid != -1)
                ViewModel.SummaryUpdateAddressFromGrid(updateAddressid, addressType);

            return PartialView("Summary", ViewModel.CreateSummaryModel(false, GetAdressenSelectionLink));
        }

        string GetAdressenSelectionLink(string addressType)
        {
            var adressEditAvailable = ViewModel.SummaryAdressEditAvailable(addressType);
            var adressSelectionAvailable = ViewModel.SummaryAdressSelectionAvailable(addressType);

            return this.RenderPartialViewToString("Partial/SummaryAddressSelectionLink", new dynamic[]
                {
                    addressType, 
                    adressSelectionAvailable,
                    adressEditAvailable
                });
        }

        [HttpPost]
        public ActionResult Adresse(string addressType)
        {
            var address = ViewModel.GetAdresseFromType(addressType);
            if (address == null)
                return new EmptyResult();

            ViewBag.AdressType = addressType;
            return PartialView("Partial/AdresseOverlay", address);
        }

        [HttpPost]
        public ActionResult AdresseForm(Adresse model, string addressType)
        {
            // Avoid ModelState clearing on saving => because automatic model validation (via data annotations) would be omitted !!!
            // ModelState.Clear();

            if (ModelState.IsValid)
            {
                ViewModel.SetAdresse(model);

                LogonContext.DataContextPersist(ViewModel);
            }

            model.IsValid = ModelState.IsValid;

            return PartialView("Partial/AdresseOverlayForm", model);
        }

        #endregion
    }
}
