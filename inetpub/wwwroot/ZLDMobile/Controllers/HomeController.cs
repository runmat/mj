using System.Web.Mvc;
using MvcTools.Controllers;
using GeneralTools.Contracts;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Zulassung.MobileErfassung.Contracts;
using CkgDomainLogic.Zulassung.MobileErfassung.Services;
using CkgDomainLogic.Zulassung.MobileErfassung.ViewModels;
using MvcTools.Web;
using CkMobileMvcTools.Services;

namespace ZLDMobile.Controllers
{
    /// <summary>
    /// Controller für Startseiten-/Hauptmenü-Aufruf
    /// </summary>
    public class HomeController : LogonCapableController
    {
        private static IAppSettings AppSettings { get { return MvcApplication.AppSettings; } }

        public new ILogonContextDataServiceZLDMobile LogonContext { get { return (ILogonContextDataServiceZLDMobile)base.LogonContext; } }

        private IZulMobileErfassungDataService ZulMobileErfassungDataService
        {
            get
            {
                if (AppSettings.IsClickDummyMode)
                {
                    return new ZulMobileErfassungDataServiceTest(LogonContext);
                }

                return new ZulMobileErfassungDataServiceSAP(S.AP, AppSettings, LogonContext);
            }
        }

        protected static ILogonContext CreateLogonContext()
        {
            if (AppSettings.IsClickDummyMode)
            {
                return new LogonContextTestZLDMobile();
            }

            return new LogonContextDataServiceSqlDatabaseZLDMobile();
        }

        public override string DataContextKey { get { return "ZulMobileErfassungViewModel"; } }

        public ZulMobileErfassungViewModel ViewModel
        {
            get { return SessionStore<ZulMobileErfassungViewModel>.GetModel(() => null); }
            set { SessionStore<ZulMobileErfassungViewModel>.Model = value; }
        }

        public HomeController() : base(CreateLogonContext())
        {
        }

        /// <summary>
        /// Default-Action, leitet zum Hauptmenü weiter
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            // Wenn nicht angemeldet, Login erzwingen
            var logonAction = UrlGetLogonAction("", "", "", "");
            if (logonAction != null)
                return logonAction;

            return RedirectToAction("Selection");
        }

        /// <summary>
        /// Aufruf des Hauptmenüs (bei nur einer Anwendung wird direkt dorthin gesprungen)
        /// </summary>
        /// <returns></returns>
        public ActionResult Selection()
        {
            // Wenn nicht angemeldet, Login erzwingen
            var logonAction = UrlGetLogonAction("", "", "", "");
            if (logonAction != null)
                return logonAction;

            if (ViewModel == null)
            {
                ViewModel = new ZulMobileErfassungViewModel(ZulMobileErfassungDataService, LogonContext);
            }

            // Wenn nur eine Anwendung zur Auswahl, diese automatisch aufrufen
            if ((ViewModel.Anwendungen != null) && (ViewModel.Anwendungen.Count == 1))
            {
                return RedirectToAction(ViewModel.Anwendungen[0].AppAction, ViewModel.Anwendungen[0].AppController);
            }

            return View(ViewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Ihre App-Beschreibungsseite.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Ihre Kontaktseite.";

            return View();
        }

    }
}
