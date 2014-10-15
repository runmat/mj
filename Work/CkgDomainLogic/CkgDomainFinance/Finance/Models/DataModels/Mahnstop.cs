using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Finance.Models
{
    public class Mahnstop
    {
        public string EquiNr { get; set; }

        public string MaterialNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.PAID)]
        public string PAID { get; set; }

        [LocalizedDisplay(LocalizeConstants.Account)]
        public string Kontonummer { get; set; }

        public string CIN { get; set; }

        [LocalizedDisplay(LocalizeConstants.Document)]
        public string Dokument { get; set; }

        [LocalizedDisplay(LocalizeConstants.DunningBlock)]
        public bool Mahnsperre { get; set; }

        [LocalizedDisplay(LocalizeConstants.DunningStopUntil)]
        public DateTime? MahnstopBis { get; set; }

        [LocalizedDisplay(LocalizeConstants.Comment)]
        public string Bemerkung { get; set; }

        [GridExportIgnore]
        [ModelMappingCompareIgnore]
        public bool IsEdited { get; set; }
    }
}