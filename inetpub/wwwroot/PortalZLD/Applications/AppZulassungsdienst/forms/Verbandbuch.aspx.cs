using System;
using System.Configuration;
using System.Text;
using System.Web.UI;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;

namespace AppZulassungsdienst.forms
{
    public partial class Verbandbuch : System.Web.UI.Page
    {
        private User m_User;

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            Common.GetAppIDFromQueryString(this);
            lblHead.Text =
                (string) m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            if (!IsPostBack)
            {
                ShowVerbandbuch();
            }
        }


        protected void btnErfassung_Click(object sender, EventArgs e)
        {
            string host = "https://sgwt.kroschke.de/";
            string destination = string.Format("/ServicesMvc/Verbandbuch/Erfassung?vkbur={0}&ra={1}&rb={2}",
                m_User.Kostenstelle,
                ConfigurationManager.AppSettings["VerbandbuchRemoteLoginKey"],
                ConfigurationManager.AppSettings["VerbandbuchRemoteLoginTimestamp"]);

            OpenLink(host + destination);
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            string host = "https://sgwt.kroschke.de/";
            string destination = string.Format("/ServicesMvc/Verbandbuch/Report?vkbur={0}&ra={1}&rb={2}",
                m_User.Kostenstelle,
                ConfigurationManager.AppSettings["VerbandbuchRemoteLoginKey"],
                ConfigurationManager.AppSettings["VerbandbuchRemoteLoginTimestamp"]);


            OpenLink(host + destination);
        }

        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            Response.Redirect("/PortalZLD/Start/Selection.aspx?AppID=" + Session["AppID"].ToString());
        }

        #endregion

        #region Methods

        private void ShowVerbandbuch()
        {
            string debug = string.Format("/ServicesMvc/Verbandbuch/Erfassung?vkbur={0}&ra={1}&rb={2}",
                m_User.Kostenstelle,
                ConfigurationManager.AppSettings["VerbandbuchRemoteLoginKey"],
                ConfigurationManager.AppSettings["VerbandbuchRemoteLoginTimestamp"]);

        }

        private void OpenLink(string zielUrl)
        {
            var sb = new StringBuilder();

            if (!Page.ClientScript.IsStartupScriptRegistered("OpenExternalLink"))
            {
                sb.Append("<script type=\"text/javascript\">");
                sb.Append("window.open(\"" + zielUrl + "\",\"_blank\",\"left=0,top=0,resizable=YES,scrollbars=YES\");");
                sb.Append("</script>");
            }

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "OpenExternalLink", sb.ToString());

        }
    }


    #endregion
  }
