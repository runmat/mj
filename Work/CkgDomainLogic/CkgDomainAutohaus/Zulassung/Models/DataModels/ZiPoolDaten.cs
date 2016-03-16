using System.Collections.Generic;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Services;

namespace CkgDomainLogic.Autohaus.Models
{
    public class ZiPoolDaten
    {
        public ZiPoolGrunddaten Grunddaten { get; set; }

        public List<ZiPoolDetaildaten> Details { get; set; }

        public static List<Domaenenfestwert> WaehlbareDienstleistungen
        {
            get
            {
                return new List<Domaenenfestwert>
                {
                    new Domaenenfestwert { Wert = "ZUL", Beschreibung = Localize.Registration },
                    new Domaenenfestwert { Wert = "UMS", Beschreibung = Localize.ReRegistration },
                    new Domaenenfestwert { Wert = "UMK", Beschreibung = Localize.Relabelling },
                    new Domaenenfestwert { Wert = "EFS", Beschreibung = Localize.SpareVehicleRegistration }
                };
            }
        }

        public ZiPoolDaten()
        {
            Grunddaten = new ZiPoolGrunddaten();
            Details = new List<ZiPoolDetaildaten>();
        }
    }
}
