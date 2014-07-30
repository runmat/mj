using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    /// <summary>
    /// Parametersatz für die Fahrzeugbriefsuche
    /// </summary>
    public class FahrzeugbriefSuchparameter
    {
        [LocalizedDisplay(LocalizeConstants.Selection)]
        public string Selektion { get; set; }

        public string SelektionsOptionen { get { return "alle,alle;bestand,nur Briefe im Bestand;tempvers,nur temporär versendete"; } }
    }
}
