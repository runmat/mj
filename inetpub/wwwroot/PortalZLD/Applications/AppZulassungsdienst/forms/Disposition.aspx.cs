using System;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using GeneralTools.Models;
using GeneralTools.Services;

namespace AppZulassungsdienst.forms
{
    public partial class Disposition : System.Web.UI.Page
    {
        private User m_User;
        private clsDisposition objDispo;

        protected string DispoModus { get { return (objDispo == null ? "" : objDispo.Modus); } }

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);

            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            lblError.Text = "";
            lblMessage.Text = "";

            if (Session["objDispo"] == null)
            {
                objDispo = new clsDisposition(m_User.Reference, m_User.UserName) { Modus = "1" };
                Session["objDispo"] = objDispo;
            }
            else
            {
                objDispo = (clsDisposition)Session["objDispo"];
            }

            if (!IsPostBack)
            {
                FillForm();
                SelectionChanged();
            }
            else
            {
                if (Request.Params["__EVENTTARGET"] == "txtZulDate")
                    SelectionChanged();
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

        /// <summary>
        /// Für jede Gridzeile die Fahrer-Dropdownliste füllen/initialisieren
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDispositionen_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    if (e.Row.DataItem != null)
                    {
                        var lbl = (Label)e.Row.FindControl("lblAmt");
                        var ddl = (DropDownList)e.Row.FindControl("ddlFahrer");
                        ddl.DataSource = objDispo.Fahrerliste;
                        ddl.DataValueField = "UserId";
                        ddl.DataTextField = "UserName";
                        ddl.DataBind();
                        var dispo = objDispo.Dispositionen.FirstOrDefault(d => d.Amt == lbl.Text);
                        if (dispo != null)
                        {
                            var mobUser = dispo.MobileUserId;
                            ddl.SelectedValue = (string.IsNullOrEmpty(mobUser) ? "0" : mobUser);
                        }
                    }
                    break;

                case DataControlRowType.Footer:
                    e.Row.Cells[0].Text = "Gesamt:";
                    e.Row.Cells[3].Text = objDispo.Dispositionen.Sum(d => d.AnzahlVorgaenge).ToString();
                    e.Row.Cells[5].Text = objDispo.Dispositionen.Sum(d => d.GebuehrAmt).ToString("f");
                    break;
            }
        }

        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            Response.Redirect("/PortalZLD/Start/Selection.aspx?AppID=" + Session["AppID"].ToString());
        }

        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            SelectionChanged();
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            DatenSpeichern(false);
        }

        protected void cmdSend_Click(object sender, EventArgs e)
        {
            DatenSpeichern(true);
        }

        protected void OnSelectionChanged(object sender, EventArgs e)
        {
            SelectionChanged();
        }

        #endregion

        #region Methods

        private void FillForm()
        {
            // Morgiges Datum als Default-Wert setzen, dabei ggf. Wochenenden/Feiertage überspringen
            var morgen = SkipWeekendsAndHolidays(DateTime.Today.AddDays(1));
            txtZulDate.Text = morgen.ToString("ddMMyy");

            // Bei Änderung des gewählten Zulassungsdatums Daten neu einlesen
            txtZulDate.Attributes.Add("onblur", "__doPostBack('txtZulDate', '');");
            lbtnGestern.Attributes.Add("onclick", "SetDate( -1,'" + txtZulDate.ClientID + "');__doPostBack('txtZulDate', '');");
            lbtnHeute.Attributes.Add("onclick", "SetDate( 0,'" + txtZulDate.ClientID + "');__doPostBack('txtZulDate', '');");
            lbtnMorgen.Attributes.Add("onclick", "SetDate( +1,'" + txtZulDate.ClientID + "');__doPostBack('txtZulDate', '');");
        }

        private void Fillgrid()
        {
            if (objDispo.Dispositionen.None())
            {
                cmdSave.Enabled = false;
                cmdSend.Enabled = false;

                lblError.Text = "Keine disponierbaren Vorgänge für den " + objDispo.ZulDat + " vorhanden!";
            }
            else
            {
                cmdSave.Enabled = true;
                cmdSend.Enabled = true;
            }

            gvDispositionen.DataSource = objDispo.Dispositionen;
            gvDispositionen.DataBind();
        }

        /// <summary>
        /// Zulassungsdatum aus Textbox in Objekt übernehmen
        /// </summary>
        private bool ApplyZulDate()
        {
            DateTime tmpDatum;
            if (!DateTime.TryParseExact(txtZulDate.Text, "ddMMyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out tmpDatum))
                return false;

            objDispo.ZulDat = tmpDatum.ToString("dd.MM.yyyy");

            return true;
        }

        private DateTime SkipWeekendsAndHolidays(DateTime datum)
        {
            var tmpdat = datum;

            for (var i = 0; i < 5; i++)
            {
                if ((tmpdat.DayOfWeek == DayOfWeek.Saturday) || (tmpdat.DayOfWeek == DayOfWeek.Sunday) || (IstFeiertag(tmpdat)))
                    tmpdat = tmpdat.AddDays(1);
                else
                    break;
            }

            return tmpdat;
        }

        private static bool IstFeiertag(DateTime datum)
        {
            var dfej = new DeutscheFeiertageEinesJahres(datum.Year);

            return dfej.Feiertage.Any(ft => ft.Datum.Date == datum.Date);
        }

        private void SelectionChanged()
        {
            chkAlleAemter.Visible = rbNichtDisponiert.Checked;

            if (ApplyZulDate())
            {
                if (rbBereitsDisponiert.Checked)
                    objDispo.Modus = "2";
                else if (rbBereitsInArbeit.Checked)
                    objDispo.Modus = "3";
                else
                    objDispo.Modus = "1";

                objDispo.AlleAemter = (rbNichtDisponiert.Checked && chkAlleAemter.Checked);

                objDispo.LoadDispos();
                Session["objDispo"] = objDispo;
                Fillgrid();
            }
        }

        private void DatenSpeichern(bool senden)
        {
            var disponierte = 0;
            var nichtDisponierte = "";

            // Eingaben aus Grid übernehmen
            foreach (GridViewRow gRow in gvDispositionen.Rows)
            {
                var lblAmt = (Label)gRow.FindControl("lblAmt");
                var ddlFahrer = (DropDownList)gRow.FindControl("ddlFahrer");
                var chkVorschuss = (CheckBox)gRow.FindControl("chkVorschuss");
                var txtVorschussBetrag = (TextBox)gRow.FindControl("txtVorschussBetrag");

                var dispo = objDispo.Dispositionen.FirstOrDefault(d => d.Amt == lblAmt.Text);
                if (dispo != null)
                {
                    if (ddlFahrer.SelectedItem != null)
                    {
                        if (ddlFahrer.SelectedItem.Value == "0")
                        {
                            dispo.MobileUserId = "";
                            dispo.MobileUserName = "";
                            if (string.IsNullOrEmpty(nichtDisponierte))
                                nichtDisponierte = lblAmt.Text;
                            else
                                nichtDisponierte += ", " + lblAmt.Text;
                        }
                        else
                        {
                            dispo.MobileUserId = ddlFahrer.SelectedItem.Value;
                            dispo.MobileUserName = ddlFahrer.SelectedItem.Text;
                            disponierte++;
                        }
                    }

                    dispo.Vorschuss = chkVorschuss.Checked;
                    dispo.VorschussBetrag = txtVorschussBetrag.Text.ToNullableDecimal();
                }
            }

            if (objDispo.Dispositionen.Any(d => d.Vorschuss && (!d.VorschussBetrag.HasValue || d.VorschussBetrag.Value <= 0)))
            {
                Session["objDispo"] = objDispo;
                lblError.Text = "Bitte geben Sie für alle Ämter, bei denen Sie 'Vorschuss' gewählt haben, die Höhe des Vorschusses an";
                return;
            }

            // Daten speichern (nur dann wirklich an SAP senden, wenn disponiert)
            var datenAnSapSenden = (senden && disponierte > 0);

            objDispo.SaveDispos(datenAnSapSenden);

            if (objDispo.ErrorOccured)
            {
                lblError.Text = "Fehler beim " + (datenAnSapSenden ? "Absenden" : "Speichern") + " der Daten: " + objDispo.Message;
            }
            else
            {
                lblMessage.Text = "Daten erfolgreich " + (datenAnSapSenden ? "an SAP übertragen" : "gespeichert");

                if (datenAnSapSenden)
                {
                    if (objDispo.Dispositionen.Any(d => !string.IsNullOrEmpty(d.SaveError)))
                    {
                        lblError.Text = "Achtung! Einige Dispositionen konnten nicht gespeichert werden";
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(nichtDisponierte))
                            lblError.Text = "Achtung! Für die Ämter " + nichtDisponierte + " wurde noch kein Fahrer ausgewählt";

                        objDispo.LoadDispos();
                    }

                    Fillgrid();
                }
                else if (senden)
                {
                    lblError.Text = "Es wurden keine Sätze an SAP übertragen, weil für keines der Ämter ein Fahrer ausgewählt wurde";
                }
            }

            Session["objDispo"] = objDispo;
        }

        #endregion
    }
}