using System;
using GeneralTools.Models;

namespace CkgDomainLogic.Zanf.Models
{
    public class ZanfAdresse
    {
        public string AnforderungsNr { get; set; }

        public string AuftragsNr { get; set; }

        public string Partnerrolle { get; set; }

        public string Anrede { get; set; }

        public string Name1 { get; set; }

        public string Name2 { get; set; }

        public string Strasse { get; set; }

        public string Hausnummer { get; set; }

        public string Plz { get; set; }

        public string Ort { get; set; }

        public string AdresseZeile1 { get { return String.Format("{0} {1} {2}", Anrede, Name1, Name2).Trim(); } }

        public string AdresseZeile2 { get { return String.Format("{0} {1}", Strasse, Hausnummer).Trim(); } }

        public string AdresseZeile3 { get { return String.Format("{0} {1}", Plz, Ort).Trim(); } }

        public string Adresse
        {
            get
            {
                return AdresseZeile1 +
                    (AdresseZeile2.IsNotNullOrEmpty() ? Environment.NewLine : "") + AdresseZeile2 +
                    (AdresseZeile3.IsNotNullOrEmpty() ? Environment.NewLine : "") + AdresseZeile3;
            }
        }
    }
}
