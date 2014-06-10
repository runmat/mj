using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AutohausPortal.lib;
using CKG.Base.Kernel;
using CKG.Base.Kernel.Common;
using CKG.Base;
using System.Drawing.Imaging;
using CKG.Base.Business;
using CKG;
using System.Data.SqlClient;
using System.Net;
using System.IO;
using System.Configuration;
using System.Data;
using System.Collections;
using WebTools.Services;

namespace AutohausPortal.Start
{
    public partial class Login : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User = new CKG.Base.Kernel.Security.User();
        private CKG.Base.Kernel.Security.App m_App;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["objUser"] != null) 
            {
                m_User = (CKG.Base.Kernel.Security.User)Session["objUser"];
                if (!m_User.LoggedOn && !m_User.DoubleLoginTry) 
                {
                    if (User.Identity.IsAuthenticated == false) 
                    {
                        if (lnkPasswortVergessen.Visible == false) 
                        {
                            Response.Redirect(BouncePage(this), true);
                        }
                    }
                }
            }

            if (UrlRemoteUserProcessLogin()) { return; }

            litAlert.Text = "";
            txtUsername.Focus();
            Page.Title = "Anmeldung";
            if (!IsPostBack) 
            {
                if (!CheckUniqueSessionID())
                {
                    Response.Redirect(BouncePage(this), true);
                }
                displayMessages();
                StandardLogin.Visible = true;
                divStandardLogin_bottom.Visible = StandardLogin.Visible;
                DoubleLogin2.Visible = false;
                divDoubleLogin2_bottom.Visible = DoubleLogin2.Visible;
                Session["CaptchaGen1"] = GenerateRandomCode();
                Session["CaptchaGen2"] = GenerateRandomCode();

                String sSSORemoteHost = ConfigurationManager.AppSettings["SSORemoteHost"].ToString();
                if ( Request.QueryString["key"] != null) 
                    {

                        String sKey = Request.QueryString["key"].ToString();

                        CKG.Base.Kernel.Security.CryptNew clsCrypt = new CKG.Base.Kernel.Security.CryptNew();

                        String DeCryptedKey = clsCrypt.psDecrypt(sKey);
                        String strDomainError  = "Die Anmeldung ist in der aktuellen Konfiguration nicht möglich.<br>Setzen Sie sich bitte mit Ihrer Kontaktperson bei der Christoph Kroschke GmbH in Verbindung.";

                        String strUserName = GetDomainUser(DeCryptedKey.ToUpper());
                        if (m_User.Login(strUserName, Session.SessionID.ToString()))
                        {
                            System.Web.Security.FormsAuthentication.RedirectFromLoginPage(m_User.UserID.ToString(), false);
                        }
                        else
                        {
                            lblError.Text = strDomainError + "<br>(" + m_User.ErrorMessage + ")";
                            cmdLogin.Enabled = false;
                        }                            
                        return;
                    }
                Int32 intRestrictedCustomerId = CheckRestrictedIP();
                if (intRestrictedCustomerId > -1) 
                {
                    if (Request.QueryString["Logon"] == null || Request.QueryString["Logon"].ToString().ToUpper() != "OPEN")
                    {
                        String strIpError = "Die Anmeldung ist in der aktuellen Konfiguration nicht möglich.<br>Setzen Sie sich bitte mit Ihrer Kontaktperson beim DAD bzw. der Christoph Kroschke GmbH in Verbindung.";
                        String strIpStandardUser = GiveIpStandardUser(intRestrictedCustomerId);

                        if (strIpStandardUser.Length > 0) 
                        {
                            if (m_User.Login(strIpStandardUser, Session.SessionID.ToString()))
                            {
                                System.Web.Security.FormsAuthentication.RedirectFromLoginPage(m_User.UserID.ToString(), false);
                            }
                            else
                            { 
                                lblError.Text = strIpError + "<br>(" + m_User.ErrorMessage + ")";
                                cmdLogin.Enabled = false;
                            }
                        
                        }
                    }
                }

            }


        }

        public String BouncePage(System.Web.UI.Page Form)
        {
            return "/" +  ConfigurationManager.AppSettings["ApplicationKey"].ToString() + "/Start/Bounce.aspx?ReturnURL=" + System.Web.HttpUtility.UrlEncode(Form.Request.RawUrl);
        }

        private Boolean CheckUniqueSessionID()
        {
            DataTable table;
            Boolean blnReturn = true;

            try
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"].ToString());
                SqlCommand command = conn.CreateCommand();
                command.CommandText = "SELECT id FROM LogWebAccess WHERE idSession = @idSession";
                command.Parameters.AddWithValue("@idSession", Session.SessionID.ToString());
                
                SqlDataAdapter da = new SqlDataAdapter(command);
                conn.Open();
                table = new DataTable();
                da.Fill(table);
                if (table.Rows.Count > 0)
                {
                    blnReturn = false;
                }
                conn.Close();
                conn.Dispose();
                da.Dispose();
            }
            catch (Exception)
            {
            }
            return blnReturn;
        }

        private Int32 CheckRestrictedIP()
        {
        
            Object result;
            SqlConnection conn= new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"].ToString());
            SqlCommand command;
            Int32 intReturn= -1 ;
            try
            {
                conn.Open();
                command = new SqlCommand("SELECT CustomerID FROM IpAddresses WHERE IpAddress = '" + Request.UserHostAddress + "'", conn);

                result = command.ExecuteScalar();
                if (result != null ) 
                {
                    if (IsNumeric(result.ToString())) 
                    {
                        Int32.TryParse(result.ToString(), out intReturn);
                    }
                
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return intReturn;
        }

        private void displayMessages()
        {
            try 
	        {
                DataTable table = new DataTable();
	            table.Columns.Add("Created", typeof(DateTime));
                table.Columns.Add("Title", typeof(String));
                table.Columns.Add("Message", typeof(String));

                DateTime jetzt = DateTime.Now;
                String text;
                String htext;

                cbxLogin_TEST.Checked = true;
                cbxLogin_PROD.Checked = true;

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"]))
                {
                    conn.Open();

                    SqlCommand command = conn.CreateCommand();

                    command.CommandText = "SELECT * FROM LoginUserMessage" +
                                  " WHERE (@jetzt BETWEEN ShowMessageFrom AND ShowMessageTo) OR (@jetzt BETWEEN LockLoginFrom AND LockLoginTo)" +
                                  " ORDER BY ID DESC";

                    command.Parameters.AddWithValue("@jetzt", jetzt);

                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            // Nachricht anzeigen?
                            if (dr["ShowMessageFrom"] != DBNull.Value && jetzt > (DateTime) dr["ShowMessageFrom"]
                                && dr["ShowMessageTo"] != DBNull.Value && jetzt < (DateTime) dr["ShowMessageTo"])
                            {
                                DataRow newRow = table.NewRow();

                                newRow["Created"] = (DateTime)dr["Created"];

                                // Überschrift formatieren
                                text = dr["Title"].ToString();
                                text = text.Replace("{c=", "{font color=");
                                text = text.Replace("{/c}", "{/font}");
                                text = text.Replace("{", "<");
                                text = text.Replace("}", ">");
                                newRow["Title"] = text;

                                // Nachricht formatieren
                                text = dr["Message"].ToString();
                                if (text.Contains("{h}"))
                                {
                                    htext = text.Substring(text.IndexOf("{h}") + 3, text.IndexOf("{/h}") - text.IndexOf("{h}") - 3);
                                    text = text.Replace("{h}", "<a href=\"");
                                    text = text.Replace("{/h}", "\" target = \"_BLANK\">" + htext + "</a>");
                                }
                                text = text.Replace("{c=", "{font color=");
                                text = text.Replace("{/c}", "{/font}");
                                text = text.Replace("{", "<");
                                text = text.Replace("}", ">");
                                newRow["Message"] = text;

                                table.Rows.Add(newRow);
                            }

                            // Login sperren?
                            if (dr["LockLoginFrom"] != DBNull.Value && jetzt > (DateTime)dr["LockLoginFrom"]
                                && dr["LockLoginTo"] != DBNull.Value && jetzt < (DateTime)dr["LockLoginTo"])
                            {
                                if ((bool)dr["LockForTest"])
                                {
                                    cbxLogin_TEST.Checked = false;
                                }
                                if ((bool)dr["LockForProd"])
                                {
                                    cbxLogin_PROD.Checked = false;
                                }
                            }
                        }
                    }

                    conn.Close();
                }

                Repeater1.DataSource = table;
                Repeater1.DataBind();

                if (table.Rows.Count == 0) 
                {
                    divRepeater.Visible = false;
                    divRepeater_bottom.Visible = divRepeater.Visible;
                }
	        }
	        catch (Exception)
	        {

	        }
        }

        private string GenerateRandomCode()
        {
            Random random = new Random();

            string s = "";
            for (int i = 0; i <= 1; i++)
            {
                if (i == 0)
                {
                    s = String.Concat(s, random.Next(2).ToString());
                }
                else
                {
                    s = String.Concat(s, random.Next(10).ToString());
                }
            }
            return s;
        }

        private string GetDomainUser(String strDomainUser)
        {
            SqlConnection Connection = new SqlConnection();
            SqlCommand Command = new SqlCommand();
            ArrayList Applications = new ArrayList();
            String DomainUser = "";
            try
            {
                Connection.ConnectionString = ConfigurationManager.AppSettings["Connectionstring"].ToString();
                Command.Connection = Connection;
                Command.CommandType = CommandType.Text;
                Command.CommandText = "SELECT UserName FROM DomainUser Where DomainName = '" + strDomainUser + "'";

                
                Connection.Open();
                DomainUser = Command.ExecuteScalar().ToString();
                Connection.Close();
                

            }
            catch (Exception ex)
            {
                if (Connection.State == ConnectionState.Open) {Connection.Close();}
                return "";
                lblError.Text = "Beim Laden der Anwendungen ist ein Fehler aufgetreten.<br>(" + ex.Message + ")";
            }
            return DomainUser;
        }

        private String GiveIpStandardUser(Int32 intCust )
    {
        //Ermittele IpStandardUser des Kunden

        Object result;
        SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"].ToString());
        SqlCommand command ;
        String strReturn  = "";

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
            lblError.Text = ex.Message;
        }
        finally
        {    
            conn.Close();
            conn.Dispose();
        }

        return strReturn;
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

        protected void btnEmpty_Click(object sender, ImageClickEventArgs e)
        {
            btnLogin_Click(sender, e);
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                MessageLabel.Text = "";
                Boolean blnPasswdlink = lnkPasswortVergessen.Visible;
                if ((this.Session["objUser"] != null) && this.User.Identity.IsAuthenticated == false && blnPasswdlink == false) 
                {
                    //---JVE: User nicht mehr in der Session gespeichert bzw. nicht Authentifiziert---
                    Response.Redirect(BouncePage(this), true);
                    return;
                }

                if (UrlRemoteUserProcessLogin()) { return; }

                lnkPasswortVergessen.Text = "Passwort vergessen?";
                lnkPasswortVergessen.Visible = false;
                if (m_User.Login(txtUsername.Text, txtPassword.Text, Session.SessionID.ToString(), blnPasswdlink))
                {

                    if (Request.QueryString["ReturnURL"] != null)
                    {
                        var returnUrl = Request.QueryString["ReturnURL"].ToLower();
                        if (returnUrl.Contains("mvc/"))
                        {
                            m_User.SetLastLogin(DateTime.Now);
                            Session["objUser"] = m_User;
                            System.Web.Security.FormsAuthentication.RedirectFromLoginPage(m_User.UserID.ToString(), false);
                            Response.Redirect(string.Format("{0}?un={1}", returnUrl, CryptoMd5.EncryptToUrlEncoded(m_User.UserName)));
                            return;
                        }
                    }

                    //'Prüfe IP-Adress-Regelung
                    if (m_User.Customer.IpRestriction)
                    {
                        String strIpError = "Die Anmeldung ist in der aktuellen Konfiguration nicht möglich.<br>Setzen Sie sich bitte mit Ihrer Kontaktperson beim DAD bzw. der Christoph Kroschke GmbH in Verbindung.";
                        if (m_User.Customer.IpAddresses.Select("IpAddress='" + Request.UserHostAddress + "'").Length == 0)
                        {
                            lblError.Text = strIpError;
                            return;
                        }
                    }

                    if (!checkLogin())
                    {
                        return;
                    }
                    m_User.SetLastLogin(DateTime.Now);
                    Session["objUser"] = m_User;
                    if (m_User.DoubleLoginTry)
                    {
                        StandardLogin.Visible = false;
                        divStandardLogin_bottom.Visible = StandardLogin.Visible;
                        DoubleLogin2.Visible = true;
                        divDoubleLogin2_bottom.Visible = DoubleLogin2.Visible;
                    }
                    else
                    {
                        System.Web.Security.FormsAuthentication.RedirectFromLoginPage(m_User.UserID.ToString(), false);

                        //zur späteren Benutzung (iframe)
                        //FormsAuthentication.SetAuthCookie(m_User.UserID.ToString, False)
                        //Response.Write("<script language='javascript'>")
                        //Response.Write("window.open('Selection.aspx' ,'Zoic','width=600, height=400,toobar=yes,addressbar=yes,menubar=yes,scrollbars=yes,resizable=yes');")
                        //Response.Write("window.location.href ='Login.aspx';")
                        //Response.Write("<" + "/" + "script" + ">")
                    }
                }
                else //Error-Property bei User-Objekt einfügen und hier darstellen
                {
                    if (m_User.ErrorMessage.Length > 0)
                    {
                        if (m_User.ErrorMessage == "4174")
                        {
                            //'Benutzer existiert und die Voraussetzungen zur Passwortanforderung
                            //'per geheimer Frage sind gegeben
                            Session["objUser"] = m_User;
                            lblError.Text = "Fehler bei der Anmeldung.";
                            lnkPasswortVergessen.Visible = true;
                            lbtnHelpCenter.Visible = false;
                        }
                        else if (m_User.ErrorMessage == "9999")
                        {
                            lblError.Text = "Fehler bei der Anmeldung. Prüfen Sie Ihre Eingaben!";
                            lnkPasswortVergessen.Visible = true;
                            lbtnHelpCenter.Visible = false;
                        }
                        else
                        {
                            if (m_User.AccountIsLockedOut && m_User.AccountIsLockedBy == "User")
                            {
                                if (m_User.Email.Length > 0 && m_User.Customer.ForcePasswordQuestion && m_User.QuestionID > -1)
                                {
                                    System.Web.Security.FormsAuthentication.RedirectFromLoginPage(m_User.UserID.ToString(), false);
                                }

                                else
                                {
                                    lblError.Text = "Fehler bei der Anmeldung<br>(" + m_User.ErrorMessage + ")";
                                    lnkPasswortVergessen.Visible = true;
                                    lbtnHelpCenter.Visible = false;
                                    lnkPasswortVergessen_Click(sender, e);
                                }
                            }
                            else if (m_User.AccountIsLockedOut && m_User.AccountIsLockedBy == "Now")
                            {
                                lblError.Text = "Fehler bei der Anmeldung<br>(" + m_User.ErrorMessage + ")";
                                lnkPasswortVergessen.Visible = true;
                                lbtnHelpCenter.Visible = false;
                                lnkPasswortVergessen.Text = "Entsperren";
                            }
                            else
                            {
                                lblError.Text = "Fehler bei der Anmeldung<br>(" + m_User.ErrorMessage + ")";
                                lnkPasswortVergessen.Visible = true;
                                lbtnHelpCenter.Visible = false;
                            }
                        }
                    }
                    else 
                    {
                        lblError.Text = "Fehler bei der Anmeldung.";
                        lnkPasswortVergessen.Visible = true;                    
                    }
                
                }


            }
            catch (Exception ex)
            {
                m_App = new CKG.Base.Kernel.Security.App(m_User);
                m_App.WriteErrorText(1, txtUsername.Text, "Login", "btnLogin_Click", ex.ToString());
                lblError.Text = "Fehler bei der Anmeldung (" + ex.Message + ")";                

            }
        }

        private bool UrlRemoteUserProcessLogin()
        {
            string remoteUserName = "";
            string remoteUserPwdHashed = "";
            UrlRemoteUserTryLogin(ref remoteUserName, ref remoteUserPwdHashed);

            if (!String.IsNullOrEmpty(remoteUserName))
            {
                if (m_User.Login(remoteUserName, remoteUserPwdHashed, "", false))
                {
                    System.Web.Security.FormsAuthentication.RedirectFromLoginPage(m_User.UserID.ToString(), false);
                }
                else
                {
                    lblError.Text = "URL Remote-Login fehlgeschlagen! <br>(" + m_User.ErrorMessage + ")";
                    cmdLogin.Enabled = false;
                }
                return true;
            }

            return false;
        }

        private void UrlRemoteUserTryLogin(ref string userName, ref string userPwd)
        {
            if (IsPostBack) { return; }

            if (Request.QueryString["ra"] == null) { return; }
            if (Request.QueryString["rb"] == null) { return; }

            string rid = Request.QueryString["ra"].ToString();
            string dat = Request.QueryString["rb"].ToString();

            if (String.IsNullOrEmpty(rid)) { return; }
            if (String.IsNullOrEmpty(dat)) { return; }

            if (rid.Length < 30) { return; }
            if (!UrlRemoteHashedDateIsValid(dat)) { return; }

            int webUserID =  DbGetIntValue(String.Format("select isnull(UserID,-1) from WebUser where UrlRemoteLoginKey = '{0}'", rid));
            if (webUserID == -1) { return; }

            int customerID = DbGetIntValue(String.Format("select isnull(CustomerID,-1) from WebUser where UserID = {0}", webUserID));
            if (customerID == -1) { return; }

            bool customerRemoteLoginAllowed = (0 < DbGetIntValue(String.Format("select isnull(AllowUrlRemoteLogin,-1) from Customer where CustomerID = {0}", customerID)));
            if (!customerRemoteLoginAllowed) { return; }

            userName = DbGetStringValue(String.Format("select isnull(Username,'') from WebUser where UserID = {0}", webUserID));
            userPwd = DbGetStringValue(String.Format("select isnull(Password,'') from WebUser where UserID = {0}", webUserID));

            if (Request.QueryString["logouturl"] != null)
            {
                Session["UrlRemoteLogin_LogoutUrl"] = HttpUtility.UrlDecode(Request.QueryString["logouturl"].ToString());
            }
        }

        private bool UrlRemoteHashedDateIsValid(string strHashedDate)
        {
            if (String.IsNullOrEmpty(strHashedDate)) { return false; }
            if (strHashedDate.Length != 10) { return false; }

            string strEncryptedDate = "";
            int i;
            bool reversal = false;
            for (i = 0; i < strHashedDate.Length; i++)
            {
                char hashedChar = strHashedDate[i];
                int hashedVal = Convert.ToInt32(hashedChar);
                
                strEncryptedDate = strEncryptedDate + Convert.ToChar((!reversal ? hashedVal : Convert.ToInt32('A') + Convert.ToInt32('Z') - hashedVal) - 30);
                reversal = !reversal;
            }

            if (String.IsNullOrEmpty(strEncryptedDate)) { return false; }
            if (strEncryptedDate.Length != 10) { return false; }

            int intHour;
            if (!Int32.TryParse(strEncryptedDate.Substring(0, 2), out intHour)) { return false; }

            string strDate = strEncryptedDate.Substring(2, 8);
            if (!IsNumeric(strDate)) { return false; }

            DateTime dDate;
            try
            {
                dDate = new DateTime(Int32.Parse(strDate.Substring(4, 4)), Int32.Parse(strDate.Substring(2, 2)), Int32.Parse(strDate.Substring(0, 2)), intHour, 0, 0);
            }
            catch
            {
                return false;
            }

            TimeSpan differenceToNow = dDate - DateTime.Now;
            if (Math.Abs(differenceToNow.TotalMinutes) > 120) { return false; }

            return true;

        }

        private string DbGetStringValue(string sql)
        {
            object result;
            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"]);
            SqlCommand command;
            string strReturn;

            try
            {
                conn.Open();

                command = new SqlCommand(sql, conn);

                result = command.ExecuteScalar();

                strReturn = Convert.ToString(result);
            }
            catch(Exception ex)
            {
                strReturn = "";
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            return strReturn;
        }

        private int DbGetIntValue(string sql)
        {
            int iValue;
            string sValue = DbGetStringValue(sql);

            if (String.IsNullOrEmpty(sValue))
            {
                return -1;
            }

            if (sValue.ToLower().Contains("false") || sValue.ToLower().Contains("true"))
            {
                return (sValue.ToLower() == "true" ? 1 : 0);
            }

            try
            {
                iValue = Int32.Parse(sValue);
            }
            catch(Exception ex)
            {
                iValue = -1;
            }

            return iValue;
        }

        private Boolean checkLogin()
        {
            if (m_User.HighestAdminLevel == CKG.Base.Kernel.Security.AdminLevel.Master)
            {
                return true;
            }
            else
            {
                if( (!cbxLogin_TEST.Checked) && (!cbxLogin_PROD.Checked) )
                {
                    //Weder CKE noch CKP - Login erlaubt (nur DAD-Admin)
                    lblError.Text = "Die Anmeldung ist z.Z. gesperrt.";
                    return false;
                }
                if ((cbxLogin_TEST.Checked) && (!cbxLogin_PROD.Checked))
                {
                    //Nur CKE - Login erlaubt
                    if (!m_User.IsTestUser) 
                    {
                        lblError.Text = "Die Anmeldung ist z.Z. gesperrt.";
                        return false;
                    }
                }
                if ((!cbxLogin_TEST.Checked) && (cbxLogin_PROD.Checked))
                {
                    //Nur CKE - Login erlaubt
                    if (m_User.IsTestUser)
                    {
                        lblError.Text = "Die Anmeldung ist z.Z. gesperrt.";
                        return false;
                    }
                }                 
            }

            return true;
        }

        protected void lnkPasswortVergessen_Click(object sender, EventArgs e)
        {
        MessageLabel.Text = "";
        if (m_User.UserID == -1 )
        {
            Session["LostPassword"] = 1;
            txtWebUserName.Text = txtUsername.Text;
            //lblRedStar.Visible = true;
            divKontakt.Visible = !divKontakt.Visible;
            divKontakt_bottom.Visible = divKontakt.Visible;
            if (divKontakt.Visible == true) { GenerateCaptcha(); }
        }
        else if (m_User.Email.Length > 0 && m_User.Customer.ForcePasswordQuestion && m_User.QuestionID > -1 )
            {System.Web.Security.FormsAuthentication.RedirectFromLoginPage(m_User.UserID.ToString(), false);}
        else
            {
                Session["LostPassword"] = 1;
                txtWebUserName.Text = txtUsername.Text;
                divProblem.Visible = false;
                divProblemTrenner.Visible = false;
                txtProblem.Visible = false;
                divKontakt.Visible = !divKontakt.Visible;
                divKontakt_bottom.Visible = divKontakt.Visible;
                if (divKontakt.Visible == true) { GenerateCaptcha(); }            
            }
        }

        protected void cmdBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(BouncePage(this), true);
        }

        protected void cmdContinue_Click(object sender, EventArgs e)
        {
            m_User.DoubleLoginTry = false;
            m_User.SetLoggedOn(m_User.UserName, true, Session.SessionID.ToString());
            m_User.SessionID = Session.SessionID.ToString();
            Session["objUser"] = m_User;
            System.Web.Security.FormsAuthentication.RedirectFromLoginPage(m_User.UserID.ToString(), false);
        }

        private void GenerateCaptcha()
        {
            String SavePath = ConfigurationManager.AppSettings["CaptchaImageSave"].ToString();
            if (Session["CaptchaGen1"] != null)
            {
                String imagekey = Session["CaptchaGen1"].ToString();
                // Create a CAPTCHA image using the text stored in the Session object.
                CaptchaImage ci = new CaptchaImage(imagekey, 80, 30, "Century Schoolbook");

                ci.Image.Save(SavePath + imagekey + ".jpg", ImageFormat.Jpeg);

                // Dispose of the CAPTCHA image object.
                ci.Dispose();

                imgCatcha1.Src = "..\\Temp\\Pictures\\" + imagekey + ".jpg";         
            }
            if (Session["CaptchaGen2"] != null)
            {
                String imagekey = Session["CaptchaGen2"].ToString();
                // Create a CAPTCHA image using the text stored in the Session object.
                CaptchaImage ci = new CaptchaImage(imagekey, 80, 30, "Century Schoolbook");

                ci.Image.Save(SavePath + imagekey + ".jpg", ImageFormat.Jpeg);

                // Dispose of the CAPTCHA image object.
                ci.Dispose();

                imgCatcha2.Src = "..\\Temp\\Pictures\\" + imagekey + ".jpg";
            }
        }

        private Boolean validateHelpData()
        {
            MessageLabel.Text = "";
            Boolean breturn = false;
            if (Session["LostPassword"].ToString() == "1") 
            {
                if (txtWebUserName.Text.Trim().Length == 0) 
                { 
                    MessageLabel.Text = "Bitte geben Sie Ihren Benutzernamen ein!";
                    txtProblem.BorderColor = System.Drawing.ColorTranslator.FromHtml("#C40000");
                    breturn = true;                                
                }
            }
            if (ddlAnrede.SelectedValue == "-")
            { 
                MessageLabel.Text = "Bitte Plichfelder ausfüllen!";
                ddlAnrede.BorderColor = System.Drawing.ColorTranslator.FromHtml("#C40000");
                breturn = true;                
            }
            if (txtName.Text.Trim().Length == 0)
            {   
                MessageLabel.Text = "Bitte Plichfelder ausfüllen!";
                txtName.BorderColor = System.Drawing.ColorTranslator.FromHtml("#C40000");
                breturn = true;
            }
            if (txtVorname.Text.Trim().Length == 0)
            {   
                MessageLabel.Text = "Bitte Plichfelder ausfüllen!";
                txtVorname.BorderColor = System.Drawing.ColorTranslator.FromHtml("#C40000");
                breturn = true;
            }
            if (txtFirma.Text.Trim().Length == 0)
            {   
                MessageLabel.Text = "Bitte Plichfelder ausfüllen!";
                txtFirma.BorderColor = System.Drawing.ColorTranslator.FromHtml("#C40000");
                breturn = true;
            }
            if (txtTelefon.Text.Trim().Length == 0)
            {   
                MessageLabel.Text = "Bitte Plichfelder ausfüllen!";
                txtTelefon.BorderColor = System.Drawing.ColorTranslator.FromHtml("#C40000");
                breturn = true;
            }

            if (txtEmail.Text.Trim().Length == 0)
            {   
                MessageLabel.Text = "Bitte Plichfelder ausfüllen!";
                txtEmail.BorderColor = System.Drawing.ColorTranslator.FromHtml("#C40000");
                breturn = true;
            }
            else
            {
                if (HelpProcedures.EmailAddressCheck(txtEmail.Text.Trim()) ==false)
                {
                    MessageLabel.Text = "<br />Email-Adresse nicht im richtigen Format(yxz@firma.de))";
                    txtEmail.BorderColor = System.Drawing.ColorTranslator.FromHtml("#C40000");
                    breturn = true;
                }
            }
            if (Session["LostPassword"].ToString() == "0" )
            {
                if (txtProblem.Text.Trim().Length == 0)
                {   
                    MessageLabel.Text = "Bitte Plichfelder ausfüllen!";
                    txtProblem.BorderColor = System.Drawing.ColorTranslator.FromHtml("#C40000");
                    breturn = true;
                }
            }
            return breturn;
        }

        protected void lbtnHelpCenter_Click(object sender, EventArgs e)
        {
            MessageLabel.Text = "";
            Session["LostPassword"] = 0;
            //lblRedStar.Visible = false;
            divKontakt.Visible = !divKontakt.Visible;
            divKontakt_bottom.Visible = divKontakt.Visible;
            if (divKontakt.Visible == true) 
            {
                divProblem.Visible = true;
                divProblemTrenner.Visible = true;
                txtProblem.Visible = true;
                GenerateCaptcha();
            }

        }

        protected void cmdSend_Click(object sender, EventArgs e)
        {
            Int32 num1  = 0;
            Int32 num2  = 0;
            MessageLabel.Text = "";
            if (!validateHelpData())
            {
                Int32.TryParse(Session["CaptchaGen1"].ToString(), out num1);
                Int32.TryParse(Session["CaptchaGen2"].ToString(), out num2);
                if (CodeNumberTextBox.Text == (num1 + num2).ToString())
                {
                    GenerateMailBody();
                    Session["CaptchaGen1"]= GenerateRandomCode();
                    Session["CaptchaGen2"] = GenerateRandomCode();
                    MessageLabel.Text = "E-Mail wurde versand!";
                    CodeNumberTextBox.Text = "";
                    ddlAnrede.SelectedValue = "-";
                    txtName.Text = "";
                    txtVorname.Text = "";
                    txtTelefon.Text = "";
                    txtEmail.Text = "";
                    txtFirma.Text = "";
                    divKontakt.Visible = false;
                    divKontakt_bottom.Visible = divKontakt.Visible;
                }
                else
                {
                    MessageLabel.Text = "Versuchen Sie es normal oder generieren Sie neu.";
                    Session["CaptchaGen1"] = GenerateRandomCode();
                    Session["CaptchaGen2"] = GenerateRandomCode();     
                    GenerateCaptcha();
                }
            }
        }

        private void GenerateMailBody()
        {
            String str = "";
            if(Session["LostPassword"].ToString() == "1" )
            {
                str = "Helpdesk-Auftrag: Passwort vergessen \r\n---------------------- \r\n\r\n";
            }
            else
            {
                str = "Helpdesk-Auftrag \r\n---------------------- \r\n\r\n";
            }
            if(txtWebUserName.Text.Trim().Length > 0 )
            {
                str = "Benutzername: " + txtWebUserName.Text.Trim() + "\r\n---------------------- \r\n\r\n";
            }
            else
            {
                str = "Benutzername: nicht angegeben \r\n---------------------- \r\n\r\n";
            }

            str += "\r\n";
            //Benutzerdaten
            str += "ANREDE         : " + ddlAnrede.SelectedItem.Text + "\r\n";
            str += "NAME           : " + txtName.Text + "\r\n";
            str += "VORNAME        : " + txtVorname.Text + "\r\n";
            str += "FIRMA          : " + txtFirma.Text + "\r\n";
            str += "TELEFON        : " + txtTelefon.Text + "\r\n";
            str += "EMAILADRESSE   : " + txtEmail.Text + "\r\n";
            if (Session["LostPassword"].ToString() == "0")
            {
                str += "Frage/Problem  : " + txtProblem.Text + "\r\n";
            }


            str += "\r\nAUF DIESE MAIL NICHT ANTWORTEN.";
            SendMail(str);
        }

        private void SendMail(string message)
        {
            try
            {
                System.Net.Mail.MailMessage Mail;
                System.Net.Mail.MailAddress smtpMailSender = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["SmtpMailSender"]);
                String smtpMailServer = "", MailAdresses = "";

                Mail = new System.Net.Mail.MailMessage();
                Mail.Body = message;
                Mail.From = smtpMailSender;
                ZLDCommon.LeseMailEmpfaenger("1", ref MailAdresses);

                if (MailAdresses.Trim().Split(';').Length > 0) 
                { 
                    String[] Adressen = MailAdresses.Trim().Split(';');

                    foreach (String tmpStr in Adressen)
                    {
                        Mail.To.Add(tmpStr);
                    }
                }
                else if (MailAdresses.Length > 0)
                {
                    Mail.To.Add(MailAdresses);              
                }
                else
                {
                    throw new Exception("Kein Mailempfänger gefunden");
                }

                Mail.Subject = "Helpdeskanfrage der Login-Seite Kroschke Kundenportal";
                Mail.IsBodyHtml = false;
                smtpMailServer = ConfigurationManager.AppSettings["SmtpMailServer"];
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(smtpMailServer);
                client.Send(Mail);
            }
            catch (Exception)
            {
                MessageLabel.Text = "Fehler beim Versenden der E-Mail.";
            }
        }

        protected void cmdRefresh_Click(object sender, EventArgs e)
        {
            Session["CaptchaGen1"] = GenerateRandomCode();
            Session["CaptchaGen2"] = GenerateRandomCode();
            MessageLabel.Text = "";
            CodeNumberTextBox.Text = "";
            GenerateCaptcha();

        }
    }
}