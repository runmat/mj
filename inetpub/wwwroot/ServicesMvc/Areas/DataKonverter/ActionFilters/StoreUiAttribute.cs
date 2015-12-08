using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CkgDomainLogic.DataKonverter.ViewModels;
using MvcTools.Web;
using ServicesMvc.Areas.DataKonverter.Models;
using ServicesMvc.DataKonverter.Controllers;

namespace ServicesMvc.Areas.DataKonverter.ActionFilters
{
    /// <summary>
    /// Evtl. mit übermittelte UI-Daten ins Viewmodel überführen...
    /// </summary>
    public class StoreUiAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext) 
        {
            var dataMapper = ((AdminController)filterContext.Controller).ViewModel.DataMapper;

            var processors = filterContext.RequestContext.HttpContext.Request["processors"];
            var connections = filterContext.RequestContext.HttpContext.Request["connections"];

            // Aktuelle UI-Infos ins Viewmodel überführen...
            if (!string.IsNullOrEmpty(processors))
            {
                var processorList = JSon.Deserialize<Processor[]>(processors);
                dataMapper.Processors = new List<Processor>(processorList);
            }
            if (!string.IsNullOrEmpty(connections))
            {
                var connectionList = JSon.Deserialize<DataConnection[]>(connections);
                dataMapper.DataConnections = new List<DataConnection>(connectionList);
            }
        }
    }
}