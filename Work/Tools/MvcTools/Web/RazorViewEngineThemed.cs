using System;
using System.Web;
using System.Web.Mvc;
using GeneralTools.Contracts;
using GeneralTools.Models;

namespace MvcTools.Web
{
    public class RazorViewEngineThemed : RazorViewEngine
    {
        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            ViewEngineResult result = null;

            var logonContext = (ILogonContext)SessionHelper.GetSessionObject("LogonContext");
            if (logonContext != null && logonContext.CurrentLayoutTheme.IsNotNullOrEmpty())
            {
                if (partialViewName.ToLower().Contains("usermenu"))
                {
                    var x = partialViewName;
                }
                if (partialViewName.ToLower().Contains("formsearchbox"))
                {
                    var x = partialViewName;
                }
                if (partialViewName.ToLower().Contains("partial/suche"))
                {
                    var x = partialViewName;
                }

                var partialViewFullPath = string.Format("_Themes/{0}/{1}", logonContext.CurrentLayoutTheme, partialViewName);
                result = base.FindPartialView(controllerContext, partialViewFullPath, useCache);
            }

            //Fall back to default search path if no other view has been selected  
            if (result == null || result.View == null)
            {
                result = base.FindPartialView(controllerContext, partialViewName, useCache);
            }

            return result;
        }


        public static void TrySetPartialViewMarkerModeFromRequestToSession()
        {
            TrySetValueFromRequestToSession("pvm");
        }

        public static void EnforcePartialViewMarkerMode()
        {
            if (HttpContext.Current.Session != null)
                HttpContext.Current.Session["pvm"] = "1";
        }


        public static void TrySetThemeFromRequestToSession()
        {
            TrySetValueFromRequestToSession("theme", val =>
            {
                var logonContext = (ILogonContext)SessionHelper.GetSessionObject("LogonContext");
                if (logonContext != null)
                    logonContext.CurrentLayoutTheme = val;
            });
        }


        private static void TrySetValueFromRequestToSession(string key, Action<string> onSetAction = null)
        {
            if (HttpContext.Current != null && HttpContext.Current.Request.HttpMethod.NotNullOrEmpty().ToUpper().Contains("GET"))
                if (HttpContext.Current.Session != null && HttpContext.Current.Request[key] != null)
                {
                    var val = HttpContext.Current.Request[key];
                    HttpContext.Current.Session[key] = val;
                    
                    if (onSetAction != null)
                        onSetAction(val);
                }
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            var usePartialViewMarkers = false;
            if (controllerContext.HttpContext != null && controllerContext.HttpContext.Session != null)
                usePartialViewMarkers = (SessionHelper.GetSessionString("pvm").NotNullOrEmpty() == "1");

            if (!usePartialViewMarkers)
                return base.CreatePartialView(controllerContext, partialPath);

            return new RazorViewWithWrapperMarkers(controllerContext, partialPath, layoutPath: null, runViewStartPages: false, viewStartFileExtensions: FileExtensions, viewPageActivator: ViewPageActivator);
        }
    }
}
