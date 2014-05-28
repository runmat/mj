using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.General.Models
{
    /// <summary>
    /// Fahrgestellnummer (10-stellig)
    /// </summary>
    public class Fahrgestellnummer10
    {
        [LocalizedDisplay(LocalizeConstants.ChassisNo10)]
        public string FIN { get; set; }

        public Fahrgestellnummer10()
        {
            this.FIN = "";
        }

        public Fahrgestellnummer10(string fin)
        {
            this.FIN = fin;
        }
    }
}
