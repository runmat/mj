using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using AutohausPortal.lib;

namespace AutohausPortal.MasterPage
{
    public partial class Login : System.Web.UI.MasterPage
    {
        private CKG.Base.Kernel.Security.User m_User;

        protected void Page_Load(object sender, EventArgs e)
        {
            string strLogoPath = "";
            string strLogoPath2 = "";
            string strDocuPath = "";
            string strTitle = null;
            HttpBrowserCapabilities bc = default(HttpBrowserCapabilities);
            bc = Request.Browser;
            String Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            lblCopyright.Text = "© " + DateTime.Now.Year.ToString() + " Christoph Kroschke GmbH " + " vers." + Version;
            
            //m_User = (CKG.Base.Kernel.Security.User)Session["objUser"];
            m_User = MVC.GetSessionUserObject();
           
            lnkImpressum.NavigateUrl = "/AutohausPortal/(S(" + Session.SessionID + "))/Info/ImpressumLogin.aspx";
            lnkKontakt.NavigateUrl = "/AutohausPortal/(S(" + Session.SessionID + "))/Info/KontaktLogin.aspx";

            if (HttpContext.Current.Request.UserAgent != null && HttpContext.Current.Request.UserAgent.ToLower().Contains("msie 10"))
            {
                this.Head1.Controls.Add(new LiteralControl("<META content=\"IE=9,chrome=1\" http-equiv=\"X-UA-Compatible\">"));
            }

            if (!IsPostBack)
                {
                    if (Request.UrlReferrer != null)
                    { lnkBack.NavigateUrl = Request.UrlReferrer.ToString(); }
                    else { lnkBack.NavigateUrl = "/AutohausPortal/(S(" + Session.SessionID + "))/Start/Login.aspx"; }                    
                }

        }
    }
}