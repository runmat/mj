using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using GeneralTools.Models;

namespace CkgDomainLogic.Feinstaub.Models
{
    public class FeinstaubCheckUI : IValidatableObject
    {
        private string _fahrzeugklasse;
        [Required]
        [StringLength(4)]
        [LocalizedDisplay("Fahrzeugklasse")]
        public string Fahrzeugklasse
        {
            get { return _fahrzeugklasse; }
            set { _fahrzeugklasse = value.NotNullOrEmpty().ToUpper(); }
        }

        public string FahrzeugklasseHint { get { return "Hinweis: Code aus Feld J auf ZB1 erfassen. Alter Fahrzeugschein: erste 2 Zeichen aus Feld Schlüsselnummer zu 1."; } }

        private string _codeAufbau;
        [Required]
        [StringLength(4)]
        [LocalizedDisplay("Code Aufbau")]
        public string CodeAufbau
        {
            get { return _codeAufbau; } 
            set { _codeAufbau = value.NotNullOrEmpty().ToUpper(); }
        }

        public string CodeAufbauHint { get { return "Hinweis: Code aus Feld 4 auf ZB1 erfassen. Alter Fahrzeugschein: letzte 4 Zeichen aus Feld Schlüsselnummer zu 1."; } }

        private string _kraftstoffcode;
        [Required]
        [StringLength(4)]
        [LocalizedDisplay("Kraftstoffcode")]
        public string Kraftstoffcode
        {
            get { return _kraftstoffcode; }
            set { _kraftstoffcode = value.NotNullOrEmpty().ToUpper(); }
        }

        public string KraftstoffcodeHint { get { return "Hinweis: Code aus Feld 10 auf ZB1 erfassen. Alter Fahrzeugschein: siehe Feld 5."; } }

        private string _emissionsschluesselnummer;
        [Required]
        [StringLength(2)]
        [LocalizedDisplay("Emissionsschlüssel-Nr.")]
        public string Emissionsschluesselnummer
        {
            get { return _emissionsschluesselnummer; }
            set { _emissionsschluesselnummer = value.NotNullOrEmpty().ToUpper(); }
        }

        public string EmissionsschluesselnummerHint { get { return "Hinweis: Letzte 2 Zeichen aus dem Code aus Feld 14.1 auf ZB1 erfassen. Alter Fahrzeugschein: letzte 2 Zeichen aus Feld Schlüsselnummer zu 1."; } }

        private string _kennzeichenTeil1;
        [Required]
        [StringLength(3)]
        [LocalizedDisplay("Kennzeichen")]
        public string KennzeichenTeil1
        {
            get { return _kennzeichenTeil1; }
            set { _kennzeichenTeil1 = value.NotNullOrEmpty().ToUpper(); }
        }

        private string _kennzeichenTeil2;
        [Required]
        [StringLength(6)]
        [LocalizedDisplay("Kennzeichen")]
        public string KennzeichenTeil2
        {
            get { return _kennzeichenTeil2; }
            set { _kennzeichenTeil2 = value.NotNullOrEmpty().ToUpper(); }
        }

        [ModelMappingCompareIgnore]
        [GridHidden]
        public bool IsValid { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!String.IsNullOrEmpty(Fahrzeugklasse) && Fahrzeugklasse.ToUpper().Contains("O"))
            {
                yield return new ValidationResult("Code für die Fahrzeugklasse darf kein 'O' enthalten", new[] { "Fahrzeugklasse" });
            }

            if (!String.IsNullOrEmpty(CodeAufbau) && CodeAufbau.ToUpper().Contains("O"))
            {
                yield return new ValidationResult("Code für die Aufbau-Art darf kein 'O' enthalten", new[] { "CodeAufbau" });
            }

            if (!String.IsNullOrEmpty(Emissionsschluesselnummer) && Emissionsschluesselnummer.ToUpper().Contains("O"))
            {
                yield return new ValidationResult("Emissionsschlüsselnummer darf kein 'O' enthalten", new[] { "Emissionsschluesselnummer" });
            }

            var regexItem = new Regex("^[a-zA-ZäöüÄÖÜ]{1,2}[0-9]{1,4}$");

            if (!String.IsNullOrEmpty(KennzeichenTeil2) && !regexItem.IsMatch(KennzeichenTeil2))
            {
                yield return new ValidationResult("Kennzeichen Teil 2 muss aus 1-2 Buchstaben und 1-4 Zahlen bestehen und ohne Leerzeichen zwischen Buchstaben und Zahlen", new[] { "KennzeichenTeil2" });
            }
        }
    }
}
