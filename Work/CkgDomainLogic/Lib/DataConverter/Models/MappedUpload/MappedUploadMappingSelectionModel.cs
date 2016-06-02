using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.DataConverter.ViewModels;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.DataConverter.Models
{
    public class MappedUploadMappingSelectionModel : IValidatableObject
    {
        [Required]
        [LocalizedDisplay(LocalizeConstants.Mapping)]
        public int MappingId { get; set; }

        [XmlIgnore]
        public List<DataConverterMappingInfo> MappingList { get { return GetViewModel == null ? new List<DataConverterMappingInfo>() : GetViewModel().DataMappingsForCustomerAndProcess; } }

        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<DataConverterViewModel> GetViewModel { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (MappingList.None())
                yield return new ValidationResult(Localize.NoDataMappingFound);
        }
    }
}
