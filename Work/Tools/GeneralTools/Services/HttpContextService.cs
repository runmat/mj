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
            appID = 0;
            userID = 0;
            customerID = 0;
            kunnr = 0;
            portalType = 0;

            if (HttpContext.Current != null)
            {
                // try to obtain "appID, userID, customerID, kunnr, portalType"
                // => .. from Request QueryString
                var cp = HttpContext.Current.Request["cp"];

                // => .. or from Session
                if (String.IsNullOrEmpty(cp) && HttpContext.Current.Session != null && HttpContext.Current.Session["cp"] != null)
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
                else if (HttpContext.Current.Session != null && HttpContext.Current.Session["LastAppID"] != null)
                {
                    appID = HttpContext.Current.Session["LastAppID"].ToString().ToInt(0);
                    userID = HttpContext.Current.Session["LastUserID"].ToString().ToInt(0);
                    customerID = HttpContext.Current.Session["LastCustomerID"].ToString().ToInt(0);
                    kunnr = HttpContext.Current.Session["LastKunnr"].ToString().ToInt(0);
                    portalType = HttpContext.Current.Session["LastPortalType"].ToString().ToInt(0);
                }
            }
        }

        public static void TrySessionSetUserData(int appID, int userID, int customerID, int kunnr, int portalType)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session["LastAppID"] = appID;
                HttpContext.Current.Session["LastUserID"] = userID;
                HttpContext.Current.Session["LastCustomerID"] = customerID;
                HttpContext.Current.Session["LastKunnr"] = kunnr;
                HttpContext.Current.Session["LastPortalType"] = portalType;
            }
        }
    }
}
