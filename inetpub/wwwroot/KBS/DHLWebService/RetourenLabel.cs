using System;
using System.Text;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Collections;
using System.Xml;

namespace DHLWebService
{
    public class RetourenLabel
    {

        private string sError = string.Empty;

        private Stopwatch stopwatch = new Stopwatch();
        private Stream requestStream = null;
        private WebRequest request;
        private WebResponse response;
        private string[] strArray = new string[8];
        private string sEncoding = "UTF-8";
        private string sVersion = "1.0";
        private string sRequestUriString = "https://amsel.dpwn.net/abholportal/gw/lp/SoapConnector";
        private string sSoepEnv = "http://schemas.xmlsoap.org/soap/envelope/";
        private string sVar3bl = "https://amsel.dpwn.net/abholportal/gw/lp/schema/1.0/var3bl";
        private string sWss = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
        private string sPassText = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText";

        private string strHtml = string.Empty;

        #region Properties

        public string User { get; set; }
        public string Password { get; set; }
        public string PortalID { get; set; }
        public string DeliveryName { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ResponseString { get; private set; }
        public string BoxNumber { get; private set; }
        public string ShipentReference { get; set; }
        public string CustomerRefeence { get; set; }
        public string ReturnPath { get; set; }
        public string CareOfName { get; private set; }

        public bool IsSaveRequestFile { get; set; }

        public string LabelExtension { get; private set; }
        public string RequestExtension { get; private set; }
        public string FullPathToLabel { get; private set; }


        public string xmlFilePath;
        string innerText = string.Empty;

        #endregion

        #region CTOR

        public RetourenLabel()
        {
            ////Testzugang DHL
            //this.User = "ws_online_retoure";
            //this.Password = "Anfang1!";
            //this.PortalID = "OnlineRetoure";
            //this.DeliveryName = "Deutschland_Var3";

            //Zugang DAD
            this.User = "webdad";
            this.Password = "dad2012!";
            this.PortalID = "dad";
            this.DeliveryName = "RetourenWeb01";

            this.IsSaveRequestFile = false;
            this.LabelExtension = ".pdf";
            this.RequestExtension = ".xml";

            this.FullPathToLabel = string.Empty;
            this.ReturnPath = "c:/Temp";


            this.Name1 = "Christoph Kroschke GmbH";
            this.Name2 = "";
            this.Street = "Ladestraße";
            this.StreetNumber = "1";
            this.PostalCode = "22926";
            this.City = "Ahrensburg";
            this.Email = "MaxMusterman@mustersonstwie1256262484.de";
            this.Phone = "040 22334455";
            this.ShipentReference = "";
            this.CustomerRefeence = "";
            this.CareOfName = "CareofName";
            this.BoxNumber = string.Empty;


        }

        #endregion

        public string DoService(out string error)
        {
            //Zeitstempel wird für Dateinamen verwendet
            string timeStamp = string.Format("{0:yyyyMMdd_hhmmss}", DateTime.Now);
            error = string.Empty;

            //request xml String erzeugen
            try
            {

                strHtml +=
                  "<?xml version=\"" + sVersion + "\" encoding=\"" + sEncoding + "\"?>" +
                  "<soapenv:Envelope xmlns:soapenv=\"" + sSoepEnv + "\" xmlns:var=\"" + sVar3bl + "\">" +
                  "<soapenv:Header><wsse:Security soapenv:mustUnderstand=\"1\" xmlns:wsse=\"" + sWss + "\">" +
                  "<wsse:UsernameToken><wsse:Username>" + User + "</wsse:Username>" +
                  "<wsse:Password Type=\"" + sPassText + "\">" + Password + "</wsse:Password>" +
                  "</wsse:UsernameToken>" +
                  "</wsse:Security>" +
                  "</soapenv:Header>" +
                  "<soapenv:Body>" +
                  "<var:BookLabelRequest portalId =\"" + PortalID + "\" deliveryName=\"" + DeliveryName + "\" shipmentReference=\"" + ShipentReference + "\"" +
                  " customerReference=\"" + CustomerRefeence + "\" " +
                  "labelFormat=\"PDF\" senderName1=\"" + Name1 + "\" senderName2=\"" + Name2 + "\"" +
                  " senderCareOfName=\"" + CareOfName + "\" senderContactPhone=\"" + Phone + "\" senderStreet=\"" + Street + "\" senderStreetNumber=\"" + StreetNumber + "\" " +
                  "senderBoxNumber=\"" + BoxNumber + "\" senderPostalCode=\"" + PostalCode + "\" senderCity=\"" + City + "\"/>" +
                  "</soapenv:Body></soapenv:Envelope>";

                string filename = timeStamp + "_DHLRetourenlabel";
                xmlFilePath = ReturnPath + "/" + filename + RequestExtension;

                //Prüfen ob Datei vorhanden sonst umbenennen
                for (int i = 2; i < 10000; i++)
                {
                    if (!File.Exists(xmlFilePath))
                    {
                        break;
                    }

                    xmlFilePath = ReturnPath + "/" + filename + "_" + i + RequestExtension;
                }

                // XML strucktur Speichern wenn gewünscht
                if (IsSaveRequestFile)
                {
                    StreamWriter writer = new StreamWriter(xmlFilePath);
                    writer.WriteLine(strHtml);
                    writer.Flush();
                    writer.Close();
                    writer = null;
                }


                // request erstellen
                request = WebRequest.Create(sRequestUriString);
                request.Method = "POST";
                request.ContentType = "application/xml";

                byte[] bytes = Encoding.UTF8.GetBytes(strHtml);

                //TODO entfernen von return für Produktiv / Test
                //return "C:/temp/20120807_104721_RetourenLabel.pdf";

                request.ContentLength = bytes.Length;
                requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                stopwatch.Start();

            }
            catch (Exception ex1)
            {
                error = "Folgender Fehler ist aufgetreten:\n" + ex1.Message;
                return string.Empty;

            }
            finally
            {
                if (requestStream != null)
                    requestStream.Close();

            }

            try
            {

                IEnumerator enumerator;

                response = (HttpWebResponse)request.GetResponse();
                ResponseString = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8")).ReadToEnd();
                stopwatch.Stop();


                StreamWriter writer2 = new StreamWriter(xmlFilePath + "_Out.xml");
                writer2.WriteLine(ResponseString);
                writer2.Flush();
                writer2.Close();
                writer2 = null;


                XmlDocument document = new XmlDocument();
                document.LoadXml(ResponseString);


                XmlTextReader reader2 = new XmlTextReader(xmlFilePath);
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(reader2.NameTable);
                nsmgr.AddNamespace("var3bl", "https://amsel.dpwn.net/abholportal/gw/lp/schema/1.0/var3bl");

                XmlNodeList elementsByTagName = document.GetElementsByTagName("var3bl:BookLabelResponse");

                // Rückgabewerte in ein Array schreiben
                try
                {

                    enumerator = elementsByTagName.GetEnumerator();

                    while (enumerator.MoveNext())
                    {
                        IEnumerator enumerator2;
                        XmlNode current = (XmlNode)enumerator.Current;
                        innerText = current.SelectSingleNode("var3bl:label", nsmgr).InnerText;

                        try
                        {
                            enumerator2 = current.Attributes.GetEnumerator();
                            while (enumerator2.MoveNext())
                            {
                                XmlAttribute attribute = (XmlAttribute)enumerator2.Current, XmlAttribute;
                                if (attribute.Name == "idc")
                                {
                                    strArray[0] = attribute.Value;
                                }

                                if (attribute.Name == "routingCode")
                                {
                                    strArray[1] = attribute.Value;
                                }

                                if (attribute.Name == "faultstring")
                                {
                                    strArray[2] = attribute.Value;
                                }
                            }
                        }
                        finally
                        {
                        }
                    }

                    if (innerText == "")
                    {

                        error = "Label konnte nicht empfangen werden.";
                        return string.Empty;

                    }
                    else
                    {


                        FullPathToLabel = ReturnPath + "/" + timeStamp + "_RetourenLabel" + LabelExtension;

                        for (int i = 2; i < 100000; i++)
                        {
                            if (!File.Exists(FullPathToLabel))
                            {
                                break;
                            }

                            FullPathToLabel = ReturnPath + "/" + timeStamp + "_RetourenLabel_" + i + LabelExtension;
                        }


                        byte[] array = Convert.FromBase64String(innerText);

                        FileStream stream2 = File.Create(FullPathToLabel);
                        stream2.Write(array, 0, array.Length);
                        stream2.Close();

                        if (File.Exists(xmlFilePath + "_Out.xml"))
                        {

                            try
                            {
                                File.Delete(xmlFilePath + "_Out.xml");
                            }
                            catch
                            {

                            }

                        }

                        return FullPathToLabel;

                    }

                }
                finally
                {

                }



            }
            catch (Exception ex)
            {

                error = "Fehler" + ex.Message;
                return string.Empty;
            }

        }//end func


    }//end class
}//end ns

