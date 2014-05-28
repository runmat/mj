using System;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Leasing.Models
{
    public class Klaerfall
    {
        [LocalizedDisplay(LocalizeConstants.CustomerNo)]
        public string Kundennummer { get; set; }
        
        [LocalizedDisplay(LocalizeConstants.Customer)]
        public string Kundenname { get; set; }

        [LocalizedDisplay(LocalizeConstants.LeasingContractNo)]
        public string Leasingvertragsnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfFirstWriting)]
        public DateTime? Erstanschreiben { get; set; }

        [LocalizedDisplay(LocalizeConstants.Reminder1)]
        public DateTime? Mahnung1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Reminder2)]
        public DateTime? Mahnung2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Reminder3)]
        public DateTime? Mahnung3 { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReceiptOfAuthorization)]
        public DateTime? EingangVollmacht { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReceiptOfCommercialRegister)]
        public DateTime? EingangHandelsregister { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReceiptOfBusinessRegistration)]
        public DateTime? EingangGewerbeanmeldung { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReceiptOfIdCard)]
        public DateTime? EingangPersonalausweis { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReceiptOfDirectDebitMandate)]
        public DateTime? EingangEinzugsermaechtigung { get; set; }

        [LocalizedDisplay(LocalizeConstants.EVB)]
        public string EVB { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReceiptOfInsuranceCertificate)]
        public DateTime? EingangSicherungsschein { get; set; }

        [LocalizedDisplay(LocalizeConstants.Remark)]
        public string Bemerkung { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name1)]
        public string Name1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name2)]
        public string Name2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name3)]
        public string Name3 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Street)]
        public string Strasse { get; set; }

        [LocalizedDisplay(LocalizeConstants.PostCode)]
        public string Plz { get; set; }

        [LocalizedDisplay(LocalizeConstants.City)]
        public string Ort { get; set; }

        [LocalizedDisplay(LocalizeConstants.Email)]
        public string Email { get; set; }

        [LocalizedDisplay(LocalizeConstants.Email2)]
        public string Email2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.DayOfDelivery)]
        public DateTime? Liefertermin { get; set; }

        [LocalizedDisplay(LocalizeConstants.CustomerManager)]
        public string Kundenbetreuer { get; set; }

        [LocalizedDisplay(LocalizeConstants.Manufacturer)]
        public string Hersteller { get; set; }

        [LocalizedDisplay(LocalizeConstants.VehicleType)]
        public string Fahrzeugtyp { get; set; }

        [LocalizedDisplay(LocalizeConstants.PreferredDate)]
        public DateTime? Wunschdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.DAT_Receiptdoc)]
        public DateTime? DruckdatumAnschreiben { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string Status { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChargeTax)]
        public bool SteuerAbrechnen { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationFor)]
        public string ZulassungAuf { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationBy)]
        public string ZulassungDurch { get; set; }

        [LocalizedDisplay(LocalizeConstants.OrderType)]
        public string Bestellart { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        public DateTime? Zulassungsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.DocumentsComplete)]
        public bool UnterlagenKomplett { get; set; }

    }
}
