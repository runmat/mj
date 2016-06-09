using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    public class EquiLebenslaufBrief
    {
        [LocalizedDisplay(LocalizeConstants.Zb2Dispatch)]
        public DateTime? AusgangZb2 { get; set; }
    }
}
