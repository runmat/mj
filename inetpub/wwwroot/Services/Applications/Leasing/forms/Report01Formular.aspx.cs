using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Business;
using CKG.Base.Kernel.Security;
using System.Configuration;


namespace Leasing.forms
{
    public partial class Report01Formular : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private DataTable m_objTable;
        private String strKennzeichen;
        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            try
            {
                String AppName = Request.AppRelativeCurrentExecutionFilePath;
                if (AppName == "Selection" && m_User.Applications.Select(AppName).Length == 0 )
                {
                    lblError.Visible = true;
                }
                else if (Request.QueryString["strKennzeichen"] == null)
                {
                    lblError.Visible = true;
                }
                else 
                {
                    strKennzeichen = Request.QueryString["strKennzeichen"].ToString();
                    m_objTable = (DataTable)(Session["ResultTable"]);
                    m_App = new App(m_User);
                }
                if (IsPostBack == false)
                {

                    DataRow[] rows = m_objTable.Select("Kennzeichen='" + strKennzeichen + "'");
                    if (rows.Length > 0)
                    {
                        if (rows[0]["Kennzeichen"] != null && rows[0]["Kennzeichen"].ToString().Trim().Length>0)
                        {
                            lblFahrzeugkennzeichen.Text =rows[0]["Kennzeichen"].ToString().Trim();
                        }
                        if (rows[0]["Fahrzeugart"] != null && rows[0]["Fahrzeugart"].ToString().Trim().Length > 0)
                        {
                            lblFahrzeugUndAufbauart.Text = rows[0]["Fahrzeugart"].ToString().Trim();
                        }
                        if (rows[0]["Hersteller"] != null && rows[0]["Hersteller"].ToString().Trim().Length > 0)
                        {
                            lblHersteller.Text = rows[0]["Hersteller"].ToString().Trim();
                        }
                        if (rows[0]["TypSchlüssel"] != null && rows[0]["TypSchlüssel"].ToString().Trim().Length > 0)
                        {
                            lblTypUndAusfuehrung.Text = "Typ Schlüssel: " + rows[0]["TypSchlüssel"].ToString().Trim();
                        }
                        if (rows[0]["Ausführung"] != null && rows[0]["Ausführung"].ToString().Trim().Length > 0)
                        {
                            if (lblTypUndAusfuehrung.Text.EndsWith("T"))
                            {
                                lblTypUndAusfuehrung.Text += ", Ausführung: " + rows[0]["Ausführung"].ToString().Trim();
                            }
                            else 
                            {
                                lblTypUndAusfuehrung.Text = "Ausführung: " + rows[0]["Ausführung"].ToString().Trim();
                            }
                            
                        }
                        if (rows[0]["Fahrgestellnummer"] != null && rows[0]["Fahrgestellnummer"].ToString().Trim().Length > 0)
                        {
                            lblFIN.Text = rows[0]["Fahrgestellnummer"].ToString().Trim();
                        }
                        if (rows[0]["Name"] != null && rows[0]["Name"].ToString().Trim().Length > 0)
                        {
                            lblName.Text = rows[0]["Name"].ToString().Trim();
                        }
                        if (rows[0]["Postleitzahl"] != null && rows[0]["Postleitzahl"].ToString().Trim().Length > 0)
                        {
                            lblWohnhaft.Text = rows[0]["Postleitzahl"].ToString().Trim();
                        }
                        if (rows[0]["Straße"] != null && rows[0]["Straße"].ToString().Trim().Length > 0)
                        {
                            lblWohnhaft.Text += ", " + rows[0]["Straße"].ToString().Trim();
                        }
                        if (rows[0]["AnzahlSchilder"].ToString().Trim()=="V")
                        {
                            CheckBox1.Checked = false;
                        }
                        if (rows[0]["AnzahlSchilder"].ToString().Trim() == "H")
                        {
                            CheckBox2.Checked = false;
                        }
                    }
                    else
                    {
                        lblError.Visible = true;
                    }
                
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
