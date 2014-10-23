using System;
using System.Web;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    public class VersandBeauftragungX
    {
        [LocalizedDisplay(LocalizeConstants.ContractNo)]
        public string VertragsNummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        [GridHidden]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string FIN { get; set; }

        [LocalizedDisplay(LocalizeConstants.EquipmentNo)]
        public string EquiNummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.Memo)]
        public string Memo { get; set; }

        [LocalizedDisplay(LocalizeConstants.OrderDate)]
        public DateTime? DatumBeauftragung { get; set; }

        [LocalizedDisplay(LocalizeConstants.Holder)]
        [GridHidden]
        public string VersandAdresse { get; set; }

        [LocalizedDisplay(LocalizeConstants.Holder)]
        [GridHidden]
        [GridRawHtml]
        public IHtmlString VersandAdresseAsHtml { get { return new HtmlString(VersandAdresse.Replace(",", "<br />")); } }
    }
}
