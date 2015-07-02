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
            var model = filterContext.Controller.ViewData.Model as HolBringServiceViewModel;
            if (model != null)
            {
                filterContext.Controller.ViewData["GlobalViewData"] = model.GlobalViewData;
            }
        }
    }
}