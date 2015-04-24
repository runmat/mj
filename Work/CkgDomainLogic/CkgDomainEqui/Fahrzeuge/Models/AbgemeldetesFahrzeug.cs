using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    [GridColumnsAutoPersist]
    public class AbgemeldetesFahrzeug
    {
        [LocalizedDisplay(LocalizeConstants.Status)]
        public string Status { get; set; }

        [LocalizedDisplay(LocalizeConstants.Description)]
        public string StatusBezeichnung { get; set; }

        [LocalizedDisplay(LocalizeConstants.VIN17)]
        public string FIN { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReturnDate)]
        public DateTime? RueckgabeDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Site)]
        public string Standort { get; set; }

        [LocalizedDisplay(LocalizeConstants.Type)]
        public string Art { get; set; }

        [LocalizedDisplay(LocalizeConstants.Department)]
        public string Abteilung { get; set; }

        [LocalizedDisplay(LocalizeConstants.KM)]
        public string Kilometer { get; set; }

        [LocalizedDisplay(LocalizeConstants.FactoryNoShort)]
        public string Betriebsnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.CheckText)]
        public string Bemerkung { get; set; }

        [LocalizedDisplay(LocalizeConstants.Model)]
        public string Modell { get; set; }

        [LocalizedDisplay(LocalizeConstants.VIN10)]
        public string FIN10 { get; set; }

        [LocalizedDisplay(LocalizeConstants.CancellationOrder)]
        public DateTime? AbmeldeAuftragDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Cancellation)]
        public DateTime? AbmeldeDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.CostCenter)]
        public string Kostenstelle { get; set; }

        [LocalizedDisplay(LocalizeConstants.Destination)]
        public string Zielort { get; set; }

        [LocalizedDisplay(LocalizeConstants.DepartmentHead)]
        public string AbteilungsLeiter { get; set; }

        [GridHidden]
        public bool HistorieAvailable { get { return Art.IsNotNullOrEmpty() || Bemerkung.IsNotNullOrEmpty(); } }

        [GridHidden]
        public string HistorieCssClass { get { return HistorieAvailable ? "" : "hide"; } }


        [GridExportIgnore]
        [LocalizedDisplay(LocalizeConstants.Action)]
        public string Action1 { get; set; }

        [GridExportIgnore]
        [LocalizedDisplay(LocalizeConstants.ActionDot)]
        public string Action2 { get; set; }
    }
}
