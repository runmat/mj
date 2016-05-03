using System;
using System.Collections.Generic;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    public class EquiHistorieRemarketing
    {
        [LocalizedDisplay(LocalizeConstants.ZB2No)]
        public string BriefNr { get; set; }

        public string EreignisText { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfZb2Receipt)]
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

        public EquiGutachtendaten Gutachtendaten { get; set; }

        public EquiVersanddaten Versanddaten { get; set; }



        

        public EquiEinsteuerung Einsteuerungsdaten { get; set; }

        public EquiAussteuerung Aussteuerungsdaten { get; set; }

        public EquiTypdaten Typdaten { get; set; }

        public List<EquiMeldungsdaten> LebenslaufZb2 { get; set; }

        public List<EquiMeldungsdaten> LebenslaufFsm { get; set; }

        public List<EquiTueteninhalt> InhalteFsm { get; set; }

        public List<FahrzeugAnforderung> FahrzeugAnforderungen { get; set; }

        public bool FahrzeugAnforderungenAnzeigen { get; set; }
    }
}
