using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    /// <summary>
    /// Equipment für Mahnreport
    /// </summary>
    public class EquiMahn
    {
        [LocalizedDisplay(LocalizeConstants.ContractNo)]
        public string VertragsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string FahrgestellNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.FirstRegistration)]
        public DateTime? Erstzulassung { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfDispatch)]
        public DateTime? Versanddatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.OverdueSince)]
        public DateTime? UeberfaelligSeit { get; set; }

        [LocalizedDisplay(LocalizeConstants.DunningLevel)]
        public string Mahnstufe { get; set; }

        [LocalizedDisplay(LocalizeConstants.Recipient)]
        public string EmpfaengerName { get; set; }

        [LocalizedDisplay(LocalizeConstants.Street)]
        public string EmpfaengerStrasse { get; set; }

        [LocalizedDisplay(LocalizeConstants.PostCode)]
        public string EmpfaengerPlz { get; set; }

        [LocalizedDisplay(LocalizeConstants.City)]
        public string EmpfaengerOrt { get; set; }
    }
}
