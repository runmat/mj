using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using GeneralTools.Contracts;
using GeneralTools.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Zulassung.MobileErfassung.ViewModels;
using MvcTools.Web;
using CkgDomainLogic.Zulassung.MobileErfassung.Models;
using System.Globalization;
using CkgDomainLogic.General.Controllers;

namespace ZLDMobile.Controllers
{
    /// <summary>
    /// Controller für die eigentlichen Anwendungsseiten "Mobile Erfassung"
    /// </summary>
    public class ErfassungMobilController : LogonCapableController
    {
        private static IAppSettings AppSettings { get { return MvcApplication.AppSettings; } }

        public new ILogonContextDataServiceZLDMobile LogonContext { get { return (ILogonContextDataServiceZLDMobile)base.LogonContext; } }

        static protected ILogonContext CreateLogonContext()
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

        public ErfassungMobilController() : base(CreateLogonContext())
        {
        }

        /// <summary>
        /// Seite zur Vorgangsbearbeitung anzeigen
        /// </summary>
        /// <returns></returns>
        public ActionResult EditZLDVorgaenge()
        {
            // Wenn nicht angemeldet, Login erzwingen
            var logonAction = UrlGetLogonAction("", "", "", "");
            if (logonAction != null)
                return logonAction;

            return View(ViewModel);
        }

        /// <summary>
        /// Seite zur Vorgangserstellung anzeigen
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateZLDVorgang()
        {
            // Wenn nicht angemeldet, Login erzwingen
            var logonAction = UrlGetLogonAction("", "", "", "");
            if (logonAction != null)
                return logonAction;

            return View(ViewModel);
        }

        /// <summary>
        /// Lädt die Haupt-Datenstruktur ZLDMobileData inkl. Stammdaten
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoadDatenstruktur()
        {
            // Wenn nicht angemeldet, Login erzwingen
            if (UrlGetLogonAction("", "", "", "") != null)
                return Json(JsonConvert.SerializeObject("unauthenticated"));

            return Json(JsonConvert.SerializeObject(ViewModel.ZLDMobileData));
        }

        /// <summary>
        /// Lädt die aktuellen Ämter für den angemeldeten User
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoadAemterVorgaenge()
        {
            // Wenn nicht angemeldet, Login erzwingen
            if (UrlGetLogonAction("", "", "", "") != null)
                return Json(JsonConvert.SerializeObject("unauthenticated"));

            List<AmtVorgaenge> aemterMitVorgaengen = ViewModel.GetAemterVorgaenge();
            // Daten vor JSON-Übergabe aufbereiten
            foreach (AmtVorgaenge amv in aemterMitVorgaengen)
            {
                amv.ZulDatText = DateTime.Today.ToString("dd.MM.yyyy");
            }
            return Json(JsonConvert.SerializeObject(aemterMitVorgaengen));
        }

        /// <summary>
        /// Lädt die aktuelle Vorgangsliste für den angemeldeten User
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoadVorgaenge()
        {
            // Wenn nicht angemeldet, Login erzwingen
            if (UrlGetLogonAction("", "", "", "") != null)
                return Json(JsonConvert.SerializeObject("unauthenticated"));

            List<Vorgang> vorgaenge = ViewModel.GetVorgaenge();
            // Daten vor JSON-Übergabe aufbereiten
            foreach (Vorgang vg in vorgaenge)
            {
                vg.AmtEdit = vg.Amt;
                vg.ZulDatText = vg.ZulDat.ToString("dd.MM.yyyy");
                vg.ZulDatTextEdit = vg.ZulDatText;
                vg.StatusDurchgefuehrt = (vg.Status == "2");
                vg.StatusFehlgeschlagen = (vg.Status == "F");
                vg.VersandzulVkBur = ((vg.BlTyp == "VZ" || vg.BlTyp == "AV") ? vg.VkBur : "");
            }
            return Json(JsonConvert.SerializeObject(vorgaenge));
        }

