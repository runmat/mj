using System.Collections;
using System.Web.Mvc;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.VersEvents.Contracts;
using CkgDomainLogic.VersEvents.Models;
using CkgDomainLogic.VersEvents.ViewModels;
using GeneralTools.Contracts;
using MvcTools.Web;
using Telerik.Web.Mvc;

namespace ServicesMvc.Controllers
{
    public class VersEventsController : CkgDomainController
    {
        public override string DataContextKey { get { return "VersEventsViewModel"; } }

        public VersEventsViewModel ViewModel { get { return GetViewModel<VersEventsViewModel>(); } }


        public VersEventsController(IAppSettings appSettings, ILogonContextDataService logonContext, IVersEventsDataService versEventsDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, versEventsDataService);
            InitModelStatics();
        }

        [CkgApplication]
        public ActionResult Index()
        {
            ViewModel.DataInit();

            return View(ViewModel);
        }

        [CkgApplication]
        public ActionResult Termin()
        {
            ViewModel.DataInit();

            VersEvent.EventOrteTemplateFunction = versEvent => this.RenderPartialViewToString("Event/EventGridOrteDetails", versEvent);

            // Test
            TerminSelect(6);

            return View(ViewModel);
        }

        [CkgApplication]
        public ActionResult Vorgang()
        {
            ViewModel.DataInit();

            return View(ViewModel);
        }

        void InitModelStatics()
        {
            VersEvent.GetViewModel = GetViewModel<VersEventsViewModel>;
            VorgangTermin.GetViewModel = GetViewModel<VersEventsViewModel>;
        }


        #region Vorgaenge


        [GridAction]
        public ActionResult VorgaengeAjaxBinding()
        {
            return View(new GridModel(ViewModel.VorgaengeFiltered));
        }

