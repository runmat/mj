using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using AppZulassungsdienst.lib;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using System.Data;
using System.Web.UI;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Anzeige und Pflege der Kassenbestände
    /// </summary>
    public partial class KassenabrechnungNeu : Page
    {
        private User _mUser;
        private Kassenabrechnung _objKassenabrechnung;

        #region Events

        /// <summary>
        /// Page_Load Ereignis. Prüfen ob die Anwendung dem Benutzer zugeordnet ist. Evtl. Stammdaten laden.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            _mUser = Common.GetUser(this);
            Common.FormAuth(this, _mUser);
            Common.GetAppIDFromQueryString(this);
            lblHead.Text = (string)_mUser.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            lblError.Text = "";
            if (String.IsNullOrEmpty(_mUser.Reference))
            {
                lblErrorMain.Text = "Es wurde keine Benutzerreferenz angegeben! Somit können keine Stammdaten ermittelt werden!";
                return;
            }
            if (!IsPostBack)
            {
                _objKassenabrechnung = new Kassenabrechnung(_mUser.Reference);
                Session["objKassenabrechnung"] = _objKassenabrechnung;

                _objKassenabrechnung.GetPeriodeFromDateNeu(DateTime.Today);
                if (!_objKassenabrechnung.ErrorOccured)
                {
                    txtStartDate.Text = _objKassenabrechnung.DatumVon.ToShortDateString();
                    txtEndDate.Text = _objKassenabrechnung.DatumBis.ToShortDateString();

                    FillWerte();
                }
                else
                {
                    lblErrorMain.Text = _objKassenabrechnung.Message;
                }
            }
            else
            {
                if (Session["objKassenabrechnung"] == null)
                    Session["objKassenabrechnung"] = new Kassenabrechnung(_mUser.Reference);

                _objKassenabrechnung = (Kassenabrechnung)Session["objKassenabrechnung"];
                ScriptManager.RegisterStartupScript(phrJsRunner, phrJsRunner.GetType(), "jsSetScrollPos",
                                                  "SetScrollPos();", true);
            }
        }

        /// <summary>
        /// Page_Unload Ereignis. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Unload(object sender, EventArgs e)
        {
            Session["objKassenabrechnung"] = _objKassenabrechnung;
        }

        /// <summary>
        /// Auf Eingaben im Grid reagieren. Löschen, Splitten und Bestätigen von Barzahlungen der Vorgänge.
        /// Kopfbereich aktualisieren,
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewCommandEventArgs</param>
        protected void gvDaten_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Del":
                    lblMessage.Text = "";
                    _objKassenabrechnung.DeleteHead2(e.CommandArgument.ToString());
                    // Refresh after Delete
                    DataTable table = _objKassenabrechnung.DocHeads;
                    gvDaten.DataSource = table.DefaultView;
                    gvDaten.DataBind();
                    visibility();
                    addButtonAttr(table, gvDaten);
                    FillWerte();
                    if (_objKassenabrechnung.ErrorOccured)
                    {
                        lblError.Text = _objKassenabrechnung.Message;
                    }
                    break;

                case "Split":
                    lblMessage.Text = "";
                    DataRow[] headRow =
                    _objKassenabrechnung.DocHeads.Select("POSTING_NUMBER='" + e.CommandArgument + "'");
                    if (!proofVorfallGridRow(ref headRow[0]))
                    {
                        ScriptManager.RegisterStartupScript(phrJsRunner, phrJsRunner.GetType(), "jsopenkDialog",
                                                            "openDialogAndBlock(' ');", true);

                        DataRow[] pos = _objKassenabrechnung.DocPos.Select("POSTING_NUMBER='" + e.CommandArgument + "'");

                        hfPostingNumber.Value = e.CommandArgument.ToString();

                        // neuer Kopf ohne Position ? neue Posistion anlegen
                        if (pos.Length == 0)
                        {
                            AddNewPosForSave(e.CommandArgument.ToString(), headRow[0]);
                            pos = _objKassenabrechnung.DocPos.Select("POSTING_NUMBER='" + e.CommandArgument + "'");
                        }

                        switch (_objKassenabrechnung.VorfallGewaehlt)
                        {
                            case Kassenabrechnung.VorfallFilter.Einahmen:
                                hfGesamt.Value = String.Format("{0:N}", headRow[0]["H_RECEIPTS"]);
                                lblGesamtShow.Text = String.Format("{0:N}", headRow[0]["H_RECEIPTS"]);
                                if (pos.Length == 1)
                                {
                                    pos[0]["KUNNR"] = headRow[0]["KUNNR"];
                                }
                                break;
                            case Kassenabrechnung.VorfallFilter.Ausgaben:
                                hfGesamt.Value = String.Format("{0:N}", headRow[0]["H_PAYMENTS"]);
                                lblGesamtShow.Text = String.Format("{0:N}", headRow[0]["H_PAYMENTS"]);
                                if (pos.Length == 1)
                                {
                                    pos[0]["LIFNR"] = headRow[0]["LIFNR"];
                                }
                                break;
                        }

                        if (pos.Length == 1)
                        {
                            pos[0]["SGTXT"] = headRow[0]["SGTXT"];
                            pos[0]["ALLOC_NMBR"] = headRow[0]["ZUONR"];
                        }
                        DataView posRows = _objKassenabrechnung.DocPos.DefaultView;
                        posRows.RowFilter = "POSTING_NUMBER='" + e.CommandArgument + "'";

                        GridView1.DataSource = posRows;
                        GridView1.DataBind();
                        String status = headRow[0]["Status"].ToString();
                        hfStatus.Value = status;
                        visibility();
                        addButtonAttr(posRows.ToTable(), GridView1);
                        calculate();
                        GridView1.Enabled = true;
                        cmdNewPos2.Enabled = true;
                        cmdRefresh.Enabled = true;

                        if (status != "" && status != "ZE")
                        {
                            GridView1.Enabled = false;
                            cmdNewPos2.Enabled = false;
                            cmdRefresh.Enabled = false;
                        }
                        ScriptManager.RegisterStartupScript(phrJsRunner, phrJsRunner.GetType(), "jsUnblockDialog",
                                                            "unblockDialog();", true);
                    }
                    break;

                case "Refresh":
                    if (HeadGridCalculate(e.CommandArgument.ToString()))
                    {
                        RefreshHeadGrid();
                    }

                    break;

                case "Confirm":
                    DataRow[] confirmHeadRow = _objKassenabrechnung.DocHeads.Select("POSTING_NUMBER='" + e.CommandArgument + "'");
                    if (confirmHeadRow.Length > 0 && !proofVorfallGridRow(ref confirmHeadRow[0]))
                    {
                        if (confirmHeadRow.Length == 1)
                        {
                            confirmHeadRow[0]["ASTATUS"] = "ZB";
                            _objKassenabrechnung.SavePosition2(confirmHeadRow[0]["POSTING_NUMBER"].ToString());
                            RefreshHeadGrid();
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Ändern des Geschäftsvorfalles im Grid für Splitten der Vorgänge.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void ddlVorfall_SelectedIndexChanged2(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow gvRow = (GridViewRow)ddl.Parent.Parent;
            TextBox txtBetragBruttoEinnahmen = (TextBox)gvRow.FindControl("txtBetragBruttoEinnahmen");
            TextBox txtBetragBruttoAusgaben = (TextBox)gvRow.FindControl("txtBetragBruttoAusgaben");
            TextBox txtDebitor = (TextBox)gvRow.FindControl("txtDebitor");
            TextBox txtKreditor = (TextBox)gvRow.FindControl("txtKreditor");
            TextBox txtZuordnung = (TextBox)gvRow.FindControl("txtZuordnung");
            TextBox txtFreitext = (TextBox)gvRow.FindControl("txtFreitext");
            TextBox txtKST = (TextBox)gvRow.FindControl("txtKST");

            switch (_objKassenabrechnung.VorfallGewaehlt)
            {
                case Kassenabrechnung.VorfallFilter.Einahmen:
                    lblHead.Text = "Kassenabrechnung  - Einnahmen";
                    txtBetragBruttoEinnahmen.Focus();
                    break;
                case Kassenabrechnung.VorfallFilter.Ausgaben:
                    lblHead.Text = "Kassenabrechnung  - Ausgaben";
                    txtBetragBruttoAusgaben.Focus();
                    break;
            }

            if (hfStatus.Value == "" || hfStatus.Value == "ZE")
            {
                txtDebitor.Enabled = _objKassenabrechnung.CheckDebiNeeded(ddl.SelectedValue);
                txtKreditor.Enabled = _objKassenabrechnung.CheckKrediNeeded(ddl.SelectedValue);
            }
            else
            {
                txtDebitor.Enabled = false;
                txtKreditor.Enabled = false;
            }
            if (!txtDebitor.Enabled) txtDebitor.Text = string.Empty;
            if (!txtKreditor.Enabled) txtKreditor.Text = string.Empty;

            DataRow dr = _objKassenabrechnung.Geschaeftsvorfaelle.Select("TRANSACT_NUMBER = '" + ddl.SelectedValue + "'")[0];
            if (dr["ZUONR_SPERR"].ToString() == "X")
            {
                txtZuordnung.Enabled = false;
            }
            else
            {
                txtZuordnung.Enabled = true;
            }
            if (dr["TEXT_SPERR"].ToString() == "X")
            {
                txtFreitext.Enabled = false;
            }
            else
            {
                txtFreitext.Enabled = true;
                txtFreitext.Text = dr["TEXT_VBL"].ToString();
            }
            // Für Vorfall 75 (Auslagen SVA-Gebühren) Kst. mit 4061 vorbelegen
            if (dr["TRANSACT_NUMBER"].ToString().Trim() == "75")
            {
                txtKST.Text = "4061";
            }
            else
            {
                txtKST.Text = _objKassenabrechnung.VKBUR;
            }
        }

        /// <summary>
        /// Ändern des Geschäftsvorfalles im Hauptgrid.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void ddlVorfall_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow gvRow = (GridViewRow)ddl.Parent.Parent;
            Label lblStatus = (Label)gvRow.FindControl("lblStatus");
            TextBox txtBetragBruttoEinnahmen = (TextBox)gvRow.FindControl("txtBetragBruttoEinnahmen");
            TextBox txtBetragBruttoAusgaben = (TextBox)gvRow.FindControl("txtBetragBruttoAusgaben");
            TextBox txtDebitor = (TextBox)gvRow.FindControl("txtDebitor");
            TextBox txtKreditor = (TextBox)gvRow.FindControl("txtKreditor");
            TextBox txtZuordnung = (TextBox)gvRow.FindControl("txtZuordnung");
            TextBox txtFreitext = (TextBox)gvRow.FindControl("txtFreitext");
            TextBox txtKST = (TextBox)gvRow.FindControl("txtKST");

            switch (_objKassenabrechnung.VorfallGewaehlt)
            {
                case Kassenabrechnung.VorfallFilter.Einahmen:
                    lblHead.Text = "Kassenabrechnung  - Einnahmen";
                    txtBetragBruttoEinnahmen.Focus();
                    break;
                case Kassenabrechnung.VorfallFilter.Ausgaben:
                    lblHead.Text = "Kassenabrechnung  - Ausgaben";
                    txtBetragBruttoAusgaben.Focus();
                    break;
            }

            if (lblStatus.Text == "" || lblStatus.Text == "ZE")
            {
                txtDebitor.Enabled = _objKassenabrechnung.CheckDebiNeeded(ddl.SelectedValue);
                txtKreditor.Enabled = _objKassenabrechnung.CheckKrediNeeded(ddl.SelectedValue);
                txtKreditor.Visible = txtKreditor.Enabled;
                txtDebitor.Visible = txtDebitor.Enabled;
                if (!txtDebitor.Visible && !txtKreditor.Visible)
                {
                    txtDebitor.Visible = true;
                }
            }
            else
            {
                txtDebitor.Enabled = false;
                txtKreditor.Enabled = false;
            }
            if (!txtDebitor.Enabled) txtDebitor.Text = string.Empty;
            if (!txtKreditor.Enabled) txtKreditor.Text = string.Empty;

            DataRow dr = _objKassenabrechnung.Geschaeftsvorfaelle.Select("TRANSACT_NUMBER = '" + ddl.SelectedValue + "'")[0];
            if (dr["ZUONR_SPERR"].ToString() == "X")
            {
                txtZuordnung.Enabled = false;
            }
            else
            {
                txtZuordnung.Enabled = true;
            }
            if (dr["TEXT_SPERR"].ToString() == "X")
            {
                txtFreitext.Enabled = false;
            }
            else
            {
                txtFreitext.Enabled = true;
                txtFreitext.Text = dr["TEXT_VBL"].ToString();
            }
            // Für Vorfall 75 (Auslagen SVA-Gebühren) Kst. mit 4061 vorbelegen
            if (dr["TRANSACT_NUMBER"].ToString().Trim() == "75")
            {
                txtKST.Text = "4061";
            }
            else
            {
                txtKST.Text = _objKassenabrechnung.VKBUR;
            }
        }

        /// <summary>
        /// Tagesdatum setzen. Werte im Kopfbereich füllen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnToday_Click(object sender, EventArgs e)
        {
            lblErrorMain.Text = "";
            txtStartDate.Text = DateTime.Today.ToShortDateString();
            txtEndDate.Text = DateTime.Today.ToShortDateString();

            _objKassenabrechnung.DatumVon = DateTime.Today;
            _objKassenabrechnung.DatumBis = DateTime.Today;
            FillWerte();

            ShowData(false);
        }

        /// <summary>
        /// Laufende Periode setzen. Werte im Kopfbereich füllen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnCurrentPeriod_Click(object sender, EventArgs e)
        {
            lblErrorMain.Text = "";
            _objKassenabrechnung.GetPeriodeFromDateNeu(DateTime.Today);
            if (!_objKassenabrechnung.ErrorOccured)
            {
                txtStartDate.Text = _objKassenabrechnung.DatumVon.ToShortDateString();
                txtEndDate.Text = _objKassenabrechnung.DatumBis.ToShortDateString();

                FillWerte();
            }
            else
            {
                lblError.Text = _objKassenabrechnung.Message;
            }

            ShowData(false);
        }

        /// <summary>
        /// Aktuelle Woche setzen. Werte im Kopfbereich füllen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnThisWeek_Click(object sender, EventArgs e)
        {
            lblErrorMain.Text = "";
            DateTime firstDayOfWeek = DateTime.Today;
            DateTime lastDayOfWeek = DateTime.Today;

            switch (DateTime.Today.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    lastDayOfWeek = lastDayOfWeek.AddDays(6);
                    break;
                case DayOfWeek.Tuesday:
                    firstDayOfWeek = firstDayOfWeek.AddDays(-1);
                    lastDayOfWeek = lastDayOfWeek.AddDays(5);
                    break;
                case DayOfWeek.Wednesday:
                    firstDayOfWeek = firstDayOfWeek.AddDays(-2);
                    lastDayOfWeek = lastDayOfWeek.AddDays(4);
                    break;
                case DayOfWeek.Thursday:
                    firstDayOfWeek = firstDayOfWeek.AddDays(-3);
                    lastDayOfWeek = lastDayOfWeek.AddDays(3);
                    break;
                case DayOfWeek.Friday:
                    firstDayOfWeek = firstDayOfWeek.AddDays(-4);
                    lastDayOfWeek = lastDayOfWeek.AddDays(2);
                    break;
                case DayOfWeek.Saturday:
                    firstDayOfWeek = firstDayOfWeek.AddDays(-5);
                    lastDayOfWeek = lastDayOfWeek.AddDays(1);
                    break;
                case DayOfWeek.Sunday:
                    firstDayOfWeek = firstDayOfWeek.AddDays(-6);
                    break;
            }
            txtStartDate.Text = firstDayOfWeek.ToShortDateString();
            txtEndDate.Text = lastDayOfWeek.ToShortDateString();

            _objKassenabrechnung.DatumVon = firstDayOfWeek;
            _objKassenabrechnung.DatumBis = lastDayOfWeek;
            FillWerte();

            ShowData(false);
        }

        /// <summary>
        /// Neue Zeile im Hauptgrid einfügen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbNewLine_Click(object sender, EventArgs e)
        {
            DataTable table = _objKassenabrechnung.DocHeads;

            proofVorfallGrid(ref table, true);
            _objKassenabrechnung.GetNewPostingNumber();
            if (_objKassenabrechnung.ErrorOccured)
            {
                lblError.Text = _objKassenabrechnung.Message;
                return;
            }
            DataRow tblRow = table.NewRow();
            tblRow["Ampel"] = "/PortalZLD/Images/onebit_10.png";
            tblRow["BUDAT"] = DateTime.Today.ToShortDateString();
            tblRow["BUKRS"] = _objKassenabrechnung.VKORG;
            tblRow["KOSTL"] = _objKassenabrechnung.VKBUR;
            tblRow["CAJO_NUMBER"] = _objKassenabrechnung.KassenbuchNr;
            tblRow["Posting_Number"] = _objKassenabrechnung.NewPostingNumber;
            tblRow["New"] = "1";
            tblRow["ASTATUS"] = "";

            table.Rows.Add(tblRow);

            gvDaten.DataSource = table.DefaultView;
            gvDaten.DataBind();
            visibility();
            addButtonAttr(table, gvDaten);

            GridViewRow gvRow = gvDaten.Rows[gvDaten.Rows.Count - 1];
            DropDownList ddl = (DropDownList)gvRow.FindControl("ddlVorfall");
            ddl.Focus();
            TextBox txtDebitor = (TextBox)gvRow.FindControl("txtDebitor");
            TextBox txtKreditor = (TextBox)gvRow.FindControl("txtKreditor");
            switch (_objKassenabrechnung.VorfallGewaehlt)
            {
                case Kassenabrechnung.VorfallFilter.Einahmen:
                    lblHead.Text = "Kassenabrechnung  - Einnahmen";
                    break;
                case Kassenabrechnung.VorfallFilter.Ausgaben:
                    lblHead.Text = "Kassenabrechnung  - Ausgaben";
                    break;
            }

            txtDebitor.Enabled = _objKassenabrechnung.CheckDebiNeeded(ddl.SelectedValue);
            txtKreditor.Enabled = _objKassenabrechnung.CheckKrediNeeded(ddl.SelectedValue);
            if (!txtDebitor.Enabled) txtDebitor.Text = string.Empty;
            if (!txtKreditor.Enabled) txtKreditor.Text = string.Empty;
            lblErrorMain.Text = "";
            lblMessage.Text = "";
            lblError.Text = "";
        }

        /// <summary>
        /// Aktuelle Einahmen anzeigen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbShowEinnahmen_Click(object sender, EventArgs e)
        {
            lblErrorMain.Text = "";
            lblHead.Text = "Kassenabrechnung  - Einnahmen";

            DateTime dtBegin;
            DateTime dtEnd;
            DateTime.TryParse(txtStartDate.Text, out dtBegin);
            DateTime.TryParse(txtEndDate.Text, out dtEnd);
            _objKassenabrechnung.DatumVon = dtBegin;
            _objKassenabrechnung.DatumBis = dtEnd;

            _objKassenabrechnung.FillPositionen2(Kassenabrechnung.VorfallFilter.Einahmen);

            if (_objKassenabrechnung.ErrorOccured)
            {
                lblErrorMain.Text = _objKassenabrechnung.Message;
            }
            else
            {
                if (_objKassenabrechnung.DocHeads.Rows.Count == 0)
                {
                    lblErrorMain.Text = "Es sind keine Einträge vorhanden!";
                }

                try
                {
                    ShowData(true);
                }
                catch (Exception ex)
                {
                    lblErrorMain.Text = ex.Message;
                }
            }
        }

        /// <summary>
        /// Aktuelle Ausgaben anzeigen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbShowAusgaben_Click(object sender, EventArgs e)
        {
            lblErrorMain.Text = "";
            lblHead.Text = "Kassenabrechnung  - Ausgaben";

            DateTime dtBegin;
            DateTime dtEnd;
            DateTime.TryParse(txtStartDate.Text, out dtBegin);
            DateTime.TryParse(txtEndDate.Text, out dtEnd);
            _objKassenabrechnung.DatumVon = dtBegin;
            _objKassenabrechnung.DatumBis = dtEnd;

            _objKassenabrechnung.FillPositionen2(Kassenabrechnung.VorfallFilter.Ausgaben);

            if (_objKassenabrechnung.ErrorOccured)
            {
                lblErrorMain.Text = _objKassenabrechnung.Message;
            }
            else
            {
                if (_objKassenabrechnung.DocHeads.Rows.Count == 0)
                {
                    lblErrorMain.Text = "Es sind keine Einträge vorhanden!";
                }

                try
                {
                    ShowData(true);
                }
                catch (Exception ex)
                {
                    lblErrorMain.Text = ex.Message;
                }
            }
        }

        /// <summary>
        /// Startdatum geändert -> formatiert anzeigen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void txtStartDate_TextChanged(object sender, EventArgs e)
        {
            DateTime date;
            DateTime.TryParse(txtStartDate.Text, out date);
            _objKassenabrechnung.DatumVon = date;
        }

        /// <summary>
        /// Endedatum geändert -> formatiert anzeigen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void txtEndDate_TextChanged(object sender, EventArgs e)
        {
            DateTime date;
            DateTime.TryParse(txtEndDate.Text, out date);
            _objKassenabrechnung.DatumBis = date;
        }

        /// <summary>
        /// Speicherfunktion aufrufen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        /// <summary>
        /// Kopfbereich anzeigen. Vorgangsbereich ausblenden.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            lblHead.Text = "Kassenabrechnung";
            lblErrorMain.Text = "";
            ShowData(false);
        }

        /// <summary>
        /// Buchen der Vorgänge in SAP. Kopfbereich aktulisieren.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuchen_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            lblMessage.Text = "";
            lblErrorMain.Text = "";
            DataTable table = _objKassenabrechnung.DocHeads;
            Int32 iCount = 0;

            Boolean custError = proofVorfallGrid(ref table, false);
            if (!custError)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (table.Rows[i]["New"].ToString() == "0")
                    {
                        DataRow row = table.Rows[i];
                        String status = row["Status"].ToString();
                        String aStatus = row["ASTATUS"].ToString();
                        Boolean bError = !proofVorfallGridRow(ref row);
                        if (bError)
                        {
                            if (status == "" || status == "ZE")
                            {
                                if ((Boolean)row["Auswahl"])
                                {
                                    if (aStatus != "ZA")
                                    {
                                        _objKassenabrechnung.Buchen2(row);
                                        if (_objKassenabrechnung.ErrorOccured)
                                        {
                                            lblError.Text += _objKassenabrechnung.Message;
                                        }
                                    }
                                    else if (!lblError.Text.Contains("Bestätigen Sie \"Betrag erhalten\" um Vorgänge buchen zu können!"))
                                    {
                                        lblError.Text += "Bestätigen Sie \"Betrag erhalten\" um Vorgänge buchen zu können!";
                                    }
                                    iCount++;
                                }
                            }
                        }
                    }
                    else
                    {
                        lblError.Text += "Es können nur Einträge gebucht werden, die bereits gesichert wurden.";
                    }
                }
            }
            if (iCount > 0)
            {

                _objKassenabrechnung.FillPositionen2(_objKassenabrechnung.VorfallGewaehlt);
                gvDaten.DataSource = _objKassenabrechnung.DocHeads;
                gvDaten.DataBind();
                visibility();
                table = _objKassenabrechnung.DocHeads;
                addButtonAttr(table, gvDaten);
                FillWerte();
                if (lblError.Text == "")// Fehler? nochmal prüfen um Details anzeigen zu können
                {
                    lblMessage.Text = "Daten für die Kassenabrechnung gebucht!";

                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        DataRow row = table.Rows[i];
                        proofVorfallGridRow(ref row);
                    }
                }
            }
            else if (!custError) { lblError.Text = "Keine Vorgänge zum Buchen markiert!"; }

            if (_objKassenabrechnung.VorfallGewaehlt == Kassenabrechnung.VorfallFilter.Einahmen)
            {
                lblHead.Text = "Kassenabrechnung  - Einnahmen";
            }
            else if (_objKassenabrechnung.VorfallGewaehlt == Kassenabrechnung.VorfallFilter.Ausgaben)
            {
                lblHead.Text = "Kassenabrechnung  - Ausgaben";
            }
        }

        /// <summary>
        /// Zurück zur Startseite.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            Response.Redirect("/PortalZLD/Start/Selection.aspx?AppID=" + Session["AppID"]);
        }

        /// <summary>
        /// Datum Textbox im Hauptgrid an Javafunktion binden.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDaten_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem == null)
                return;
            TextBox txtDatum = (TextBox)e.Row.FindControl("txtDatum");
            txtDatum.Attributes.Add("onfocus", "datePick('#" + txtDatum.ClientID + "')");
        }

        /// <summary>
        /// Neu Position im "Splitgrid" einfügen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdNewPos_Click(object sender, EventArgs e)
        {
            DataTable table = _objKassenabrechnung.DocPos;

            proofPosGrid();
            DataView posRows = _objKassenabrechnung.DocPos.DefaultView; //
            posRows.RowFilter = "POSTING_NUMBER='" + hfPostingNumber.Value + "'";
            DataRow tblRow = table.NewRow();
            tblRow["KOSTL"] = _objKassenabrechnung.VKBUR;
            tblRow["Posting_Number"] = hfPostingNumber.Value;
            tblRow["Position_Number"] = "";
            table.Rows.Add(tblRow);
            GridView1.DataSource = posRows;
            GridView1.DataBind();

            addButtonAttr(posRows.ToTable(), GridView1);
        }

        /// <summary>
        /// Neu Positionen im "Splitgrid" in der Tabelle speichern.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdSavePos_Click(object sender, EventArgs e)
        {
            if (!proofPosGrid())
            {
                Refresh();
                DataRow[] rowPos = _objKassenabrechnung.DocHeads.Select("POSTING_NUMBER='" + hfPostingNumber.Value + "'");

                if (rowPos.Length == 1)
                {
                    DataRow[] rowPosSteuer =
                        _objKassenabrechnung.DocPos.Select("POSTING_NUMBER='" + hfPostingNumber.Value + "'");
                    if (rowPosSteuer.Length > 0)
                    {
                        switch (_objKassenabrechnung.VorfallGewaehlt)
                        {
                            case Kassenabrechnung.VorfallFilter.Einahmen:
                                rowPos[0]["H_RECEIPTS"] = lblGesamtPosShow.Text;
                                break;
                            case Kassenabrechnung.VorfallFilter.Ausgaben:
                                rowPos[0]["H_PAYMENTS"] = lblGesamtPosShow.Text;
                                break;
                        }

                        rowPos[0]["H_NET_AMOUNT"] = hfNettoGesamt.Value;
                        rowPos[0]["LIFNR"] = "*";
                        rowPos[0]["KUNNR"] = "*";
                    }
                }

                RefreshHeadGrid();
                Session["objKassenabrechnung"] = _objKassenabrechnung;
                ScriptManager.RegisterStartupScript(phrJsRunner, phrJsRunner.GetType(), "jsCloseDialg",
                                                    "closeDialogandSave();", true);
            }
        }

        /// <summary>
        /// Daten aus dem "Splitgrid" sammeln, kumlieren und anzeigen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">ImageClickEventArgs</param>
        protected void cmdRefresh_Click(object sender, ImageClickEventArgs e)
        {
            proofPosGrid();
            Refresh();
        }

        /// <summary>
        /// Das Hauptgrid aktualisieren.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnRefreshGrid_Click(object sender, EventArgs e)
        {
            RefreshHeadGrid();
        }

        /// <summary>
        /// Splitdialog schliessen und Eingaben übernehmen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdCloseDialog_Click(object sender, EventArgs e)
        {
            if (!proofPosGrid())
            {
                addButtonAttr(_objKassenabrechnung.DocHeads, gvDaten);
                Session["objKassenabrechnung"] = _objKassenabrechnung;
                ScriptManager.RegisterStartupScript(phrJsRunner, phrJsRunner.GetType(), "jsCloseDialg", "closeDialog();", true);
            }
        }

        /// <summary>
        /// Posititionen aus dem "Splitgrid" löschen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewCommandEventArgs</param>
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            lblError.Text = "";
            try
            {
                if (e.CommandName == "Del")
                {
                    DataRow[] rowsToDel =
                            _objKassenabrechnung.DocPos.Select("POSTING_NUMBER='" + hfPostingNumber.Value + "'");
                    DataRow delRow = rowsToDel[Convert.ToInt16(e.CommandArgument)];
                    _objKassenabrechnung.DocPos.Rows.Remove(delRow);
                    DataView posRows = _objKassenabrechnung.DocPos.DefaultView;//
                    posRows.RowFilter = "POSTING_NUMBER='" + hfPostingNumber.Value + "'";

                    GridView1.DataSource = posRows;
                    GridView1.DataBind();

                    addButtonAttr(posRows.ToTable(), GridView1);
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        /// <summary>
        /// Über die angebenene Zeitspanne selektieren und Daten anzeigen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbtnTimeRange_Click(object sender, EventArgs e)
        {
            lblErrorMain.Text = "";
            DateTime dtBegin;
            DateTime dtEnd;
            DateTime.TryParse(txtStartDate.Text, out dtBegin);
            DateTime.TryParse(txtEndDate.Text, out dtEnd);
            _objKassenabrechnung.DatumVon = dtBegin;
            _objKassenabrechnung.DatumBis = dtEnd;
            FillWerte();

            ShowData(false);
        }

        /// <summary>
        /// Alle Vorgänge im Hauptgrid zum Speichern markieren.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdMark_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow gvRow in gvDaten.Rows)
            {
                CheckBox chkAuswahl = (CheckBox)gvRow.FindControl("chkAuswahl");
                chkAuswahl.Checked = chkAuswahl.Visible;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Setzen des Errorstyles/Fehlertextes für Controls.
        /// </summary>
        /// <param name="txtcontrol">Textbox</param>
        /// <param name="errorControl">Label</param>
        /// <param name="errText">Fehlertext</param>
        private void SetErrBehavior(TextBox txtcontrol, Label errorControl, string errText)
        {
            txtcontrol.BorderColor = System.Drawing.Color.Red;
            txtcontrol.BorderStyle = BorderStyle.Solid;
            txtcontrol.BorderWidth = 1;

            errorControl.Text = errText;
        }

        /// <summary>
        /// Prüft, ob der Zuordnungstext dem SAP-seitig definierten Format entspricht
        /// </summary>
        /// <param name="zuordnungstext">Text aus Textbox "Zuordnung"</param>
        /// <param name="formattext">Format für Zuordnungsfeld aus SAP</param>
        /// <returns></returns>
        private bool CheckFormatZuordnungstext(string zuordnungstext, string formattext)
        {
            bool erg = false;

            try
            {
                if (String.IsNullOrEmpty(formattext))
                {
                    erg = true;
                }
                else
                {
                    if (String.IsNullOrEmpty(zuordnungstext))
                    {
                        erg = false;
                    }
                    else
                    {
                        int tempint;
                        int jahr;
                        int woche;
                        string inText = zuordnungstext.Trim(' ');

                        if (formattext.StartsWith("JJ"))
                        {
                            string[] teileFormat = formattext.Trim(' ').Split(' ');
                            string[] teileInput = inText.Split(' ');

                            if (teileFormat.Length == teileInput.Length)
                            {
                                DateTime tempdate;

                                switch (teileFormat[0])
                                {
                                    case "JJJJMMTT":
                                        erg = DateTime.TryParseExact(teileInput[0], "yyyyMMdd",
                                            System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out tempdate);
                                        break;

                                    case "JJMMTT":
                                        erg = DateTime.TryParseExact(teileInput[0], "yyMMdd",
                                            System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out tempdate);
                                        break;

                                    case "JJJJKW":
                                        if ((teileInput[0].Length == 6) && (Int32.TryParse(teileInput[0].Substring(0, 4), out jahr))
                                            && (Int32.TryParse(teileInput[0].Substring(4, 2), out woche)))
                                        {
                                            if ((jahr > 2000) && (jahr < 3000) && (woche > 0) && (woche < 54))
                                            {
                                                erg = true;
                                            }
                                        }
                                        break;

                                    case "JJJJKWKST":
                                        if ((teileInput[0].Length == 10) && (Int32.TryParse(teileInput[0].Substring(0, 4), out jahr))
                                            && (Int32.TryParse(teileInput[0].Substring(4, 2), out woche)) && Int32.TryParse(teileInput[0].Substring(6, 4), out tempint))
                                        {
                                            if ((jahr > 2000) && (jahr < 3000) && (woche > 0) && (woche < 54))
                                            {
                                                erg = true;
                                            }
                                        }
                                        break;

                                    default:
                                        erg = true;
                                        break;
                                }

                                if ((erg) && (teileFormat.Length > 1))
                                {
                                    for (int i = 1; i < teileFormat.Length; i++)
                                    {
                                        if ((teileFormat[i].ToUpper() == "KST") || (teileFormat[i].ToUpper() == "KUNNR"))
                                        {
                                            if (Int32.TryParse(teileInput[i], out tempint))
                                            {
                                                erg = true;
                                            }
                                            else
                                            {
                                                erg = false;
                                                break;
                                            }
                                        }
                                        else if (teileFormat[i].ToUpper() == "AMT")
                                        {
                                            if (Int32.TryParse(teileInput[i], out tempint))
                                            {
                                                erg = false;
                                                break;
                                            }
                                            else
                                            {
                                                erg = true;
                                            }
                                        }
                                        else
                                        {
                                            erg = true;
                                        }
                                    }
                                }
                            }
                        }
                        else if ((formattext.ToUpper() == "ANZAHL SCHILDER") || (formattext.ToUpper() == "KST") || (formattext.ToUpper() == "KUNNR"))
                        {
                            erg = Int32.TryParse(inText, out tempint);
                        }
                        else if (formattext.ToUpper() == "AMT")
                        {
                            erg = !Int32.TryParse(inText, out tempint);
                        }
                        else
                        {
                            erg = true;
                        }
                    }
                }
            }
            catch { }

            return erg;
        }

        /// <summary>
        /// Gesamtes Hauptgrid auf fehlerhafte Eingaben prüfen und korrekte Eingaben in der Vorgangstabelle speichern.
        /// </summary>
        /// <param name="tblData">Vorgangstabelle</param>
        /// <param name="NewLine">hinzufügen eines Vorgangs = kein Fehler ausgeben </param>
        /// <returns>true bei Fehler</returns>
        private bool proofVorfallGrid(ref DataTable tblData, Boolean NewLine)
        {
            bool blError = false;
            int i = 0;

            foreach (GridViewRow gvRow in gvDaten.Rows)
            {
                if (tblData.Rows[i]["Status"].ToString() == "ZE" || tblData.Rows[i]["Status"].ToString() == "")
                {
                    DropDownList ddl = (DropDownList)gvRow.FindControl("ddlVorfall");
                    TextBox txtDate = (TextBox)gvRow.FindControl("txtDatum");
                    TextBox txtKst = (TextBox)gvRow.FindControl("txtKST");
                    TextBox txtZuordnung = (TextBox)gvRow.FindControl("txtZuordnung");
                    TextBox txtFreitext = (TextBox)gvRow.FindControl("txtFreitext");
                    TextBox txtBarcode = (TextBox)gvRow.FindControl("txtBarcode");
                    TextBox txtAuftrag = (TextBox)gvRow.FindControl("txtAuftrag");
                    CheckBox chkAuswahl = (CheckBox)gvRow.FindControl("chkAuswahl");

                    DataRow dr = _objKassenabrechnung.Geschaeftsvorfaelle.Select("TRANSACT_NUMBER = '" + ddl.SelectedValue + "'")[0];

                    tblData.Rows[i]["BUKRS"] = _objKassenabrechnung.VKORG;
                    tblData.Rows[i]["TRANSACT_NUMBER"] = ddl.SelectedValue;

                    string strBudat = txtDate.Text.Trim();
                    if (strBudat != "")
                    {
                        tblData.Rows[i]["BUDAT"] = txtDate.Text.Trim();
                    }
                    else if (!NewLine && chkAuswahl.Checked)
                    {
                        SetErrBehavior(txtDate, lblError, "Es wurde kein Datum eingetragen!");
                        blError = true;
                    }

                    string strKst = txtKst.Text.Trim();
                    if (strKst != "")
                    {
                        tblData.Rows[i]["KOSTL"] = txtKst.Text.Trim();
                    }
                    else if (!NewLine && chkAuswahl.Checked)
                    {
                        SetErrBehavior(txtKst, lblError, "Es wurde keine Kostenstelle eingetragen!");
                        blError = true;
                    }

                    string strZuonr = txtZuordnung.Text;
                    if ((dr["ZUONR_SPERR"].ToString() != "X"))
                    {
                        tblData.Rows[i]["ZUONR"] = strZuonr;

                        if (!CheckFormatZuordnungstext(strZuonr, dr["ZUONR_VBL"].ToString()))
                        {
                            SetErrBehavior(txtZuordnung, lblError, "Im Feld Zuordnung bitte '" + dr["ZUONR_VBL"].ToString() + "' eingeben!");
                            blError = true;
                        }
                    }

                    string strFreitext = txtFreitext.Text;
                    if (dr["TEXT_SPERR"].ToString() != "X")
                    {
                        if (strFreitext != "")
                        {
                            tblData.Rows[i]["SGTXT"] = strFreitext;
                        }
                        else if (!NewLine && chkAuswahl.Checked)
                        {
                            SetErrBehavior(txtFreitext, lblError, "Es wurde kein Text eingetragen!");
                            blError = true;
                        }
                    }

                    if (ddl.SelectedValue.Trim(' ') == "21" || ddl.SelectedValue.Trim(' ') == "38")
                    {
                        if (txtAuftrag.Text.Trim(' ') != "") //21 or 38
                        {
                            tblData.Rows[i]["ORDERID"] = txtAuftrag.Text;
                        }
                        else if (!NewLine && chkAuswahl.Checked)
                        {
                            SetErrBehavior(txtAuftrag, lblPosError, "Es wurde keine Auftragsnummer eingetragen!");
                            blError = true;
                        }
                    }
                    else
                    {
                        tblData.Rows[i]["ORDERID"] = txtAuftrag.Text;
                    }

                    string strDocNum = txtBarcode.Text.Trim();
                    if (strDocNum != "")
                    {
                        if (strDocNum.Length == 6)
                        {
                            tblData.Rows[i]["DOCUMENT_NUMBER"] = txtBarcode.Text.Trim();
                        }
                        else if (!NewLine && chkAuswahl.Checked)
                        {
                            SetErrBehavior(txtBarcode, lblError, "Barcode nicht 6 Zeichen lang!");
                            blError = true;
                        }

                    }
                    else if (!NewLine && chkAuswahl.Checked)
                    {
                        SetErrBehavior(txtBarcode, lblError, "Es wurde kein Barcode eingetragen!");
                        blError = true;
                    }

                    decimal betrag;
                    TextBox txtDebitor = (TextBox)gvRow.FindControl("txtDebitor");
                    TextBox txtKreditor = (TextBox)gvRow.FindControl("txtKreditor");
                    tblData.Rows[i]["LIFNR"] = "";
                    tblData.Rows[i]["KUNNR"] = "";
                    if ((txtKreditor.Enabled) || (txtKreditor.Text.Trim() != ""))
                    {
                        if (txtKreditor.Text.Trim() != "")
                        {
                            tblData.Rows[i]["LIFNR"] = txtKreditor.Text.Trim();
                            txtKreditor.BorderColor = System.Drawing.Color.Empty;
                        }
                        else if (!NewLine && chkAuswahl.Checked)
                        {
                            SetErrBehavior(txtKreditor, lblError, "Es wurde kein Kreditor eingetragen!");
                            blError = true;
                        }
                    }
                    if ((txtDebitor.Enabled) || (txtDebitor.Text.Trim() != ""))
                    {
                        if (txtDebitor.Text.Trim() != "")
                        {
                            tblData.Rows[i]["KUNNR"] = txtDebitor.Text.Trim();
                            txtDebitor.BorderColor = System.Drawing.Color.Empty;
                        }
                        else if (!NewLine && chkAuswahl.Checked)
                        {
                            SetErrBehavior(txtDebitor, lblError, "Es wurde kein Kreditor eingetragen!");
                            blError = true;
                        }
                    }

                    switch (_objKassenabrechnung.VorfallGewaehlt)
                    {
                        case Kassenabrechnung.VorfallFilter.Einahmen:
                            TextBox txtBruttoEin = (TextBox)gvRow.FindControl("txtBetragBruttoEinnahmen");

                            if (txtBruttoEin.Text.Trim() != "")
                            {
                                betrag = Convert.ToDecimal(txtBruttoEin.Text);
                                tblData.Rows[i]["H_RECEIPTS"] = betrag;
                                tblData.Rows[i]["H_PAYMENTS"] = "0,00";
                            }
                            else if (!NewLine && chkAuswahl.Checked)
                            {
                                SetErrBehavior(txtBruttoEin, lblError, "Es wurde kein Betrag eingegeben!");
                                blError = true;
                            }
                            break;
                        case Kassenabrechnung.VorfallFilter.Ausgaben:
                            TextBox txtBruttoAus = (TextBox)gvRow.FindControl("txtBetragBruttoAusgaben");

                            if (txtBruttoAus.Text.Trim() != "")
                            {
                                betrag = Convert.ToDecimal(txtBruttoAus.Text);
                                tblData.Rows[i]["H_RECEIPTS"] = "0,00";
                                tblData.Rows[i]["H_PAYMENTS"] = betrag;

                            }
                            else if (!NewLine && chkAuswahl.Checked)
                            {
                                SetErrBehavior(txtBruttoAus, lblError, "Es wurde kein Betrag eingegeben!");
                                blError = true;
                            }
                            break;
                    }
                }

                i++;
            }
            return blError;
        }

        /// <summary>
        /// einzelne Zeile im Hauptgrid auf fehlerhafte Eingaben prüfen und korrekte Eingaben in der Vorgangstabelle speichern.
        /// </summary>
        /// <param name="gvRow">Zeile</param>
        /// <returns>true bei Fehler</returns>
        private bool proofVorfallGridRow(ref DataRow gvRow)
        {
            int index = _objKassenabrechnung.DocHeads.Rows.IndexOf(gvRow);
            GridViewRow gvDatenRow = gvDaten.Rows[index];
            bool blError = false;

            if (gvRow["Status"].ToString() == "ZE" || gvRow["Status"].ToString() == "")
            {
                DropDownList ddl = (DropDownList)gvDatenRow.FindControl("ddlVorfall");
                TextBox txtDate = (TextBox)gvDatenRow.FindControl("txtDatum");
                TextBox txtKst = (TextBox)gvDatenRow.FindControl("txtKST");
                TextBox txtZuordnung = (TextBox)gvDatenRow.FindControl("txtZuordnung");
                TextBox txtFreitext = (TextBox)gvDatenRow.FindControl("txtFreitext");
                TextBox txtBarcode = (TextBox)gvDatenRow.FindControl("txtBarcode");
                TextBox txtAuftrag = (TextBox)gvDatenRow.FindControl("txtAuftrag");
                CheckBox chkAuswahl = (CheckBox)gvDatenRow.FindControl("chkAuswahl");

                DataRow dr = _objKassenabrechnung.Geschaeftsvorfaelle.Select("TRANSACT_NUMBER = '" + ddl.SelectedValue + "'")[0];

                gvRow["BUKRS"] = _objKassenabrechnung.VKORG;
                gvRow["TRANSACT_NUMBER"] = ddl.SelectedValue;
                gvRow["Auswahl"] = chkAuswahl.Checked;
                string strBudat = txtDate.Text.Trim();
                DateTime tmpDat;
                if (strBudat != "" && DateTime.TryParse(strBudat, out tmpDat))
                {
                    gvRow["BUDAT"] = tmpDat;
                }
                else
                {
                    SetErrBehavior(txtDate, lblError, "Es wurde kein gültiges Datum eingetragen!");
                    blError = true;
                }

                string strKst = txtKst.Text.Trim();
                if (strKst != "")
                {
                    gvRow["KOSTL"] = txtKst.Text.Trim();
                }
                else
                {
                    SetErrBehavior(txtKst, lblError, "Es wurde keine Kostenstelle eingetragen!");
                    blError = true;
                }

                string strZuonr = txtZuordnung.Text;
                if ((dr["ZUONR_SPERR"].ToString() != "X"))
                {
                    gvRow["ZUONR"] = strZuonr;

                    if (!CheckFormatZuordnungstext(strZuonr, dr["ZUONR_VBL"].ToString()))
                    {
                        SetErrBehavior(txtZuordnung, lblError, "Im Feld Zuordnung bitte '" + dr["ZUONR_VBL"].ToString() + "' eingeben!");
                        blError = true;
                    }
                }

                string strFreitext = txtFreitext.Text;
                if (dr["TEXT_SPERR"].ToString() != "X")
                {
                    if (strFreitext != "")
                    {
                        gvRow["SGTXT"] = strFreitext;
                    }
                    else
                    {
                        SetErrBehavior(txtFreitext, lblError, "Es wurde kein Text eingetragen!");
                        blError = true;
                    }
                }

                if (ddl.SelectedValue.Trim(' ') == "21" || ddl.SelectedValue.Trim(' ') == "38")
                {
                    if (txtAuftrag.Text.Trim(' ') != "") //21 or 38
                    {
                        gvRow["ORDERID"] = txtAuftrag.Text;
                    }
                    else
                    {
                        SetErrBehavior(txtAuftrag, lblError, "Es wurde keine Auftragsnummer eingetragen!");
                        blError = true;
                    }
                }
                else
                {
                    gvRow["ORDERID"] = txtAuftrag.Text;
                }

                string strDocNum = txtBarcode.Text.Trim();
                if (strDocNum != "")
                {
                    if (strDocNum.Length == 6)
                    {
                        gvRow["DOCUMENT_NUMBER"] = txtBarcode.Text.Trim();
                    }
                    else
                    {
                        SetErrBehavior(txtBarcode, lblError, "Barcode nicht 6 Zeichen lang!");
                        blError = true;
                    }

                }
                else
                {
                    SetErrBehavior(txtBarcode, lblError, "Es wurde kein Barcode eingetragen!");
                    blError = true;
                }

                decimal betrag;
                TextBox txtDebitor = (TextBox)gvDatenRow.FindControl("txtDebitor");
                TextBox txtKreditor = (TextBox)gvDatenRow.FindControl("txtKreditor");
                gvRow["KUNNR"] = "";
                gvRow["LIFNR"] = "";
                if ((txtKreditor.Enabled) || (txtKreditor.Text.Trim() != ""))
                {
                    if (txtKreditor.Text.Trim() != "")
                    {
                        gvRow["LIFNR"] = txtKreditor.Text.Trim();
                        txtKreditor.BorderColor = System.Drawing.Color.Empty;
                    }
                    else
                    {
                        SetErrBehavior(txtKreditor, lblError, "Es wurde kein Kreditor eingetragen!");
                        blError = true;
                    }
                }
                if ((txtDebitor.Enabled) || (txtDebitor.Text.Trim() != ""))
                {
                    if (txtDebitor.Text.Trim() != "")
                    {
                        gvRow["KUNNR"] = txtDebitor.Text.Trim();
                        txtDebitor.BorderColor = System.Drawing.Color.Empty;
                    }
                    else
                    {
                        SetErrBehavior(txtDebitor, lblError, "Es wurde kein Debitor eingetragen!");
                        blError = true;
                    }

                }
                switch (_objKassenabrechnung.VorfallGewaehlt)
                {
                    case Kassenabrechnung.VorfallFilter.Einahmen:
                        TextBox txtBruttoEin = (TextBox)gvDatenRow.FindControl("txtBetragBruttoEinnahmen");

                        if (txtBruttoEin.Text.Trim() != "")
                        {
                            betrag = Convert.ToDecimal(txtBruttoEin.Text);
                            gvRow["H_RECEIPTS"] = betrag;
                            gvRow["H_PAYMENTS"] = "0,00";
                        }
                        else
                        {
                            SetErrBehavior(txtBruttoEin, lblError, "Es wurde kein Betrag eingegeben!");
                            blError = true;
                        }
                        break;
                    case Kassenabrechnung.VorfallFilter.Ausgaben:
                        TextBox txtBruttoAus = (TextBox)gvDatenRow.FindControl("txtBetragBruttoAusgaben");

                        if (txtBruttoAus.Text.Trim() != "")
                        {
                            betrag = Convert.ToDecimal(txtBruttoAus.Text);
                            gvRow["H_RECEIPTS"] = "0,00";
                            gvRow["H_PAYMENTS"] = betrag;
                        }
                        else
                        {
                            SetErrBehavior(txtBruttoAus, lblError, "Es wurde kein Betrag eingegeben!");
                            blError = true;
                        }
                        break;
                }

                if (_objKassenabrechnung.tblError != null)
                {
                    if (_objKassenabrechnung.tblError.Rows.Count > 0)
                    {
                        if (_objKassenabrechnung.tblError.Select("Postingnr='" + gvRow["POSTING_NUMBER"] + "' AND ErrorNr='134'").Length > 0)
                        {
                            SetErrBehavior(txtFreitext, lblError, "Das Feld Text ist ein Pflichtfeld, bitte einen Wert eingeben!");
                            blError = true;
                        }
                        if (_objKassenabrechnung.tblError.Select("Postingnr='" + gvRow["POSTING_NUMBER"] + "' AND ErrorNr='135'").Length > 0)
                        {
                            SetErrBehavior(txtZuordnung, lblError, " Das Feld Zuordnung ist ein Pflichtfeld, bitte einen Wert eingeben!");
                            blError = true;
                        }
                        if (_objKassenabrechnung.tblError.Select("Postingnr='" + gvRow["POSTING_NUMBER"] + "' AND ErrorNr='136'").Length > 0)
                        {
                            SetErrBehavior(txtBarcode, lblError, "Das Feld Barcode ist ein Pflichtfeld, bitte einen Wert eingeben!");
                            blError = true;
                        }
                    }
                }
            }
            return blError;
        }

        /// <summary>
        /// Gesamtes "Splitgrid" auf fehlerhafte Eingaben prüfen und korrekte Eingaben in der Vorgangstabelle speichern.
        /// </summary>
        /// <returns>true bei Fehler</returns>
        private bool proofPosGrid()
        {
            lblPosError.Text = "";
            bool blError = false;

            List<DataRow> listePos =
                new List<DataRow>(_objKassenabrechnung.DocPos.Select("POSTING_NUMBER= '" + hfPostingNumber.Value + "'"));
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                GridViewRow gvRow = GridView1.Rows[i];
                DropDownList ddl = (DropDownList)gvRow.FindControl("ddlVorfall");
                TextBox txtKst = (TextBox)gvRow.FindControl("txtKST");
                TextBox txtFreitext = (TextBox)gvRow.FindControl("txtFreitext");
                TextBox txtAuftrag = (TextBox)gvRow.FindControl("txtAuftrag");
                TextBox txtZuordnung = (TextBox)gvRow.FindControl("txtZuordnung");
                Label lblPositionNr = (Label)gvRow.FindControl("lblPositionNr");

                DataRow dr = _objKassenabrechnung.Geschaeftsvorfaelle.Select("TRANSACT_NUMBER = '" + ddl.SelectedValue + "'")[0];

                DataRow t = listePos[i];
                if (lblPositionNr.Text == t["POSITION_NUMBER"].ToString())
                {
                    t["BUKRS"] = _objKassenabrechnung.VKORG;
                    t["TRANSACT_NUMBER"] = ddl.SelectedValue;
                    if (ddl.SelectedValue == "*")
                    {
                        SetErrBehavior(txtKst, lblPosError, "Es wurde kein Geschäftsvorfall gewählt!");
                        ddl.BorderColor = System.Drawing.Color.Red;
                        blError = true;
                    }
                    string strKst = txtKst.Text.Trim();
                    if (strKst != "")
                    {
                        t["KOSTL"] = txtKst.Text.Trim();
                    }
                    else
                    {
                        SetErrBehavior(txtKst, lblPosError, "Es wurde keine Kostenstelle eingetragen!");
                        blError = true;
                    }

                    string strFreitext = txtFreitext.Text;
                    if (dr["TEXT_SPERR"].ToString() != "X")
                    {
                        if (strFreitext != "")
                        {
                            t["SGTXT"] = strFreitext;
                        }
                        else
                        {
                            SetErrBehavior(txtFreitext, lblPosError, "Es wurde kein Text eingetragen!");
                            blError = true;
                        }
                    }

                    string strZuonr = txtZuordnung.Text;
                    if ((dr["ZUONR_SPERR"].ToString() != "X"))
                    {
                        t["ALLOC_NMBR"] = strZuonr;

                        if (!CheckFormatZuordnungstext(strZuonr, dr["ZUONR_VBL"].ToString()))
                        {
                            SetErrBehavior(txtZuordnung, lblPosError, "Im Feld Zuordnung bitte '" + dr["ZUONR_VBL"].ToString() + "' eingeben!");
                            blError = true;
                        }
                    }

                    if (ddl.SelectedValue.Trim(' ') == "21" || ddl.SelectedValue.Trim(' ') == "38")
                    {
                        if (txtAuftrag.Text.Trim(' ') != "") //21 or 38
                        {
                            t["ORDERID"] = txtAuftrag.Text;
                        }
                        else
                        {
                            SetErrBehavior(txtAuftrag, lblPosError, "Es wurde keine Auftragsnummer eingetragen!");
                            blError = true;
                        }
                    }
                    else
                    {
                        t["ORDERID"] = "";
                    }

                    decimal betrag;

                    switch (_objKassenabrechnung.VorfallGewaehlt)
                    {
                        case Kassenabrechnung.VorfallFilter.Einahmen:
                            TextBox txtBruttoEin = (TextBox)gvRow.FindControl("txtBetragBruttoEinnahmen");
                            TextBox txtDebitor = (TextBox)gvRow.FindControl("txtDebitor");

                            if (txtBruttoEin.Text.Trim() != "")
                            {
                                betrag = Convert.ToDecimal(txtBruttoEin.Text);
                                t["P_RECEIPTS"] = betrag;
                                t["P_PAYMENTS"] = "0,00";
                                t["LIFNR"] = "";

                                Boolean debiNeeded = _objKassenabrechnung.CheckDebiNeeded(ddl.SelectedValue);

                                if ((txtDebitor.Enabled) || (txtDebitor.Text.Trim() != ""))
                                {
                                    if (txtDebitor.Text.Trim() != "" || !debiNeeded)
                                    {
                                        t["KUNNR"] = txtDebitor.Text.Trim();
                                        txtDebitor.BorderColor = System.Drawing.Color.Empty;
                                    }
                                    else
                                    {
                                        SetErrBehavior(txtDebitor, lblPosError, "Es wurde kein Debitor eingetragen!");
                                        blError = true;
                                    }
                                }
                            }
                            else
                            {
                                SetErrBehavior(txtBruttoEin, lblPosError, "Es wurde kein Betrag eingegeben!");
                                blError = true;
                            }
                            break;
                        case Kassenabrechnung.VorfallFilter.Ausgaben:
                            TextBox txtBruttoAus = (TextBox)gvRow.FindControl("txtBetragBruttoAusgaben");
                            TextBox txtKreditor = (TextBox)gvRow.FindControl("txtKreditor");

                            if (txtBruttoAus.Text.Trim() != "")
                            {
                                betrag = Convert.ToDecimal(txtBruttoAus.Text);
                                t["P_RECEIPTS"] = "0,00";
                                t["P_PAYMENTS"] = betrag;
                                t["KUNNR"] = "";

                                Boolean krediNeeded = _objKassenabrechnung.CheckKrediNeeded(ddl.SelectedValue);

                                if ((txtKreditor.Enabled) || (txtKreditor.Text.Trim() != ""))
                                {
                                    if (txtKreditor.Text.Trim() != "" || !krediNeeded)
                                    {
                                        t["LIFNR"] = txtKreditor.Text.Trim();
                                        txtKreditor.BorderColor = System.Drawing.Color.Empty;
                                    }
                                    else
                                    {
                                        SetErrBehavior(txtKreditor, lblPosError, "Es wurde kein Kreditor eingetragen!");
                                        blError = true;
                                    }
                                }
                            }
                            else
                            {
                                SetErrBehavior(txtBruttoAus, lblPosError, "Es wurde kein Betrag eingegeben!");
                                blError = true;
                            }
                            break;
                    }
                }
            }

            return blError;
        }

        /// <summary>
        /// Steuerung der Felder für Einnahmen und Ausgaben.
        /// </summary>
        /// <param name="tblData">Vorgangstabelle</param>
        /// <param name="grdView">Hauptgrid/Splitgrid</param>
        private void addButtonAttr(DataTable tblData, GridView grdView)
        {
            int i = 0;
            foreach (GridViewRow gvRow in grdView.Rows)
            {
                // Vorfall Dropdown befüllen
                DropDownList ddl = (DropDownList)gvRow.FindControl("ddlVorfall");

                DataView tmpDataView = _objKassenabrechnung.Geschaeftsvorfaelle.DefaultView;
                tmpDataView.Sort = "TRANSACT_NAME";
                ddl.DataSource = tmpDataView;
                ddl.DataValueField = "TRANSACT_NUMBER";
                ddl.DataTextField = "TRANSACT_NAME";
                ddl.DataBind();

                var valueToSelect = tblData.Rows[i]["TRANSACT_NUMBER"].ToString();
                if (ddl.Items.FindByValue(valueToSelect) != null)
                    ddl.SelectedValue = valueToSelect;

                // Steuerung der Felder für Einnahmen und Ausgaben
                Boolean debiNeeded = _objKassenabrechnung.CheckDebiNeeded(valueToSelect);
                Boolean krediNeeded = _objKassenabrechnung.CheckKrediNeeded(valueToSelect);
                TextBox ctrl = (TextBox)gvRow.FindControl("txtBetragBruttoEinnahmen");
                TextBox ctrl2 = (TextBox)gvRow.FindControl("txtBetragBruttoAusgaben");
                TextBox txtDebitor = (TextBox)gvRow.FindControl("txtDebitor");
                TextBox txtKreditor = (TextBox)gvRow.FindControl("txtKreditor");
                Label lblPostNr = (Label)gvRow.FindControl("lblPostNr");
                if (lblPostNr == null)
                {
                    lblPostNr = (Label)gvRow.FindControl("lblPostingNr");
                }

                DataRow[] rowStatus = _objKassenabrechnung.DocHeads.Select("POSTING_NUMBER = '" + lblPostNr.Text + "'");
                String status = rowStatus[0]["Status"].ToString();

                // Kreditoren-/Debitoren-Felder nur freischalten, wenn Datensatz noch nicht gebucht
                if (status != "S")
                {
                    if (krediNeeded)
                    {
                        txtDebitor.Enabled = false;
                        txtDebitor.Visible = false;
                        txtKreditor.Enabled = true;
                        txtKreditor.Visible = true;
                    }
                    if (debiNeeded)
                    {
                        txtDebitor.Enabled = true;
                        txtDebitor.Visible = true;
                        txtKreditor.Enabled = false;
                        txtKreditor.Visible = false;
                    }
                    if (!debiNeeded && !krediNeeded)
                    {
                        txtDebitor.Enabled = false;
                        txtKreditor.Enabled = false;
                        txtKreditor.Visible = false;
                        txtDebitor.Visible = false;
                        switch (_objKassenabrechnung.VorfallGewaehlt)
                        {
                            case Kassenabrechnung.VorfallFilter.Einahmen:
                                txtDebitor.Visible = true;
                                break;
                            case Kassenabrechnung.VorfallFilter.Ausgaben:
                                txtKreditor.Visible = true;
                                break;
                        }
                    }

                    if (!IsEnabledDebitor(lblPostNr.Text))
                    {
                        txtDebitor.Enabled = false;
                    }
                }

                switch (_objKassenabrechnung.VorfallGewaehlt)
                {
                    case Kassenabrechnung.VorfallFilter.Einahmen:
                        ctrl.Visible = true;
                        ctrl2.Visible = false;
                        break;
                    case Kassenabrechnung.VorfallFilter.Ausgaben:
                        ctrl.Visible = false;
                        ctrl2.Visible = true;
                        break;
                }

                i++;
            }
        }

        /// <summary>
        /// Werte im Kopfbereich füllen.
        /// </summary>
        private void FillWerte()
        {
            _objKassenabrechnung.FillWerte2(_objKassenabrechnung.DatumVon, _objKassenabrechnung.DatumBis);

            if (!_objKassenabrechnung.ErrorOccured)
            {
                lblAnfangsbestand.Text = String.Format("{0:N}", _objKassenabrechnung.Anfangssaldo);
                lblSummeEinnahmen.Text = String.Format("{0:N}", _objKassenabrechnung.SummeEinnahmen);
                lblSummeAusgaben.Text = String.Format("{0:N}", _objKassenabrechnung.SummeAusgaben);
                lblEndbestand.Text = String.Format("{0:N}", _objKassenabrechnung.Endsaldo);
            }
            else
            {
                lblErrorMain.Text = _objKassenabrechnung.Message;
            }
        }

        /// <summary>
        /// Kopfdaten und Vorgangsdaten ein- oder ausblenden.
        /// </summary>
        /// <param name="show">true/false</param>
        private void ShowData(bool show)
        {
            if (show)
            {
                pUebersicht.Visible = false;
                pEingabe.Visible = true;
                KassenGridHeader.Visible = pEingabe.Visible;
                pNewLine.Visible = true;
                lbtnBuchen.Visible = true;
                lbtnSave.Visible = true;
                lbtnBack.Visible = true;
                DataTable table = _objKassenabrechnung.DocHeads;
                gvDaten.DataSource = table.DefaultView;
                gvDaten.DataBind();
                visibility();
                addButtonAttr(table, gvDaten);
                txtStartDate.Visible = false;
                txtEndDate.Visible = false;
                lblStartDate.Text = txtStartDate.Text;
                lblStartDate.Visible = true;
                lblEndDate.Text = txtEndDate.Text;
                lblEndDate.Visible = true;
            }
            else
            {
                pUebersicht.Visible = true;
                pEingabe.Visible = false;
                KassenGridHeader.Visible = pEingabe.Visible;
                pNewLine.Visible = false;
                lbtnBuchen.Visible = false;
                lbtnSave.Visible = false;
                lbtnBack.Visible = false;

                txtStartDate.Visible = true;
                txtEndDate.Visible = true;
                lblStartDate.Visible = false;
                lblEndDate.Visible = false;

            }
            lblMessage.Text = "";
        }

        /// <summary>
        /// Speichern der Vorgänge in SAP. "Ampel" aktualisieren.
        /// </summary>
        private void Save()
        {
            DataTable table = _objKassenabrechnung.DocHeads;
            lblMessage.Text = "";
            lblError.Text = "";
            lblErrorMain.Text = "";

            _objKassenabrechnung.CreateErrorTable();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow row = table.Rows[i];
                String status = row["Status"].ToString();
                Boolean bError = !proofVorfallGridRow(ref row);
                if (bError)
                {
                    if (status == "" || status == "ZE")
                    {
                        AddNewPosForSave(row["POSTING_NUMBER"].ToString(), row);
                        _objKassenabrechnung.SavePosition2(row["POSTING_NUMBER"].ToString());
                        if (_objKassenabrechnung.ErrorOccured)
                        {
                            lblError.Text += _objKassenabrechnung.Message;
                        }
                        else
                        {
                            HeadGridCalculate(row["POSTING_NUMBER"].ToString());
                            row["Status"] = "ZE";
                            row["New"] = "0";
                            row["Auswahl"] = false;
                            if (row["AStatus"].ToString() == "ZA")
                            {
                                row["Ampel"] = "/PortalZLD/Images/InfoAuto.gif";
                            }
                            else
                            {
                                row["Ampel"] = "/PortalZLD/Images/onebit_07.png";
                            }
                        }
                    }
                }
            }

            table = _objKassenabrechnung.DocHeads;

            gvDaten.DataSource = table.DefaultView;
            gvDaten.DataBind();
            visibility();
            addButtonAttr(table, gvDaten);
            lblMessage.Text = "Daten für die Kassenabrechnung gesichert!";
            if (lblError.Text != "")// Fehler? nochmal prüfen um Details anzeigen zu können
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    table = _objKassenabrechnung.DocHeads;
                    DataRow row = table.Rows[i];
                    proofVorfallGridRow(ref row);
                }
                lblMessage.Text = "";
            }

            FillWerte();

            if (_objKassenabrechnung.VorfallGewaehlt == Kassenabrechnung.VorfallFilter.Einahmen)
            {
                lblHead.Text = "Kassenabrechnung  - Einnahmen";
            }
            else if (_objKassenabrechnung.VorfallGewaehlt == Kassenabrechnung.VorfallFilter.Ausgaben)
            {
                lblHead.Text = "Kassenabrechnung  - Ausgaben";
            }
        }

        /// <summary>
        /// Steuerung welche Felder im Hauptgrid(gvDaten) angezeigt werden.
        /// </summary>
        /// <param name="postNr">POSTING_NUMBER</param>
        /// <returns>true/false</returns>
        protected bool ShowDel3(String postNr)
        {
            if (_objKassenabrechnung.VorfallGewaehlt == Kassenabrechnung.VorfallFilter.Einahmen)
            {
                lblHead.Text = "Kassenabrechnung  - Einnahmen";
            }
            else if (_objKassenabrechnung.VorfallGewaehlt == Kassenabrechnung.VorfallFilter.Ausgaben)
            {
                lblHead.Text = "Kassenabrechnung  - Ausgaben";
            }

            DataRow[] rowStatus = _objKassenabrechnung.DocHeads.Select("POSTING_NUMBER = '" + postNr + "'");
            String status = rowStatus[0]["Status"].ToString();
            switch (status)
            {
                case "":
                    return true;
                case "ZE":
                    return true;
                case "ZG":
                    return false;
                case "ZL":
                    return false;
                case "S":
                    return false;
                case "P":
                    return false;
                default:

                    return false;
            }
        }

        /// <summary>
        /// Steuerung ob die Checkbox zur Auswahl von Vorgängen angezeigt wird.
        /// </summary>
        /// <param name="postNr">POSTING_NUMBER</param>
        /// <returns>true/false</returns>
        protected bool ShowAuswahl(String postNr)
        {
            DataRow[] rowStatus = _objKassenabrechnung.DocHeads.Select("POSTING_NUMBER = '" + postNr + "'");
            String status = rowStatus[0]["Status"].ToString();
            if (status == "ZE") { return true; }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        protected bool ShowDel2(int rowIndex)
        {
            if (_objKassenabrechnung.VorfallGewaehlt == Kassenabrechnung.VorfallFilter.Einahmen)
            {
                lblHead.Text = "Kassenabrechnung  - Einnahmen";
            }
            else if (_objKassenabrechnung.VorfallGewaehlt == Kassenabrechnung.VorfallFilter.Ausgaben)
            {
                lblHead.Text = "Kassenabrechnung  - Ausgaben";
            }

            String posNr =
                _objKassenabrechnung.DocPos.DefaultView.ToTable().Rows[rowIndex]["POSITION_NUMBER"].ToString();
            return posNr == "" && rowIndex != 0;
        }

        /// <summary>
        /// Splitten einer Rechnung. Neuer Kopf ohne Position dann muss eine neue Posistion angelegt werden.
        /// </summary>
        /// <param name="postNr">POSTING_NUMBER</param>
        /// <param name="headRow">Kopfzeile</param>
        private void AddNewPosForSave(String postNr, DataRow headRow)
        {
            DataRow[] posRows = _objKassenabrechnung.DocPos.Select("POSTING_NUMBER='" + postNr + "'"); //

            if (posRows.Length == 0)
            {
                DataRow tblRow = _objKassenabrechnung.DocPos.NewRow();
                WriteHeadRowToPosRow(headRow, tblRow);

                _objKassenabrechnung.DocPos.Rows.Add(tblRow);

            }
            else if (posRows.Length == 1)
            {
                DataRow tblRow = posRows[0];
                WriteHeadRowToPosRow(headRow, tblRow);
            }
        }

        /// <summary>
        /// Bei neu angelgten Vorgängen wird zuerst der Kopf angegelegt dann 
        /// müssen die Kopfdaten in eine neue Position eingefügt werden.
        /// </summary>
        /// <param name="postNr">POSTING_NUMBER</param>
        /// <param name="headRow">Kopfzeile</param>
        private void WriteHeadToPos(String postNr, DataRow headRow)
        {
            DataRow[] posRows = _objKassenabrechnung.DocPos.Select("POSTING_NUMBER='" + postNr + "'"); //

            if (posRows.Length == 1)
            {
                DataRow tblRow = posRows[0];
                WriteHeadRowToPosRow(headRow, tblRow);
            }
        }

        private static void WriteHeadRowToPosRow(DataRow headRow, DataRow tblRow)
        {
            tblRow["BUKRS"] = headRow["BUKRS"];
            tblRow["KOSTL"] = headRow["KOSTL"];
            tblRow["CAJO_NUMBER"] = headRow["CAJO_NUMBER"];
            tblRow["Posting_Number"] = headRow["POSTING_NUMBER"];
            tblRow["Position_Number"] = "";
            tblRow["TRANSACT_NUMBER"] = headRow["TRANSACT_NUMBER"];
            tblRow["P_RECEIPTS"] = headRow["H_RECEIPTS"];
            tblRow["P_PAYMENTS"] = headRow["H_PAYMENTS"];
            tblRow["P_NET_AMOUNT"] = headRow["H_NET_AMOUNT"];
            tblRow["KOSTL"] = headRow["KOSTL"];
            tblRow["ORDERID"] = headRow["ORDERID"];
            tblRow["ALLOC_NMBR"] = headRow["ZUONR"];
            tblRow["SGTXT"] = headRow["SGTXT"];
            tblRow["LIFNR"] = headRow["LIFNR"];
            tblRow["KUNNR"] = headRow["KUNNR"];
        }

        /// <summary>
        /// MwSt im SAP berechnen lassen, neu kumulieren und anzeigen.
        /// </summary>
        private void Refresh()
        {
            _objKassenabrechnung.GetMwSt2(hfPostingNumber.Value);
            if (!_objKassenabrechnung.ErrorOccured)
            {
                DataView posRows = _objKassenabrechnung.DocPos.DefaultView; //
                posRows.RowFilter = "POSTING_NUMBER='" + hfPostingNumber.Value + "'";
                GridView1.DataSource = posRows;
                GridView1.DataBind();
                addButtonAttr(posRows.ToTable(), GridView1);
                calculate();
            }
            else
            {
                lblError.Text = _objKassenabrechnung.Message;
            }
        }

        /// <summary>
        /// Das Hauptgrid aktualisieren.
        /// </summary>
        private void RefreshHeadGrid()
        {
            gvDaten.DataSource = _objKassenabrechnung.DocHeads;
            gvDaten.DataBind();
            visibility();
            addButtonAttr(_objKassenabrechnung.DocHeads, gvDaten);
        }

        /// <summary>
        /// Kumulieren der Daten aus dem "Splitgrid".
        /// </summary>
        private void calculate()
        {
            decimal betrag = 0;
            decimal betragNetto = 0;
            foreach (GridViewRow gvRow in GridView1.Rows)
            {
                TextBox txtBetragNetto = (TextBox)gvRow.FindControl("txtBetragNetto");
                switch (_objKassenabrechnung.VorfallGewaehlt)
                {
                    case Kassenabrechnung.VorfallFilter.Einahmen:
                        TextBox txtBruttoEin = (TextBox)gvRow.FindControl("txtBetragBruttoEinnahmen");

                        if (txtBruttoEin.Text.Trim() != "")
                        {
                            betrag += Convert.ToDecimal(txtBruttoEin.Text);
                        }
                        if (txtBetragNetto.Text.Trim() != "")
                        {
                            betragNetto += Convert.ToDecimal(txtBetragNetto.Text);
                        }

                        break;
                    case Kassenabrechnung.VorfallFilter.Ausgaben:
                        TextBox txtBruttoAus = (TextBox)gvRow.FindControl("txtBetragBruttoAusgaben");

                        if (txtBruttoAus.Text.Trim() != "")
                        {
                            betrag += Convert.ToDecimal(txtBruttoAus.Text);

                        }
                        if (txtBetragNetto.Text.Trim() != "")
                        {
                            betragNetto += Convert.ToDecimal(txtBetragNetto.Text);
                        }
                        break;
                }
            }

            lblGesamtPosShow.Text = String.Format("{0:N}", betrag);
            hfNettoGesamt.Value = String.Format("{0:N}", betragNetto);
            decimal betragGes = 0;
            if (hfGesamt.Value != "")
            {
                betragGes += Convert.ToDecimal(hfGesamt.Value);
            }
            if (betragGes != betrag)
            {
                if (betragGes < betrag)
                {
                    lblDiffShow.Text = String.Format("{0:N}", betrag - betragGes);
                }
                else if (betragGes > betrag)
                {
                    lblDiffShow.Text = String.Format("{0:N}", betragGes - betrag);
                }
            }
            else
            {
                lblDiffShow.Text = "0,00";
            }
        }

        /// <summary>
        /// Kumulieren der Daten im Hauptgrid.
        /// </summary>
        /// <param name="postingNr">POSTING_NUMBER</param>
        /// <returns>true bei Eingabefehler</returns>
        public Boolean HeadGridCalculate(String postingNr)
        {
            DataRow[] headRow =
           _objKassenabrechnung.DocHeads.Select("POSTING_NUMBER='" + postingNr + "'");
            Boolean bError = !proofVorfallGridRow(ref headRow[0]);
            if (bError)
            {
                DataView posRows = _objKassenabrechnung.DocPos.DefaultView; //
                posRows.RowFilter = "POSTING_NUMBER='" + postingNr + "'";


                // neuer Kopf ohne Position ? neue Posistion anlegen
                if (posRows.ToTable().Rows.Count == 0)
                {
                    AddNewPosForSave(postingNr, headRow[0]);
                    posRows = _objKassenabrechnung.DocPos.DefaultView;
                }

                switch (_objKassenabrechnung.VorfallGewaehlt)
                {
                    case Kassenabrechnung.VorfallFilter.Einahmen:

                        if (posRows.ToTable().Rows.Count > 0)
                        {
                            posRows.ToTable().Rows[0]["KUNNR"] = headRow[0]["KUNNR"];
                        }
                        break;
                    case Kassenabrechnung.VorfallFilter.Ausgaben:
                        if (posRows.ToTable().Rows.Count > 0)
                        {
                            posRows.ToTable().Rows[0]["LIFNR"] = headRow[0]["LIFNR"];
                        }
                        break;
                }

                if (posRows.ToTable().Rows.Count > 0)
                {
                    posRows.ToTable().Rows[0]["SGTXT"] = headRow[0]["SGTXT"];
                }
                if (posRows.ToTable().Rows.Count == 1)
                {
                    String sTaxCode =
                        _objKassenabrechnung.GetTaxCode(posRows.ToTable().Rows[0]["TRANSACT_NUMBER"].ToString());
                    if (sTaxCode != "")// Kein TAXCODE, keine MwSt-Berechnung!!
                    {
                        WriteHeadToPos(postingNr, headRow[0]);
                        _objKassenabrechnung.GetMwSt2(postingNr);
                        if (!_objKassenabrechnung.ErrorOccured)
                        {
                            decimal betrag = 0;
                            decimal betragNetto = 0;
                            DataRow[] docPosRows = _objKassenabrechnung.DocPos.Select("POSTING_NUMBER='" + postingNr + "'");
                            foreach (DataRow rowToCalc in docPosRows)
                            {
                                switch (_objKassenabrechnung.VorfallGewaehlt)
                                {
                                    case Kassenabrechnung.VorfallFilter.Einahmen:

                                        if (rowToCalc["P_RECEIPTS"].ToString() != "")
                                        {
                                            betrag += Convert.ToDecimal(rowToCalc["P_RECEIPTS"].ToString());
                                        }
                                        if (rowToCalc["P_NET_AMOUNT"].ToString() != "")
                                        {
                                            betragNetto += Convert.ToDecimal(rowToCalc["P_NET_AMOUNT"].ToString());

                                        }
                                        break;
                                    case Kassenabrechnung.VorfallFilter.Ausgaben:
                                        if (rowToCalc["P_PAYMENTS"].ToString() != "")
                                        {
                                            betrag += Convert.ToDecimal(rowToCalc["P_PAYMENTS"].ToString());
                                        }
                                        if (rowToCalc["P_NET_AMOUNT"].ToString() != "")
                                        {
                                            betragNetto += Convert.ToDecimal(rowToCalc["P_NET_AMOUNT"].ToString());

                                        }
                                        break;
                                }
                            }

                            if (_objKassenabrechnung.VorfallGewaehlt == Kassenabrechnung.VorfallFilter.Einahmen)
                            {
                                headRow[0]["H_RECEIPTS"] = String.Format("{0:N}", betrag);
                            }
                            else
                            {
                                headRow[0]["H_PAYMENTS"] = String.Format("{0:N}", betrag);
                            }
                            headRow[0]["H_NET_AMOUNT"] = String.Format("{0:N}", betragNetto);
                        }
                        else
                        {
                            lblError.Text = _objKassenabrechnung.Message;
                        }
                    }
                    else
                    {
                        if (_objKassenabrechnung.VorfallGewaehlt == Kassenabrechnung.VorfallFilter.Einahmen)
                        {
                            headRow[0]["H_NET_AMOUNT"] = headRow[0]["H_RECEIPTS"];
                        }
                        else
                        {
                            headRow[0]["H_NET_AMOUNT"] = headRow[0]["H_PAYMENTS"];
                        }
                        DataRow[] docPosRows = _objKassenabrechnung.DocPos.Select("POSTING_NUMBER='" + postingNr + "'");
                        if (docPosRows.Length == 1)
                        {
                            WriteHeadToPos(postingNr, headRow[0]);
                        }
                    }
                }
                else
                {
                    return true;
                }
            }
            return bError;
        }

        /// <summary>
        /// Sichtbarkeit der Controls im Hauptgrid setzén.
        /// </summary>
        private void visibility()
        {
            Boolean bShowMark = false;
            foreach (GridViewRow gvRow in gvDaten.Rows)
            {
                DropDownList ddl = (DropDownList)gvRow.FindControl("ddlVorfall");
                TextBox txtVorfall = (TextBox)gvRow.FindControl("txtVorfall");
                Label lblPostNr = (Label)gvRow.FindControl("lblPostNr");
                TextBox txtBetragBruttoEinnahmen = (TextBox)gvRow.FindControl("txtBetragBruttoEinnahmen");
                TextBox txtBetragBruttoAusgaben = (TextBox)gvRow.FindControl("txtBetragBruttoAusgaben");
                TextBox txtDebitor = (TextBox)gvRow.FindControl("txtDebitor");
                TextBox txtKreditor = (TextBox)gvRow.FindControl("txtKreditor");
                TextBox txtDatum = (TextBox)gvRow.FindControl("txtDatum");
                TextBox txtKST = (TextBox)gvRow.FindControl("txtKST");
                TextBox txtZuordnung = (TextBox)gvRow.FindControl("txtZuordnung");
                TextBox txtFreitext = (TextBox)gvRow.FindControl("txtFreitext");
                TextBox txtAuftrag = (TextBox)gvRow.FindControl("txtAuftrag");
                TextBox txtBarcode = (TextBox)gvRow.FindControl("txtBarcode");
                ImageButton imgRefresh2 = (ImageButton)gvRow.FindControl("imgRefresh2");
                ImageButton ibtnDel = (ImageButton)gvRow.FindControl("ibtnDel");
                CheckBox chkAuswahl = (CheckBox)gvRow.FindControl("chkAuswahl");

                bool blnShowdel3 = ShowDel3(lblPostNr.Text);

                ddl.Visible = blnShowdel3;
                txtVorfall.Visible = !blnShowdel3;
                txtBetragBruttoEinnahmen.Enabled = blnShowdel3;
                txtBetragBruttoAusgaben.Enabled = blnShowdel3;
                txtDebitor.Enabled = blnShowdel3;
                txtKreditor.Enabled = blnShowdel3;
                txtDatum.Enabled = blnShowdel3;
                txtKST.Enabled = blnShowdel3;
                txtZuordnung.Enabled = (blnShowdel3 && IsEnabledZuordnung(lblPostNr.Text));
                txtFreitext.Enabled = (blnShowdel3 && IsEnabledFreitext(lblPostNr.Text));
                txtAuftrag.Enabled = blnShowdel3;
                txtBarcode.Enabled = blnShowdel3;

                chkAuswahl.Visible = ShowAuswahl(lblPostNr.Text);
                if (chkAuswahl.Visible) { bShowMark = true; }
                imgRefresh2.Enabled = blnShowdel3;
                ibtnDel.Visible = blnShowdel3;

                switch (_objKassenabrechnung.VorfallGewaehlt)
                {
                    case Kassenabrechnung.VorfallFilter.Einahmen:
                        txtBetragBruttoEinnahmen.Visible = true;
                        txtBetragBruttoAusgaben.Visible = false;
                        txtDebitor.Visible = true;
                        txtKreditor.Visible = false;
                        break;
                    case Kassenabrechnung.VorfallFilter.Ausgaben:
                        txtBetragBruttoEinnahmen.Visible = false;
                        txtBetragBruttoAusgaben.Visible = true;
                        txtDebitor.Visible = false;
                        txtKreditor.Visible = true;
                        break;
                }

            }
            cmdMark.Visible = bShowMark;
        }

        private bool IsEnabledZuordnung(string postNr)
        {
            DataRow[] rows;

            rows = _objKassenabrechnung.DocHeads.Select("POSTING_NUMBER = '" + postNr + "'");
            if (rows.Length > 0)
            {
                DataRow rowPost = rows[0];
                if (!String.IsNullOrEmpty(rowPost["TRANSACT_NUMBER"].ToString()))
                {
                    rows = _objKassenabrechnung.Geschaeftsvorfaelle.Select("TRANSACT_NUMBER = '" + rowPost["TRANSACT_NUMBER"].ToString() + "'");
                    if (rows.Length > 0)
                    {
                        DataRow dr = rows[0];
                        if ((dr["ZUONR_SPERR"].ToString() == "X") || (rowPost["ASTATUS"].ToString() == "ZA") || (rowPost["ASTATUS"].ToString() == "ZB"))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private bool IsEnabledFreitext(string postNr)
        {
            DataRow[] rows;

            rows = _objKassenabrechnung.DocHeads.Select("POSTING_NUMBER = '" + postNr + "'");
            if (rows.Length > 0)
            {
                DataRow rowPost = rows[0];
                if (!String.IsNullOrEmpty(rowPost["TRANSACT_NUMBER"].ToString()))
                {
                    rows = _objKassenabrechnung.Geschaeftsvorfaelle.Select("TRANSACT_NUMBER = '" + rowPost["TRANSACT_NUMBER"].ToString() + "'");
                    if (rows.Length > 0)
                    {
                        DataRow dr = rows[0];
                        if ((dr["TEXT_SPERR"].ToString() == "X") || (rowPost["ASTATUS"].ToString() == "ZA") || (rowPost["ASTATUS"].ToString() == "ZB"))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private bool IsEnabledDebitor(string postNr)
        {
            DataRow[] rows;

            rows = _objKassenabrechnung.DocHeads.Select("POSTING_NUMBER = '" + postNr + "'");
            if (rows.Length > 0)
            {
                DataRow rowPost = rows[0];
                if ((rowPost["ASTATUS"].ToString() == "ZA") || (rowPost["ASTATUS"].ToString() == "ZB"))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}
