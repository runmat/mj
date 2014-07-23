using System;
using ERPConnect;
using System.Data;
using System.Configuration;

namespace GeodeService
{
    class Program
    {
        static void Main(string[] args)
        {
            // define server object
            LIC.SetLic("5DVZ5588DC-25444");
            RFCServer s = new RFCServer();
            const int numberOfServers = 10;
            s.GatewayHost = ConfigurationManager.AppSettings["GatewayHost"];
            s.GatewayService = ConfigurationManager.AppSettings["GatewayService"];
            s.ProgramID = ConfigurationManager.AppSettings["ProgramID"];
            
            s.IncomingCall += new RFCServer.OnIncomingCall(s_IncomingCall);

            // Add and register function module
            for (int i = 0; i < numberOfServers; i++)
            {
                Console.WriteLine("RFC Server GeocodeService instance {0}", i);
                RFCServerFunction f = s.RegisteredFunctions.Add("Z_V_KCL_GEOKODIERUNG_001");
                CreatePara_KCL_Geocodierung_001(f);
                f = s.RegisteredFunctions.Add("ZBC_GEOCODIERUNG_001");
                CreatePara_ZBC_GEOKODIERUNG_001(f);
            }
            // start server
            s.Start(false);

            Console.Write("Server gestarted. Beliebige Taste drücken um zu beenden.");
            Console.ReadLine();
            s.Stop();    
        }

