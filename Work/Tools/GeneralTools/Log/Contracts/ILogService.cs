using System;
using System.Data;
using GeneralTools.Models;

namespace GeneralTools.Contracts
{
    public interface ILogService
    {
        string LogFileName { get; set; }

        string AppName { get; set; }


        LogItem LogFatal(Exception exception, ILogonContext logonContext, object dataContext = null);

        LogItem LogError(Exception exception, ILogonContext logonContext, object dataContext = null);

        void LogElmahError(Exception exception, ILogonContext logonContext, object dataContext = null);

        LogItem LogInfo(ILogonContext logonContext, object dataContext = null);

        LogItem Log(LogItem logItem, ILogonContext logonContext, Exception exception = null, object dataContext = null);

        void LogSapCall(string bapiName, string logon, DataTable import, DataTable export, bool success, double dauer);

        void LogPageVisit(int appID, int userID, int customerID, int kunnr, int portalType, string userIDText = null);

        
        /// <summary>
        /// used for more detailed logging information in deeper scope (like SAP).
        /// points to another logitem that provides info about this scope.
        /// </summary>
        string GetLogItemIDtoLinkAsChild(ILogonContext logonContext);
        
        void SetLogItemIDtoLinkAsChild(ILogonContext logonContext, string value);
    }
}
