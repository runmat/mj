using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;

namespace AutohausPortal.Start
{
    public partial class Bounce : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Boolean blnDatabaseEntries = (ConfigurationManager.AppSettings["DatabaseEntries"] == "ON");

                String strURL = this.Request.QueryString["ReturnURL"].ToString();
                if (strURL == String.Empty)
                {
                    if ((this.Request.UrlReferrer != null))
                    {
                        strURL = this.Request.UrlReferrer.AbsoluteUri;
                        if (blnDatabaseEntries)
                        {
                            WriteLog("Request.UrlReferrer.AbsoluteUri: " + this.Request.UrlReferrer.AbsoluteUri + ", Request.UrlReferrer.AbsolutePath: " + this.Request.UrlReferrer.AbsolutePath);
                        }
                    }
                }
                else
                {
                    if (blnDatabaseEntries)
                    {
                        WriteLog("Request.QueryString(\"ReturnURL\"): " + this.Request.QueryString["ReturnURL"].ToString());
                    }
                }

                if (strURL.IndexOf("?") > 0) 
                {
                    strURL = strURL.Substring(0, strURL.IndexOf("?"));
                }
                if (!(strURL == string.Empty)) {
	                Response.Redirect(strURL + "?Logon=open", true);
                } 
                else 
                {
	                WriteError();
                }
            }
            catch (Exception)
            {

                WriteError();
            }
        }

        private void WriteLog(string strMessage)
        {
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(ConfigurationManager.AppSettings["Connectionstring"].ToString());

            try
            {
                System.Data.SqlClient.SqlCommand cmdWriteLog = new System.Data.SqlClient.SqlCommand("INSERT INTO LogBounce (Message,UserHostAddress) VALUES (@Message,@UserHostAddress)", conn);

                conn.Open();

                cmdWriteLog.Parameters.AddWithValue("@Message", strMessage);
                cmdWriteLog.Parameters.AddWithValue("@UserHostAddress", this.Request.UserHostAddress);
                cmdWriteLog.ExecuteNonQuery();
                cmdWriteLog.Dispose();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
        }

        private void WriteError()
        {
            Response.Write("<b>Unable to redirect!</b>");
            Response.Write("<BR>");
            Response.Write("ReturnURL: ");
            Response.Write(this.Request.QueryString["ReturnURL"].ToString());
            Response.Write("<BR>");
            Response.Write("UrlReferrer: ");
            Response.Write(this.Request.UrlReferrer);
            Response.Write("<BR>");
            Response.Write("RawUrl: ");
            Response.Write(this.Request.RawUrl);
            Response.Write("<BR>");
            Response.Write("Url.AbsoluteUri: ");
            Response.Write(this.Request.Url.AbsoluteUri);
            Response.Write("<BR>");

        }
    }
}