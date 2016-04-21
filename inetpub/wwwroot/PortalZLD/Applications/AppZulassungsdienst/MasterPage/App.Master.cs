using System;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Text.RegularExpressions;

namespace AppZulassungsdienst.MasterPage
{
    public partial class App : System.Web.UI.MasterPage
    {
        private CKG.Base.Kernel.Security.User m_User;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!Scriptmanager1.IsInAsyncPostBack) 
            { 
                string strDocuPath = "";
                HttpBrowserCapabilities bc = Request.Browser;

                //Aktuelles Jahr ins Copyright setzen.
                lblCopyright.Text = lblCopyright.Text.Replace("year", DateTime.Now.Year.ToString());
                lblHeaderHint.Text = ConfigurationManager.AppSettings["PortalHeaderHint"];
                lnkLogout.NavigateUrl = "/PortalZLD/(S(" + Session.SessionID + "))/Start/Logout.aspx";
                m_User = (CKG.Base.Kernel.Security.User)Session["objUser"];
                if (m_User == null)
                {
                    if (!IsPostBack)
                    {
                        tdHandbuch.Visible = false;
                        lnkHauptmenue.Text = "Anmeldung";
                        tdChangePasword.Visible = false;
                        lblUserName.Visible = false;
                        imgLogo.Visible = false;

                        PlaceHeader.Visible = true;
                    }
                }
                else
                {
                    imgLogo.Visible = true;
                    PlaceHeader.Visible = false;
                    lnkHauptmenue.NavigateUrl = "/PortalZLD/(S(" + Session.SessionID + "))/Start/Selection.aspx";
                    lnkChangePassword.NavigateUrl = "/PortalZLD/(S(" + Session.SessionID + "))/Start/ChangePassword.aspx";
                    lnkLogout.NavigateUrl = "/PortalZLD/(S(" + Session.SessionID + "))/Start/Logout.aspx";
                    lnkContact.NavigateUrl = "/PortalZLD/(S(" + Session.SessionID + "))/Info/ContactPage.aspx";
                    lnkResponsible.NavigateUrl = "/PortalZLD/(S(" + Session.SessionID + "))/Info/ResponsiblePage.aspx";
                    lnkImpressum.NavigateUrl = "/PortalZLD/(S(" + Session.SessionID + "))/Info/Impressum.aspx";

                    PlaceHeader.Visible = false;

                    lnkChangePassword.Visible = true;
                    lblBenutzer.Text = "Benutzer: ";
                    if (m_User.IsLeiterZLD)
                    {
                        lblUserName.Text = m_User.UserName + " (Kst.: " + m_User.Kostenstelle + ")";
                    }
                    else
                    {
                        lblUserName.Text = m_User.UserName;
                    }  
                    lnkLogout.Visible = true;
                    lnkChangePassword.Visible = true;

                    string strCSSLink;
                    {
                        if (bc.Type == "IE6")
                        {

                            strCSSLink = "<link href=\"/PortalZLD/Styles/defaultIE6.css\" media=\"screen, projection\" type=\"text/css\" rel=\"stylesheet\" />";

                            string strCSSPath = m_User.Customer.CustomerStyle.CssPath;
                            if (strCSSPath.Contains(".css"))
                            {
                                string[] strCSS = Regex.Split(strCSSPath, ".css");

                                if (strCSS.Length == 2)
                                {
                                    strCSSPath = strCSS[0] + "IE6.css";
                                    strCSSLink += "<link href=\"" + strCSSPath + "\" media=\"screen, projection\" type=\"text/css\" rel=\"stylesheet\" />";
                                }
                            }
                        }
                        else
                        {
                            strCSSLink = "<link href=\"/PortalZLD/Styles/default.css\" media=\"screen, projection\" type=\"text/css\" rel=\"stylesheet\" />";
                            strCSSLink += "<link href=\"" + m_User.Customer.CustomerStyle.CssPath + "\" media=\"screen, projection\" type=\"text/css\" rel=\"stylesheet\" />";
                        }
                    }
                    this.Head1.Controls.Add(new LiteralControl(strCSSLink));

                    if (HttpContext.Current.Request.UserAgent != null && HttpContext.Current.Request.UserAgent.ToLower().Contains("msie 10"))
                    {
                        this.Head1.Controls.Add(new LiteralControl("<META content=\"IE=9,chrome=1\" http-equiv=\"X-UA-Compatible\">"));
                    } 

                    if (this.Page.User.Identity.IsAuthenticated)
                    {
                        switch (this.Page.Title)
                        {
                            case "Startseite":
                                tdHauptmenue.Attributes.Add("class", "active");
                                tdChangePasword.Attributes.Add("class", ""); 
                                tdContact.Attributes.Add("class", "");
                                tdImpressum.Attributes.Add("class", "");
                                tdResponsible.Attributes.Add("class", "");

                                break;
                            case "Passwort ändern":
                                tdHauptmenue.Attributes.Add("class", "");
                                tdChangePasword.Attributes.Add("class", "active");
                                tdContact.Attributes.Add("class", "");
                                tdImpressum.Attributes.Add("class", "");
                                tdResponsible.Attributes.Add("class", "");
                                break;
                            case "Kontaktseite":
                                tdHauptmenue.Attributes.Add("class", "");
                                tdChangePasword.Attributes.Add("class", "");
                                tdContact.Attributes.Add("class", "active");
                                tdImpressum.Attributes.Add("class", "");
                                tdResponsible.Attributes.Add("class", "");
                                break;
                            case "Impressum":
                                tdHauptmenue.Attributes.Add("class", "");
                                tdChangePasword.Attributes.Add("class", ""); 
                                tdContact.Attributes.Add("class", "");
                                tdImpressum.Attributes.Add("class", "active");
                                tdResponsible.Attributes.Add("class", "");
                                break;
                            case "Ansprechpartner":
                                tdHauptmenue.Attributes.Add("class", "");
                                tdChangePasword.Attributes.Add("class", ""); 
                                tdContact.Attributes.Add("class", "");
                                tdImpressum.Attributes.Add("class", "");
                                tdResponsible.Attributes.Add("class", "active");
                                break;
                            default:
                                tdHauptmenue.Attributes.Add("class", "");
                                tdChangePasword.Attributes.Add("class", "");
                                tdContact.Attributes.Add("class", "");
                                tdImpressum.Attributes.Add("class", "");
                                tdResponsible.Attributes.Add("class", "");
                                break;
                        }

                        if (this.Page.User.Identity.IsAuthenticated)
                        {
                            string strTitle;

                            if (this.Page.Title == "Startseite")
                            {
                                strTitle = m_User.Customer.CustomerName + " - " + "Startseite";

                                this.Page.Title = strTitle;
                            }
                            else if ((Session["AppID"] == null) || (Session["AppID"].ToString() == "0"))
                            {
                                this.Page.Title = m_User.Customer.CustomerName;
                            }
                            else
                            {
                                strTitle = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

                                this.Page.Title = m_User.Customer.CustomerName + " - " + strTitle;
                            }
                        }
                    }

                    if (m_User.GroupID > 0)
                    {
                        strDocuPath = m_User.Groups.get_ItemByID(m_User.GroupID).DocuPath;

                        System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(m_User.App.Connectionstring);

                        CKG.Base.Kernel.Admin.EmployeeList _EmployeeAssigned = new CKG.Base.Kernel.Admin.EmployeeList(m_User.GroupID, m_User.Customer.AccountingArea, cn);
                        _EmployeeAssigned.GetAssigned();
                        if (_EmployeeAssigned.DefaultView.Count > 0)
                        {
                            _EmployeeAssigned.DefaultView.Sort = "EmployeeName";
                            tdResponsible.Visible = true;
                        }
                        else
                        {
                            tdResponsible.Visible = false;
                        }
                    }
                    if (strDocuPath == string.Empty)
                    {
                        strDocuPath = m_User.Customer.DocuPath;
                    }
                    if (!String.IsNullOrEmpty(strDocuPath))
                    {
                        tdHandbuch.Visible = true;
                        lnkHandbuch.NavigateUrl = strDocuPath.Replace("Applications/AppZulassungsdienst/" , "");
                    }
                }
            }
        }
    }
}
