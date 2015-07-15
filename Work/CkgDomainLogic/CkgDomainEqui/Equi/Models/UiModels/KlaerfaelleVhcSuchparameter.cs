using System;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    public class KlaerfaelleVhcSuchparameter
    {
        [LocalizedDisplay(LocalizeConstants.Selection)]
        public string Auswahl { get; set; }

        public string AuswahlOptionen { get { return String.Format("K,{0};D,{1}", Localize.ClarificationCases, Localize.DataWithoutZB2); } }
    }
}
