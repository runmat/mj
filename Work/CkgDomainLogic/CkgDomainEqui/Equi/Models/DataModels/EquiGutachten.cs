using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    public class EquiGutachten
    {
        [LocalizedDisplay(LocalizeConstants.SequenceNo)]
        public string LaufendeNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Surveyor)]
        public string Gutachter { get; set; }

        [LocalizedDisplay(LocalizeConstants.SurveyReceipt)]
        public DateTime? EingangGutachten { get; set; }

        [LocalizedDisplay(LocalizeConstants.RepairCalculation)]
        public string ReparaturKalkulation { get; set; }
    }
}
