using System;
using CkgDomainLogic.DomainCommon.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Autohaus.Models
{
    public class Adressdaten
    {
        public string Partnerrolle { get; set; }

        public string BelegNr { get; set; }

        public Adresse Adresse { get; set; }

        public string Name { get { return String.Format("{0}{1}", Adresse.Name1, (Adresse.Name2.IsNullOrEmpty() ? "" : " " + Adresse.Name2)); } }

        public bool AdresseVollstaendig { get { return (Adresse.Name1.IsNotNullOrEmpty() && Adresse.Strasse.IsNotNullOrEmpty() && Adresse.PLZ.IsNotNullOrEmpty() && Adresse.Ort.IsNotNullOrEmpty()); } }

        public Adressdaten()
        {
        }

        public Adressdaten(string kennungAdresse, string land = "DE")
        {
            Adresse = new Adresse { Kennung = kennungAdresse, Land = land };
        }
    }
}
