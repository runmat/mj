using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Globalization;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.WFM.ViewModels;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.WFM.Models
{
    public class WfmToDo
    {
        [LocalizedDisplay(LocalizeConstants.CaseNoDeregistrationOrder)]
        public string VorgangsNrAbmeldeauftrag { get; set; }

        [LocalizedDisplay(LocalizeConstants.SequenceNo)]
        public string LaufendeNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Task)]
        public string Aufgabe { get; set; }

        [LocalizedDisplay(LocalizeConstants.TaskId)]
        public string TaskId { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string FahrgestellNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.ToDoWho)]
        public string ToDoWer { get; set; }

        [LocalizedDisplay(LocalizeConstants.StartDate)]
        public DateTime? Startdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.StartTime)]
        public string Startzeit { get; set; }

        [LocalizedDisplay(LocalizeConstants.NominalDate)]
        public DateTime? Solldatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.NominalTime)]
        public string Sollzeit { get; set; }

        [LocalizedDisplay(LocalizeConstants.ActualDate)]
        public DateTime? Istdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.ActualTime)]
        public string Istzeit { get; set; }

        [LocalizedDisplay(LocalizeConstants.Date)]
        public DateTime? DatumZeit
        {
            get
            {
                DateTime tmpZeit;
                var tmpDat = Startdatum;

                if (tmpDat.HasValue
                    && DateTime.TryParseExact(Startzeit, "HHmmss", CultureInfo.CurrentCulture, DateTimeStyles.None, out tmpZeit))
                {
                    return tmpDat.Value.AddHours(tmpZeit.Hour).AddMinutes(tmpZeit.Minute).AddSeconds(tmpZeit.Second);
                }

                return tmpDat;
            }
        }

        [LocalizedDisplay(LocalizeConstants.User)]
        public string User { get; set; }

        [LocalizedDisplay(LocalizeConstants.Annotation)]
        public string Anmerkung { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string Status { get; set; }

        [LocalizedDisplay(LocalizeConstants.NextTaskId)]
        public string FolgetaskId { get; set; }

        [LocalizedDisplay(LocalizeConstants.NextTask)]
        public string FolgetaskAufgabe { get; set; }

        [GridHidden, NotMapped, XmlIgnore]
        public bool IstHoechsteLfdNr
        {
            get { return GetViewModel != null && GetViewModel().IstHoechsteLfdNr(LaufendeNr.ToInt()); }
        }

        static string GroupWithToDoConfirmationRights { get { return ConfigurationManager.AppSettings["GroupWithToDoConfirmationRights"] ?? "KUNDE"; } }

        [GridHidden, NotMapped, XmlIgnore]
        public bool ToDoKundeBestaetigungsBerechtigung
        {
            get 
            {
                return (ToDoWer.NotNullOrEmpty().ToUpper() == GroupWithToDoConfirmationRights) && IsUnConfirmed && IstHoechsteLfdNr; 
            }
        }

        [GridHidden, NotMapped, XmlIgnore]
        public bool IsUnConfirmed { get { return Status.NotNullOrEmpty() == "1"; } }

        [GridHidden, NotMapped, XmlIgnore]
        public bool IsConfirmed { get { return Status.NotNullOrEmpty() == "2"; } }
        
        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<WfmViewModel> GetViewModel { get; set; }
    }
}
