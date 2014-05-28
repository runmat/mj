using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Xml;
using AutoAct.Rest;
using GeneralTools.Contracts;
using GeneralTools.Services;
using Mobile.Entities;
using Mobile.Interfaces;
using Mobile.Utils;
using RestSharp;

namespace mobile
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            ISettingsReader settingsReader = new SettingsReader();
            ILogService logService = new LogService("mobile.de Export", string.Empty);
            IFileHelper fileHelper = new FileHelper(logService, settingsReader);
            IRestClient restClient = new RestClient();

            MobileRest mobileRest = new MobileRest(logService, restClient, settingsReader);

            var add = new ad();

            //add.SellerInventoryKeyValue = "17301829";
            //add.DescriptionValue = "FAHRZEUG  T E S T  ÜBERTRAGUNG: Beschreibung";
            //add.VehicleModelDescriptionValue = "Model Beschreibung";
            //add.VehicleMileageValue = "12345";
            //add.VehicleFirstRegistrationValue = "2013-06";
            //add.VehicleKbaHsnValue = "8566";
            //add.VehicleKbaTsnValue = "ABJ";
            //add.PriceGrossPricesConsumerPriceAmountValue = "999999";
            //add.VehicleMakeValue = "FORD";
            //add.VehicleModelValue = "Focus";
            //add.Visibility = "RESERVED";

            add.SellerInventoryKeyValue = "17301829";
            add.DescriptionValue = "FAHRZEUG  T E S T  ÜBERTRAGUNG: Beschreibung";
            add.VehicleModelDescriptionValue = "Model Beschreibung";
            add.VehicleMileageValue = "12345";
            add.VehicleFirstRegistrationValue = "2013-05";
            add.VehicleKbaHsnValue = "1313";
            add.VehicleKbaTsnValue = "AAQ";
            add.PriceGrossPricesConsumerPriceAmountValue = "999999";
            add.VehicleMakeValue = "Mercedes-Benz";
            //add.VehicleModelValue = "S 350 CDI 4MATIC";
            add.VehicleModelValue = "ANDERE";
            add.Visibility = "RESERVED";




            var resultPostAd = mobileRest.PostAd(add);

            var urlDesInserats = (string)resultPostAd.Value;
            var inseratId = urlDesInserats.Split(@"/".ToCharArray()[0]).Last();

            var resultGetInsert = mobileRest.GetAd(inseratId);


            //var upload = new UploadFile
            //    {
            //        Content = fileHelper.ReadAllBytes(@"C:\Users\braken\Desktop\header_pic.jpg"),
            //        FileName = "header_pic.jpg"
            //    };

            //var resultUploadFile = mobileRest.PostImage(16042, new[] {upload});
        }
    }
}
