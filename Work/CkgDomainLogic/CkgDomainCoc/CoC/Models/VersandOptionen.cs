using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.CoC.Models
{
    public class VersandOptionen
    {
        [XmlIgnore]
        public CocEntity SelectedFahrzeug { get; set; }

        [LocalizedDisplay(LocalizeConstants.ShippingOptions)]
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

        [GridHidden, NotMapped, XmlIgnore, ModelMappingCompareIgnore]
        public string BemerkungAsString { get { return Bemerkung.NotNullOr("- " + Localize.None.ToLower() + " -"); } }

        [ModelMappingCompareIgnore]
        public bool IsValid { get; set; }

        public string GetSummaryString()
        {
            var s = string.Format("{0}", VersandOption.Name);

            s += string.Format("<br/><br/>{0}:<br/>{1}", Localize.Comment, BemerkungAsString);

            return s;
        }
    }
}
