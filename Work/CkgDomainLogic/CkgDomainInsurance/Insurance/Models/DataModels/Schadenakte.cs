using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Insurance.Models
{
    public class Schadenakte
    {
        [LocalizedDisplay(LocalizeConstants.DamageCase)]
        public Schadenfall Schadenfall { get; set; }
    }
}
