using System;
using System.Xml;

/// Summary description for EIDIdentity
//Author Majid salehi (c) Fraunhofer Fokus
/// 
namespace epa.elan
{
    public class EIDIdentity
    {

        public EIDIdentity()
        {
        }

        public EIDIdentity(XmlDocument assertionDoc)
        {
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(assertionDoc.NameTable);
            nsMgr.AddNamespace("saml", "urn:oasis:names:tc:SAML:2.0:assertion");
            nsMgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            nsMgr.AddNamespace("idp", "http://bsi.bund.de/eID/");
            nsMgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");
            nsMgr.AddNamespace("def", "http://bsi.bund.de/eID/");

            XmlNodeList nodes = assertionDoc.SelectNodes("/saml:Assertion/saml:AttributeStatement", nsMgr);

            foreach (XmlNode nodeAttributeStatement in nodes)
            {
                foreach (XmlNode node in nodeAttributeStatement.ChildNodes)
                {
                    String attribute = node.Attributes[0].Value;
                    switch (attribute)
                    {
                        case "DateOfBirth":
                            DateTime dt; dt = Convert.ToDateTime(node.InnerText);
                            DateOfBirth = dt;
                            break;
                        case "FamilyNames":
                            FamilyNames = node.InnerText;
                            break;
                        case "GivenNames":
                            GivenNames = node.InnerText;
                            break;
                        case "AcademicTitle":
                            AcademicTitle = node.InnerText;
                            break;
                        case "CommunityId":
                            CommunityID = node.InnerText;
                            break;
                        case "CommunityVerification":
                            break;
                        case "IssuingState":
                            IssuingState = node.InnerText;
                            break;
                        case "DateOfExpiry":
                            DateOfExpiry = node.InnerText;
                            break;
                        case "DocumentType":
                            DocumentType = node.InnerText;
                            break;
                        case "ArtisticName":
                            ArtisticName = node.InnerText;
                            break;
                        case "AgeVerification":
                            String strValueResult = "false", strValueRequest = "18";
                            foreach (XmlNode cNode in node.ChildNodes)
                            {
                                foreach (XmlNode cCNode in cNode.ChildNodes)
                                {
                                    if (cCNode.Name == "Request")
                                        strValueRequest = cCNode.InnerXml;
                                    if (cCNode.Name == "Result")
                                        strValueResult = cCNode.InnerXml;
                                }
                            }
                            AgeVerification = new AgeVerificationResultType
                            {
                                Request = ushort.Parse(strValueRequest),
                                Result = strValueResult == "true"
                            };
                            break;
                        case "DocumentValidity":
                            String strValueValidityResult = "not valid", strValueValidityDateResult = DateTime.Now.ToShortDateString();
                            foreach (XmlNode cNode in node.ChildNodes)
                            {
                                foreach (XmlNode cCNode in cNode.ChildNodes)
                                {
                                    if (cCNode.Name == "eid:ReferenceDate")
                                        strValueValidityDateResult = cCNode.InnerXml;
                                    if (cCNode.Name == "eid:Status")
                                        strValueValidityResult = cCNode.InnerXml;
                                }
                            }
                            DocumentValidity = new DocumentValidityResultType
                            {
                                ReferenceDate = Convert.ToDateTime(strValueValidityDateResult),
                                Status = strValueValidityResult
                            };
                            break;
                        case "PlaceOfResidence":
                            foreach (XmlNode cCNode in node.ChildNodes)
                            {
                                if (cCNode.Name == "saml:AttributeValue")
                                    foreach (XmlNode ccCNode in cCNode.ChildNodes)
                                    {
                                        if (ccCNode.Name == "eid:State")
                                            State = ccCNode.InnerXml;
                                        if (ccCNode.Name == "eid:Country")
                                            Country = ccCNode.InnerXml;
                                        if (ccCNode.Name == "eid:City")
                                            City = ccCNode.InnerXml;
                                        if (ccCNode.Name == "eid:Street")
                                            Street = ccCNode.InnerXml;
                                    }
                            }

                            PlaceOfResidence = new PlaceType
                            {
                                Street = Street,
                                City = City,
                                State = State,
                                Country = Country
                            };

                            break;
                        case "PlaceOfBirth":
                            foreach (XmlNode cCNode in node.ChildNodes)
                            {
                                if (cCNode.Name == "saml:AttributeValue")
                                    foreach (XmlNode ccCNode in cCNode.ChildNodes)
                                    {
                                        if (ccCNode.Name == "eid:State")
                                            State = ccCNode.InnerXml;
                                        if (ccCNode.Name == "eid:Country")
                                            Country = ccCNode.InnerXml;
                                        if (ccCNode.Name == "eid:City")
                                            City = ccCNode.InnerXml;
                                        if (ccCNode.Name == "eid:Street")
                                            Street = ccCNode.InnerXml;
                                    }
                            }

                            PlaceOfBirth = new PlaceType
                            {
                                Street = Street,
                                City = City,
                                State = State,
                                Country = Country
                            };

                            break;
                        default:
                            break;
                    }
                }
            }
        }



        public string AcademicTitle { get; set; }
        private string Street { get; set; }
        private string State { get; set; }
        private string City { get; set; }
        private string Country { get; set; }
        public AgeVerificationResultType AgeVerification { get; set; }
        public string ArtisticName { get; set; }
        public string CommunityID { get; set; }
        public String CommunityVerification { get; set; }
        public DateTime DateOfBirth { get; set; }
        public String DateOfExpiry { get; set; }
        public string DocumentType { get; set; }
        public DocumentValidityResultType DocumentValidity { get; set; }
        public string FamilyNames { get; set; }
        public string GivenNames { get; set; }
        public string IssuingState { get; set; }
        public PlaceType PlaceOfBirth { get; set; }
        public PlaceType PlaceOfResidence { get; set; }
        public byte[] RestrictedIdentification { get; set; }
    }

}