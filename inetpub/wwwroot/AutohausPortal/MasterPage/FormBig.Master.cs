using System;
using System.Web;
using System.Web.UI;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using AutohausPortal.lib;
using CKG.Base.Kernel.Security;

namespace AutohausPortal.MasterPage
{
    public partial class FormBig : System.Web.UI.MasterPage
    {
        private User m_User;

        protected void Page_Load(object sender, EventArgs e)
        {
            String Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            lblCopyright.Text = "© " + DateTime.Now.Year.ToString() + " Christoph Kroschke GmbH " + " vers." + Version;        

            m_User = MVC.GetSessionUserObject();

            string strAppID = Request.QueryString["AppID"];

            lblHeadLine.Text = (string)m_User.Applications.Select("AppID = '" + strAppID + "'")[0]["AppFriendlyName"];
            lnkImpressum.NavigateUrl = "/AutohausPortal/(S(" + Session.SessionID + "))/Info/Impressum.aspx";
            lnkChangePassword.NavigateUrl = "/AutohausPortal/(S(" + Session.SessionID + "))/Start/ChangePassword.aspx";
            lnkLogout.NavigateUrl = "/AutohausPortal/(S(" + Session.SessionID + "))/Start/Logout.aspx";
            lnkStart.NavigateUrl = "/AutohausPortal/(S(" + Session.SessionID + "))/Start/Selection.aspx";

            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"]);

            try
            {
                if (HttpContext.Current.Request.UserAgent != null && (HttpContext.Current.Request.UserAgent.ToLower().Contains("msie 10") || HttpContext.Current.Request.UserAgent.ToLower().Contains("rv:11.0")))
                {
                    this.Head1.Controls.Add(new LiteralControl("<META content=\"IE=9,chrome=1\" http-equiv=\"X-UA-Compatible\">"));
                }

                if (!IsPostBack)
                {
                    if (Request.UrlReferrer != null)
                    { lnkBack.NavigateUrl = Request.UrlReferrer.ToString(); }
                    else { lnkBack.NavigateUrl = "/AutohausPortal/(S(" + Session.SessionID + "))/Start/Selection.aspx"; }
                    conn.Open();

                    SqlCommand command = new SqlCommand("SELECT AppType,DisplayName FROM ApplicationType ORDER BY Rank", conn);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataTable table = new DataTable();
                    da.Fill(table);

                    DataTable appTable = m_User.Applications.Copy();
                    MVC.MvcPrepareDataRowsUrl(appTable, m_User.UserName);

                    getAuftraege();

                    DataRow[] appRows = appTable.Select("AppName='Auftraege' AND AppInMenu=1");
                    if (appRows.Length > 0)
                    {
                        lnkMenge.NavigateUrl = appRows[0]["AppURL"].ToString();
                    }
                    lnkToAuftrag.NavigateUrl = lnkMenge.NavigateUrl; 
                }
            }
            finally
            {
                if (conn.State == ConnectionState.Open) { conn.Close(); }
            }
        }

        private void getAuftraege()
        {
            lnkMenge.Text = Session["AnzahlAuftraege"].ToString();
        }
    }
}