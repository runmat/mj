using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using CKG.Base.Kernel.Security;
using GeneralTools.Models;
using GeneralTools.Services;
using WebTools.Services;

namespace ZLDBaseMvc
{
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

        public static string MvcPrepareUrl(string appUrl, string appID, string userName, bool forceMvcLayout = false, string alternativeRootpath = "")
        {
            var relativeUrl = appUrl;
            if (string.IsNullOrEmpty(relativeUrl))
                relativeUrl = "";

            relativeUrl = relativeUrl.ToLower();
            string returnUrl;
            if (!string.IsNullOrEmpty(relativeUrl) && !relativeUrl.StartsWith("mvc/"))
            {
                returnUrl = appUrl + "?AppID=" + appID;
            }
            else
            {
                var user = GetSessionUserObject();
                var mvcLayoutAsWebFormsInline = false;
                if (user != null && user.Customer != null)
                    mvcLayoutAsWebFormsInline = user.Customer.MvcLayoutAsWebFormsInline;

                relativeUrl = relativeUrl.Replace("mvc/", "");
                relativeUrl = relativeUrl.Replace("../forms/", "");
                var path = HttpContext.Current.Request.Path;

                var rootFolder = "";
                string rootFolderMvc;

                var folders = path.Split('/');
                if (folders.Length > 1)
                {
                    rootFolder = folders[1];
                }

                if (alternativeRootpath.IsNotNullOrEmpty())
                {
                    //path = alternativeRoopath;
                    //rootFolder = alternativeRootpath;
                    rootFolderMvc = alternativeRootpath;
                }
                else
                {
                    rootFolderMvc = string.Format("{0}Mvc", rootFolder);
                }

                var crypteduserName = CryptoMd5Web.EncryptToUrlEncoded(userName);

                relativeUrl = string.Format("/{0}/{1}?un={2}&appID={3}", rootFolderMvc, relativeUrl, crypteduserName, appID);
                if (mvcLayoutAsWebFormsInline && !forceMvcLayout)
                {
                    var inlineUrl = string.Format("/{0}/(S({1}))/Start/Mvc.aspx?appID={2}", rootFolder, HttpContext.Current.Session.SessionID, appID);
                    relativeUrl = inlineUrl;
                }

                returnUrl = relativeUrl;
            }

            return returnUrl;
        }

        public static void CryptAndSaveAppSettings()
        {
            var appRootFolder = HttpContext.Current.Server.MapPath("~/");
            var fileName = Path.Combine(appRootFolder, "App_Data");
            fileName = Path.Combine(fileName, "AppSettings.config");
            var xmlDict = new XmlDictionary<string, string>();
            ConfigurationManager.AppSettings.AllKeys.ToList().ForEach(key => xmlDict.Add(CryptoMd5.Encrypt(key), CryptoMd5.Encrypt(ConfigurationManager.AppSettings[key])));
            XmlService.XmlSerializeToFile(xmlDict, fileName);
        }
    }
}
