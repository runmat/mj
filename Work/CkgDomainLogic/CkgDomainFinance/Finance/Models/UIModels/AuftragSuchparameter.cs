using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Finance.Models
{
    /// <summary>
    /// Parametersatz für die Auftragssuche
    /// </summary>
    public class AuftragSuchparameter : IValidatableObject
    {
        [Required]
        [LocalizedDisplay(LocalizeConstants.ProcessDateFrom)]
        public DateTime? WunschlieferdatumVon { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.ProcessDateTo)]
        public DateTime? WunschlieferdatumBis { get; set; }

        [LocalizedDisplay(LocalizeConstants.Selection)]
        public Selektionsart Selektion { get; set; }

        public List<AuftragSucheSelektionsart> AuswahlSelektionsart
        {
            get
            {
                List<AuftragSucheSelektionsart> liste = new List<AuftragSucheSelektionsart>();
                liste.Add(new AuftragSucheSelektionsart(Selektionsart.GebuehrenInBearbeitung, Localize.TaxesInProgress));
                liste.Add(new AuftragSucheSelektionsart(Selektionsart.AlleGebuehren, Localize.AllTaxes));
                return liste;
            }
        }

        [ModelMappingCompareIgnore]
        [GridHidden]
        public bool IsValid { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if ((WunschlieferdatumVon.HasValue) && (WunschlieferdatumBis.HasValue) && (WunschlieferdatumVon.Value > WunschlieferdatumBis.Value))
                yield return new ValidationResult(Localize.DateRangeInvalid, new[] { "WunschlieferdatumBis" });
        }
    }
}
