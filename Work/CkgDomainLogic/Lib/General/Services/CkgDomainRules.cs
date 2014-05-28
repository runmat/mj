using GeneralTools.Models;

namespace CkgDomainLogic.General.Services
{
    public class CkgDomainRules
    {
        public static bool IstKroschkeAutohaus(string kundenNr) { return kundenNr.NotNullOrEmpty().Trim() == "261010"; }
    }
}
