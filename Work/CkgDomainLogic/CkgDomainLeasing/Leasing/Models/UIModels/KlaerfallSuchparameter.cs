using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Leasing.Models
{
    /// <summary>
    /// Parametersatz für die Klaerfall-Suche
    /// </summary>
    public class KlaerfallSuchparameter : IValidatableObject
    {
        public bool Klaerfaelle { get; set; }

        [LocalizedDisplay(LocalizeConstants.Selection)]
        public int AuswahlFehlendeDaten { get; set; }

        public List<KlaerfallSucheArtFehlendeDaten> AuswahlArtFehlendeDaten
        {
            get
            {
                List<KlaerfallSucheArtFehlendeDaten> liste = new List<KlaerfallSucheArtFehlendeDaten>();
                liste.Add(new KlaerfallSucheArtFehlendeDaten(1, Localize.MissingMails));
                liste.Add(new KlaerfallSucheArtFehlendeDaten(2, Localize.MissingReceiptLetter));
                return liste;
            }
        }

        [LocalizedDisplay(LocalizeConstants.LeasingContractNo)]
        public string Leasingvertragsnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.CustomerNo)]
        public string Kundennummer { get; set; }

        [ModelMappingCompareIgnore]
        [GridHidden]
        public bool IsValid { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Mindestens eines der Suchkriterien muss gesetzt sein
            if ((!Klaerfaelle) && (AuswahlFehlendeDaten < 1) && (String.IsNullOrEmpty(Leasingvertragsnummer)) && (String.IsNullOrEmpty(Kundennummer)))
            {
                yield return new ValidationResult(Localize.AtLeastOneSearchCriterionRequired, new[] { "" });
            }
        }
    }
}
