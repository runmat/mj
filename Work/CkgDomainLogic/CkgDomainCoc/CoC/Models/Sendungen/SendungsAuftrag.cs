using System;
using CkgDomainLogic.General.Services;
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

        [LocalizedDisplay(LocalizeConstants.Date)]
        public DateTime? VersandDatum { get; set; }

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
        [GridRawHtmlButForceGridExport]
        public string VersandAdresseAsText { get; set; }

        [LocalizedDisplay(LocalizeConstants.Shipping)]
        public string VersandWeg { get; set; }

        public string VersandKey { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReferenceNo)]
        public string PoolNummer { get; set; }

        [LocalizedDisplay(LocalizeConstants._ZBIINr)]
        public string Fahrzeugbrief { get; set; }

        [LocalizedDisplay(LocalizeConstants.Reference1)]
        public string Referenz1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Reference2)]
        public string Referenz2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.MaterialNo)]
        public string Materialnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.Description)]
        public string Bezeichnung { get; set; }


        [LocalizedDisplay(LocalizeConstants.ShippingSurvey)]
        [GridExportIgnore]
        public string VersandIdSurveyLink
        {
            get
            {
                var keySuffix = VersandKey.NotNullOrEmpty().ToUpper();

                switch (keySuffix)
                {
                    case "1":
                        keySuffix = "DHL";
                        break;
                    case "2":
                        keySuffix = "TNT";
                        break;
                    case "3":
                        keySuffix = "GO";
                        break;
                    case "4":
                        keySuffix = "UPS";
                        break;
                }

                var key = string.Format("Url_{0}", keySuffix);

                var surveyLink = GeneralConfiguration.GetConfigValue("Sendungsverfolgung", key);
                if (surveyLink.IsNullOrEmpty())
                    return "#";

                return string.Format(surveyLink, VersandID);
            }
        }
    }
}
