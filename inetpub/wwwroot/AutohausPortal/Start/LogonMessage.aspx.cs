using System;
using System.Web.UI;

public partial class LogonMessage : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["LOGONMSGDATA"] != null)
        {
            lblMessage.Text = Session["LOGONMSGDATA"].ToString().Replace("\r\n", "<br/>");
        }
    }
}
