using System;
using System.Web;
using GeneralTools.Models;

namespace GeneralTools.Services
{
    public class HttpContextService
    {
        public static void TryGetUserDataFromUrlOrSession(out int appID, out int userID, out int customerID,
                                                          out int kunnr, out int portalType)
        {
            // try to obtain "appID, userID, customerID, kunnr, portalType"
            // => .. from Request QueryString
            var cp = HttpContext.Current.Request["cp"];
            if (String.IsNullOrEmpty(cp) && HttpContext.Current.Session != null &&
                HttpContext.Current.Session["cp"] != null)
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

        public static void TryGetAppIdFromUrlOrSession(out int appID)
        {
            // try to obtain "appID"
            // => .. from Request QueryString
            var strId = HttpContext.Current.Request["AppID"];
            if (String.IsNullOrEmpty(strId) && HttpContext.Current.Session != null &&
                HttpContext.Current.Session["AppID"] != null)
                // => .. or from Session
                strId = HttpContext.Current.Session["AppID"].ToString();

            int intID;
            if (!String.IsNullOrEmpty(strId) && Int32.TryParse(strId, out intID))
            {
                appID = intID;
            }
            else
            {
                appID = 0;
            }
        }

        public static object TryGetValueFromSession(string valueName)
        {
            if (HttpContext.Current.Session != null && HttpContext.Current.Session[valueName] != null)
            {
                return HttpContext.Current.Session[valueName];
            }

            return null;
        }
    }
}
