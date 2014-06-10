using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;
using CkgDomainLogic.Insurance.ViewModels;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Insurance.Models
{
    public class VersEvent : Store 
    {
        [Key]
        [SelectListKey]
        public int ID { get; set; }

        [GridHidden]
        [LocalizedDisplay(LocalizeConstants.CustomerNo)]
        public string KundenNr { get; set; }

        [GridHidden]
        [LocalizedDisplay(LocalizeConstants.DeleteDate)]
        public DateTime? LoeschDatum { get; set; }

        [GridHidden]
        [LocalizedDisplay(LocalizeConstants.DeleteUser)]
        public string LoeschUser { get; set; }


        [LocalizedDisplay(LocalizeConstants.EventName)]
        [SelectListText]
        [Required]
        public string EventName { get; set; }

        [LocalizedDisplay(LocalizeConstants.Region)]
        public string Region { get; set; }

        [LocalizedDisplay(LocalizeConstants.VersEventLocations)]
        [GridRawHtml]
        public string EventOrteTemplateHtml { get { return EventOrteTemplateFunction == null ? "-" : EventOrteTemplateFunction(this); } }

        [NotMapped]
        [LocalizedDisplay(LocalizeConstants.VersEventLocations)]
        public string OrtsNamen
        {
            get { return string.Join(", ", Orte.Select(ort => ort.OrtName)); }
        }

        [LocalizedDisplay(LocalizeConstants.Categorie)]
        public string Kategorie { get; set; }

        [LocalizedDisplay(LocalizeConstants.VehicleVolume)]
        public int FahrzeugVolumen { get; set; }

        [LocalizedDisplay(LocalizeConstants.EventDescription)]
        [GridHidden]
        public string Beschreibung { get; set; }

        [LocalizedDisplay(LocalizeConstants.Date)]
        public DateTime StartDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateTo)]
        [GridHidden]
        public DateTime EndDatum { get; set; }


        [LocalizedDisplay(LocalizeConstants.CreateDate)]
        public DateTime AnlageDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.CreateUser)]
        public string AnlageUser { get; set; }

        
        [NotMapped]
        public static Func<VersEventsViewModel> GetViewModel { get; set; }

        public static Func<VersEvent, string> EventOrteTemplateFunction { get; set; }

        [GridHidden]
        [NotMapped]
        [ScriptIgnore]
        public List<VersEventOrt> Orte
        {
            get { return PropertyCacheGet(() => (GetViewModel == null ? new List<VersEventOrt>() : GetViewModel().EventsDataService.VersEventOrteGet(this))); }
        }


        public IEnumerable<VersEventOrt> GetValidOrte(string boxArt, Schadenfall schadenfall)
        {
            if (boxArt.IsNullOrEmpty())
                return new List<VersEventOrt>();

            var boxenOrteIds = Orte.SelectMany(ort => ort.GetValidBoxen(boxArt, schadenfall).Select(box => box.VersOrtID));

            return Orte.Where(ort => boxenOrteIds.Contains(ort.ID)).ToList();
        }

        public void OrteReset()
        {
            PropertyCacheClear(this, m => m.Orte);
        }
    }
}
