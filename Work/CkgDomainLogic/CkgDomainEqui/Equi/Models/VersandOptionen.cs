using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    public class VersandOptionen
    {
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

        [GridHidden, NotMapped, XmlIgnore, ModelMappingCompareIgnore]
        public string BemerkungAsString { get { return Bemerkung.NotNullOr("- " + Localize.None.ToLower() +  " -"); } }

        [LocalizedDisplay(LocalizeConstants.CauseOfDispatch)]
        [Required]
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

        public bool AufAbmeldungWartenAvailable { get; set; }

        [ModelMappingCompareIgnore]
        public bool IsValid { get; set; }

        public string GetSummaryString()
        {
            var s = string.Format("{0}", VersandOption.Name);

            if (AufAbmeldungWartenAvailable && AufAbmeldungWarten)
                s += string.Format("<br/>{0}", Localize.WaitForDeregistration);

            s += string.Format("<br/><br/>{0}:<br/>{1}", Localize.Comment, BemerkungAsString);

            s += string.Format("<br/><br/>{0}: {1}", Localize.CauseOfDispatch, VersandGrund.Bezeichnung);

            return s;
        }
    }
}
