// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GeneralTools.Models;
using CkgDomainLogic.General.Contracts;
using GeneralTools.Contracts;

namespace CkgDomainLogic.DomainCommon.Services
{
    public class WeatherDataServiceOpenWeatherMap : IWeatherDataService
    {
        IGeneralConfigurationProvider GeneralConfigurationProvider { get { return DependencyResolver.Current.GetService<IGeneralConfigurationProvider>(); } }

        private string ApiKey { get { return GeneralConfigurationProvider.GetConfigAllServerVal("LicenseData", "WeatherDataService_OpenWeatherMap_ApiKey"); } }


        public string GetWeatherData(string cityAndCountry)
        {
            var url = string.Format("http://api.openweathermap.org/data/2.5/forecast?q={0}&mode=json&appid={1}", cityAndCountry, ApiKey);

            var request = WebRequest.Create(url);
            try
            {
                var response = request.GetResponse();
                var stream = response.GetResponseStream();
                if (stream == null)
                {
                    response.Close();
                    return "";
                }

                var streamReader = new StreamReader(stream);
                var json = streamReader.ReadToEnd();

                response.Close();
                streamReader.Close();

                return json;
            }
            catch (WebException)
            {
                return "";
            }
        }
    }
}