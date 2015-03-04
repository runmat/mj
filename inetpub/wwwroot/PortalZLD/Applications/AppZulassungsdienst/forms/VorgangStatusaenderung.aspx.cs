using System;
using System.Data;
using System.Linq;
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
        private StatusaenderungVorgang objStatusaenderung;
        private ZLDCommon objCommon;

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            if (Session["objCommon"] == null)
            {
                objCommon = new ZLDCommon(m_User.Reference);
                objCommon.getSAPDatenStamm();
                objCommon.getSAPZulStellen();
                objCommon.LadeKennzeichenGroesse();
                Session["objCommon"] = objCommon;
            }
            else
            {
                objCommon = (ZLDCommon)Session["objCommon"];
            }

            if (!IsPostBack)
            {
                objStatusaenderung = new StatusaenderungVorgang(m_User.Reference);
                objStatusaenderung.LoadStatuswerte();
                objStatusaenderung.LoadBelegtypen();
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

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            SaveStatus();
        }

        #endregion

        #region Methods

        private void DoSubmit()
        {
            lblError.Text = "";

            if (String.IsNullOrEmpty(txtSearchID.Text))
            {
                lblError.Text = "Bitte geben Sie eine ID an!";
                return;
            }

            objStatusaenderung.IDSuche = txtSearchID.Text.Trim();

            objStatusaenderung.LoadVorgang();

            Session["objStatusaenderung"] = objStatusaenderung;

            if (objStatusaenderung.ErrorOccured)
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
            lblBelegtypDisplay.Text = objStatusaenderung.BelegtypLangtext;
            if (objStatusaenderung.Zulassungsdatum.HasValue)
            {
                lblZulassungsdatumDisplay.Text = objStatusaenderung.Zulassungsdatum.Value.ToShortDateString();
            }
            else
            {
                lblZulassungsdatumDisplay.Text = "";
            }
            lblKundennummerDisplay.Text = objStatusaenderung.Kundennummer.TrimStart('0');

            var kunde = objCommon.KundenStamm.FirstOrDefault(k => k.KundenNr == objStatusaenderung.Kundennummer.TrimStart('0'));

            lblKundeDisplay.Text = (kunde != null ? kunde.Name1 : "");

            lblKreisDisplay.Text = objStatusaenderung.Kreis;
            lblKennzeichenDisplay.Text = objStatusaenderung.Kennzeichen;
            lblBEBStatusDisplay.Text = objStatusaenderung.BEBStatusText;
            ddlBEBStatus.SelectedValue = objStatusaenderung.BEBStatus;

            Panel2.Visible = true;
            cmdSave.Visible = true;
        }

        private void SaveStatus()
        {
            try
            {
                objStatusaenderung.BEBStatusNeu = ddlBEBStatus.SelectedValue;

                objStatusaenderung.SaveVorgang();

                if (objStatusaenderung.ErrorOccured)
                {
                    lblError.Text = "Fehler beim Speichern: " + objStatusaenderung.Message;
                }
                else
                {
                    lblError.Text = "Der neue Vorgangs-Status wurde erfolgreich gespeichert!";
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

        #endregion
    }
}