        [HttpPost]
        public ActionResult FilterVorgangGrid(string filterValue, string filterColumns)
        {
            ViewModel.VorgaengeFilter(filterValue, filterColumns);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult VorgangCreate()
        {
            ViewModel.InsertMode = true;
            ModelState.Clear();
            return PartialView("Vorgang/VorgangDetailsForm", ViewModel.VorgangCreate().SetInsertMode(ViewModel.InsertMode));
        }

        [HttpPost]
        public ActionResult VorgangEdit(int id)
        {
            ViewModel.InsertMode = false;
            ModelState.Clear();
            return PartialView("Vorgang/VorgangDetailsForm", ViewModel.VorgangGet(id).SetInsertMode(ViewModel.InsertMode));
        }

        [HttpPost]
        public ActionResult VorgangDelete(int id)
        {
            ViewModel.VorgangDelete(id);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult VorgangGridSelect()
        {
            ViewModel.DataMarkForRefreshVorgaenge();

            return PartialView("Vorgang/VorgangGrid");
        }

        [HttpPost]
        public ActionResult VorgangDetailsFormSave(Vorgang model)
        {
            // Avoid ModelState clearing on saving 
            // => because automatic model validation (via data annotations) would be omitted !!!
            // ModelState.Clear();

            ViewModel.VorgangValidate(model, ViewModel.InsertMode, ModelState.AddModelError);

            if (ModelState.IsValid)
            {
                if (ViewModel.InsertMode)
                    ViewModel.VorgangAdd(model);

                model = ViewModel.VorgangSave(model, ModelState.AddModelError);
            }

            model.InsertModeTmp = ViewModel.InsertMode;

            return PartialView("Vorgang/VorgangDetailsForm", model);
        }

        #endregion


        #region Termine

        [HttpPost]
        public ActionResult TerminCreate(int versBoxID = 0, int vorgangID = 0)
        {
            ModelState.Clear();

            return PartialView("Termin/TerminDetailsForm", ViewModel.TerminAdd(ViewModel.TerminCreate(versBoxID, vorgangID)));
        }

        [HttpPost]
        public ActionResult TerminSelect(int terminID)
        {
            ModelState.Clear();

            return PartialView("Termin/TerminDetailsForm", ViewModel.TerminGet(terminID));
        }

        [HttpPost]
        public ActionResult TerminDetailsFormSave(VorgangTermin model)
        {
            ViewModel.TerminValidate(model, ModelState.AddModelError);

            if (ModelState.IsValid)
                model = ViewModel.TerminSave(model, ModelState.AddModelError);

            return PartialView("Termin/TerminDetailsForm", model);
        }

        #endregion


        #region VersEvents

        [HttpPost]
        public ActionResult EventOrtGridSelect()
        {
            ViewModel.DataMarkForRefresh();

            return PartialView("Event/EventGrid");
        }

        [GridAction]
        public ActionResult VersEventAjaxBinding()
        {
            return View(new GridModel(ViewModel.VersEventsFiltered));
        }

        [HttpPost]
        public ActionResult FilterVersEventGrid(string filterValue, string filterColumns)
        {
            ViewModel.VersEventsFilter(filterValue, filterColumns);

            return new EmptyResult();
        }


        [HttpPost]
        public ActionResult VersEventCreate()
        {
            ModelState.Clear();
            return PartialView("Konfigurator/VersEventDetailsForm", ViewModel.VersEventAdd(ViewModel.VersEventCreate()));
        }


        [HttpPost]
        public ActionResult VersEventEdit(int id)
        {
            ModelState.Clear();
            return PartialView("Konfigurator/VersEventDetailsForm", ViewModel.VersEventGet(id));
        }

        [HttpPost]
        public ActionResult VersEventDelete(int id)
        {
            ViewModel.VersEventDelete(id);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult VersEventDetailsFormSave(VersEvent model)
        {
            ViewModel.VersEventValidate(model, ModelState.AddModelError);

            if (ModelState.IsValid)
                model = ViewModel.VersEventSave(model, ModelState.AddModelError);

            return PartialView("Konfigurator/VersEventDetailsForm", model);
        }

        #endregion


        #region VersEventOrte

        [GridAction]
        public ActionResult VersEventOrtAjaxBinding()
        {
            return View(new GridModel(ViewModel.VersEventOrteFiltered));
        }

        [HttpPost]
        public ActionResult FilterVersEventOrtGrid(string filterValue, string filterColumns)
        {
            ViewModel.VersEventOrteFilter(filterValue, filterColumns);

            return new EmptyResult();
        }


        [HttpPost]
        public ActionResult VersEventOrtCreate()
        {
            ModelState.Clear();
            return PartialView("Konfigurator/VersEventOrtDetailsForm", ViewModel.VersEventOrtAdd(ViewModel.VersEventOrtCreate()));
        }


        [HttpPost]
        public ActionResult VersEventOrtEdit(int id)
        {
            ModelState.Clear();
            return PartialView("Konfigurator/VersEventOrtDetailsForm", ViewModel.VersEventOrtGet(id));
        }

        [HttpPost]
        public ActionResult VersEventOrtDelete(int id)
        {
            ViewModel.VersEventOrtDelete(id);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult VersEventOrtDetailsFormSave(VersEventOrt model)
        {
            ViewModel.VersEventOrtValidate(model, ModelState.AddModelError);

            if (ModelState.IsValid)
                model = ViewModel.VersEventOrtSave(model, ModelState.AddModelError);

            return PartialView("Konfigurator/VersEventOrtDetailsForm", model);
        }

        #endregion


        #region VersEventOrtBoxen

        [GridAction]
        public ActionResult VersEventOrtBoxAjaxBinding()
        {
            return View(new GridModel(ViewModel.VersEventOrtBoxenFiltered));
        }

        [HttpPost]
        public ActionResult FilterVersEventOrtBoxGrid(string filterValue, string filterColumns)
        {
            ViewModel.VersEventOrtBoxenFilter(filterValue, filterColumns);

            return new EmptyResult();
        }


        [HttpPost]
        public ActionResult VersEventOrtBoxCreate()
        {
            ModelState.Clear();
            return PartialView("Konfigurator/VersEventOrtBoxDetailsForm", ViewModel.VersEventOrtBoxAdd(ViewModel.VersEventOrtBoxCreate()));
        }


        [HttpPost]
        public ActionResult VersEventOrtBoxEdit(int id)
        {
            ModelState.Clear();
            return PartialView("Konfigurator/VersEventOrtBoxDetailsForm", ViewModel.VersEventOrtBoxGet(id));
        }

        [HttpPost]
        public ActionResult VersEventOrtBoxDelete(int id)
        {
            ViewModel.VersEventOrtBoxDelete(id);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult VersEventOrtBoxDetailsFormSave(VersEventOrtBox model)
        {
            ViewModel.VersEventOrtBoxValidate(model, ModelState.AddModelError);

            if (ModelState.IsValid)
                model = ViewModel.VersEventOrtBoxSave(model, ModelState.AddModelError);

            return PartialView("Konfigurator/VersEventOrtBoxDetailsForm", model);
        }

        #endregion


        
        [HttpPost]
        public ActionResult ObsoleteSaveEvents()
        {
            //ViewModel.SaveEvents();

            return new EmptyResult();
        }


        #region Export

        protected override IEnumerable GetGridExportData()
        {
            return ViewModel.FilteredObjectsCurrent();
        }

        #endregion
    }
}
