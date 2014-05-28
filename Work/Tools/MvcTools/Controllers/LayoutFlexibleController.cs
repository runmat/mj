using System.Web.Mvc;

namespace MvcTools.Controllers
{
    public abstract class LayoutFlexibleController : Controller 
    {
        protected internal ViewResult View(bool useRawLayout)
        {
            return PrepareView(View(), useRawLayout);
        }

        protected internal ViewResult View(bool useRawLayout, object model)
        {
            return PrepareView(View(model), useRawLayout);
        }

        protected internal ViewResult View(bool useRawLayout, string viewName)
        {
            return PrepareView(View(viewName), useRawLayout);
        }

        protected internal ViewResult View(bool useRawLayout, string viewName, object model)
        {
            return PrepareView(View(viewName, model), useRawLayout);
        }

        static ViewResult PrepareView(ViewResult viewResult, bool useRawLayout)
        {
            if (useRawLayout)
                viewResult.MasterName = "~/Views/Shared/_LayoutRaw.cshtml";

            return viewResult;
        }
    }
}
