using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Finance.Models
{
    public class Auftrag
    {
        [LocalizedDisplay(LocalizeConstants.OrderID)]
        public string Auftragsnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.PreferredDeliveryDate)]
        public DateTime Wunschlieferdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractType)]
        public string Vertragsart { get; set; }

        [LocalizedDisplay(LocalizeConstants.Account)]
        public string Kontonummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.CIN)]
        public string CIN { get; set; }

        [LocalizedDisplay(LocalizeConstants.PAID)]
        public string PAID { get; set; }

        [LocalizedDisplay(LocalizeConstants.CreditUser)]
        public string Kreditnehmer { get; set; }

        [LocalizedDisplay(LocalizeConstants.TempDispatch)]
        public DateTime DatumTempVersand { get; set; }

        [LocalizedDisplay(LocalizeConstants.NameOfRegistrationOffice)]
        public string KreditorAusBestellung { get; set; }

        [LocalizedDisplay(LocalizeConstants.CauseOfExpenditure)]
        public string GrundDerAuslage { get; set; }

        [LocalizedDisplay(LocalizeConstants.InvoiceReceiptDate)]
        public DateTime Rechnungseingangsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.NetEffort)]
        public string Wert { get; set; }

        [LocalizedDisplay(LocalizeConstants.IncludedTaxes)]
        public string Gebuehren { get; set; }

        [LocalizedDisplay(LocalizeConstants.Comment)]
        public string BemerkungZeile1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Comment2)]
        public string BemerkungZeile2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Done)]
        public bool Erledigt { get; set; }
    }
}
