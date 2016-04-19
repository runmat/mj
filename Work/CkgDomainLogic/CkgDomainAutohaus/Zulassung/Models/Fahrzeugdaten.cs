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

        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<KroschkeZulassungViewModel> GetZulassungViewModel { get; set; }

        [LocalizedDisplay(LocalizeConstants.AhZulassungReferenceNo)]
        public string AuftragsNr
        {
            get { return _auftragsNr.NotNullOrEmpty().ToUpper(); }
            set { _auftragsNr = value.NotNullOrEmpty().ToUpper(); }
        }

        [LocalizedDisplay(LocalizeConstants.VIN)]
        [Required]
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

        [LocalizedDisplay(LocalizeConstants.LicensePlateType)]
        public string KennzeichenTyp { get; set; }

        [XmlIgnore]
        public List<SelectItem> KennzeichenTypen { get { return GetZulassungViewModel().Zulassung.KennzeichenTypen; } }

        public string GetSummaryString()
        {
            var s = string.Format("{0}: {1}", Localize.OrderNumber, AuftragsNr);
            s += string.Format("<br/>{0}: {1}", Localize.VehicleType, Fahrzeugart.Beschreibung);
            s += string.Format("<br/>{0}: {1}", Localize.VIN, FahrgestellNr);
            s += string.Format("<br/>{0}: {1}", Localize.SellerAbbreviation, VerkaeuferKuerzel);
            s += string.Format("<br/>{0}: {1}", Localize.ZB2, Zb2Nr);
            s += string.Format("<br/>{0}: {1}", Localize.CostCenter, Kostenstelle);
            s += string.Format("<br/>{0}: {1}", Localize.OrderCode, BestellNr);

            return s;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var vm = GetZulassungViewModel();
            if (vm.ModusSonderzulassungErsatzkennzeichen)
            {
                if (Kennzeichen.IsNullOrEmpty())
                    yield return new ValidationResult(Localize.FieldIsRequired, new[] { "Kennzeichen" });
            }
        }
    }
}
