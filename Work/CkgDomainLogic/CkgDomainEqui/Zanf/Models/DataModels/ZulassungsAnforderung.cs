using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Zanf.Models
{
    [GridColumnsAutoPersist]
    public class ZulassungsAnforderung
    {
        [LocalizedDisplay(LocalizeConstants.RequestNo)]
        public string AnforderungsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.OrderID)]
        public string AuftragsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Service)]
        public string Dienstleistung { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string FahrgestellNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReferenceNo)]
        public string ReferenzNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.OrderDate)]
        public DateTime? AuftragsDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        public DateTime? ZulassungsDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string Status { get; set; }

        [LocalizedDisplay(LocalizeConstants.EquipmentNo)]
        public string EquiNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.User)]
        public string User { get; set; }

        public ZanfAdresse HalterAlt { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarOwnerOld)]
        public string AdresseHalterAlt { get { return HalterAlt.Adresse; } }

        public ZanfAdresse Halter { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarOwner)]
        public string AdresseHalter { get { return Halter.Adresse; } }

        public ZanfAdresse Haendler { get; set; }

        [LocalizedDisplay(LocalizeConstants.Dealer)]
        public string AdresseHaendler { get { return Haendler.Adresse; } }

        [LocalizedDisplay(LocalizeConstants.ClarificationCaseText)]
        public string KlaerfallText { get; set; }

        [GridExportIgnore]
        [LocalizedDisplay(LocalizeConstants.Action)]
        public string Action1 { get; set; }

        public ZulassungsAnforderung()
        {
            HalterAlt = new ZanfAdresse();
            Halter = new ZanfAdresse();
            Haendler = new ZanfAdresse();
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
