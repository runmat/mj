using System;
using System.Web.Services;
using UPSVersandWrapperService.UPSShip;

namespace UPSVersandWrapperService
{
    [WebService(Namespace = "http://kroschke.de/")]
    public class ServiceData : WebService
    {
        [WebMethod]
        public ShipmentResponse UPS_ProcessShipment(ShipmentRequest ShipmentRequest)
        {
            try
            {
                var securityToken = new UPSSecurity
                {
                    UsernameToken = new UPSSecurityUsernameToken { Username = Common.UpsUsername, Password = Common.UpsPassword },
                    ServiceAccessToken = new UPSSecurityServiceAccessToken { AccessLicenseNumber = Common.UpsAccessKey }
                };

                var service = new ShipService { Url = Common.UpsUrl, UPSSecurityValue = securityToken };

                return service.ProcessShipment(ShipmentRequest);
            }
            catch (System.Web.Services.Protocols.SoapException soapEx)
            {
                throw new Exception(String.Format("UPS_ProcessShipment, Fehler: {0} -> {1}", soapEx.Message, soapEx.Detail.InnerText));
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("UPS_ProcessShipment, Fehler: {0}", ex.Message));
            }
        }

        #region Test

        [WebMethod]
        public ShipmentResponse UPS_ProcessShipmentTest()
        {
            try
            {
                var securityToken = new UPSSecurity
                {
                    UsernameToken = new UPSSecurityUsernameToken { Username = Common.UpsUsername, Password = Common.UpsPassword },
                    ServiceAccessToken = new UPSSecurityServiceAccessToken { AccessLicenseNumber = Common.UpsAccessKey }
                };

                var service = new ShipService { Url = Common.UpsUrl, UPSSecurityValue = securityToken };

                var req = createTestShipmentRequest();

                return service.ProcessShipment(req);
            }
            catch (System.Web.Services.Protocols.SoapException soapEx)
            {
                throw new Exception(String.Format("UPS_ProcessShipmentTest, Fehler: {0} -> {1}", soapEx.Message, soapEx.Detail.InnerText));
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("UPS_ProcessShipmentTest, Fehler: {0}", ex.Message));
            }
        }

        private ShipmentRequest createTestShipmentRequest()
        {
            var shipmentCharge = new ShipmentChargeType
            {
                BillShipper = new BillShipperType { AccountNumber = "Y018X0" },
                Type = "01"
            };

            var paymentInfo = new PaymentInfoType { ShipmentCharge = new[] { shipmentCharge } };

            var shipperAddress = new ShipAddressType
            {
                AddressLine = new[] { "LADESTRASSE 1" },
                City = "AHRENSBURG",
                PostalCode = "22926",
                CountryCode = "DE"
            };

            var shipper = new ShipperType
            {
                ShipperNumber = "Y018X0",
                Address = shipperAddress,
                Name = "KROSCHKE GMBH",
                AttentionName = "Versand",
                Phone = new ShipPhoneType { Number = "04102 804-0" }
            };

            var shipToAddress = new ShipToAddressType
            {
                AddressLine = new[] { "Strasse mit Nummer" },
                City = "Berlin",
                PostalCode = "10100",
                CountryCode = "DE"
            };

            var shipTo = new ShipToType
            {
                Address = shipToAddress,
                Name = "Zustelladresse",
                AttentionName = "Kontaktperson, wenn leer = .",
                Phone = new ShipPhoneType { Number = "Tel, leer=000" }
            };

            var refNumbers = new[]
                    {
                        new ReferenceNumberType {Code = "DP", Value = "ID:121212" },
                        new ReferenceNumberType {Code = "EI", Value = "ID:2525" }
                    };

            var package = new PackageType
            {
                Description = "Warenbeschreibung",
                Packaging = new PackagingType { Code = "02", Description = "Package" },
                PackageWeight = new PackageWeightType
                {
                    UnitOfMeasurement = new ShipUnitOfMeasurementType { Code = "KGS", Description = "KG" },
                    Weight = "2"
                }
            };

            var shipment = new ShipmentType
            {
                Description = "ShipmentRequest",
                PaymentInformation = paymentInfo,
                Shipper = shipper,
                ShipTo = shipTo,
                ReferenceNumber = refNumbers,
                Service = new ServiceType { Code = "11", Description = "UPS Standard" },
                Package = new[] { package }
            };

            return new ShipmentRequest
            {
                Request = new RequestType { RequestOption = new[] { "nonvalidate" } },
                Shipment = shipment,
                LabelSpecification = new LabelSpecificationType { LabelImageFormat = new LabelImageFormatType { Code = "GIF" } }
            };
        }

        #endregion
    }
}
