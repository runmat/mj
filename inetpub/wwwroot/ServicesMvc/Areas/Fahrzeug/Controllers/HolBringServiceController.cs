using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models.HolBringService;
using CkgDomainLogic.Fahrzeuge.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using GeneralTools.Contracts;
using GeneralTools.Models;
using MvcTools.Web;

namespace ServicesMvc.Fahrzeug.Controllers
{
    public class HolBringServiceController : CkgDomainController 
    {

        public override string DataContextKey { get { return GetDataContextKey<HolBringServiceViewModel>(); } }

        public HolBringServiceViewModel ViewModel
        {
            get { return GetViewModel<HolBringServiceViewModel>(); }
            set { SetViewModel(value); }
        }

        public HolBringServiceController(IAppSettings appSettings, ILogonContextDataService logonContext,
            IHolBringServiceDataService holBringServiceDataService) 
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, holBringServiceDataService);
            InitModelStatics();
        }

        static void InitModelStatics()
        {}

        [CkgApplication]
        public ActionResult Index()
        {
            ViewModel.DataInit();

            ViewData["FahrzeugArtenList"] = ViewModel.Fahrzeugarten;

            return View(ViewModel);
        }

        [HttpPost]
        public ActionResult Auftraggeber(Auftraggeber model) 
        {
            if (ModelState.IsValid)
            {
                ViewModel.Auftraggeber = model;

                if (!string.IsNullOrEmpty(model.Kunde))
                {
                    ViewModel.Abholung.AbholungKunde = model.Kunde;
                    ViewModel.Anlieferung.AnlieferungKunde = model.Kunde;
                }
            }

            ViewData["FahrzeugArtenList"] = ViewModel.Fahrzeugarten;
            return PartialView("Partial/Auftraggeber", model);
        }

        [HttpPost]
        public ActionResult Abholung(Abholung model)
        {
            // Wenn Action nicht durch Absenden aufgerufen wird, model aus ViewModel übernehmen und etwaige ModelStateErrors entfernen
            if (Request["formSubmit"] != "ok")     
            {
                model = ViewModel.Abholung;
                foreach (var modelValue in ModelState.Values)
                    modelValue.Errors.Clear();
            }

            if (ModelState.IsValid)
            {
                ViewModel.Abholung = model;

                ViewModel.CopyDefaultValuesToAnlieferung(model);
            }

            ViewData["DropDownHourList"] = ViewModel.DropDownHours;
            ViewData["DropDownMinuteList"] = ViewModel.DropDownMinutes;
            return PartialView("Partial/Abholung", model);
        }

        [HttpPost]
        public ActionResult Anlieferung(Anlieferung model)
        {
            // Wenn Action nicht durch Absenden aufgerufen wird, model aus ViewModel übernehmen und etwaige ModelStateErrors entfernen
            if (Request["formSubmit"] != "ok")  
            {
                model = ViewModel.Anlieferung;
                foreach (var modelValue in ModelState.Values)
                    modelValue.Errors.Clear();
            }

            if (ModelState.IsValid)
            {
                ViewModel.Anlieferung = model;
            }

            ViewData["DropDownHourList"] = ViewModel.DropDownHours;
            ViewData["DropDownMinuteList"] = ViewModel.DropDownMinutes;

            return PartialView("Partial/Anlieferung", model);
        }

        [HttpPost]
        public ActionResult Upload(Upload model)
        {
            // Wenn Action nicht durch Absenden aufgerufen wird, model aus ViewModel übernehmen und etwaige ModelStateErrors entfernen
            if (Request["formSubmit"] != "ok")
            {
                model = ViewModel.Upload;
                foreach (var modelValue in ModelState.Values)
                    modelValue.Errors.Clear();
            }

            if (ModelState.IsValid)
            {
                ViewModel.Upload = model;
            }

            return PartialView("Partial/Upload", model);
        }

        [HttpPost]
        public ActionResult UploadPdfStart(IEnumerable<HttpPostedFileBase> uploadFiles)
        {
            if (uploadFiles == null || uploadFiles.None())
                return Json(new { success = false, message = Localize.ErrorNoFileSelected }, "text/plain");

            // because we are uploading in async mode, our "e.files" collection always has exact 1 entry:
            var file = uploadFiles.ToArray()[0];

            if (!ViewModel.PdfUploadFileSave(file.FileName, file.SavePostedFile))
                return Json(new { success = false, message = Localize.ErrorFileCouldNotBeSaved }, "text/plain");

            return Json(new
            {
                success = true,
                message = "ok",
                uploadFileName = file.FileName,
            }, "text/plain");
        }



    }
}
