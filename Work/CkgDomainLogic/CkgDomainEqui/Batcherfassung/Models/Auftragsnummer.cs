using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.FzgModelle.Models
{
    public class Auftragsnummer : Store
    {            
        [LocalizedDisplay(LocalizeConstants.OrderNumber)]
        public string Nummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.OrderNumberDescription)]
        public string AuftragsNrText { get; set; }

        public string AuftragsNrUndText { get { return string.Format("{0} - {1}", Nummer, AuftragsNrText); } }
    }
}
