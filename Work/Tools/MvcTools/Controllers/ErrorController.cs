using System;
using System.Web.Mvc;
using MvcTools.Models;

namespace MvcTools.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index(int statusCode, Exception exception, string originControllerName)
        {
            var model = new ErrorModel
                {
                    HttpStatusCode = statusCode, 
                    Exception = exception, 
                    OriginControllerName = originControllerName,
                };

            try { Response.StatusCode = statusCode; } catch {}

            return View(model);
        }
    }
}