using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Finance.Models
{
    public class Pruefpunkt
    {
        [LocalizedDisplay(LocalizeConstants.AccountNo)]
        public string Kontonummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.PAID)]
        public string PAID { get; set; }

        public string BucID { get; set; }

        [LocalizedDisplay(LocalizeConstants.ActionName)]
        public string Aktionsname { get; set; }

        public string PruefpunktID { get; set; }

        [LocalizedDisplay(LocalizeConstants.CheckPoint)]
        public string PruefpunktText { get; set; }

        public string Pruefstatus { get; set; }

        [LocalizedDisplay(LocalizeConstants.Result)]
        public string Ergebnis { get; set; }

        [LocalizedDisplay(LocalizeConstants.Checkdate)]
        public DateTime? Pruefdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Comment)]
        public string Bemerkung { get; set; }
    }
}
