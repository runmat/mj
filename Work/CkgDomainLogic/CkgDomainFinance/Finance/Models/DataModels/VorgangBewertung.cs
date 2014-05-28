using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Finance.Models
{
    public class VorgangBewertung : IValidatableObject
    {
        [LocalizedDisplay(LocalizeConstants.Account)]
        public string Kontonummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.CIN)]
        public string CIN { get; set; }

        [LocalizedDisplay(LocalizeConstants.PAID)]
        public string PAID { get; set; }

        [LocalizedDisplay(LocalizeConstants.Checkdate)]
        public DateTime Pruefdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.DealerPurchaseValueNet)]
        public string HaendlerNettoEkWert { get; set; }

        [LocalizedDisplay(LocalizeConstants.DealerSaleValueNet)]
        public string HaendlerNettoVkWert { get; set; }

        [LocalizedDisplay(LocalizeConstants.DealerPurchaseValueGross)]
        public string HaendlerBruttoEkWert { get; set; }

        [LocalizedDisplay(LocalizeConstants.DealerSaleValueGross)]
        public string HaendlerBruttoVkWert { get; set; }

        [LocalizedDisplay(LocalizeConstants.Cost)]
        public string Kosten { get; set; }

        [LocalizedDisplay(LocalizeConstants.Selection)]
        public Erfassungsart Erfassungstyp { get; set; }

        public List<VorgangSucheErfassungsart> AuswahlErfassungsart
        {
            get
            {
                List<VorgangSucheErfassungsart> liste = new List<VorgangSucheErfassungsart>();
                liste.Add(new VorgangSucheErfassungsart(Erfassungsart.Gutachterwert, Localize.AcquisitionSurveyorValue));
                liste.Add(new VorgangSucheErfassungsart(Erfassungsart.Vermarktungserloese, Localize.AcquisitionMarketingProceeds));
                return liste;
            }
        }

        [ModelMappingCompareIgnore]
        [GridHidden]
        public bool IsValid { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Erfassungstyp == Erfassungsart.Gutachterwert)
            {
                if (String.IsNullOrEmpty(HaendlerBruttoEkWert) || String.IsNullOrEmpty(HaendlerNettoEkWert))
                    yield return new ValidationResult(Localize.AllRequiredInputFieldsMustBeFilled, new[] { "HaendlerBruttoEkWert" });
            }
            else
            {
                if (String.IsNullOrEmpty(HaendlerBruttoVkWert) || String.IsNullOrEmpty(HaendlerNettoVkWert) || String.IsNullOrEmpty(Kosten))
                    yield return new ValidationResult(Localize.AllRequiredInputFieldsMustBeFilled, new[] { "HaendlerBruttoVkWert" });
            }
        }
    }
}
