using System;
using System.Web.Mvc;
using CkgDomainLogic.Fahrzeuge.ViewModels;
using ServicesMvc.Areas.Fahrzeug.Models.HolBringService;

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

            //object modelTry;
            //var x = filterContext.Controller.ViewData.TryGetValue("GlobalViewData", out modelTry);
            //var globalViewData = (GlobalViewData)modelTry;
            //if (globalViewData == null)
            //{
            //    var asdf = 1;
            //}
            //else
            //{
            //    filterContext.Controller.ViewData["GlobalViewData"] = globalViewData;
            //}            

            //var globalViewData = new GlobalViewData();

            //var model = filterContext.Controller.ViewData.Model as HolBringServiceViewModel;
            //if (model != null)
            //{
            //    // filterContext.Controller.ViewData["GlobalViewData"] = model.GlobalViewData;

            //    globalViewData = model.GlobalViewData;

            //    //object modelTest;
            //    //var x = filterContext.Controller.ViewData.TryGetValue("GlobalViewData", out modelTest);
            //    //var modelTest2 = (GlobalViewData)modelTest;

            //}
            //else
            //{
            //    var model2 = filterContext.Controller.ViewData.Values; 

            //    object modelTest;
            //    var x = filterContext.Controller.ViewData.TryGetValue("GlobalViewData", out modelTest);
            //    globalViewData = (GlobalViewData) modelTest;
            //}

            //filterContext.Controller.ViewData["GlobalViewData"] = globalViewData;

        }
    }
}