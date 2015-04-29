using System;
using System.Linq;
using System.Web.UI;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Selektion Übersicht offener Versandzulassungen und Rechnungsprüfung.
    /// </summary>
    public partial class ChangeStatusVersand : Page
    {
        private User m_User;
        private VorVersand objVersandZul;
        private ZLDCommon objCommon;

        #region Events

        protected void Page_Init(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            Common.GetAppIDFromQueryString(this);
            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            if (String.IsNullOrEmpty(m_User.Reference))
            {
                lblError.Text = "Es wurde keine Benutzerreferenz angegeben! Somit können keine Stammdaten ermittelt werden!";
                return;
            }
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

            InitLargeDropdowns();
            InitJava();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                objVersandZul = new VorVersand(m_User.Reference);
            }
            else
            {
                objVersandZul = (VorVersand)Session["objVersandZul"];
                objVersandZul.SelID = txtID.Text;
                objVersandZul.SelKunde = txtKunnr.Text;
                objVersandZul.SelKreis = txtStVa.Text;
                objVersandZul.SelLief = ddlLief.SelectedValue;
                objVersandZul.SelDatum = ZLDCommon.toShortDateStr(txtZulDate.Text);
                objVersandZul.SelDatumBis = ZLDCommon.toShortDateStr(txtZulDateBis.Text);

                if (rbAuswahl1.Checked) { objVersandZul.SelStatus = "1"; }
                if (rbAuswahl2.Checked) { objVersandZul.SelStatus = "2"; }
                if (rbAuswahl3.Checked) { objVersandZul.SelStatus = "3"; }
            }

            Session["objVersandZul"] = objVersandZul;
        }

        /// <summary>
        /// Auswahl/Eingabe des zu selektierenden Kreises. Laden des 
        /// zuständigen ZLD/externen Dienstleiters.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void ddlStVa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (String.Compare(objVersandZul.SelKreis, ddlStVa.SelectedValue) != 0)
            {
                txtStVa.Text = ddlStVa.SelectedValue;
                objVersandZul.SelKreis = txtStVa.Text;

                if (objVersandZul.BestLieferanten == null)
                {
                    objVersandZul.getBestLieferant();
                }

                if (objVersandZul.ErrorOccured)
                {
                    ddlLief.Items.Clear();
                    lblError.Text = "Fehler beim Laden der Lieferanten/Zulassungsdienste!";
                    Session["objVersandZul"] = objVersandZul;
                }
                else
                {
                    ddlLief.DataSource = objVersandZul.BestLieferanten;
                    ddlLief.DataValueField = "LIFNR";
                    ddlLief.DataTextField = "NAME1";
                    ddlLief.DataBind();
                    ddlLief.SelectedValue = "0";
                    Session["objNacherf"] = objVersandZul;
                }
            }
        }

        /// <summary>
        /// Auswahl/Eingabe des zu selektierenden Kunden.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void ddlKunnr_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtKunnr.Text = (ddlKunnr.SelectedValue == "0" ? "" : ddlKunnr.SelectedValue);
        }

        /// <summary>
        /// Selektionsdaten an SAP übergeben
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdCreate_Click(object sender, EventArgs e)
        {
            lblError.Text = "";

            objVersandZul.FillVersanZul(objCommon.KundenStamm);
            Session["objVersandZul"] = objVersandZul;

            if (objVersandZul.ErrorOccured || objVersandZul.Liste.Rows.Count == 0)
            {
                lblError.Text = objVersandZul.Message;
            }
            else
            {
                Response.Redirect("ChangeStatusVersandList.aspx?AppID=" + Session["AppID"].ToString());
            }
        }

        /// <summary>
        /// Enter-Button-Dummy.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">ImageClickEventArgs</param>
        protected void btnEmpty_Click(object sender, ImageClickEventArgs e)
        {
            cmdCreate_Click(sender, e);
        }

        /// <summary>
        /// Auswahl/Eingabe des zu selektierenden Kreises. Laden des 
        /// zuständigen ZLD/externen Dienstleiters.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void txtStVa_TextChanged(object sender, EventArgs e)
        {
            objVersandZul.SelKreis = txtStVa.Text.ToUpper();
            ddlStVa.SelectedValue = txtStVa.Text.ToUpper();
            txtStVa.Text = txtStVa.Text.ToUpper();

            if (!String.IsNullOrEmpty(objVersandZul.SelKreis))
            {
                objVersandZul.getBestLieferant();

                if (objVersandZul.ErrorOccured)
                {
                    ddlLief.Items.Clear();
                    lblError.Text = "Fehler beim Laden der Lieferanten/Zulassungsdienste!";
                    Session["objNacherf"] = objVersandZul;
                }
                else
                {
                    ddlLief.DataSource = objVersandZul.BestLieferanten;
                    ddlLief.DataValueField = "LIFNR";
                    ddlLief.DataTextField = "NAME1";
                    ddlLief.DataBind();
                    ddlLief.SelectedValue = "0";
                    Session["objNacherf"] = objVersandZul;
                }
            }
            else
            {
                ddlLief.Items.Clear();
            }

            ddlStVa.Focus();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Dropdowns mit großen Datenmengen (ohne ViewState!)
        /// </summary>
        private void InitLargeDropdowns()
        {
            //Kunde
            ddlKunnr.DataSource = objCommon.KundenStamm.Where(k => !k.Inaktiv).ToList();
            ddlKunnr.DataValueField = "KundenNr";
            ddlKunnr.DataTextField = "Name";
            ddlKunnr.DataBind();

            //StVa
            ddlStVa.DataSource = objCommon.StvaStamm;
            ddlStVa.DataValueField = "Landkreis";
            ddlStVa.DataTextField = "Bezeichnung";
            ddlStVa.DataBind();
        }

        private void InitJava()
        {
            txtKunnr.Attributes.Add("onkeyup", "FilterItems(this.value," + ddlKunnr.ClientID + ")");
            txtKunnr.Attributes.Add("onblur", "SetDDLValue(this," + ddlKunnr.ClientID + ")");
        }

        #endregion
    }
}
