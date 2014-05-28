using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace SapORM.Models
{
    public class HistoryAuftragFilter 
    {
        // ReSharper disable LocalizableElement

        [LocalizedDisplay(LocalizeConstants.OrderID)]
        [RequiredAsGroup]
        public string AuftragsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants._ErfassungsdatumVon)]
        [RequiredAsGroup]
        public DateTime? ErfassungsDatumVon { get; set; }

        [LocalizedDisplay(LocalizeConstants._Bis)]
        [RequiredAsGroup]
        public DateTime? ErfassungsDatumBis { get; set; }

        [LocalizedDisplay(LocalizeConstants._AuftragsdatumVon)]
        [RequiredAsGroup]
        public DateTime? UeberfuehrungsDatumVon { get; set; }

        [LocalizedDisplay(LocalizeConstants._Bis)]
        [RequiredAsGroup]
        public DateTime? UeberfuehrungsDatumBis { get; set; }

        [RequiredAsGroup]
        public string Referenz { get; set; }

        [LocalizedDisplay(LocalizeConstants._Auftragsart)]
        public string AuftragsArt { get; set; }

        [LocalizedDisplay(LocalizeConstants._AlleOrganisationen)]
        public bool AlleOrganisationen { get; set; }

        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.CustomerNo)]
        public string KundenNr { get; set; }

        [LocalizedDisplay(LocalizeConstants._KundenReferenz)]
        public string KundenReferenz { get; set; }

        // ReSharper restore LocalizableElement
    }
}
