using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    public class EquiBelastungsanzeige
    {
        [LocalizedDisplay(LocalizeConstants.SequenceNo)]
        public string LaufendeNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.CreationDate)]
        public DateTime? Erstellungsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Sum)]
        public decimal? Summe { get; set; }

        [LocalizedDisplay(LocalizeConstants.Sum)]
        public string SummeFormatted { get { return (Summe.HasValue ? string.Format("{0} €", Summe) : ""); } }

        [LocalizedDisplay(LocalizeConstants.Surveyor)]
        public string Gutachter { get; set; }

        [LocalizedDisplay(LocalizeConstants.SurveyId)]
        public string GutachtenId { get; set; }

        [LocalizedDisplay(LocalizeConstants.Mileage)]
        public decimal? KmStand { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string Status { get; set; }

        [LocalizedDisplay(LocalizeConstants.DamageBillNo)]
        public string SchadenrechnungsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfDamageBill)]
        public DateTime? DatumSchadenrechnung { get; set; }

        [LocalizedDisplay(LocalizeConstants.ObjectionText)]
        public string WiderspruchText { get; set; }

        [LocalizedDisplay(LocalizeConstants.ObjectionDate)]
        public DateTime? WiderspruchDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.BlockText)]
        public string BlockadeText { get; set; }

        [LocalizedDisplay(LocalizeConstants.BlockDate)]
        public DateTime? BlockadeDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.BlockUser)]
        public string BlockadeUser { get; set; }
    }
}
