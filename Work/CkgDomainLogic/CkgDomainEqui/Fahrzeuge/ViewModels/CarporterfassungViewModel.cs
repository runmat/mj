using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using CkgDomainLogic.UPSShip;
using DocumentTools.Services;
using GeneralTools.Models;
using GeneralTools.Services;
using SapORM.Contracts;

namespace CkgDomainLogic.Fahrzeuge.ViewModels
{
    public class CarporterfassungViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public ICarporterfassungDataService DataService { get { return CacheGet<ICarporterfassungDataService>(); } }

        public CarporterfassungModel AktuellesFahrzeug { get; set; }

        [XmlIgnore]
        public List<CarporterfassungModel> Fahrzeuge { get; set; }

        [XmlIgnore]
        public List<CarporterfassungModel> FahrzeugeFiltered
        {
            get { return PropertyCacheGet(() => Fahrzeuge); }
            protected set { PropertyCacheSet(value); }
        }

        public bool EditMode { get; set; }

        public void Init()
        {
            DataMarkForRefresh();
            LoadFahrzeugModel();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.FahrzeugeFiltered);
        }

        public void LoadFahrzeugModel(string kennzeichen = null)
        {
            EditMode = true;

            if (kennzeichen != null)
            {
                AktuellesFahrzeug = Fahrzeuge.FirstOrDefault(f => f.Kennzeichen == kennzeichen);
                if (AktuellesFahrzeug != null)
                    return;
            }            

            AktuellesFahrzeug = new CarporterfassungModel
                {
                    CarportId = LogonContext.User.Reference,
                    KundenNr = LogonContext.KundenNr.ToSapKunnr(),
                    DemontageDatum = DateTime.Today
                };
        }

        public string DeleteFahrzeugModel(string kennzeichen)
        {
            var fzg = Fahrzeuge.FirstOrDefault(f => f.Kennzeichen == kennzeichen);
            if (fzg == null)
                return "";

            Fahrzeuge.Remove(fzg);

            return fzg.ObjectKey;
        }

        public void LoadFahrzeugdaten(string kennzeichen, string bestandsnummer, string fin)
        {
            AktuellesFahrzeug = DataService.LoadFahrzeugdaten(kennzeichen.NotNullOrEmpty().ToUpper(), bestandsnummer, fin.NotNullOrEmpty().ToUpper());

            if (AktuellesFahrzeug != null && Fahrzeuge.Any(f => f.Kennzeichen == AktuellesFahrzeug.Kennzeichen))
                AktuellesFahrzeug = new CarporterfassungModel { TmpStatus = "VEHICLE_ALREADY_EXISTS" };
        }

        public void AddFahrzeug(CarporterfassungModel item)
        {
            item.Kennzeichen = item.Kennzeichen.NotNullOrEmpty().ToUpper();
            item.FahrgestellNr = item.FahrgestellNr.NotNullOrEmpty().ToUpper();

            // Nur einen Datensatz zu einem Kennzeichen zulassen
            if (Fahrzeuge.Any(f => f.Kennzeichen == item.Kennzeichen))
                Fahrzeuge.RemoveAll(f => f.Kennzeichen == item.Kennzeichen);

            Fahrzeuge.Add(item);
            DataMarkForRefresh();
        }

        public void RemoveFahrzeug(CarporterfassungModel item)
        {
            Fahrzeuge.Remove(item);
            DataMarkForRefresh();
        }

        public void SaveFahrzeuge()
        {
            EditMode = false;

            var objectKeyDict = Fahrzeuge.ToDictionary(t => t.Kennzeichen, t => t.ObjectKey);
            Fahrzeuge = DataService.SaveFahrzeuge(Fahrzeuge);
            // restore shopping cart ID's
            Fahrzeuge.ForEach(f => f.ObjectKey = objectKeyDict[f.Kennzeichen]);

            DataMarkForRefresh();
        }

        public void ClearList()
        {
            Fahrzeuge.RemoveAll(f => String.IsNullOrEmpty(f.Status));
            DataMarkForRefresh();
        }

        public byte[] GetLieferschein()
        {
            var tblLieferschein = new DataTable("Lieferschein");
            tblLieferschein.Columns.Add("Nr");
            tblLieferschein.Columns.Add("Kennzeichen");
            tblLieferschein.Columns.Add("Fahrgestellnummer");
            tblLieferschein.Columns.Add("Hersteller");
            tblLieferschein.Columns.Add("Demontagedatum");
            tblLieferschein.Columns.Add("Vorlage ZBI");
            tblLieferschein.Columns.Add("Anzahl Kennzeichen");
            tblLieferschein.Columns.Add("Web User");
            tblLieferschein.Columns.Add("Carport ID");
            tblLieferschein.Columns.Add("Erfassungsdatum");
            tblLieferschein.Columns.Add("Bestandsnummer");
            tblLieferschein.Columns.Add("Auftragsnummer");
            tblLieferschein.AcceptChanges();

            var tblKopf = new DataTable("Kopf");
            tblKopf.Columns.Add("CarportID");
            tblKopf.Columns.Add("Name1");
            tblKopf.Columns.Add("Name2");
            tblKopf.Columns.Add("LieferscheinNummer");
            tblKopf.Columns.Add("Kundenname");
            tblKopf.AcceptChanges();

            var nr = 1;
            var lieferscheinNr = "";
            foreach (var fzg in Fahrzeuge.Where(f => String.IsNullOrEmpty(f.Status)).OrderBy(f => f.Kennzeichen).ToList())
            {
                if (nr == 1)
                {
                    var newKopfRow = tblKopf.NewRow();
                    newKopfRow["CarportID"] = fzg.CarportId;
                    newKopfRow["Name1"] = LogonContext.User.LastName;
                    newKopfRow["Name2"] = LogonContext.User.FirstName;
                    newKopfRow["LieferscheinNummer"] = fzg.LieferscheinNr;
                    lieferscheinNr = fzg.LieferscheinNr;
                    newKopfRow["Kundenname"] = LogonContext.CustomerName;
                    tblKopf.Rows.Add(newKopfRow);
                }

                var newRow = tblLieferschein.NewRow();
                newRow["Nr"] = nr;
                newRow["Kennzeichen"] = fzg.Kennzeichen;
                newRow["Fahrgestellnummer"] = fzg.FahrgestellNr;
                newRow["Hersteller"] = "";
                newRow["Demontagedatum"] = fzg.DemontageDatum.ToString("dd.MM.yyyy");
                newRow["Vorlage ZBI"] = "";
                newRow["Anzahl Kennzeichen"] = fzg.AnzahlKennzeichen;
                newRow["Web User"] = LogonContext.UserName;
                newRow["Carport ID"] = fzg.CarportId;
                newRow["Erfassungsdatum"] = DateTime.Now.ToShortDateString();
                newRow["Bestandsnummer"] = fzg.MvaNr;
                newRow["Auftragsnummer"] = fzg.AuftragsNr;
                tblLieferschein.Rows.Add(newRow);
                nr++;
            }

            var imageHt = new Hashtable();
            var ms = BarcodeService.CreateBarcode(lieferscheinNr);
            imageHt.Add("Logo3", ms);

            var docFactory = new WordDocumentFactory(tblLieferschein, imageHt);

            return docFactory.CreateDocumentAndReturnBytes(Localize.Fahrzeuge_Carporterfassung, Path.Combine(AppSettings.RootPath, @"Documents\Templates\Bestellung.doc"), tblKopf);
        }

        public string GenerateUpsShippingOrderHtml()
        {
            var adresseDad = DataService.GetCarportInfo("DAD");
            var adresseCarport = DataService.GetCarportInfo(LogonContext.User.Reference);

            if (adresseDad == null || adresseCarport == null)
                return Localize.NoAddressTypesAvailableForThisCustomer;

            var username = GeneralConfiguration.GetConfigValue("UpsShippingWebService", "Username");
            var password = GeneralConfiguration.GetConfigValue("UpsShippingWebService", "Password");
            var accessKey = GeneralConfiguration.GetConfigValue("UpsShippingWebService", "AccessKey");

            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password) || String.IsNullOrEmpty(accessKey))
                return Localize.NoAccessDataFoundInDatabase;

            try
            {
                var securityToken = new UPSSecurity
                    {
                        UsernameToken = new UPSSecurityUsernameToken {Username = username, Password = password},
                        ServiceAccessToken = new UPSSecurityServiceAccessToken {AccessLicenseNumber = accessKey}
                    };

                var shipmentCharge = new ShipmentChargeType
                    {
                        BillShipper = new BillShipperType {AccountNumber = adresseDad.KundenNr},
                        Type = "01"
                    };

                var paymentInfo = new PaymentInfoType {ShipmentCharge = new[] {shipmentCharge}};

                var shipperAddress = new ShipAddressType
                    {
                        AddressLine = new[] {adresseDad.StrasseHausnummer},
                        City = adresseDad.Ort,
                        PostalCode = adresseDad.Plz,
                        CountryCode = adresseDad.Land
                    };

                var shipper = new ShipperType
                    {
                        ShipperNumber = adresseDad.KundenNr,
                        Address = shipperAddress,
                        Name = adresseDad.Name1,
                        AttentionName = adresseDad.Name2,
                        Phone = new ShipPhoneType { Number = adresseDad.Telefon }
                    };

                var shipToAddress = new ShipToAddressType
                    {
                        AddressLine = new[] { adresseDad.StrasseHausnummer },
                        City = adresseDad.Ort,
                        PostalCode = adresseDad.Plz,
                        CountryCode = adresseDad.Land
                    };

                var shipTo = new ShipToType
                    {
                        Address = shipToAddress,
                        Name = adresseDad.Name1,
                        AttentionName = adresseDad.Name2,
                        Phone = new ShipPhoneType { Number = adresseDad.Telefon }
                    };

                var shipFromAddress = new ShipAddressType
                {
                    AddressLine = new[] { adresseCarport.StrasseHausnummer },
                    City = adresseCarport.Ort,
                    PostalCode = adresseCarport.Plz,
                    CountryCode = adresseCarport.Land
                };

                var shipFrom = new ShipFromType
                {
                    Address = shipFromAddress,
                    Name = adresseCarport.Name1,
                    AttentionName = adresseCarport.Name2,
                    Phone = new ShipPhoneType { Number = adresseCarport.Telefon }
                };

                var refNumbers = new[]
                    {
                        new ReferenceNumberType {Code = "PO", Value = Fahrzeuge.First().LieferscheinNr},
                        new ReferenceNumberType {Code = "DP", Value = adresseCarport.CarportId}
                    };

                var package = new PackageType
                    {
                        Packaging = new PackagingType {Code = "02", Description = "Package"},
                        PackageWeight = new PackageWeightType
                            {
                                UnitOfMeasurement = new ShipUnitOfMeasurementType {Code = "KGS", Description = "KG"},
                                Weight = "10"
                            }
                    };

                var shipment = new ShipmentType
                    {
                        Description = "ShipmentRequest",
                        PaymentInformation = paymentInfo,
                        Shipper = shipper,
                        ShipTo = shipTo,
                        ShipFrom = shipFrom,
                        ReferenceNumber = refNumbers,
                        Service = new ServiceType {Code = "11", Description = "UPS Standard"},
                        Package = new[] {package}
                    };

                var shipmentRequest = new ShipmentRequest
                    {
                        Request = new RequestType {RequestOption = new[] {"nonvalidate"}},
                        Shipment = shipment,
                        LabelSpecification =
                            new LabelSpecificationType {LabelImageFormat = new LabelImageFormatType {Code = "GIF"}}
                    };

                var shipService = new ShipService {UPSSecurityValue = securityToken};

                // ReSharper disable CSharpWarnings::CS0612
                System.Net.ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy();
                // ReSharper restore CSharpWarnings::CS0612

                var shipmentResponse = shipService.ProcessShipment(shipmentRequest);

                var result = shipmentResponse.ShipmentResults.PackageResults.First();

                var gifHexString = result.ShippingLabel.GraphicImage;
                var htmlBytes = Convert.FromBase64String(result.ShippingLabel.HTMLImage);

                var htmlString = Encoding.Default.GetString(htmlBytes);

                var strImgPattern = "<IMG SRC=\"[^\"]*?\"";
                var strImgReplace = String.Format("<IMG SRC=\"data:image/gif;base64,{0}\"", gifHexString);

                htmlString = Regex.Replace(htmlString, strImgPattern, strImgReplace);

                return htmlString;
            }
            catch (System.Web.Services.Protocols.SoapException soapEx)
            {
                return String.Format("{0}: {1} -> {2}", Localize.Error, soapEx.Message, soapEx.Detail.InnerText);
            }
            catch (Exception ex)
            {
                return String.Format("{0}: {1}", Localize.Error, ex.Message);
            }
        }

        public void FilterFahrzeuge(string filterValue, string filterProperties)
        {
            FahrzeugeFiltered = Fahrzeuge.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }
    }
}
