using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Script.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Insurance.ViewModels;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Insurance.Models
{
    [Table("VersEventSchadenfall")]
    public class Schadenfall : IValidatableObject
    {
        private string _kennzeichen;

        [Key]
        [LocalizedDisplay(LocalizeConstants.Id)]
        public int ID { get; set; }

        [GridHidden]
        [LocalizedDisplay(LocalizeConstants.CustomerNo)]
        public string KundenNr { get; set; }

        [GridHidden]
        [LocalizedDisplay(LocalizeConstants.DeleteDate)]
        public DateTime? LoeschDatum { get; set; }

        [GridHidden]
        [LocalizedDisplay(LocalizeConstants.DeleteUser)]
        public string LoeschUser { get; set; }


        [Required]
        [LocalizedDisplay(LocalizeConstants.DamageEvent)]
        public int EventID { get; set; }

        [LocalizedDisplay(LocalizeConstants.EventName)]
        [NotMapped]
        public string EventName { get { return Event.EventName; } }

        [Required]
        [LocalizedDisplay(LocalizeConstants.Partner)]
        public string VersicherungID { get; set; }

        [LocalizedDisplay(LocalizeConstants.Partner)]
        [NotMapped]
        public string VersicherungName
        {
            get
            {
                if (GetViewModel == null) 
                    return "";
                
                var vs = GetViewModel().Versicherungen.FirstOrDefault(e => e.Key == VersicherungID);
                if (vs == null) 
                    return "";
                
                return vs.Text;
            }
        }

        [LocalizedDisplay(LocalizeConstants.AnotherFemale)]
        public string VersicherungAndere { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen
        {
            get { return _kennzeichen; }
            set { _kennzeichen = value.NotNullOrEmpty().ToUpper(); }
        }

        [LocalizedDisplay(LocalizeConstants.FirstName)]
        public string Vorname { get; set; }

        [LocalizedDisplay(LocalizeConstants.LastName)]
        public string Nachname { get; set; }

        [EmailAddress]
        [LocalizedDisplay(LocalizeConstants.Email)]
        public string Email { get; set; }

        [LocalizedDisplay(LocalizeConstants.PhoneNo)]
        public string TelefonNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarManufacturer)]
        public string FzgHersteller { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarModel)]
        public string FzgModell { get; set; }

        [LocalizedDisplay(LocalizeConstants.ExcessAmount)]
        public string SelbstbeteiligungsHoehe { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReferenceNo)]
        public string Referenznummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.CollectiveInspection)]
        public bool Sammelbesichtigung { get; set; }

        [LocalizedDisplay(LocalizeConstants.CreateDate)]
        public DateTime AnlageDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.CreateDate)]
        public string AnlageDatumAsString { get { return AnlageDatum.ToString("dd.MM.yyyy"); } }  // workaround, because of unknown javascript error on direct binding of "AnlageDatum"

        [LocalizedDisplay(LocalizeConstants.CreateUser)]
        public string AnlageUser { get; set; }

        //[XmlIgnore]
        //static public List<SelectItem> Versicherungen { get; set; }
        
        [NotMapped]
        [ScriptIgnore]
        public static Func<VersEventsViewModel> GetViewModel { get; set; }

        [GridHidden]
        [NotMapped]
        [ScriptIgnore]
        public VersEvent Event
        {
            get
            {
                return GetViewModel != null
                           ? (GetViewModel().VersEvents.FirstOrDefault(e => e.ID == EventID)
                                ?? new VersEvent())
                           : null;
            }
        }


        [ModelMappingCompareIgnore]
        [GridHidden]
        [NotMapped]
        public bool InsertModeTmp { get; set; }

        public Schadenfall SetInsertMode(bool insertMode)
        {
            InsertModeTmp = insertMode;
            return this;
        }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ID > 0 && EventID < 0)
                yield return new ValidationResult(Localize.EventInvalid, new[] {"EventID"});
        }
    }
}
