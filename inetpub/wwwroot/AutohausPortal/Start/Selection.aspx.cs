﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Business;
using CKG.Base.Kernel;
using CKG.Base.Kernel.Common;
using System.Configuration;
using System.Data;
using AutohausPortal.lib;
using CKG.Base.Kernel.Security;
using System.Data.SqlClient;

namespace AutohausPortal.Start
{
    public partial class Selection : Page
    {
        private User m_User;
        private App m_App;

        public DataView MenuChangeSource;
        public DataView MenuChangeAHSource;
        public DataView MenuReportSource;
        public DataView MenuToolsSource;

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["ShowOtherString"] = "";
            Session["Authorization"] = null;
            Session["AuthorizationID"] = null;
            Session["WaitObject"] = null;
            Session["ResultTable"] = null;
            Session["ExcelTable"] = null;
            Int32 iLoop;
            Object item;
            String sName;
            for (iLoop = Session.Contents.Count - 1; iLoop >= 0; iLoop += -1) 
            {
                item = Session.Contents.Keys[iLoop];
                if (item != null)
                {
                        sName = item.ToString();
                        if(sName.Substring(0,3)=="App"){Session.Contents[iLoop] = null;}
                        if (sName.Substring(0, 3) == "../") { Session.Contents[iLoop] = null; }
                }
            }
            m_User = Common.GetUser(this);
            try 
            {	        
	            Alert.alert(ref litAlert, m_User.Customer.CustomerId);
            }
            catch (Exception)
            {
                Response.Redirect(ConfigurationManager.AppSettings["Exit"]);         
            }
            try 
	        {
                if (m_User.FailedLogins > 0)
                {
                    if(m_User.Email.Length > 0 && m_User.Customer.ForcePasswordQuestion && m_User.QuestionID > -1)
                    {
                        try
                        {
                            Response.Redirect("ChangePassword.aspx?pwdreq=true");
                        }
                        catch (Exception DoNothing) {}
                    }
                    else
                    {
                        try
                        {
                            Response.Redirect(ConfigurationManager.AppSettings["Exit"]);   
                        }
                        catch (Exception DoNothing) { }                        
                    }
                }
                if (m_User.LoggedOn == false) 
                {
                    try
                    {
                        Response.Redirect(ConfigurationManager.AppSettings["Exit"]);
                    }
                    catch (Exception DoNothing) { }                     
                }
                if (m_User.SessionID != Session.SessionID.ToString())
                {
                    if (!m_User.Customer.AllowMultipleLogin)
                    {
                        Response.Redirect(ConfigurationManager.AppSettings["Exit"] + "?DoubleLogin=True");                          
                    }
                }
                m_App = new App(m_User);
                if (m_User.PasswordExpired  && m_User.InitialPassword)
                {
                    try
                    {
                        Response.Redirect("ChangePassword.aspx?pwdreq=true");
                    }
                    catch (Exception DoNothing) { }
                }
                else if (m_User.InitialPassword)
                {
                    try
                    {
                        Response.Redirect("FirstLogin.aspx");
                    }
                    catch (Exception DoNothing) { }
                }
                if (m_User.Email.Length > 0 && m_User.Customer.ForcePasswordQuestion && m_User.QuestionID == -1)
                {
                    try
                    {
                        Response.Redirect("ChangePassword.aspx?qstreq=true");
                    }
                    catch (Exception DoNothing) { }
                }

                if (m_User.HighestAdminLevel > AdminLevel.None && m_User.FirstLevelAdmin == false)
                {
                    try
                    {
                            Response.Redirect("../Admin/AdministrationMenu.aspx");
                    }
                    catch (Exception DoNothing) { }
                }
                String strStartMethod = null;
                Boolean blnStartMethod = false;
                String strStartMessage = null;
                if (m_User.Groups.HasGroups)
                {
                    strStartMethod = m_User.Groups[0].StartMethod;
                    if (strStartMethod != null && strStartMethod.Length > 0)
                    {
                        if (Session["StartMethodExecuted"] == null)
                        {
                            try
                            {
                                Response.Redirect(strStartMethod, false);
                            }
                            catch (Exception DoNothing)
                            {
                            }
                            blnStartMethod = true;
                            Session.Add("StartMethodExecuted", "X");
                        }
                        else
                        {
                            blnStartMethod = true;
                        }
                    }
                    else
                    {
                        blnStartMethod = true;
                    }
                    if (blnStartMethod && !String.IsNullOrEmpty(m_User.Groups[0].Message) && m_User.ReadMessageCount <= m_User.Groups[0].MaxReadMessageCount)
                    {
                        strStartMessage = m_User.Groups[0].Message;
                    }
                }

                // wenn keine Startmessage für Gruppe -> prüfen, ob Startmessage für Kunde vorhanden
                if ((String.IsNullOrEmpty(strStartMessage)) && (!String.IsNullOrEmpty(m_User.Customer.Message)) && (m_User.ReadCustomerMessageCount <= m_User.Customer.MaxReadMessageCount))
                {
                    strStartMessage = m_User.Customer.Message;
                }

                if (!String.IsNullOrEmpty(strStartMessage))
                {
                    ShowMessageDialog(strStartMessage);
                }

                Session["BackLink"] = null;

                if (!IsPostBack) 
                {
                    DataView dvAppLinks = m_User.Applications.DefaultView;
                    dvAppLinks.Sort = "AppRank";
                    dvAppLinks.RowFilter = "AppType='Change' AND AppInMenu=1";
                    MenuChangeSource = new DataView(m_User.Applications);
                    MenuChangeSource.RowFilter = "AppType='Change' AND AppInMenu=1";

                    dvAppLinks.RowFilter = "AppType='ChangeAH' AND AppInMenu=1";
                    MenuChangeAHSource = new DataView(m_User.Applications);
                    MenuChangeAHSource.RowFilter = "AppType='ChangeAH' AND AppInMenu=1";

                    dvAppLinks.RowFilter = "AppType='Report' AND AppInMenu=1";
                    MenuReportSource = new DataView(m_User.Applications);
                    MenuReportSource.RowFilter = "AppType='Report' AND AppInMenu=1";

                    dvAppLinks.RowFilter = "AppType='Tools' AND AppInMenu=1";
                    MenuToolsSource = new DataView(m_User.Applications);
                    MenuToolsSource.RowFilter = "AppType='Tools' AND AppInMenu=1";

                    ShowAnsprechpartner();
                }
	        }
	        catch (Exception ex)
	        {
                if (m_App == null){m_App = new App(m_User);}
                m_App.WriteErrorText(1, m_User.UserName, "Selection", "Page_Load", ex.ToString());
                lblError.Text = "Fehler bei der Ermittlung der Menüpunkte (" + ex.Message + ")";
                lblError.Visible = true;
	        }
        }

