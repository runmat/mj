using System.Web.Mvc;
using GeneralTools.Contracts;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Zulassung.MobileErfassung.ViewModels;
using MvcTools.Web;
using CkgDomainLogic.Zulassung.MobileErfassung.Models.UiModels;

namespace ZLDMobile.Controllers
{
    /// <summary>
    /// Controller für Anmeldung und Passwortänderung
    /// </summary>
    public class LoginController : LogonCapableController
    {
        private static IAppSettings AppSettings { get { return MvcApplication.AppSettings; } }

        public new ILogonContextDataServiceZLDMobile LogonContext { get { return (ILogonContextDataServiceZLDMobile)base.LogonContext; } }


        protected static ILogonContext CreateLogonContext()
        {
            if (AppSettings.IsClickDummyMode)
                return new LogonContextTestZLDMobile();
            
            return new LogonContextDataServiceSqlDatabaseZLDMobile();
        }

        public override string DataContextKey { get { return "ZulMobileErfassungViewModel"; } }

        public ZulMobileErfassungViewModel ViewModel
        {
            get { return SessionStore<ZulMobileErfassungViewModel>.GetModel(() => null); }
            set { SessionStore<ZulMobileErfassungViewModel>.Model = value; }
        }
        
        public LoginController() : base(CreateLogonContext())
        {
        }

        /// <summary>
        /// Anzeige der Login-Seite
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            ViewBag.Message = "";

            return View();
        }

        /// <summary>
        /// Anmeldung durchführen und im Erfolgsfall zum Hauptmenü weiterleiten
        /// </summary>
        /// <param name="logonData"></param>
        /// <returns></returns>
        public ActionResult Anmelden(Anmeldeinformationen logonData)
        {
            if (LogonContext.LogonUser(logonData.UserName, logonData.Password))
            {
                Session["AngemeldeterUser"] = logonData.UserName;
                return RedirectToAction("Selection", "Home");
            }

            ViewBag.Message = "Anmeldung fehlgeschlagen";

            return View("Login");
        }

        /// <summary>
        /// Aktuellen Benutzer abmelden
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            LogonContext.LogoutUser();
            Session["AngemeldeterUser"] = null;

            return View("Login");
        }

        /// <summary>
        /// Passwortänderungsseite anzeigen
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangePassword()
        {
            // Wenn nicht angemeldet, Login erzwingen
            var logonAction = UrlGetLogonAction("", "", "", "");
            if (logonAction != null)
                return logonAction;

            ViewBag.Message = "";

            return View();
        }

        /// <summary>
        /// Passwortänderung durchführen
        /// </summary>
        /// <param name="pwChangeData"></param>
        /// <returns></returns>
        public ActionResult PasswortAendern(Passwortaenderung pwChangeData)
        {
            // Wenn nicht angemeldet, Login erzwingen
            var logonAction = UrlGetLogonAction("", "", "", "");
            if (logonAction != null)
                return logonAction;

            if (pwChangeData.AltesPasswort == pwChangeData.NeuesPasswort)
            {
                ViewBag.Message = "Altes und neues Kennwort dürfen nicht gleich sein";
                return View("ChangePassword");
            }

            if (pwChangeData.NeuesPasswort != pwChangeData.NeuesPasswortConfirm)
            {
                ViewBag.Message = "Neues Kennwort und Kennwortbestätigung stimmen nicht überein";
                return View("ChangePassword");
            }

            if (LogonContext.ChangePassword(pwChangeData.AltesPasswort, pwChangeData.NeuesPasswort))
            {
                ViewBag.Message = "Ihr Kennwort wurde erfolgreich geändert";
            }
            else
            {
                ViewBag.Message = "Kennwortänderung fehlgeschlagen";
            }

            return View("ChangePassword");
        }

    }
}
