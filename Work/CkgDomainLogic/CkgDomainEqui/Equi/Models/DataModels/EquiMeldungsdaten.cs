using System;
using System.ComponentModel.DataAnnotations;
using GeneralTools.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    /// <summary>
    /// Equi-Meldungsdatensatz
    /// </summary>
    public class EquiMeldungsdaten
    {
        [LocalizedDisplay(LocalizeConstants._Meldungsnummer)]
        public string Meldungsnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants._Durchfuehrung)]
        public DateTime? Durchfuehrungsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants._Erfassungsdatum)]
        public DateTime? Erfassungsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants._Kurztext)]
        public string Kurztext { get; set; }

        [LocalizedDisplay(LocalizeConstants._BeauftragtDurch)]
        public string BeauftragtDurch { get; set; }

        [LocalizedDisplay(LocalizeConstants._VersandName, 1)]
        public string VersandName1 { get; set; }

        [LocalizedDisplay(LocalizeConstants._VersandName, 2)]
        public string VersandName2 { get; set; }

        [LocalizedDisplay(LocalizeConstants._VersandStrasse)]
        public string VersandStrasse { get; set; }

        [LocalizedDisplay(LocalizeConstants._VersandHausnummer)]
        public string VersandHausnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants._VersandPlz)]
        public string VersandPlz { get; set; }

        [LocalizedDisplay(LocalizeConstants._VersandOrt)]
        public string VersandOrt { get; set; }

        [LocalizedDisplay(LocalizeConstants.ShippingAddress)]
        public string Versandadresse
        {
            get
            {
                return VersandName1 + " " + VersandName2
                       + ((String.IsNullOrEmpty(VersandStrasse) && String.IsNullOrEmpty(VersandHausnummer)) ? ""
                            : Environment.NewLine + VersandStrasse + " " + VersandHausnummer)
                       + ((String.IsNullOrEmpty(VersandPlz) && String.IsNullOrEmpty(VersandOrt)) ? ""
                            : Environment.NewLine + VersandPlz + " " + VersandOrt);
            }
        }

        [LocalizedDisplay(LocalizeConstants._Versandart)]
        public string Versandart { get; set; }

        public EquiMeldungsdaten()
        {
            this.Meldungsnummer = "";
            this.Durchfuehrungsdatum = null;
            this.Erfassungsdatum = null;
            this.Kurztext = "";
            this.BeauftragtDurch = "";
            this.VersandName1 = "";
            this.VersandName2 = "";
            this.VersandStrasse = "";
            this.VersandHausnummer = "";
            this.VersandPlz = "";
            this.VersandOrt = "";
            this.Versandart = "";
        }

        public EquiMeldungsdaten(string meldungsnr, DateTime? durchfdat, DateTime? erdat, string kurztext, string beauftragtdurch, 
            string vers_name1, string vers_name2, string vers_strasse, string vers_hausnr, string vers_plz, string vers_ort, string versart)
        {
            this.Meldungsnummer = meldungsnr;
            this.Durchfuehrungsdatum = durchfdat;
            this.Erfassungsdatum = erdat;
            this.Kurztext = kurztext;
            this.BeauftragtDurch = beauftragtdurch;
            this.VersandName1 = vers_name1;
            this.VersandName2 = vers_name2;
            this.VersandStrasse = vers_strasse;
            this.VersandHausnummer = vers_hausnr;
            this.VersandPlz = vers_plz;
            this.VersandOrt = vers_ort;
            this.Versandart = versart;
        }
    }
}
