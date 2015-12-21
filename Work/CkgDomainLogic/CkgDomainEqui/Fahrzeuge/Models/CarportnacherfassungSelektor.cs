using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.Fahrzeuge.ViewModels;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class CarportnacherfassungSelektor : Store 
    {
        [LocalizedDisplay(LocalizeConstants.OrderNumber)]
        public string AuftragsNr { get; set; }

        [Length(7, true)]
        [RegularExpression(@"^[a-zA-Z]{2}\d{5}$", ErrorMessage = "Ungültiges Bestandsnummer-Format")]
        [LocalizedDisplay(LocalizeConstants.InventoryNumber)]
        public string BestandsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string FahrgestellNr
        {
            get { return PropertyCacheGet(() => ""); }
            set { PropertyCacheSet(value.NotNullOrEmpty().ToUpper()); }
        }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen
        {
            get { return PropertyCacheGet(() => ""); }
            set { PropertyCacheSet(value.NotNullOrEmpty().ToUpper()); }
        }

        public int AnzahlTreffer
        {
            get { return (GetViewModel != null && GetViewModel().Fahrzeuge != null ? GetViewModel().Fahrzeuge.Count : 0); }
        }

        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<CarporterfassungViewModel> GetViewModel { get; set; }

        public string UserCarportId
        {
            get { return (GetViewModel != null ? GetViewModel().UserCarportId : ""); }
        }

        public string UserOrganization
        {
            get { return (GetViewModel != null ? GetViewModel().UserOrganization : ""); }
        }

        public bool UserAllOrganizations
        {
            get { return (GetViewModel != null && GetViewModel().UserAllOrganizations); }
        }
    }
}
