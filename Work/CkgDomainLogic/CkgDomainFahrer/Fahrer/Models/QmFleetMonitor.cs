using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrer.Models
{
    public class QmFleetMonitor
    {
        [LocalizedDisplay(LocalizeConstants.Assessment)]
        public string Bewertung { get; set; }

        [LocalizedDisplay(LocalizeConstants.Kindness)]
        public decimal? Freundlichkeit { get; set; }

        [LocalizedDisplay(LocalizeConstants.Appearance)]
        public decimal? Erscheinungsbild { get; set; }

        [LocalizedDisplay(LocalizeConstants.Professionalism)]
        public decimal? Professionalitaet { get; set; }

        [LocalizedDisplay(LocalizeConstants.Punctuality)]
        public decimal? Puenktlichkeit { get; set; }

        [LocalizedDisplay(LocalizeConstants.OverallImpression)]
        public decimal? GesamtEindruck { get; set; }

        [LocalizedDisplay(LocalizeConstants.Admission)]
        public decimal? Einweisung { get; set; }

        [LocalizedDisplay(LocalizeConstants.VehicleState)]
        public decimal? FahrzeugZustand { get; set; }
    }
}
