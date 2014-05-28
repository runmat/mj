using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GeneralTools.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    /// <summary>
    /// Equi-Historie
    /// </summary>
    public class EquiHistorie
    {
        [LocalizedDisplay(LocalizeConstants.EquipmentNo)]
        public string Equipmentnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationNo)]
        public string Briefnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants._AltesKennzeichen)]
        public string KennzeichenAlt { get; set; }

        [LocalizedDisplay(LocalizeConstants._AlteBriefnummer)]
        public string BriefnummerAlt { get; set; }

        [LocalizedDisplay(LocalizeConstants._Briefaufbietung)]
        public bool Briefaufbietung { get; set; }

        [LocalizedDisplay(LocalizeConstants._Fahrgestellnummer)]
        public string Fahrgestellnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants._Erstzulassung)]
        public DateTime? Erstzulassungsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.CancellationDate)]
        public DateTime? Abmeldedatum { get; set; }

        [LocalizedDisplay(LocalizeConstants._Zulassungsfaehig)]
        public bool StatusZulassungsfaehig { get; set; }

        [LocalizedDisplay(LocalizeConstants._Zugelassen)]
        public bool StatusZugelassen { get; set; }

        [LocalizedDisplay(LocalizeConstants._Abgemeldet)]
        public bool StatusAbgemeldet { get; set; }

        [LocalizedDisplay(LocalizeConstants._BeiAbmeldung)]
        public bool StatusBeiAbmeldung { get; set; }

        [LocalizedDisplay(LocalizeConstants._OhneZulassung)]
        public bool StatusOhneZulassung { get; set; }

        [LocalizedDisplay(LocalizeConstants._Gesperrt)]
        public bool StatusGesperrt { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string Status
        {
            get
            {
                var erg = "";

                if (StatusZulassungsfaehig)
                {
                    erg = "Zulassungsfähig";
                }
                else if (StatusZugelassen)
                {
                    erg = "Zugelassen";
                }
                else if (StatusAbgemeldet)
                {
                    erg = "Abgemeldet";
                }
                else if (StatusBeiAbmeldung)
                {
                    erg = "Bei Abmeldung";
                }
                else if (StatusOhneZulassung)
                {
                    erg = "Ohne Zulassung";
                }
                else if (StatusGesperrt)
                {
                    erg = "Gesperrt";
                }

                return erg;
            }
        }

        [LocalizedDisplay(LocalizeConstants._CocBescheinigungVorhanden)]
        public bool CocVorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.Location, 1)]
        public string StandortName1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Location, 2)]
        public string StandortName2 { get; set; }

        [LocalizedDisplay(LocalizeConstants._StandortStrasse)]
        public string StandortStrasse { get; set; }

        [LocalizedDisplay(LocalizeConstants._StandortHausnummer)]
        public string StandortHausnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants._StandortPlz)]
        public string StandortPlz { get; set; }

        [LocalizedDisplay(LocalizeConstants._StandortOrt)]
        public string StandortOrt { get; set; }

        [LocalizedDisplay(LocalizeConstants.Location)]
        public string Standort
        {
            get
            {
                return StandortName1 + " " + StandortName2
                    + ((String.IsNullOrEmpty(StandortStrasse) && String.IsNullOrEmpty(StandortHausnummer)) ? ""
                        : Environment.NewLine + StandortStrasse + " " + StandortHausnummer)
                    + ((String.IsNullOrEmpty(StandortPlz) && String.IsNullOrEmpty(StandortOrt)) ? ""
                        : Environment.NewLine + StandortPlz + " " + StandortOrt);
            }
        }

        [LocalizedDisplay(LocalizeConstants.CarOwnerName, 1)]
        public string HalterName1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarOwnerName, 2)]
        public string HalterName2 { get; set; }

        [LocalizedDisplay(LocalizeConstants._HalterStrasse)]
        public string HalterStrasse { get; set; }

        [LocalizedDisplay(LocalizeConstants._HalterHausnummer)]
        public string HalterHausnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants._HalterPlz)]
        public string HalterPlz { get; set; }

        [LocalizedDisplay(LocalizeConstants._HalterOrt)]
        public string HalterOrt { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarOwner)]
        public string Halter
        {
            get
            {
                return HalterName1 + " " + HalterName2
                    + ((String.IsNullOrEmpty(HalterStrasse) && String.IsNullOrEmpty(HalterHausnummer)) ? ""
                        : Environment.NewLine + HalterStrasse + " " + HalterHausnummer)
                    + ((String.IsNullOrEmpty(HalterPlz) && String.IsNullOrEmpty(HalterOrt)) ? ""
                        : Environment.NewLine + HalterPlz + " " + HalterOrt);
            }
        }

        [LocalizedDisplay(LocalizeConstants._AbcKennzeichen)]
        public string AbcKennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants._Versanddatum)]
        public DateTime? Versanddatum { get; set; }

        [LocalizedDisplay(LocalizeConstants._Lagerort)]
        public string Lagerort
        {
            get
            {
                string erg = "";

                switch (AbcKennzeichen)
                {
                    case "":
                        erg = "DAD";
                        break;
                    case "0":
                        erg = "DAD";
                        break;
                    case "1":
                        if ((Versanddatum == null) || (Versanddatum == DateTime.MinValue))
                        {
                            erg = "temporär angefordert";
                        }
                        else
                        {
                            erg = "temporär versendet";
                        }
                        break;
                    case "2":
                        if ((Versanddatum == null) || (Versanddatum == DateTime.MinValue))
                        {
                            erg = "endgültig angefordert";
                        }
                        else
                        {
                            erg = "endgültig versendet";
                        }
                        break;
                }

                return erg;
            }
        }

        [LocalizedDisplay(LocalizeConstants._Ummeldedatum)]
        public DateTime? Ummeldedatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Manufacturer)]
        public string Hersteller { get; set; }

        [LocalizedDisplay(LocalizeConstants._Fahrzeugmodell)]
        public string Fahrzeugmodell { get; set; }

        [LocalizedDisplay(LocalizeConstants._HerstSchluessel)]
        public string HerstellerSchluessel { get; set; }

        [LocalizedDisplay(LocalizeConstants._Typschluessel)]
        public string TypSchluessel { get; set; }

        [LocalizedDisplay(LocalizeConstants._VarianteVersion)]
        public string VarianteVersion { get; set; }

        [LocalizedDisplay(LocalizeConstants._Ordernummer)]
        public string Referenz1 { get; set; }

        [LocalizedDisplay(LocalizeConstants._Meldungsdaten)]
        public List<EquiMeldungsdaten> Meldungsdaten { get; set; }

        public EquiHistorie()
        {
            this.Equipmentnummer = "";
            this.Kennzeichen = "";
            this.Briefnummer = "";
            this.KennzeichenAlt = "";
            this.BriefnummerAlt = "";
            this.Briefaufbietung = false;
            this.Fahrgestellnummer = "";
            this.Erstzulassungsdatum = null;
            this.Abmeldedatum = null;
            this.StatusZulassungsfaehig = false;
            this.StatusZugelassen = false;
            this.StatusAbgemeldet = false;
            this.StatusBeiAbmeldung = false;
            this.StatusOhneZulassung = false;
            this.StatusGesperrt = false;
            this.CocVorhanden = false;
            this.StandortName1 = "";
            this.StandortName2 = "";
            this.StandortStrasse = "";
            this.StandortHausnummer = "";
            this.StandortPlz = "";
            this.StandortOrt = "";
            this.HalterName1 = "";
            this.HalterName2 = "";
            this.HalterStrasse = "";
            this.HalterHausnummer = "";
            this.HalterPlz = "";
            this.HalterOrt = "";
            this.AbcKennzeichen = "";
            this.Versanddatum = null;
            this.Ummeldedatum = null;
            this.Hersteller = "";
            this.Fahrzeugmodell = "";
            this.HerstellerSchluessel = "";
            this.TypSchluessel = "";
            this.VarianteVersion = "";
            this.Referenz1 = "";
            this.Meldungsdaten = new List<EquiMeldungsdaten>();
        }

        public EquiHistorie(string equinr, string kennz, string briefnr, string kennzAlt, string briefnrAlt, bool briefaufbietung, string fahrgestellnr, 
            DateTime? ersetzungsdat, DateTime? abmdat, bool statZulf, bool statZugel, bool statAbg, bool statBeiAbm, bool statOhneZul, bool statGesp, 
            bool coc, string stoName1, string stoName2, string stoStrasse, string stoHausnr, string stoPlz, string stoOrt, string halterName1, 
            string halterName2, string halterStrasse, string halterHausnr, string halterPlz, string halterOrt, string abcKennz, DateTime? versdat, 
            DateTime? ummeldedat, string hersteller, string fzgModell, string herstSchluessel, string typSchluessel, string varVersion, string ref1)
        {
            this.Equipmentnummer = equinr;
            this.Kennzeichen = kennz;
            this.Briefnummer = briefnr;
            this.KennzeichenAlt = kennzAlt;
            this.BriefnummerAlt = briefnrAlt;
            this.Briefaufbietung = briefaufbietung;
            this.Fahrgestellnummer = fahrgestellnr;
            this.Erstzulassungsdatum = ersetzungsdat;
            this.Abmeldedatum = abmdat;
            this.StatusZulassungsfaehig = statZulf;
            this.StatusZugelassen = statZugel;
            this.StatusAbgemeldet = statAbg;
            this.StatusBeiAbmeldung = statBeiAbm;
            this.StatusOhneZulassung = statOhneZul;
            this.StatusGesperrt = statGesp;
            this.CocVorhanden = coc;
            this.StandortName1 = stoName1;
            this.StandortName2 = stoName2;
            this.StandortStrasse = stoStrasse;
            this.StandortHausnummer = stoHausnr;
            this.StandortPlz = stoPlz;
            this.StandortOrt = stoOrt;
            this.HalterName1 = halterName1;
            this.HalterName2 = halterName2;
            this.HalterStrasse = halterStrasse;
            this.HalterHausnummer = halterHausnr;
            this.HalterPlz = halterPlz;
            this.HalterOrt = halterOrt;
            this.AbcKennzeichen = abcKennz;
            this.Versanddatum = versdat;
            this.Ummeldedatum = ummeldedat;
            this.Hersteller = hersteller;
            this.Fahrzeugmodell = fzgModell;
            this.HerstellerSchluessel = herstSchluessel;
            this.TypSchluessel = typSchluessel;
            this.VarianteVersion = varVersion;
            this.Referenz1 = "";
            this.Meldungsdaten = new List<EquiMeldungsdaten>();
        }
    }
}
