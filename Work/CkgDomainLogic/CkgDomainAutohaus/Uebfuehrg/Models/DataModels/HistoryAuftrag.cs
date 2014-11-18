using System;
using GeneralTools.Models;
using GeneralTools.Resources;
using SapORM.Contracts;

namespace CkgDomainLogic.Uebfuehrg.Models
{
    public class HistoryAuftrag
    {
        [LocalizedDisplay(LocalizeConstants.OrderID)]
        public string AuftragsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants._IhreReferenz)]
        public string Referenz { get; set; }

        [LocalizedDisplay(LocalizeConstants._Kundenberater)]
        public string Kundenberater { get; set; }

        [LocalizedDisplay(LocalizeConstants.OrderDate)]
        public DateTime? AuftragsDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants._Fahrt)]
        public string Fahrt { get; set; }

        [LocalizedDisplay(LocalizeConstants._Kennzeichen)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants._FahrzgeugTyp)]
        public string Typ { get; set; }

        [LocalizedDisplay(LocalizeConstants._Ueberfuehrungsdatum)]
        public DateTime? UeberfuehrungsDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants._Abgabedatum)]
        public DateTime? AbgabeDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants._FahrtVon)]
        public string FahrtVonOrt { get; set; }

        [LocalizedDisplay(LocalizeConstants._FahrtNach)]
        public string FahrtNachOrt { get; set; }

        [LocalizedDisplay(LocalizeConstants._GefahreneKm)]
        public string GefahreneKilometer { get; set; }

        [LocalizedDisplay(LocalizeConstants._Klaerfall)]
        public string Klaerfall { get; set; }

        [LocalizedDisplay(LocalizeConstants.Contactperson)]
        public string Ansprechpartner { get; set; }

        [LocalizedDisplay(LocalizeConstants.Phone)]
        public string Telefon { get; set; }

        [LocalizedDisplay(LocalizeConstants.Email)]
        public string Email { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string FahrgestellNr { get; set; }

        public string AuftragsNrWebView { get { return AuftragsNr.ToSapKunnr(); } }

        public string AuftragsNrWebViewTrimmed
        {
            get
            {
                int anr;
                if (!Int32.TryParse(AuftragsNrWebView, out anr))
                    return AuftragsNrWebView;

                return anr.ToString();
            }
        }
    }
}