        private static void s_IncomingCall(RFCServer Sender, RFCServerFunction CalledFunction)
        {
            if (CalledFunction.FunctionName == "Z_V_KCL_GEOKODIERUNG_001")
            {
                String sAktion = CalledFunction.Imports["I_AKTION"].ParamValue.ToString().TrimEnd();
                CalledFunction.Exports["E_ENTFERNUNG"].ParamValue = "";
                CalledFunction.Exports["E_FEHLER_ST"].ParamValue = "";
                CalledFunction.Exports["E_FEHLER_ZI"].ParamValue = "";
                if (sAktion == "A" || sAktion == "E")
                {
                    //Startadresse
                    String sPLz = CalledFunction.Imports["I_POST_CODE1_ST"].ParamValue.ToString().TrimEnd();
                    String sOrt = CalledFunction.Imports["I_CITY1_ST"].ParamValue.ToString().TrimEnd();
                    String sStrasse = CalledFunction.Imports["I_STREET_ST"].ParamValue.ToString().TrimEnd();
                    String sHNr = CalledFunction.Imports["I_HOUSE_NUM1_ST"].ParamValue.ToString().TrimEnd();
                    String sLand = CalledFunction.Imports["I_COUNTRY_ST"].ParamValue.ToString().TrimEnd();

                    GeoSearch.GeocodeAddress(sStrasse, sPLz, sOrt, sHNr, sLand, true, ref CalledFunction);
                    //Zieladresse
                    sPLz = CalledFunction.Imports["I_POST_CODE1_ZI"].ParamValue.ToString().TrimEnd();
                    sOrt = CalledFunction.Imports["I_CITY1_ZI"].ParamValue.ToString().TrimEnd();
                    sStrasse = CalledFunction.Imports["I_STREET_ZI"].ParamValue.ToString().TrimEnd();
                    sHNr = CalledFunction.Imports["I_HOUSE_NUM1_ZI"].ParamValue.ToString().TrimEnd();
                    sLand = CalledFunction.Imports["I_COUNTRY_ZI"].ParamValue.ToString().TrimEnd();
                    if (sPLz.Length + sOrt.Length + sStrasse.Length > 0)
                    {
                        GeoSearch.GeocodeAddress(sStrasse, sPLz, sOrt, sHNr, sLand, false, ref CalledFunction);
                    }
                   
                    if (sAktion == "E")
                    {
                        int CountStartAdress = CalledFunction.Tables["GT_GEO_START"].Rows.Count;
                        int CountZielAdress = CalledFunction.Tables["GT_GEO_Ziel"].Rows.Count;
                        if (CountStartAdress == 1 && CountZielAdress == 1)
                        {
                            GeoSearch.CalculateRouteRequest(ref CalledFunction);
                        }
                        if (CountStartAdress > 1)
                        { CalledFunction.Exports["E_FEHLER_ST"].ParamValue = ">1"; }
                        if (CountZielAdress > 1)
                        { CalledFunction.Exports["E_FEHLER_ZI"].ParamValue = ">1"; }
                    }
                }
                else if (sAktion == "G")
                {
                    GeoSearch.CalculateCordRoute(ref CalledFunction);
                }         
            }
            else if (CalledFunction.FunctionName == "ZBC_GEOCODIERUNG_001")
            {
                String sAktion = CalledFunction.Tables["GT_IN"].Rows[0]["AKTION"].ToString().TrimEnd();
                if (sAktion == "A" || sAktion == "E")
                {
                    String lfd = CalledFunction.Tables["GT_IN"].Rows[0]["LFDNR"].ToString().TrimEnd();
                    //Startadresse
                    String sPLz = CalledFunction.Tables["GT_IN"].Rows[0]["POST_CODE1_ST"].ToString().TrimEnd();
                    String sOrt = CalledFunction.Tables["GT_IN"].Rows[0]["CITY1_ST"].ToString().TrimEnd();
                    String sStrasse = CalledFunction.Tables["GT_IN"].Rows[0]["STREET_ST"].ToString().TrimEnd();
                    String sHNr = CalledFunction.Tables["GT_IN"].Rows[0]["HOUSE_NUM1_ST"].ToString().TrimEnd();
                    String sLand = CalledFunction.Tables["GT_IN"].Rows[0]["COUNTRY_ST"].ToString().TrimEnd();

                    GeoSearch.GeocodeAddressTable(sStrasse, sPLz, sOrt, sHNr, lfd, sLand, ref CalledFunction);
                    //Zieladresse
                    sPLz = CalledFunction.Tables["GT_IN"].Rows[0]["POST_CODE1_ZI"].ToString().TrimEnd();
                    sOrt = CalledFunction.Tables["GT_IN"].Rows[0]["CITY1_ZI"].ToString().TrimEnd();
                    sStrasse = CalledFunction.Tables["GT_IN"].Rows[0]["STREET_ZI"].ToString().TrimEnd();
                    sHNr = CalledFunction.Tables["GT_IN"].Rows[0]["HOUSE_NUM1_ZI"].ToString().TrimEnd();
                    sLand = CalledFunction.Tables["GT_IN"].Rows[0]["COUNTRY_ZI"].ToString().TrimEnd();
                    if (sPLz.Length + sOrt.Length + sStrasse.Length > 0)
                    {
                        GeoSearch.GeocodeAddressTable(sStrasse, sPLz, sOrt, sHNr, lfd, sLand, ref CalledFunction);
                    } 
                                              
                    if (sAktion == "E")
                    {
                        DataTable tblAdress;
                        tblAdress = CalledFunction.Tables["GT_ADRS"].ToADOTable();
                        int CountAdress = tblAdress.Select("LFDNR = '" + lfd + "'").Length;
                        if (CountAdress == 2)
                        {
                            DataRow [] dRow  = tblAdress.Select("LFDNR = '" + lfd + "'");
                            string GEOX_ST = "", GEOY_ST = "", GEOX_ZI = "", GEOY_ZI = "";

                            for (int i = 0; i < dRow.Length; i++)
                            {
                                if (i==0)
                                {
                                    GEOX_ST = dRow[i]["GEOX"].ToString();
                                    GEOY_ST = dRow[i]["GEOY"].ToString();                                    
                                }
                                if (i == 1)
                                {
                                    GEOX_ZI = dRow[i]["GEOX"].ToString();
                                    GEOY_ZI = dRow[i]["GEOY"].ToString();
                                }
                            }

                            GeoSearch.CalculateCordRouteTable(lfd,GEOX_ST, GEOY_ST, GEOX_ZI, GEOY_ZI, ref CalledFunction);
                        }
                    }
                }
                if (sAktion == "G")
                {
                    foreach (RFCStructure rowStructure in CalledFunction.Tables["GT_IN"].Rows)
                    {
                        String lfd = rowStructure["LFDNR"].ToString().TrimEnd();
                        String GEOX_ST = rowStructure["GEOX_ST"].ToString();
                        String GEOY_ST = rowStructure["GEOY_ST"].ToString();
                        String GEOX_ZI = rowStructure["GEOX_ZI"].ToString();
                        String GEOY_ZI = rowStructure["GEOY_ZI"].ToString();
                  
                        GeoSearch.CalculateCordRouteTable(lfd, GEOX_ST, GEOY_ST, GEOX_ZI, GEOY_ZI, ref CalledFunction);
                    }
                } 
            }
            else
                throw new ERPException("Function unknown");
        }

