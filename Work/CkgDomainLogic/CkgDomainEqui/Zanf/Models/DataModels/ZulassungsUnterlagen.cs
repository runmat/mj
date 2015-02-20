using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Zanf.Models
{
    public class ZulassungsUnterlagen
    {
        [LocalizedDisplay(LocalizeConstants.Customer)]
        public string HalterId { get; set; }

        [LocalizedDisplay(LocalizeConstants.Location)]
        public string Standort { get; set; }

        [LocalizedDisplay(LocalizeConstants.AuthorizationPresent)]
        public bool VollmachtVorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.AuthorizationMissing)]
        public bool VollmachtFehlt { get; set; }

        [LocalizedDisplay(LocalizeConstants.CommercialRegisterPresent)]
        public bool HandelsregisterVorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.CommercialRegisterMissing)]
        public bool HandelsregisterFehlt { get; set; }

        [LocalizedDisplay(LocalizeConstants.IdCardPresent)]
        public bool PersonalausweisVorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.IdCardMissing)]
        public bool PersonalausweisFehlt { get; set; }

        [LocalizedDisplay(LocalizeConstants.BusinessRegistrationPresent)]
        public bool GewerbeanmeldungVorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.BusinessRegistrationMissing)]
        public bool GewerbeanmeldungFehlt { get; set; }

        [LocalizedDisplay(LocalizeConstants.DirectDebitMandatePresent)]
        public bool EinzugsermaechtigungVorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.DirectDebitMandateMissing)]
        public bool EinzugsermaechtigungFehlt { get; set; }

        [LocalizedDisplay(LocalizeConstants.Complete)]
        public bool Vollstaendig { get; set; }

        [LocalizedDisplay(LocalizeConstants.EvbNumber)]
        public string EvbNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.EvbValidFrom)]
        public DateTime? EvbGueltigVon { get; set; }

        [LocalizedDisplay(LocalizeConstants.EvbValidTo)]
        public DateTime? EvbGueltigBis { get; set; }

        [LocalizedDisplay(LocalizeConstants.Comment)]
        public string Bemerkung { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationDocumentsAtRegistrationOffice)]
        public bool ZulassungsunterlagenBeiZls { get; set; }

        [LocalizedDisplay(LocalizeConstants.KeyNoRegistrationOffice)]
        public string SchluesselNrZls { get; set; }

        [LocalizedDisplay(LocalizeConstants.EvbMissing)]
        public bool EvbFehlt { get; set; }

        [LocalizedDisplay(LocalizeConstants.AlwaysOrderParticulateSticker)]
        public bool FeinstaubplaketteImmerBeauftragen { get; set; }

        [LocalizedDisplay(LocalizeConstants.ShipLicensePlateEnhancer)]
        public bool KennzeichenverstaerkerVersenden { get; set; }

        [LocalizedDisplay(LocalizeConstants.PersonalisedLicenseNo)]
        public string Wunschkennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.Misc)]
        public string Sonstiges { get; set; }

        [LocalizedDisplay(LocalizeConstants.Attorney1)]
        public string Bevollmaechtigter1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.ValidTo1)]
        public DateTime? GueltigBis1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Attorney2)]
        public string Bevollmaechtigter2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.ValidTo2)]
        public DateTime? GueltigBis2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Attorney3)]
        public string Bevollmaechtigter3 { get; set; }

        [LocalizedDisplay(LocalizeConstants.ValidTo3)]
        public DateTime? GueltigBis3 { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationArea)]
        public string Zulassungskreis { get; set; }

        [LocalizedDisplay(LocalizeConstants.AcquisitionDateCommercialRegBusinessReg)]
        public DateTime? BeschaffungsdatumHandelsregisterGewerbeanmeldung { get; set; }

        [LocalizedDisplay(LocalizeConstants.DirectDebitMandateVehicleRelated)]
        public bool EinzugsermaechtigungFahrzeugbezogen { get; set; }

        [LocalizedDisplay(LocalizeConstants.ExternalCostumerNo)]
        public string ExterneKundenNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeleteDate)]
        public DateTime? Loeschdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Deleted)]
        public bool Geloescht { get; set; }

        [LocalizedDisplay(LocalizeConstants.WebUser)]
        public string Webuser { get; set; }

        [LocalizedDisplay(LocalizeConstants.Barcode)]
        public string Barcode { get; set; }

        [LocalizedDisplay(LocalizeConstants.AuthorizationValidFrom)]
        public DateTime? VollmachtGueltigVon { get; set; }

        [LocalizedDisplay(LocalizeConstants.AuthorizationValidTo)]
        public DateTime? VollmachtGueltigBis { get; set; }

        [LocalizedDisplay(LocalizeConstants.CommercialRegisterValidFrom)]
        public DateTime? HandelsregisterGueltigVon { get; set; }

        [LocalizedDisplay(LocalizeConstants.CommercialRegisterValidTo)]
        public DateTime? HandelsregisterGueltigBis { get; set; }

        [LocalizedDisplay(LocalizeConstants.BusinessRegistrationValidFrom)]
        public DateTime? GewerbeanmeldungGueltigVon { get; set; }

        [LocalizedDisplay(LocalizeConstants.BusinessRegistrationValidTo)]
        public DateTime? GewerbeanmeldungGueltigBis { get; set; }

        [LocalizedDisplay(LocalizeConstants.StockOriginalAuthorization)]
        public string BestandOriginalVollmacht { get; set; }

        [LocalizedDisplay(LocalizeConstants.DocumentId)]
        public string DokumentId { get; set; }

        [LocalizedDisplay(LocalizeConstants.RecordingDate)]
        public DateTime? Erfassungsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.RecordedBy)]
        public string ErfasstVon { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChangeDate)]
        public DateTime? Aenderungsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChangedBy)]
        public string GeaendertVon { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarOwnerName)]
        public string HalterName { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarOwnerCity)]
        public string HalterOrt { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string Status { get; set; }
    }
}
