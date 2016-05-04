using System;
using System.Collections.Generic;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    public class EquiHistorieRemarketing
    {
        public string EreignisText { get; set; }

        [LocalizedDisplay(LocalizeConstants.Zb2Receipt)]
        public DateTime? EingangZb2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.KeyReceipt)]
        public DateTime? EingangSchluessel { get; set; }

        [LocalizedDisplay(LocalizeConstants.EquipmentNo)]
        public string Equipmentnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfDelivery)]
        public DateTime? Auslieferdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        public DateTime? Zulassungsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.HcReceipt)]
        public DateTime? HcEingang { get; set; }

        [LocalizedDisplay(LocalizeConstants.HcCity)]
        public string HcOrt { get; set; }

        [LocalizedDisplay(LocalizeConstants.Mileage)]
        public string KmStand { get; set; }

        [LocalizedDisplay(LocalizeConstants.Marketing)]
        public bool Vermarktung { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateContractAdversity)]
        public DateTime? DatumVertragswidrigkeit { get; set; }

        [LocalizedDisplay(LocalizeConstants.TypeContractAdversity)]
        public string ArtVertragswidrigkeit { get; set; }

        [LocalizedDisplay(LocalizeConstants.TuevFeedback)]
        public DateTime? TuevRueckmeldung { get; set; }

        [LocalizedDisplay(LocalizeConstants.TuevManuallyAssigned)]
        public DateTime? TuevManuellBeauftragt { get; set; }

        [LocalizedDisplay(LocalizeConstants.RentalVehicleBillingDate)]
        public DateTime? MietfahrzeugAbrechnungsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.RentalVehicleBuyBackBillCreated)]
        public DateTime? MietfahrzeugRueckkaufrechnungErstellt { get; set; }

        public EquiHistorieRemarketingInfo HistorieInfo { get; set; }

        public List<EquiGutachten> Gutachten { get; set; }

        public EquiVersanddaten Versanddaten { get; set; }

        public List<EquiLebenslaufBrief> LebenslaufBrief { get; set; }

        public List<EquiLebenslaufSchluessel> LebenslaufSchluessel { get; set; }

        public List<SelectItem> Adressen { get; set; }

        public List<EquiBelastungsanzeige> Belastungsanzeigen { get; set; }

        public List<EquiRechnungsdaten> Rechnungsdaten { get; set; }

        public List<EquiVorschaden> Vorschäden { get; set; }

        public EquiZusatzdatenRemarketing Zusatzdaten { get; set; }

        public List<EquiAusstattung> Ausstattungen { get; set; }

        public EquiSchadenrechnung Schadenrechnung { get; set; }

        public EquiTypdaten Typdaten { get; set; }

        public List<EquiLebenslauf> Lebenslauf { get; set; }
    }
}