        private static void CreatePara_KCL_Geocodierung_001(RFCServerFunction f)
        {
            RFCTable starttable = f.Tables.Add("GT_GEO_START");
            starttable.Columns.Add("GEOX", 30, 0, RFCTYPE.CHAR);
            starttable.Columns.Add("GEOY", 30, 0, RFCTYPE.CHAR);
            starttable.Columns.Add("ADRESSE", 200, 0, RFCTYPE.CHAR);
            starttable.Columns.Add("MARK", 1, 0, RFCTYPE.CHAR);

            RFCTable zieltable = f.Tables.Add("GT_GEO_ZIEL");
            zieltable.Columns.Add("GEOX", 30, 0, RFCTYPE.CHAR);
            zieltable.Columns.Add("GEOY", 30, 0, RFCTYPE.CHAR);
            zieltable.Columns.Add("ADRESSE", 200, 0, RFCTYPE.CHAR);
            zieltable.Columns.Add("MARK", 1, 0, RFCTYPE.CHAR);

            f.Imports.Add("I_AKTION", RFCTYPE.CHAR, 50, 0);

            f.Imports.Add("I_COUNTRY_ST", RFCTYPE.CHAR, 3, 0);
            f.Imports.Add("I_POST_CODE1_ST", RFCTYPE.CHAR, 10, 0);
            f.Imports.Add("I_CITY1_ST", RFCTYPE.CHAR, 40, 0);
            f.Imports.Add("I_STREET_ST", RFCTYPE.CHAR, 60, 0);
            f.Imports.Add("I_HOUSE_NUM1_ST", RFCTYPE.CHAR, 10, 0);

            f.Imports.Add("I_COUNTRY_ZI", RFCTYPE.CHAR, 3, 0);
            f.Imports.Add("I_POST_CODE1_ZI", RFCTYPE.CHAR, 10, 0);
            f.Imports.Add("I_CITY1_ZI", RFCTYPE.CHAR, 40, 0);
            f.Imports.Add("I_STREET_ZI", RFCTYPE.CHAR, 60, 0);
            f.Imports.Add("I_HOUSE_NUM1_ZI", RFCTYPE.CHAR, 10, 0);

            f.Imports.Add("I_GEOX_ST", RFCTYPE.CHAR, 30, 0);
            f.Imports.Add("I_GEOY_ST", RFCTYPE.CHAR, 30, 0);
            f.Imports.Add("I_GEOX_ZI", RFCTYPE.CHAR, 30, 0);
            f.Imports.Add("I_GEOY_ZI", RFCTYPE.CHAR, 30, 0);

            f.Exports.Add("E_FEHLER_ST", RFCTYPE.CHAR, 10, 0);
            f.Exports.Add("E_FEHLER_ZI", RFCTYPE.CHAR, 10, 0);
            f.Exports.Add("E_ENTFERNUNG", RFCTYPE.CHAR, 10, 0);
        }

