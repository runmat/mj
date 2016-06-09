using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    public class EquiLebenslaufSchluessel
    {
        [LocalizedDisplay(LocalizeConstants.KeyDispatch)]
        public DateTime? AusgangSchluessel { get; set; }
    }
}
