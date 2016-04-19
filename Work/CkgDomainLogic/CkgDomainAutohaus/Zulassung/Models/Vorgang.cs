using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using GeneralTools.Models;
using GeneralTools.Resources;
using Localize = CkgDomainLogic.General.Services.Localize;

namespace CkgDomainLogic.Autohaus.Models
{
    public class Vorgang
    {
        private List<PdfFormular> _zusatzformulare = new List<PdfFormular>();

        [LocalizedDisplay(LocalizeConstants.ReceiptNo)]
        public string BelegNr { get; set; }

        [LocalizedDisplay(LocalizeConstants._Auftragsart)]
        public string BeauftragungsArt { get; set; }

        public string WebGroupId { get; set; }

        public string WebUserId { get; set; }

        public string ZulassungFromShoppingCartParameters
        {
            get
            {
                if (BeauftragungsArt == "VERSANDZULASSUNG")
                    return "?versandzulassung=1";

                if (BeauftragungsArt == "SONDERZULASSUNG")
                    return "?sonderzulassung=1";

                if (BeauftragungsArt == "SCHNELLABMELDUNG")
                    return "?schnellabmeldung=1";

                if (BeauftragungsArt == "ABMELDUNG" || BeauftragungsArt == "MASSENABMELDUNG")
                    return "?abmeldung=1";

                if (BeauftragungsArt == "VERSANDZULASSUNGPARTNER")
                    return "?versandzulassung=1&partnerportal=1";

                if (BeauftragungsArt.StartsWith("SONDERZULASSUNG"))
                {
                    var sonderzulassungMode = !BeauftragungsArt.Contains("_") ? "" : BeauftragungsArt.Split('_')[1].ToLower();   // z. B. SONDERZULASSUNG_ERSATZKENNZEICHEN
                    return "?sonderzulassung=1&sonderzulassungMode=" + sonderzulassungMode;
                }

                return "";
            }
        }

        public string VkOrg { get; set; }

        public string VkBur { get; set; }

        public string Vorerfasser { get; set; }

        public string Aenderer { get; set; }

        public DateTime? ErfassungsDatum { get; set; }

        public string ErfassungsZeit { get; set; }

        public string VorgangsStatus { get; set; }

        public Rechnungsdaten Rechnungsdaten { get; set; }

        public BankAdressdaten BankAdressdaten { get; set; }

        public List<AuslieferAdresse> AuslieferAdressen { get; set; }

        public Fahrzeugdaten Fahrzeugdaten { get; set; }

        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string FahrgestellNr {get { return Fahrzeugdaten.FahrgestellNr; } }

        [LocalizedDisplay(LocalizeConstants.EvbNumber)]
        public string EvbNr { get { return Zulassungsdaten.EvbNr; } }

        public Adressdaten Halter { get; set; }

        public BankAdressdaten ZahlerKfzSteuer { get; set; }

        public Adressdaten VersandAdresse { get; set; }

        public bool Ist48hZulassung { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeliveryTimeBy)]
        public string LieferuhrzeitBis { get; set; }

        [XmlIgnore]
        public string LieferuhrzeitBisFormatted
        {
            get
            {
                var tmpZeit = LieferuhrzeitBis;

                if (!String.IsNullOrEmpty(tmpZeit) && tmpZeit.Length == 6)
                    tmpZeit = tmpZeit.Substring(0, 2) + ":" + tmpZeit.Substring(2, 2) + ":" + tmpZeit.Substring(4, 2);

                return tmpZeit;
            }
        }

        public List<Kunde> Kunden { get; set; }

        public string HalterName
        {
            get
            {
                if (Zulassungsdaten.IsSchnellabmeldung)
                    return Zulassungsdaten.HalterNameSchnellabmeldung;

                if (Halter != null)
                    return Halter.Name;

                return "";
            }
        }

        public bool HalterGewerblich { get { return (Halter != null && Halter.Adresse != null && Halter.Adresse.Gewerblich); } }

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

        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        public DateTime? Zulassungsdatum { get { return (BeauftragungsArt.NotNullOrEmpty().ToUpper().Contains("ABMELDUNG") ? null : Zulassungsdaten.Zulassungsdatum); } }

        [LocalizedDisplay(LocalizeConstants.CancellationDate)]
        public DateTime? Abmeldedatum { get { return (BeauftragungsArt.NotNullOrEmpty().ToUpper().Contains("ABMELDUNG") ? Zulassungsdaten.Zulassungsdatum : null); } }

        public OptionenDienstleistungen OptionenDienstleistungen { get; set; }

        [XmlIgnore]
        public List<PdfFormular> Zusatzformulare
        {
            get { return _zusatzformulare; }
            set { _zusatzformulare = value; }
        }

        [XmlIgnore]
        public byte[] KundenformularPdf { get; set; }

        [XmlIgnore]
        public byte[] VersandlabelPdf { get; set; }

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

        [XmlIgnore]
        public List<SelectItem> KennzeichenTypen
        {
            get
            {
                return new List<SelectItem>
                {
                    new SelectItem {Key = "8", Text = "Kennzeichen vorne"},
                    new SelectItem {Key = "801", Text = "Kennzeichen hinten"},
                    new SelectItem {Key = "800", Text = "Kennzeichen vorn und hinten"},
                };
            }
        }

        public Versanddaten Versanddaten { get; set; }

        public bool IsSelected { get; set; }

