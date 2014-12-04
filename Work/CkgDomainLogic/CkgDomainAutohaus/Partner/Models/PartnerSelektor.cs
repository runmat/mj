using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Partner.Models
{
    public class PartnerSelektor
    {
        private string _fin;

        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string FIN
        {
            get { return _fin.NotNullOrEmpty().ToUpper(); }
            set { _fin = value.NotNullOrEmpty().ToUpper(); }
        }

        [LocalizedDisplay(LocalizeConstants.January)]
        public bool TestFlag { get; set; }

        public string PartnerKennung { get { return !TestFlag ? "HALTER" : "KAEUFER"; } }
    }
}
