using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.General.Models
{
    /// <summary>
    /// Fahrgestellnummer
    /// </summary>
    public class Fahrgestellnummer
    {
        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string FIN { get; set; }

        public Fahrgestellnummer()
        {
            this.FIN = "";
        }

        public Fahrgestellnummer(string fin)
        {
            this.FIN = fin;
        }
    }
}