        private void ShowAnsprechpartner()
        {
            if ((m_User != null) && (m_User.Customer != null) && (m_User.Groups.Count > 0))
            {
                DataTable result = new DataTable();

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"]))
                {
                    conn.Open();

                    SqlDataAdapter daApp = new SqlDataAdapter("SELECT *, Name2 + ',' + Name1 + ' (' + [position] + ')' AS EmployeeName" +
                                                              " FROM Contact INNER JOIN ContactGroups ON Contact.id = ContactGroups.ContactID " +
                                                              " WHERE (ContactGroups.CustomerID = @CustomerID) AND (ContactGroups.GroupID = @GroupID)", conn);

                    daApp.SelectCommand.Parameters.AddWithValue("@CustomerID", m_User.Customer.CustomerId);
                    daApp.SelectCommand.Parameters.AddWithValue("@GroupID", m_User.GroupID);
                    daApp.Fill(result);

                    // Standard-Kontakt nur ersetzen, wenn für die Gruppe gepflegt
                    if (result.DefaultView.Count > 0)
                    {
                        DataRowView drv = result.DefaultView[0];
                        string strName = drv["Name1"] + " " + drv["Name2"];

                        if (!String.IsNullOrEmpty(strName))
                        {
                            lblKontaktdaten.Text = strName + "<br>"
                                               + "Tel " + drv["Telefon"] + "<br>"
                                               + "Fax " + drv["Fax"] + "<br>"
                                               + "E-Mail " + drv["Mail"];
                        }
                        else
                        {
                            lblKontaktdaten.Text = "Tel " + drv["Telefon"] + "<br>"
                                                   + "Fax " + drv["Fax"] + "<br>"
                                                   + "E-Mail " + drv["Mail"];
                        }

                        // Standard-Kontaktbild nur ersetzen, wenn für diesen Kontakt eines hinterlegt ist
                        if (!String.IsNullOrEmpty(drv["PictureName"].ToString()))
                        {
                            string pfad = ConfigurationManager.AppSettings["UploadPathContacts"];
                            imgAnsprechpartner.ImageUrl = pfad.Replace('\\', '/') + "responsible/" + drv["PictureName"];
                        }
                    }

                    conn.Close();
                }
            }
        }
        
        public string GetUrlString(string strAppUrl, string strAppID)
        {
            var paramlist = "";

            getAppParameters(strAppID, ref paramlist);
            if (strAppUrl.Substring(0, 4) == "http")
            {
                strAppUrl = (strAppUrl);
            }
            else
            {
                strAppUrl = MVC.MvcPrepareUrl(strAppUrl, strAppID, m_User.UserName);
            }

            return strAppUrl;
        }

        public bool getAppParameters(string strAppID, ref string paramlist)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand command = new SqlCommand();
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable result = new DataTable();

            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM ApplicationParamlist WHERE id_app = " + strAppID;
            conn.ConnectionString = ConfigurationManager.AppSettings["Connectionstring"].ToString();
            command.Connection = conn;

            try
            {
                conn.Open();
                adapter.SelectCommand = command;
                adapter.Fill(result);
                paramlist = string.Empty;
                if (result.Rows.Count != 0)
                {
                    paramlist = result.Rows[0]["paramlist"].ToString();
                }
                return true;
            }
            catch (Exception ex)
            {
                paramlist = string.Empty;
                return false;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            if (!e.IsFromDetailTable)
            {
                if (Session["objZLDSuche"]!= null)
                {
                    ZLD_Suche objZLDSuche = (ZLD_Suche)Session["objZLDSuche"];
                    RadGrid1.DataSource = objZLDSuche.AuftragsdatenStart;
                }   
            }
        }

        private void ShowMessageDialog(string text)
        {
            var cScript = "window.showModalDialog(\"LogonMessage.aspx\",null,\"dialogWidth:605px; dialogHeight:405px; center:yes; scroll:no;\");";

            Session["LOGONMSGDATA"] = text;

            Literal1.Text = "		<script language=\"JavaScript\">" + Environment.NewLine;
            Literal1.Text += "			<!-- //" + Environment.NewLine;
            Literal1.Text += cScript + Environment.NewLine;
            Literal1.Text += "			//-->" + Environment.NewLine;
            Literal1.Text += "		</script>" + Environment.NewLine;
            m_User.Groups[0].Message = "";
        }

    }
}