using System;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class FahrzeugvoravisierungUploadModel
    {

        [ImportIgnore()]
        public bool IsSelected { get; set; }
              
        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string Fahrgestellnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.OrderNumber)]
        public string Auftragsnummer { get; set; }
                      
        [LocalizedDisplay(LocalizeConstants.OrderNumberEqui)]
        public string AuftragsnummerEqui { get; set; }

        [LocalizedDisplay(LocalizeConstants.UnitNumber)]
        public string UnitNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.ModelID)]
        public string ModelID { get; set; }

        [LocalizedDisplay(LocalizeConstants.ModelID2UnitNr)]
        public string ModelID2UnitNr { get; set; }


        [LocalizedDisplay(LocalizeConstants.Registration)]
        public DateTime? DatumZulassung { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string Status { get; set; }

        [ImportIgnore()]
        [LocalizedDisplay(LocalizeConstants.Error)]
        public string ValidationErrors { get; set; }
        
    }
}
