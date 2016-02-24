using System;
using System.Collections.Generic;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Autohaus.Models
{
    public class StatusverfolgungDetails
    {
        [LocalizedDisplay(LocalizeConstants.Status)]
        public string AktuellerStatusCode { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string AktuellerStatusText
        {
            get
            {
                switch (AktuellerStatusCode)
                {
                    case "1":
                    case "4":
                    case "5":
                    case "A":
                        return string.Format("{0} / {1}", Localize.RegistrationStatusOrdered, Localize.RegistrationStatusOnTheWay);

                    case "2":
                        return string.Format("{0} / {1}", Localize.RegistrationStatusRegistrationSuccessful, Localize.RegistrationStatusOnTheWayBack);

                    case "F":
                        return Localize.RegistrationStatusRegistrationFailed;

                    case "S":
                        return Localize.RegistrationStatusCancelled;

                    case "L":
                        return Localize.RegistrationStatusDeleted;

                    case "7":
                        return Localize.RegistrationStatusCompleted;

                    default:
                        return "";
                }
            }
        }

        [LocalizedDisplay(LocalizeConstants.Time)]
        public DateTime? AktuellerStatusDatumUhrzeit { get; set; }

        [LocalizedDisplay(LocalizeConstants.Time)]
        public string AktuellerStatusDatumUhrzeitFormatted
        {
            get { return AktuellerStatusDatumUhrzeit.ToString("dd.MM.yyyy HH:mm:ss").Replace(" 00:00:00", ""); }
        }

        public ZulassungsReportModel Zulassungsdaten { get; set; }

        public List<StatusverfolgungStatusItem> Statusverfolgungsdaten { get; set; }

        public StatusverfolgungDetails()
        {
            Zulassungsdaten = new ZulassungsReportModel();
            Statusverfolgungsdaten = new List<StatusverfolgungStatusItem>();
        }
    }
}
