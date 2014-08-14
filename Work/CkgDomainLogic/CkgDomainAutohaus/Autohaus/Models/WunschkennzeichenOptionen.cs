using System.Collections.Generic;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;
using System.Linq;

namespace CkgDomainLogic.Autohaus.Models
{
    public class WunschkennzeichenOptionen
    {
        public List<VinWunschkennzeichen> WunschkennzeichenList { get; set; }

        [ModelMappingCompareIgnore]
        public bool IsValid { get; set; }


        public void SetZulassungsKreis(string zulassungsKreis)
        {
            if (!WunschkennzeichenList.AnyAndNotNull())
                return;

            WunschkennzeichenList.ForEach(kennzeichen => kennzeichen.ZulassungsKreis = zulassungsKreis);
        }

        public string GetSummaryString()
        {
            return WunschkennzeichenList.Select(w => w.WunschKennzeichenAsString.FormatIfNotNull("{0}:<br />{this}", w.UniqueKey))
                                            .JoinIfNotNull("<br />");
        }
    }
}
