using System;

public partial class LogonMessage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["LOGONMSGDATA"] != null)
        {
            lblMessage.Text = Session["LOGONMSGDATA"].ToString().Replace("\r\n", "<br/>");
        }
    }
}
