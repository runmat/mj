using System.Collections.Generic;
using System.Web.Mvc;
using GeneralTools.Models;
using MvcTools.Web;
using ServicesMvc.DataConverter.Controllers;

namespace ServicesMvc.Areas.DataConverter.ActionFilters
{
    /// <summary>
    /// Evtl. mit übermittelte UI-Daten ins Viewmodel überführen...
    /// </summary>
    public class StoreUiAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext) 
        {
            var dataMapping = ((AdminController)filterContext.Controller).ViewModel.DataConverter.DataMapping;

            var processors = filterContext.RequestContext.HttpContext.Request["processors"];
            var connections = filterContext.RequestContext.HttpContext.Request["connections"];

            // Aktuelle UI-Infos ins Viewmodel überführen...
            if (!string.IsNullOrEmpty(processors))
            {
                var processorList = JSon.Deserialize<Processor[]>(processors);
                dataMapping.Processors = new List<Processor>(processorList);
            }
            if (!string.IsNullOrEmpty(connections))
            {
                var connectionList = JSon.Deserialize<DataConnection[]>(connections);
                dataMapping.DataConnections = new List<DataConnection>(connectionList);
            }
        }
    }
}