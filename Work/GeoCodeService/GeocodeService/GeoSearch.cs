using System;
using System.Configuration;
using System.IO;
using GeodeService.GeocodeService;
using ERPConnect;
using System.Globalization;

namespace GeodeService
{
    class GeoSearch
    {
        public static bool LogDebugData { get { return (String.Compare(ConfigurationManager.AppSettings["LogDebugData"], "true", true) == 0); } }

        public static string LogFilePath { get { return ConfigurationManager.AppSettings["LogPath"] + "GeocodeLog_" + DateTime.Today.ToString("yyyyMMdd") +".txt"; } }

        public static void CalculateRouteRequest(ref RFCServerFunction ServerFunc)
        {
            try
            {
                // Set a Bing Maps key before making a request
                string key = "AqPnjKCh8aPu2peBsLG2RmfNOUJhjWv1McpmcLT8ma_c92x7GXgEzeSbp7LnQgav";
                double Geo_Start_X = 0;
                double Geo_Start_Y = 0;
                double Geo_Ziel_X = 0;
                double Geo_Ziel_Y = 0;
                bool bFehlerData = false;
                RFCTable tblGeo;

                tblGeo = ServerFunc.Tables["GT_GEO_START"];
                if (tblGeo.Rows.Count == 1)
                {
                    String sGeoX = tblGeo.Rows[0]["GEOX"].ToString().Replace(",", ".");
                    String sGeoY = tblGeo.Rows[0]["GEOY"].ToString().Replace(",", ".");
                    try
                    {
                        Geo_Start_X = double.Parse(sGeoX, CultureInfo.InvariantCulture);
                        Geo_Start_Y = double.Parse(sGeoY, CultureInfo.InvariantCulture);
                    }
                    catch (Exception)
                    { bFehlerData = true; }
                }

                tblGeo = ServerFunc.Tables["GT_GEO_ZIEL"];
                if (tblGeo.Rows.Count == 1)
                {
                    String sGeoX = tblGeo.Rows[0]["GEOX"].ToString().Replace(",", ".");
                    String sGeoY = tblGeo.Rows[0]["GEOY"].ToString().Replace(",", ".");
                    try
                    {
                        Geo_Ziel_X = double.Parse(sGeoX, CultureInfo.InvariantCulture);
                        Geo_Ziel_Y = double.Parse(sGeoY, CultureInfo.InvariantCulture);
                    }
                    catch (Exception)
                    { bFehlerData = true; }
                }

                if (bFehlerData == false)
                {
                    RouteService.RouteRequest routeRequest = new RouteService.RouteRequest();

                    // Set the credentials using a valid Bing Maps key
                    routeRequest.Credentials = new RouteService.Credentials();
                    routeRequest.Credentials.ApplicationId = key;

                    // Set the start, stop, and end points
                    RouteService.Waypoint[] waypoints = new RouteService.Waypoint[2];
                    waypoints[0] = new RouteService.Waypoint();
                    waypoints[0].Description = "Start";
                    waypoints[0].Location = new RouteService.Location();
                    waypoints[0].Location.Latitude = Geo_Start_X;
                    waypoints[0].Location.Longitude = Geo_Start_Y;
                                       
                    waypoints[1] = new RouteService.Waypoint();
                    waypoints[1].Description = "End";
                    waypoints[1].Location = new RouteService.Location();
                    waypoints[1].Location.Latitude = Geo_Ziel_X;
                    waypoints[1].Location.Longitude = Geo_Ziel_Y;

                    if (LogDebugData)
                    {
                        // DEBUG: Log request data to txt
                        using (StreamWriter writer = new StreamWriter(LogFilePath, true))
                        {
                            writer.WriteLine("Request(Route): Latitude1={0}, Longitude1={1}, Latitude2={2}, Longitude2={3}",
                                Geo_Start_X.ToString(), Geo_Start_Y.ToString(), Geo_Ziel_X.ToString(), Geo_Ziel_Y.ToString());
                            writer.Close();
                        }
                    }

                    routeRequest.Waypoints = waypoints;
                    System.Net.ServicePointManager.Expect100Continue = false;
                    // Make the calculate route request
                    RouteService.RouteServiceClient routeService = new RouteService.RouteServiceClient("BasicHttpBinding_IRouteService");
                    routeService.Open();
                    routeService.Endpoint.Name = "BasicHttpBinding_IRouteService";

                    RouteService.RouteResponse routeResponse = routeService.CalculateRoute(routeRequest);
                    // Iterate through each itinerary item to get the route directions
                    Double dEntf = Math.Round(routeResponse.Result.Summary.Distance, 0);
                    ServerFunc.Exports["E_ENTFERNUNG"].ParamValue = dEntf.ToString();

                    if (LogDebugData)
                    {
                        // DEBUG: Log response data to txt
                        using (StreamWriter writer = new StreamWriter(LogFilePath, true))
                        {
                            writer.WriteLine("-> Response(Route) : Distance={0}",
                                             dEntf.ToString());
                            writer.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write("Ein Fehler ist aufgetreten: " + ex.Message); 
            }
        }

        public static void GeocodeAddress(string Street, string PostalCode, string City, string Hausnr, string sLand, Boolean isStart, ref RFCServerFunction ServerFunc)
        {
            try
            {
                string key = "AqPnjKCh8aPu2peBsLG2RmfNOUJhjWv1McpmcLT8ma_c92x7GXgEzeSbp7LnQgav";
                GeocodeRequest geocodeRequest = new GeocodeRequest();

                // Set the credentials using a valid Bing Maps key
                geocodeRequest.Credentials = new Credentials();
                geocodeRequest.Credentials.ApplicationId = key;

                // Set the full address query
                geocodeRequest.Address = new Address();
                geocodeRequest.Address.Locality = City;
                geocodeRequest.Address.AddressLine = Street + " " + Hausnr;
                geocodeRequest.Address.PostalCode = PostalCode;
                geocodeRequest.Address.CountryRegion = sLand;
                geocodeRequest.Culture = "de-DE";
                
                // Set the options to only return high confidence results 
                ConfidenceFilter[] filters = new ConfidenceFilter[1];
                filters[0] = new ConfidenceFilter();
                filters[0].MinimumConfidence = Confidence.Low;

                // Add the filters to the options
                GeocodeOptions geocodeOptions = new GeocodeOptions();
                geocodeOptions.Filters = filters;
                geocodeRequest.Options = geocodeOptions;
                System.Net.ServicePointManager.Expect100Continue = false;

                if (LogDebugData)
                {
                    // DEBUG: Log request data to txt
                    using (StreamWriter writer = new StreamWriter(LogFilePath, true))
                    {
                        writer.WriteLine("Request(Adresse): AdressLine={0}, PostalCode={1}, Locality={2}, CountryRegion={3}", 
                            Street + " " + Hausnr, PostalCode, City, sLand);
                        writer.Close();
                    }
                }

                // Make the geocode request
                GeocodeServiceClient geocodeService = new GeocodeServiceClient("BasicHttpBinding_IGeocodeService");
                geocodeService.Open();
                geocodeService.Endpoint.Name = "BasicHttpBinding_IGeocodeService";
                GeocodeResponse geocodeResponse = geocodeService.Geocode(geocodeRequest);
                RFCTable tblGeo;
                if (isStart)
                {
                    tblGeo = ServerFunc.Tables["GT_GEO_START"];
                }
                else
                {
                    tblGeo = ServerFunc.Tables["GT_GEO_ZIEL"];
                }

                if (geocodeResponse.Results.Length > 0)
                {
                    RFCStructure row;
                    for (int i = geocodeResponse.Results.Length - 1; i >= 0; i--)
                    {
                        row = tblGeo.AddRow();

                        row["GEOX"] = geocodeResponse.Results[i].Locations[0].Latitude;
                        row["GEOY"] = geocodeResponse.Results[i].Locations[0].Longitude;

                        String Ort = geocodeResponse.Results[i].Address.Locality;
                        String Strasse = geocodeResponse.Results[i].Address.AddressLine;
                        String PLZ = geocodeResponse.Results[i].Address.PostalCode;
                        String sRetLand = geocodeResponse.Results[i].Address.CountryRegion;

                        if (LogDebugData)
                        {
                            // DEBUG: Log response data to txt
                            using (StreamWriter writer = new StreamWriter(LogFilePath, true))
                            {
                                writer.WriteLine("-> Response(Adresse) Line {0}: Latitude={1}, Longitude={2}, AddressLine={3}, PostalCode={4}, Locality={5}, CountryRegion={6}", 
                                    (i + 1).ToString(), row["GEOX"], row["GEOY"], Strasse, PLZ, Ort, sRetLand);
                                writer.Close();
                            }
                        }

                        if (Strasse.Contains(Hausnr)&& Hausnr.Length > 0 )
                        {
                            Strasse = Strasse.Replace(Hausnr, "");
                        }
                        if (Strasse.Contains(Hausnr.ToUpper()) && Hausnr.Length > 0)
                        {
                            Strasse = Strasse.Replace(Hausnr.ToUpper(), "");//kommt vor das 8A statt 8a zurück kommt
                        }
                        if (Ort.Length == 0 && row["GEOX"].ToString().Length > 0 && row["GEOY"].ToString().Length > 0)
                        {
                            Ort = City;
                        }
                        if (isStart)
                        {
                            if (Strasse.Trim().Length == 0) { ServerFunc.Exports["E_FEHLER_ST"].ParamValue = "Strasse"; }
                            if (PLZ.Trim().Length == 0) { ServerFunc.Exports["E_FEHLER_ST"].ParamValue = "PLZ"; }
                            if (Ort.Trim().Length == 0) { ServerFunc.Exports["E_FEHLER_ST"].ParamValue = "Ort"; }
                        }
                        else
                        {
                            if (Strasse.Trim().Length == 0) { ServerFunc.Exports["E_FEHLER_ZI"].ParamValue = "Strasse"; }
                            if (PLZ.Trim().Length == 0) { ServerFunc.Exports["E_FEHLER_ZI"].ParamValue = "PLZ"; }
                            if (Ort.Trim().Length == 0) { ServerFunc.Exports["E_FEHLER_ZI"].ParamValue = "Ort"; }
                        }

                        row["ADRESSE"] = sRetLand + ", " + PLZ + ", " + Ort + ", " + Strasse.Trim() + ", " + Hausnr;
                        row["MARK"] = "";
                    }
                }
                else
                {
                    if (LogDebugData)
                    {
                        // DEBUG: Log response data to txt
                        using (StreamWriter writer = new StreamWriter(LogFilePath, true))
                        {
                            writer.WriteLine("-> Response(Adresse): NO_DATA");
                            writer.Close();
                        }
                    }

                    if (isStart)
                    {
                        RFCStructure row;
                        row = tblGeo.AddRow();
                        row["ADRESSE"] = " , , , , ";
                        ServerFunc.Exports["E_FEHLER_ST"].ParamValue = "NO_DATA";
                        
                    }
                    else
                    { ServerFunc.Exports["E_FEHLER_ZI"].ParamValue = "NO_DATA";
                        RFCStructure row;
                        row = tblGeo.AddRow();
                        row["ADRESSE"] = " , , , , ";
                    }
                }
            }
            catch (Exception ex)
            {

                Console.Write("Ein Fehler ist aufgetreten: " + ex.Message); 
            }
        }

        public static void CalculateCordRoute(ref RFCServerFunction ServerFunc)
        {
            try
            {
                // Set a Bing Maps key before making a request
                string key = "AqPnjKCh8aPu2peBsLG2RmfNOUJhjWv1McpmcLT8ma_c92x7GXgEzeSbp7LnQgav";
                double Geo_Start_X = 0;
                double Geo_Start_Y = 0;
                double Geo_Ziel_X = 0;
                double Geo_Ziel_Y = 0;
                bool bFehlerData = false;

                String sGeoX = ServerFunc.Imports["I_GEOX_ST"].ParamValue.ToString().TrimEnd().Replace(",", ".");
                String sGeoY = ServerFunc.Imports["I_GEOY_ST"].ParamValue.ToString().TrimEnd().Replace(",", ".");
                if (sGeoX.Length == 0) {ServerFunc.Exports["E_FEHLER_ST"].ParamValue = "I_GEOX_ST"; }
                if (sGeoY.Length == 0) {ServerFunc.Exports["E_FEHLER_ST"].ParamValue = "I_GEOY_ST"; }

                try
                {
                    Geo_Start_X = double.Parse(sGeoX, CultureInfo.InvariantCulture);
                    Geo_Start_Y = double.Parse(sGeoY, CultureInfo.InvariantCulture);
                }
                catch (Exception)
                { bFehlerData = true; }

                sGeoX = ServerFunc.Imports["I_GEOX_ZI"].ParamValue.ToString().TrimEnd().Replace(",", ".");
                sGeoY = ServerFunc.Imports["I_GEOY_ZI"].ParamValue.ToString().TrimEnd().Replace(",", ".");
                if (sGeoX.Length == 0) { ServerFunc.Exports["E_FEHLER_ZI"].ParamValue = "I_GEOX_ZI"; }
                if (sGeoY.Length == 0) { ServerFunc.Exports["E_FEHLER_ZI"].ParamValue = "I_GEOY_ZI"; }
                try
                {
                    Geo_Ziel_X = double.Parse(sGeoX, CultureInfo.InvariantCulture);
                    Geo_Ziel_Y = double.Parse(sGeoY, CultureInfo.InvariantCulture);
                }
                catch (Exception)
                { bFehlerData = true; }

                if (bFehlerData == false)
                {
                    RouteService.RouteRequest routeRequest = new RouteService.RouteRequest();

                    // Set the credentials using a valid Bing Maps key
                    routeRequest.Credentials = new RouteService.Credentials();
                    routeRequest.Credentials.ApplicationId = key;
                    // Set the start, stop, and end points
                    RouteService.Waypoint[] waypoints = new RouteService.Waypoint[2];
                    waypoints[0] = new RouteService.Waypoint();
                    waypoints[0].Description = "Start";
                    waypoints[0].Location = new RouteService.Location();
                    waypoints[0].Location.Latitude = Geo_Start_X;
                    waypoints[0].Location.Longitude = Geo_Start_Y;

                    waypoints[1] = new RouteService.Waypoint();
                    waypoints[1].Description = "End";
                    waypoints[1].Location = new RouteService.Location();
                    waypoints[1].Location.Latitude = Geo_Ziel_X;
                    waypoints[1].Location.Longitude = Geo_Ziel_Y;

                    if (LogDebugData)
                    {
                        // DEBUG: Log request data to txt
                        using (StreamWriter writer = new StreamWriter(LogFilePath, true))
                        {
                            writer.WriteLine("Request(RouteCoord): Latitude1={0}, Longitude1={1}, Latitude2={2}, Longitude2={3}",
                                Geo_Start_X.ToString(), Geo_Start_Y.ToString(), Geo_Ziel_X.ToString(), Geo_Ziel_Y.ToString());
                            writer.Close();
                        }
                    }

                    routeRequest.Waypoints = waypoints;
                    System.Net.ServicePointManager.Expect100Continue = false;
                    // Make the calculate route request
                    RouteService.RouteServiceClient routeService = new RouteService.RouteServiceClient("BasicHttpBinding_IRouteService");
                    routeService.Open();
                    routeService.Endpoint.Name = "BasicHttpBinding_IRouteService";

                    RouteService.RouteResponse routeResponse = routeService.CalculateRoute(routeRequest);
                    // Iterate through each itinerary item to get the route directions
                    Double dEntf = Math.Round(routeResponse.Result.Summary.Distance, 0);
                    ServerFunc.Exports["E_ENTFERNUNG"].ParamValue = dEntf.ToString();

                    if (LogDebugData)
                    {
                        // DEBUG: Log response data to txt
                        using (StreamWriter writer = new StreamWriter(LogFilePath, true))
                        {
                            writer.WriteLine("-> Response(RouteCoord) : Distance={0}",
                                             dEntf.ToString());
                            writer.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write("Ein Fehler ist aufgetreten: " + ex.Message); 
            }
        }

        public static void CalculateCordRouteTable(string lfdnr, string GEOX_ST, string GEOY_ST, string GEOX_ZI, string GEOY_ZI, ref RFCServerFunction ServerFunc)
        {
            try
            {
                // Set a Bing Maps key before making a request
                string key = "AqPnjKCh8aPu2peBsLG2RmfNOUJhjWv1McpmcLT8ma_c92x7GXgEzeSbp7LnQgav";
                double Geo_Start_X = 0;
                double Geo_Start_Y = 0;
                double Geo_Ziel_X = 0;
                double Geo_Ziel_Y = 0;
                bool bFehlerData = false;
                string FEHLER_ST = "";
                string FEHLER_ZI = "";

                String sGeoX = GEOX_ST.TrimEnd().Replace(",", ".");
                String sGeoY = GEOY_ST.TrimEnd().Replace(",", ".");
                if (sGeoX.Length == 0) { FEHLER_ST = "I_GEOX_ST"; }
                if (sGeoY.Length == 0) { FEHLER_ST = "I_GEOY_ST"; }
                try
                {
                    Geo_Start_X = double.Parse(sGeoX, CultureInfo.InvariantCulture);
                    Geo_Start_Y = double.Parse(sGeoY, CultureInfo.InvariantCulture);
                }
                catch (Exception)
                { bFehlerData = true; }

                sGeoX = GEOX_ZI.TrimEnd().Replace(",", ".");
                sGeoY = GEOY_ZI.TrimEnd().Replace(",", ".");
                if (sGeoX.Length == 0) { FEHLER_ZI = "I_GEOX_ZI"; }
                if (sGeoY.Length == 0) { FEHLER_ZI = "I_GEOX_ZI"; }
                try
                {
                    Geo_Ziel_X = double.Parse(sGeoX, CultureInfo.InvariantCulture);
                    Geo_Ziel_Y = double.Parse(sGeoY, CultureInfo.InvariantCulture);
                }
                catch (Exception)
                { bFehlerData = true; }

                if (bFehlerData == false)
                {
                    RouteService.RouteRequest routeRequest = new RouteService.RouteRequest();

                    // Set the credentials using a valid Bing Maps key
                    routeRequest.Credentials = new RouteService.Credentials();
                    routeRequest.Credentials.ApplicationId = key;
                    // Set the start, stop, and end points
                    RouteService.Waypoint[] waypoints = new RouteService.Waypoint[2];
                    waypoints[0] = new RouteService.Waypoint();
                    waypoints[0].Description = "Start";
                    waypoints[0].Location = new RouteService.Location();
                    waypoints[0].Location.Latitude = Geo_Start_X;
                    waypoints[0].Location.Longitude = Geo_Start_Y;

                    waypoints[1] = new RouteService.Waypoint();
                    waypoints[1].Description = "End";
                    waypoints[1].Location = new RouteService.Location();
                    waypoints[1].Location.Latitude = Geo_Ziel_X;
                    waypoints[1].Location.Longitude = Geo_Ziel_Y;

                    if (LogDebugData)
                    {
                        // DEBUG: Log request data to txt
                        using (StreamWriter writer = new StreamWriter(LogFilePath, true))
                        {
                            writer.WriteLine("Request(RouteCoord): Latitude1={0}, Longitude1={1}, Latitude2={2}, Longitude2={3}",
                                Geo_Start_X.ToString(), Geo_Start_Y.ToString(), Geo_Ziel_X.ToString(), Geo_Ziel_Y.ToString());
                            writer.Close();
                        }
                    }

                    routeRequest.Waypoints = waypoints;
                    System.Net.ServicePointManager.Expect100Continue = false;
                    // Make the calculate route request
                    RouteService.RouteServiceClient routeService = new RouteService.RouteServiceClient("BasicHttpBinding_IRouteService");
                    routeService.Open();
                    routeService.Endpoint.Name = "BasicHttpBinding_IRouteService";

                    RouteService.RouteResponse routeResponse = routeService.CalculateRoute(routeRequest);
                    // Iterate through each itinerary item to get the route directions
                    RFCTable tblGeo;
                    tblGeo = ServerFunc.Tables["GT_ENTFERUNG"];

                    RFCStructure row;
                    row = tblGeo.AddRow();
                    row["LFDNR"] = lfdnr;
                    row["FEHLER_ST"] = FEHLER_ST;
                    row["FEHLER_ZI"] = FEHLER_ZI;
                    Double dEntf = Math.Round(routeResponse.Result.Summary.Distance,0);
                    row["ENTFERNUNG"] = dEntf.ToString();
                    row["GEOX_ST"] = Geo_Start_X.ToString();
                    row["GEOY_ST"] = Geo_Start_Y.ToString();
                    row["GEOX_ZI"] = Geo_Ziel_X.ToString();
                    row["GEOY_ZI"] = Geo_Ziel_Y.ToString();

                    if (LogDebugData)
                    {
                        // DEBUG: Log response data to txt
                        using (StreamWriter writer = new StreamWriter(LogFilePath, true))
                        {
                            writer.WriteLine("-> Response(RouteCoord) : Distance={0}",
                                             dEntf.ToString());
                            writer.Close();
                        }
                    }
                }
                else 
                {
                    RFCTable tblGeo;
                    tblGeo = ServerFunc.Tables["GT_ENTFERUNG"];                    
                    RFCStructure row;
                    row = tblGeo.AddRow();
                    row["LFDNR"] = lfdnr;
                    row["FEHLER_ST"] = FEHLER_ST;
                    row["FEHLER_ZI"] = FEHLER_ZI;
                    row["ENTFERNUNG"] = "";
                    row["GEOX_ST"] = Geo_Start_X.ToString();
                    row["GEOY_ST"] = Geo_Start_Y.ToString();
                    row["GEOX_ZI"] = Geo_Ziel_X.ToString();
                    row["GEOY_ZI"] = Geo_Ziel_Y.ToString();                
                }
            }
            catch (Exception ex)
            {
                Console.Write("Ein Fehler ist aufgetreten: " + ex.Message);
            }
        }

        public static void GeocodeAddressTable(string Street, string PostalCode, string City, string Hausnr, string lfdnr, string sLand, ref RFCServerFunction ServerFunc)
        {
            try
            {
                string key = "AqPnjKCh8aPu2peBsLG2RmfNOUJhjWv1McpmcLT8ma_c92x7GXgEzeSbp7LnQgav";
                GeocodeRequest geocodeRequest = new GeocodeRequest();

                // Set the credentials using a valid Bing Maps key
                geocodeRequest.Credentials = new Credentials();
                geocodeRequest.Credentials.ApplicationId = key;

                // Set the full address query
                geocodeRequest.Address = new Address();
                geocodeRequest.Address.Locality = City;
                geocodeRequest.Address.AddressLine = Street + " " + Hausnr;
                geocodeRequest.Address.PostalCode = PostalCode;
                geocodeRequest.Address.CountryRegion = sLand; 
                geocodeRequest.Culture = "de-DE";

                // Set the options to only return high confidence results 
                ConfidenceFilter[] filters = new ConfidenceFilter[1];
                filters[0] = new ConfidenceFilter();
                filters[0].MinimumConfidence = Confidence.Low;

                // Add the filters to the options
                GeocodeOptions geocodeOptions = new GeocodeOptions();
                geocodeOptions.Filters = filters;
                geocodeRequest.Options = geocodeOptions;
                System.Net.ServicePointManager.Expect100Continue = false;

                if (LogDebugData)
                {
                    // DEBUG: Log request data to txt
                    using (StreamWriter writer = new StreamWriter(LogFilePath, true))
                    {
                        writer.WriteLine("Request(Adresse): AdressLine={0}, PostalCode={1}, Locality={2}, CountryRegion={3}",
                            Street + " " + Hausnr, PostalCode, City, sLand);
                        writer.Close();
                    }
                }

                // Make the geocode request
                GeocodeServiceClient geocodeService = new GeocodeServiceClient("BasicHttpBinding_IGeocodeService");
                geocodeService.Open();
                geocodeService.Endpoint.Name = "BasicHttpBinding_IGeocodeService";
                GeocodeResponse geocodeResponse = geocodeService.Geocode(geocodeRequest);
                RFCTable tblGeo;

                tblGeo = ServerFunc.Tables["GT_ADRS"];
                RFCStructure row;
                if (geocodeResponse.Results.Length > 0)
                {                   
                    for (int i = geocodeResponse.Results.Length - 1; i >= 0; i--)
                    {
                        row = tblGeo.AddRow();
                        row["LFDNR"] = lfdnr;
                        row["GEOX"] = geocodeResponse.Results[i].Locations[0].Latitude;
                        row["GEOY"] = geocodeResponse.Results[i].Locations[0].Longitude;

                        String Ort = geocodeResponse.Results[i].Address.Locality;
                        String Strasse = geocodeResponse.Results[i].Address.AddressLine;
                        String PLZ = geocodeResponse.Results[i].Address.PostalCode;
                        String sRetLand = geocodeResponse.Results[i].Address.CountryRegion;

                        if (LogDebugData)
                        {
                            // DEBUG: Log response data to txt
                            using (StreamWriter writer = new StreamWriter(LogFilePath, true))
                            {
                                writer.WriteLine("-> Response(Adresse) Line {0}: Latitude={1}, Longitude={2}, AddressLine={3}, PostalCode={4}, Locality={5}, CountryRegion={6}",
                                    (i + 1).ToString(), row["GEOX"], row["GEOY"], Strasse, PLZ, Ort, sRetLand);
                                writer.Close();
                            }
                        }

                        if (Ort.Length == 0 && row["GEOX"].ToString().Length > 0 && row["GEOY"].ToString().Length > 0)
                        {
                            Ort = City;
                        }
                        if (Strasse.Contains(Hausnr) && Hausnr.Length > 0)
                        {
                            Strasse = Strasse.Replace(Hausnr, "");    
                        }
                        if (Strasse.Contains(Hausnr.ToUpper()) && Hausnr.Length > 0)
                        {
                            Strasse = Strasse.Replace(Hausnr.ToUpper(), "");//kommt vor das 8A statt 8a zurück kommt
                        }
                        
                        row["COUNTRY"] = sRetLand;
                        row["POST_CODE1"] = PLZ;
                        row["CITY1"] = Ort;
                        row["STREET"] = Strasse;
                        row["HOUSE_NUM1"] = Hausnr;

                        if (Strasse.Trim().Length == 0) { row["FEHLER"] = "Strasse"; }
                        if (PLZ.Trim().Length == 0) { row["FEHLER"] = "PLZ"; }
                        if (Ort.Trim().Length == 0) { row["FEHLER"] = "Ort"; }
                    }
                }
                else
                {
                    if (LogDebugData)
                    {
                        // DEBUG: Log response data to txt
                        using (StreamWriter writer = new StreamWriter(LogFilePath, true))
                        {
                            writer.WriteLine("-> Response(Adresse): NO_DATA");
                            writer.Close();
                        }
                    }

                    row = tblGeo.AddRow();
                    row["LFDNR"] = lfdnr;
                    row["FEHLER"] = "NO_DATA";
                }
            }
            catch (Exception ex)
            {
                Console.Write("Ein Fehler ist aufgetreten: " + ex.Message);
            }
        }
    }
}