        public Vorgang()
        {
            Rechnungsdaten = new Rechnungsdaten();
            BankAdressdaten = new BankAdressdaten("RE", true);
            AuslieferAdressen = new List<AuslieferAdresse>();
            AuslieferAdressenPartnerRollen.ForEach(p => AuslieferAdressen.Add(new AuslieferAdresse(p.Key)));
            AuslieferAdressen.ForEach(a => a.Materialien = AuslieferAdresse.AlleMaterialien);
            Fahrzeugdaten = new Fahrzeugdaten
            {
                FahrzeugartId = "1"
            };
            Halter = new Adressdaten("HALTER") { Partnerrolle = "ZH"};
            ZahlerKfzSteuer = new BankAdressdaten("Z6", false, "ZAHLERKFZSTEUER");
            VersandAdresse = new Adressdaten("") { Partnerrolle = "ZZ" };
            Zulassungsdaten = new Zulassungsdaten();
            OptionenDienstleistungen = new OptionenDienstleistungen();
            Versanddaten = new Versanddaten();
        }

        [XmlIgnore, ScriptIgnore]
        string SummaryHeaderText
        {
            get
            {
                if (Zulassungsdaten.ModusAbmeldung)
                    return Localize.OrderSummaryVehicleCancellation;

                if (Zulassungsdaten.ModusVersandzulassung)
                    return Localize.OrderSummaryVehicleMailOrderRegistration;

                if (Zulassungsdaten.ModusSonderzulassung)
                    return Localize.OrderSummaryVehicleSpecialRegistration;

                return Localize.OrderSummaryVehicleRegistration;
            }
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
        string VersandAdresseSummaryString
        {
            get
            {
                var s = "";

                s += VersandAdresse.Adresse.GetPostLabelString();

                if (Ist48hZulassung)
                {
                    s += "<br/>" + Localize.ExpressRegistration48h;

                    if (!String.IsNullOrEmpty(LieferuhrzeitBis))
                        s += String.Format("<br/>{0}: {1} Uhr", Localize.DeliveryTimeBy, LieferuhrzeitBisFormatted);
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
                var auslieferAdresse = item;

                item.Materialien = AuslieferAdresse.AlleMaterialien.Where(m => AuslieferAdressen.None(a => a.Adressdaten.Partnerrolle != auslieferAdresse.Adressdaten.Partnerrolle && a.ZugeordneteMaterialien.Contains(m.Key))).ToList();
            }
        }

        public GeneralSummary CreateSummaryModel(string auslieferAdressenLink, string[] stepKeys)
        {
            var keysToLower = stepKeys.Select(s => s.ToLower());

            var summaryModel = new GeneralSummary
            {
                Header = SummaryHeaderText,
                Items = new ListNotEmpty<GeneralEntity>
                        (
                            SummaryBeauftragungsHeaderKunde,

                            SummaryBeauftragungsHeader,

                            new GeneralEntity
                            {
                                Title = Localize.InvoiceData,
                                Body = Rechnungsdaten.GetSummaryString(Kunden),
                            },

                            (!keysToLower.Contains("fahrzeugdaten")
                                    ? null :
                                    new GeneralEntity
                                    {
                                        Title = Localize.VehicleData,
                                        Body = Fahrzeugdaten.GetSummaryString()
                                    }),

                            (!keysToLower.Contains("halter")
                                    ? null :
                                    new GeneralEntity
                                    {
                                        Title = Localize.Holder,
                                        Body = Halter.Adresse.GetPostLabelString()
                                    }),

                            (!keysToLower.Contains("zahlerkfzsteuer")
                                    ? null :
                                    new GeneralEntity
                                    {
                                        Title = Localize.CarTaxPayer,
                                        Body = ZahlerKfzSteuer.GetSummaryString()
                                    }),

                            (!keysToLower.Contains("zulassungsdaten")
                                    ? null :
                                    new GeneralEntity
                                    {
                                        Title = (Zulassungsdaten.ModusAbmeldung ? Localize.Cancellation : Localize.Registration),
                                        Body = Zulassungsdaten.GetSummaryString()
                                    }),

                            (!keysToLower.Contains("optionendienstleistungen")
                                    ? null :
                                    new GeneralEntity
                                    {
                                        Title = Localize.RegistrationOptions,
                                        Body = OptionenDienstleistungen.GetSummaryString()
                                    }),

                            (!keysToLower.Contains("versanddaten")
                                    ? null :
                                    new GeneralEntity
                                    {
                                        Title = Localize.ShippingAddress,
                                        Body = VersandAdresseSummaryString
                                    }),

                            (BankAdressdaten.GetSummaryString().IsNullOrEmpty()
                                    ? null :
                                    new GeneralEntity
                                    {
                                        Title = Localize.DataForEndCustomerInvoice,
                                        Body = BankAdressdaten.GetSummaryString()
                                    }),

                            (Zulassungsdaten.ModusAbmeldung
                                    ? null :
                                    new GeneralEntity
                                    {
                                        Title = Localize.DeliveryAddresses,
                                        Body = AuslieferAdressenSummaryString + auslieferAdressenLink
                                    }),

                            (!keysToLower.Contains("versanddaten")
                                    ? null :
                                    new GeneralEntity
                                    {
                                        Title = Localize.Shipping,
                                        Body = Versanddaten.GetSummaryString()
                                    })
                        )
            };

            return summaryModel;
        }
    }
}
