using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Services.Description;
using System.Collections;
using System.Xml;
using System.Net;
using System.Security;

using System.Data;
using System.Configuration;

using ERPConnect;

using Netigate_Fahrzeugauslieferung_DAD.se.netigate.www;

namespace Netigate_Fahrzeugauslieferung_DAD
{
    class Program
    {

        //private static System.Collections.Generic.List<DynSapProxyObj> Proxys = new System.Collections.Generic.List<DynSapProxyObj>();
 

        static void Main(string[] args)
        {

            int CustomerId = 2382;
            string strGUID = "BEA9DFA8-706E-4574-8BB1-C9202FE00E87";

            DateTime tempdatum = DateTime.MinValue;
            string tempstring = "";

            DataTable ImportTable = new DataTable();

            DataColumn MANDT = new DataColumn();
            MANDT.ColumnName = "MANDT";
            ImportTable.Columns.Add(MANDT);

            DataColumn LIFNR = new DataColumn();
            LIFNR.ColumnName = "LIFNR";
            ImportTable.Columns.Add(LIFNR);

            DataColumn DATUM = new DataColumn();
            DATUM.ColumnName = "DATUM";
            ImportTable.Columns.Add(DATUM);

            DataColumn UZEIT = new DataColumn();
            UZEIT.ColumnName = "UZEIT";
            ImportTable.Columns.Add(UZEIT);

            DataColumn KENNZ = new DataColumn();
            KENNZ.ColumnName = "KENNZ";
            ImportTable.Columns.Add(KENNZ);

            DataColumn Frage5 = new DataColumn();
            Frage5.ColumnName = "FRAGE5";
            ImportTable.Columns.Add(Frage5);

            DataColumn Frage6 = new DataColumn();
            Frage6.ColumnName = "FRAGE6";
            ImportTable.Columns.Add(Frage6);

            DataColumn Frage7 = new DataColumn();
            Frage7.ColumnName = "FRAGE7";
            ImportTable.Columns.Add(Frage7);

            DataColumn Frage8 = new DataColumn();
            Frage8.ColumnName = "FRAGE8";
            ImportTable.Columns.Add(Frage8);

            DataColumn Frage9 = new DataColumn();
            Frage9.ColumnName = "FRAGE9";
            ImportTable.Columns.Add(Frage9);

            DataColumn Frage10 = new DataColumn();
            Frage10.ColumnName = "FRAGE10";
            ImportTable.Columns.Add(Frage10);

            DataColumn Frage11 = new DataColumn();
            Frage11.ColumnName = "FRAGE11";
            ImportTable.Columns.Add(Frage11);


            se.netigate.www.NetigateAPI s = new se.netigate.www.NetigateAPI();
            //s.Proxy = myProxy;
            s.Url = "https://www.netigate.se/netigateapidad/netigateapi.asmx";

            XmlNode x1 = s.GetSurveyListByCustomerId(CustomerId, strGUID);

            var Year = DateTime.Today.Year;

            foreach (XmlNode xn in x1.SelectNodes("Customer/Surveys/Survey"))
            {
                if (xn["SurveyName"].InnerText == "Fahrzeugauslieferung_DAD_" + Year)
                {
                    Console.WriteLine(xn["SurveyName"].InnerText + "\r\n");
                    Console.WriteLine(xn["SurveyId"].InnerText + "\r\n\r\n");

                    NetigateAPI napi = new NetigateAPI();
                    XmlNode Respondents;
                    Respondents = napi.GetFullAnsweredRespondentList(int.Parse(xn["SurveyId"].InnerText), xn["strGUID"].InnerText);

                    foreach (XmlNode xnRespondent in Respondents.SelectNodes("Respondents/Respondent"))
                    {

                        DataRow newRow = ImportTable.NewRow();

                        newRow["MANDT"] = ConfigurationManager.AppSettings["MANDT"].ToString();

                        Console.WriteLine(xnRespondent["Email"].InnerText + "\r\n");
                        try
                        {
                            XmlNode Answers = napi.GetFullAnswersFromRespondent(xnRespondent["AnswerSetId"].InnerText, int.Parse(xn["SurveyId"].InnerText), xn["strGUID"].InnerText);

                            foreach (XmlNode xnAnswer in Answers.SelectNodes("AnswerSets/AnswerSet/Answers/Answer"))
                            {
                                //zum Testen werden die Daten angezeigt

                                tempstring = xnAnswer["QMText"].InnerText;

                                switch (tempstring)
                                {
                                    case "Fahrernummer":
                                        Console.WriteLine("Fahrernummer: " + xnAnswer["AValue"].InnerText);
                                        newRow["LIFNR"] = xnAnswer["AValue"].InnerText;
                                        break;

                                    case "Übergabedatum":
                                        Console.WriteLine("Übergabedatum: " + xnAnswer["AValue"].InnerText);
                                        if (DateTime.TryParse(xnAnswer["AValue"].InnerText, out tempdatum))
                                        {
                                            newRow["DATUM"] = tempdatum.ToString("yyyyMMdd");
                                        }
                                        else
                                        {
                                            newRow["DATUM"] = "";
                                        }
                                        newRow["UZEIT"] = "";
                                        break;

                                    case "Kennzeichen":
                                        Console.WriteLine("Kennzeichen: " + xnAnswer["AValue"].InnerText);
                                        newRow["KENNZ"] = xnAnswer["AValue"].InnerText;
                                        break;

                                    case "A) Freundlichkeit des Fahrers":
                                        Console.WriteLine("A) Freundlichkeit des Fahrers:" + xnAnswer["AValue"].InnerText);
                                        newRow["FRAGE5"] = xnAnswer["AValue"].InnerText;
                                        break;

                                    case "B) Äußeres Erscheinungsbild des Fahrers":
                                        Console.WriteLine("B) Äußeres Erscheinungsbild des Fahrers: " + xnAnswer["AValue"].InnerText);
                                        newRow["FRAGE6"] = xnAnswer["AValue"].InnerText;
                                        break;

                                    case "C) Professionalität / Kompetenz der Fahrzeugübergabe/Fahrzeugübernahme":
                                        Console.WriteLine("C) Professionalität / Kompetenz der Fahrzeugübergabe/Fahrzeugübernahme:" + xnAnswer["AValue"].InnerText);
                                        newRow["FRAGE7"] = xnAnswer["AValue"].InnerText;
                                        break;

                                    case "D) Pünktlichkeit des Fahrers":
                                        Console.WriteLine("D) Pünktlichkeit des Fahrers:" + xnAnswer["AValue"].InnerText);
                                        newRow["FRAGE8"] = xnAnswer["AValue"].InnerText;
                                        break;

                                    case "E) Gesamteindruck der Fahrzeugübergabe / Fahrzeugübernahme":
                                        Console.WriteLine("E) Gesamteindruck der Fahrzeugübergabe / Fahrzeugübernahme:" + xnAnswer["AValue"].InnerText);
                                        newRow["FRAGE9"] = xnAnswer["AValue"].InnerText;
                                        break;

                                    case "A) Einweisung auf das Fahrzeug":
                                        Console.WriteLine("A) Einweisung auf das Fahrzeug:" + xnAnswer["AValue"].InnerText);
                                        newRow["FRAGE10"] = xnAnswer["AValue"].InnerText;
                                        break;

                                    case "B) Fahrzeugzustand bei Übergabe":
                                        Console.WriteLine("B) Fahrzeugzustand bei Übergabe:" + xnAnswer["AValue"].InnerText + "\r\n\r\n");
                                        newRow["FRAGE11"] = xnAnswer["AValue"].InnerText;
                                        break;
                                }
                            }
                        }
                        catch
                        {
                        }

                        var importTableCheck = newRow;
                        ImportTable.Rows.Add(newRow);

                        Console.WriteLine("Rows: " + ImportTable.Rows.Count + "\r\n\r\n");
                    }
                }
            }


            //Übergabe der Tabelle ImportTable an SAP hier

            ERPConnect.LIC.SetLic(ConfigurationManager.AppSettings["ErpConnectLicense"].ToString());

            R3Connection con = new R3Connection(ConfigurationManager.AppSettings["SAPAppServerHost"].ToString(),
                              int.Parse(ConfigurationManager.AppSettings["SAPSystemNumber"].ToString()), 
                              ConfigurationManager.AppSettings["SAPUsername"].ToString(),
                              ConfigurationManager.AppSettings["SAPPassword"].ToString(),
                             "DE", ConfigurationManager.AppSettings["SAPClient"].ToString()); 
            con.Open(false);

            RFCFunction func = con.CreateFunction("Z_UEB_FLEET_INPUT");      
            
            
            RFCTable surveys = func.Tables["IT_DATA"];

            foreach (DataRow xrow in ImportTable.Rows)
            {
                RFCStructure survey = surveys.AddRow();
                foreach (DataColumn xColumn in ImportTable.Columns)
                {
                    survey[xColumn.ColumnName] = xrow[xColumn.ColumnName];
                }
            }

            try    {
                func.Execute();
            }  
            catch (ERPException e)  
            { 
                Console.WriteLine(e.Message);   
                Console.ReadLine(); 
                return;
            } 

            Console.WriteLine("Anzahl der importierten Zeilen:");
            Console.WriteLine(func.Tables["IT_DATA"].Rows.Count);
            int i = 0;
            string dots = "";
            DateTime xPause = DateTime.Now.AddSeconds(10);
            do
            {
                //Pause, damit man was sieht
                System.TimeSpan Zeitspanne = xPause.Subtract(DateTime.Now);
                //Console.Write("\r" + Zeitspanne.Seconds);
                dots = "";
                for (i = 1; i <= Zeitspanne.Seconds; i++) {
                    dots += ".";
                }
                for (i = 1; i <= Zeitspanne.Seconds + 10; i++)
                {
                    dots += " ";
                }

                Console.Write("\r" + dots);
            } while (DateTime.Now < xPause);

        }


    }

}
