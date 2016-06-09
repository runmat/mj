using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    public class EquiLebenslauf
    {
        [LocalizedDisplay(LocalizeConstants.Date)]
        public DateTime Datum { get; set; }

        [LocalizedDisplay(LocalizeConstants.EventEreignis)]
        public string Ereignis { get; set; }

        public EquiLebenslauf(DateTime datum, string ereignis)
        {
            Datum = datum;
            Ereignis = ereignis;
        }
    }
}
