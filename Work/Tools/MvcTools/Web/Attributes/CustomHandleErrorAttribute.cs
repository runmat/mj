using System;
using System.Web;
using System.Web.Mvc;
using GeneralTools.Contracts;
using GeneralTools.Log.Services;
using GeneralTools.Services;

namespace MvcTools.Web
{
    public class CustomHandleErrorAttribute : HandleErrorAttribute
    {
        //private readonly ILog _logger;
        private ILogService _logService;

        public CustomHandleErrorAttribute(ILogService logService)
        {
            //_logger = LogManager.GetLogger("MyLogger");
            _logService = logService;
        }

        public override void OnException(ExceptionContext filterContext)
        {
            // Fehler soll auf jeden Fall geloggt werden, unabhängig von der Behandlung in MVC
            
            if (filterContext.ExceptionHandled)  // || !filterContext.HttpContext.IsCustomErrorEnabled)
            {
                return;
            }

            if (new HttpException(null, filterContext.Exception).GetHttpCode() != 500)
            {
                return;
            }

            if (!ExceptionType.IsInstanceOfType(filterContext.Exception))
            {
                return;
            }

            var logonContext = SessionStore.GetCurrentLogonContext();

            // if the request is AJAX return JSON else view.
            if (filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                filterContext.Result = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new
                    {
                        error = true,
                        message = filterContext.Exception.Message
                    }
                };
            }
            else
            {
                var controllerName = (string)filterContext.RouteData.Values["controller"];
                var actionName = (string)filterContext.RouteData.Values["action"];
                var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);

                var masterPage = Master;
                if (String.IsNullOrEmpty(masterPage))
                    masterPage = String.Format("~/Views/Shared/{0}.cshtml", (logonContext != null && logonContext.MvcEnforceRawLayout ? "_LayoutRaw" : "_Layout"));
                
                filterContext.Result = new ViewResult
                {
                    ViewName = View,
                    MasterName = masterPage,
                    ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                    TempData = filterContext.Controller.TempData
                };
            }

            if (_logService == null)
            {
                _logService = new LogService(string.Empty, string.Empty);        
            }

            var dataContext = SessionStore.GetCurrentDataContext();

            string strSessionInfo = "";

            if (dataContext != null)
            {
                strSessionInfo = XmlService.XmlSerializeRawBulkToString(dataContext, dataContext.GetType());
            }

            var wrapperException = new CkgSessionWrapperException(strSessionInfo, filterContext.Exception);
            _logService.LogElmahError(wrapperException, logonContext, dataContext);

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = 500;

            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }
}
