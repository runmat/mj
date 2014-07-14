using System;
using System.Data;
using System.Web.UI;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Vorgang Statusänderung
    /// </summary>
    public partial class VorgangStatusaenderung : Page
    {
        private User m_User;
        private App m_App;
        private StatusaenderungVorgang objStatusaenderung;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);

            m_App = new App(m_User); //erzeugt ein App_objekt 
            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            if (!IsPostBack)
            {
                objStatusaenderung = new StatusaenderungVorgang(ref m_User, m_App, "");
                objStatusaenderung.LoadStatuswerte(Session["AppID"].ToString(), Session.SessionID, this);
                Session["objStatusaenderung"] = objStatusaenderung;

                Title = lblHead.Text;
            }
            else if (Session["objStatusaenderung"] != null)
            {
                objStatusaenderung = (StatusaenderungVorgang)Session["objStatusaenderung"];
            }
        }

        private void Page_PreRender(object sender, EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        private void Page_Unload(object sender, EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            Response.Redirect("/PortalZLD/Start/Selection.aspx?AppID=" + Session["AppID"].ToString());
        }

        protected void NewSearch_Click(object sender, ImageClickEventArgs e)
        {
            Panel1.Visible = !Panel1.Visible;
            cmdCreate.Visible = !cmdCreate.Visible;
        }

        protected void cmdCreate_Click(object sender, EventArgs e)
        {
            DoSubmit();
        }

        private void DoSubmit()
        {
            lblError.Text = "";

            if (String.IsNullOrEmpty(txtSearchID.Text))
            {
                lblError.Text = "Bitte geben Sie eine ID an!";
                return;
            }

            objStatusaenderung.IDSuche = txtSearchID.Text.Trim();

            objStatusaenderung.LoadVorgang(Session["AppID"].ToString(), Session.SessionID, this);

            Session["objStatusaenderung"] = objStatusaenderung;

            if (objStatusaenderung.Status != 0)
            {
                lblError.Text = "Fehler: " + objStatusaenderung.Message;
            }
            else
            {
                FillForm();
            }
        }     

        private void FillForm()
        {
            Panel1.Visible = false;
            cmdCreate.Visible = false;

            var dvTemp = new DataView(objStatusaenderung.tblBEBStatusWerte);
            dvTemp.RowFilter = "DOMVALUE_L = '2' OR DOMVALUE_L = 'F' OR DOMVALUE_L = 'L' OR DOMVALUE_L = '" + objStatusaenderung.BEBStatus + "'";
            ddlBEBStatus.DataSource = dvTemp;
            ddlBEBStatus.DataTextField = "DDTEXT";
            ddlBEBStatus.DataValueField = "DOMVALUE_L";
            ddlBEBStatus.DataBind();

            lblIDDisplay.Text = objStatusaenderung.ID;
            lblBelegtypDisplay.Text = objStatusaenderung.Belegtyp;
            if (objStatusaenderung.Zulassungsdatum.HasValue)
            {
                lblZulassungsdatumDisplay.Text = objStatusaenderung.Zulassungsdatum.Value.ToShortDateString();
            }
            else
            {
                lblZulassungsdatumDisplay.Text = "";
            }
            lblKundennummerDisplay.Text = objStatusaenderung.Kundennummer;
            lblKreisDisplay.Text = objStatusaenderung.Kreis;
            lblKennzeichenDisplay.Text = objStatusaenderung.Kennzeichen;
            lblBEBStatusDisplay.Text = objStatusaenderung.BEBStatusText;
            ddlBEBStatus.SelectedValue = objStatusaenderung.BEBStatus;

            Panel2.Visible = true;
            cmdSave.Visible = true;
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            SaveStatus();
        }

        private void SaveStatus()
        {
            try
            {
                objStatusaenderung.BEBStatusNeu = ddlBEBStatus.SelectedValue;

                objStatusaenderung.SaveVorgang(Session["AppID"].ToString(), Session.SessionID, this);

                if (objStatusaenderung.Status != 0)
                {
                    lblError.Text = "Fehler beim Speichern: " + objStatusaenderung.Message;
                }
                else
                {
                    lblError.Text = "Der neu Vorgangs-Status wurde erfolgreich gespeichert!";
                    objStatusaenderung.IDSuche = "";
                    Panel2.Visible = false;
                    cmdSave.Visible = false;
                    Panel1.Visible = true;
                    cmdCreate.Visible = true;
                }

                Session["objStatusaenderung"] = objStatusaenderung;
            }
            catch (Exception ex)
            {
                lblError.Text = "Fehler beim Speichern: " + ex.Message;
            }
        }
    }
}
