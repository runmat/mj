using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CkgDomainLogic.Zulassung.MobileErfassung.Models
{
    /// <summary>
    /// Dieses Objekt enthält die Vorgangsliste sowie ein Objekt mit div. Stammdatenlisten und dient u.a. als 
    /// Hauptdatenstruktur für die LocalStorage-(Offline-)Speicherung
    /// </summary>
    public class Datencontainer
    {
        [Display(Name = "Benutzername")]
        public string Username { get; set; }

        [Display(Name = "Ämter mit aktuellen Vorgängen")]
        public List<AmtVorgaenge> AemterMitVorgaengen { get; set; }

        [Display(Name = "Vorgänge")]
        public List<Vorgang> Vorgaenge { get; set; }

        [Display(Name = "Stammdaten")]
        public Stammdatencontainer Stammdaten { get; set; }

        public Datencontainer(string username)
        {
            Username = username;
            // Stammdaten
            Stammdaten = new Stammdatencontainer();
            // Vorgangsdaten
            AemterMitVorgaengen = new List<AmtVorgaenge>();
            Vorgaenge = new List<Vorgang>();
        }
    }
}
