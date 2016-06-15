using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.CoC.ViewModels;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.CoC.Models
{
    public class SendungsAuftragPlaceSelektor : Store
    {


        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<SendungenViewModel> GetViewModel { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateRange)]
        public DateRange DatumRangeZul
        {
            get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last7Days) { IsSelected = true }); }
            set { PropertyCacheSet(value); }
        }

        public List<SelectItem> Standorte
        {
            get { return GetViewModel == null ? new List<SelectItem>() : GetViewModel().Standorte; }
        }
        public string Fahrzeugstandort { get; set; }

        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string FIN { get; set; }

        [LocalizedDisplay(LocalizeConstants._Kennzeichen)]
        public string Kennzeichen { get; set; }


    }
}