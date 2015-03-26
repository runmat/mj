using System;
using System.Globalization;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.WFM.Models
{
    public class WfmInfo
    {
        [LocalizedDisplay(LocalizeConstants.SequenceNo)]
        public string LaufendeNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.User)]
        public string User { get; set; }

        [LocalizedDisplay(LocalizeConstants.Date)]
        public DateTime? Datum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Time)]
        public string Zeit { get; set; }

        [LocalizedDisplay(LocalizeConstants.Date)]
        public DateTime? DatumZeit
        {
            get
            {
                DateTime tmpZeit;
                var tmpDat = Datum;

                if (tmpDat.HasValue
                    && DateTime.TryParseExact(Zeit, "HHmmss", CultureInfo.CurrentCulture, DateTimeStyles.None, out tmpZeit))
                {
                    return tmpDat.Value.AddHours(tmpZeit.Hour).AddMinutes(tmpZeit.Minute).AddSeconds(tmpZeit.Second);
                }

                return tmpDat;
            }
        }

        [LocalizedDisplay(LocalizeConstants.ToDoWho)]
        public string ToDoWer { get; set; }

        [LocalizedDisplay(LocalizeConstants.InfoText)]
        public string Text { get; set; }
    }
}
