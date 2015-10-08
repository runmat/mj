using System.ComponentModel.DataAnnotations;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Leasing.Models
{
    public class UeberfaelligeRuecksendungenSuchparameter
    {
        [Required]
        [LocalizedDisplay(LocalizeConstants.NumberOfDays)]
        public int AnzahlTage { get; set; }

        public static int[] AuswahlAnzahlTage { get { return new[] { 5, 10, 15, 20, 25, 30 }; } }
    }
}
