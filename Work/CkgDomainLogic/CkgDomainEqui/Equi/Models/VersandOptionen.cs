using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.Equi.ViewModels;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    public class VersandOptionen : IValidatableObject
    {
        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<BriefversandViewModel> GetViewModel { get; set; }

        [XmlIgnore]
        public Fahrzeugbrief SelectedFahrzeug { get; set; }

        [LocalizedDisplay(LocalizeConstants.ShippingOption)]
        [Required]
        public string VersandOptionKey { get; set; }

        public VersandOption VersandOption
        {
            get
            {
                if (OptionenList == null)
                    return new VersandOption();

                var option = OptionenList.FirstOrDefault(vo => vo.ID == VersandOptionKey);
                if (option == null)
                    return new VersandOption();

                return option;
            }
        }

        [XmlIgnore]
        static public List<VersandOption> OptionenList { get; set; }

        [LocalizedDisplay(LocalizeConstants.Comment)]
        [MaxLength(60)]
        public string Bemerkung { get; set; }

        [RequiredConditional]
        [LocalizedDisplay(LocalizeConstants.CauseOfDispatch)]
        public string VersandGrundKey { get; set; }

        public VersandGrund VersandGrund
        {
            get
            {
                if (GruendeList == null)
                    return new VersandGrund();

                var grund = GruendeList.FirstOrDefault(vo => vo.Code == VersandGrundKey);
                if (grund == null)
                    return new VersandGrund();

                return grund;
            }
        }

        [XmlIgnore]
        static public List<VersandGrund> GruendeList { get; set; }

        [LocalizedDisplay(" ")]
        public bool AufAbmeldungWarten { get; set; }

        public bool AufAbmeldungWartenAvailable { get { return GetViewModel != null && GetViewModel().VersandOptionAufAbmeldungWartenAvailable; } }

        public BriefversandModus VersandModus { get { return (GetViewModel == null ? BriefversandModus.Brief : GetViewModel().VersandModus); } }

        public bool VersandGrundIsRequired { get { return VersandModus != BriefversandModus.Schluessel; } }

        [ModelMappingCompareIgnore]
        public bool IsValid { get; set; }

        public string GetSummaryString()
        {
            var s = string.Format("{0}", VersandOption.Name);

            if (AufAbmeldungWartenAvailable && AufAbmeldungWarten)
                s += string.Format("<br/>{0}", Localize.WaitForDeregistration);

            if (Bemerkung.IsNotNullOrEmpty())
                s += string.Format("<br/><br/>{0}:<br/>{1}", Localize.Comment, Bemerkung);

            s += string.Format("<br/><br/>{0}: {1}", Localize.CauseOfDispatch, VersandGrund.Bezeichnung);

            return s;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (VersandGrundIsRequired && VersandGrundKey.IsNullOrEmpty())
                yield return new ValidationResult(Localize.ThisFieldIsRequired, new[] { "VersandGrundKey" });
        }
    }
}
