using RefImplBibl.Interfaces;
using RefImplBibl.Models;

namespace RefImpl.Services
{
    public class AnwenderInfoProvider : IAnwenderInfoProvider
    {
        public Anwender GetAnwender()
        {
            return new Anwender
            {
                Nachname = "Mustermann",
                Vorname = "Max",
                Rolle = "Sachbearbeiter",
                Mandant = "Tesla",
                Anmeldename = "max.mustermann"
            };
        }

        public override string ToString()
        {
            var anwender = GetAnwender();
            return string.Format(@"Nachname = {0}
Vorname = {1}
Rolle = {2}
Mandant = {3}
Anmeldename = {4}", 
                  
                  anwender.Nachname, anwender.Vorname, anwender.Rolle, anwender.Mandant, anwender.Anmeldename);
        }
    }
}