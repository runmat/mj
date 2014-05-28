using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Configuration;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using dk.nita.saml20;
using dk.nita.saml20.Bindings;
using dk.nita.saml20.Utils;
//Author Majid salehi (c) Fraunhofer Fokus


    public class AuthnRequestGenerator
    {

        public AuthnRequestGenerator()
        {

        }
        public string ToUTCString(DateTime value)
        {
            return XmlConvert.ToString(value, XmlDateTimeSerializationMode.Utc);
        }
        /// <summary>
        /// Deserializes an xml document back into an object
        /// </summary>
        /// <param name="xml">The xml data to deserialize</param>
        /// <param name="type">The type of the object being deserialized</param>
        /// <returns>A deserialized object</returns>
        public  object Deserialize(XmlDocument xml)
        {
            XmlSerializer s = new XmlSerializer(typeof(AuthnRequestType));
            string xmlString = xml.OuterXml.ToString();
            byte[] buffer = ASCIIEncoding.UTF8.GetBytes(xmlString);
            MemoryStream ms = new MemoryStream(buffer);
            XmlReader reader = new XmlTextReader(ms);
            Exception caught = null;

            try
            {
                object o = s.Deserialize(reader);
                return o;
            }

            catch (Exception e)
            {
                caught = e;
            }
            finally
            {
                reader.Close();

                if (caught != null)
                    throw caught;
            }
            return null;
        }

        /// <summary>
        /// Serializes an object into an Xml Document
        /// </summary>
        /// <param name="o">The object to serialize</param>
        /// <returns>An Xml Document consisting of said object's data</returns>
        public  XmlDocument Serialize(object o)
        {

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();                    
            XmlSerializer s = new XmlSerializer(o.GetType());

            MemoryStream ms = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(ms, new UTF8Encoding());
            writer.Formatting = Formatting.Indented;
            writer.IndentChar = ' ';
            writer.Indentation = 5;
            Exception caught = null;

            try
            {
                s.Serialize(writer, o, namespaces);
                XmlDocument xml = new XmlDocument();
                string xmlString = ASCIIEncoding.UTF8.GetString(ms.ToArray());
                xml.LoadXml(xmlString);
                return xml;
            }
            catch (Exception e)
            {
                caught = e;
            }
            finally
            {
                writer.Close();
                ms.Close();

                if (caught != null)
                    throw caught;
            }
            return null;
        }
   
        public AuthnRequestType CreateAuthnRequestType(String sessionKey)
        {
            AuthnRequestType authnRequestType = new AuthnRequestType();
            try
            {
                authnRequestType.Version = "2.0";             
                //authnRequestType.ID = "id" + Guid.NewGuid().ToString("N");
                authnRequestType.ID = sessionKey;                          

                System.DateTime now = System.DateTime.UtcNow;                               
                authnRequestType.IssueInstant = ToUTCString(now);

                NameIDType issuerID = new NameIDType();
                string IssuerID = ConfigurationSettings.AppSettings["IssuerID"];
                issuerID.Value = IssuerID;
                authnRequestType.Issuer = issuerID;
                authnRequestType.ForceAuthn = false;
                string eIDServerUrl = ConfigurationSettings.AppSettings["eIDServerUrl"];
                authnRequestType.Destination = eIDServerUrl;
                authnRequestType.ProtocolBinding = "urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST";

                RequestAttributesType requestAttributes = new RequestAttributesType();
                requestAttributes.RequestAllDefaults = true;
               

                RequestAttributeType[] requestAttributeArray = new RequestAttributeType[5];

                requestAttributeArray[0] = new RequestAttributeType();
                requestAttributeArray[0].Name = "DateOfBirth";
                requestAttributeArray[0].Required = true;
              
                               
                requestAttributeArray[1] = new RequestAttributeType();
                requestAttributeArray[1].Name = "GivenNames";              
                requestAttributeArray[1].Required = true;

                requestAttributeArray[2] = new RequestAttributeType();
                requestAttributeArray[2].Name = "FamilyNames";
                requestAttributeArray[2].Required = true;


                requestAttributeArray[3] = new RequestAttributeType();
                requestAttributeArray[3].Name = "PlaceOfResidence";               
                requestAttributeArray[3].Required = true;

                requestAttributeArray[4] = new RequestAttributeType();
                requestAttributeArray[4].Name = "PlaceOfBirth";
                requestAttributeArray[4].Required = true;
                
                requestAttributes.RequestAttribute = requestAttributeArray;
             
                System.Xml.XmlElement[] xmlElementArray = new System.Xml.XmlElement[1];
                xmlElementArray[0] = GetXmlElement(requestAttributes);
                ExtensionsType extensionsType = new ExtensionsType();
                extensionsType.Any = xmlElementArray;
                authnRequestType.Extensions = extensionsType;
            }
            catch (Exception ex)
            {
                System.Console.Out.WriteLine(ex.Message);
               
            }
            return authnRequestType;
        }

        XmlElement GetXmlElement(RequestAttributesType requestAttributes)
        {
            XmlElement xmlElement = null;
            try
            {
                //write object to xml string
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(RequestAttributesType));
                StringWriter stringWriter = new StringWriter();
                xmlSerializer.Serialize(stringWriter, requestAttributes);
                String sb = null;
                XmlDocument doc = new XmlDocument();
                sb = stringWriter.ToString();
                doc.LoadXml(sb);
                xmlElement = doc.DocumentElement;
            }
            catch (Exception ex)
            {
                System.Console.Out.WriteLine(ex.Message);
            }
            return xmlElement;
        }
        public XmlDocument CreateAuthenRequest(string sessionKey)
        {

            AuthnRequestGenerator authnRequestGenerator = new AuthnRequestGenerator();
            AuthnRequestType request = authnRequestGenerator.CreateAuthnRequestType(sessionKey);
            
            return GetXml(request);

            //HTTPRedirect(SAMLAction.SAMLRequest, GetXml(request));
        }

        /// <summary>
        /// Gets LogoutResponse as an XmlDocument
        /// </summary>
        /// <returns></returns>
        public XmlDocument GetXml(AuthnRequestType myAuthnRequestType)
        {
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            doc.LoadXml(Serialization.SerializeToXmlString(myAuthnRequestType));
            return doc;
        }

        /// <summary>
        /// Transfers the message to the given endpoint using the HTTP-Redirect binding.
        /// </summary> 
        /// <param name="action">action Object of type SAMLAction</param>
        /// <param name="message">message Object of type XmlNode</param>
        /// <returns></returns>
        public String HTTPRedirect(SAMLAction action, XmlNode message)
        {
            if (message.FirstChild is XmlDeclaration)
                message.RemoveChild(message.FirstChild);

            HttpRedirectBindingBuilder builder = new HttpRedirectBindingBuilder();

            if (action == SAMLAction.SAMLRequest)
                builder.Request = message.OuterXml;
            else
                builder.Response = message.OuterXml;
                      
            string SignCertFilename = ConfigurationSettings.AppSettings["SignCertFilename"];
            string SignCertPasswd = ConfigurationSettings.AppSettings["SignCertPasswd"];
                                            
            //CreateLogFiles Err = new CreateLogFiles();
            //Err.ErrorLog(@"C:\log\", tempStr);
            
            X509Certificate2 sigCert = new X509Certificate2(SignCertFilename, SignCertPasswd, X509KeyStorageFlags.MachineKeySet);
                       
            builder.signingKey = (RSACryptoServiceProvider)sigCert.PrivateKey; ;

            string eIDServerUrl = ConfigurationSettings.AppSettings["eIDServerUrl"];

            UriBuilder url = new UriBuilder(eIDServerUrl);
            url.Query = builder.ToQuery();
            return url.ToString();
            
           // HttpContext.Current.Response.Redirect(url.ToString(), true);
        }

    }

