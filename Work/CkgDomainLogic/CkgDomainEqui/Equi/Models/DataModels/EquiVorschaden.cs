using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    public class EquiVorschaden
    {
        [LocalizedDisplay(LocalizeConstants.CreationDate)]
        public DateTime? Erstellungsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Price)]
        public decimal? Preis { get; set; }
    }
}
