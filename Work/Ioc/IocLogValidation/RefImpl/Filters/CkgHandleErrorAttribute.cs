using System.Web.Mvc;
using NLog;
using RefImplBibl.Interfaces;
using RefImplBibl.Logging;

namespace RefImpl.Filters
{
    public class CkgHandleErrorAttribute : HandleErrorAttribute
    {
        public IAnwenderInfoProvider AnwenderInfoProvider { get; set; }

        public ErrorLogger ErrorLogger { get; set; }

        public override void OnException(ExceptionContext filterContext)
        {
            var logger = LogManager.GetLogger("ElmahLogger");
            CkgSessionWrapperException exception = new CkgSessionWrapperException(AnwenderInfoProvider.ToString(), filterContext.Exception);
            logger.LogException(LogLevel.Error, "Error", exception);
            filterContext.ExceptionHandled = true;
            ErrorLogger.Log(exception); // Auch in einer Datei loggen, sollte Elmah ausfallen (DB Fehler) werden die Fehler trotzdem geschrieben
            base.OnException(filterContext);
        }
    }
}