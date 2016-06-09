using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Remarketing.ViewModels;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Remarketing.Models
{
    public class FehlendeDatenSelektor : Store
    {
        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<FehlendeDatenViewModel> GetViewModel { get; set; }

        [LocalizedDisplay(LocalizeConstants.Selection)]
        public string Auswahl { get; set; }

        [XmlIgnore]
        public static string AuswahlList
        {
            get
            {
                return string.Format("A,{0};F,{1};K,{2}",
                    Localize.SelectionCarRentalCompany,
                    Localize.UploadChassisNumbers,
                    Localize.UploadLicenseNumbers);
            }
        }

        [LocalizedDisplay(LocalizeConstants.CarRentalCompany)]
        public string Vermieter { get; set; }

        [XmlIgnore]
        public static List<Vermieter> VermieterList { get { return (GetViewModel == null ? new List<Vermieter>() : GetViewModel().Vermieter); } }

        [LocalizedDisplay(LocalizeConstants.Date)]
        public DateRange DatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days)); } set { PropertyCacheSet(value); } }

        private string _fahrgestellNr;
        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string FahrgestellNr
        {
            get { return _fahrgestellNr; }
            set { _fahrgestellNr = value.NotNullOrEmpty().ToUpper().Replace(" ", ""); }
        }

        private string _kennzeichen;
        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen
        {
            get { return _kennzeichen; }
            set { _kennzeichen = value.NotNullOrEmpty().ToUpper().Replace(" ", ""); }
        }

        [LocalizedDisplay(LocalizeConstants.Type)]
        public string Bestandsart { get; set; }

        [XmlIgnore]
        public static string BestandsartList
        {
            get
            {
                return string.Format("A,{0};C,{1};Z,{2};R,{3};G,{4};S,{5};T,{6};B,{7}",
                    Localize.AllIncompleteData,
                    Localize.CarportArrivalMissing,
                    Localize.Zb2ReceiptMissing,
                    Localize.BuyBackBillMissing,
                    Localize.SurveyReleaseMissing,
                    Localize.DeactivationDateMissing,
                    Localize.HandOverTuevMissing,
                    Localize.CreationDebitNoteMissing);
            }
        }

        public bool AuswahlChanged { get; set; }
    }
}
