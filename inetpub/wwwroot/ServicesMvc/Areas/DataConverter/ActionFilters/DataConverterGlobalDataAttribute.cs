using System.Web.Mvc;
using ServicesMvc.DataConverter.Controllers;

namespace ServicesMvc.Areas.DataConverter
{
    public class DataConverterInjectGlobalDataAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // GlobalViewData aus dem Controller-ViewModel in ViewData schreiben, damit in partial Views verfügbar, ohne dass dies in jedem ActionResult per ViewData["GlobalViewData"] = ViewModel.GlobalViewData; angestoßen werden muss
            var globalViewData = ((AdminController)filterContext.Controller).ViewModel.GlobalViewData;
            filterContext.Controller.ViewData["GlobalViewData"] = globalViewData;

            // Wenn Action nicht durch Absenden aufgerufen wurde, sondern durch AjaxRequest vom Wizard, etwaige ModelStateErrors entfernen, damit keine Validierungsfehler angezeigt werden (diese liegen beim ersten AjaxRequest ja noch nicht vor)
            if (filterContext.RequestContext.HttpContext.Request["firstRequest"] == "ok")
            {
                foreach (var modelValue in filterContext.Controller.ViewData.ModelState.Values)
                    modelValue.Errors.Clear();
            }
        }
    }
}