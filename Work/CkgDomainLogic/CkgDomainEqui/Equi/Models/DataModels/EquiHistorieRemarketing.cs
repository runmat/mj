using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
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
        public decimal? KmStand { get; set; }

        [LocalizedDisplay(LocalizeConstants.LossSelfMarketing)]
        public bool Selbstvermarktung { get; set; }

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

        public int AnzahlReparaturKalkulationen
        {
            get
            {
                if (Gutachten == null || Gutachten.None())
                    return 0;

                var maxLfdNr = Gutachten.Max(g => g.LaufendeNr.ToInt(0));

                return (maxLfdNr == 0 ? 0 : Gutachten.First(g => g.LaufendeNr.ToInt(0) == maxLfdNr).ReparaturKalkulation.ToInt(0));
            }
        }

        public bool HasReparaturKalkulation
        {
            get { return (AnzahlReparaturKalkulationen > 0); }
        }

        public bool HasTuevGutachten
        {
            get { return (Gutachten != null && Gutachten.Any(g => g.Gutachter == "TUEV")); }
        }

        public string TuevGutachtenBaseUrl { get; set; }

        public string TuevGutachtenUrl
        {
            get { return (string.IsNullOrEmpty(TuevGutachtenBaseUrl) ? "" : string.Format("{0}{1}", TuevGutachtenBaseUrl, HistorieInfo.FahrgestellNr)); }
        }

        public EquiVersanddaten Versanddaten { get; set; }

        public List<EquiLebenslaufBrief> LebenslaufBrief { get; set; }

        [LocalizedDisplay(LocalizeConstants.Zb2Dispatch)]
        public DateTime? AusgangZb2
        {
            get { return (LebenslaufBrief == null ? null : LebenslaufBrief.FirstOrDefault(new EquiLebenslaufBrief()).AusgangZb2); }
        }

        public List<EquiLebenslaufSchluessel> LebenslaufSchluessel { get; set; }

        [LocalizedDisplay(LocalizeConstants.KeyDispatch)]
        public DateTime? AusgangSchluessel
        {
            get { return (LebenslaufSchluessel == null ? null : LebenslaufSchluessel.FirstOrDefault(new EquiLebenslaufSchluessel()).AusgangSchluessel); }
        }

        public List<SelectItem> Adressen { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarRentalCompany)]
        public string Vermieter
        {
            get { return (Adressen == null ? "" : Adressen.FirstOrDefault(a => a.Key == "VERM", new SelectItem()).Text); }
        }

        [LocalizedDisplay(LocalizeConstants.TakeInCenter)]
        public string Hereinnahmecenter
        {
            get { return (Adressen == null ? "" : Adressen.FirstOrDefault(a => a.Key == "HC", new SelectItem()).Text); }
        }

        public EquiBelastungsanzeige Belastungsanzeige { get; set; }

        public bool HasBelastungsanzeige { get { return (Belastungsanzeige != null && !string.IsNullOrEmpty(Belastungsanzeige.LaufendeNr)); } }

        public string UploaddatumSchadensgutachten { get; set; }

        public bool HasSchadensgutachten { get { return (!string.IsNullOrEmpty(UploaddatumSchadensgutachten)); } }

        public List<EquiRechnungsdaten> Rechnungsdaten { get; set; }

        public List<EquiVorschaden> Vorschaeden { get; set; }

        [LocalizedDisplay(LocalizeConstants.PreviousDamages)]
        public string HatVorschaeden { get { return (Vorschaeden.AnyAndNotNull() ? Localize.Yes : Localize.No); } }

        public EquiZusatzdatenRemarketing Zusatzdaten { get; set; }

        public List<EquiAusstattung> Ausstattungen { get; set; }

        public EquiAusstattung AusstattungModell
        {
            get { return (Ausstattungen == null ? new EquiAusstattung() : Ausstattungen.FirstOrDefault(a => a.PaketTyp == "M", new EquiAusstattung())); }
        }

        public EquiAusstattung AusstattungAussenfarbe
        {
            get { return (Ausstattungen == null ? new EquiAusstattung() : Ausstattungen.FirstOrDefault(a => a.PaketTyp == "A", new EquiAusstattung())); }
        }

        public EquiAusstattung AusstattungInnenfarbe
        {
            get { return (Ausstattungen == null ? new EquiAusstattung() : Ausstattungen.FirstOrDefault(a => a.PaketTyp == "I", new EquiAusstattung())); }
        }

        public List<EquiAusstattung> AusstattungenSonstige
        {
            get { return (Ausstattungen == null ? new List<EquiAusstattung>() : Ausstattungen.Where(a => a.PaketTyp.In("P,E")).OrderBy(a => a.PaketId).ToList()); }
        }

        public EquiSchadenrechnung Schadenrechnung { get; set; }

        public bool HasRechnung
        {
            get { return (Schadenrechnung != null && !string.IsNullOrEmpty(Schadenrechnung.RechnungsNr)); }
        }

        public EquiTypdaten Typdaten { get; set; }

        public List<EquiLebenslauf> Lebenslauf { get; set; }
    }
}
