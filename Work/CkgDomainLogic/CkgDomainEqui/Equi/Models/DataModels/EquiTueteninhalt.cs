using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    public class EquiTueteninhalt
    {
        [LocalizedDisplay(LocalizeConstants.Description)]
        public string Bezeichnung { get; set; }

        [LocalizedDisplay(LocalizeConstants.Quantity)]
        public int? Anzahl { get; set; }
    }
}
