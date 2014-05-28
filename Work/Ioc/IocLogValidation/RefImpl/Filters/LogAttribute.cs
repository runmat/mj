using System.Web.Mvc;
using Castle.Core.Logging;
using NLog;

namespace RefImpl.Filters
{
    public class LogAttribute : ActionFilterAttribute
    {
        private readonly ILogger _logger;


        public LogAttribute(ILogger logger)
        {
            _logger = logger.CreateChildLogger("Filter");
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            LogEventInfo logEventInfo = new LogEventInfo();
            logEventInfo.Level = LogLevel.Debug;



            base.OnActionExecuting(filterContext);
        }
    }
}