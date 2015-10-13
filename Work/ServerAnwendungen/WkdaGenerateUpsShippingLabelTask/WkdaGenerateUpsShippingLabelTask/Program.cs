using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GeneralTools.Services;
using NReco.PdfGenerator;
using SapORM.Models;
using WkdaGenerateUpsShippingLabelTask.UPSShip;

namespace WkdaGenerateUpsShippingLabelTask
{
    class Program
    {
        static void Main()
        {
            try
            {
                Console.WriteLine("Start...");

                Console.WriteLine("Lösche alte Dateien in " + Konfiguration.WkdaLabelAblagePfad + "...");

                if (!String.IsNullOrEmpty(Konfiguration.WkdaLabelAblagePfad))
                {
                    foreach (string filePath in Directory.GetFiles(Konfiguration.WkdaLabelAblagePfad, "*.pdf", SearchOption.AllDirectories))
                    {
                        FileService.TryFileDelete(filePath);
                    }
                }
                Console.WriteLine("Zum Beenden beliebige Taste drücken...");
                Console.ReadKey();
                Z_DPM_READ_ZDAD_AUFTR_006.Init(S.AP, "I_KUNNR, I_KENNUNG", Konfiguration.WkdaKunnr, "WKDA");

                var sapResults = Z_DPM_READ_ZDAD_AUFTR_006.GT_WEB.GetExportListWithExecute(S.AP);

                var dadAdresse = sapResults.FirstOrDefault(x => x.POS_KURZTEXT == "DAD");
                if (dadAdresse == null)
                {
                    Console.WriteLine("Fehler: DAD-Adresse nicht in SAP gefunden");
                    EventLog.WriteEntry("WkdaGenerateUpsShippingLabelTask", "Fehler: DAD-Adresse nicht in SAP gefunden", EventLogEntryType.Warning);
                    return;
                }

                foreach (var item in sapResults.Where(x => x.POS_KURZTEXT != "DAD"))
                {
                    Console.WriteLine("Generiere Versandlabel für Carport " + item.POS_KURZTEXT + "...");

                    var strLabelHtml = GenerateUpsShippingOrderHtml(dadAdresse, item);
                    var pdfBytes = ConvertHtmlToPdf(strLabelHtml);

                    var ablageVerzeichnis = Path.Combine(Konfiguration.WkdaLabelAblagePfad, item.POS_KURZTEXT);
                    if (!Directory.Exists(ablageVerzeichnis))
                        FileService.TryDirectoryCreate(ablageVerzeichnis);

                    File.WriteAllBytes(Path.Combine(ablageVerzeichnis, Konfiguration.WkdaLabelDateiname), pdfBytes);
                }

                Console.WriteLine("Fertig.");

                if (Konfiguration.pauseAfterCompletion)
                {
                    Console.WriteLine();
                    Console.WriteLine("Zum Beenden beliebige Taste drücken...");
                    Console.ReadKey();
                }  
            }
            catch (Exception ex)
            {
                Console.WriteLine("WkdaGenerateUpsShippingLabelTask - Fehler: " + ex.ToString());
                EventLog.WriteEntry("WkdaGenerateUpsShippingLabelTask", "Fehler: " + ex.ToString(), EventLogEntryType.Warning);
            }
        }

        private static string GenerateUpsShippingOrderHtml(Z_DPM_READ_ZDAD_AUFTR_006.GT_WEB dadAdresse, Z_DPM_READ_ZDAD_AUFTR_006.GT_WEB carportAdresse)
        {
            try
            {
                var securityToken = new UPSSecurity
                {
                    UsernameToken = new UPSSecurityUsernameToken { Username = Konfiguration.UpsShippingWebServiceUsername, Password = Konfiguration.UpsShippingWebServicePassword },
                    ServiceAccessToken = new UPSSecurityServiceAccessToken { AccessLicenseNumber = Konfiguration.UpsShippingWebServiceAccessKey }
                };

                var shipmentCharge = new ShipmentChargeType
                {
                    BillShipper = new BillShipperType { AccountNumber = carportAdresse.INTNR },
                    Type = "01"
                };

                var paymentInfo = new PaymentInfoType { ShipmentCharge = new[] { shipmentCharge } };

                var shipperAddress = new ShipAddressType
                {
                    AddressLine = new[] { carportAdresse.STRAS },
                    City = carportAdresse.ORT01,
                    PostalCode = carportAdresse.PSTLZ,
                    CountryCode = carportAdresse.LAND1
                };

                var shipper = new ShipperType
                {
                    ShipperNumber = carportAdresse.INTNR,
                    Address = shipperAddress,
                    Name = carportAdresse.NAME1,
                    AttentionName = carportAdresse.NAME2,
                    Phone = new ShipPhoneType { Number = carportAdresse.TELNR }
                };

                var shipToAddress = new ShipToAddressType
                {
                    AddressLine = new[] { dadAdresse.STRAS },
                    City = dadAdresse.ORT01,
                    PostalCode = dadAdresse.PSTLZ,
                    CountryCode = dadAdresse.LAND1
                };

                var shipTo = new ShipToType
                {
                    Address = shipToAddress,
                    Name = dadAdresse.NAME1,
                    AttentionName = dadAdresse.NAME2,
                    Phone = new ShipPhoneType { Number = dadAdresse.TELNR }
                };

                var refNumbers = new[]
                    {
                        new ReferenceNumberType {Code = "DP", Value = carportAdresse.POS_KURZTEXT}
                    };

                var package = new PackageType
                {
                    Packaging = new PackagingType { Code = "02", Description = "Package" },
                    PackageWeight = new PackageWeightType
                    {
                        UnitOfMeasurement = new ShipUnitOfMeasurementType { Code = "KGS", Description = "KG" },
                        Weight = "10"
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

                var shipmentRequest = new ShipmentRequest
                {
                    Request = new RequestType { RequestOption = new[] { "nonvalidate" } },
                    Shipment = shipment,
                    LabelSpecification =
                        new LabelSpecificationType { LabelImageFormat = new LabelImageFormatType { Code = "GIF" } }
                };

                var shipService = new ShipService { Url = Konfiguration.UpsShippingWebServiceUrl, UPSSecurityValue = securityToken };

#pragma warning disable 618
                System.Net.ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy();
#pragma warning restore 618

                var shipmentResponse = shipService.ProcessShipment(shipmentRequest);

                var result = shipmentResponse.ShipmentResults.PackageResults.First();

                var gifHexString = result.ShippingLabel.GraphicImage;
                var htmlBytes = Convert.FromBase64String(result.ShippingLabel.HTMLImage);

                var htmlString = Encoding.Default.GetString(htmlBytes);

                var strImgPattern = "<IMG SRC=\"[^\"]*?\"";
                var strImgReplace = string.Format("<IMG SRC=\"data:image/gif;base64,{0}\"", gifHexString);

                htmlString = Regex.Replace(htmlString, strImgPattern, strImgReplace);

                return htmlString;
            }
            catch (System.Web.Services.Protocols.SoapException soapEx)
            {
                return string.Format("Fehler: {0} -> {1}", soapEx.Message, soapEx.Detail.InnerText);
            }
            catch (Exception ex)
            {
                return string.Format("Fehler: {0}", ex.Message);
            }
        }

        private static byte[] ConvertHtmlToPdf(string strHtml)
        {
            var pdfConv = new HtmlToPdfConverter
            {
                Size = PageSize.A4,
                Zoom = 1.5f,
                ExecutionTimeout = TimeSpan.FromMinutes(1)
            };

            return pdfConv.GeneratePdf(strHtml);
        }
    }
}
