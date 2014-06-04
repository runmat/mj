using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using org.fokus.npa.connector;

namespace nPa
{
    public partial class Start : System.Web.UI.Page
    {

        string URL;

        protected void Page_Load(object sender, EventArgs e)
        {

            URL = this.Context.Request.UrlReferrer.ToString();

            NpaInit();
            NpaSendRequest();


        }


        protected void NpaInit()
        {
            String strPath = @"C:\inetpub\wwwroot\nPa\eIDConfig.xml";

            org.fokus.npa.connector.eIDConnector.getInstance().Init(strPath);

        }

        protected void NpaSendRequest()
        {
            org.fokus.npa.connector.eIDConnector myConnector = org.fokus.npa.connector.eIDConnector.getInstance();
            eIDHandler handler = myConnector.createHandler("basicProfile");

            //add it to the current session

            System.Web.HttpContext.Current.Session.Add("eIDHandler", handler);
            System.Web.HttpContext.Current.Session.Add("ReturnURL", URL);

            handler.handleRequest();

        }



    }
}
