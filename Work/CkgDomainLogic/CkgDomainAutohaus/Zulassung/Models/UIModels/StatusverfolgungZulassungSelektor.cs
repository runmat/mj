using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.Autohaus.ViewModels;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Autohaus.Models
{
    public class StatusverfolgungZulassungSelektor : Store, IZulassungsReportSelektor, IValidatableObject
    {
        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<StatusverfolgungZulassungViewModel> GetViewModel { get; set; }

        [LocalizedDisplay(LocalizeConstants.Customer)]
        public string KundenNr { get; set; }

        [XmlIgnore]
        public static List<Kunde> Kunden
        {
            get { return (GetViewModel == null ? new List<Kunde>() : GetViewModel().Kunden).CopyAndInsertAtTop(new Kunde("", Localize.DropdownDefaultOptionAll)); }
        }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        [KennzeichenPartial]
        public string Kennzeichen
        {
            get { return PropertyCacheGet(() => "").NotNullOrEmpty().ToUpper(); }
            set { PropertyCacheSet(value.NotNullOrEmpty().ToUpper()); }
        }

        [LocalizedDisplay(LocalizeConstants.DispatchType)]
        public string AuftragsArt
        {
            get { return PropertyCacheGet(() => "1"); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public static string AuftragsArtOptionen
        {
            get { return string.Format("1,{0};2,{1};3,{2}", Localize.AllOrders, Localize.FinishedOrders, Localize.OpenOrders); }
        }

        [LocalizedDisplay(LocalizeConstants.OrderDate)]
        public DateRange AuftragsDatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days)); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        public DateRange ZulassungsDatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days, true)); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.CarOwner)]
        public string Referenz1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string Referenz2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.AhZulassungCostcenter)]
        public string Referenz3 { get; set; }

        [LocalizedDisplay(LocalizeConstants.AhZulassungOrderNo)]
        public string Referenz4 { get; set; }

        [LocalizedDisplay(LocalizeConstants.AhZulassungReferenceNo)]
        public string Referenz5 { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!AuftragsDatumRange.IsSelected && !ZulassungsDatumRange.IsSelected)
                yield return new ValidationResult(Localize.PleaseChooseAtLeastOneOption, new[] { "AuftragsDatumRange", "ZulassungsDatumRange" });
        }
    }
}
