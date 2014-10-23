using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Finance.Models
{
    /// <summary>
    /// Parametersatz für die Mahnungssuche
    /// </summary>
    public class MahnungVorErsteingangSuchparameter
    {
        [Required]
        [LocalizedDisplay(LocalizeConstants.ContractType)]
        public string Vertragsart { get; set; }

        public List<string> AuswahlVertragsart { get; set; }

        [LocalizedDisplay(LocalizeConstants.DunningLevel1)]
        public bool Mahnstufe1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.DunningLevel2)]
        public bool Mahnstufe2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.DunningLevel3)]
        public bool Mahnstufe3 { get; set; }

        [LocalizedDisplay(LocalizeConstants.DunningBlockSet)]
        public bool MahnsperreGesetzt { get; set; }
    }
}
