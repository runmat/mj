<%@ WebHandler Language="C#" Class="ResponseHandler" %>

//Author Majid salehi (c) Fraunhofer Fokus

using System;
using System.Web;
using System.Configuration;
using System.Web.SessionState;
using System.Xml;
using System.IO;

public class ResponseHandler : IHttpHandler, IRequiresSessionState
{


    /// <summary>
    /// This method handles the authentication response triggered by the action in the service method
    /// </summary>
    /// <param name="context">context Object of type HttpContext</param>
    /// <returns></returns>
    public void ProcessRequest(HttpContext context)
    {

        string message = string.Empty;
        System.String htmlHeader = "<html><head><title>ASPNET Example ASPX .NetFramework 3.5 eID Connector</title></head><body>";
        System.String htmlFooter = "</body></html>";
        System.String htmlSuccessStart = "<h1 align=\"center\">";
        System.String htmlSuccess = "Fraunhofer FOKUS</h1><br /><br/>";
        htmlSuccess += "<b><p>Received Attributes</p></b><br><br>";
        System.String htmlSuccessEnd = "<br/>";


        eIDConnector.Response.AssertionData eidId = eIDConnector.Response.SAMLResponseHandler.getAssertionData(context,
                                                                                 ConfigurationSettings.AppSettings["EncrCertFilename"],
                                                                                 ConfigurationSettings.AppSettings["EncrCertPasswd"]);

        String PlaceOfResidence = "", DateOfBirth = "", DocumentValidity = "", GivenNames = "", FamilyNames = "", ArtisticName = "", AcademicTitle = "", IssuingState = "", PlaceOfBirth = "", DocumentType = "", RequestID = "", RestrictedIdentification = "";

        //RestrictedIdentification
        if (eidId.RestrictedIdentification != null)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            RestrictedIdentification = enc.GetString(eidId.RestrictedIdentification);
            htmlSuccess += ("Restricted Identification: " + "<b>" + RestrictedIdentification + "</b><br />");
        }
        //RequestID
        if (!String.IsNullOrEmpty(eidId.RequestID))
        {
            RequestID = eidId.RequestID;
            htmlSuccess += ("Request ID: " + "<b>" + RequestID + "</b><br />");
        }
        //GivenNames
        if (!String.IsNullOrEmpty(eidId.GivenNames))
        {
            GivenNames = eidId.GivenNames;
            htmlSuccess += ("Given Names: " + "<b>" + GivenNames + "</b><br />");
        }

        //FamilyNames
        if (!String.IsNullOrEmpty(eidId.FamilyNames))
        {
            FamilyNames = eidId.FamilyNames;
            htmlSuccess += ("Family Names: " + "<b>" + FamilyNames + "</b><br />");
        }

        //PlaceOfResidence    
        if (eidId.PlaceOfResidence != null)
        {
            PlaceOfResidence = eidId.PlaceOfResidence.Country + "," + eidId.PlaceOfResidence.State + "," + eidId.PlaceOfResidence.City + "," + eidId.PlaceOfResidence.Street + "," + eidId.PlaceOfResidence.ZipCode;
            htmlSuccess += ("Place of Residence: " + "<b>" + PlaceOfResidence + "</b><br />");
        }

       
        
        //PlaceOfBirth            
         if (!String.IsNullOrEmpty(eidId.PlaceOfBirthFreetextPlace))
        {   
            PlaceOfBirth = eidId.PlaceOfBirthFreetextPlace;     
            htmlSuccess += ("Place Of Birth: " + "<b>" + eidId.PlaceOfBirthFreetextPlace + "</b><br />");
        }

        //DateOfBirth
        if (eidId.DateOfBirth.ToShortDateString() != "1/1/0001")
        {
            DateOfBirth = eidId.DateOfBirth.ToShortDateString();
            htmlSuccess += ("Date of Birth: " + "<b>" + DateOfBirth + "</b><br />");

        }


        //DocumentValidity
        DocumentValidity = "not valid at " + DateTime.Now.ToShortDateString();
        if (eidId.DocumentValidity != null)
        {
            DocumentValidity = eidId.DocumentValidity.Status + " " + eidId.DocumentValidity.ReferenceDate;
            htmlSuccess += ("Document Validity: " + "<b>" + DocumentValidity + "</b><br />");
        }

        //AgeVerification
        if (eidId.AgeVerification != null)
        {
            htmlSuccess += ("AgeVerification: Over " + "<b>" + eidId.AgeVerification.Request + "</b>" + " is " + "<b>" + eidId.AgeVerification.Result + "</b><br />");
        }

