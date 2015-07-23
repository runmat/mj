using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Finance.Models
{
    public class Versendung
    {
        [LocalizedDisplay(LocalizeConstants.RequestType)]
        public string Anforderungsart { get; set; }

        [LocalizedDisplay(LocalizeConstants.CauseOfDispatch)]
        public string Versandgrund { get; set; }

        [LocalizedDisplay(LocalizeConstants.Account)]
        public string Kontonummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.CIN)]
        public string CIN { get; set; }

        [LocalizedDisplay(LocalizeConstants.PAID)]
        public string PAID { get; set; }

        [LocalizedDisplay(LocalizeConstants.SecurityIdCMS)]
        public string SicherheitsIdCMS { get; set; }

        [LocalizedDisplay(LocalizeConstants.CreateDate)]
        public DateTime? Anlagedatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZB2No)]
        public string Zb2Nummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.DispatchType)]
        public string Versandart { get; set; }

        [LocalizedDisplay(LocalizeConstants.RecipientType)]
        public string EmpfaengerArt { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name)]
        public string EmpfaengerName { get; set; }

        [LocalizedDisplay(LocalizeConstants.FirstName)]
        public string EmpfaengerVorname { get; set; }

        [LocalizedDisplay(LocalizeConstants.Title)]
        public string EmpfaengerAnrede { get; set; }

        [LocalizedDisplay(LocalizeConstants.Street)]
        public string EmpfaengerStrasse { get; set; }

        [LocalizedDisplay(LocalizeConstants.HouseNo)]
        public string EmpfaengerHausnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.PostCode)]
        public string EmpfaengerPostleitzahl { get; set; }

        [LocalizedDisplay(LocalizeConstants.City)]
        public string EmpfaengerOrt { get; set; }

        [LocalizedDisplay(LocalizeConstants.Country)]
        public string EmpfaengerLand { get; set; }

        [LocalizedDisplay(LocalizeConstants.SystemSign)]
        public string Systemkennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.CustomerID)]
        public string AuftraggeberId { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name)]
        public string AnfordererName { get; set; }

        [LocalizedDisplay(LocalizeConstants.FirstName)]
        public string AnfordererVorname { get; set; }

        [LocalizedDisplay(LocalizeConstants.Title)]
        public string AnfordererAnrede { get; set; }

        [LocalizedDisplay(LocalizeConstants.Street)]
        public string AnfordererStrasse { get; set; }

        [LocalizedDisplay(LocalizeConstants.HouseNo)]
        public string AnfordererHausnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.PostCode)]
        public string AnfordererPostleitzahl { get; set; }

        [LocalizedDisplay(LocalizeConstants.City)]
        public string AnfordererOrt { get; set; }

        [LocalizedDisplay(LocalizeConstants.Country)]
        public string AnfordererLand { get; set; }

        [LocalizedDisplay(LocalizeConstants.Phone1)]
        public string AnfordererTelefon1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Phone2)]
        public string AnfordererTelefon2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Mobile)]
        public string AnfordererMobil { get; set; }

        [LocalizedDisplay(LocalizeConstants.Fax1)]
        public string AnfordererFax1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Fax2)]
        public string AnfordererFax2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Email)]
        public string AnfordererEmail { get; set; }

        [LocalizedDisplay(LocalizeConstants.Info)]
        public string AnfordererInfo { get; set; }

        [LocalizedDisplay(LocalizeConstants.ClientLd)]
        public bool ClientLd { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChangeDate)]
        public DateTime? Aenderungsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.AcquisitionDate)]
        public DateTime? Uebernahmedatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.TransmissionDate)]
        public DateTime? Uebermittlungsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Annulation)]
        public bool StornoVersand { get; set; }

        [LocalizedDisplay(LocalizeConstants.PicklistForm)]
        public string PicklistenFormular { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name)]
        public string AnsprechpartnerName { get; set; }

        [LocalizedDisplay(LocalizeConstants.FirstName)]
        public string AnsprechpartnerVorname { get; set; }

        [LocalizedDisplay(LocalizeConstants.Title)]
        public string AnsprechpartnerAnrede { get; set; }

        [LocalizedDisplay(LocalizeConstants.AuthorizationUser)]
        public string AutorisierungsUser { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractType)]
        public string Vertragsart { get; set; }

        [LocalizedDisplay(LocalizeConstants.FinalDispatch)]
        public DateTime? EndgueltigerVersand { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfDispatch)]
        public DateTime? Versanddatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.CauseOfDispatch)]
        public string VersandgrundText { get; set; }

        [LocalizedDisplay(LocalizeConstants.WayOfRequest)]
        public string Anforderungsweg { get; set; }
    }
}
