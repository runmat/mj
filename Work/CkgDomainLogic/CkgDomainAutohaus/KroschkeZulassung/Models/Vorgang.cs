using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.KroschkeZulassung.Models
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

        public Adresse Halterdaten { get; set; }

        public string Halter
        {
            get
            {
                if (Halterdaten != null)
                    return String.Format("{0} {1}", Halterdaten.Name1, Halterdaten.Name2);

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
            BankAdressdaten = new BankAdressdaten();
            Fahrzeugdaten = new Fahrzeugdaten { FahrzeugartId = "1" };
            Halterdaten = new Adresse { Land = "DE", Kennung = "HALTER" };
            Zulassungsdaten = new Zulassungsdaten();
            OptionenDienstleistungen = new OptionenDienstleistungen();
        }


        [XmlIgnore, ScriptIgnore]
        public string BeauftragungBezeichnungKunde
        {
            get
            {
                return String.Format("{0}: {1}, {2}, {3}, {4}",
                    Fahrzeugdaten.AuftragsNr,
                    Rechnungsdaten.Kunde.KundenNameNr,
                    Zulassungsdaten.Zulassungsart.MaterialText,
                    Halter,
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
                Header = Localize.OrderSummaryVehicleRegistration,
                Items = new ListNotEmpty<GeneralEntity>
                        (
                            SummaryBeauftragungsHeaderKunde,

                            SummaryBeauftragungsHeader,

                            new GeneralEntity
                            {
                                Title = Localize.InvoiceData,
                                Body = Rechnungsdaten.GetSummaryString(),
                            },

                            new GeneralEntity
                            {
                                Title = Localize.VehicleData,
                                Body = Fahrzeugdaten.GetSummaryString(),
                            },

                            new GeneralEntity
                            {
                                Title = Localize.Holder,
                                Body = Halterdaten.GetPostLabelString(),
                            },

                            new GeneralEntity
                            {
                                Title = Localize.Registration,
                                Body = Zulassungsdaten.GetSummaryString(),
                            },

                            new GeneralEntity
                            {
                                Title = Localize.RegistrationOptions,
                                Body = OptionenDienstleistungen.GetSummaryString(),
                            },

                            new GeneralEntity
                            {
                                Title = Localize.DataForEndCustomerInvoice,
                                Body = BankAdressdaten.GetSummaryString(),
                            }
                        )
            };

            return summaryModel;
        }
    }
}
