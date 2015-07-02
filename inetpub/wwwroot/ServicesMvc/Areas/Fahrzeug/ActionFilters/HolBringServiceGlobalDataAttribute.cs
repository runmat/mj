using System;
using System.Web.Mvc;
using CkgDomainLogic.Fahrzeuge.ViewModels;
using ServicesMvc.Areas.Fahrzeug.Models.HolBringService;
using ServicesMvc.Fahrzeug.Controllers;

namespace ServicesMvc.Areas.Fahrzeug
{
    public class HolBringServiceInjectGlobalDataAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

            // Wenn Action nicht durch Absenden aufgerufen wurde, sondern durch AjaxRequest vom Wizard, etwaige ModelStateErrors entfernen, damit keine Validierungsfehler angezeigt werden (diese liegen beim ersten AjaxRequest ja noch nicht vor)
            if (filterContext.RequestContext.HttpContext.Request["formSubmit"] != "ok")
            {
                foreach (var modelValue in filterContext.Controller.ViewData.ModelState.Values)
                    modelValue.Errors.Clear();
            }

            var descriptor = filterContext.ActionDescriptor;
            var actionName = descriptor.ActionName;
            var controllerName = descriptor.ControllerDescriptor.ControllerName;

            // GlobalViewData aus dem Controller-ViewModel in ViewData schreiben, damit in partial Views verfügbar, ohne dass dies in jedem ActionResult per ViewData["GlobalViewData"] = ViewModel.GlobalViewData; angestoßen werden muss
            var globalViewData = ((HolBringServiceController) filterContext.Controller).ViewModel.GlobalViewData;


            filterContext.Controller.ViewData["GlobalViewData"] = globalViewData;

        }
    }
}