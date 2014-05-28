using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using dk.nita.saml20;
using System.Xml;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace nPaService
{
    /// <summary>
    /// Zusammenfassungsbeschreibung für Service1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Um das Aufrufen dieses Webdiensts aus einem Skript mit ASP.NET AJAX zuzulassen, heben Sie die Auskommentierung der folgenden Zeile auf. 
    // [System.Web.Script.Services.ScriptService]
    public class DataService : System.Web.Services.WebService
    {

        public string AcademicTitle { get; set; }
        private string Street { get; set; }
        private string State { get; set; }
        private string City { get; set; }
        private string Country { get; set; }
        public string ArtisticName { get; set; }
        public string CommunityID { get; set; }
        public String CommunityVerification { get; set; }
        public DateTime DateOfBirth { get; set; }
        public String DateOfExpiry { get; set; }
        public string DocumentType { get; set; }
        public string FamilyNames { get; set; }
        public string GivenNames { get; set; }
        public string IssuingState { get; set; }
        public PlaceType PlaceOfBirth { get; set; }
        public PlaceType PlaceOfResidence { get; set; }
        public byte[] RestrictedIdentification { get; set; }




        [WebMethod]
        public adress GetAdress(String User, String Password, String EncryptedData)
        {
            adress ad = new adress();

            if (ConfigurationManager.AppSettings["Username"] != User || ConfigurationManager.AppSettings["Password"] != Password)
            {
                ad.ErrorMessage = "Username oder Password nicht korrekt.";
                return ad;
            }


            //XmlDocument NewXDoc = new XmlDocument();

            try
            {

            


            //NewXDoc.Load(new StringReader(EncryptedData));


            //string EncrCertFilename = ConfigurationSettings.AppSettings["EncrCertFilename"];
            //string EncrCertPasswd = ConfigurationSettings.AppSettings["EncrCertPasswd"];

            //X509Certificate2 cert = new X509Certificate2(EncrCertFilename, EncrCertPasswd, X509KeyStorageFlags.MachineKeySet);

            //Saml20EncryptedAssertion encryptedAssertion = new Saml20EncryptedAssertion((RSA)cert.PrivateKey, NewXDoc);
            //encryptedAssertion.Decrypt();


            //XmlElement xmlAssertion = encryptedAssertion.Assertion.DocumentElement;

            //XmlDocument assertionDoc = new XmlDocument();
            //assertionDoc.LoadXml(xmlAssertion.OuterXml);

            //XmlNamespaceManager nsMgr = new XmlNamespaceManager(assertionDoc.NameTable);
            //nsMgr.AddNamespace("saml", "urn:oasis:names:tc:SAML:2.0:assertion");
            //nsMgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            //nsMgr.AddNamespace("idp", "http://bsi.bund.de/eID/");
            //nsMgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");
            //nsMgr.AddNamespace("def", "http://bsi.bund.de/eID/");

            //XmlNodeList nodes = assertionDoc.SelectNodes("/saml:Assertion/saml:AttributeStatement", nsMgr);

            //foreach (XmlNode nodeAttributeStatement in nodes)
            //{
            //    foreach (XmlNode node in nodeAttributeStatement.ChildNodes)
            //    {
            //        String attribute = node.Attributes[0].Value;
            //        switch (attribute)
            //        {
            //            case "DateOfBirth":
            //                DateTime dt; dt = Convert.ToDateTime(node.InnerText);
            //                DateOfBirth = dt;
            //                break;
            //            case "FamilyNames":
            //                FamilyNames = node.InnerText;
            //                break;
            //            case "GivenNames":
            //                GivenNames = node.InnerText;
            //                break;
            //            case "PlaceOfResidence":
            //                foreach (XmlNode cCNode in node.ChildNodes)
            //                {
            //                    if (cCNode.Name == "saml:AttributeValue")
            //                        foreach (XmlNode ccCNode in cCNode.ChildNodes)
            //                        {
            //                            if (ccCNode.Name == "md:State")
            //                                State = ccCNode.InnerXml;
            //                            if (ccCNode.Name == "md:Country")
            //                                Country = ccCNode.InnerXml;
            //                            if (ccCNode.Name == "md:City")
            //                                City = ccCNode.InnerXml;
            //                            if (ccCNode.Name == "md:Street")
            //                                Street = ccCNode.InnerXml;
            //                        }
            //                }

            //                PlaceOfResidence = new PlaceType
            //                {
            //                    Street = Street,
            //                    City = City,
            //                    State = State,
            //                    Country = Country
            //                };

            //                break;
            //            case "PlaceOfBirth":
            //                foreach (XmlNode cCNode in node.ChildNodes)
            //                {
            //                    if (cCNode.Name == "saml:AttributeValue")
            //                        foreach (XmlNode ccCNode in cCNode.ChildNodes)
            //                        {
            //                            if (ccCNode.Name == "md:State")
            //                                State = ccCNode.InnerXml;
            //                            if (ccCNode.Name == "md:Country")
            //                                Country = ccCNode.InnerXml;
            //                            if (ccCNode.Name == "md:City")
            //                                City = ccCNode.InnerXml;
            //                            if (ccCNode.Name == "md:Street")
            //                                Street = ccCNode.InnerXml;
            //                        }
            //                }

            //                PlaceOfBirth = new PlaceType
            //                {
            //                    Street = Street,
            //                    City = City,
            //                    State = State,
            //                    Country = Country
            //                };

            //                break;
            //            default:
            //                break;
            //        }
            //    }
            //}


                EncryptedData = Decrypt(EncryptedData);


                string[] DecryptedData = EncryptedData.Split('|');

                ad.GivenNames = DecryptedData[0].ToString();
                ad.FamilyNames = DecryptedData[1].ToString();
                ad.Street = DecryptedData[2].ToString();
                ad.PostalCode = DecryptedData[3].ToString();
                ad.City = DecryptedData[4].ToString();
                ad.DateOfBirth = DecryptedData[5].ToString();
                ad.PlaceOfBirth = DecryptedData[6].ToString();


            //ad.DateOfBirth = DateOfBirth.ToShortDateString();
            //ad.FamilyNames = FamilyNames;
            //ad.GivenNames = GivenNames;
            //ad.PlaceOfBirth = PlaceOfBirth.City;
            //ad.Street = PlaceOfResidence.Street;
            //ad.City = PlaceOfResidence.City;
            }
            catch (Exception)
            {
                ad.DateOfBirth = "";
                ad.FamilyNames = "";
                ad.GivenNames = "";
                ad.PlaceOfBirth = "";
                ad.Street = "";
                ad.PostalCode = "";
                ad.City = "";
                ad.ErrorMessage = "Inkorrekte Datenübergabe";
               
            }
            return ad;
        }

        String Decrypt(String EncryptedString)
        {
            String DecryptedString;


            RijndaelManaged rd = new RijndaelManaged();
            int rijndaelIvLength = 16;
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] key = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes("@S33wolf"));

            md5.Clear();

            byte[] encdata = Convert.FromBase64String(EncryptedString);
            MemoryStream ms = new MemoryStream(encdata);
            byte[] iv = new byte[16];

            ms.Read(iv, 0, rijndaelIvLength);
            rd.IV = iv;
            rd.Key = key;

            CryptoStream cs = new CryptoStream(ms, rd.CreateDecryptor(), CryptoStreamMode.Read);


            int DataLength = ((int)ms.Length - rijndaelIvLength);

            byte[] Data = new byte[DataLength + 1];

            int i = cs.Read(Data, 0, Data.Length);

            DecryptedString = System.Text.Encoding.UTF8.GetString(Data, 0, i);
            cs.Close();
            rd.Clear();

            return DecryptedString;

        }

    }


 


    public partial class PlaceType
    {

        private string streetField;

        private string cityField;

        private string stateField;

        private string countryField;

        /// <remarks/>
        public string Street
        {
            get
            {
                return this.streetField;
            }
            set
            {
                this.streetField = value;
            }
        }

        /// <remarks/>
        public string City
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
            }
        }

        /// <remarks/>
        public string State
        {
            get
            {
                return this.stateField;
            }
            set
            {
                this.stateField = value;
            }
        }

        /// <remarks/>
        public string Country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }
    }




}