        /// <summary>
        /// Speichert einen empfangenen Vorgang und sendet das Resultat als Antwort
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveVorgang(string vorgang)
        {
            // Wenn nicht angemeldet, Login erzwingen
            if (UrlGetLogonAction("", "", "", "") != null)
                return Json(JsonConvert.SerializeObject("unauthenticated"));

            VorgangTransaktionsErgebnis erg;

            try
            {
                Vorgang vorg = JsonConvert.DeserializeObject<Vorgang>(vorgang);
                // Daten nach JSON-Übergabe übernehmen
                vorg.Amt = vorg.AmtEdit;

                DateTime tmpDat;
                if (DateTime.TryParseExact(vorg.ZulDatTextEdit, "dd.MM.yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out tmpDat))
                {
                    if (tmpDat > DateTime.MinValue)
                    {
                        vorg.ZulDat = tmpDat;
                    }
                }

                if (vorg.StatusDurchgefuehrt)
                {
                    vorg.Status = "2";
                }
                else if (vorg.StatusFehlgeschlagen)
                {
                    vorg.Status = "F";
                }
                // ggf. Infotext abschneiden, falls zu lang
                if ((!String.IsNullOrEmpty(vorg.Infotext)) && (vorg.Infotext.Length > 40))
                {
                    vorg.Infotext = vorg.Infotext.Substring(0, 40);
                }

                string speicherResult = ViewModel.SaveVorgang(vorg);

                if (String.IsNullOrEmpty(speicherResult))
                {
                    erg = new VorgangTransaktionsErgebnis(vorg.Id, "OK", "");
                }
                else if (speicherResult == "ein oderer mehrere Sätze konnten nicht geändert werden") // Text kommt genau so aus SAP
                {
                    // Vorgang wurde nicht gespeichert, weil er bereits bearbeitet/durchgeführt wurde
                    erg = new VorgangTransaktionsErgebnis(vorg.Id, "OK", "NOTSAVED");
                }
                else
                {
                    erg = new VorgangTransaktionsErgebnis(vorg.Id, "ERROR", speicherResult);
                }
            }
            catch (Exception ex)
            {
                erg = new VorgangTransaktionsErgebnis("0", "APPERROR", "Fehler beim Speichern: " + ex.Message);
                Services.ErrorLogging.WriteErrorToLogFile("Fehler beim Speichern (SaveVorgang): " + ex.Message, ex.ToString());
            }

            return Json(JsonConvert.SerializeObject(erg));
        }

        /// <summary>
        /// Lädt die aktuellen Stammdaten (Stammdatenobjekt mit div. Listen)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoadStammdaten()
        {
            // Wenn nicht angemeldet, Login erzwingen
            if (UrlGetLogonAction("", "", "", "") != null)
                return Json(JsonConvert.SerializeObject("unauthenticated"));

            return Json(JsonConvert.SerializeObject(ViewModel.ZLDMobileData.Stammdaten));
        }

        /// <summary>
        /// Sendet "OK" als Antwort - kann genutzt werden, um zu prüfen, ob der Server erreichbar ist
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ReturnHeartbeat()
        {
            return Json("OK");
        }

        /// <summary>
        /// Gibt für die angegebenen Vorgänge die BEB-Stati zurück
        /// </summary>
        /// <param name="vorgangIds"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetBEBStatusVorgaenge(string vorgangIds)
        {
            // Wenn nicht angemeldet, Login erzwingen
            if (UrlGetLogonAction("", "", "", "") != null)
                return Json(JsonConvert.SerializeObject("unauthenticated"));

            List<string> vorgIds = JsonConvert.DeserializeObject<List<string>>(vorgangIds);
            return Json(JsonConvert.SerializeObject(ViewModel.GetVorgangBEBStatus(vorgIds)));
        }

        /// <summary>
        /// Lädt die selektierbaren VkBurs für den angemeldeten User
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoadVkBurListe()
        {
            // Wenn nicht angemeldet, Login erzwingen
            if (UrlGetLogonAction("", "", "", "") != null)
                return Json(JsonConvert.SerializeObject("unauthenticated"));

            List<string> vkBurs = ViewModel.GetVkBurs();

            return Json(JsonConvert.SerializeObject(vkBurs));
        }

        /// <summary>
        /// Lädt die Stammdaten zum gewählten VkBur und gibt eine entsprechend initialisierte Eingabemaske zurück
        /// </summary>
        /// <param name="vkBur"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ApplyVkBur(string vkBur)
        {
            // Wenn nicht angemeldet, Login erzwingen
            if (UrlGetLogonAction("", "", "", "") != null)
                return Json(JsonConvert.SerializeObject("unauthenticated"));

            ViewModel.ApplyVkBur(vkBur);

            return PartialView("CreateZLDVorgang/CreateVorgangPartial", ViewModel);
        }

        /// <summary>
        /// Speichert einen neu angelegten Vorgang und sendet das Resultat als Antwort
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveNewVorgang(string vorgang)
        {
            // Wenn nicht angemeldet, Login erzwingen
            if (UrlGetLogonAction("", "", "", "") != null)
                return Json(JsonConvert.SerializeObject("unauthenticated"));

            VorgangTransaktionsErgebnis erg;

            try
            {
                Vorgang vorg = JsonConvert.DeserializeObject<Vorgang>(vorgang);
                // Daten nach JSON-Übergabe übernehmen
                vorg.Amt = vorg.AmtEdit;

                DateTime tmpDat;
                if (DateTime.TryParseExact(vorg.ZulDatTextEdit, "dd.MM.yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out tmpDat))
                {
                    if (tmpDat > DateTime.MinValue)
                    {
                        vorg.ZulDat = tmpDat;
                    }
                }

                vorg.Id = "Z000000001";
                vorg.VkOrg = LogonContext.VkOrg;
                vorg.VkBur = ViewModel.VkBurNeuanlage;
                vorg.Referenz1 = vorg.Referenz1.NotNullOrEmpty().ToUpper();
                vorg.Referenz2 = vorg.Referenz2.NotNullOrEmpty().ToUpper();
                vorg.Kennzeichen = vorg.Kennzeichen.NotNullOrEmpty().ToUpper();

                vorg.Positionen.ForEach(p => p.KopfId = vorg.Id);

                string speicherResult = ViewModel.SaveVorgang(vorg);

                if (String.IsNullOrEmpty(speicherResult))
                {
                    erg = new VorgangTransaktionsErgebnis(vorg.Id, "OK", "");
                }
                else
                {
                    erg = new VorgangTransaktionsErgebnis(vorg.Id, "ERROR", speicherResult);
                }
            }
            catch (Exception ex)
            {
                erg = new VorgangTransaktionsErgebnis("0", "APPERROR", "Fehler beim Speichern: " + ex.Message);
                Services.ErrorLogging.WriteErrorToLogFile("Fehler beim Speichern (SaveVorgang): " + ex.Message, ex.ToString());
            }

            return Json(JsonConvert.SerializeObject(erg));
        }
    }
}
