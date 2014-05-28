using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;
using GeneralTools.Models;

namespace CkgDomainLogic.Feinstaub.Models
{
    public class FeinstaubSuchparameter : IValidatableObject
    {
        private string _kennzeichenTeil1;
        [StringLength(3)]
        [LocalizedDisplay("Kennzeichen")]
        public string KennzeichenTeil1
        {
            get { return _kennzeichenTeil1; }
            set { _kennzeichenTeil1 = value.NotNullOrEmpty().ToUpper(); }
        }

        private string _kennzeichenTeil2;
        [StringLength(6)]
        [LocalizedDisplay("Kennzeichen")]
        public string KennzeichenTeil2
        {
            get { return _kennzeichenTeil2; }
            set { _kennzeichenTeil2 = value.NotNullOrEmpty().ToUpper(); }
        }

        [LocalizedDisplay("Erfassungsdatum")]
        public string ErfassungsdatumVon { get; set; }

        [LocalizedDisplay("bis")]
        public string ErfassungsdatumBis { get; set; }

        public string ErfassungsdatumHint { get { return "Max. selektierbarer Zeitraum 90 Tage"; } }

        [ModelMappingCompareIgnore]
        [GridHidden]
        public bool IsValid { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var blnKennzeichen = (!String.IsNullOrEmpty(KennzeichenTeil1) && !String.IsNullOrEmpty(KennzeichenTeil2));
            var blnDatumVon = (!String.IsNullOrEmpty(ErfassungsdatumVon));
            var blnDatumVBis = (!String.IsNullOrEmpty(ErfassungsdatumBis));

            if (blnKennzeichen)
            {
                var regexItem = new Regex("^[a-zA-ZäöüÄÖÜ]{1,2}[0-9]{1,4}$");

                if (!regexItem.IsMatch(KennzeichenTeil2))
                {
                    yield return new ValidationResult("Kennzeichen Teil 2 muss aus 1-2 Buchstaben und 1-4 Zahlen bestehen und ohne Leerzeichen zwischen Buchstaben und Zahlen", new[] { "KennzeichenTeil2" });
                }
            }

            DateTime vonDatum = DateTime.MinValue;
            if (blnDatumVon && !DateTime.TryParseExact(ErfassungsdatumVon, "dd.MM.yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out vonDatum))
                yield return new ValidationResult("Von-Datum ungültig", new[] { "DatumVon" });

            DateTime bisDatum = DateTime.MinValue;
            if (blnDatumVBis && !DateTime.TryParseExact(ErfassungsdatumBis, "dd.MM.yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out bisDatum))
                yield return new ValidationResult("Bis-Datum ungültig", new[] { "DatumBis" });

            if (blnDatumVon && blnDatumVBis && vonDatum > bisDatum)
                yield return new ValidationResult("Datumsbereich ungültig", new[] { "DatumBis" });

            if (!blnKennzeichen)
            {
                // Bei Suche ohne Kennzeichen ist der Zeitraum Pflicht und darf max. 90 Tage groß sein
                if (!blnDatumVon || !blnDatumVBis || (vonDatum - bisDatum).TotalDays > 90)
                    yield return new ValidationResult("Bei Suche ohne gültiges Kennzeichen muss ein Zeitraums (max. 90 Tage) angegeben werden", new[] { "DatumBis" });
            }
        }
    }
}
