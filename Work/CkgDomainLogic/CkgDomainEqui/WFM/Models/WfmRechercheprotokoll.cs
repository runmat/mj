using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.WFM.Models
{
    public class WfmRechercheprotokoll
    {
        public string KennungAnsprechpartner { get; set; }

        [LocalizedDisplay(LocalizeConstants.CompanyOrName)]
        public string Name { get; set; }

        [LocalizedDisplay(LocalizeConstants.Phone)]
        public string Telefon { get; set; }

        [LocalizedDisplay(LocalizeConstants.PhoneNoNotAssigned)]
        public bool TelefonNrNichtVergeben { get; set; }

        public WfmRechercheprotokollVersuch Versuch1 { get; set; }

        public WfmRechercheprotokollVersuch Versuch2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.EmailAddress)]
        public string EmailAdresse { get; set; }

        [LocalizedDisplay(LocalizeConstants.Contactperson)]
        public string Ansprechpartner { get; set; }

        public WfmRechercheprotokoll()
        {
            Versuch1 = new WfmRechercheprotokollVersuch();
            Versuch2 = new WfmRechercheprotokollVersuch();
        }
    }
}
