using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class FahrzeuguebersichtPDI
    {              
        public string PDIKey { get; set; }

        [LocalizedDisplay(LocalizeConstants.Pdi)]
        public string PDIText { get; set; }       
    }
}
