using System;
using System.Data;
using System.Linq;
using System.Web;
using CKG.Base.Kernel.Security;
using WebTools.Services;

namespace AutohausPortal.lib
{
    /// <summary>
    /// 
    /// </summary>
    public class MVC
    {
        public static User GetSessionUserObject()
        {
            var context = HttpContext.Current;

            var user = (User)context.Session["objUser"];
            if (user == null)
            {
                var requestUserName = context.Request["un"];
                if (requestUserName == null)
                    return null;

                var decryptedUserName = CryptoMd5.Decrypt(requestUserName);

                user = new User();
                user.Login(decryptedUserName, context.Session.SessionID);
                context.Session["objUser"] = user;
            }

            var rawUrl = context.Request.RawUrl;
            var index = rawUrl.IndexOf("?un=", StringComparison.Ordinal);
            if (index == -1)
                index = rawUrl.IndexOf("&un=", StringComparison.Ordinal);

            if (index != -1)
            {
                rawUrl = rawUrl.Substring(0, index);
                context.Response.Redirect(rawUrl);
                context.Response.End();
            }

            return user;
        }

        public static void MvcPrepareDataRowsUrl(DataTable appTable, string userName)
        {
            appTable.Select().ToList().ForEach(r => MvcPrepareDataRowUrl(r, userName));
        }

        private static void MvcPrepareDataRowUrl(DataRow row, string userName)
        {
            row["AppURL"] = MvcPrepareUrl(row["AppURL"].ToString(), row["AppID"].ToString(), userName);
        }

        public static string MvcPrepareUrl(string appUrl, string appID, string userName)
        {
            var relativeUrl = appUrl;
            if (string.IsNullOrEmpty(relativeUrl))
                return null;

            relativeUrl = relativeUrl.ToLower();
            var returnUrl = "";
            if (!relativeUrl.StartsWith("mvc/"))
            {
                returnUrl = appUrl + "?AppID=" + appID;
            }
            else
            {
                relativeUrl = relativeUrl.Replace("mvc/", "");
                relativeUrl = relativeUrl.Replace("../forms/", "");
                var path = HttpContext.Current.Request.Path;

                var folders = path.Split('/');
                if (folders.Length > 1)
                {
                    var rootFolder = folders[1];
                    var rootFolderMvc = string.Format("{0}Mvc", rootFolder);

                    var crypteduserName = CryptoMd5.EncryptToUrlEncoded(userName);

                    relativeUrl = string.Format("/{0}/{1}?un={2}&appID={3}", rootFolderMvc, relativeUrl, crypteduserName, appID);

                    returnUrl = relativeUrl;
                }
            }

            return returnUrl;
        }
    }
}