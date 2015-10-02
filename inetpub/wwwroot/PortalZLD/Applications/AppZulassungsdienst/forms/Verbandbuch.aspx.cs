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


        }


        protected void btnErfassung_Click(object sender, EventArgs e)
        {
            string destination = string.Format("{0}Erfassung?vkbur={1}&ra={2}&rb={3}",
                ConfigurationManager.AppSettings["VerbandbuchHostAdress"],
                m_User.Kostenstelle,
                ConfigurationManager.AppSettings["VerbandbuchRemoteLoginKey"],
                ConfigurationManager.AppSettings["VerbandbuchRemoteLoginTimestamp"]);

            OpenLink(destination);
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            string destination = string.Format("{0}Report?vkbur={1}&ra={2}&rb={3}",
                ConfigurationManager.AppSettings["VerbandbuchHostAdress"],
                m_User.Kostenstelle,
                ConfigurationManager.AppSettings["VerbandbuchRemoteLoginKey"],
                ConfigurationManager.AppSettings["VerbandbuchRemoteLoginTimestamp"]);


            OpenLink(destination);
        }

        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            Response.Redirect("/PortalZLD/Start/Selection.aspx?AppID=" + Session["AppID"].ToString());
        }

        #endregion

        #region Methods

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
