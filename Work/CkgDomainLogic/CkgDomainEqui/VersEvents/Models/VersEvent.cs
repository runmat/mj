using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.VersEvents.ViewModels;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.VersEvents.Models
{
    public class VersEvent
    {
        [Key]
        public int ID { get; set; }

        [GridHidden]
        [LocalizedDisplay(LocalizeConstants.CustomerNo)]
        public int KundenNr { get; set; }

        [GridHidden]
        [LocalizedDisplay(LocalizeConstants.DeleteDate)]
        public DateTime? LoeschDatum { get; set; }

        [GridHidden]
        [LocalizedDisplay(LocalizeConstants.DeleteUser)]
        public string LoeschUser { get; set; }


        [LocalizedDisplay(LocalizeConstants.EventName)]
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
        public List<VersEventOrt> Orte
        {
            get { return GetViewModel == null ? new List<VersEventOrt>() : GetViewModel().DataService.VersEventOrteGet(this); }
        }
    }
}
