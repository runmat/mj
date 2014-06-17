using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Logs.Models
{
    public class WebServiceTrafficLogItemUI
    {
        public int Id { get; set; }

        [LocalizedDisplay(LocalizeConstants.Type)]
        public string Type { get; set; }

        [LocalizedDisplay(LocalizeConstants.Date)]
        public DateTime Time_Stamp { get; set; }

        [LocalizedDisplay(LocalizeConstants.Content)]
        public string AllXmlPreview { get; set; }
    }
}
