using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.WFM.ViewModels;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.WFM.Models
{
    [GridColumnsAutoPersist]
    public class WfmAuftrag 
    {
        [LocalizedDisplay(LocalizeConstants.CaseNoDeregistrationOrder)]
        public string VorgangsNrAbmeldeauftrag { get; set; }

        [LocalizedDisplay(LocalizeConstants.OrderID)]
        public string AuftragsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.CustomerOrderId)]
        public string KundenAuftragsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string FahrgestellNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZB1)]
        public string Zb1Nr { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZB2)]
        public string Zb2Nr { get; set; }

        [LocalizedDisplay(LocalizeConstants.EquipmentNoZb1)]
        public string EquiNrZb1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.EquipmentNoZb2)]
        public string EquiNrZb2 { get; set; }

        [GridHidden]
        public string AbmeldeArtCode { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeRegistrationType)]
        public string AbmeldeArt
        {
            get
            {
                switch (AbmeldeArtCode)
                {
                    case "1":
                        return Localize.Standard;
                    case "2":
                        return Localize.ClarificationCases;
                    case "3":
                        return Localize.WithoutOrder;
                    default:
                        return AbmeldeArtCode;
                }
            }
        }

        [GridHidden]
        public bool AbmeldeArtIstStandard { get { return AbmeldeArtCode.NotNullOrEmpty() == "1"; } }

        [GridHidden]
        public string AbmeldeStatusCode { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeRegistrationStatus)]
        public string AbmeldeStatus
        {
            get
            {
                switch (AbmeldeStatusCode)
                {
                    case "0":
                        return Localize.Outstanding;
                    case "1":
                        return Localize.WorkInProgress;
                    case "2":
                        return Localize.Deregistered;
                    case "3":
                        return Localize.Cancelled;
                    default:
                        return AbmeldeStatusCode;
                }
            }
        }

        [LocalizedDisplay(LocalizeConstants.DeRegistrationDate)]
        public DateTime? AbmeldeDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReversalDate)]
        public DateTime? StornoDatum { get; set; }

        [GridHidden]
        public bool IstStorniert { get { return StornoDatum != null; } }

        [LocalizedDisplay(LocalizeConstants.Zb1AvailableDate)]
        public DateTime? Zb1VorhandenDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Zb2AvailableDate)]
        public DateTime? Zb2VorhandenDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicensePlateFrontAvailableDate)]
        public DateTime? KennzeichenVornVorhandenDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicensePlateRearAvailableDate)]
        public DateTime? KennzeichenHintenVorhandenDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicensePlateFrontVoidedDate)]
        public DateTime? KennzeichenVornEntwertetDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicensePlateRearVoidedDate)]
        public DateTime? KennzeichenHintenEntwertetDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicensePlateFrontStolenDate)]
        public DateTime? KennzeichenVornGestohlenDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicensePlateRearStolenDate)]
        public DateTime? KennzeichenHintenGestohlenDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.SalesReceipt)]
        public string Verkaufsbeleg { get; set; }

        [LocalizedDisplay(LocalizeConstants.Carport)]
        public string Carport { get; set; }

        [LocalizedDisplay(LocalizeConstants.RecordingDate)]
        public DateTime? Erfassungsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.ResubmissionCustomerDate)]
        public DateTime? WiedervorlageKundeDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.ResubmissionScDate)]
        public DateTime? WiedervorlageScDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.ApprovalZlsDate)]
        public DateTime? ZustimmungZlsDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.ApprovalRecipient)]
        public string ZustimmungEmpfaenger { get; set; }

        [LocalizedDisplay(LocalizeConstants.ApprovalUser)]
        public string ZustimmungUser { get; set; }

        [LocalizedDisplay(LocalizeConstants.NewRequestZlsDate)]
        public DateTime? NeuanforderungZlsDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.NewRequestRecipient)]
        public string NeuanforderungEmpfaenger { get; set; }

        [LocalizedDisplay(LocalizeConstants.NewRequestUser)]
        public string NeuanforderungUser { get; set; }

        [LocalizedDisplay(LocalizeConstants.Selection1)]
        public bool Selektion1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Selection2)]
        public bool Selektion2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Selection3)]
        public bool Selektion3 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Reference1)]
        public string Referenz1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Reference2)]
        public string Referenz2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Reference3)]
        public string Referenz3 { get; set; }

        [LocalizedDisplay(LocalizeConstants.CaseNoAncestorOrder)]
        public string VorgangsNrVorgaengerAuftrag { get; set; }

        [LocalizedDisplay(LocalizeConstants.SequenceNo)]
        public string LaufendeNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Task)]
        public string Aufgabe { get; set; }

        [LocalizedDisplay(LocalizeConstants.Recorded)]
        public decimal? Erfasst { get; set; }

        [LocalizedDisplay(LocalizeConstants.End)]
        public decimal? Ende { get; set; }

        [LocalizedDisplay(LocalizeConstants.TaskId)]
        public string TaskId { get; set; }

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

        [LocalizedDisplay(LocalizeConstants.User)]
        public string User { get; set; }

        [LocalizedDisplay(LocalizeConstants.Annotation)]
        public string Anmerkung { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string Status { get; set; }

        [LocalizedDisplay(LocalizeConstants.NextTaskId)]
        public string FolgetaskId { get; set; }


        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<WfmViewModel> GetViewModel { get; set; }

        [GridExportIgnore]
        [LocalizedDisplay(LocalizeConstants.Action)]
        public string Action { get; set; }
    }
}
