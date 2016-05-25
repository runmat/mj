using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Leasing.Models
{
    public class EndgueltigerVersandModel
    {
        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string   Fahrgestellnummer { get; set; }
        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }
        [LocalizedDisplay(LocalizeConstants.ContractNo)]
        public string Vertragsnummer { get; set; }
        [LocalizedDisplay(LocalizeConstants.DateOfDispatch)]
        public DateTime?  Versanddatum { get; set; }

        [GridHidden]
        public bool IsSelected { get; set; }
        [LocalizedDisplay(LocalizeConstants.Action)]   
        public string Aktion { get; set; }

        [GridHidden]
        public string EQUNR { get; set; }


    }
}
