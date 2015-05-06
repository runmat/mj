using System;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class Treuhandfreigabe
    {

        public bool IsSelected { get; set; }


        [LocalizedDisplay(LocalizeConstants.Name)]
        public string Name { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }
           
        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string Fahrgestellnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationNo)]
        public string Zb2Nummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.NameAG)]
        public string NameAG { get; set; }

        // [LocalizedDisplay(LocalizeConstants.NameAG)]
        public string AGNummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.NameTG)]
        public string NameTG { get; set; }

        // [LocalizedDisplay(LocalizeConstants.NameAG)]
        public string TGNummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractNo)]
        public string Vertragsnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.ShippingState)]
        public string Versandstatus { get; set; }

        [LocalizedDisplay(LocalizeConstants.ShippingAddress)]
        public string Versandadresse { get; set; }

        [LocalizedDisplay(LocalizeConstants.References)]
        public string Referenz { get; set; }

        [LocalizedDisplay(LocalizeConstants.Originator)]
        public string Ersteller { get; set; }

        [LocalizedDisplay(LocalizeConstants.OriginationDateOf)]
        public DateTime? Erstelldatum { get; set; }
    }
}
