using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.Fahrer.ViewModels;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrer.Models
{
    public class ProtokollEditModel : IValidatableObject
    {
        public FahrerAuftragsProtokoll Protokoll { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.OverpassDate)]
        public DateTime? UeberfuehrungsDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.PickupDate)]
        public DateTime? AbholDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.PickupTime)]
        public string AbholUhrzeit { get; set; }

        [LocalizedDisplay(LocalizeConstants.HandoverDate)]
        public DateTime? UebergabeDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Mileage)]
        public int Kilometerstand { get; set; }

        [LocalizedDisplay(LocalizeConstants.SignatureAvailable)]
        public bool UnterschriftVorhanden { get; set; }

        [XmlIgnore]
        public List<SelectItem> QmCodeList { get { return GetViewModel == null ? new List<SelectItem>() : GetViewModel().QmCodes; } }

        [LocalizedDisplay(LocalizeConstants.QmCode)]
        public string QmCode { get; set; }

        [LocalizedDisplay(LocalizeConstants.QmComment)]
        public string QmBemerkung { get; set; }

        [LocalizedDisplay(LocalizeConstants.EmailAddressee)]
        public string MailAdressen { get; set; }

        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<FahrerViewModel> GetViewModel { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            DateTime tmpDat;
            if (!String.IsNullOrEmpty(AbholUhrzeit) && !DateTime.TryParseExact(AbholUhrzeit, "HHmm", CultureInfo.CurrentCulture, DateTimeStyles.None, out tmpDat))
                yield return new ValidationResult(Localize.TimeInvalid, new[] { "AbholUhrzeit" });
        }
    }
}
