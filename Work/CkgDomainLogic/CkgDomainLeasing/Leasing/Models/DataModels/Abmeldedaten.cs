using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Leasing.Models
{
    public class Abmeldedaten
    {
        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string KennzeichenShow { get { return Kennzeichen; } }
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.LeasingContractNo)]
        public string LeasingvertragsnummerShow { get { return Leasingvertragsnummer; } }
        public string Leasingvertragsnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationNo)]
        public string BriefnummerShow { get { return Briefnummer; } }
        public string Briefnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string FahrgestellnummerShow { get { return Fahrgestellnummer; } }
        public string Fahrgestellnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfDeregistrationReceipt)]
        public DateTime AbmeldeeingangShow { get { return Abmeldeeingang; } }
        public DateTime Abmeldeeingang { get; set; }

        [LocalizedDisplay(LocalizeConstants.Approval)]
        public bool FreigabeShow { get { return Freigabe; } }

        [LocalizedDisplay(LocalizeConstants.Approval)]
        [GridExportIgnore]
        public bool Freigabe { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string StatusShow { get { return Status; } }
        public string Status { get; set; }

        public string Fehler { get; set; }

    }
}
