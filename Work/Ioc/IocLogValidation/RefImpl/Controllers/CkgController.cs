using System.Web.Mvc;
using RefImplBibl.Interfaces;

namespace RefImpl.Controllers
{
    public class CkgController : Controller
    {
        protected readonly IAnwenderInfoProvider _anwenderInfoProvider;

        protected CkgController(IAnwenderInfoProvider anwenderInfoProvider)
        {
            _anwenderInfoProvider = anwenderInfoProvider;
        }

        ///// <summary>
        ///// Fehlerbehandlung für alle 
        ///// </summary>
        ///// <param name="filterContext"></param>
        //protected override void OnException(ExceptionContext filterContext)
        //{
        //    var logger = LogManager.GetLogger("Exception");
        //    CkgSessionWrapperException exception = new CkgSessionWrapperException(_anwenderInfoProvider.ToString(), filterContext.Exception);
        //    logger.LogException(LogLevel.Error, "Error", exception);
        //    filterContext.ExceptionHandled = true;
        //    base.OnException(filterContext);
        //}
    }
}
