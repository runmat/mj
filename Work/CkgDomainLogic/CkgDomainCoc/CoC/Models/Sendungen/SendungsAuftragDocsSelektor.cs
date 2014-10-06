using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.CoC.Models
{
    public class SendungsAuftragDocsSelektor : Store
    {
        [LocalizedDisplay(LocalizeConstants._blank)]
        public bool FilterTestNurMitSendungsNummer { get; set; }
    }
}