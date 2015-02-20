using System;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Zanf.Models
{
    public class ZulassungsUnterlagenSuchparameter
    {
        [LocalizedDisplay(LocalizeConstants.CarOwnerName)]
        public string HalterName { get; set; }

        [LocalizedDisplay(LocalizeConstants.Selection)]
        public string Auswahl { get; set; }

        [XmlIgnore]
        public string AuswahlOptionen
        {
            get
            {
                return String.Format("A,{0};V,{1};U,{2}",
                    Localize.All,
                    Localize.Complete,
                    Localize.Incomplete);
            }
        }
    }
}
