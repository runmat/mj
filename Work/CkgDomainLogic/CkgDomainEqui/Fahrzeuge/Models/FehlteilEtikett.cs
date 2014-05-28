using System.ComponentModel.DataAnnotations;
using System.Linq;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class FehlteilEtikett 
    {
        public int ID { get; set; }

        [LocalizedDisplay(LocalizeConstants.VIN)]
        [Required]
        [VIN]
        public string VIN { get; set; }

        [GridHidden]
        public string Kennzeichen { get; set; }

        [GridHidden]
        public string ValidationErrors { get; set; }

        [GridHidden]
        public string CustomerNo { get; set; }

        [LocalizedDisplay(LocalizeConstants.Content, 1)]
        [MaxLength(30)]
        public string Content1 { get; set; }
        
        [GridHidden]
        [LocalizedDisplay(LocalizeConstants.Header, 1)]
        [MaxLength(25)]
        public string Header1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Content, 2)]
        [MaxLength(30)]
        public string Content2 { get; set; }
        
        [GridHidden]
        [LocalizedDisplay(LocalizeConstants.Header, 2)]
        [MaxLength(25)]
        public string Header2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Content, 3)]
        [MaxLength(30)]
        public string Content3 { get; set; }
        
        [GridHidden]
        [LocalizedDisplay(LocalizeConstants.Header, 3)]
        [MaxLength(25)]
        public string Header3 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Content, 4)]
        [MaxLength(30)]
        public string Content4 { get; set; }
        
        [GridHidden]
        [LocalizedDisplay(LocalizeConstants.Header, 4)]
        [MaxLength(25)]
        public string Header4 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Content, 5)]
        [MaxLength(30)]
        public string Content5 { get; set; }
       
        [GridHidden]
        [LocalizedDisplay(LocalizeConstants.Header, 5)]
        [MaxLength(25)]
        public string Header5 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Content, 6)]
        [MaxLength(30)]
        public string Content6 { get; set; }
        
        [GridHidden]
        [LocalizedDisplay(LocalizeConstants.Header, 6)]
        [MaxLength(25)]
        public string Header6 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Content, 7)]
        [MaxLength(30)]
        public string Content7 { get; set; }
        
        [GridHidden]
        [LocalizedDisplay(LocalizeConstants.Header, 7)]
        public string Header7 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Content, 8)]
        [MaxLength(30)]
        public string Content8 { get; set; }
        
        [GridHidden]
        [LocalizedDisplay(LocalizeConstants.Header, 8)]
        [MaxLength(25)]
        public string Header8 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Content, 9)]
        [MaxLength(30)]
        public string Content9 { get; set; }
       
        [GridHidden]
        [LocalizedDisplay(LocalizeConstants.Header, 9)]
        [MaxLength(25)]
        public string Header9 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Content, 10)]
        [MaxLength(30)]
        public string Content10 { get; set; }
        
        [GridHidden]
        [LocalizedDisplay(LocalizeConstants.Header, 10)]
        [MaxLength(25)]
        public string Header10 { get; set; }

        [GridHidden]
        public bool IsValid { get; set; }

        [GridHidden]
        [LocalizedDisplay(LocalizeConstants.LabelPosition)]
        public int LayoutPosition { get; set; }

        [GridHidden]
        public bool PrintMode { get; set; }


        public bool MemberHasValidationError(string propertyName = null)
        {
            if (propertyName.IsNullOrEmpty())
                return ValidationErrors.IsNotNullOrEmpty();

            return ValidationErrors.NotNullOrEmpty().Split(',').Contains(propertyName);
        }
    }
}
