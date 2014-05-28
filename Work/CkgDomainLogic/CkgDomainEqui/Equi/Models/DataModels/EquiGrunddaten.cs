using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    /// <summary>
    /// Grunddaten-/Equipment-Datensatz
    /// </summary>
    public class EquiGrunddaten
    {
        [LocalizedDisplay(LocalizeConstants.VIN17)]
        public string Fahrgestellnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.VIN10)]
        public string Fahrgestellnummer10 { get; set; }

        [LocalizedDisplay(LocalizeConstants.RecordingDate)]
        public DateTime? Erfassungsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants._ZBIINr)]
        public string TechnIdentnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants._Erstzulassung)]
        public DateTime? Erstzulassungsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Cancellation)]
        public DateTime? Abmeldedatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.FactoryNoShort)]
        public string Betrieb { get; set; }

        [LocalizedDisplay(LocalizeConstants._Standortkuerzel)]
        public string Standort { get; set; }

        [LocalizedDisplay(LocalizeConstants.Location)]
        public string StandortBez { get; set; }

        [LocalizedDisplay(LocalizeConstants.Coc)]
        public bool CocVorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants._EingangCoc)]
        public DateTime? EingangCoc { get; set; }

        [LocalizedDisplay(LocalizeConstants._Modell)]
        public string Handelsname { get; set; }

        [LocalizedDisplay(LocalizeConstants.Destination)]
        public string Zielort { get; set; }

        public EquiGrunddaten()
        {
            this.Fahrgestellnummer = "";
            this.Fahrgestellnummer10 = "";
            this.Erfassungsdatum = null;
            this.Kennzeichen = "";
            this.TechnIdentnummer = "";
            this.Erstzulassungsdatum = null;
            this.Abmeldedatum = null;
            this.Betrieb = "";
            this.Standort = "";
            this.StandortBez = "";
            this.CocVorhanden = false;
            this.EingangCoc = null;
            this.Handelsname = "";
        }

        public EquiGrunddaten(string fgnr, string fgnr10, DateTime? erdat, string kennz, string tidnr, DateTime? ersdat, DateTime? abmdat, 
            string betrieb, string stort, string stortbez, bool coc, DateTime? cocdat, string handelsname)
        {
            this.Fahrgestellnummer = fgnr;
            this.Fahrgestellnummer10 = fgnr10;
            this.Erfassungsdatum = erdat;
            this.Kennzeichen = kennz;
            this.TechnIdentnummer = tidnr;
            this.Erstzulassungsdatum = ersdat;
            this.Abmeldedatum = abmdat;
            this.Betrieb = betrieb;
            this.Standort = stort;
            this.StandortBez = stortbez;
            this.CocVorhanden = coc;
            this.EingangCoc = cocdat;
            this.Handelsname = handelsname;
        }
    }
}
