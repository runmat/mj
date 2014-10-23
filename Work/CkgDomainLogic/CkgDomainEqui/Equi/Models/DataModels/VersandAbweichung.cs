using System;
using System.Web;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    public class VersandAbweichungX
    {
        [LocalizedDisplay(LocalizeConstants.ContractNo)]
        public string VertragsNummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string FIN { get; set; }

        [LocalizedDisplay(LocalizeConstants.EquipmentNo)]
        public string EquiNummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.Memo)]
        public string Memo { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfReceipt)]
        public DateTime? DatumEingang { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfStart)]
        public DateTime? DatumAusgang { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        [GridHidden]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.Holder)]
        [GridHidden]
        public string HalterAdresse { get; set; }

        [LocalizedDisplay(LocalizeConstants.Holder)]
        [GridHidden]
        [GridRawHtml]
        public IHtmlString HalterAdresseAsHtml { get { return new HtmlString(HalterAdresse.Replace(",", "<br />")); } }
    }
}
