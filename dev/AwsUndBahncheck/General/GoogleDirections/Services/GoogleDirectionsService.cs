using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using GoogleDirections.Contracts;

namespace GoogleDirections.Services
{
    public static class GoogleDirectionsService
    {
        private const string GoogleDirectionsUrl =
            @"http://maps.google.com/maps/api/directions/xml?origin={0}&destination={1}&sensor=false";

        public static void CalculateRoute(string startAddressAsString, string startStreet, string endAddressAsString, string endStreet, out bool routeCalculationStatus, out double routeDistanceKm, 
                                                PredicateAddressCaching routeCalculationCachingLoad = null, ActionAddressCaching routeCalculationCachingSave = null)
        {
            routeCalculationStatus = false;
            routeDistanceKm = 0;

            if ((routeCalculationCachingLoad != null && routeCalculationCachingSave == null) ||
                (routeCalculationCachingLoad == null && routeCalculationCachingSave != null))
                throw new ArgumentException("Die Parameter 'routeCalculationCachingLoad' und 'RouteCalculationCachingSave' müssen entweder beide null oder beide gesetzt sein!");
                
            if (routeCalculationCachingLoad != null)
                if (true == (routeCalculationStatus = routeCalculationCachingLoad(startAddressAsString, endAddressAsString, out routeDistanceKm)))
                    return;

            var routeUrl = string.Format(GoogleDirectionsUrl, startAddressAsString, endAddressAsString);
            var routeXml = ReadHttpRequest(routeUrl);

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(routeXml);

            var startAddressNodes = xmlDoc.SelectNodes("/DirectionsResponse/route/leg/start_address");
            if (startAddressNodes != null && startAddressNodes.Count == 1)
                if (!RouteXmlContainsValidStreet(startAddressNodes[0].InnerText, startStreet))
                    return;
            var endAddressNodes = xmlDoc.SelectNodes("/DirectionsResponse/route/leg/end_address");
            if (endAddressNodes != null && endAddressNodes.Count == 1)
                if (!RouteXmlContainsValidStreet(endAddressNodes[0].InnerText, endStreet))
                    return;
            
            //var statusNodes = xmlDoc.SelectNodes("/DirectionsResponse/status");
            //if (statusNodes != null && statusNodes.Count == 1)
            //    routeCalculationStatus = (statusNodes[0].InnerText.ToUpper() == "OK");

            var distanceNodes = xmlDoc.SelectNodes("/DirectionsResponse/route/leg/distance/value");
            if (distanceNodes != null && distanceNodes.Count == 1)
            {
                int distanceMm;
                routeCalculationStatus = (Int32.TryParse(distanceNodes[0].InnerText.ToUpper(), out distanceMm));
                if (routeCalculationStatus)
                {
                    routeDistanceKm = distanceMm/1000.0;

                    if (routeCalculationCachingSave != null)
                        routeCalculationCachingSave(startAddressAsString, endAddressAsString, routeDistanceKm);
                }
            }
        }

        private static bool RouteXmlContainsValidStreet(string routeXml, string street)
        {
            const int minValidStreetLen = 7;

            if (string.IsNullOrEmpty(street))
                return true;

            routeXml = routeXml.ToLower();
            street = street.ToLower();

            if (street.Length <= minValidStreetLen)
                return routeXml.Contains(street);

            for (var i = street.Length; i >= minValidStreetLen; i--)
                if (routeXml.Contains(street.Substring(0, i)))
                    return true;

            return false;
        }

        private static string ReadHttpRequest(string url)
        {
            var html = "";

            var myWebRequest = WebRequest.Create(url);
            var myWebResponse = myWebRequest.GetResponse();
            if (myWebResponse == null) return "";
            var receiveStream = myWebResponse.GetResponseStream();
            if (receiveStream == null) return "";
            var encode = Encoding.GetEncoding("utf-8");
            var readStream = new StreamReader(receiveStream, encode);
            var read = new Char[256];
            var count = readStream.Read(read, 0, 256);
            while (count > 0)
            {
                var str = new String(read, 0, count);
                html += str;
                count = readStream.Read(read, 0, 256);
            }
            readStream.Close();
            myWebResponse.Close();

            return html;
        }


        #region Extensions for "IRoute"

        public static void RouteCalculateRoute(this IRoute r)
        {
            bool routeCalculationOk;
            double distanceKm;

            CalculateRoute(r.StartAddress.AsString, r.StartAddress.Street, r.EndAddress.AsString, r.EndAddress.Street, out routeCalculationOk, out distanceKm, 
                            r.RouteCalculationCachingLoad, r.RouteCalculationCachingSave);

            r.RouteCalculationOk = routeCalculationOk;
            r.DistanceKm = distanceKm;
        }

        public static void RouteCalculationReset(this IRoute r)
        {
            r.StartAddress.UseGeoLocation = false;
            r.StartAddress.GeoLocation = "";

            r.EndAddress.UseGeoLocation = false;
            r.EndAddress.GeoLocation = "";
            
            r.RouteCalculationOk = false;
            r.DistanceKm = 0;
        }

        #endregion
    }
}
