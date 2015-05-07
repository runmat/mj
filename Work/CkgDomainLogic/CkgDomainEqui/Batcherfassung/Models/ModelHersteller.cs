using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.FzgModelle.ViewModels;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.FzgModelle.Models
{
    public class ModelHersteller : Store
    {
        [LocalizedDisplay(LocalizeConstants.ModelID)]
        public string ModelID { get; set; }

        [LocalizedDisplay(LocalizeConstants.Model)]        
        public string Modellbezeichnung { get; set; }

        [LocalizedDisplay(LocalizeConstants.Manufacturer)]       
        public string HerstellerCode { get; set; }
         
        [LocalizedDisplay(LocalizeConstants.Manufacturer)]
        public string HerstellerName { get; set; }

        [LocalizedDisplay(LocalizeConstants.SippCode)]        
        public string SippCode { get; set; }

        [LocalizedDisplay(LocalizeConstants.Bluetooth)]
        public bool Bluetooth { get; set; }

        [LocalizedDisplay(LocalizeConstants.EngineType)]
        public string Antrieb { get; set; }

        [LocalizedDisplay(LocalizeConstants.Towbar)]
        public bool AnhaengerKupplung { get; set; }
    }
}
