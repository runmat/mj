using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.VersEvents.ViewModels;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.VersEvents.Models
{
    [Table("VersEventOrtBoxVorgangTermin")]
    public class VorgangTermin
    {
        [Key]
        public int ID { get; set; }

        [GridHidden]
        [LocalizedDisplay(LocalizeConstants.CustomerNo)]
        public int KundenNr { get; set; }

        [GridHidden]
        public int VersBoxID { get; set; }

        [GridHidden]
        public int VersEventID { get; set; }

        [GridHidden]
        public int VersOrtID { get; set; }

        [GridHidden]
        public int VorgangID { get; set; }

        [LocalizedDisplay(LocalizeConstants.Date)]
        public DateTime Datum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Time)]
        public string Zeit { get; set; }

        [LocalizedDisplay(LocalizeConstants.Duration)]
        public int DauerMinuten { get; set; }
        
        [LocalizedDisplay(LocalizeConstants.Note)]
        public string Bemerkung { get; set; }

        [LocalizedDisplay(LocalizeConstants.VersEventLocation)]
        [NotMapped]
        public string EventAndOrtAsText
        {
            get
            {
                return (VersEventID == 0 || VersOrtID == 0)
                            ? Localize.DropdownDefaultOptionPleaseChoose
                            : string.Format("{0} / {1}", Event.EventName, EventOrt.OrtAsText);
            }
        }


        [LocalizedDisplay(LocalizeConstants.CreateDate)]
        public DateTime AnlageDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.CreateUser)]
        public string AnlageUser { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeleteDate)]
        public DateTime? LoeschDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeleteUser)]
        public string LoeschUser { get; set; }


        [NotMapped]
        [GridHidden]
        public static Func<VersEventsViewModel> GetViewModel { get; set; }


        [GridHidden]
        [NotMapped]
        public Vorgang Vorgang
        {
            get
            {
                return GetViewModel != null 
                           ? (GetViewModel().Vorgaenge.FirstOrDefault(v => v.ID == VorgangID) 
                                ?? new Vorgang {Name1 = Localize.DropdownDefaultOptionPleaseChoose})
                           : null;
            }
        }

        [GridHidden]
        [NotMapped]
        public VersEvent Event
        {
            get
            {
                return GetViewModel != null
                          ? (GetViewModel().VersEvents.FirstOrDefault(e => e.ID == VersEventID)
                              ?? new VersEvent { EventName = "" })
                          : null;
            }
        }

        [GridHidden]
        [NotMapped]
        public VersEventOrt EventOrt
        {
            get
            {
                return GetViewModel != null
                          ? (GetViewModel().VersEventOrteAlle.FirstOrDefault(e => e.ID == VersOrtID)
                              ?? new VersEventOrt { Ort = "" })
                          : null;
            }
        }

        [GridHidden]
        [NotMapped]
        public string EventOrtSelectCssClass { get { return VersOrtID == 0 ? "blue" : "black"; } }

        [GridHidden]
        [NotMapped]
        public string EventOrtSelectLabel { get { return string.Format("{0} {1}", "Event-Ort", (VersOrtID == 0 ? "wählen" : "ändern")); } }

        [GridHidden]
        [NotMapped]
        public string VorgangSelectCssClass { get { return VorgangID == 0 ? "blue" : "black"; } }

        [GridHidden]
        [NotMapped]
        public string VorgangSelectLabel { get { return string.Format("{0} {1}", "Vorgang", (VorgangID == 0 ? "wählen" : "ändern")); } }
    }
}
