using System;
using System.Linq;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using GeneralTools.Contracts;
using Mobile.Entities;
using Mobile.Interfaces;
using Mobile.Resources;
using Mobile.Rest;
using RestSharp;

namespace AutoAct.Rest
{
    public class MobileRest : IMobileRest
    {
        #region Privates and Constructor

        private readonly ILogService _logService;
        private readonly IRestClient _client;
        private readonly ISettingsReader _settingsReader;
        private readonly CustomConverter _serializer = new CustomConverter { ContentType = "application/json" };
        const string Vehicle = "vehicle";
        const string Price = "price";
        const string Sitespecifics = "site-specifics";
        const string Nsxsi = "http://www.w3.org/2001/XMLSchema-instance";
        const string Ns = "http://services.mobile.de/schema/seller/seller-ad-1.1";
        const string Nsvehicle = "http://services.mobile.de/schema/seller/vehicle-1.0";
        const string Nssitespecifics = "http://services.mobile.de/schema/seller/site-specifics-1.0";
        const string Nsprice = "http://services.mobile.de/schema/seller/price-1.0";
        const string XsischemaLocation = "http://services.mobile.de/schema/seller/seller-ad-1.1 http://services.mobile.de/schema/seller/seller-ad-1.1.xsd";

        public MobileRest(ILogService logService, IRestClient client, ISettingsReader settingsReader)
        {
            _logService = logService;
            _client = client;
            _settingsReader = settingsReader;
            _client.BaseUrl = _settingsReader.MobiledeUrl;
        }

        #endregion

        #region Pictures

        public Result<bool> PostImage(int inseratId, UploadFile[] files)
        {
            var uri = string.Format("/seller-api/sellers/{0}/ads/{1}/images", _settingsReader.SellerId, inseratId);

            var request = CreateRequest(uri, Method.PUT, "application/vnd.de.mobile.seller-ad-v1.1+xml");

            request.AddFile("image", files[0].Content, files[0].FileName, "image/jpeg");

            var response = _client.Execute<object>(request);

            return response.StatusCode == HttpStatusCode.OK
                       ? new Result<bool> {Value = true}
                       : new Result<bool> {Value = false};
        }

        #endregion

        #region ad

        /// <summary>
        /// Ermittelt alle eingestellte Inserate eines Kunden
        /// </summary>
        /// <returns></returns>
        public Result<ad> GetAdsOfSeller(string sellerid)
        {
            throw new NotImplementedException("ermittlung der Inserate eines Sellers aus SAP implementieren");
        }

