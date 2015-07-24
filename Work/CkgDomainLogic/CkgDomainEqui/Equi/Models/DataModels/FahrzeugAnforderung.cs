using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.Equi.ViewModels;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    public class FahrzeugAnforderung : IValidatableObject 
    {
        private string _kennzeichen;
        private string _dokTyp;

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string Fahrgestellnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        [Kennzeichen]
        public string Kennzeichen
        {
            get { return _kennzeichen.NotNullOrEmpty().ToUpper(); }
            set { _kennzeichen = value.NotNullOrEmpty().ToUpper(); }
        }

        [LocalizedDisplay(LocalizeConstants.EmailExtern)]
        [EmailAddress]
        public string EmailExtern { get; set; }

        [LocalizedDisplay(LocalizeConstants.EmailCreateUserSendTo)]
        public bool SendEmailToAnlageUser { get; set; }

        [LocalizedDisplay(LocalizeConstants.EmailCreateUser)]
        [EmailAddress]
        public string EmailAnlageUser { get; set; }

        [LocalizedDisplay(LocalizeConstants.DocumentType)]
        [Required]
        public string DokTyp
        {
            get { return _dokTyp.NotNullOrEmpty().ToUpper(); }
            set { _dokTyp = value.NotNullOrEmpty().ToUpper(); }
        }

        [LocalizedDisplay(LocalizeConstants.CreateUser)]
        public string AnlageUser { get; set; }

        [LocalizedDisplay(LocalizeConstants.CreateDate)]
        public DateTime? AnlageDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Remark)]
        public string Bemerkung { get; set; }

        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<EquiHistorieVermieterViewModel> GetViewModel { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!SendEmailToAnlageUser && EmailExtern.IsNullOrEmpty())
                yield return new ValidationResult(Localize.PleaseProvideEmailAddress, new[] { "EmailExtern" });
        }
    }
}
