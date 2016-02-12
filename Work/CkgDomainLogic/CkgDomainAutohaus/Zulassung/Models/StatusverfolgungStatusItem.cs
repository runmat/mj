using System;
using System.Collections.Generic;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Autohaus.Models
{
    public class StatusverfolgungStatusItem
    {
        [LocalizedDisplay(LocalizeConstants.Status)]
        public int StatusNr { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsFailed { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string StatusText
        {
            get
            {
                switch (StatusNr)
                {
                    case 1:
                        return Localize.RegistrationStatusOrdered;

                    case 2:
                        return Localize.RegistrationStatusOnTheWay;

                    case 3:
                        return (IsFailed ? Localize.RegistrationStatusRegistrationFailed : Localize.RegistrationStatusRegistrationSuccessful);

                    case 4:
                        return Localize.RegistrationStatusOnTheWayBack;

                    case 5:
                        return Localize.RegistrationStatusCompleted;

                    default:
                        return "";
                }
            }
        }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string StatusIconFileName
        {
            get
            {
                var fileNameTemplate = "";

                switch (StatusNr)
                {
                    case 1:
                        fileNameTemplate = "1_beauftragt_{0}.png";
                        break;

                    case 2:
                        fileNameTemplate = "2_unterwegs_{0}.png";
                        break;

                    case 3:
                        fileNameTemplate = "3_bearbeitet_{0}.png";
                        break;

                    case 4:
                        fileNameTemplate = "4_unterwegsZurueck_{0}.png";
                        break;

                    case 5:
                        fileNameTemplate = "5_abgeschlossen_{0}.png";
                        break;
                }

                return string.Format(fileNameTemplate, (IsFailed ? "rot" : IsCompleted ? "blau" : "grau"));
            }
        }

        [LocalizedDisplay(LocalizeConstants.Time)]
        public DateTime? StatusDatumUhrzeit { get; set; }

        [LocalizedDisplay(LocalizeConstants.Time)]
        public string StatusDatumUhrzeitFormatted
        {
            get { return StatusDatumUhrzeit.ToString("dd.MM.yyyy HH:mm:ss"); }
        }

        public List<StatusverfolgungTrackingInfo> TrackingInfos { get; set; }

        public StatusverfolgungStatusItem()
        {
            TrackingInfos = new List<StatusverfolgungTrackingInfo>();
        }
    }
}
