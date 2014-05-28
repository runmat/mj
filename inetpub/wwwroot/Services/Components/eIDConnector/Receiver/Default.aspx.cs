using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Receiver
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("https://sgwt.kroschke.de/Services/Components/eIDConnector/Receiver/ResponseHandler.ashx");
        }
    }
}
