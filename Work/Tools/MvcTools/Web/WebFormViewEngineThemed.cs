using System.Web.Mvc;
using GeneralTools.Contracts;
using GeneralTools.Models;

namespace MvcTools.Web
{
    public class WebFormViewEngineThemed : RazorViewEngine
    {
        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            ViewEngineResult result = null;

            var logonContext = (ILogonContext)SessionHelper.GetSessionObject("LogonContext");
            if (logonContext != null && logonContext.KundenNr.NotNullOrEmpty().Trim('0') == "240145")
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

                result = base.FindPartialView(controllerContext, "_Themes/AH2015/" + partialViewName, useCache);
            }

            //Fall back to default search path if no other view has been selected  
            if (result == null || result.View == null)
            {
                result = base.FindPartialView(controllerContext, partialViewName, useCache);
            }

            return result;
        }
    }
}
