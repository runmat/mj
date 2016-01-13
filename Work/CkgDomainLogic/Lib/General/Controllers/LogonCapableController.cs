using System;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Services;
using MvcTools.Controllers;
using MvcTools.Web;
using WebTools.Services;

namespace CkgDomainLogic.General.Controllers
{
    public abstract class LogonCapableController : DataContextCapableController 
    {
        public ILogonContext LogonContext
        {
            get { return SessionHelper.GetSessionObject<ILogonContext>("LogonContext"); }
            set { SessionHelper.SetSessionValue("LogonContext", value); }
        }

        protected virtual bool NeedsAuhentification { get { return true; } }

        protected virtual bool NeedsDefaultIndexActionInUrl { get { return true; } }


        protected LogonCapableController(ILogonContext newLogonContext)
        {
            if (LogonContext == null || (newLogonContext != null && newLogonContext.UserName.IsNotNullOrEmpty()))
                LogonContext = newLogonContext;
        }


        public virtual void ValidateMaintenance()
        {
        }

        new protected internal ViewResult View()
        {
            return base.View(LogonContext.MvcEnforceRawLayout);
        }

        new protected internal ViewResult View(object model)
        {
            return View(LogonContext.MvcEnforceRawLayout, model);
        }

        new protected internal ViewResult View(string viewName)
        {
            return View(LogonContext.MvcEnforceRawLayout, viewName);
        }

        new protected internal ViewResult View(string viewName, object model)
        {
            return View(LogonContext.MvcEnforceRawLayout, viewName, model);
        }

        #region URL Logon

        bool UrlSetLogOn(string requestUserName, string urlRemoteLoginKey, string requestRemoteLoginLogoutUrl)
        {
            var success = true;
            if (requestUserName.IsNullOrEmpty())
                LogonContext.Clear();
            
            if (requestUserName != LogonContext.UserName)
            {
                var logonContext = LogonContext;
                
                Session.Clear();

                LogonContext = logonContext;

                if (urlRemoteLoginKey.IsNotNullOrEmpty())
                {
                    success = LogonContext.LogonUserWithUrlRemoteLoginKey(urlRemoteLoginKey);
                    if (success)
                        LogonContext.LogoutUrl = HttpUtility.UrlDecode(requestRemoteLoginLogoutUrl);
                }
                else
                    success = LogonContext.LogonUser(requestUserName);
            }

            if (!success)
                requestUserName = null;

            SessionHelper.SetSessionValue("LogonUserName", requestUserName);

            return success;
        }

        public bool UrlLogOn(string requestUserName, string urlRemoteLoginKey, string requestRemoteLoginLogoutUrl)
        {
            if (!LogonContext.UserNameIsValid(requestUserName))
                return false;

            return UrlSetLogOn(requestUserName, urlRemoteLoginKey, requestRemoteLoginLogoutUrl);
        }

        public void UrlLogOff()
        {
            UrlSetLogOn(null, null, null);
        }

        protected virtual int GetTokenExpirationMinutes()
        {
            return 120;
        }

        private bool ValidateUrlRemoteLogin(string requestRemoteLoginID, string requestRemoteLoginDateTime, string requestRemoteLoginLogoutUrl)
        {
            if (!UserSecurityService.UrlRemoteUserTryLogin(requestRemoteLoginID, requestRemoteLoginDateTime, GetTokenExpirationMinutes()))
                return false;
            
            if (Request["logouturl"] != null)
                SessionHelper.SetSessionValue("UrlRemoteLogin_LogoutUrl", HttpUtility.UrlDecode(requestRemoteLoginLogoutUrl));

            return true;
        }

