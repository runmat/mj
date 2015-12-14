using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.ViewModels;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.DomainCommon.Models
{
    public class HaendlerAdressenSelektor : IValidatableObject
    {
        [LocalizedDisplay(LocalizeConstants.DealerNo)]
        public string HaendlerNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Country)]
        public string LandCode { get; set; }

        [XmlIgnore, GridHidden, NotMapped]
        public LandExt Land
        {
            get { return LaenderListWithDefaultOption.FirstOrDefault(l => l.CodeExt == LandCode, null); }
        }

        [LocalizedDisplay(LocalizeConstants.AddressType)]
        public string AdressenTyp { get; set; }

        public static List<SelectItem> AdressenTypen
        {
            get
            {
                return new List<SelectItem>
                    {
                        new SelectItem ("HAENDLER", Localize.Dealer),
                        new SelectItem ("LAND", Localize.Country),
                    };
            }
        }

        public bool LandAdressenModus { get { return AdressenTyp == "LAND"; } }
        public bool HaendlerAdressenModus { get { return AdressenTyp != "LAND"; } }


        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<HaendlerAdressenViewModel> GetViewModel { get; set; }

        [XmlIgnore, GridHidden, NotMapped]
        public List<LandExt> LaenderListWithDefaultOption
        {
            get { return GetViewModel == null ? new List<LandExt>() : GetViewModel().LaenderListWithOptionAll; }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (HaendlerAdressenModus && LandCode.IsNullOrEmpty())
                yield return new ValidationResult("Bei Händleradressen bitte noch das Land angeben", new []{ "LandCode" });
        }
    }
}
