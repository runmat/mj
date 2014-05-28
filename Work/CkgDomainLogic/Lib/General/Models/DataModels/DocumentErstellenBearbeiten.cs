using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.General.Models.DataModels
{
    public class DocumentErstellenBearbeiten : IValidatableObject
    {
        public int ID { get; set; }

        [LocalizedDisplay(LocalizeConstants.DocumentName)]
        public string Name { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.DocumentType)]
        public int? DocTypeID { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.UserGroups)]
        public List<string> SelectedWebGroups { get; set; }

        public List<SelectListItem> AvailableWebGroups { get; set; }

        public List<SelectListItem> AvailableDocumentTypes { get; set; }

        [ModelMappingCompareIgnore]
        [GridHidden]
        public bool IsValid { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Dokumentart und Gruppenzuordnung müssen gesetzt sein
            if (DocTypeID == null)
            {
                yield return new ValidationResult(Localize.DocumentDocumentTypeRequired, new[] { "DocTypeID" });
            }
            if ((SelectedWebGroups == null) || (SelectedWebGroups.Count == 0))
            {
                yield return new ValidationResult(Localize.DocumentGroupAssignmentRequired, new[] { "SelectedWebGroups" });
            }
        }
    }
}
