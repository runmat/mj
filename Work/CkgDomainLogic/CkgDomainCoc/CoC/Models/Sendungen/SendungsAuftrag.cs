using System;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.CoC.Models
{
    public class SendungsAuftrag
    {
        [LocalizedDisplay(LocalizeConstants.CreateDate)]
        public DateTime? AnlageDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string FIN { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.InvoiceNo)]
        public string RechnungsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        public DateTime? ZulassungsDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.ShippingID)]
        public string VersandID { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractNo)]
        public string VertragsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReferenceNo)]
        public string Referenz { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string StatusText { get; set; }

        [LocalizedDisplay(LocalizeConstants.Delivered)]
        public bool StatusAusgeliefert { get { return StatusText.NotNullOrEmpty().ToLower() == "ausgeliefert"; } }

        [LocalizedDisplay(LocalizeConstants.ShippingAddress)]
        public string VersandAdresseAsText { get; set; }

        [LocalizedDisplay(LocalizeConstants.Shipping)]
        public string VersandWeg { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReferenceNo)]
        public string PoolNummer { get; set; }

        [LocalizedDisplay(LocalizeConstants._ZBIINr)]
        public string Fahrzeugbrief { get; set; }

        [LocalizedDisplay(LocalizeConstants.ShippingSurvey)]
        [GridExportIgnore]
        public string VersandIdSurveyLink
        {
            get
            {
                var keySuffixArray = VersandWeg.NotNullOrEmpty().Split(' ').ToArrayOrEmptyArray();
                var keySuffix = (keySuffixArray.Length > 1 ? keySuffixArray[0] : VersandWeg.NotNullOrEmpty()).ToUpper();
                var key = string.Format("SendungsverfolgungExternUrl_{0}", keySuffix);

                var surveyLink = ApplicationConfiguration.GetApplicationConfigValue(key, "1", 1);
                if (surveyLink.IsNullOrEmpty())
                    return "#";

                return string.Format(surveyLink, VersandID);
            }
        }
    }
}
