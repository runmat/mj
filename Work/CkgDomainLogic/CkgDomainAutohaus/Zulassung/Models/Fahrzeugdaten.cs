using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.Autohaus.ViewModels;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Autohaus.Models
{
    public class Fahrzeugdaten : IValidatableObject
    {
        private string _kostenstelle;
        private string _bestellNr;
        private string _auftragsNr;
        private string _fahrgestellNr;
        private string _kennzeichen;
        private int _kennzeichenMenge = 1;

        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<KroschkeZulassungViewModel> GetZulassungViewModel { get; set; }

        [LocalizedDisplay(LocalizeConstants.AhZulassungReferenceNo)]
        public string AuftragsNr
        {
            get { return _auftragsNr.NotNullOrEmpty().ToUpper(); }
            set { _auftragsNr = value.NotNullOrEmpty().ToUpper(); }
        }

        [LocalizedDisplay(LocalizeConstants.VIN)]
        [RequiredConditional]
        public string FahrgestellNr
        {
            get { return _fahrgestellNr.NotNullOrEmpty().ToUpper(); }
            set { _fahrgestellNr = value.NotNullOrEmpty().ToUpper(); }
        }

        [LocalizedDisplay(LocalizeConstants.ZB2)]
        public string Zb2Nr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Autohaus_KroschkePrimeKennzeichenetikette_Info)]
        public bool HasEtikett { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarColor)]
        [RequiredConditional]
        public string Farbe { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarModel)]
        [RequiredConditional]
        public string FzgModell { get; set; }

        [LocalizedDisplay(LocalizeConstants.VehicleType)]
        [Required]
        public string FahrzeugartId { get; set; }

        public Domaenenfestwert Fahrzeugart
        {
            get
            {
                if (FahrzeugartList == null)
                    return new Domaenenfestwert();

                var option = FahrzeugartList.FirstOrDefault(fa => fa.Wert == FahrzeugartId);
                if (option == null)
                    return new Domaenenfestwert();

                return option;
            }
        }

        [XmlIgnore]
        static public List<Domaenenfestwert> FahrzeugartList { get; set; }

        public bool IstAnhaenger { get { return (FahrzeugartId.NotNullOrEmpty().Trim() == "3"); } }

        public bool IstMotorrad { get { return (FahrzeugartId.NotNullOrEmpty().Trim() == "5"); } }

        [LocalizedDisplay(LocalizeConstants.AhZulassungSalesman)]
        public string VerkaeuferKuerzel { get; set; }

        [LocalizedDisplay(LocalizeConstants.AhZulassungCostcenter)]
        public string Kostenstelle
        {
            get { return _kostenstelle.NotNullOrEmpty().ToUpper(); }
            set { _kostenstelle = value.NotNullOrEmpty().ToUpper(); }
        }

        [LocalizedDisplay(LocalizeConstants.AhZulassungOrderNo)]
        public string BestellNr
        {
            get { return _bestellNr.NotNullOrEmpty().ToUpper(); }
            set { _bestellNr = value.NotNullOrEmpty().ToUpper(); }
        }

        [LocalizedDisplay(LocalizeConstants.TuevAu)]
        [RequiredConditional]
        public string TuevAu { get; set; }

        [LocalizedDisplay(LocalizeConstants.Holder)]
        public string HalterName { get { return GetZulassungViewModel().Zulassung.Halter.Adresse.GetFullName(); } }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string KennzeichenHinweis { get { return Localize.WillBeFilledAutomatically; } }

        [XmlIgnore]
        Rechnungsdaten AG { get { return GetZulassungViewModel().Zulassung.Rechnungsdaten; } }
        [XmlIgnore]
        public Adresse AgAdresse { get { return AG.GetKunde(GetZulassungViewModel().Kunden).Adresse; } }
        [XmlIgnore]
        public string AgName { get { return AG.GetKunde(GetZulassungViewModel().Kunden).Adresse.GetFullName(); } }
        [XmlIgnore]
        public string AgKundenNr { get { return AG.GetKunde(GetZulassungViewModel().Kunden).KundenNr.NotNullOrEmpty().TrimStart('0'); } }

        public int AnzahlHinzuzufuegendeFahrzeuge { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        [Kennzeichen]
        [RequiredConditional]
        public string Kennzeichen
        {
            get { return _kennzeichen.NotNullOrEmpty().ToUpper(); }
            set { _kennzeichen = value.NotNullOrEmpty().ToUpper(); }
        }

        [LocalizedDisplay(LocalizeConstants.Amount)]
        [RequiredConditional]
        [Range(1.0, 1999.0)]
        public int KennzeichenMenge
        {
            get { return _kennzeichenMenge; }
            set { _kennzeichenMenge = value; }
        }

        [LocalizedDisplay(LocalizeConstants.LicensePlateType)]
        public string ErsatzKennzeichenTyp { get; set; }

        [XmlIgnore]
        public List<SelectItem> ErsatzKennzeichenTypen { get { return GetZulassungViewModel().Zulassung.ErsatzKennzeichenTypen; } }

        [LocalizedDisplay(LocalizeConstants.Service)]
        public string HaendlerKennzeichenTyp { get; set; }

        [XmlIgnore]
        public List<SelectItem> HaendlerKennzeichenTypen { get { return GetZulassungViewModel().Zulassung.HaendlerKennzeichenTypen; } }

        public string GetSummaryString()
        {
            var s = AuftragsNr.PrependIfNotNull(string.Format("{0}: ", Localize.AhZulassungReferenceNo));

            const string mask = "<br/>{0}: ";
            s += Fahrzeugart.Beschreibung.PrependIfNotNull(string.Format(mask, Localize.VehicleType));
            s += FahrgestellNr.PrependIfNotNull(string.Format(mask, Localize.VIN));
            s += VerkaeuferKuerzel.PrependIfNotNull(string.Format(mask, Localize.AhZulassungSalesman));
            s += Zb2Nr.PrependIfNotNull(string.Format(mask, Localize.ZB2));
            s += BestellNr.PrependIfNotNull(string.Format(mask, Localize.AhZulassungOrderNo));
            s += Kostenstelle.PrependIfNotNull(string.Format(mask, Localize.AhZulassungCostcenter));

            return s;
        }

        public string GetSummaryStringErsatzkennzeichen(DateTime? zulassungsDatum)
        {
            const string mask = "<br/>{0}: ";
            var vm = GetZulassungViewModel();

            var s = AuftragsNr.PrependIfNotNull(string.Format("{0}: ", Localize.AhZulassungReferenceNo));

            s += zulassungsDatum?.ToShortDateString().PrependIfNotNull(string.Format(mask, Localize.RegistrationDate));

            var kk = vm.Zulassung.ErsatzKennzeichenTypen.FirstOrDefault(k => k.Key == vm.Zulassung.Zulassungsdaten.ZulassungsartMatNr);
            if (kk != null)
                s += kk.Text.PrependIfNotNull("<br/>");

            s += vm.Zulassung.Zulassungsdaten.Kennzeichen.PrependIfNotNull(string.Format(mask, Localize.LicenseNo));

            s += VerkaeuferKuerzel.PrependIfNotNull(string.Format(mask, Localize.AhZulassungSalesman));
            s += BestellNr.PrependIfNotNull(string.Format(mask, Localize.AhZulassungOrderNo));
            s += Kostenstelle.PrependIfNotNull(string.Format(mask, Localize.AhZulassungCostcenter));

            return s;
        }

        public string GetSummaryStringHaendlerkennzeichen(DateTime? zulassungsDatum)
        {
            const string mask = "<br/>{0}: ";
            var vm = GetZulassungViewModel();

            var s = AuftragsNr.PrependIfNotNull(string.Format("{0}: ", Localize.AhZulassungReferenceNo));

            s += zulassungsDatum?.ToShortDateString().PrependIfNotNull(string.Format(mask, Localize.RegistrationDate));

            var kk = vm.Zulassung.HaendlerKennzeichenTypen.FirstOrDefault(k => k.Key == vm.Zulassung.Zulassungsdaten.ZulassungsartMatNr);
            if (kk != null)
                s += kk.Text.PrependIfNotNull("<br/>");

            s += vm.Zulassung.Zulassungsdaten.ZulassungsartMenge.ToString().PrependIfNotNull(string.Format(mask, Localize.Amount));
            s += vm.Zulassung.Zulassungsdaten.Kennzeichen.PrependIfNotNull(string.Format(mask, Localize.LicenseNo));

            s += VerkaeuferKuerzel.PrependIfNotNull(string.Format(mask, Localize.AhZulassungSalesman));
            s += BestellNr.PrependIfNotNull(string.Format(mask, Localize.AhZulassungOrderNo));
            s += Kostenstelle.PrependIfNotNull(string.Format(mask, Localize.AhZulassungCostcenter));

            return s;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var vm = GetZulassungViewModel();
            if (vm.SonderzulassungsMode == SonderzulassungsMode.Ersatzkennzeichen)
            {
                if (Kennzeichen.IsNullOrEmpty())
                    yield return new ValidationResult(Localize.FieldIsRequired, new[] { "Kennzeichen" });
                if (FahrgestellNr.IsNullOrEmpty())
                    yield return new ValidationResult(Localize.FieldIsRequired, new[] { "FahrgestellNr" });
            }

            if (vm.SonderzulassungsMode == SonderzulassungsMode.Haendlerkennzeichen)
            {
                if (Kennzeichen.IsNullOrEmpty())
                    yield return new ValidationResult(Localize.FieldIsRequired, new[] { "Kennzeichen" });
            }
        }
    }
}
