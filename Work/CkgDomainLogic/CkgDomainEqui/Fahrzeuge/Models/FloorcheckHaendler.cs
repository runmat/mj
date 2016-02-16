using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.Fahrzeuge.ViewModels;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class FloorcheckHaendler
    {
       
        [LocalizedDisplay(LocalizeConstants.DealerNo)]
        public string HaendlerNummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.Dealer)]        
        public string HaendlerName { get; set; }

        [LocalizedDisplay(LocalizeConstants.DealerCity)]
        public string HaendlerOrt { get; set; }
                     
    }
}