        private static void CreatePara_ZBC_GEOKODIERUNG_001(RFCServerFunction f)
        {
            RFCTable tblImport = f.Tables.Add("GT_IN");
            tblImport.Columns.Add("LFDNR",10,RFCTYPE.NUM);
            tblImport.Columns.Add("AKTION", 1, 0, RFCTYPE.CHAR);
            tblImport.Columns.Add("COUNTRY_ST", 3, 0, RFCTYPE.CHAR);
            tblImport.Columns.Add("POST_CODE1_ST", 10, 0, RFCTYPE.CHAR);
            tblImport.Columns.Add("CITY1_ST", 40, 0, RFCTYPE.CHAR);
            tblImport.Columns.Add("STREET_ST", 60 , 0, RFCTYPE.CHAR);
            tblImport.Columns.Add("HOUSE_NUM1_ST", 10, 0, RFCTYPE.CHAR);
            tblImport.Columns.Add("COUNTRY_ZI", 3, 0, RFCTYPE.CHAR);
            tblImport.Columns.Add("POST_CODE1_ZI", 10, 0, RFCTYPE.CHAR);
            tblImport.Columns.Add("CITY1_ZI", 40, 0, RFCTYPE.CHAR);
            tblImport.Columns.Add("STREET_ZI", 60, 0, RFCTYPE.CHAR);
            tblImport.Columns.Add("HOUSE_NUM1_ZI", 10, 0, RFCTYPE.CHAR);
            tblImport.Columns.Add("GEOX_ST", 30, 0, RFCTYPE.CHAR);
            tblImport.Columns.Add("GEOY_ST", 30, 0, RFCTYPE.CHAR);
            tblImport.Columns.Add("GEOX_ZI", 30, 0, RFCTYPE.CHAR);
            tblImport.Columns.Add("GEOY_ZI", 30, 0, RFCTYPE.CHAR);

            RFCTable tblEntfernung = f.Tables.Add("GT_ENTFERUNG");
            tblEntfernung.Columns.Add("LFDNR", 10, RFCTYPE.NUM);
            tblEntfernung.Columns.Add("FEHLER_ST", 10, 0, RFCTYPE.CHAR);
            tblEntfernung.Columns.Add("FEHLER_ZI", 10, 0, RFCTYPE.CHAR);
            tblEntfernung.Columns.Add("ENTFERNUNG", 5, 0, RFCTYPE.CHAR);
            tblEntfernung.Columns.Add("GEOX_ST", 30, 0, RFCTYPE.CHAR);
            tblEntfernung.Columns.Add("GEOY_ST", 30, 0, RFCTYPE.CHAR);
            tblEntfernung.Columns.Add("GEOX_ZI", 30, 0, RFCTYPE.CHAR);
            tblEntfernung.Columns.Add("GEOY_ZI", 30, 0, RFCTYPE.CHAR);

            RFCTable tblAdresse = f.Tables.Add("GT_ADRS");
            tblAdresse.Columns.Add("LFDNR", 10, RFCTYPE.NUM);
            tblAdresse.Columns.Add("COUNTRY", 3, 0, RFCTYPE.CHAR);
            tblAdresse.Columns.Add("POST_CODE1", 10, 0, RFCTYPE.CHAR);
            tblAdresse.Columns.Add("CITY1", 40, 0, RFCTYPE.CHAR);
            tblAdresse.Columns.Add("STREET", 60, 0, RFCTYPE.CHAR);
            tblAdresse.Columns.Add("HOUSE_NUM1", 10, 0, RFCTYPE.CHAR);
            tblAdresse.Columns.Add("GEOX", 30, 0, RFCTYPE.CHAR);
            tblAdresse.Columns.Add("GEOY", 30, 0, RFCTYPE.CHAR);
            tblAdresse.Columns.Add("FEHLER", 10, 0, RFCTYPE.CHAR);
        }
    }
}
