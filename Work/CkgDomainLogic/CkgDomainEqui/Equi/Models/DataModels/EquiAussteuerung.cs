using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    public class EquiAussteuerung
    {
        [LocalizedDisplay(LocalizeConstants.MinimumHoldingDate)]
        public DateTime? Mindesthaltedatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeRegistrationAssignment)]
        public DateTime? Abmeldebeauftragung { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeRegistrationPerformance)]
        public DateTime? Abmeldedurchfuehrung { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeRegistrationDate)]
        public DateTime? Abmeldedatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.CrashedVehicle)]
        public bool Unfallfahrzeug { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarportArrival)]
        public DateTime? CarportEingang { get; set; }

        public string CarportId { get; set; }

        public string CarportName { get; set; }

        [LocalizedDisplay(LocalizeConstants.Carport)]
        public string Carport { get { return String.Format("{0} {1}", CarportId, CarportName); } }

        [LocalizedDisplay(LocalizeConstants.NumberOfLicensePlatesAvailable)]
        public string AnzahlKennzeichenVorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.Zb1Available)]
        public bool Zb1Vorhanden { get; set; }
    }
}