        /// <summary>
        /// Ein Inserat wird zu mobile.de verschickt
        /// </summary>
        /// <param name="ad"></param>
        /// <returns></returns>
        public Result<object> PostAd(ad ad)
        {
            var uri = string.Format("/seller-api/sellers/{0}/ads", _settingsReader.SellerId);

            XmlDocument doc = new XmlDocument();

            XmlElement adDoc = doc.CreateElement("ad");
            doc.AppendChild(adDoc);

            adDoc.SetAttribute("xmlns:xsi", Nsxsi);
            adDoc.SetAttribute("schemaLocation", Nsxsi, XsischemaLocation);
            adDoc.SetAttribute("xmlns", Ns);
            adDoc.SetAttribute("xmlns:vehicle", Nsvehicle);
            adDoc.SetAttribute("xmlns:site-specifics", Nssitespecifics);
            adDoc.SetAttribute("xmlns:price", Nsprice);

            XmlElement sellerInventoryKey = doc.CreateElement("seller-inventory-key");
            sellerInventoryKey.SetAttribute("value", ad.SellerInventoryKeyValue);
            adDoc.AppendChild(sellerInventoryKey);

            XmlElement description = doc.CreateElement("description");
            description.InnerText = ad.DescriptionValue;
            adDoc.AppendChild(description);

            // <visibilities><visibility>RESERVED</visibility></visibilities>
            XmlElement visibilities = doc.CreateElement("visibilities");
            XmlElement visibility = doc.CreateElement("visibility");
            visibility.InnerText = ad.Visibility;
            visibilities.AppendChild(visibility);
            adDoc.AppendChild(visibilities);

            XmlElement vehicleVehicle = doc.CreateElement(Vehicle, Vehicle, Nsvehicle);
            adDoc.AppendChild(vehicleVehicle);

            // Specification
            XmlElement vehicleSpecification = doc.CreateElement(Vehicle, "classification", Nsvehicle);
            vehicleVehicle.AppendChild(vehicleSpecification);

            // <vehicle:vehicle-class key="Car" />
            XmlElement vehicleVehicleClass = doc.CreateElement(Vehicle, "vehicle-class", Nsvehicle);
            vehicleVehicleClass.SetAttribute("key", "Car");
            vehicleSpecification.AppendChild(vehicleVehicleClass);

            // <vehicle:category key="Limousine" />
            XmlElement vehicleCategory = doc.CreateElement(Vehicle, "category", Nsvehicle);
            vehicleCategory.SetAttribute("key", "Limousine");
            vehicleSpecification.AppendChild(vehicleCategory);

            // <vehicle:make key="Ford" />
            XmlElement vehicleMake = doc.CreateElement(Vehicle, "make", Nsvehicle);
            vehicleMake.SetAttribute("key", ad.VehicleMakeValue);
            vehicleSpecification.AppendChild(vehicleMake);

            // <vehicle:model key="Focus" />
            XmlElement vehicleModel = doc.CreateElement(Vehicle, "model", Nsvehicle);
            vehicleModel.SetAttribute("key", ad.VehicleModelValue);
            vehicleSpecification.AppendChild(vehicleModel);

            // <vehicle:model-description value="Fahrzeugbeschreibung" />
            XmlElement vehicleModelDescription = doc.CreateElement(Vehicle, "model-description", Nsvehicle);
            vehicleModelDescription.SetAttribute("value", ad.VehicleModelDescriptionValue);
            vehicleVehicle.AppendChild(vehicleModelDescription);

            // <vehicle:damage-and-unrepaired value="false" />
            XmlElement vehicleDamageAndUnrepaired = doc.CreateElement(Vehicle, "damage-and-unrepaired", Nsvehicle);
            vehicleDamageAndUnrepaired.SetAttribute("value", "false");
            vehicleVehicle.AppendChild(vehicleDamageAndUnrepaired);

            // <vehicle:accident-damaged value="false" />
            XmlElement vehicleAccidentDamaged = doc.CreateElement(Vehicle, "accident-damaged", Nsvehicle);
            vehicleAccidentDamaged.SetAttribute("value", "false");
            vehicleVehicle.AppendChild(vehicleAccidentDamaged);

            // <vehicle:roadworthy value="true" />
            XmlElement vehicleRoadworthy = doc.CreateElement(Vehicle, "roadworthy", Nsvehicle);
            vehicleRoadworthy.SetAttribute("value", "true");
            vehicleVehicle.AppendChild(vehicleRoadworthy);

            // specifis
            XmlElement vehicleSpecifics = doc.CreateElement(Vehicle, "specifics", Nsvehicle);
            vehicleVehicle.AppendChild(vehicleSpecifics);

            // <vehicle:mileage value="142000"/>
            XmlElement vehicleMileage = doc.CreateElement(Vehicle, "mileage", Nsvehicle);
            vehicleMileage.SetAttribute("value", ad.VehicleMileageValue);
            vehicleSpecifics.AppendChild(vehicleMileage);

            // site-specifics
            XmlElement vehicleSiteSpecifis = doc.CreateElement(Vehicle, "site-specifics", Nsvehicle);
            vehicleVehicle.AppendChild(vehicleSiteSpecifis);

            // <site-specifics:first-registration value="2013-09" />
            XmlElement vehicleFirstRegistration = doc.CreateElement(Sitespecifics, "first-registration", Nssitespecifics);
            vehicleFirstRegistration.SetAttribute("value", ad.VehicleFirstRegistrationValue);
            vehicleSiteSpecifis.AppendChild(vehicleFirstRegistration);

            // <site-specifics:kba hsn="8566" tsn="ABJ" />
            XmlElement vehicleKba = doc.CreateElement(Sitespecifics, "kba", Nssitespecifics);
            vehicleKba.SetAttribute("hsn", ad.VehicleKbaHsnValue);
            vehicleKba.SetAttribute("tsn", ad.VehicleKbaTsnValue);
            vehicleSiteSpecifis.AppendChild(vehicleKba);

            // Price

            XmlElement priceprice = doc.CreateElement(Price, Price, Nsprice);
            adDoc.AppendChild(priceprice);

            priceprice.SetAttribute("type", "FIXED");
            priceprice.SetAttribute("currency", "EUR");

            XmlElement priceGrossPrices = doc.CreateElement(Price, "gross-prices", Nsprice);
            priceprice.AppendChild(priceGrossPrices);

            XmlElement priceGrossPricesComnsumerPriceAmount = doc.CreateElement(Price, "consumer-price-amount", Nsprice);
            priceGrossPrices.AppendChild(priceGrossPricesComnsumerPriceAmount);
            priceGrossPricesComnsumerPriceAmount.SetAttribute("value", ad.PriceGrossPricesConsumerPriceAmountValue);


            var request = CreateRequestWithBody(uri, Method.POST, doc, "application/vnd.de.mobile.seller-ad-v1.1+xml");

            var response = _client.Execute<object>(request);

            var result = EvaluateResponse(response);

            // Inserat wurde erstellt
            if (response.StatusCode == HttpStatusCode.Created)
            {
                result.Value = response.Headers.Single(x => x.Name == "Location").Value.ToString();

                return result;
            }

            return result;
        }

