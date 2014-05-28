using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrer.Models
{
    public class QmFahrer
    {
        [GridHidden]
        public string CodeGruppe { get; set; }

        [LocalizedDisplay(LocalizeConstants.Code)]
        public string SchadensCode { get; set; }

        [LocalizedDisplay(LocalizeConstants.ShortText)]
        public string KatalogText { get; set; }

        [LocalizedDisplay(LocalizeConstants.Amount)]
        public decimal? MengeFehler { get; set; }

        [LocalizedDisplay(LocalizeConstants.PreviousYear)]
        public decimal? MengeVorjahr { get; set; }


        [GridHidden]
        public bool RowIsHighlighted { get { return MengeFehler.GetValueOrDefault() != MengeVorjahr.GetValueOrDefault(); } }
    }
}