        //ArtisticName
        if (!String.IsNullOrEmpty(eidId.ArtisticName))
        {
            ArtisticName = eidId.ArtisticName;
            htmlSuccess += ("Artistic Name: " + "<b>" + ArtisticName + "</b><br />");
        }

        //AcademicTitle
        if (!String.IsNullOrEmpty(eidId.AcademicTitle))
        {
            AcademicTitle = eidId.AcademicTitle;
            htmlSuccess += ("Academic Title: " + "<b>" + AcademicTitle + "</b><br />");
        }

        //IssuingState
        if (!String.IsNullOrEmpty(eidId.IssuingState))
        {
            IssuingState = eidId.IssuingState;
            htmlSuccess += ("Issuing State: " + "<b>" + IssuingState + "</b><br />");
        }

        //IssuingState
        if (!String.IsNullOrEmpty (eidId.DocumentType))
        {
            DocumentType = eidId.DocumentType;
            htmlSuccess += ("Document Type: " + "<b>" + DocumentType + "</b><br />");
        }
        htmlSuccess += ("<br>");


        //XmlDocument xmlDom = new XmlDocument();


        XmlDocument xmlDom = eIDConnector.Response.SAMLResponseHandler.getAssertionXmlDocument(context,
                                                                        ConfigurationSettings.AppSettings["EncrCertFilename"],
                                                                        ConfigurationSettings.AppSettings["EncrCertPasswd"]);



        StringWriter sw = new StringWriter();
        XmlTextWriter xw = new XmlTextWriter(sw);
        xmlDom.WriteTo(xw);

        string SapString;

        SapString = sw.ToString(); 
        
        
        Receiver.DataAccess da = new Receiver.DataAccess();

       
        da.SaveXml(SapString, RequestID);

        // Testserver
        //context.Response.Redirect("https://sgwt.kroschke.de/Services/" + "(S(" + RequestID + "))" + "/Components/ComCommon/Beauftragung/Change02s.aspx" + 
        context.Response.Redirect("https://sgwt.kroschke.de/Services/" + "(S(" + RequestID + "))" + "/Components/ComCommon/Beauftragung2/Change02s.aspx" + 
        "?eIdentityResponse=" + "true" + 
        "&GivenNames=" + context.Server.UrlEncode(GivenNames) + 
        "&FamilyNames=" + context.Server.UrlEncode(FamilyNames) + 
        "&PlaceOfResidence=" + context.Server.UrlEncode(PlaceOfResidence) + 
        "&DateOfBirth=" + context.Server.UrlEncode(DateOfBirth) + 
        "&PlaceOfBirth=" + context.Server.UrlEncode(PlaceOfBirth) + 
       "&AppID=1596");   //sgwt Beauftragung2 Change02s
        //"&AppID=1321"); //sgwt Beauftragung Change02s

      //  // Lokaler Test
      //  //context.Response.Redirect("http://localhost/Services/" + "(S(" + RequestID + "))" + "/Components/ComCommon/Beauftragung/Change02s.aspx" +
      //  context.Response.Redirect("http://localhost/Services/" + "(S(" + RequestID + "))" + "/Components/ComCommon/Beauftragung2/Change02s.aspx" +
      //"?eIdentityResponse=" + "true" +
      //"&GivenNames=" + context.Server.UrlEncode(GivenNames) +
      //"&FamilyNames=" + context.Server.UrlEncode(FamilyNames) +
      //"&PlaceOfResidence=" + context.Server.UrlEncode(PlaceOfResidence) +
      //"&DateOfBirth=" + context.Server.UrlEncode(DateOfBirth) +
      //"&PlaceOfBirth=" + context.Server.UrlEncode(PlaceOfBirth) +
      ////"&AppID=1423"); //local Beauftragung Change02s
      //"&AppID=1721"); //local Beauftragung2 Change02s

        //------------[ html output ]----------
        //htmlSuccess += ("<br><form action=\"" + context.Request.ApplicationPath + "/Default.aspx\" method=\"post\">" + "<input name\"demo\" value=\"back to main page\" type=\"submit\"/>" + "</form>");
        //System.String pageString = htmlHeader + htmlSuccessStart + htmlSuccess + htmlSuccessEnd + htmlFooter;
        //printPage(context.Response, pageString);
    }


    /// <summary>
    /// Print tha page in html format in Response
    /// </summary>
    /// <param name="res">The HttpResponse</param>
    /// <param name="content">the html string</param>
    /// <returns>notthing</returns>
    private void printPage(System.Web.HttpResponse res, System.String content)
    {
        res.ContentType = "text/html";
        System.IO.TextWriter writer = res.Output;
        writer.WriteLine(content);
    }


    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
    

}