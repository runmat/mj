using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Leasing.Models
{
    public class ZB1Kopie
    {
        [LocalizedDisplay(LocalizeConstants.CustomerNo)]
        public string Kundennummer { get; set; }
        
        [LocalizedDisplay(LocalizeConstants.ReceivedOn)]
        public DateTime Erstelldatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractNo)]
        public string Vertragsnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string Fahrgestellnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        public DateTime Zulassungsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarOwnerName)]
        public string Haltername { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZB1CopyAvailable)]
        public string ZB1KopieVorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZB1Copy)]
        public string ZB1KopieVorhandenText {
            get 
            {
                if ((!String.IsNullOrEmpty(ZB1KopieVorhanden)) && (ZB1KopieVorhanden.ToUpper() == "JA"))
                {
                    return "Vorhanden";
                }
                else
                {
                    return "Fehlt";
                }
            }
        }

    }
}