        public ActionResult UrlGetLogonAction(string requestUserName, string requestRemoteLoginID, string requestRemoteLoginDateTime, string requestRemoteLoginLogoutUrl,
            string requestSurroundingSessionId = "") 
        {
            if (Session == null)
                return new EmptyResult();

            var rawUrl = Request.RawUrl;

            if (LogonContext.UserName.IsNullOrEmpty())
            {
                var machineName = Environment.MachineName.ToUpper();
                if (machineName.StartsWith("AHW")) // only available for machine names starting with "AHW"
                {
                    var autoLogonUser = GeneralConfiguration.GetConfigValue("Login", "AutoLogonUserName_" + machineName);
                    if (autoLogonUser.IsNotNullOrEmpty())
                    {
                        // Auto Logon a User only for debug purposes 
                        // and only available for machine names starting with "AHW"
                        LogonContext.LogonUser(autoLogonUser);
                        LogonContext.TrySetLogoutLink();
                        return Redirect(rawUrl);
                    }
                }
            }

            var loginUrl = LogonContext.GetLoginUrl(Server.UrlEncode(rawUrl));

            if (loginUrl.IsNullOrEmpty())
                // a missing login url should signal success and avoid any redirection here:
                // login is not necessary here:
                return null;


            // services integrated request login
            var decryptedRequestUserName = CryptoMd5.Decrypt(requestUserName);

            
            // External URL Remote Login
            if (ValidateUrlRemoteLogin(requestRemoteLoginID, requestRemoteLoginDateTime, requestRemoteLoginLogoutUrl))
                decryptedRequestUserName = LogonContext.GetUserNameFromUrlRemoteLoginKey(requestRemoteLoginID);


            // check if MVC was called in embedded mode (iFrame) and the surrounding session has changed
            var blnSurroundingSessionHasChanged = false;
            if (requestSurroundingSessionId.IsNotNullOrEmpty())
            {
                if (Session["SurroundingSessionId"] != null && Session["SurroundingSessionId"].ToString() != requestSurroundingSessionId)
                {
                    blnSurroundingSessionHasChanged = true;
                }
                Session["SurroundingSessionId"] = requestSurroundingSessionId;
            }


            if ((decryptedRequestUserName.IsNotNullOrEmpty() && LogonContext.UserName.IsNotNullOrEmpty() && decryptedRequestUserName != LogonContext.UserName) || blnSurroundingSessionHasChanged)
            {
                // User has changed ("Session user" valid and also "Request user" valid)
                // => ensure to log off the old user and create a new LogonContext later on here
                UrlLogOff();
            }

            LogonContext.TrySetLogoutLink();

            if ((LogonContext.UserName.IsNotNullOrEmpty() && decryptedRequestUserName.IsNullOrEmpty()) || !NeedsAuhentification)
                // a valid usercontext in session plus missing "request user" and "remote login user" should signal success and avoid any redirection here:
                return null;

            if (decryptedRequestUserName.IsNotNullOrEmpty())
            {
                if (UrlLogOn(decryptedRequestUserName, requestRemoteLoginID, requestRemoteLoginLogoutUrl))
                {
                    // redirect to our origin action but without(!) url parameters:
                    if (rawUrl.Contains("?"))
                        Request.QueryString.ToDictionary().ToList().ForEach(r => ControllerContext.RouteData.Values.Add(r.Key, r.Value));

                    var urlKeysToIgnore = new[] { "un", "appid", "ra", "rb", "logouturl" };
                    var urlParams = Request.QueryString.ToDictionary().Where(p => !urlKeysToIgnore.Contains(p.Key.ToLower()));
                    var urlQueryString = "";
                    // Explizites UrlEncode erforderlich, weil Werte aus QueryString beim Auslesen stets automatisch decoded werden
                    if (urlParams.Any())
                        urlParams.ToList().ForEach(p => urlQueryString += string.Format("{0}{1}={2}", (urlQueryString == "" ? "?" : "&"), p.Key, HttpUtility.UrlEncode(p.Value)));

                    var controllerUrlPart = ControllerContext.RouteData.GetRequiredString("controller") + "/";
                    if (rawUrl.ToLower().Contains(string.Format("/{0}", controllerUrlPart.ToLower())))
                        controllerUrlPart = "";

                    var redirectUrl = string.Format("{0}{1}{2}",
                        controllerUrlPart,
                        ControllerContext.RouteData.GetRequiredString("action"),
                        urlQueryString);

                    if (redirectUrl.ToLower().StartsWith("domaincommon/index"))
                        redirectUrl = "~/";

                    if (!NeedsDefaultIndexActionInUrl && redirectUrl.ToLower().EndsWith("index"))
                        redirectUrl = string.Format("~/{0}", ControllerContext.RouteData.GetRequiredString("controller"));

                    Session["cp"] = Request["cp"];

                    if (requestSurroundingSessionId.IsNotNullOrEmpty())
                        Session["SurroundingSessionId"] = requestSurroundingSessionId;

                    return Redirect(redirectUrl);
                }
            }

            return Redirect(loginUrl);
        }