        public Result<object> GetAd(string inseratId)
        {
            var uri = string.Format("/seller-api/sellers/{0}/ads/{1}", _settingsReader.SellerId, inseratId);

            var request = CreateRequest(uri, Method.GET, "application/vnd.de.mobile.seller-ad-v1.1+xml");

            var response = _client.Execute<object>(request);

            var result = EvaluateResponse(response);

            return result;
        }

        #endregion

        #region Privates

        /// <summary>
        /// Einheitliche Behandlung für Restoperation die einen  der Rückgabewerte aus einer  mit AutoAct
        /// AutoAct 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        private Result<T> EvaluateResponse<T>(IRestResponse<T> response)
        {
            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created || response.StatusCode == 0)
            {
                return new Result<T> { Value = response.Data };
            }

            var result = new Result<T> { Errors = _serializer.Deserialize<errors>(response) };

            if (result.Errors == null)
            {
                result.Errors = new errors{ error = new[]{new error{ key = response.StatusCode.ToString(), message = "Fehler bei einem Rest Aufruf"}}};
            }

            return result;
        }

        /// <summary>
        /// Request mit einem PayLoad in Body lassen sich nicht direkt durch RestSharp erzeugen
        /// - Verwende den CusotmConverter
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="method"></param>
        /// <param name="ns">Liste der gültigen Namespaces für diesen Request</param>
        /// <param name="body"></param>
        /// <param name="accept"></param>
        /// <returns></returns>
        private IRestRequest CreateRequestWithBody(string uri, Method method, XmlDocument body, string accept)
        {
            var request = CreateRequest(uri, method, accept);

            if (body == null)
            {
                return request;
            }

            request.AddHeader("Content-Type", accept);
            request.XmlSerializer = new RestSharp.Serializers.DotNetXmlSerializer();

            request.RequestFormat = DataFormat.Xml;
            
            request.AddParameter(accept, body.OuterXml , ParameterType.RequestBody);

            return request;
        }

        private IRestRequest CreateRequest(string uri, Method method, string accept)
        {
            IRestRequest request = new RestRequest(uri, method);
            request.Resource = uri;
            request.AddHeader("Accept-Language", ApplicationStrings.LanguageHeader);
            request.AddHeader("X-MOBILE-SELLER-TOKEN", _settingsReader.Token);
            if (string.IsNullOrEmpty(accept) == false)
            {
                request.AddHeader("Accept", accept);
            }

            return request;
        }

        #endregion


    }
}
