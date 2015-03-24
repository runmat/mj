using System.Collections.Generic;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.WFM.Models
{
    public class WfmAuftragSelektor : Store 
    {
        public List<SelectItem> AlleAbmeldearten
        {
            get
            {
                return PropertyCacheGet(() => new List<SelectItem>
                {
                    new SelectItem("1", Localize.Standard),
                    new SelectItem("2", Localize.ClarificationCases)
                });
            }
        }

        public List<SelectItem> AlleAbmeldestatus
        {
            get
            {
                return PropertyCacheGet(() => new List<SelectItem>
                {
                    new SelectItem("0", Localize.Outstanding),
                    new SelectItem("1", Localize.WorkInProgress),
                    new SelectItem("2", Localize.Deregistered),
                    new SelectItem("3", Localize.Cancelled)
                });
            }
        }

        [LocalizedDisplay(LocalizeConstants.DeRegistrationType)]
        public List<string> Abmeldearten { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeRegistrationStatus)]
        public List<string> Abmeldestatus { get; set; }

        [LocalizedDisplay(LocalizeConstants.Selection1)]
        public bool Selektionsfeld1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Selection2)]
        public bool Selektionsfeld2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Selection3)]
        public bool Selektionsfeld3 { get; set; }

        public string Selektionsfeld1Name { get; set; }
        public string Selektionsfeld2Name { get; set; }
        public string Selektionsfeld3Name { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string FahrgestellNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.CustomerOrderId)]
        public string KundenAuftragsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Reference1)]
        public string Referenz1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Reference2)]
        public string Referenz2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Reference3)]
        public string Referenz3 { get; set; }

        public string Referenz1Name { get; set; }
        public string Referenz2Name { get; set; }
        public string Referenz3Name { get; set; }
    }
}
