using System.IO;
using System.Web.Mvc;

namespace MvcTools.Web
{
    public static class MvcControllerExtensions
    {
        #region PartialView ToString

        public static string RenderPartialViewToString(this Controller controller)
        {
            return controller.RenderPartialViewToString(null, null);
        }

        public static string RenderPartialViewToString(this Controller controller, string viewName)
        {
            return controller.RenderPartialViewToString(viewName, null);
        }

        public static string RenderPartialViewToString(this Controller controller, object model)
        {
            return controller.RenderPartialViewToString(null, model);
        }

        public static string RenderPartialViewToString(this Controller controller, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = controller.ControllerContext.RouteData.GetRequiredString("action");

            controller.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        #endregion
    }
}
