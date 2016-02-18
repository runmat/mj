using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.ZldPartner.ViewModels;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.ZldPartner.Models
{
    public class OffeneZulassung
    {
        [LocalizedDisplay(LocalizeConstants.VoucherNo)]
        public string BelegNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.VoucherPosition)]
        public string BelegPosition { get; set; }

        public bool Hauptposition { get; set; }

        public string DatensatzId { get { return BelegNr.NotNullOrEmpty().PadLeft(10, '0') + BelegPosition.NotNullOrEmpty().PadLeft(5, '0'); } }

        [LocalizedDisplay(LocalizeConstants.MaterialNo)]
        public string MaterialNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Service)]
        public string MaterialText { get; set; }

        [LocalizedDisplay(LocalizeConstants.Supplier)]
        public string Lieferant { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeliveryDate)]
        public string LieferDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarOwner)]
        public string Halter { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string FahrgestellNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZB2No)]
        public string Zb2Nr { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationDistrict)]
        public string ZulassungsKreis { get; set; }

        [LocalizedDisplay(LocalizeConstants._Kennzeichen)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        public string ZulassungsDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.OrderNumber)]
        public string AuftragsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.OrderPosition)]
        public string AuftragsPosition { get; set; }

        [LocalizedDisplay(LocalizeConstants.Fee)]
        public string Gebuehr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Price)]
        public string Preis { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string Status { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string StatusText
        {
            get
            {
                switch (Status)
                {
                    case "EGG":
                        return "eingegangen";
                    case "IA":
                        return "in Arbeit";
                    case "DGF":
                        return "durchgeführt";
                    case "STO":
                        return "storniert";
                    case "FGS":
                        return "fehlgeschlagen";
                    default:
                        return Status;
                }
            }
        }

        [LocalizedDisplay(LocalizeConstants.FeeRelevant)]
        public bool Gebuehrenrelevant { get; set; }

        [LocalizedDisplay(LocalizeConstants.Origin)]
        public string Herkunft { get; set; }

        [LocalizedDisplay(LocalizeConstants.VoucherFeePosition)]
        public string BelegGebuehrenPosition { get; set; }

        [LocalizedDisplay(LocalizeConstants.InstructedBy)]
        public string BeauftragtVon { get; set; }

        [LocalizedDisplay(LocalizeConstants.Phone)]
        public string Telefon { get; set; }

        [LocalizedDisplay(LocalizeConstants.Email)]
        public string Email { get; set; }

        [LocalizedDisplay(LocalizeConstants.Reason)]
        public string StornoGrundId { get; set; }

        [LocalizedDisplay(LocalizeConstants.Reason)]
        public string StornoGrund
        {
            get
            {
                return (GetViewModel != null && GetViewModel().Gruende.Any(g => g.GrundId == StornoGrundId) ? GetViewModel().Gruende.First(g => g.GrundId == StornoGrundId).GrundText : StornoGrundId);
            }
        }

        public string BemerkungLangtextNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Comment)]
        public string Bemerkung { get; set; }

        public string Erfasser { get; set; }

        public string ValidationMessage
        {
            get
            {
                if (ValidationErrorList.AnyAndNotNull())
                    return ValidationErrorList.First().ErrorMessage;

                return "";
            }
        }

        [XmlIgnore]
        public List<ValidationResult> ValidationErrorList { get; set; }

        [XmlIgnore]
        public string ValidationErrorsJson { get { return new JavaScriptSerializer().Serialize(ValidationErrorList); } }

        [ModelMappingCompareIgnore]
        public bool ValidationOk { get { return String.IsNullOrEmpty(ValidationMessage); } }

        public string SaveMessage { get; set; }

        [ModelMappingCompareIgnore]
        public bool SaveOk { get { return String.IsNullOrEmpty(SaveMessage); } }

        [LocalizedDisplay(LocalizeConstants.Hint)]
        public string BearbeitungsStatus
        {
            get
            {
                if (!ValidationOk)
                    return ValidationMessage;

                if (!SaveOk)
                    return SaveMessage;

                return "";
            }
        }

        public bool IsChanged { get; set; }

        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<ZldPartnerZulassungenViewModel> GetViewModel { get; set; }

        [XmlIgnore]
        public bool EditMode { get { return GetViewModel != null && GetViewModel().EditMode; } }

        public OffeneZulassung()
        {
            IsChanged = false;
            ValidationErrorList = new List<ValidationResult>();
        }
    }
}
