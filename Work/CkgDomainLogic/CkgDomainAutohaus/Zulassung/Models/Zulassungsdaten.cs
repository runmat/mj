using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.Autohaus.ViewModels;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Autohaus.Models
{
    public class Zulassungsdaten : IValidatableObject
    {
        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<KroschkeZulassungViewModel> GetZulassungViewModel { get; set; }

        private string _wunschkennzeichen2;
        private string _wunschkennzeichen3;
        private string _kennzeichen;

        [Required]
        [LocalizedDisplay(LocalizeConstants.RegistrationType)]
        public string ZulassungsartMatNr { get; set; }

        public Material Zulassungsart
        {
            get
            {
                if (MaterialList == null)
                    return new Material();

                var option = MaterialList.FirstOrDefault(m => m.MaterialNr == ZulassungsartMatNr);
                if (option == null)
                    return new Material();

                return option;
            }
        }

        [XmlIgnore]
        static List<Material> MaterialList { get { return GetZulassungViewModel == null ? new List<Material>() : GetZulassungViewModel().Zulassungsarten; } }

        public string Belegtyp { get { return Zulassungsart.Belegtyp; } }

        [Required]
        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        public DateTime? Zulassungsdatum { get; set; }

        [Required]
        [StringLength(3)]
        [LocalizedDisplay(LocalizeConstants.RegistrationDistrict)]
        public string Zulassungskreis { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationDistrict)]
        public string ZulassungskreisBezeichnung { get; set; }

        [LocalizedDisplay(LocalizeConstants.EvbNumber)]
        public string EvbNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.PersonalisedLicenseNo)]
        public string Kennzeichen
        {
            get { return _kennzeichen.NotNullOrEmpty().ToUpper(); }
            set { _kennzeichen = value.NotNullOrEmpty().ToUpper(); }
        }

        [LocalizedDisplay(LocalizeConstants.PersonalisedNumberPlate)]
        public bool WunschkennzeichenVorhanden
        {
            get
            {
                return (KennzeichenReserviert ||
                        (KennzeichenIsValid(Kennzeichen) || KennzeichenIsValid(Wunschkennzeichen2) || KennzeichenIsValid(Wunschkennzeichen3)));
            }
        }

        public static string ZulassungskreisToKennzeichenLinkeSeite(string zulassungsKreis)
        {
            return string.Format("{0}-", zulassungsKreis.NotNullOrEmpty().ToUpper().RemoveDigits());
        }

        public static bool KennzeichenIsValid(string kennnzeichen)
        {
            return kennnzeichen.IsNotNullOrEmpty() && !kennnzeichen.LastCharIs('-');
        }

        [LocalizedDisplay(LocalizeConstants.PersonalisedLicenseNo2)]
        public string Wunschkennzeichen2
        {
            get { return _wunschkennzeichen2.NotNullOrEmpty().ToUpper(); }
            set { _wunschkennzeichen2 = value.NotNullOrEmpty().ToUpper(); }
        }

        [LocalizedDisplay(LocalizeConstants.PersonalisedLicenseNo3)]
        public string Wunschkennzeichen3
        {
            get { return _wunschkennzeichen3.NotNullOrEmpty().ToUpper(); }
            set { _wunschkennzeichen3 = value.NotNullOrEmpty().ToUpper(); }
        }

        [LocalizedDisplay(LocalizeConstants.LicenseNoReserved)]
        public bool KennzeichenReserviert { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReservationNo)]
        public string ReservierungsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReservationName)]
        public string ReservierungsName { get; set; }

        public static bool IstNeuzulassung(string matNr) { return (matNr.TrimStart('0') == "593"); }

        public static bool IstGebrauchtzulassung(string matNr) { return (matNr.TrimStart('0') == "588"); }

        public static bool Ist72HVersandzulassung(string matNr) { return (matNr.TrimStart('0') == "598"); }

        public static bool IstAbmeldung(string matNr) { return (matNr.TrimStart('0') == "573" || matNr.TrimStart('0') == "584" || matNr.TrimStart('0') == "669"); }

        public static bool IstUmkennzeichnung(string matNr) { return (matNr.TrimStart('0') == "596"); }

        public static bool IstKurzzeitzulassung(string matNr) { return (matNr.TrimStart('0') == "592"); }

        public static bool IstFirmeneigeneZulassung(string matNr) { return (matNr.TrimStart('0') == "619"); }

        public static bool IstZollzulassung(string matNr) { return (matNr.TrimStart('0') == "600"); }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!IstKurzzeitzulassung(ZulassungsartMatNr) && !IstZollzulassung(ZulassungsartMatNr) && string.IsNullOrEmpty(EvbNr))
                yield return new ValidationResult(Localize.EvbNumberRequired, new[] { "EvbNr" });

            if (!string.IsNullOrEmpty(EvbNr) && EvbNr.Length != 7)
                yield return new ValidationResult(Localize.EvbNumberLengthMustBe7, new[] { "EvbNr" });

            if (KennzeichenReserviert && (string.IsNullOrEmpty(ReservierungsName) || string.IsNullOrEmpty(ReservierungsNr)))
                yield return new ValidationResult(Localize.ReservationDataRequired, new[] { "ReservierungsName", "ReservierungsNr" });
        }

        public string GetSummaryString()
        {
            var s = "";

            if (Zulassungsart != null)
                s += string.Format("{0}: {1}", Localize.RegistrationType, Zulassungsart.MaterialText);

            s += string.Format("<br/>{0}: {1}", Localize.RegistrationDate, (Zulassungsdatum.HasValue ? Zulassungsdatum.Value.ToShortDateString() : ""));
            s += string.Format("<br/>{0}: {1} {2}", Localize.RegistrationDistrict, Zulassungskreis, ZulassungskreisBezeichnung);

            if (!string.IsNullOrEmpty(EvbNr))
                s += string.Format("<br/>{0}: {1}", Localize.EvbNumber, EvbNr);

            s += string.Format("<br/>{0}: {1}", Localize.PersonalisedLicenseNo, Kennzeichen);

            if (KennzeichenReserviert)
            {
                s += string.Format("<br/>{0}", Localize.LicenseNoReserved);
                s += string.Format("<br/>{0}: {1}", Localize.ReservationNo, ReservierungsNr);
                s += string.Format("<br/>{0}: {1}", Localize.ReservationName, ReservierungsName);
            }
            else
            {
                s += string.Format("<br/>{0}: {1}", Localize.PersonalisedLicenseNo2, Wunschkennzeichen2);
                s += string.Format("<br/>{0}: {1}", Localize.PersonalisedLicenseNo3, Wunschkennzeichen3);
            }

            return s;
        }
    }
}
