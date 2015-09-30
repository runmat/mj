using System;
using System.Configuration;
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
            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            if (!IsPostBack)
            {
                ShowVerbandbuch();
            }
        }

        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            Response.Redirect("/PortalZLD/Start/Selection.aspx?AppID=" + Session["AppID"].ToString());
        }

        #endregion

        #region Methods

        private void ShowVerbandbuch()
        {
            ifrVerbandbuch.Attributes["src"] = string.Format("ServicesMvc/Verbandbuch/Erfassung?vkbur={0}&ra={1}&rb={2}", 
                m_User.Kostenstelle,
                ConfigurationManager.AppSettings["VerbandbuchRemoteLoginKey"],
                ConfigurationManager.AppSettings["VerbandbuchRemoteLoginTimestamp"]);
        }

        #endregion
    }
}