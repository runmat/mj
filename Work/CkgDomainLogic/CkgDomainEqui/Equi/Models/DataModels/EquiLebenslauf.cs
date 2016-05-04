using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    public class EquiLebenslauf
    {
        [LocalizedDisplay(LocalizeConstants.Date)]
        public DateTime? Datum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Event)]
        public string Ereignis { get; set; }
    }
}
