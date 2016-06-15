using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AppZulassungsdienst.lib.Models;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using System.Data;
using GeneralTools.Models;
using SmartSoft.PdfLibrary;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Nacherfassung Listenansicht.
    /// </summary>
    public partial class ChangeZLDNachListe : Page
    {
        private User m_User;
        protected NacherfZLD objNacherf;
        private ZLDCommon objCommon;

        #region Events

        /// <summary>
        /// Page_Load Ereignis. Prüfen ob die Anwendung dem Benutzer zugeordnet ist. Stammdaten laden.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
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

            GridNavigation1.setGridElment(ref GridView1);
            GridNavigation1.PagerChanged += GridView1_PageIndexChanged;
            GridNavigation1.PageSizeChanged += GridView1_ddlPageSizeChanged;

            //Response.Cache.SetCacheability(HttpCacheability.ServerAndNoCache);
            //Browser Back Problem! Seite wird nicht mehr aus dem Browsercache abgerufen sondern immmer vom Server
            //IsPostback ist nachdem betätigen des BrowserBackButtons immer false
            //mit Session Variabeln kombinieren um darauf zu reagieren

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
                if (objNacherf.DataFilterActive)
                {
                    ddlSuche.SelectedValue = objNacherf.DataFilterProperty;
                    txtSuche.Text = objNacherf.DataFilterValue;
                    ibtnNoFilter.Visible = true;
                }

                // ggf. letzte Seitengröße/-nummer wiederherstellen
                if (objNacherf.LastPageSize > 0)
                {
                    GridView1.PageSize = objNacherf.LastPageSize;
                    GridNavigation1.PagerSize = objNacherf.LastPageSize;
                }

                Fillgrid(objNacherf.LastPageIndex);
                ShowHideColumns();

                // Je nach Modus bestimmte Controls ein-/ausblenden
                if (objNacherf.SelVorgang != "NZ" && objNacherf.SelVorgang != "ON" && !objNacherf.SelVorgang.StartsWith("A"))
                {
                    cmdalleBar.Visible = false;
                    cmdalleEC.Visible = false;
                    cmdalleRE.Visible = false;
                }
                else
                {
                    cmdalleEC.Visible = (!objNacherf.SelAnnahmeAH && !objNacherf.SelAenderungAngenommene && !objNacherf.SelSofortabrechnung);
                    cmdalleBar.Visible = (!objNacherf.SelAnnahmeAH && !objNacherf.SelAenderungAngenommene && !objNacherf.SelSofortabrechnung);
                    cmdalleRE.Visible = (!objNacherf.SelAnnahmeAH && !objNacherf.SelAenderungAngenommene && !objNacherf.SelSofortabrechnung);
                }

                tblGebuehr.Visible = (!objNacherf.SelAnnahmeAH && !objNacherf.SelAenderungAngenommene && !objNacherf.SelSofortabrechnung);

                if (objNacherf.SelAnnahmeAH)
                {
                    cmdOK.Text = "» alle annehmen";
                }
                else if (objNacherf.SelAenderungAngenommene)
                {
                    cmdOK.Visible = false;
                    cmdSend.Visible = false;
                }

                if (objNacherf.SelVorgang.In("VZ,VE,AV,AX"))
                {
                    tr2.Visible = false;
                }
            }
            else
            {
                lblMessage.Visible = false;
            }
        }

        /// <summary>
        /// Neuen Seitenindex ausgewählt. Auf Eingaben überprüfen(CheckGrid).
        /// </summary>
        /// <param name="pageindex">Seitenindex</param>
        private void GridView1_PageIndexChanged(Int32 pageindex)
        {
            CheckGrid(GridCheckMode.CheckNone);
            Fillgrid(pageindex);
            objNacherf.LastPageIndex = pageindex;
            Session["objNacherf"] = objNacherf;
        }

        /// <summary>
        /// Anzahl der Daten im Gridview geändert. Auf Eingaben überprüfen(CheckGrid).
        /// </summary>
        private void GridView1_ddlPageSizeChanged()
        {
            CheckGrid(GridCheckMode.CheckNone);
            Fillgrid();
            objNacherf.LastPageSize = GridView1.PageSize;
            Session["objNacherf"] = objNacherf;
        }

        /// <summary>
        /// Nach bestimmter Spalte sortieren. Auf Eingaben überprüfen(CheckGrid).
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewSortEventArgs</param>
        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            CheckGrid(GridCheckMode.CheckNone);
            Session["objNacherf"] = objNacherf;
            Fillgrid(0, e.SortExpression);
        }

        /// <summary>
        /// Zurück zur Selektionsseite.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            if (objNacherf.SelAnnahmeAH)
            {
                Response.Redirect("ChangeZLDSelect.aspx?AppID=" + Session["AppID"].ToString() + "&B=true&A=true");
            }
            else if (objNacherf.SelAenderungAngenommene)
            {
                Response.Redirect("ChangeZLDSelect.aspx?AppID=" + Session["AppID"].ToString() + "&B=true&E=true");
            }
            else if (objNacherf.SelSofortabrechnung)
            {
                Response.Redirect("ChangeZLDSelect.aspx?AppID=" + Session["AppID"].ToString() + "&B=true&S=true");
            }
            else if (objNacherf.SelUploadRechnungsanhaenge)
            {
                Response.Redirect("UploadRechnungsanhang.aspx?AppID=" + Session["AppID"].ToString());
            }
            else if (objNacherf.SelVorgang.In("VZ,VE,AV,AX"))
            {
                Response.Redirect("ChangeSelectVersand.aspx?AppID=" + Session["AppID"].ToString());
            }
            else
            {
                Response.Redirect("ChangeZLDSelect.aspx?AppID=" + Session["AppID"].ToString() + "&B=true");
            }

        }

        /// <summary>
        /// Page_PreRender-Ereignis. Spaltenübersetzung aufrufen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void Page_PreRender(object sender, EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        /// <summary>
        /// Page_Unload-Ereignis.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Unload(object sender, EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        /// <summary>
        /// GridView1_RowCommand-Ereignis.
        /// Editieren(weiterleiten zu ChangeZLDNach.aspx), Löschkennzeichzen setzen, Eingaben auf OK setzen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e"></param>
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

                        Response.Redirect("ChangeZLDNach.aspx?AppID=" + Session["AppID"].ToString() + "&ID=" + lblID.Text + "&B=true");
                        break;

                    case "Del":
                        if (objNacherf.SelSofortabrechnung)
                        {
                            lblError.Text = "Das Löschen von Vorgängen ist in dieser Funktion nicht zulässig!";
                            return;
                        }

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
                        calculateGebuehr();
                        Session["objNacherf"] = objNacherf;
                        break;

                    case "OK":
                        newLoeschkz = (objNacherf.SelAnnahmeAH ? "A" : "O");

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

                        calculateGebuehr();
                        Session["objNacherf"] = objNacherf;
                        break;

                    case "Versand":
                        newLoeschkz = "V";

                        Int32.TryParse(e.CommandArgument.ToString(), out Index);
                        lblID = (Label)GridView1.Rows[Index].FindControl("lblsapID");
                        lblLoeschKZ = (Label)GridView1.Rows[Index].FindControl("lblPosLoesch");

                        if (lblLoeschKZ.Text == "L")
                            throw new Exception("Bitte entfernen Sie zuerst das Löschkennzeichen!");

                        objNacherf.UpdateWebBearbeitungsStatus(lblID.Text, "10", newLoeschkz);

                        if (objNacherf.ErrorOccured)
                        {
                            lblError.Text = objNacherf.Message;
                            return;
                        }

                        lblLoeschKZ.Text = newLoeschkz;

                        foreach (GridViewRow row in GridView1.Rows)
                        {
                            if (GridView1.DataKeys[row.RowIndex] != null && GridView1.DataKeys[row.RowIndex]["SapId"].ToString() == lblID.Text)
                            {
                                lblLoeschKZ = (Label)row.FindControl("lblPosLoesch");
                                lblLoeschKZ.Text = newLoeschkz;

                                SetGridRowEdited(row, true);
                            }
                        }

                        calculateGebuehr();
                        Session["objNacherf"] = objNacherf;
                        break;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        /// <summary>
        /// Filterstring zusammenbauen und an das Gridview übergeben.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">ImageClickEventArgs</param>
        protected void ibtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            lblError.Text = "";

            if (ddlSuche.SelectedValue == "Zulassungsdatum")
            {
                if (!String.IsNullOrEmpty(txtSuche.Text))
                {
                    String SelDatum = ZLDCommon.toShortDateStr(txtSuche.Text);
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
                    String SelID = txtSuche.Text;
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

            ShowButtons();

            calculateGebuehr();

            trSuche.Visible = true;
            ibtnNoFilter.Visible = true;
        }

        /// <summary>
        /// Filter des Grids aufheben.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">ImageClickEventArgs</param>
        protected void ibtnNoFilter_Click(object sender, ImageClickEventArgs e)
        {
            lblError.Text = "";
            ddlSuche.SelectedIndex = 0;
            txtSuche.Text = "";
            lblMessage.Visible = false;

            CheckGrid(GridCheckMode.CheckNone);

            objNacherf.DataFilterActive = false;
            Session["objNacherf"] = objNacherf;

            Fillgrid();

            ddlSuche.SelectedIndex = 0;
            txtSuche.Text = "";

            ibtnNoFilter.Visible = false;
            ShowButtons();

            calculateGebuehr();

            cmdContinue.Visible = false;
        }

        /// <summary>
        /// Absenden der Daten aufrufen Save().
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdSend_Click(object sender, EventArgs e)
        {
            // Im "Neue AH-Vorgänge"-Modus nur Daten per "Update" in SAP speichern und Grid aktualisieren, 
            // sonst per "Save" in SAP auch SD-Aufträge und Barquittungen erzeugen
            if (objNacherf.SelAnnahmeAH)
            {
                Update(GridCheckMode.CheckOnlyRelevant, true);

                cmdSend.Enabled = false;
                cmdSave.Enabled = false;
                cmdOK.Enabled = false;
                cmdContinue.Visible = true;
                Fillgrid(0, "", GridFilterMode.ShowOnlyAandLandV);
                trSuche.Visible = false;
                lblGesamtGebAmt.Text = "0,00";
                lblGesamtGebEC.Text = "0,00";
                lblGesamtGebBar.Text = "0,00";
                lblGesamtGebRE.Text = "0,00";
                lblGesamtGeb.Text = "0,00";

                ShowHideColumns(true);
            }
            else
            {
                Save();
            }
        }

        /// <summary>
        /// Neuen Seitenindex ausgewählt.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewPageEventArgs</param>
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Fillgrid(e.NewPageIndex);
        }

        /// <summary>
        /// Alle Datensätze die kein Löschkennzeichen besitzen auf O = OK setzen!
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdOK_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            if (!CheckGrid(GridCheckMode.CheckAll))
            {
                List<ZLDVorgangUINacherfassung> liste;

                if (objNacherf.DataFilterActive)
                {
                    liste = objNacherf.Vorgangsliste.Where(vg =>
                        ZLDCommon.FilterData(vg, objNacherf.DataFilterProperty, objNacherf.DataFilterValue, true)).ToList();
                }
                else
                {
                    liste = objNacherf.Vorgangsliste;
                }

                var newLoeschkz = (objNacherf.SelAnnahmeAH ? "A" : "O");

                foreach (var item in liste)
                {
                    if (item.WebBearbeitungsStatus != "L")
                        item.WebBearbeitungsStatus = newLoeschkz;
                }

                Session["objNacherf"] = objNacherf;
                Fillgrid();
            }
        }

        /// <summary>
        /// Bei Zeilenbindung das Zulassungsdatum formatieren.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewRowEventArgs</param>
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox Zuldat = (TextBox)e.Row.FindControl("txtZulassungsdatum");
                if (!String.IsNullOrEmpty(Zuldat.Text))
                {
                    String tmpDate = Zuldat.Text;
                    Zuldat.Text = tmpDate.Substring(0, 2) + tmpDate.Substring(3, 2) + tmpDate.Substring(8, 2);
                }
            }
        }

        /// <summary>
        /// Alle Datensätze auf EC-Bezahlung setzen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdalleEC_Click(object sender, EventArgs e)
        {
            SetAlleZahlart(Zahlart.EC);
        }

        /// <summary>
        /// Alle Datensätze auf Barzahlung setzen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdalleBar_Click(object sender, EventArgs e)
        {
            SetAlleZahlart(Zahlart.Bar);
        }

        /// <summary>
        /// Alle Datensätze auf Bezahlung per Rechnung setzen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdalleRE_Click(object sender, EventArgs e)
        {
            SetAlleZahlart(Zahlart.Rechnung);
        }

        /// <summary>
        /// Exceltabelle generieren und ausgeben.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lnkCreateExcel_Click(object sender, EventArgs e)
        {
            CheckGrid(GridCheckMode.CheckNone);
            DataTable tblTemp = CreateExcelTable();

            CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory excelFactory = new CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory();
            string filename = String.Format("{0:yyyyMMdd_HHmmss_}", DateTime.Now) + m_User.UserName;
            excelFactory.CreateDocumentAndSendAsResponse(filename, tblTemp, Page);
        }

        /// <summary>
        /// Speichern der Daten aufrufen Update().
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdSave_Click(object sender, EventArgs e)
        {
            Update(GridCheckMode.CheckNone);
        }

        /// <summary>
        /// Nach dem Absenden alle nicht zum Absenden markierte Vorgänge wieder Anzeigen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
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
                cmdOK.Enabled = false;
                cmdalleEC.Enabled = false;
                cmdalleBar.Enabled = false;
                cmdalleRE.Enabled = false;
                trSuche.Visible = false;
                tblGebuehr.Visible = false;
                lblError.Text = "Keine Daten zur bestehenden Selektion vorhanden!";
            }
            else
            {
                Result.Visible = true;
                cmdSend.Enabled = true;
                cmdSave.Enabled = true;
                cmdOK.Enabled = true;
                cmdalleEC.Enabled = !objNacherf.SelAnnahmeAH;
                cmdalleBar.Enabled = !objNacherf.SelAnnahmeAH;
                cmdalleRE.Enabled = !objNacherf.SelAnnahmeAH;
                trSuche.Visible = (!objNacherf.SelAnnahmeAH && !objNacherf.SelSofortabrechnung);
                tblGebuehr.Visible = (!objNacherf.SelAnnahmeAH && !objNacherf.SelSofortabrechnung);
                Fillgrid();
                calculateGebuehr();
            }
            cmdContinue.Visible = false;

            ShowHideColumns();

            lblMessage.Visible = false;
        }

        /// <summary>
        /// Ändern der Zahlungsart auf EC für einen Vorgang. Gesamtpreiskalkulation aufrufen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void rbEC_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbEC = (RadioButton)sender;
            GridViewRow gvRow = (GridViewRow)rbEC.Parent.Parent;
            Label lblID = (Label)gvRow.FindControl("lblsapID");
            Label lblIDPos = (Label)gvRow.FindControl("lblid_pos");

            ChangeZahlart(lblID.Text, lblIDPos.Text, Zahlart.EC);
        }

        /// <summary>
        /// Ändern der Zahlungsart auf Bar für einen Vorgang. Gesamtpreiskalkulation aufrufen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void rbBar_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbBar = (RadioButton)sender;
            GridViewRow gvRow = (GridViewRow)rbBar.Parent.Parent;
            Label lblID = (Label)gvRow.FindControl("lblsapID");
            Label lblIDPos = (Label)gvRow.FindControl("lblid_pos");

            ChangeZahlart(lblID.Text, lblIDPos.Text, Zahlart.Bar);
        }

        /// <summary>
        /// Ändern der Zahlungsart auf Rechnung für einen Vorgang. Gesamtpreiskalkulation aufrufen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rbRE_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbRE = (RadioButton)sender;
            GridViewRow gvRow = (GridViewRow)rbRE.Parent.Parent;
            Label lblID = (Label)gvRow.FindControl("lblsapID");
            Label lblIDPos = (Label)gvRow.FindControl("lblid_pos");

            ChangeZahlart(lblID.Text, lblIDPos.Text, Zahlart.Rechnung);
        }

        /// <summary>
        /// Barquittungsdialog schließen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdClose_Click(object sender, EventArgs e)
        {
            MPEBarquittungen.Hide();
        }

        /// <summary>
        /// Sofortabrechnungsdialog schließen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdClose2_Click(object sender, EventArgs e)
        {
            MPESofortabrechnungen.Hide();
        }

        /// <summary>
        /// Barquittung drucken.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Print")
            {
                Session["App_ContentType"] = "Application/pdf";
                Session["App_Filepath"] = e.CommandArgument;
                ResponseHelper.Redirect("Printpdf.aspx", "_blank", "left=0,top=0,resizable=YES,scrollbars=YES");
                MPEBarquittungen.Show();
            }
        }

        /// <summary>
        /// Sofortabrechnung drucken.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView3_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Print")
            {
                Session["App_ContentType"] = "Application/pdf";
                Session["App_Filepath"] = e.CommandArgument;
                ResponseHelper.Redirect("Printpdf.aspx", "_blank", "left=0,top=0,resizable=YES,scrollbars=YES");
                MPESofortabrechnungen.Show();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Bindet die Vorgangsdaten(tblEingabeListe) an das Gridview(GridView1). 
        /// Binden der Kalkulationfunktion(Jscript) an Controls. Zeilen farbig markieren für Barkunden.
        /// </summary>
        /// <param name="intPageIndex">Index der Gridviewseite</param>
        /// <param name="strSort">Sortierung nach</param>
        /// <param name="filterMode"></param>
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

                case GridFilterMode.ShowOnlyAandLandV:
                    if (objNacherf.DataFilterActive)
                    {
                        srcList = objNacherf.Vorgangsliste.Where(vg =>
                            ZLDCommon.FilterData(vg, objNacherf.DataFilterProperty, objNacherf.DataFilterValue, true) && vg.WebBearbeitungsStatus.In("A,L,V")).ToList();
                    }
                    else
                    {
                        srcList = objNacherf.Vorgangsliste.Where(vg => vg.WebBearbeitungsStatus.In("A,L,V")).ToList();
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

            if (srcList == null || srcList.Count == 0)
            {
                GridView1.Visible = false;
                Result.Visible = false;
                trSuche.Visible = false;

                if (objNacherf.DataFilterActive)
                {
                    cmdSend.Enabled = false;
                    cmdOK.Enabled = false;
                    cmdalleEC.Enabled = false;
                    cmdalleBar.Enabled = false;
                    cmdalleRE.Enabled = false;
                    trSuche.Visible = true;
                    tblGebuehr.Visible = false;
                    ibtnNoFilter.Visible = true;
                    lblError.Text = "Keine Daten zur bestehenden Selektion vorhanden!";
                }
            }
            else
            {
                Result.Visible = true;
                GridView1.Visible = true;
                trSuche.Visible = (!objNacherf.SelAnnahmeAH && !objNacherf.SelAenderungAngenommene && !objNacherf.SelSofortabrechnung);
                Int32 intTempPageIndex = (intPageIndex > -1 ? intPageIndex : GridView1.PageIndex);
                String strTempSort = "";
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
                    System.Reflection.PropertyInfo prop = typeof(ZLDVorgangUINacherfassung).GetProperty(strTempSort);

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

                calculateGebuehr();

                // Zeilen mit gleicher ID gleich färben
                if (GridView1.DataKeys.Count > 0 && GridView1.DataKeys[0] != null)
                {
                    String myId = GridView1.DataKeys[0]["SapId"].ToString();
                    String Css = "ItemStyle";
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
                            // andere Hintergrundfarbe bei Barkunden
                            if (objNacherf.Vorgangsliste.Any(v => v.SapId == myId && v.BarzahlungKunde.IsTrue()))
                            {
                                row.CssClass = "GridTableBarkunde";
                            }

                            // Anfügen von Javascript-Funktionen(Helper.js) für die Gesamtgebührenanzeige
                            TextBox txtGebPreis = (TextBox)row.FindControl("txtGebPreis");
                            HiddenField txtGebPreisOld = (HiddenField)row.FindControl("txtGebPreisOld");
                            RadioButton rbEC = (RadioButton)row.FindControl("rbEC");
                            RadioButton rbBar = (RadioButton)row.FindControl("rbBar");
                            RadioButton rbRE = (RadioButton)row.FindControl("rbRE");
                            Label lblLoeschKZ = (Label)row.FindControl("lblPosLoesch");
                            Label lblid_pos = (Label)row.FindControl("lblid_pos");
                            HiddenField hfMenge = (HiddenField)row.FindControl("hfMenge");
                            Int32 iMenge = 1;
                            if (hfMenge.Value.IsNumeric()) Int32.TryParse(hfMenge.Value, out iMenge);

                            if (objNacherf.Vorgangsliste.Any(v => v.SapId == myId && v.PositionsNr == "10" && v.Flieger.IsTrue()))
                            {
                                row.Cells[3].CssClass = "TablePadding Flieger";
                            }

                            if (objNacherf.SelVorgang.In("VZ,VE,AV,AX"))
                            {
                                txtGebPreis.Attributes.Add("onchange", "CalculateGebAmt('" + txtGebPreis.ClientID + "','" + txtGebPreisOld.ClientID + "','" +
                                                        lblGesamtGeb.ClientID + "','" + lblLoeschKZ.ClientID + "'," + iMenge + ")");
                            }
                            else
                            {
                                if (m_User.Groups[0].Authorizationright == 0)
                                {
                                    txtGebPreis.Attributes.Add("onchange", "CalculateGebAmt('" + txtGebPreis.ClientID + "','" + txtGebPreisOld.ClientID + "','"
                                                                + lblGesamtGeb.ClientID + "','" + lblLoeschKZ.ClientID + "'," + iMenge + ")");


                                    TextBox txtPreis_Amt = (TextBox)row.FindControl("txtPreis_Amt");
                                    HiddenField txtPreis_AmtOld = (HiddenField)row.FindControl("txtPreis_AmtOld");

                                    txtPreis_Amt.Attributes.Add("onchange", "Calculate('" + txtPreis_Amt.ClientID + "','" + txtPreis_AmtOld.ClientID + "','"
                                            + lblGesamtGebAmt.ClientID + "','" + lblLoeschKZ.ClientID +
                                            "','" + rbEC.ClientID + "','" + lblGesamtGebEC.ClientID +
                                            "','" + rbBar.ClientID + "','" + lblGesamtGebBar.ClientID +
                                            "','" + rbRE.ClientID + "','" + lblGesamtGebRE.ClientID + "'," + iMenge + ")");

                                }
                                else
                                {
                                    txtGebPreis.Attributes.Add("onchange", "Calculate('" + txtGebPreis.ClientID + "','" + txtGebPreisOld.ClientID + "','"
                                    + lblGesamtGeb.ClientID + "','" + lblLoeschKZ.ClientID +
                                    "','" + rbEC.ClientID + "','" + lblGesamtGebEC.ClientID +
                                    "','" + rbBar.ClientID + "','" + lblGesamtGebBar.ClientID +
                                    "','" + rbRE.ClientID + "','" + lblGesamtGebRE.ClientID + "'," + iMenge + ")");
                                }

                            }

                            if (lblid_pos.Text != "10")
                            {
                                rbEC.Attributes.Add("style", "display:none");
                                rbBar.Attributes.Add("style", "display:none");
                                rbRE.Attributes.Add("style", "display:none");
                            }

                            // In der normalen Nacherfassung Status "A" nicht anzeigen
                            if ((!objNacherf.SelAnnahmeAH) && (lblLoeschKZ.Text == "A"))
                            {
                                lblLoeschKZ.Text = "";
                            }
                        }
                    }
                }

                if (m_User.Groups[0].Authorizationright == 1)
                {
                    // Gebühr Amt bei einigen Filialen ausblenden
                    GridView1.Columns[10].Visible = false;
                    lblGesamtGebAmt.Visible = false;
                    Label2.Visible = false;
                }
            }
        }

        /// <summary>
        /// Alle Fehlerstyles im gesamten Grid entfernen.
        /// </summary>
        private void ClearGridErrors()
        {
            foreach (GridViewRow gvRow in GridView1.Rows)
            {
                ClearGridRowErrors(gvRow);
            }
        }

        /// <summary>
        /// Fehlerstyles einer bestimmten Zeile entfernen.
        /// </summary>
        /// <param name="gvRow"></param>
        private void ClearGridRowErrors(GridViewRow gvRow)
        {
            Label ZulDate = (Label)gvRow.FindControl("lblZulassungsdatum");
            ZulDate.BackColor = System.Drawing.Color.Empty;
            TextBox txtBox = (TextBox)gvRow.FindControl("txtZulassungsdatum");
            txtBox.BorderColor = ZLDCommon.BorderColorDefault;
            txtBox = (TextBox)gvRow.FindControl("txtPreis");
            txtBox.BorderColor = ZLDCommon.BorderColorDefault;
            txtBox = (TextBox)gvRow.FindControl("txtGebPreis");
            txtBox.BorderColor = ZLDCommon.BorderColorDefault;
            txtBox = (TextBox)gvRow.FindControl("txtSteuer");
            txtBox.BorderColor = ZLDCommon.BorderColorDefault;
            txtBox = (TextBox)gvRow.FindControl("txtPreisKZ");
            txtBox.BorderColor = ZLDCommon.BorderColorDefault;
            txtBox = (TextBox)gvRow.FindControl("txtPreis_Amt");
            txtBox.BorderColor = ZLDCommon.BorderColorDefault;
            txtBox = (TextBox)gvRow.FindControl("txtKreisKZ");
            txtBox.BorderColor = ZLDCommon.BorderColorDefault;
            txtBox = (TextBox)gvRow.FindControl("txtKennzAbc");
            txtBox.BorderColor = ZLDCommon.BorderColorDefault;
        }

        /// <summary>
        /// Gebührenkalkulation beim Postback.
        /// </summary>
        private void calculateGebuehr()
        {
            Decimal gesamt = 0;
            Decimal gesamtEC = 0;
            Decimal gesamtBar = 0;
            Decimal gesamtRE = 0;

            List<ZLDVorgangUINacherfassung> liste;

            if (objNacherf.DataFilterActive)
            {
                liste = objNacherf.Vorgangsliste.Where(vg =>
                    ZLDCommon.FilterData(vg, objNacherf.DataFilterProperty, objNacherf.DataFilterValue, true)).ToList();
            }
            else
            {
                liste = objNacherf.Vorgangsliste;
            }

            foreach (var item in liste)
            {
                var mat = objCommon.MaterialStamm.FirstOrDefault(m => m.MaterialNr == item.MaterialNr);

                if (item.WebBearbeitungsStatus != "L" && item.Gebuehr.HasValue && mat != null && proofGebMat(mat.MaterialNr))
                {
                    var valueToAdd = item.Gebuehr.GetValueOrDefault(0) * item.Menge.GetValueOrDefault(0);

                    gesamt += valueToAdd;

                    if (m_User.Groups[0].Authorizationright == 1)
                    {
                        if (item.Zahlart_EC.IsTrue())
                            gesamtEC += valueToAdd;

                        if (item.Zahlart_Bar.IsTrue())
                            gesamtBar += valueToAdd;

                        if (item.Zahlart_Rechnung.IsTrue())
                            gesamtRE += valueToAdd;
                    }
                }
            }
            lblGesamtGeb.Text = String.Format("{0:0.00}", gesamt);
            if (m_User.Groups[0].Authorizationright == 1)
            {
                lblGesamtGebEC.Text = String.Format("{0:0.00}", gesamtEC);
                lblGesamtGebBar.Text = String.Format("{0:0.00}", gesamtBar);
                lblGesamtGebRE.Text = String.Format("{0:0.00}", gesamtRE);
            }
            gesamt = 0;
            gesamtEC = 0;
            gesamtBar = 0;
            gesamtRE = 0;
            //mit Gebühr Amt
            if (m_User.Groups[0].Authorizationright == 0)
            {
                foreach (var item in liste)
                {
                    var mat = objCommon.MaterialStamm.FirstOrDefault(m => m.MaterialNr == item.MaterialNr);

                    if (item.WebBearbeitungsStatus != "L" && item.GebuehrAmt.HasValue && mat != null && proofGebMat(mat.MaterialNr))
                    {
                        var valueToAdd = item.GebuehrAmt.GetValueOrDefault(0) * item.Menge.GetValueOrDefault(0);

                        gesamt += valueToAdd;

                        if (item.Zahlart_EC.IsTrue())
                            gesamtEC += valueToAdd;

                        if (item.Zahlart_Bar.IsTrue())
                            gesamtBar += valueToAdd;

                        if (item.Zahlart_Rechnung.IsTrue())
                            gesamtRE += valueToAdd;
                    }
                }
                lblGesamtGebAmt.Text = String.Format("{0:0.00}", gesamt);
                lblGesamtGebEC.Text = String.Format("{0:0.00}", gesamtEC);
                lblGesamtGebBar.Text = String.Format("{0:0.00}", gesamtBar);
                lblGesamtGebRE.Text = String.Format("{0:0.00}", gesamtRE);
            }
        }

        private Boolean CheckGrid(GridCheckMode checkMode)
        {
            ClearGridErrors();
            Boolean bError = false;
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
            bool pruefungsrelevant = false;

            if (einzelsatzPruefung)
                ClearGridRowErrors(gvRow);

            try
            {
                Label lblID = (Label)gvRow.FindControl("lblSapId");
                Label posID = (Label)gvRow.FindControl("lblid_pos");
                Label ZulDate = (Label)gvRow.FindControl("lblZulassungsdatum");
                RadioButton rb = (RadioButton)gvRow.FindControl("rbBar");
                RadioButton rbEC = (RadioButton)gvRow.FindControl("rbEC");
                RadioButton rbRE = (RadioButton)gvRow.FindControl("rbRE");
                Label lblLoeschKZ = (Label)gvRow.FindControl("lblPosLoesch");
                TextBox ZulDateBox = (TextBox)gvRow.FindControl("txtZulassungsdatum");

                var bBar = false;
                var bEC = false;
                var bRE = false;

                String Loeschkz = lblLoeschKZ.Text;
                if (lblLoeschKZ.Text == "L")
                    Loeschkz = "L";

                if (checkMode == GridCheckMode.CheckAll)
                {
                    pruefungsrelevant = true;
                }
                else if (checkMode == GridCheckMode.CheckOnlyRelevant)
                {
                    pruefungsrelevant = ((objNacherf.SelAnnahmeAH && Loeschkz == "A") || Loeschkz == "O");
                }

                var pos = objNacherf.Vorgangsliste.FirstOrDefault(vg => vg.SapId == lblID.Text && vg.PositionsNr == posID.Text);

                if (GridView1.Columns[23].Visible)
                {
                    bBar = rb.Checked;
                    bEC = rbEC.Checked;
                    bRE = rbRE.Checked;
                }
                else if (pos != null) // Bezahlung aus der Liste laden wenn die Columns nicht sichtbar
                {
                    bBar = pos.Zahlart_Bar.IsTrue();
                    bEC = pos.Zahlart_EC.IsTrue();
                    bRE = pos.Zahlart_Rechnung.IsTrue();
                }

                // nur bei der Hauptdienstleistung muss ein Zul.-Datum eingeben werden (intPosID == 10)
                // Textbox nur Sichtbar bei beauftragte Versandzulassungen, nur dann Prüfung !!
                if (posID.Text == "10" && ZulDateBox.Visible && pruefungsrelevant)
                {
                    if (String.IsNullOrEmpty(ZulDateBox.Text) || !checkDate(ZulDateBox))
                    {
                        ZulDateBox.BorderColor = ZLDCommon.BorderColorError;
                        lblError.Text = "Bitte geben Sie ein gültiges Zulassungsdatum für die markierten Dienstleistungen/Artikel ein!";
                        return true;
                    }
                }

                // nur bei der Hauptdienstleistung muss ein Zul.-Datum eingeben werden (intPosID == 10)
                // Label nur Sichtbar bei Nacherfassung, nur dann Prüfung !!
                if (posID.Text == "10" && ZulDate.Visible && pruefungsrelevant && String.IsNullOrEmpty(ZulDate.Text))
                {
                    ZulDate.BackColor = ZLDCommon.BorderColorError;
                    lblError.Text = "Bitte geben Sie ein Zulassungsdatum für die markierten Dienstleistungen/Artikel ein!";
                    return true;
                }

                TextBox txtBoxPreis = (TextBox)gvRow.FindControl("txtPreis");
                Decimal decPreis = 0;

                if (txtBoxPreis.Text.IsDecimal())
                {
                    Decimal.TryParse(txtBoxPreis.Text, out decPreis);
                    txtBoxPreis.Text = String.Format("{0:0.00}", decPreis);
                }

                // Preisprüfung nur wenn aus dem Materialstamm NULLPREIS_OK == ""
                // Preisprüfung für "neue AH-Vorgänge" überspringen
                if (pruefungsrelevant && decPreis == 0 && !objNacherf.SelAnnahmeAH)
                {
                    Label lblMatnr = (Label)gvRow.FindControl("lblMatnr");

                    var mat = objCommon.MaterialStamm.FirstOrDefault(m => m.MaterialNr == lblMatnr.Text);
                    if (txtBoxPreis.Visible && mat != null && !mat.NullpreisErlaubt)
                    {
                        txtBoxPreis.BorderColor = ZLDCommon.BorderColorError;
                        lblError.Text = "Bitte geben Sie einen Preis für die markierten Dienstleistungen/Artikel ein!";
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

                TextBox txtBoxGebuehrenAmt = (TextBox)gvRow.FindControl("txtPreis_Amt");
                Decimal decGebAmt = 0;

                if (txtBoxGebuehrenAmt.Text.IsDecimal())
                {
                    Decimal.TryParse(txtBoxGebuehrenAmt.Text, out decGebAmt);
                    txtBoxGebuehrenAmt.Text = String.Format("{0:0.00}", decGebAmt);
                }

                //ist der Kunde ein Pauschalkunde,  Gebühr und Gebühr Amt unterschiedlich und 
                //das Gebührenmaterial nicht SD relevant darf der Vorgang nicht abgesendet werden
                // Prüfung für "Sofortabrechnung" überspringen
                if (pruefungsrelevant && !objNacherf.SelSofortabrechnung && m_User.Groups[0].Authorizationright != 1 && pos != null)
                {
                    var kunde = objCommon.KundenStamm.FirstOrDefault(k => k.KundenNr == pos.KundenNr);
                    var mat = objCommon.MaterialStamm.FirstOrDefault(m => m.MaterialNr == pos.MaterialNr);

                    if (kunde != null && kunde.Pauschal && decGeb != decGebAmt && mat != null)
                    {
                        var gebMatNr = (kunde.OhneUst ? mat.GebuehrenMaterialNr : mat.GebuehrenMitUstMaterialNr);

                        bool SDRelGeb = objNacherf.GetSDRelevantsGeb(lblID.Text, posID.Text, gebMatNr);
                        if (txtBoxGebuehrenAmt.Visible && !SDRelGeb)
                        {
                            txtBoxGebuehrenAmt.BorderColor = ZLDCommon.BorderColorError;
                            lblError.Text = "Bei Pauschalkunden dürfen Gebühr und Gebühr Amt nicht unterschiedlich sein!";
                            return true;
                        }
                    }
                }

                TextBox txtBoxSteuer = (TextBox)gvRow.FindControl("txtSteuer");
                Decimal decSteuer = 0;

                if (txtBoxSteuer.Text.IsDecimal())
                {
                    Decimal.TryParse(txtBoxSteuer.Text, out decSteuer);
                    txtBoxSteuer.Text = String.Format("{0:0.00}", decSteuer);
                }

                TextBox txtBoxPreisKennz = (TextBox)gvRow.FindControl("txtPreisKZ");
                Decimal decPreisKZ = 0;

                if (txtBoxPreisKennz.Text.IsDecimal())
                {
                    Decimal.TryParse(txtBoxPreisKennz.Text, out decPreisKZ);
                    txtBoxPreisKennz.Text = String.Format("{0:0.00}", decPreisKZ);
                }

                TextBox txtAmt = (TextBox)gvRow.FindControl("txtKreisKZ");
                if (posID.Text == "10" && pruefungsrelevant && objNacherf.SelAnnahmeAH && txtAmt.Visible && String.IsNullOrEmpty(txtAmt.Text))
                {
                    txtAmt.BorderColor = ZLDCommon.BorderColorError;
                    lblError.Text = "Bitte geben Sie ein Amt ein!";
                    return true;
                }

                TextBox txtKennzAbc = (TextBox)gvRow.FindControl("txtKennzAbc");
                if (posID.Text == "10" && pruefungsrelevant && !objNacherf.SelAnnahmeAH && !objNacherf.SelSofortabrechnung && txtKennzAbc.Visible && String.IsNullOrEmpty(txtKennzAbc.Text))
                {
                    txtKennzAbc.BorderColor = ZLDCommon.BorderColorError;
                    lblError.Text = "Bitte geben Sie das vollständige Kennzeichen ein!";
                    return true;
                }

                // Daten aktualisieren
                if (pos != null)
                {
                    // immer nur die Felder ändern, die man auch im Grid bearbeiten kann
                    if (objNacherf.SelAnnahmeAH || objNacherf.SelAenderungAngenommene)
                    {
                        if (txtAmt.Visible)
                            pos.Landkreis = txtAmt.Text.NotNullOrEmpty().ToUpper();
                    }
                    else if (objNacherf.SelSofortabrechnung)
                    {
                        if (txtBoxPreis.Visible)
                            pos.Preis = decPreis;

                        if (txtBoxGebuehren.Visible)
                            pos.Gebuehr = decGeb;

                        if (txtBoxPreisKennz.Visible)
                            pos.PreisKennzeichen = decPreisKZ;
                    }
                    else
                    {
                        if (txtBoxPreis.Visible)
                            pos.Preis = decPreis;

                        if (txtBoxGebuehren.Visible)
                            pos.Gebuehr = decGeb;

                        if (txtBoxGebuehrenAmt.Visible)
                            pos.GebuehrAmt = decGebAmt;

                        if (txtBoxSteuer.Visible)
                            pos.Steuer = decSteuer;

                        if (txtBoxPreisKennz.Visible)
                            pos.PreisKennzeichen = decPreisKZ;

                        if (rbEC.Visible)
                        {
                            pos.Zahlart_Bar = bBar;
                            pos.Zahlart_EC = bEC;
                            pos.Zahlart_Rechnung = bRE;
                        }
                    }

                    if (txtKennzAbc.Visible)
                        pos.KennzeichenTeil2 = txtKennzAbc.Text.NotNullOrEmpty().ToUpper();

                    if (ZulDateBox.Visible)
                        pos.Zulassungsdatum = ZulDateBox.Text.ToNullableDateTime("ddMMyy");
                    else if (ZulDate.Visible)
                        pos.Zulassungsdatum = ZulDate.Text.ToNullableDateTime("dd.MM.yyyy");

                    if (pos.PositionsNr == "10")
                    {
                        foreach (var item in objNacherf.Vorgangsliste.Where(vg => vg.SapId == pos.SapId))
                        {
                            if (txtKennzAbc.Visible)
                                item.KennzeichenTeil2 = txtKennzAbc.Text.NotNullOrEmpty().ToUpper();

                            if (ZulDateBox.Visible)
                                item.Zulassungsdatum = ZulDateBox.Text.ToNullableDateTime("ddMMyy");
                            else if (ZulDate.Visible)
                                item.Zulassungsdatum = ZulDate.Text.ToNullableDateTime("dd.MM.yyyy");

                            if ((objNacherf.SelAnnahmeAH || objNacherf.SelAenderungAngenommene) && txtAmt.Visible)
                                item.Landkreis = txtAmt.Text.NotNullOrEmpty().ToUpper();
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

        /// <summary>
        /// Validation Datum.
        /// </summary>
        /// <param name="ZulDate">Zulassungdatum</param>
        /// <returns>false bei Eingabefehler</returns>
        private Boolean checkDate(TextBox ZulDate)
        {
            String ZDat = ZLDCommon.toShortDateStr(ZulDate.Text);

            if (String.IsNullOrEmpty(ZDat))
            {
                lblError.Text = "Ungültiges Zulassungsdatum!";
                ZulDate.BackColor = ZLDCommon.BorderColorError;
                return false;
            }

            if (!ZDat.IsDate())
            {
                lblError.Text = "Ungültiges Zulassungsdatum: Falsches Format.";
                ZulDate.BackColor = ZLDCommon.BorderColorError;
                return false;
            }

            DateTime tagesdatum = DateTime.Today;
            int i = 60;
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
                ZulDate.BackColor = ZLDCommon.BorderColorError;
                return false;
            }

            tagesdatum = DateTime.Today;
            tagesdatum = tagesdatum.AddYears(1);
            if (DateNew > tagesdatum)
            {
                lblError.Text = "Das Datum darf max. 1 Jahr in der Zukunft liegen!";
                ZulDate.BackColor = ZLDCommon.BorderColorError;
                return false;
            }

            return true;
        }

        /// <summary>
        ///  Markieren ob eine Vorgang bearbeitet wurde(Font=Bold).
        /// </summary>
        /// <param name="gvRow"></param>
        /// <param name="Edited"></param>
        private void SetGridRowEdited(GridViewRow gvRow, Boolean Edited)
        {
            try
            {
                Label lblsapID = (Label)gvRow.FindControl("lblsapID");
                Label ZulDate = (Label)gvRow.FindControl("lblZulassungsdatum");
                Label lblLoeschKZ = (Label)gvRow.FindControl("lblPosLoesch");
                Label lblKundennr = (Label)gvRow.FindControl("lblKundennr");
                Label lblKundenname = (Label)gvRow.FindControl("lblKundenname");
                Label lblMatbez = (Label)gvRow.FindControl("lblMatbez");
                Label lblZulassungsdatum = (Label)gvRow.FindControl("lblZulassungsdatum");
                Label lblReferenz1 = (Label)gvRow.FindControl("lblReferenz1");
                Label lblKennKZ1 = (Label)gvRow.FindControl("lblKennKZ1");
                Label lblReserviert = (Label)gvRow.FindControl("lblReserviert");
                Label lblWunschKennz = (Label)gvRow.FindControl("lblWunschKennz");

                lblsapID.Font.Bold = Edited;
                lblLoeschKZ.Font.Bold = Edited;
                ZulDate.Font.Bold = Edited;
                lblKundennr.Font.Bold = Edited;
                lblKundenname.Font.Bold = Edited;
                lblMatbez.Font.Bold = Edited;
                lblZulassungsdatum.Font.Bold = Edited;
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

        private void ChangeZahlart(string sapId, string posNr, Zahlart zArt)
        {
            // Zahlart aktualisieren
            objNacherf.Vorgangsliste.Where(vg => vg.SapId == sapId).ToList().ForEach(vg =>
            {
                vg.Zahlart_EC = (zArt == Zahlart.EC);
                vg.Zahlart_Bar = (zArt == Zahlart.Bar);
                vg.Zahlart_Rechnung = (zArt == Zahlart.Rechnung);
            });

            var pos = objNacherf.Vorgangsliste.FirstOrDefault(vg => vg.SapId == sapId && vg.PositionsNr == posNr);

            foreach (GridViewRow row in GridView1.Rows)
            {
                if (GridView1.DataKeys[row.RowIndex] != null)
                {
                    if (GridView1.DataKeys[row.RowIndex]["SapId"].ToString() == sapId)
                    {
                        // Zahlart im Grid bei allen dazugehörigen Positionen aktualisieren
                        var rbEC = (RadioButton)row.FindControl("rbEC");
                        var rbBar = (RadioButton)row.FindControl("rbBar");
                        var rbRE = (RadioButton)row.FindControl("rbRE");

                        rbEC.Checked = (zArt == Zahlart.EC);
                        rbBar.Checked = (zArt == Zahlart.Bar);
                        rbRE.Checked = (zArt == Zahlart.Rechnung);

                        // Preise aktualisieren
                        if (pos != null && GridView1.DataKeys[row.RowIndex]["PositionsNr"].ToString() == pos.PositionsNr)
                        {
                            var txtGebPreis = (TextBox)row.FindControl("txtGebPreis");
                            var txtPreis_Amt = (TextBox)row.FindControl("txtPreis_Amt");

                            pos.Gebuehr = txtGebPreis.Text.ToNullableDecimal();
                            pos.GebuehrAmt = (m_User.Groups[0].Authorizationright == 0 ? txtPreis_Amt.Text.ToNullableDecimal() : txtGebPreis.Text.ToNullableDecimal());
                        }
                    }
                }
            }

            calculateGebuehr();
        }

        private void SetAlleZahlart(Zahlart zArt)
        {
            lblError.Text = "";

            if (!CheckGrid(GridCheckMode.CheckNone))
            {
                List<ZLDVorgangUINacherfassung> liste;

                if (objNacherf.DataFilterActive)
                {
                    liste = objNacherf.Vorgangsliste.Where(vg =>
                        ZLDCommon.FilterData(vg, objNacherf.DataFilterProperty, objNacherf.DataFilterValue, true)).ToList();
                }
                else
                {
                    liste = objNacherf.Vorgangsliste;
                }

                foreach (var item in liste)
                {
                    item.Zahlart_EC = (zArt == Zahlart.EC);
                    item.Zahlart_Bar = (zArt == Zahlart.Bar);
                    item.Zahlart_Rechnung = (zArt == Zahlart.Rechnung);
                }

                Session["objNacherf"] = objNacherf;
                Fillgrid();
            }
        }

        /// <summary>
        /// Exceltabelle generieren.
        /// </summary>
        /// <returns></returns>
        private DataTable CreateExcelTable()
        {
            DataTable tblTemp = new DataTable();

            tblTemp.Columns.Add("ID", typeof(String));
            tblTemp.Columns.Add("L/OK", typeof(String));
            tblTemp.Columns.Add("Kundennr", typeof(String));
            tblTemp.Columns.Add("Kundenname", typeof(String));
            tblTemp.Columns.Add("Dienstleistung", typeof(String));
            tblTemp.Columns.Add("Zul.-Datum", typeof(String));
            tblTemp.Columns.Add("Referenz1", typeof(String));
            tblTemp.Columns.Add("Kennz.", typeof(String));
            tblTemp.Columns.Add("R/W", typeof(String));

            // Für "Neue AH-Vorgänge" und "Änderung angenommene Vorgänge" Excel-Layout anpassen
            if (objNacherf.SelAnnahmeAH || objNacherf.SelAenderungAngenommene)
            {
                tblTemp.Columns.Add("Referenz2", typeof(String));
                tblTemp.Columns.Add("Amt", typeof(String));
                tblTemp.Columns.Add("Bemerkung", typeof(String));
            }
            else if (objNacherf.SelSofortabrechnung)
            {
                tblTemp.Columns.Add("Preis", typeof(String));
                tblTemp.Columns.Add("Preis KZ", typeof(String));
                tblTemp.Columns.Add("EC", typeof(String));
                tblTemp.Columns.Add("Bar", typeof(String));
                tblTemp.Columns.Add("RE", typeof(String));
            }
            else
            {
                tblTemp.Columns.Add("Preis", typeof(String));
                tblTemp.Columns.Add("Gebühr", typeof(String));
                tblTemp.Columns.Add("Steuern", typeof(String));
                tblTemp.Columns.Add("Preis KZ", typeof(String));
                tblTemp.Columns.Add("EC", typeof(String));
                tblTemp.Columns.Add("Bar", typeof(String));
                tblTemp.Columns.Add("RE", typeof(String));
            }

            List<ZLDVorgangUINacherfassung> liste;

            if (objNacherf.DataFilterActive)
            {
                liste = objNacherf.Vorgangsliste.Where(vg =>
                    ZLDCommon.FilterData(vg, objNacherf.DataFilterProperty, objNacherf.DataFilterValue, true)).ToList();
            }
            else
            {
                liste = objNacherf.Vorgangsliste;
            }

            foreach (var item in liste)
            {
                DataRow NewRow = tblTemp.NewRow();

                NewRow["ID"] = item.SapId;
                NewRow["L/OK"] = item.WebBearbeitungsStatus;
                NewRow["Kundennr"] = item.KundenNr;
                NewRow["Kundenname"] = item.KundenName;
                NewRow["Dienstleistung"] = item.MaterialName;
                NewRow["Zul.-Datum"] = item.Zulassungsdatum.ToString("dd.MM.yyyy");
                NewRow["Referenz1"] = item.Referenz1;
                NewRow["Kennz."] = item.KennzeichenTeil1 + "-" + item.KennzeichenTeil2;
                NewRow["R/W"] = "";

                if (item.KennzeichenReservieren.IsTrue())
                    NewRow["R/W"] = "R";

                if (item.Wunschkennzeichen.IsTrue())
                    NewRow["R/W"] = "W";

                if (objNacherf.SelAnnahmeAH || objNacherf.SelAenderungAngenommene)
                {
                    NewRow["Referenz2"] = item.Referenz2;
                    NewRow["Amt"] = item.KennzeichenTeil1;
                    NewRow["Bemerkung"] = item.Bemerkung;
                }
                else if (objNacherf.SelSofortabrechnung)
                {
                    NewRow["Preis"] = item.Preis.ToString();
                    NewRow["Preis KZ"] = item.PreisKennzeichen.ToString();
                    NewRow["EC"] = item.Zahlart_EC.BoolToX();
                    NewRow["Bar"] = item.Zahlart_Bar.BoolToX();
                    NewRow["RE"] = item.Zahlart_Rechnung.BoolToX();
                }
                else
                {
                    NewRow["Preis"] = item.Preis.ToString();
                    NewRow["Gebühr"] = item.Gebuehr.ToString();
                    NewRow["Steuern"] = item.Steuer.ToString();
                    NewRow["Preis KZ"] = item.PreisKennzeichen.ToString();
                    NewRow["EC"] = item.Zahlart_EC.BoolToX();
                    NewRow["Bar"] = item.Zahlart_Bar.BoolToX();
                    NewRow["RE"] = item.Zahlart_Rechnung.BoolToX();
                }

                tblTemp.Rows.Add(NewRow);
            }

            if (!objNacherf.SelAnnahmeAH && !objNacherf.SelAenderungAngenommene && !objNacherf.SelSofortabrechnung)
            {
                DataRow GesRow = tblTemp.NewRow();
                calculateGebuehr();
                GesRow["Gebühr"] = lblGesamtGeb.Text;
                tblTemp.Rows.Add(GesRow);
            }

            return tblTemp;
        }

        /// <summary>
        /// Absenden der Daten und erzeugen von Aufträgen in SAP.
        /// Entfernen der positiv angelegten Datensätze aus dem Grid.  
        /// Evtl. Fehlermeldungen und/oder Barquittungsdialog anzeigen.
        /// </summary>
        private void Save()
        {
            if (!CheckGrid(GridCheckMode.CheckOnlyRelevant))
            {
                lblError.Text = "";

                objNacherf.SendVorgaengeToSap(objCommon.MaterialStamm, objCommon.StvaStamm, m_User.UserName, m_User.FirstName, m_User.LastName);

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

                cmdSend.Enabled = false;
                cmdSave.Enabled = false;
                cmdOK.Enabled = false;
                cmdalleEC.Enabled = false;
                cmdalleBar.Enabled = false;
                cmdalleRE.Enabled = false;
                cmdContinue.Visible = true;

                Fillgrid(0, "", (objNacherf.SelAnnahmeAH ? GridFilterMode.ShowOnlyAandLandV : GridFilterMode.ShowOnlyOandL));

                trSuche.Visible = false;
                lblGesamtGebAmt.Text = "0,00";
                lblGesamtGebEC.Text = "0,00";
                lblGesamtGebBar.Text = "0,00";
                lblGesamtGebRE.Text = "0,00";
                lblGesamtGeb.Text = "0,00";

                ShowHideColumns(true);

                if (objNacherf.SelSofortabrechnung)
                {
                    if (!String.IsNullOrEmpty(objNacherf.SofortabrechnungVerzeichnis))
                    {
                        DataTable showTable = new DataTable();
                        showTable.Columns.Add("FILENAME", typeof(String));
                        showTable.Columns.Add("Path", typeof(String));

                        var NetworkPath = ZLDCommon.GetDocRootPath(m_User.IsTestUser) + "sofortabrechnung\\";

                        List<byte[]> filesByte = new List<byte[]>();

                        String FolderName = objNacherf.SofortabrechnungVerzeichnis.TrimStart('/');

                        if (Directory.Exists(NetworkPath + FolderName))
                        {
                            var files = Directory.GetFiles(NetworkPath + FolderName + "\\", "*.pdf");
                            foreach (string sFile in files)
                            {
                                filesByte.Add(File.ReadAllBytes(sFile));
                            }

                            string TargetFileName = "Sofortabrechnung_" + FolderName + ".pdf";
                            string sPath = NetworkPath + FolderName + "\\" + TargetFileName;
                            // Mergen der einzelnen PDF´s in ein großes PDF
                            File.WriteAllBytes(sPath, PdfMerger.MergeFiles(filesByte, false));
                            DataRow PrintRow = showTable.NewRow();
                            PrintRow["FILENAME"] = TargetFileName;
                            PrintRow["Path"] = sPath;
                            showTable.Rows.Add(PrintRow);
                        }

                        GridView3.DataSource = showTable;
                        GridView3.DataBind();
                        MPESofortabrechnungen.Show();
                    }
                }
                else if (objNacherf.tblBarquittungen.Rows.Count > 0)
                {
                    if (!objNacherf.tblBarquittungen.Columns.Contains("Filename"))
                    {
                        objNacherf.tblBarquittungen.Columns.Add("Filename", typeof(String));
                        objNacherf.tblBarquittungen.Columns.Add("Path", typeof(String));

                        foreach (DataRow BarRow in objNacherf.tblBarquittungen.Rows)
                        {
                            BarRow["Filename"] = BarRow["BARQ_NR"].ToString() + ".pdf";
                            BarRow["Path"] = ZLDCommon.GetDocRootPath(m_User.IsTestUser) + "barquittung\\" + BarRow["BARQ_NR"].ToString() + ".pdf";
                        }
                    }
                    GridView2.DataSource = objNacherf.tblBarquittungen;
                    GridView2.DataBind();
                    MPEBarquittungen.Show();
                }

                Session["objNacherf"] = objNacherf;
            }
        }

        /// <summary>
        /// Speichern der in SAP.
        /// </summary>
        private void Update(GridCheckMode checkMode, bool annahmeAhSend = false)
        {
            if (!CheckGrid(checkMode))
            {
                lblError.Text = "";

                // Für "Neue AH-Vorgänge" vor dem Speichern in SAP eine Preisfindung durchführen 
                // (v.a. wichtig für Abmeldungen, bei denen man in der Listenansicht das Amt ergänzt/geändert hat)
                if (objNacherf.SelAnnahmeAH)
                    objNacherf.DoPreisfindung(objCommon.KundenStamm, objCommon.MaterialStamm, objCommon.StvaStamm, m_User.UserName);

                objNacherf.SaveVorgaengeToSap(objCommon.MaterialStamm, objCommon.StvaStamm, m_User.UserName, annahmeAhSend);

                Session["objNacherf"] = objNacherf;

                if (objNacherf.ErrorOccured)
                {
                    lblError.Text = objNacherf.Message;
                }
                else
                {
                    lblMessage.Visible = true;
                    lblMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#269700");
                    lblMessage.Text = "Datensätze in SAP gespeichert. Keine Fehler aufgetreten.";

                    calculateGebuehr();
                }

                Fillgrid();
            }
        }

        /// <summary>
        /// Aufruf aus dem Gridview der aspx-Seite. Prüft anhand der Vorgangsart ob das
        /// Zulassungsdatum geändert werden darf. Nur bei der Hauptposition(PosID == 10).
        /// Sichtbarkeit der TextBox.
        /// </summary>
        /// <param name="PosID">ID der Position</param>
        /// <param name="belegart">Vorgangsart</param>
        /// <returns>Visibility von txtZulassungsdatum im Gridview</returns>
        protected bool proofDateEditable(string PosID, String belegart)
        {
            bool bReturn = true;

            if (PosID == "10")
            {
                if (!objNacherf.SelAnnahmeAH && !objNacherf.SelAenderungAngenommene)
                {
                    switch (belegart)
                    {
                        case "NZ":
                        case "AN":
                        case "AA":
                        case "AB":
                        case "AG":
                        case "AS":
                        case "AU":
                        case "AF":
                        case "AK":
                        case "AZ":
                        case "ON":
                            bReturn = false;
                            break;
                    }
                }
            }
            else
            {
                bReturn = false;
            }

            return bReturn;
        }

        /// <summary>
        /// Aufruf aus dem Gridview der aspx-Seite. Prüft anhand der Vorgangsart ob das
        /// Zulassungsdatum geändert werden darf. Nur bei der Hauptposition(PosID == 10).
        /// Sichtbarkeit des Labels.
        /// </summary>
        /// <param name="PosID">ID der Position</param>
        /// <param name="belegart">Vorgangsart</param>
        /// <returns>Visibility von lbkZulassungsdatum im Gridview</returns>
        protected bool proofDateVisible(string PosID, String belegart)
        {
            bool bReturn = false;

            if (PosID == "10")
            {
                if (!objNacherf.SelAnnahmeAH && !objNacherf.SelAenderungAngenommene)
                {
                    switch (belegart)
                    {
                        case "NZ":
                        case "AN":
                        case "AA":
                        case "AB":
                        case "AG":
                        case "AS":
                        case "AU":
                        case "AF":
                        case "AK":
                        case "AZ":
                        case "ON":
                            bReturn = true;
                            break;
                    }
                }
            }

            return bReturn;
        }

        /// <summary>
        /// Aufruf aus dem Gridview der aspx-Seite. Kennzeichenpreis ausblenden 
        /// wenn es sich um einen Pauschalkunden handelt oder kein Kennzeichenmaterial zum
        /// Material hinterlegt ist.
        /// </summary>
        /// <param name="KundenNr">Kunde</param>
        /// <param name="Matnr">Materialnr.</param>
        /// <returns>Visibility von txtPreisKZ im Gridview</returns>
        protected bool proofPauschMat(String KundenNr, String Matnr)
        {
            return objCommon.proofPauschMat(KundenNr, Matnr);
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

        /// <summary>
        /// Anzeigen die Buttons je nach Aktion ein- oder ausblenden
        /// </summary>
        private void ShowButtons()
        {
            List<ZLDVorgangUINacherfassung> liste;

            if (objNacherf.DataFilterActive)
            {
                liste = objNacherf.Vorgangsliste.Where(vg =>
                    ZLDCommon.FilterData(vg, objNacherf.DataFilterProperty, objNacherf.DataFilterValue, true)).ToList();
            }
            else
            {
                liste = objNacherf.Vorgangsliste;
            }

            if (liste.Count == 0)
            {
                Result.Visible = false;
                cmdSend.Enabled = false;
                cmdSave.Enabled = false;
                cmdOK.Enabled = false;
                cmdalleEC.Enabled = false;
                cmdalleBar.Enabled = false;
                cmdalleRE.Enabled = false;
                trSuche.Visible = false;
                tblGebuehr.Visible = false;
                lblError.Text = "Keine Daten zur bestehenden Selektion vorhanden!";
            }
            else
            {
                Result.Visible = true;
                cmdSend.Enabled = !objNacherf.SelAenderungAngenommene;
                cmdSave.Enabled = true;
                cmdOK.Enabled = !objNacherf.SelAenderungAngenommene;
                cmdalleEC.Enabled = (!objNacherf.SelAnnahmeAH && !objNacherf.SelAenderungAngenommene);
                cmdalleBar.Enabled = (!objNacherf.SelAnnahmeAH && !objNacherf.SelAenderungAngenommene);
                cmdalleRE.Enabled = (!objNacherf.SelAnnahmeAH && !objNacherf.SelAenderungAngenommene);
                trSuche.Visible = (!objNacherf.SelAnnahmeAH && !objNacherf.SelAenderungAngenommene && !objNacherf.SelSofortabrechnung);
                tblGebuehr.Visible = (!objNacherf.SelAnnahmeAH && !objNacherf.SelAenderungAngenommene && !objNacherf.SelSofortabrechnung);
            }
        }

        private void ShowHideColumns(bool modusSenden = false)
        {
            // Status
            GridView1.Columns[0].Visible = modusSenden;
            // SapId
            GridView1.Columns[1].Visible = !modusSenden;
            // WebBearbeitungsStatus
            GridView1.Columns[2].Visible = !modusSenden;

            // Preis
            GridView1.Columns[8].Visible = (!objNacherf.SelAnnahmeAH && !objNacherf.SelAenderungAngenommene);
            // Gebühr
            GridView1.Columns[9].Visible = (!modusSenden && !objNacherf.SelAnnahmeAH && !objNacherf.SelAenderungAngenommene);
            // Gebühr Amt
            GridView1.Columns[10].Visible = (!modusSenden && !objNacherf.SelAnnahmeAH && !objNacherf.SelAenderungAngenommene && !objNacherf.SelSofortabrechnung);
            // Steuer
            GridView1.Columns[11].Visible = (!modusSenden && !objNacherf.SelAnnahmeAH && !objNacherf.SelAenderungAngenommene && !objNacherf.SelSofortabrechnung);
            // Preis KZ
            GridView1.Columns[12].Visible = (!objNacherf.SelAnnahmeAH && !objNacherf.SelAenderungAngenommene);

            // Referenz2
            GridView1.Columns[15].Visible = (objNacherf.SelAnnahmeAH || objNacherf.SelAenderungAngenommene);
            // KreisKZ
            GridView1.Columns[16].Visible = (objNacherf.SelAnnahmeAH || objNacherf.SelAenderungAngenommene);
            // Bemerkung
            GridView1.Columns[19].Visible = (!objNacherf.SelAnnahmeAH && !objNacherf.SelAenderungAngenommene);
            // Bemerkung AH
            GridView1.Columns[20].Visible = (objNacherf.SelAnnahmeAH || objNacherf.SelAenderungAngenommene);
            // Vorerf-Datum
            GridView1.Columns[21].Visible = objNacherf.SelAnnahmeAH;

            // Aktions-Buttons
            GridView1.Columns[22].Visible = !modusSenden;

            var showZahlart = (!modusSenden && !objNacherf.SelAnnahmeAH && !objNacherf.SelAenderungAngenommene && !objNacherf.SelSofortabrechnung
                && objNacherf.SelVorgang != "VZ" && objNacherf.SelVorgang != "VE" && objNacherf.SelVorgang != "AV" && objNacherf.SelVorgang != "AX");

            // EC
            GridView1.Columns[23].Visible = showZahlart;
            // Bar
            GridView1.Columns[24].Visible = showZahlart;
            // RE
            GridView1.Columns[25].Visible = showZahlart;

            // Adresse
            GridView1.Columns[26].Visible = objNacherf.SelSofortabrechnung;
            // Bankverbindung
            GridView1.Columns[27].Visible = objNacherf.SelSofortabrechnung;
        }

        #endregion
    }
}
