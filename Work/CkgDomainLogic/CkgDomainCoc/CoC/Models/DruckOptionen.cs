using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.CoC.Models
{
    public class DruckOptionen : Store, IValidatableObject 
    {
        [LocalizedDisplay(LocalizeConstants.PrintOptions)]
        [XmlIgnore]
        public object Druck { get; set; }  

        [LocalizedDisplay(LocalizeConstants.PrintZBII)]
        public bool DruckZBII { get; set; }

        [LocalizedDisplay(LocalizeConstants.PrintCoC)]
        public bool DruckCoc { get; set; }

        [XmlIgnore]
        public CocBeauftragungMode BeauftragungMode { get { return PropertyCacheGet(() => CocBeauftragungMode.Versand); } set { PropertyCacheSet(value); } }

        [XmlIgnore]
        public bool FolgeDienstleistungenVerfuegbar { get { return (BeauftragungMode == CocBeauftragungMode.Versand || BeauftragungMode == CocBeauftragungMode.VersandDuplikat); } }

        [XmlIgnore]
        public bool DruckZBIIverfuegbar { get { return (BeauftragungMode != CocBeauftragungMode.VersandDuplikat); } }

        [LocalizedDisplay(LocalizeConstants.SequenceOfServices)]
        public string FolgeDienstleistungen { get; set; }

        public bool ModusEigenDruck { get { return FolgeDienstleistungen.NotNullOrEmpty() == "eigendruck"; } }

        [XmlIgnore]
        public static string FolgeDienstleistungsOptionen { get { return string.Format("versand,{0};eigendruck,{1}", Localize.Shipping, Localize.SelfPrinting); } }


        public void SetBeauftragungMode(CocBeauftragungMode mode)
        {
            BeauftragungMode = mode;

            if (mode == CocBeauftragungMode.VersandDuplikat)
            {
                DruckZBII = false;
                DruckCoc = true;
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (BeauftragungMode == CocBeauftragungMode.Versand)
            {
                if (!DruckZBII && !DruckCoc)
                    yield return
                        new ValidationResult(Localize.ProvideAtLeastPrintZBIIorCoc, new[] { "DruckZBII", "DruckCoc" });

                if (DruckZBII && ModusEigenDruck)
                    yield return
                        new ValidationResult(Localize.ZB2SelfPrintForbidden, new[] { "DruckZBII", "FolgeDienstleistungen" });
            }
        }

        public string GetSummaryString()
        {
            var s = "";

            if (DruckZBII)
                s += string.Format("{0}", Localize.PrintZBII);
            
            if (DruckCoc)
                s += string.Format("{0}{1}", (s.IsNullOrEmpty() ? "" : "<br/>"), Localize.PrintCoC);
            
            return s;
        }
    }
}
