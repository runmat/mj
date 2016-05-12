using System;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CkgDomainLogic.Equi.Models
{
    /// <summary>
    /// Erweiterter Fahrzeugbrief-Datensatz
    /// </summary>
    public class FahrzeugbriefErweitert
    {
        [LocalizedDisplay(LocalizeConstants.EquipmentNo)]
        public string Equipmentnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string Fahrgestellnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractNo)]
        public string Vertragsnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants._ZBIINr)]
        public string TechnIdentnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants._AbcKennzeichen)]
        public string AbcKennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.Room)]
        public string Raum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Location)]
        public string Standort { get; set; }

        [LocalizedDisplay(LocalizeConstants.Location)]
        public string StandortText { get { return ((!String.IsNullOrEmpty(AbcKennzeichen) && AbcKennzeichen == "1") ? Localize.TempDispatchedSing : "DAD"); } }

        [LocalizedDisplay(LocalizeConstants.CauseOfDispatch)]
        public string Versandgrund { get; set; }

        [LocalizedDisplay(LocalizeConstants.CauseOfDispatch)]
        public string VersandgrundText { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfReceipt)]
        public DateTime? Eingangsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfDispatch)]
        public DateTime? Versanddatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfDeactivation)]
        public DateTime? Stilllegungsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.PickDate)]
        public DateTime? Pickdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Reference1)]
        public string Referenz1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Reference2)]
        public string Referenz2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name1)]
        public string Name1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name2)]
        public string Name2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Street)]
        public string Strasse { get; set; }

        [LocalizedDisplay(LocalizeConstants.HouseNo)]
        public string Hausnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.PostCode)]
        public string PLZ { get; set; }

        [LocalizedDisplay(LocalizeConstants.City)]
        public string Ort { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractStart)]
        public DateTime? VertragsBeginn { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractEnd)]
        public DateTime? VertragsEnde { get; set; }

        [LocalizedDisplay(LocalizeConstants.CustomerContinuance)]
        public string VertragsStatus { get; set; }

        [LocalizedDisplay(LocalizeConstants.BlockingNotice)]
        [Required]
        public List<string> SperrvermerkListe
        {
            get
            {
                var list = new List<string>();
                list.Add("");
                list.Add("Sicherungsübereignung");
                list.Add("Verbundhaftung");
                list.Add("C+R");
                list.Add("Other");
                return list;
            }
        }

        public string SAPError { get; set; }

    }
}
