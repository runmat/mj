using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.Autohaus.ViewModels;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Autohaus.Models
{
    public class Zulassungsdaten : IValidatableObject
    {
        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<KroschkeZulassungViewModel> GetZulassungViewModel { get; set; }

        private string _wunschkennzeichen2;
        private string _wunschkennzeichen3;
        private string _kennzeichen;
        private int _zulassungsartMenge = 1;

        public bool ModusAbmeldung { get; set; }

        public bool IsMassenzulassung { get; set; }
        public bool IsMassenabmeldung { get; set; }

        public bool IsSchnellabmeldung { get; set; }

        public bool ModusVersandzulassung { get; set; }

        public SonderzulassungsMode SonderzulassungsMode { get; set; }
        [XmlIgnore]
        public bool ModusSonderzulassung { get { return SonderzulassungsMode != SonderzulassungsMode.None; } }

        public bool ModusPartnerportal { get; set; }

        [RequiredConditional]
        [LocalizedDisplay(LocalizeConstants.RegistrationType)]
        public string ZulassungsartMatNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Amount)]
        public int ZulassungsartMenge
        {
            get { return _zulassungsartMenge; }
            set { _zulassungsartMenge = value; }
        }

        public Material Zulassungsart
        {
            get
            {
                if (MaterialList == null)
                    return new Material();

                var list = (ModusAbmeldung ? Abmeldearten : MaterialList);
                return list.FirstOrDefault(m => m.MaterialNr == ZulassungsartMatNr) ?? new Material();
            }
        }

        public bool ZulassungsartAutomatischErmitteln { get; set; }

        // 20150528 MMA 
        [LocalizedDisplay(LocalizeConstants.MindestHaltedauer)]  
        // [Range(1, 360, ErrorMessage = LocalizeConstants.MindestHaltedauerRangeError)]    // Localization per default not working,so implemented in separate validation below
        public int? MindesthaltedauerDays { get; set; }                                     // number of days

        // 20150602 MMA
        public string WunschkennzeichenReservierenUrl { get; set; }

        [XmlIgnore]
        static List<Material> MaterialList { get { return GetZulassungViewModel == null ? new List<Material>() : GetZulassungViewModel().Zulassungsarten; } }

        [XmlIgnore]
        static List<Material> Abmeldearten { get { return GetZulassungViewModel == null ? new List<Material>() : GetZulassungViewModel().Abmeldearten; } }

        public string Belegtyp { get { return Zulassungsart.Belegtyp; } }

        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        public DateTime? Zulassungsdatum { get; set; }

        [RequiredConditional]
        [LocalizedDisplay(LocalizeConstants.CancellationDate)]
        public DateTime? Abmeldedatum { get; set; }

        [StringLength(3)]
        [LocalizedDisplay(LocalizeConstants.RegistrationDistrict)]
        public string Zulassungskreis { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationDistrict)]
        public string ZulassungskreisBezeichnung { get; set; }

        [RequiredConditional]
        [LocalizedDisplay(LocalizeConstants.EvbNumber)]
        public string EvbNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.PersonalisedLicenseNo)]
        [KennzeichenPartial]
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
                return !ModusAbmeldung &&
                            (KennzeichenReserviert ||
                             (KennzeichenIsValid(Kennzeichen) || KennzeichenIsValid(Wunschkennzeichen2) || KennzeichenIsValid(Wunschkennzeichen3)));
            }
        }

        public static string ZulassungsKennzeichenLinkeSeite(string kennzeichen)
        {
            return kennzeichen.NotNullOrEmpty().ToUpper().RemoveDigits().AppendIfNotNull("-");
        }

        public static bool KennzeichenIsValid(string kennzeichen)
        {
            return kennzeichen.IsNotNullOrEmpty() && !kennzeichen.LastCharIs('-');
        }

        [LocalizedDisplay(LocalizeConstants.PersonalisedLicenseNo2)]
        [KennzeichenPartial]
        public string Wunschkennzeichen2
        {
            get { return _wunschkennzeichen2.NotNullOrEmpty().ToUpper(); }
            set { _wunschkennzeichen2 = value.NotNullOrEmpty().ToUpper(); }
        }

        [LocalizedDisplay(LocalizeConstants.PersonalisedLicenseNo3)]
        [KennzeichenPartial]
        public string Wunschkennzeichen3
        {
            get { return _wunschkennzeichen3.NotNullOrEmpty().ToUpper(); }
            set { _wunschkennzeichen3 = value.NotNullOrEmpty().ToUpper(); }
        }

        [LocalizedDisplay(LocalizeConstants.LicenseNoReserved)]
        public bool KennzeichenReserviert { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReserveExistingLicenseNo)]
        public bool VorhandenesKennzeichenReservieren { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReservationNo)]
        public string ReservierungsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReservationName)]
        public string ReservierungsName { get; set; }

        public bool Versandzulassung { get; set; }

        public bool ExpressversandMoeglich { get; set; }

        [RequiredConditional]
        [LocalizedDisplay(LocalizeConstants.DoesCarOwnerEntryExist)]
        public string HaltereintragVorhanden { get; set; }

        [XmlIgnore]
        public static string HaltereintragVorhandenOptions { get { return String.Format("J,{0};N,{1}", Localize.Yes, Localize.No); } }

        [LocalizedDisplay(LocalizeConstants.Autohaus_KroschkePrimeExpressversand_Info)]
        public bool Expressversand { get; set; }

        public bool UiUpdateOnly { get; set; }

        [RequiredConditional]
        [LocalizedDisplay(LocalizeConstants.NumberOfCancellations)]
        public int AnzahlAbmeldungen { get; set; }

        public bool AnzahlAbmeldungenAenderbar { get; set; }

        public string HalterNameSchnellabmeldung { get; set; }

        public bool IsValid { get; set; }

        public static bool IstNeuzulassung(string matNr) { return (TrimMatNr(matNr) == "593"); }

        public static bool IstGebrauchtzulassung(string matNr) { return (TrimMatNr(matNr) == "588"); }

        public static bool Ist72HVersandzulassung(string matNr) { return (TrimMatNr(matNr) == "598"); }

        public static bool IstAbmeldung(string matNr) { return (TrimMatNr(matNr) == "573" || TrimMatNr(matNr) == "584" || TrimMatNr(matNr) == "669"); }

        public static bool IstUmkennzeichnung(string matNr) { return (TrimMatNr(matNr) == "596"); }

        public static bool IstKurzzeitzulassung(string matNr) { return (TrimMatNr(matNr) == "592"); }

        public static bool IstFirmeneigeneZulassung(string matNr) { return (TrimMatNr(matNr) == "619"); }

        public static bool IstZollzulassung(string matNr) { return (TrimMatNr(matNr) == "600"); }

        static string TrimMatNr(string matNr)
        {
            return matNr.NotNullOrEmpty().TrimStart('0');
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ZulassungsartAutomatischErmitteln && !Versandzulassung && String.IsNullOrEmpty(HaltereintragVorhanden))
                yield return new ValidationResult(string.Format("{0}", Localize.CarOwnerEntryExistsRequiredHint), new[] { "HaltereintragVorhanden" });

            if (ModusAbmeldung)
            {
                if (Abmeldedatum == null)
                    yield return new ValidationResult(string.Format("{0} {1}", Localize.CancellationDate, Localize.Required.ToLower()), new[] { "Abmeldedatum" });
            }
            else
            {
                if (Zulassungskreis.IsNullOrEmpty())
                    yield return new ValidationResult(string.Format("{0} {1}", Localize.RegistrationArea, Localize.Required.ToLower()), new[] { "Zulassungskreis" });

                if (!Zulassungsdatum.HasValue)
                    yield return new ValidationResult(string.Format("{0} {1}", Localize.RegistrationDate, Localize.Required.ToLower()), new[] { "Zulassungsdatum" });

                if (ZulassungsartMatNr.IsNullOrEmpty())
                    yield return new ValidationResult(string.Format("{0} {1}", Localize.RegistrationType, Localize.Required.ToLower()), new[] { "ZulassungsartMatNr", "ZulassungsartText" });

                if (!string.IsNullOrEmpty(EvbNr) && EvbNr.Length != 7)
                    yield return new ValidationResult(Localize.EvbNumberLengthMustBe7, new[] { "EvbNr" });
            }

            foreach (var dateResult in ValidateWochenendeUndFeiertage(Zulassungsdatum, "Zulassungsdatum").ToList())
                yield return dateResult;

            foreach (var dateResult in ValidateWochenendeUndFeiertage(Abmeldedatum, "Abmeldedatum").ToList())
                yield return dateResult;

            // 20150603 MMA 8083 Pflichtfeldprüfung auf "ReservierungsName", falls "KennzeichenReserviert" aktiv...
            if (KennzeichenReserviert && ReservierungsName.IsNullOrEmpty())
                yield return new ValidationResult(string.Format("{0} {1}", Localize.ReservationName, Localize.Required.ToLower()), new[] { "ReservierungsName" });

            // 20150608 MMA 8083 Mindesthaltedauer
            if (IstFirmeneigeneZulassung(ZulassungsartMatNr) && (MindesthaltedauerDays == null || MindesthaltedauerDays < 1 || MindesthaltedauerDays > 360))
                yield return new ValidationResult(string.Format("{0}", Localize.MindestHaltedauerRangeError), new[] { "MindesthaltedauerDays" });
        }

        static IEnumerable<ValidationResult> ValidateWochenendeUndFeiertage(DateTime? dateValue, string datePropertyName)
        {
            if (dateValue == null)
                yield break;

            var datum = dateValue.GetValueOrDefault();
            if (datum < DateTime.Today)
                yield return new ValidationResult("Bitte geben Sie ein Datum ab heute an", new[] { datePropertyName });
            else if (datum.DayOfWeek == DayOfWeek.Saturday || datum.DayOfWeek == DayOfWeek.Sunday)
                yield return new ValidationResult("Bitte vermeiden Sie Wochenendtage", new[] { datePropertyName });
            else
            {
                var feiertag = DateService.GetFeiertag(datum);
                if (feiertag != null)
                    yield return new ValidationResult(
                        string.Format("Der {0} ist ein Feiertag, '{1}'. Bitte vermeiden Sie Feiertage.", datum.ToString("dd.MM.yy"), feiertag.Name)
                        , new[] { datePropertyName });
            }
        }

        public string GetSummaryString()
        {
            var s = "";

            if (Zulassungsart != null)
                s += string.Format("{0}: {1}", Localize.RegistrationType, Zulassungsart.MaterialText);

            if (ModusAbmeldung)
            {
                s += string.Format("<br/>{0}: {1}", Localize.CancellationDate, (Abmeldedatum.HasValue ? Abmeldedatum.Value.ToShortDateString() : ""));
                s += string.Format("<br/>{0}: {1} {2}", Localize.RegistrationDistrict, Zulassungskreis, ZulassungskreisBezeichnung);

                if (VorhandenesKennzeichenReservieren)
                    s += string.Format("<br/>{0}", Localize.ReserveExistingLicenseNo);
            }
            else
            {
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
            }

            return s;
        }
    }
}

