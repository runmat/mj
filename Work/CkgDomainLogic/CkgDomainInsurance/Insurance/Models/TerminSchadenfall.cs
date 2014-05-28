using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Insurance.ViewModels;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;
using MvcTools.Web;

namespace CkgDomainLogic.Insurance.Models
{
    [Table("VersEventSchadenfallOrtBoxTermin")]
    public class TerminSchadenfall : Store, IValidatableObject
    {
        [Key]
        public int ID { get; set; }

        [GridHidden]
        [LocalizedDisplay(LocalizeConstants.CustomerNo)]
        public string KundenNr { get; set; }

        [GridHidden]
        public int VersSchadenfallID { get; set; }

        [GridHidden]
        [LocalizedDisplay(LocalizeConstants.VersEventLocation)]
        public int VersOrtID { get; set; }

        [GridHidden]
        [LocalizedDisplay(LocalizeConstants.Box)]
        public int VersBoxID { get; set; }

        [GridHidden]
        [LocalizedDisplay(LocalizeConstants.BoxType)]
        [NotMapped]
        public string BoxArtGewuenscht { get; set; }

        [GridHidden]
        [LocalizedDisplay(LocalizeConstants.BoxType)]
        [NotMapped]
        public string BoxArtGewuenschtAsText { get { return BoxArtGewuenscht.IsNullOrEmpty() ? "" : VersEventOrtBox.BoxArten.ToSelectList().First(art => art.Value == BoxArtGewuenscht).Text; } }


        [LocalizedDisplay(LocalizeConstants.VersEventLocation)]
        [NotMapped]
        public string OrtAsText { get { return Ort.OrtAsShortText; } }

        [LocalizedDisplay(LocalizeConstants.Box)]
        [NotMapped]
        public string BoxAsText { get { return Box.BoxName; } }

        [LocalizedDisplay(LocalizeConstants.BoxType)]
        [NotMapped]
        public string BoxArtAsText { get { return Box.BoxArtAsText; } }


        [LocalizedDisplay(LocalizeConstants.Date)]
        public DateTime Datum { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateTo)]
        public DateTime? DatumTmpBlockerSerieBis { get; set; }

        [NotMapped]
        [GridHidden]
        [LocalizedDisplay(LocalizeConstants.Date)]
        public DateTime DatumZeitVon { get { return ZeitVon.IsNullOrEmpty() ? DateTime.MinValue : Datum.AddHours(ZeitVon.Split(':')[0].ToInt()).AddMinutes(ZeitVon.Split(':')[1].ToInt()); } }

        [NotMapped]
        [GridHidden]
        [LocalizedDisplay(LocalizeConstants.Date)]
        public DateTime DatumZeitBis { get { return ZeitBis.IsNullOrEmpty() ? DateTime.MinValue.AddMinutes(1) : Datum.AddHours(ZeitBis.Split(':')[0].ToInt()).AddMinutes(ZeitBis.Split(':')[1].ToInt()); } }

        [LocalizedDisplay(LocalizeConstants.TimeStart)]
        [Required]
        public string ZeitVon { get; set; }

        [LocalizedDisplay(LocalizeConstants.TimeEnd)]
        [Required]
        public string ZeitBis { get; set; }

        [LocalizedDisplay(LocalizeConstants.Duration)]
        [NotMapped]
        public int DauerMinuten { get { return (int)(DatumZeitBis - DatumZeitVon).TotalMinutes; } }
        
        [LocalizedDisplay(LocalizeConstants.Comment)]
        public string Bemerkung { get; set; }

        [LocalizedDisplay(LocalizeConstants.EventName)]
        [NotMapped]
        public string EventAsText
        {
            get
            {
                return (VersSchadenfallID == 0)
                            ? Localize.DropdownDefaultOptionPleaseChoose
                            : string.Format("{0}", Event.EventName);
            }
        }

        [LocalizedDisplay(LocalizeConstants.DamageCase)]
        [NotMapped]
        public string SchadenfallAsText
        {
            get { return string.Format("{0}, {1} {2}", Schadenfall.Kennzeichen, Schadenfall.Vorname, Schadenfall.Nachname); }
        }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        [NotMapped]
        public string SchadenfallKennzeichen { get { return Schadenfall.Kennzeichen; } }

        [LocalizedDisplay(LocalizeConstants.FirstName)]
        [NotMapped]
        public string SchadenfallVorname { get { return Schadenfall.Vorname; } }

        [LocalizedDisplay(LocalizeConstants.LastName)]
        [NotMapped]
        public string SchadenfallNachname { get { return Schadenfall.Nachname; } } 

        [LocalizedDisplay(LocalizeConstants.Insurance)]
        [NotMapped]
        public string VersicherungName { get { return Schadenfall.VersicherungName; } }


