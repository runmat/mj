using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.CoC.Models
{
    public class ZulassungsOptionen
    {
        [XmlIgnore]
        public CocEntity SelectedFahrzeug { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationType)]
        [Required]
        public string ZulassungsOptionKey { get; set; }

        public ZulassungsOption ZulassungsOption
        {
            get
            {
                if (OptionenList == null)
                    return new ZulassungsOption();

                var option = OptionenList.FirstOrDefault(vo => vo.ID == ZulassungsOptionKey);
                if (option == null)
                    return new ZulassungsOption();

                return option;
            }
        }

        [XmlIgnore]
        static public List<ZulassungsOption> OptionenList { get; set; }

        [LocalizedDisplay(LocalizeConstants.Comment)]
        [MaxLength(60)]
        public string Bemerkung { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeliveryDate)]
        public DateTime? AuslieferDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationArea)]
        [MaxLength(3)]
        [Required]
        [ZulassungsKreis]
        public string ZulassungsKreis { get; set; }

        [ModelMappingCompareIgnore]
        public bool IsValid { get; set; }


        public Versicherungsdaten Versicherung { get; set; }


        public string GetSummaryString()
        {
            var s = string.Format("{0}", ZulassungsOption.Name);

            if (AuslieferDatum != null)
                s += string.Format("<br/>{0}: {1:d}", Localize.DeliveryDate, AuslieferDatum);

            if (Bemerkung.IsNotNullOrEmpty())
                s += string.Format("<br/>{0}:<br/>{1}", Localize.Comment, Bemerkung);

            return s;
        }
    }
}
