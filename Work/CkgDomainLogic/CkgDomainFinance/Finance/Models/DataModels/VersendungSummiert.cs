using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Finance.Models
{
    public class VersendungSummiert
    {
        [LocalizedDisplay(LocalizeConstants.DealerNo)]
        public string HaendlerNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Dealer)]
        public string HaendlerName { get; set; }

        [LocalizedDisplay(LocalizeConstants.Dealer)]
        public string Haendler { get { return string.Format("{0} ~ {1}", HaendlerNr, HaendlerName); } }

        [LocalizedDisplay(LocalizeConstants.ContractType)]
        public string Vertragsart { get; set; }

        [LocalizedDisplay(LocalizeConstants.WayOfRequest)]
        public string Anforderungsweg { get; set; }

        [LocalizedDisplay(LocalizeConstants.RequestType)]
        public string Anforderungsart { get; set; }

        [LocalizedDisplay(LocalizeConstants.CauseOfDispatch)]
        public string Versandgrund { get; set; }

        [LocalizedDisplay(LocalizeConstants.Sum)]
        public string Summe { get; set; }

        [LocalizedDisplay(LocalizeConstants.StockDealer)]
        public string BestandHaendler { get; set; }

        [LocalizedDisplay(LocalizeConstants.AmountInPercent)]
        public string AnteilInProzent { get; set; }
    }
}
