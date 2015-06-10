using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<AuslieferAdresse> AuslieferAdressen { get; set; }

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

        public static List<SelectItem> AuslieferAdressenPartnerRollen
        {
            get
            {
                return new List<SelectItem>
                    {
                        new SelectItem("Z7", Localize.DeliveryAddress + " 1"),
                        new SelectItem("Z8", Localize.DeliveryAddress + " 2"),
                        new SelectItem("Z9", Localize.DeliveryAddress + " 3")
                    };
            }
        } 

        public Vorgang()
        {
            Rechnungsdaten = new Rechnungsdaten();
            BankAdressdaten = new BankAdressdaten("RE", true);
            AuslieferAdressen = new List<AuslieferAdresse>();
            AuslieferAdressenPartnerRollen.ForEach(p => AuslieferAdressen.Add(new AuslieferAdresse(p.Key)));
            AuslieferAdressen.ForEach(a => a.Materialien = AuslieferAdresse.AlleMaterialien);
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
        string AuslieferAdressenSummaryString
        {
            get
            {
                var s = "";

                if (AuslieferAdressen.None(a => a.ZugeordneteMaterialien.Any()))
                    return s;

                foreach (var item in AuslieferAdressen.Where(a => a.ZugeordneteMaterialien.Any()))
                {
                    if (s.IsNotNullOrEmpty())
                        s += "<br/><br/>";

                    s += String.Format("<b>{0}:</b>", String.Join(";", item.ZugeordneteMaterialien));

                    s += "<br/>" + item.Adressdaten.Adresse.GetPostLabelString();
                }

                return s;
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

        public void RefreshAuslieferAdressenMaterialAuswahl()
        {
            foreach (var item in AuslieferAdressen)
            {
                item.Materialien = AuslieferAdresse.AlleMaterialien.Where(m => AuslieferAdressen.None(a => a.Adressdaten.Partnerrolle != item.Adressdaten.Partnerrolle && a.ZugeordneteMaterialien.Contains(m.Key))).ToList();
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
                                    }),

                            (Zulassungsdaten.ModusAbmeldung || AuslieferAdressenSummaryString.IsNullOrEmpty()
                                    ? null :
                                    new GeneralEntity
                                    {
                                        Title = Localize.DeliveryAddresses,
                                        Body = AuslieferAdressenSummaryString,
                                    })
                        )
            };

            return summaryModel;
        }
    }
}
