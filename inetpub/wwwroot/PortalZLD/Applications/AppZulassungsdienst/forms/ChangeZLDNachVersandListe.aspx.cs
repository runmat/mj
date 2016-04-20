using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AppZulassungsdienst.lib.Models;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using GeneralTools.Models;

namespace AppZulassungsdienst.forms
{
    public partial class ChangeZLDNachVersandListe : Page
    {
        private User m_User;
        private NacherfZLD objNacherf;
        private ZLDCommon objCommon;

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User, "");
            Common.GetAppIDFromQueryString(this);
            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            if (Session["objNacherf"] == null)
            {
                //Session-Variable weg (Session vermutlich abgelaufen) -> zurück zum Hauptmenü
                Response.Redirect("/PortalZLD/Start/Selection.aspx?AppID=" + Session["AppID"].ToString());
                return;
            }

            objNacherf = (NacherfZLD)Session["objNacherf"];
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

            if (!IsPostBack)
            {
                if (objNacherf != null)
                {
                    if (objNacherf.DataFilterActive)
                    {
                        ddlSuche.SelectedValue = objNacherf.DataFilterProperty;
                        txtSuche.Text = objNacherf.DataFilterValue;
                        ibtnNoFilter.Visible = true;
                    }

                    if (objNacherf.MatError != 0)
                        lblError.Text = objNacherf.MatErrorText;
                }

                Fillgrid();
            }
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            CheckGrid(GridCheckMode.CheckNone);
            Session["objNacherf"] = objNacherf;
            Fillgrid(0, e.SortExpression);
        }

        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChangeZLDNachVersand.aspx?AppID=" + Session["AppID"].ToString());
        }

        private void Page_PreRender(object sender, EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        private void Page_Unload(object sender, EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Int32 Index;
                Label lblID;
                Label lblIDPos;
                Label lblLoeschKZ;
                String newLoeschkz;
                lblError.Text = "";

                switch (e.CommandName)
                {
                    case "Edt":
                        CheckGrid(GridCheckMode.CheckNone);
                        Int32.TryParse(e.CommandArgument.ToString(), out Index);
                        lblID = (Label)GridView1.Rows[Index].FindControl("lblsapID");
                        objNacherf.SelEditDurchzufVersZul = true;
                        Session["objNacherf"] = objNacherf;
                        Response.Redirect("ChangeZLDNach.aspx?AppID=" + Session["AppID"].ToString() + "&ID=" + lblID.Text + "&B=true");
                        break;

                    case "Del":
                        Int32.TryParse(e.CommandArgument.ToString(), out Index);
                        lblID = (Label)GridView1.Rows[Index].FindControl("lblsapID");
                        lblIDPos = (Label)GridView1.Rows[Index].FindControl("lblid_pos");
                        lblLoeschKZ = (Label)GridView1.Rows[Index].FindControl("lblPosLoesch");

                        newLoeschkz = (lblLoeschKZ.Text == "L" ? "" : "L");

                        objNacherf.UpdateWebBearbeitungsStatus(lblID.Text, lblIDPos.Text, newLoeschkz);

                        if (objNacherf.ErrorOccured)
                        {
                            lblError.Text = objNacherf.Message;
                            return;
                        }

                        lblLoeschKZ.Text = newLoeschkz;

                        if (lblIDPos.Text == "10")
                        {
                            foreach (GridViewRow row in GridView1.Rows)
                            {
                                if (GridView1.DataKeys[row.RowIndex] != null)
                                {
                                    if (GridView1.DataKeys[row.RowIndex]["SapId"].ToString() == lblID.Text)
                                    {
                                        lblLoeschKZ = (Label)row.FindControl("lblPosLoesch");
                                        lblLoeschKZ.Text = newLoeschkz;
                                        ImageButton ibtnedt = (ImageButton)row.FindControl("ibtnedt");
                                        ibtnedt.Visible = (newLoeschkz != "L");

                                        SetGridRowEdited(row, true);
                                    }
                                }
                            }
                        }

                        SetGridRowEdited(GridView1.Rows[Index], true);
                        Session["objNacherf"] = objNacherf;
                        break;

                    case "OK":
                        newLoeschkz = "O";

                        Int32.TryParse(e.CommandArgument.ToString(), out Index);
                        if (CheckGridRow(GridView1.Rows[Index], GridCheckMode.CheckAll, true))
                            return;

                        lblID = (Label)GridView1.Rows[Index].FindControl("lblsapID");
                        lblIDPos = (Label)GridView1.Rows[Index].FindControl("lblid_pos");
                        lblLoeschKZ = (Label)GridView1.Rows[Index].FindControl("lblPosLoesch");

                        if (lblLoeschKZ.Text == "L")
                            throw new Exception("Bitte entfernen Sie zuerst das Löschkennzeichen!");

                        objNacherf.UpdateWebBearbeitungsStatus(lblID.Text, "10", newLoeschkz);

                        if (objNacherf.ErrorOccured)
                        {
                            lblError.Text = objNacherf.Message;
                            return;
                        }

                        foreach (GridViewRow row in GridView1.Rows)
                        {
                            if (GridView1.DataKeys[row.RowIndex] != null && GridView1.DataKeys[row.RowIndex]["SapId"].ToString() == lblID.Text)
                            {
                                Label IDPos = (Label)row.FindControl("lblid_pos");
                                lblLoeschKZ = (Label)row.FindControl("lblPosLoesch");
                                if (lblLoeschKZ.Text != "L")
                                {
                                    lblLoeschKZ.Text = newLoeschkz;
                                }

                                if (IDPos.Text != lblIDPos.Text && IDPos.Text != "10")
                                {
                                    CheckGridRow(row, GridCheckMode.CheckAll, true);
                                }

                                SetGridRowEdited(row, true);
                            }
                        }

                        Session["objNacherf"] = objNacherf;
                        break;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        protected void ibtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            lblError.Text = "";

            if (ddlSuche.SelectedValue == "Zulassungsdatum")
            {
                if (!String.IsNullOrEmpty(txtSuche.Text))
                {
                    var SelDatum = ZLDCommon.toShortDateStr(txtSuche.Text);
                    if (!SelDatum.IsDate())
                    {
                        lblError.Text = "Die Eingabe konnte nicht als Datum erkannt werden!(ttmmjj)";
                        return;
                    }
                }
            }
            else if (ddlSuche.SelectedValue == "SapId")
            {
                if (!String.IsNullOrEmpty(txtSuche.Text))
                {
                    var SelID = txtSuche.Text;
                    if (!SelID.IsNumeric())
                    {
                        lblError.Text = "Die Eingabe konnte nicht als numerisch erkannt werden!";
                        return;
                    }
                }
            }

            CheckGrid(GridCheckMode.CheckNone);

            objNacherf.DataFilterActive = true;
            objNacherf.DataFilterProperty = ddlSuche.SelectedValue;
            objNacherf.DataFilterValue = txtSuche.Text;
            Session["objNacherf"] = objNacherf;

            Fillgrid();

            trSuche.Visible = true;
            ibtnNoFilter.Visible = true;
        }

        protected void ibtnNoFilter_Click(object sender, ImageClickEventArgs e)
        {
            ddlSuche.SelectedIndex = 0;
            txtSuche.Text = "";

            CheckGrid(GridCheckMode.CheckNone);

            objNacherf.DataFilterActive = false;
            Session["objNacherf"] = objNacherf;

            Fillgrid();

            ibtnSearch.Visible = true;
            ibtnNoFilter.Visible = false;
        }

        protected void cmdSend_Click(object sender, EventArgs e)
        {
            if (!CheckGrid(GridCheckMode.CheckOnlyRelevant))
            {
                lblError.Text = "";

                objNacherf.SendVorgaengeToSap(objCommon.MaterialStamm, objCommon.StvaStamm, m_User.UserName, m_User.FirstName, m_User.LastName, true);

                if (objNacherf.ErrorOccured)
                {
                    lblError.Text = objNacherf.Message;
                    return;
                }

                if (objNacherf.Vorgangsliste.Any(v => !String.IsNullOrEmpty(v.FehlerText) && v.FehlerText != "OK"))
                {
                    lblError.Text = "Es konnten ein oder mehrere Aufträge nicht in SAP gespeichert werden";
                    lblMessage.Visible = false;
                }
                else
                {
                    lblMessage.Visible = true;
                    lblMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#269700");
                    lblMessage.Text = "Datensätze in SAP gespeichert. Keine Fehler aufgetreten.";
                }

                cmdSave.Enabled = false;
                cmdSend.Enabled = false;
                cmdContinue.Visible = true;

                Fillgrid(0, "", GridFilterMode.ShowOnlyOandL);

                trSuche.Visible = false;

                ShowHideColumns(true);

                Session["objNacherf"] = objNacherf;
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox Zuldat = (TextBox)e.Row.FindControl("txtZulassungsdatum");
                if (!String.IsNullOrEmpty(Zuldat.Text))
                {
                    var tmpDate = Zuldat.Text;
                    Zuldat.Text = tmpDate.Substring(0, 2) + tmpDate.Substring(3, 2) + tmpDate.Substring(8, 2);
                }
            }
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            if (!CheckGrid(GridCheckMode.CheckNone))
            {
                lblError.Text = "";

                objNacherf.SaveVorgaengeToSap(objCommon.MaterialStamm, objCommon.StvaStamm, m_User.UserName);

                if (objNacherf.ErrorOccured)
                {
                    lblError.Text = objNacherf.Message;
                }
                else
                {
                    lblMessage.Visible = true;
                    lblMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#269700");
                    lblMessage.Text = "Datensätze in SAP gespeichert. Keine Fehler aufgetreten.";
                }
            }
        }

        protected void cmdContinue_Click(object sender, EventArgs e)
        {
            objNacherf.DeleteVorgaengeOkAndDelFromLists();

            objNacherf.DataFilterActive = false;
            ddlSuche.SelectedIndex = 0;
            txtSuche.Text = "";
            ibtnSearch.Visible = true;
            ibtnNoFilter.Visible = false;

            if (objNacherf.Vorgangsliste.Count == 0)
            {
                Fillgrid();
                Result.Visible = false;
                cmdSend.Enabled = false;
                cmdSave.Enabled = false;
                trSuche.Visible = false;
                lblError.Text = "Keine Daten zur bestehenden Selektion vorhanden!";
            }
            else
            {
                Result.Visible = true;
                cmdSend.Enabled = true;
                cmdSave.Enabled = true;
                trSuche.Visible = true;
                tab1.Visible = true;
                Fillgrid();
            }

            ShowHideColumns();

            cmdContinue.Visible = false;
            cmdSend.Enabled = true;
        }

        #endregion

        #region Methods

        private void Fillgrid(Int32 intPageIndex = -1, String strSort = "", GridFilterMode filterMode = GridFilterMode.Default)
        {
            List<ZLDVorgangUINacherfassung> srcList;

            switch (filterMode)
            {
                case GridFilterMode.ShowOnlyOandL:
                    if (objNacherf.DataFilterActive)
                    {
                        srcList = objNacherf.Vorgangsliste.Where(vg =>
                            ZLDCommon.FilterData(vg, objNacherf.DataFilterProperty, objNacherf.DataFilterValue, true) && vg.WebBearbeitungsStatus.In("O,L")).ToList();
                    }
                    else
                    {
                        srcList = objNacherf.Vorgangsliste.Where(vg => vg.WebBearbeitungsStatus.In("O,L")).ToList();
                    }
                    break;

                default:
                    if (objNacherf.DataFilterActive)
                    {
                        srcList = objNacherf.Vorgangsliste.Where(vg =>
                            ZLDCommon.FilterData(vg, objNacherf.DataFilterProperty, objNacherf.DataFilterValue, true)).ToList();
                    }
                    else
                    {
                        srcList = objNacherf.Vorgangsliste;
                    }
                    break;
            }

            if (srcList.Count == 0)
            {
                GridView1.Visible = false;
                Result.Visible = false;
                trSuche.Visible = false;
            }
            else
            {
                Result.Visible = true;
                GridView1.Visible = true;
                trSuche.Visible = true;
                var intTempPageIndex = (intPageIndex > -1 ? intPageIndex : GridView1.PageIndex);
                var strTempSort = "";
                String strDirection = null;

                if (!String.IsNullOrEmpty(strSort))
                {
                    intTempPageIndex = 0;
                    strTempSort = strSort.Trim(' ');
                    if ((Session["Sort"] == null) || ((String)Session["Sort"] == strTempSort))
                    {
                        if (Session["Direction"] == null)
                        {
                            strDirection = "desc";
                        }
                        else
                        {
                            strDirection = (String)Session["Direction"];
                        }
                    }
                    else
                    {
                        strDirection = "desc";
                    }

                    if (strDirection == "asc")
                    {
                        strDirection = "desc";
                    }
                    else
                    {
                        strDirection = "asc";
                    }

                    Session["Sort"] = strTempSort;
                    Session["Direction"] = strDirection;
                }
                else if (Session["Sort"] != null)
                {
                    strTempSort = Session["Sort"].ToString();
                    strDirection = Session["Direction"].ToString();
                }
                if (!String.IsNullOrEmpty(strTempSort))
                {
                    var prop = typeof(ZLDVorgangUINacherfassung).GetProperty(strTempSort);

                    if (strDirection == "asc")
                    {
                        GridView1.DataSource = srcList.OrderBy(v => prop.GetValue(v, null)).ToList();
                    }
                    else
                    {
                        GridView1.DataSource = srcList.OrderByDescending(v => prop.GetValue(v, null)).ToList();
                    }
                }
                else
                {
                    GridView1.DataSource = srcList.OrderBy(v => v.Belegart).ThenBy(v => v.KundenNrAsSapKunnr).ThenBy(v => v.SapId.ToLong(0)).ThenBy(v => v.PositionsNr.ToInt(0)).ToList();
                }

                GridView1.PageIndex = intTempPageIndex;
                GridView1.DataBind();

                // Zeilen mit gleicher ID gleich färben
                if (GridView1.DataKeys.Count > 0 && GridView1.DataKeys[0] != null)
                {
                    var myId = GridView1.DataKeys[0]["SapId"].ToString();
                    var Css = "ItemStyle";
                    foreach (GridViewRow row in GridView1.Rows)
                    {
                        if (GridView1.DataKeys[row.RowIndex] != null)
                        {
                            if (GridView1.DataKeys[row.RowIndex]["SapId"].ToString() == myId)
                            {
                                row.CssClass = Css;
                            }
                            else
                            {
                                if (Css == "ItemStyle")
                                {
                                    Css = "GridTableAlternate2";
                                }
                                else
                                {
                                    Css = "ItemStyle";
                                }
                                row.CssClass = Css;

                                myId = GridView1.DataKeys[row.RowIndex]["SapId"].ToString();
                            }
                        }
                    }
                }
            }
        }

        private void ClearGridErrors()
        {
            foreach (GridViewRow gvRow in GridView1.Rows)
            {
                ClearGridRowErrors(gvRow);
            }
        }

        private void ClearGridRowErrors(GridViewRow gvRow)
        {
            TextBox ZulDate = (TextBox)gvRow.FindControl("txtZulassungsdatum");
            ZulDate.BorderColor = System.Drawing.Color.Empty;
            TextBox txtBox = (TextBox)gvRow.FindControl("txtGebPreis");
            txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
        }

        private Boolean CheckGrid(GridCheckMode checkMode)
        {
            ClearGridErrors();
            var bError = false;
            try
            {
                foreach (GridViewRow gvRow in GridView1.Rows)
                {
                    if (CheckGridRow(gvRow, checkMode, false))
                    {
                        bError = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Fehler beim Speichern der Daten:" + ex.Message;
                bError = true;
            }

            return bError;
        }

        private Boolean CheckGridRow(GridViewRow gvRow, GridCheckMode checkMode, bool einzelsatzPruefung)
        {
            var pruefungsrelevant = false;

            if (einzelsatzPruefung)
                ClearGridRowErrors(gvRow);

            try
            {
                Label lblID = (Label)gvRow.FindControl("lblSapId");
                Label posID = (Label)gvRow.FindControl("lblid_pos");
                TextBox ZulDate = (TextBox)gvRow.FindControl("txtZulassungsdatum");
                Label matnr = (Label)gvRow.FindControl("lblMatnr");
                RadioButton rb = (RadioButton)gvRow.FindControl("rbBar");
                RadioButton rbEC = (RadioButton)gvRow.FindControl("rbEC");
                RadioButton rbRE = (RadioButton)gvRow.FindControl("rbRE");
                Label lblLoeschKZ = (Label)gvRow.FindControl("lblPosLoesch");

                var Loeschkz = lblLoeschKZ.Text;
                if (lblLoeschKZ.Text == "L")
                    Loeschkz = "L";

                if (checkMode == GridCheckMode.CheckAll)
                {
                    pruefungsrelevant = true;
                }
                else if (checkMode == GridCheckMode.CheckOnlyRelevant)
                {
                    pruefungsrelevant = (Loeschkz == "O");
                }

                var bBar = rb.Checked;
                var bEC = rbEC.Checked;
                var bRE = rbRE.Checked;

                if (posID.Text == "10" && pruefungsrelevant)
                {
                    if (ZulDate.Visible && (String.IsNullOrEmpty(ZulDate.Text) || !checkDate(ZulDate)))
                    {
                        ZulDate.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bc2b2b");
                        lblError.Text = "Bitte geben Sie ein gültiges Zulassungsdatum für die markierten Dienstleistungen/Artikel ein!";
                        return true;
                    }
                }

                TextBox txtBoxGebuehren = (TextBox)gvRow.FindControl("txtGebPreis");
                Decimal decGeb = 0;

                if (txtBoxGebuehren.Text.IsDecimal())
                {
                    Decimal.TryParse(txtBoxGebuehren.Text, out decGeb);
                    txtBoxGebuehren.Text = String.Format("{0:0.00}", decGeb);
                }

                if (pruefungsrelevant && decGeb == 0)
                {
                    var mat = objCommon.MaterialStamm.FirstOrDefault(m => m.MaterialNr == matnr.Text);
                    if (txtBoxGebuehren.Visible && mat != null && mat.Gebuehrenpflichtig)
                    {
                        txtBoxGebuehren.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bc2b2b");
                        lblError.Text = "Bitte geben Sie die Gebühr für die markierten Dienstleistungen/Artikel ein!";
                        return true;
                    }
                }

                TextBox txtBoxKennzAbc = (TextBox)gvRow.FindControl("txtKennzAbc");
                if (posID.Text == "10" && pruefungsrelevant && txtBoxKennzAbc.Visible && String.IsNullOrEmpty(txtBoxKennzAbc.Text))
                {
                    txtBoxKennzAbc.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bc2b2b");
                    lblError.Text = "Bitte geben Sie das vollständige Kennzeichen ein!";
                    return true;
                }

                // Daten aktualisieren
                var pos = objNacherf.Vorgangsliste.FirstOrDefault(vg => vg.SapId == lblID.Text && vg.PositionsNr == posID.Text);
                if (pos != null)
                {
                    if (txtBoxGebuehren.Visible)
                    {
                        if (!pos.Gebuehrenpaket.IsTrue())
                            pos.Gebuehr = decGeb;

                        pos.GebuehrAmt = decGeb;
                    }

                    if (txtBoxKennzAbc.Visible)
                        pos.KennzeichenTeil2 = txtBoxKennzAbc.Text.NotNullOrEmpty().ToUpper();

                    if (rbEC.Visible)
                    {
                        pos.Zahlart_Bar = bBar;
                        pos.Zahlart_EC = bEC;
                        pos.Zahlart_Rechnung = bRE;
                    }

                    if (ZulDate.Visible)
                        pos.Zulassungsdatum = ZulDate.Text.ToNullableDateTime("ddMMyy");

                    if (pos.PositionsNr == "10")
                    {
                        foreach (var item in objNacherf.Vorgangsliste.Where(vg => vg.SapId == pos.SapId))
                        {
                            if (txtBoxKennzAbc.Visible)
                                item.KennzeichenTeil2 = txtBoxKennzAbc.Text.NotNullOrEmpty().ToUpper();

                            if (ZulDate.Visible)
                                item.Zulassungsdatum = ZulDate.Text.ToNullableDateTime("ddMMyy");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Fehler beim Speichern der Daten:" + ex.Message;
                return true;
            }

            return false;
        }

        private Boolean checkDate(TextBox ZulDate)
        {
            var ZDat = ZLDCommon.toShortDateStr(ZulDate.Text);

            if (String.IsNullOrEmpty(ZDat))
            {
                lblError.Text = "Ungültiges Zulassungsdatum!";
                ZulDate.BackColor = System.Drawing.ColorTranslator.FromHtml("#bc2b2b");
                return false;
            }

            if (!ZDat.IsDate())
            {
                lblError.Text = "Ungültiges Zulassungsdatum: Falsches Format.";
                ZulDate.BackColor = System.Drawing.ColorTranslator.FromHtml("#bc2b2b");
                return false;
            }

            var tagesdatum = DateTime.Today;
            var i = 60;
            do
            {
                if (tagesdatum.DayOfWeek != DayOfWeek.Saturday && tagesdatum.DayOfWeek != DayOfWeek.Sunday)
                {
                    i--;
                }
                tagesdatum = tagesdatum.AddDays(-1);
            } while (i > 0);
            DateTime DateNew;
            DateTime.TryParse(ZDat, out DateNew);
            if (DateNew < tagesdatum)
            {
                lblError.Text = "Das Datum darf max. 60 Werktage zurück liegen!";
                ZulDate.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bc2b2b");
                return false;
            }

            tagesdatum = DateTime.Today;
            tagesdatum = tagesdatum.AddYears(1);
            if (DateNew > tagesdatum)
            {
                lblError.Text = "Das Datum darf max. 1 Jahr in der Zukunft liegen!";
                ZulDate.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bc2b2b");
                return false;
            }

            return true;
        }

        private void SetGridRowEdited(GridViewRow gvRow, Boolean Edited)
        {
            try
            {
                Label lblsapID = (Label)gvRow.FindControl("lblsapID");
                Label lblLoeschKZ = (Label)gvRow.FindControl("lblPosLoesch");
                Label lblKundennr = (Label)gvRow.FindControl("lblKundennr");
                Label lblKundenname = (Label)gvRow.FindControl("lblKundenname");
                Label lblMatbez = (Label)gvRow.FindControl("lblMatbez");
                Label lblReferenz1 = (Label)gvRow.FindControl("lblReferenz1");
                Label lblKennKZ1 = (Label)gvRow.FindControl("lblKennKZ1");
                Label lblReserviert = (Label)gvRow.FindControl("lblReserviert");
                Label lblWunschKennz = (Label)gvRow.FindControl("lblWunschKennz");

                lblsapID.Font.Bold = Edited;
                lblLoeschKZ.Font.Bold = Edited;
                lblKundennr.Font.Bold = Edited;
                lblKundenname.Font.Bold = Edited;
                lblMatbez.Font.Bold = Edited;
                lblReferenz1.Font.Bold = Edited;
                lblKennKZ1.Font.Bold = Edited;
                lblReserviert.Font.Bold = Edited;
                lblWunschKennz.Font.Bold = Edited;
            }
            catch (Exception ex)
            {
                lblError.Text = "Fehler beim Speichern der Daten:" + ex.Message;
            }
        }

        /// <summary>
        /// Gebührenmaterial vorhanden?
        /// </summary>
        /// <param name="Matnr"></param>
        /// <returns></returns>
        protected bool proofGebMat(String Matnr)
        {
            return objCommon.proofGebMat(Matnr);
        }

        private void ShowHideColumns(bool modusSenden = false)
        {
            // Status
            GridView1.Columns[1].Visible = modusSenden;
            // Sapid
            GridView1.Columns[2].Visible = !modusSenden;

            // Gebühr
            GridView1.Columns[9].Visible = !modusSenden;
            // Zuldat
            GridView1.Columns[10].Visible = !modusSenden;
            // Ref1
            GridView1.Columns[11].Visible = !modusSenden;

            // Aktions-Buttons
            GridView1.Columns[15].Visible = !modusSenden;
        }

        #endregion
    }
}
