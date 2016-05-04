using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Leasing.Models.DataModels
{
    public class AbweichungWiedereingang
    {
        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string FIN { get; set; }

        [LocalizedDisplay(LocalizeConstants.LeasingContractNo)]
        public string LV { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNoOld)]
        public string AltesKennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string NeuesKennzeichen { get; set; }


        [LocalizedDisplay(LocalizeConstants.CarOwnerNew)]
        public string NeuerHalter { get { return String.Format("{0}, {1}", NeuerHalterName, NeuerHalterOrt); } }

        [LocalizedDisplay(LocalizeConstants.CarOwnerOld)]
        public string AlterHalter { get { return String.Format("{0}, {1}", AltHalterName, AlterHalterOrt); } }

        [GridHidden]
        [LocalizedDisplay(LocalizeConstants.CarOwnerOld)]
        public string AltHalterName { get; set; }

        [GridHidden]
        [LocalizedDisplay(LocalizeConstants.CarOwner)]
        public string NeuerHalterName { get; set; }

        [GridHidden]
        [LocalizedDisplay(LocalizeConstants.CarOwnerCity)]
        public string AlterHalterOrt { get; set; }

        [GridHidden]
        [LocalizedDisplay(LocalizeConstants.CarOwnerCity)]
        public string NeuerHalterOrt { get; set; }
        
        [LocalizedDisplay(LocalizeConstants._Zulassungsdatum)]
        public DateTime? AktuellesZulDat { get; set; }

    }
}
