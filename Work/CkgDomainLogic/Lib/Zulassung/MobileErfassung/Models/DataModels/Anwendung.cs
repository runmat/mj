using System.ComponentModel.DataAnnotations;

namespace CkgDomainLogic.Zulassung.MobileErfassung.Models
{
    /// <summary>
    /// Anwendung (für Hauptmenü-Einträge)
    /// </summary>
    public class Anwendung
    {
        [Display(Name = "Bezeichnung")]
        public string AppBezeichnung { get; set; }

        [Display(Name = "Action")]
        public string AppAction { get; set; }

        [Display(Name = "Controller")]
        public string AppController { get; set; }

        public Anwendung(string appBez, string appAction, string appController)
        {
            AppBezeichnung = appBez;
            AppAction = appAction;
            AppController = appController;
        }
    }
}
