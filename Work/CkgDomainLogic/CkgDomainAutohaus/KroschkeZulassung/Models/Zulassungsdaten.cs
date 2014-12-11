using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.KroschkeZulassung.Models
{
    public class Zulassungsdaten : IValidatableObject
    {
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
        static public List<Material> MaterialList { get; set; }

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
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.PersonalisedNumberPlate)]
        public bool Wunschkennzeichen { get { return (!KennzeichenReserviert); } }

        [LocalizedDisplay(LocalizeConstants.PersonalisedLicenseNo2)]
        public string Wunschkennzeichen2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.PersonalisedLicenseNo3)]
        public string Wunschkennzeichen3 { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNoReserved)]
        public bool KennzeichenReserviert { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReservationNo)]
        public string ReservierungsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReservationName)]
        public string ReservierungsName { get; set; }

        public static bool IstNeuzulassung(string matNr) { return (matNr.TrimStart('0') == "593"); }

        public static bool IstGebrauchtzulassung(string matNr) { return (matNr.TrimStart('0') == "588"); }

        public static bool Ist72hVersandzulassung(string matNr) { return (matNr.TrimStart('0') == "598"); }

        public static bool IstAbmeldung(string matNr) { return (matNr.TrimStart('0') == "573" || matNr.TrimStart('0') == "584" || matNr.TrimStart('0') == "669"); }

        public static bool IstUmkennzeichnung(string matNr) { return (matNr.TrimStart('0') == "596"); }

        public static bool IstKurzzeitzulassung(string matNr) { return (matNr.TrimStart('0') == "592"); }

        public static bool IstFirmeneigeneZulassung(string matNr) { return (matNr.TrimStart('0') == "619"); }

        public static bool IstZollzulassung(string matNr) { return (matNr.TrimStart('0') == "600"); }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!IstKurzzeitzulassung(ZulassungsartMatNr) && !IstZollzulassung(ZulassungsartMatNr) && String.IsNullOrEmpty(EvbNr))
                yield return new ValidationResult(Localize.EvbNumberRequired, new[] { "EvbNr" });

            if (!String.IsNullOrEmpty(EvbNr) && EvbNr.Length != 7)
                yield return new ValidationResult(Localize.EvbNumberLengthMustBe7, new[] { "EvbNr" });

            if (KennzeichenReserviert && (String.IsNullOrEmpty(ReservierungsName) || String.IsNullOrEmpty(ReservierungsNr)))
                yield return new ValidationResult(Localize.ReservationDataRequired, new[] { "ReservierungsName", "ReservierungsNr" });
        }

        public string GetSummaryString()
        {
            var s = "";

            if (Zulassungsart != null)
                s += String.Format("{0}: {1}", Localize.RegistrationType, Zulassungsart.MaterialText);

            s += String.Format("<br/>{0}: {1}", Localize.RegistrationDate, (Zulassungsdatum.HasValue ? Zulassungsdatum.Value.ToShortDateString() : ""));
            s += String.Format("<br/>{0}: {1} {2}", Localize.RegistrationDistrict, Zulassungskreis, ZulassungskreisBezeichnung);

            if (!String.IsNullOrEmpty(EvbNr))
                s += String.Format("<br/>{0}: {1}", Localize.EvbNumber, EvbNr);

            s += String.Format("<br/>{0}: {1}", Localize.PersonalisedLicenseNo, Kennzeichen);

            if (KennzeichenReserviert)
            {
                s += String.Format("<br/>{0}", Localize.LicenseNoReserved);
                s += String.Format("<br/>{0}: {1}", Localize.ReservationNo, ReservierungsNr);
                s += String.Format("<br/>{0}: {1}", Localize.ReservationName, ReservierungsName);
            }
            else
            {
                s += String.Format("<br/>{0}", Localize.PersonalisedNumberPlate);
                s += String.Format("<br/>{0}: {1}", Localize.PersonalisedLicenseNo2, Wunschkennzeichen2);
                s += String.Format("<br/>{0}: {1}", Localize.PersonalisedLicenseNo3, Wunschkennzeichen3);
            }

            return s;
        }
    }
}
