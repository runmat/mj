using System;
using System.Web.SessionState;
using System.Configuration;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Xml;
using dk.nita.saml20;
using dk.nita.saml20.Bindings;
using dk.nita.saml20.Utils;



using CKG.Base.Business;
using CKG.Base.Kernel;
using CKG.Base.Kernel.Common;

//Author Majid salehi (c) Fraunhofer Fokus


public partial class _Default : System.Web.UI.Page

{

    CKG.Base.Kernel.Security.User m_User;
    CKG.Base.Kernel.Security.App m_App;


    private void RemoveRedirCookie()
    {
        Response.Cookies.Add(new HttpCookie("__CARID__", ""));
    } 

    private void AddRedirCookie()
    {

        Response.Cookies.Add(new HttpCookie("__CARID__", TextBoxCarId.Text));
    }
    public static void TheSessionId(String key, Object value)
    {
        HttpSessionState ss = HttpContext.Current.Session;
        HttpContext.Current.Session[key] = value;
          
    }
    protected void Page_Load(object sender, EventArgs e)
    {

        m_User = Common.GetUser(this);
        Common.FormAuth(this, m_User);
        Common.GetAppIDFromQueryString(this);
        m_App = new CKG.Base.Kernel.Security.App(m_User);

        //CreateAuthenRequest();
       
      
       
        //Response.Write("Session: " + Session.SessionID + ": " + Session["carid"]);

        //CreateLogFiles Err = new CreateLogFiles();
        //Err.ErrorLog(Server.MapPath("Logs/ErrorLog"), "Page_Load..");
    }

    protected void ButtonCreateSamlReq_Click(object sender, EventArgs e)
    {
        CreateAuthenRequest();
    }


    
    /// <summary>
    /// This method handles the authentication request from the user. is used to generate the authentication request.
    /// </summary>
    
    private void CreateAuthenRequest()
    {        
        AuthnRequestGenerator authnRequestGenerator = new AuthnRequestGenerator();
        AuthnRequestType request = authnRequestGenerator.CreateAuthnRequestType("test");
        TheSessionId("request.ID", request.ID);
        TheSessionId("request", request);
        HTTPRedirect(SAMLAction.SAMLRequest, GetXml(request));
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
    protected  void HTTPRedirect(SAMLAction action, XmlNode message)
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
        X509Certificate2 sigCert = new X509Certificate2(SignCertFilename,
                SignCertPasswd, X509KeyStorageFlags.Exportable);
        builder.signingKey = (RSACryptoServiceProvider)sigCert.PrivateKey; ;

        string eIDServerUrl = ConfigurationSettings.AppSettings["eIDServerUrl"];
        UriBuilder url = new UriBuilder(eIDServerUrl);
        url.Query = builder.ToQuery();

        AddRedirCookie();

        HttpContext.Current.Response.Redirect(url.ToString(), true);
    }
}