        [LocalizedDisplay(LocalizeConstants.CreateDate)]
        [GridResponsiveVisible(GridResponsive.Workstation)]
        public DateTime AnlageDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.CreateUser)]
        [GridResponsiveVisible(GridResponsive.Workstation)]
        public string AnlageUser { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeleteDate)]
        [GridHidden]
        public DateTime? LoeschDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeleteUser)]
        [GridHidden]
        public string LoeschUser { get; set; }


        [NotMapped]
        [GridHidden]
        public static Func<VersEventsViewModel> GetViewModel { get; set; }


        [GridHidden]
        [NotMapped]
        public Schadenfall Schadenfall
        {
            get { return GetViewModel().Schadenfaelle.FirstOrDefault(v => v.ID == VersSchadenfallID) ?? new Schadenfall(); }
        }

        [GridHidden]
        [NotMapped]
        public VersEvent Event
        {
            get { return (!IsBlockerDummyTermin ? Schadenfall.Event : GetViewModel().VersEventCurrent) ?? new VersEvent(); }
        }

        [GridHidden]
        [NotMapped]
        public VersEventOrt Ort
        {
            get { return Event.Orte.FirstOrDefault(ort => ort.ID == VersOrtID) ?? new VersEventOrt(); }
        }

        [GridHidden]
        [NotMapped]
        public VersEventOrtBox Box
        {
            get { return Ort.Boxen.FirstOrDefault(box => box.ID == VersBoxID) ?? new VersEventOrtBox(); }
        }


        [GridHidden]
        [NotMapped]
        public string MailtextTerminbestaetigung
        {
            get
            {
                var mailText = String.Format(Localize.VersEventMailAppointmentConfirmation, 
                    Schadenfall.Vorname, Schadenfall.Nachname,
                    Datum.ToLongDateString(), ZeitVon, Ort.OrtName, Ort.OrtName2, Ort.StrasseHausNr, Ort.PLZ, Ort.Ort, Ort.Land, Box.BoxName, Box.BoxNr, Box.BoxArtAsText);

                mailText = mailText.Replace("<br/>", "%0D%0A");

                return mailText;
            }      
        }

        [GridHidden]
        [NotMapped]
        public bool IsBlockerDummyTermin { get { return VersSchadenfallID == 0; } }


        bool ValidateTimeDuplicatesAndIntersections(Action<string, string> addModelError)
        {
            var viewModel = GetViewModel();
            if (viewModel == null)
                return false;

            var dataStoreRealTimeItems = viewModel.DataService.TermineGet(null, VersBoxID);

            if (!viewModel.InsertMode)
                // skip current appointment while comparing with the list of stored appointments
                // (but only if we are NOT in InsertMode (because item.ID will not be valid in insert mode))
                dataStoreRealTimeItems = dataStoreRealTimeItems.Where(realTimeItem => realTimeItem.ID != ID).ToList();

            var existingRealTimeItemOverlapping = dataStoreRealTimeItems
                .FirstOrDefault(realTimeItem =>
                        (DatumZeitBis > realTimeItem.DatumZeitVon) &&
                        (DatumZeitVon < realTimeItem.DatumZeitBis));
            if (existingRealTimeItemOverlapping != null)
            {
                addModelError("", string.Format("{0} '{1}', {2}: {3:dd.MM.yyyy HH:mm}-{4:HH:mm}",
                                        Localize.VersEventAppointmentCreateIntersection,
                                        existingRealTimeItemOverlapping.Schadenfall.Kennzeichen,
                                        Localize.DateFromTo,
                                        existingRealTimeItemOverlapping.DatumZeitVon,
                                        existingRealTimeItemOverlapping.DatumZeitBis
                                        ));
                return false;
            }

            return true;
        }

        public bool Validate(Action<string, string> addModelError)
        {
            if (DatumZeitVon >= DatumZeitBis)
            {
                addModelError("ZeitVon", Localize.TimeStartShouldBeBeforeTimeEnd);
                addModelError("ZeitBis", Localize.TimeStartShouldBeBeforeTimeEnd);
                return false;
            }

            if (!Ort.ValidateTimeInDayTimeRange(DatumZeitVon, DatumZeitBis, addModelError))
                return false;

            if (!ValidateTimeDuplicatesAndIntersections(addModelError))
                return false;

            return true;
        }

        public List<VersEventOrt> GetValidOrte()
        {
            return Event.GetValidOrte(GetCachedBoxArt(), Schadenfall).ToList(); 
        }

        public List<VersEventOrtBox> GetValidBoxenForThisTermin()
        {
            if (GetCachedBoxArt() == "RE")
                // für RE Modus (Werkstatt Modus) alle Boxen zulassen, weil der User die Box über die entsprechende Calendar Komponente selbst bestimmt
                return GetValidBoxen().ToList();

            return GetValidBoxen().Where(box => GetViewModel().GetTermineEinerBoxAllerSchadenFaelle(box.ID).Where(t => t.DatumZeitVon == this.DatumZeitVon).None()).ToList();
        }

        public List<VersEventOrtBox> GetValidBoxen()
        {
            return Ort.GetValidBoxen(GetCachedBoxArt(), Schadenfall).ToList();
        }

        public List<TerminSchadenfall> GetTermineForValidBoxen()
        {
            var termineAllerBoxenUeberAllerSchadenFaelle = GetValidBoxen().SelectMany(box => GetViewModel().GetTermineEinerBoxAllerSchadenFaelle(box.ID)).ToList();

            return termineAllerBoxenUeberAllerSchadenFaelle;
        }
        
        public string GetCachedBoxArt()
        {
            if (BoxArtGewuenscht.IsNotNullOrEmpty()) 
                return BoxArtGewuenscht;

            var globalBox = Ort.Boxen.FirstOrDefault(b => b.ID == VersBoxID);
            if (globalBox == null)
                return "";

            BoxArtGewuenscht = globalBox.BoxArt;

            return BoxArtGewuenscht;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Datum.DayOfWeek == DayOfWeek.Sunday)
                yield return new ValidationResult(Localize.AppointmentsOnSundayNotAvailable, new[] { "Datum" });

            if (DatumTmpBlockerSerieBis != null)
            {
                if (DatumTmpBlockerSerieBis.GetValueOrDefault() <= Datum)
                    yield return new ValidationResult(Localize.DateToMustBeBeforeDateFrom, new[] { "DatumTmpBlockerSerieBis" });
                
                if (DatumTmpBlockerSerieBis.GetValueOrDefault().DayOfWeek == DayOfWeek.Sunday)
                    yield return new ValidationResult(Localize.AppointmentsOnSundayNotAvailable, new[] { "DatumTmpBlockerSerieBis" });
            }
        }
    }
}
