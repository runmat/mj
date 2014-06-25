using System;
using System.Globalization;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Finance.Models
{
    public class TelefoniedatenItem
    {
        [LocalizedDisplay(LocalizeConstants.CustomerNo)]
        public string Kundennummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.Account)]
        public string Kontonummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.CIN)]
        public string CIN { get; set; }

        [LocalizedDisplay(LocalizeConstants.PAID)]
        public string PAID { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractType)]
        public string Vertragsart { get; set; }

        [LocalizedDisplay(LocalizeConstants.PhoneNo)]
        public string Telefonnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.CallType)]
        public string Anrufart { get; set; }

        [LocalizedDisplay(LocalizeConstants.CallDate)]
        public DateTime? Anrufdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.CallTimeFrom)]
        public string AnrufzeitVon { get; set; }

        public DateTime? AnrufStart
        {
            get 
            {
                DateTime startZeit;
                var tmpDat = Anrufdatum;

                if (tmpDat.HasValue 
                    && DateTime.TryParseExact(AnrufzeitVon, "HH:mm", CultureInfo.CurrentCulture, DateTimeStyles.None, out startZeit))
                {
                    return tmpDat.Value.AddHours(startZeit.Hour).AddMinutes(startZeit.Minute);
                }

                return tmpDat;
            }
        }

        [LocalizedDisplay(LocalizeConstants.CallTimeTo)]
        public string AnrufzeitBis { get; set; }

        [LocalizedDisplay(LocalizeConstants.CallDuration)]
        public string Anrufdauer
        {
            get
            {
                DateTime startZeit;
                DateTime endeZeit;

                if (!String.IsNullOrEmpty(AnrufzeitVon) 
                    && !String.IsNullOrEmpty(AnrufzeitBis)
                    && DateTime.TryParseExact(AnrufzeitVon, "HH:mm", CultureInfo.CurrentCulture, DateTimeStyles.None, out startZeit)
                    && DateTime.TryParseExact(AnrufzeitBis, "HH:mm", CultureInfo.CurrentCulture, DateTimeStyles.None, out endeZeit))
                {
                    return String.Format("{0} Min.", Math.Truncate((endeZeit - startZeit).TotalMinutes).ToString());
                }

                return ""; 
            }
        }

        [LocalizedDisplay(LocalizeConstants.CallerName)]
        public string Anrufername { get; set; }

        [LocalizedDisplay(LocalizeConstants.CauseOfCall)]
        public string Anrufgrund { get; set; }

        [LocalizedDisplay(LocalizeConstants.CauseOfCall)]
        public string AnrufgrundDetail { get; set; }

        [LocalizedDisplay(LocalizeConstants.CauseOfCall)]
        public string AnrufgrundGesamt { get { return String.Format("{0} - {1}", Anrufgrund, AnrufgrundDetail); } }
    }
}
