using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG;
using System.Data.SqlClient;
using System.Configuration;

namespace AutohausPortal.Start
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Web.Security.FormsAuthentication.SignOut();
            try
            {
                Session["logoutMode"] = "OK";

                if (TryUrlRemoteUserLogoutAndRedirect()) { return; }

                CKG.Base.Kernel.Security.User m_User;
                m_User = Common.GetUser(this);
                Int32 intPause = 0;
                Table1.Visible = false;
                Table2.Visible = false;
                FormDiv.Visible = false;
                int intRestrictedCustomerId = CheckRestrictedIP();
                if (intRestrictedCustomerId > -1)
                {
                    //Aha, Benutzer greift von IP zu, die Beschränkungen unterliegt

                    //Ermittele Standard-Benutzer
                    string strIpStandardUser = GiveIpStandardUser(intRestrictedCustomerId);
                    if (m_User.UserName.ToUpper() == strIpStandardUser.ToUpper())
                    {
                        Table2.Visible = true;
                        FormDiv.Visible = true;
                        intPause = -1;
                    }
                }
                    if (intPause > -1)
                    {
                        if ((Request.QueryString["DoubleLogin"] == null) || (!(Request.QueryString["DoubleLogin"].ToString()== "True"))) 
                        {
	                        m_User.SetLoggedOn(m_User.UserName, false, "");
                        } 
                        else 
                        {
	                        intPause = 4000;
	                        Table1.Visible = true;
	                        FormDiv.Visible = true;
                        }
                        Session.Abandon();
                        System.Web.Security.FormsAuthentication.RedirectFromLoginPage(m_User.UserID.ToString(), false);
                        Response.Redirect("/AutohausPortal/Start/Bounce.aspx?ReturnUrl=%2fAutohausPortal%2fStart%2fLogin.aspx?Logon=open");
                    }

            }
            catch (Exception DoNothing){}
        }
        private String GiveIpStandardUser(Int32 intCust)
        {
            //Ermittele IpStandardUser des Kunden

            Object result;
            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"].ToString());
            SqlCommand command;
            String strReturn = "";

            try
            {
                conn.Open();

                command = new SqlCommand("SELECT IpStandardUser FROM Customer WHERE CustomerID = " + intCust.ToString(), conn);

                result = command.ExecuteScalar();
                if (result != null)
                {
                    strReturn = result.ToString();
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            return strReturn;
        }
        private Int32 CheckRestrictedIP()
        {

            Object result;
            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"].ToString());
            SqlCommand command;
            Int32 intReturn = -1;
            try
            {
                conn.Open();
                command = new SqlCommand("SELECT CustomerID FROM IpAddresses WHERE IpAddress = '" + Request.UserHostAddress + "'", conn);

                result = command.ExecuteScalar();
                if (result != null)
                {
                    if (IsNumeric(result.ToString()))
                    {
                        Int32.TryParse(result.ToString(), out intReturn);
                    }

                }
            }
            catch (Exception ex)
            {
               
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return intReturn;
        }
        public static bool IsNumeric(string Value)
        {
            try
            {
                Convert.ToInt32(Value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool TryUrlRemoteUserLogoutAndRedirect()
        {
            if (Session["UrlRemoteLogin_LogoutUrl"] != null)
            {
                string urlRemoteLogin_LogoutUrl = Session["UrlRemoteLogin_LogoutUrl"].ToString();

                if (!String.IsNullOrEmpty(urlRemoteLogin_LogoutUrl))
                {
                    urlRemoteLogin_LogoutUrl = urlRemoteLogin_LogoutUrl.ToLower();
                    if (!urlRemoteLogin_LogoutUrl.StartsWith("http"))
                    {
                        urlRemoteLogin_LogoutUrl = "http://" + urlRemoteLogin_LogoutUrl;
                    }

                    Response.Redirect(urlRemoteLogin_LogoutUrl);
                    return true;
                }
            }

            return false;
        }
    }
}