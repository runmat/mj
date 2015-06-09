using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Autohaus.Models
{
    public class Vorgang
    {
        private List<PdfFormular> _zusatzformulare = new List<PdfFormular>();

        [LocalizedDisplay(LocalizeConstants.ReceiptNo)]
        public string BelegNr { get; set; }

        public string VkOrg { get; set; }

        public string VkBur { get; set; }

        public string Vorerfasser { get; set; }

        public string VorgangsStatus { get; set; }

        public Rechnungsdaten Rechnungsdaten { get; set; }

        public BankAdressdaten BankAdressdaten { get; set; }

        public Fahrzeugdaten Fahrzeugdaten { get; set; }

        public Adressdaten Halter { get; set; }

        public BankAdressdaten ZahlerKfzSteuer { get; set; }

        public List<Kunde> Kunden { get; set; }

        public string HalterName
        {
            get
            {
                if (Halter != null)
                    return Halter.Name;

                return "";
            }
        }

        public string ZahlerKfzSteuerName
        {
            get
            {
                if (ZahlerKfzSteuer != null)
                    return ZahlerKfzSteuer.Adressdaten.Name;

                return "";
            }
        }

        public Zulassungsdaten Zulassungsdaten { get; set; }

        public OptionenDienstleistungen OptionenDienstleistungen { get; set; }

        [XmlIgnore]
        public List<PdfFormular> Zusatzformulare
        {
            get { return _zusatzformulare; }
            set { _zusatzformulare = value; }
        }

        [XmlIgnore]
        public byte[] KundenformularPdf { get; set; }

        public Vorgang()
        {
            Rechnungsdaten = new Rechnungsdaten();
            BankAdressdaten = new BankAdressdaten("RE", true);
            Fahrzeugdaten = new Fahrzeugdaten { FahrzeugartId = "1" };
            Halter = new Adressdaten("HALTER") { Partnerrolle = "ZH"};
            ZahlerKfzSteuer = new BankAdressdaten("Z6", false, "ZAHLERKFZSTEUER");
            OptionenDienstleistungen = new OptionenDienstleistungen();
        }


        [XmlIgnore, ScriptIgnore]
        string BeauftragungBezeichnungKunde
        {
            get
            {
                return String.Format("{0}: {1}, {2}, {3}, {4}",
                    Fahrzeugdaten.AuftragsNr,
                    Rechnungsdaten.GetKunde(Kunden).KundenNameNr,
                    Zulassungsdaten.Zulassungsart.MaterialText,
                    HalterName,
                    Zulassungsdaten.Kennzeichen);
            }
        }

        [XmlIgnore, ScriptIgnore]
        private GeneralEntity SummaryBeauftragungsHeaderKunde
        {
            get
            {
                return new GeneralEntity
                {
                    Title = Localize.YourOrder,
                    Body = BeauftragungBezeichnungKunde,
                    Tag = "SummaryMainItem"
                };
            }
        }

        [XmlIgnore, ScriptIgnore]
        private GeneralEntity SummaryBeauftragungsHeader
        {
            get
            {
                if (BelegNr.IsNullOrEmpty())
                    return null;

                return new GeneralEntity
                {
                    Title = Localize.OurReceiptNo,
                    Body = string.Format("{0}", BelegNr),
                    Tag = "SummaryMainItem"
                };
            }
        }

        public GeneralSummary CreateSummaryModel()
        {
            var summaryModel = new GeneralSummary
            {
                Header = (Zulassungsdaten.ModusAbmeldung ? Localize.OrderSummaryVehicleCancellation : Localize.OrderSummaryVehicleRegistration),
                Items = new ListNotEmpty<GeneralEntity>
                        (
                            SummaryBeauftragungsHeaderKunde,

                            SummaryBeauftragungsHeader,

                            new GeneralEntity
                            {
                                Title = Localize.InvoiceData,
                                Body = Rechnungsdaten.GetSummaryString(Kunden),
                            },

                            new GeneralEntity
                            {
                                Title = Localize.VehicleData,
                                Body = Fahrzeugdaten.GetSummaryString(),
                            },

                            new GeneralEntity
                            {
                                Title = Localize.Holder,
                                Body = Halter.Adresse.GetPostLabelString(),
                            },

                            (Zulassungsdaten.ModusAbmeldung
                                    ? null :
                                    new GeneralEntity
                                    {
                                        Title = Localize.CarTaxPayer,
                                        Body = ZahlerKfzSteuer.GetSummaryString(),
                                    }),

                            new GeneralEntity
                            {
                                Title = (Zulassungsdaten.ModusAbmeldung ? Localize.Cancellation : Localize.Registration),
                                Body = Zulassungsdaten.GetSummaryString(),
                            },

                            (Zulassungsdaten.ModusAbmeldung 
                                    ? null :
                                    new GeneralEntity
                                    {
                                        Title = Localize.RegistrationOptions,
                                        Body = OptionenDienstleistungen.GetSummaryString(),
                                    }),

                            (BankAdressdaten.GetSummaryString().IsNullOrEmpty()
                                    ? null :
                                    new GeneralEntity
                                    {
                                        Title = Localize.DataForEndCustomerInvoice,
                                        Body = BankAdressdaten.GetSummaryString(),
                                    })
                        )
            };

            return summaryModel;
        }
    }
}
