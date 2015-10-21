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
        public string BelegNrShow { get { return BelegNr; } }
        [LocalizedDisplay(LocalizeConstants.VoucherNo)]
        public string BelegNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.VoucherPosition)]
        public string BelegPositionShow { get { return BelegPosition; } }
        [LocalizedDisplay(LocalizeConstants.VoucherPosition)]
        public string BelegPosition { get; set; }

        public string DatensatzId { get { return BelegNr.NotNullOrEmpty().PadLeft(10, '0') + BelegPosition.NotNullOrEmpty().PadLeft(5, '0'); } }

        [LocalizedDisplay(LocalizeConstants.MaterialNo)]
        public string MaterialNrShow { get { return MaterialNr; } }
        [LocalizedDisplay(LocalizeConstants.MaterialNo)]
        public string MaterialNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Service)]
        public string MaterialTextShow { get { return MaterialText; } }
        [LocalizedDisplay(LocalizeConstants.Service)]
        public string MaterialText { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeliveryDate)]
        public string LieferDatumShow { get { return LieferDatum; } }
        [LocalizedDisplay(LocalizeConstants.DeliveryDate)]
        public string LieferDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarOwner)]
        public string HalterShow { get { return Halter; } }
        [LocalizedDisplay(LocalizeConstants.CarOwner)]
        public string Halter { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string FahrgestellNrShow { get { return FahrgestellNr; } }
        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string FahrgestellNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZB2No)]
        public string Zb2NrShow { get { return Zb2Nr; } }
        public string Zb2Nr { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationDistrict)]
        public string ZulassungsKreisShow { get { return ZulassungsKreis; } }
        [LocalizedDisplay(LocalizeConstants.RegistrationDistrict)]
        public string ZulassungsKreis { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string KennzeichenShow { get { return Kennzeichen; } }
        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        public string ZulassungsDatumShow { get { return ZulassungsDatum; } }
        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        public string ZulassungsDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.OrderNumber)]
        public string AuftragsNrShow { get { return AuftragsNr; } }
        [LocalizedDisplay(LocalizeConstants.OrderNumber)]
        public string AuftragsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.OrderPosition)]
        public string AuftragsPositionShow { get { return AuftragsPosition; } }
        [LocalizedDisplay(LocalizeConstants.OrderPosition)]
        public string AuftragsPosition { get; set; }

        [LocalizedDisplay(LocalizeConstants.Fee)]
        public string GebuehrShow { get { return Gebuehr; } }
        [LocalizedDisplay(LocalizeConstants.Fee)]
        public string Gebuehr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Price)]
        public string PreisShow { get { return Preis; } }
        [LocalizedDisplay(LocalizeConstants.Price)]
        public string Preis { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string StatusShow { get { return Status; } }
        [UIHint("OffeneZulassungStatus")]
        [LocalizedDisplay(LocalizeConstants.Status)]
        public string Status { get; set; }

        public static string StatusWerte { get { return GetViewModel == null ? "" : GetViewModel().StatusWerte; } }

        public string StatusCode
        {
            get
            {
                switch (Status)
                {
                    case "in Arbeit":
                        return "IA";
                    case "durchgeführt":
                        return "DGF";
                    case "storniert":
                        return "STO";
                    case "fehlgeschlagen":
                        return "FGS";
                    default:
                        return "";
                }
            }
        }

        [LocalizedDisplay(LocalizeConstants.FeeRelevant)]
        public bool GebuehrenrelevantShow { get { return Gebuehrenrelevant; } }
        [LocalizedDisplay(LocalizeConstants.FeeRelevant)]
        public bool Gebuehrenrelevant { get; set; }

        [LocalizedDisplay(LocalizeConstants.Origin)]
        public string HerkunftShow { get { return Herkunft; } }
        [LocalizedDisplay(LocalizeConstants.Origin)]
        public string Herkunft { get; set; }

        [LocalizedDisplay(LocalizeConstants.VoucherFeePosition)]
        public string BelegGebuehrenPositionShow { get { return BelegGebuehrenPosition; } }
        [LocalizedDisplay(LocalizeConstants.VoucherFeePosition)]
        public string BelegGebuehrenPosition { get; set; }

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

        public void SetStatus(string statusCode)
        {
            switch (statusCode)
            {
                case "IA":
                    Status = "in Arbeit";
                    break;
                case "DGF":
                    Status = "durchgeführt";
                    break;
                case "STO":
                    Status = "storniert";
                    break;
                case "FGS":
                    Status = "fehlgeschlagen";
                    break;
                default:
                    Status = "";
                    break;
            }
        }
    }
}
