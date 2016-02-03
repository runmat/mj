using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.ViewModels;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.DomainCommon.Models
{
    public class DokumentErstellenBearbeiten : IValidatableObject
    {
        public int ID { get; set; }

        [LocalizedDisplay(LocalizeConstants.DocumentName)]
        public string Name { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.DocumentType)]
        public int? DocTypeID { get; set; }

        [RequiredConditional]
        [LocalizedDisplay(LocalizeConstants.UserGroups)]
        public List<string> SelectedWebGroups { get; set; }

        [LocalizedDisplay(LocalizeConstants.Tags)]
        public string Tags { get; set; }

        [XmlIgnore, GridHidden, NotMapped]
        public List<UserGroup> AvailableWebGroups
        {
            get { return GetViewModel == null ? new List<UserGroup>() : GetViewModel().UserGroupsOfCurrentCustomer; }
        }

        [XmlIgnore, GridHidden, NotMapped]
        public List<DocumentType> AvailableDocumentTypes
        {
            get { return GetViewModel == null ? new List<DocumentType>() : GetViewModel().DocumentTypes; }
        }

        public bool GeneralMode { get { return (GetViewModel != null && GetViewModel().GeneralMode); } }

        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<DocumentViewModel> GetViewModel { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Dokumentart und Gruppenzuordnung müssen gesetzt sein
            if (DocTypeID == null)
                yield return new ValidationResult(Localize.DocumentDocumentTypeRequired, new[] { "DocTypeID" });

            if ((SelectedWebGroups == null || SelectedWebGroups.None()) && !GeneralMode)
                yield return new ValidationResult(Localize.DocumentGroupAssignmentRequired, new[] { "SelectedWebGroups" });
        }
    }
}
