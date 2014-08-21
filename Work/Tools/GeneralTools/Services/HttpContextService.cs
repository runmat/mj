using System;
using System.Web;
using GeneralTools.Models;

namespace GeneralTools.Services
{
    public class HttpContextService
    {
        public static void TryGetUserDataFromUrlOrSession(out int appID, out int userID, out int customerID, out int kunnr, out int portalType)
        {
            // try to obtain "appID, userID, customerID, kunnr, portalType"
            // => .. from Request QueryString
            var cp = HttpContext.Current.Request["cp"];
            if (String.IsNullOrEmpty(cp) && HttpContext.Current.Session != null && HttpContext.Current.Session["cp"] != null)
                // => .. or from Session
                cp = HttpContext.Current.Session["cp"].ToString();

            if (!String.IsNullOrEmpty(cp) && cp.Split('_').Length >= 5)
            {
                var userContextParams = cp.Split('_');
                appID = userContextParams[0].ToInt();
                userID = userContextParams[1].ToInt();
                customerID = userContextParams[2].ToInt();
                kunnr = userContextParams[3].ToInt();
                portalType = userContextParams[4].ToInt();
            }
            else
            {
                appID = 0;
                userID = 0;
                customerID = 0;
                kunnr = 0;
                portalType = 0;
            }
        }
    }
}