        #endregion

        
        #region Logon Timout

        private static DateTime? LogonTimeoutCheckStartTime
        {
            get { return (DateTime?)SessionHelper.GetSessionObject("LogonTimeoutCheckStartTime"); }
            set { SessionHelper.SetSessionValue("LogonTimeoutCheckStartTime", value); }
        }

        private static bool? LogonTimeoutCheckIgnore
        {
            get { return (bool?)SessionHelper.GetSessionObject("LogonTimeoutCheckIgnore"); }
            set { SessionHelper.SetSessionValue("LogonTimeoutCheckIgnore", value); }
        }

        private int LogonTimeoutSeconds { get { return Session.Timeout * 60; } }
        //private int LogonTimeoutSeconds { get { return 10; } }

        private static int LogonTimeoutCheckSeconds { get { return LogonTimeoutCheckStartTime == null ? -1 : (int)(DateTime.Now - LogonTimeoutCheckStartTime.GetValueOrDefault()).TotalSeconds; } }

        private bool LogonTimeoutOccurred
        {
            get
            {
                if (!NeedsAuhentification)
                    return false;

                //if (Request.Url != null && Request.Url.ToString().ToLower().StartsWith("http://localhost/"))
                //    return false;

                return LogonTimeoutCheckSeconds > LogonTimeoutSeconds;
            }
        }

        protected override void EndExecute(IAsyncResult asyncResult)
        {
            //try { base.EndExecute(asyncResult); } catch (Exception) { }
            base.EndExecute(asyncResult);

            if (!LogonTimeoutCheckIgnore.GetValueOrDefault())
                LogonTimeoutCheckStartTime = DateTime.Now;

            LogonTimeoutCheckIgnore = false;
        }

        [HttpPost]
        public ActionResult CheckLogonTimeout()
        {
            LogonTimeoutCheckIgnore = true;

            var timeoutOccurred = LogonTimeoutOccurred; // || LogonContext.UserName.IsNullOrEmpty();
            if (timeoutOccurred)
                LogonContext = null;

            return Json(new { timeoutOccurred });
        }

        [HttpGet]
        public ActionResult CheckLogonTimeOut()
        {
            return Undefined();
        }

        [HttpGet]
        public ActionResult Undefined()
        {
            LogonContext = null;

            HttpContext.Response.Redirect("~/");

            return new EmptyResult();
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult UserLogout()
        {
            var logoutUrl = LogonContext.LogoutUrl.NotNullOrEmpty();

            if (logoutUrl.IsNotNullOrEmpty() && !logoutUrl.ToLower().StartsWith("http"))
                logoutUrl = "http://" + logoutUrl.ToLower();

            var sessionCulture = SessionHelper.GetSessionValue("UserCulture", "de-DE").NotNullOrEmpty();

            Session.Clear();

            // restore session culture
            SessionHelper.SetSessionValue("UserCulture", sessionCulture);

            return Json(new { logoutUrl });
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult UserSwitchCulture(string language)
        {
            var culture = "de-DE";
            switch (language.NotNullOrEmpty().ToUpper())
            {
                case "DE":
                    culture = "de-DE";
                    break;
                case "EN":
                    culture = "en-US";
                    break;
                case "FR":
                    culture = "fr-FR";
                    break;
            }
            SessionHelper.SetSessionValue("UserCulture", culture);
            
            return new EmptyResult();
        }

        #endregion

        [HttpPost]
        public virtual JsonResult GetAutoPostcodeCityMappings(string plz)
        {
            return new JsonResult();
        }

        protected void TestDelayShort()
        {
            var delay = ConfigurationManager.AppSettings["TestDelayShort"];
            if (delay != null)
                Thread.Sleep(Int32.Parse(delay));
        }

        protected void TestDelayLong()
        {
            var delay = ConfigurationManager.AppSettings["TestDelayLong"];
            if (delay != null && delay.NotNullOrEmpty() != "0")
                Thread.Sleep(Int32.Parse(delay));
        }

        public void ValidateApplication()
        {
            if (LogonContext.UserName.IsNullOrEmpty())
                return;

            // application name storing into logonContext  +  page visit logging   goes here:

            // 1. application name storing into logonContext  
            LogonContext.AppUrlQueryAndStore();

            // ToDo: 2. page visit logging goes here:
            //  Please use  LogonContext.AppUrl  for application identification purpose
        }
    }
}
