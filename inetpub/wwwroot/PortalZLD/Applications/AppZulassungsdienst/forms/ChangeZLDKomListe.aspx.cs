using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AppZulassungsdienst.lib.Models;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using System.Data;
using GeneralTools.Models;

namespace AppZulassungsdienst.forms
{   
    /// <summary>
    /// Kompletterfassung Listenansicht.
    /// </summary>
    public partial class ChangeZLDKomListe : Page
    {
        private User m_User;
        private KomplettZLD objKompletterf;
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

            if (String.IsNullOrEmpty(m_User.Reference))
            {
                lblError.Text = "Es wurde keine Benutzerreferenz angegeben! Somit können keine Stammdaten ermittelt werden!";
                return;
            }

            if (Session["objKompletterf"] == null)
            {
                //Session-Variable weg (Session vermutlich abgelaufen) -> zurück zur 1. Seite
                Response.Redirect("ChangeZLDKomplett.aspx?AppID=" + Session["AppID"].ToString());
                return;
            }

            objKompletterf = (KomplettZLD)Session["objKompletterf"];

            GridNavigation1.setGridElment(ref GridView1);
            GridNavigation1.PagerChanged += GridView1_PageIndexChanged;
            GridNavigation1.PageSizeChanged += GridView1_ddlPageSizeChanged;

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
                if (objKompletterf.DataFilterActive)
                {
                    ddlSuche.SelectedValue = objKompletterf.DataFilterProperty;
                    txtSuche.Text = objKompletterf.DataFilterValue;
                    ibtnNoFilter.Visible = true;
                }

                // ggf. letzte Seitengröße/-nummer wiederherstellen
                if (objKompletterf.LastPageSize > 0)
                {
                    GridView1.PageSize = objKompletterf.LastPageSize;
                    GridNavigation1.PagerSize = objKompletterf.LastPageSize;
                }

                Fillgrid(objKompletterf.LastPageIndex);

                LadeBenutzer();
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
            objKompletterf.LastPageIndex = pageindex;
            Session["objKompletterf"] = objKompletterf;
        }

        /// <summary>
        /// Anzahl der Daten im Gridview geändert. Auf Eingaben überprüfen(CheckGrid).
        /// </summary>
        private void GridView1_ddlPageSizeChanged()
        {
            CheckGrid(GridCheckMode.CheckNone);
            Fillgrid();
            objKompletterf.LastPageSize = GridView1.PageSize;
            Session["objKompletterf"] = objKompletterf;
        }

        /// <summary>
        /// Nach bestimmter Spalte sortieren. Auf Eingaben überprüfen(CheckGrid).
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewSortEventArgs</param>
        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            CheckGrid(GridCheckMode.CheckNone);
            Session["objKompletterf"] = objKompletterf;
            Fillgrid(0, e.SortExpression);
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
        /// Zurück zum Eingabedialog.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChangeZLDKomplett.aspx?AppID=" + Session["AppID"].ToString());
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
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void Page_Unload(object sender, EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        /// <summary>
        /// GridView1_RowCommand-Ereignis.
        /// Editieren(weiterleiten zu ChangeZLDKomplett.aspx), Löschkennzeichzen setzen, Eingaben auf OK setzen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewCommandEventArgs</param>
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Int32 Index;
                Label lblID;
                Label lblIDPos;
                Label lblLoeschKZ;

                switch (e.CommandName)
                {
                    case "Edt":
                        CheckGrid(GridCheckMode.CheckNone);
                        Int32.TryParse(e.CommandArgument.ToString(), out Index);
                        lblID = (Label)GridView1.Rows[Index].FindControl("lblsapID");

                        Response.Redirect("ChangeZLDKomplett.aspx?AppID=" + Session["AppID"].ToString() + "&ID=" + lblID.Text + "&B=true");
                        break;

                    case "Del":
                        CheckGrid(GridCheckMode.CheckNone);
                        lblError.Text = "";
                        Int32.TryParse(e.CommandArgument.ToString(), out Index);
                        lblID = (Label)GridView1.Rows[Index].FindControl("lblsapID");
                        lblIDPos = (Label)GridView1.Rows[Index].FindControl("lblid_pos");

                        objKompletterf.DeleteVorgangPosition(lblID.Text, lblIDPos.Text);

                        if (objKompletterf.ErrorOccured)
                        {
                            lblError.Text = objKompletterf.Message;
                            return;
                        }

                        calculateGebuehr();
                        Session["objKompletterf"] = objKompletterf;

                        Fillgrid();
                        break;

                    case "OK":
                        Int32.TryParse(e.CommandArgument.ToString(), out Index);
                        if (CheckGridRow(GridView1.Rows[Index], GridCheckMode.CheckAll, true))
                            return;

                        lblID = (Label)GridView1.Rows[Index].FindControl("lblsapID");
                        lblIDPos = (Label)GridView1.Rows[Index].FindControl("lblid_pos");
                        lblLoeschKZ = (Label)GridView1.Rows[Index].FindControl("lblPosLoesch");

                        if (lblLoeschKZ.Text == "L")
                            throw new Exception("Bitte entfernen Sie zuerst das Löschkennzeichen!");

                        var newLoeschkz = "O";

                        objKompletterf.UpdateWebBearbeitungsStatus(lblID.Text, "10", newLoeschkz);

                        if (objKompletterf.ErrorOccured)
                        {
                            lblError.Text = objKompletterf.Message;
                            return;
                        }

                        foreach (GridViewRow row in GridView1.Rows)
                        {
                            if (GridView1.DataKeys[row.RowIndex] != null && GridView1.DataKeys[row.RowIndex]["SapId"].ToString() == lblID.Text)
                            {
                                Label IDPos = (Label)row.FindControl("lblid_pos");
                                lblLoeschKZ = (Label)row.FindControl("lblPosLoesch");
                                lblLoeschKZ.Text = newLoeschkz;
                                if (IDPos.Text != lblIDPos.Text && IDPos.Text != "10")
                                {
                                    CheckGridRow(row, GridCheckMode.CheckAll, true);
                                }

                                SetGridRowEdited(row, true);
                            }
                        }

                        calculateGebuehr();
                        Session["objKompletterf"] = objKompletterf;
                        break;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        /// <summary>
        /// Filterstring zusammenbauen und an das Gridview übergeben
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">ImageClickEventArgs</param>
        protected void ibtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            lblError.Text = "";
            lblMessage.Visible = false;

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

            objKompletterf.DataFilterActive = true;
            objKompletterf.DataFilterProperty = ddlSuche.SelectedValue;
            objKompletterf.DataFilterValue = txtSuche.Text;
            Session["objKompletterf"] = objKompletterf;

            Fillgrid();

            ShowHideColumns();

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
            ddlSuche.SelectedIndex = 0;
            lblError.Text = "";
            txtSuche.Text = "";
            lblMessage.Visible = false;

            CheckGrid(GridCheckMode.CheckNone);

            objKompletterf.DataFilterActive = false;
            Session["objKompletterf"] = objKompletterf;

            Fillgrid();

            ShowHideColumns();

            ddlSuche.SelectedIndex = 0;
            txtSuche.Text = "";

            ibtnSearch.Visible = true;
            ibtnNoFilter.Visible = false;
            ShowButtons();

            calculateGebuehr();

            cmdContinue.Visible = false;
        }

        /// <summary>
        /// Absenden der Daten.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdSend_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            lblMessage.Text = "";

            if (!CheckGrid(GridCheckMode.CheckNone))
            {
                objKompletterf.SaveVorgaengeToSql(m_User.UserName);

                if (objKompletterf.ErrorOccured)
                {
                    lblError.Text = "Fehler beim Speichern der Daten in SQL. " + objKompletterf.Message;
                    return;
                }
            }

            if (!CheckGrid(GridCheckMode.CheckAll))
            {
                objKompletterf.SendVorgaengeToSap(objCommon.KundenStamm, m_User.UserName);

                if (objKompletterf.ErrorOccured)
                {
                    lblError.Text = objKompletterf.Message;
                    return;
                }

                tab1.Visible = true;

                if (objKompletterf.Vorgangsliste.Any(vg => !String.IsNullOrEmpty(vg.FehlerText) && vg.FehlerText != "OK"))
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

                Fillgrid(0);

                trSuche.Visible = false;
                lblGesamtGebAmt.Text = "0,00";
                lblGesamtGebEC.Text = "0,00";
                lblGesamtGebBar.Text = "0,00";
                lblGesamtGebRE.Text = "0,00";
                lblGesamtGeb.Text = "0,00";

                ShowHideColumns(true);

                if (objKompletterf.tblBarquittungen.Rows.Count > 0)
                {
                    if (!objKompletterf.tblBarquittungen.Columns.Contains("Filename"))
                    {
                        objKompletterf.tblBarquittungen.Columns.Add("Filename", typeof(String));
                        objKompletterf.tblBarquittungen.Columns.Add("Path", typeof(String));

                        foreach (DataRow BarRow in objKompletterf.tblBarquittungen.Rows)
                        {
                            BarRow["Filename"] = BarRow["BARQ_NR"].ToString() + ".pdf";
                            BarRow["Path"] = ZLDCommon.GetDocRootPath(m_User.IsTestUser) + "barquittung\\" + BarRow["BARQ_NR"].ToString() + ".pdf";
                        }
                    }
                    GridView2.DataSource = objKompletterf.tblBarquittungen;
                    GridView2.DataBind();
                    MPEBarquittungen.Show();
                }

                Session["KomplettZLD"] = objKompletterf;
            }
        }

        /// <summary>
        /// Zurück zum Eingabedialog(ChangeZLDKomplett.aspx) um neue Vorgänge zu erfassen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdCreate_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChangeZLDKomplett.aspx?AppID=" + Session["AppID"].ToString());
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
                List<ZLDVorgangUIKompletterfassung> liste;

                if (objKompletterf.DataFilterActive)
                {
                    liste = objKompletterf.Vorgangsliste.Where(vg =>
                        ZLDCommon.FilterData(vg, objKompletterf.DataFilterProperty, objKompletterf.DataFilterValue, true)).ToList();
                }
                else
                {
                    liste = objKompletterf.Vorgangsliste;
                }

                foreach (var item in liste)
                {
                    item.WebBearbeitungsStatus = "O";
                }

                Session["objKompletterf"] = objKompletterf;
                Fillgrid();
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
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdalleRE_Click(object sender, EventArgs e)
        {
            SetAlleZahlart(Zahlart.Rechnung);
        }

        /// <summary>
        /// Nach dem Absenden alle nicht zum Absenden markierte Vorgänge wieder Anzeigen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdContinue_Click(object sender, EventArgs e)
        {
            objKompletterf.DeleteVorgaengeOkFromList();

            objKompletterf.DataFilterActive = false;
            ddlSuche.SelectedIndex = 0;
            txtSuche.Text = "";
            ibtnSearch.Visible = true;
            ibtnNoFilter.Visible = false;

            if (objKompletterf.Vorgangsliste.Count == 0)
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
                cmdalleEC.Enabled = true;
                cmdalleBar.Enabled = true;
                cmdalleRE.Enabled = true;
                trSuche.Visible = true;
                tblGebuehr.Visible = true;
                tab1.Visible = true;
                Fillgrid();
                calculateGebuehr();
            }
            cmdContinue.Visible = false;

            ShowHideColumns();

            lblMessage.Visible = false;
        }

        /// <summary>
        /// Ändern der Zahlungsart auf EC für einen Vorgang. Gesamtpreiskalkulation(calculateGebuehr()) aufrufen.
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
        /// Ändern der Zahlungsart auf Bar für einen Vorgang. Gesamtpreiskalkulation(calculateGebuehr()) aufrufen.
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
        /// Ändern der Zahlungsart auf Rechnung für einen Vorgang. Gesamtpreiskalkulation(calculateGebuehr()) aufrufen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void rbRE_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbRE = (RadioButton)sender;
            GridViewRow gvRow = (GridViewRow)rbRE.Parent.Parent;
            Label lblID = (Label)gvRow.FindControl("lblsapID");
            Label lblIDPos = (Label)gvRow.FindControl("lblid_pos");

            ChangeZahlart(lblID.Text, lblIDPos.Text, Zahlart.Rechnung);
        }

        /// <summary>
        /// Barquittung drucken.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewCommandEventArgs</param>
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
        /// Barquittungsdialog schließen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdClose_Click(object sender, EventArgs e)
        {
            MPEBarquittungen.Hide();
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
            excelFactory.CreateDocumentAndSendAsResponse(filename, tblTemp, this.Page);
        }

        /// <summary>
        /// Speichern der geänderten Daten in der SQL-DB.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdSave_Click(object sender, EventArgs e)
        {
            if (!CheckGrid(GridCheckMode.CheckNone))
            {
                objKompletterf.SaveVorgaengeToSql(m_User.UserName);

                if (objKompletterf.ErrorOccured)
                {
                    lblError.Text = "Fehler beim Speichern der Daten in SQL. " + objKompletterf.Message;
                    return;
                }

                lblError.Text = "";
                lblMessage.Visible = true;
                lblMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#269700");
                lblMessage.Text = "Datensätze in der SQL-Datenbank gespeichert. Keine Fehler aufgetreten.";
                calculateGebuehr();
                Session["objKompletterf"] = objKompletterf;
            }
        }

        /// <summary>
        /// Wiederherstellen der eigenen Daten nach dem man die eines Kollegen gezogen hat.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdUnload_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            if (objKompletterf.Vorgangsliste.Any())
            {
                CheckGrid(GridCheckMode.CheckNone);
            }
            objKompletterf.LoadVorgaengeFromSql(objCommon.KundenStamm, m_User.UserName);

            Session["objKompletterf"] = objKompletterf;
            Fillgrid();
            ShowButtons();

            ShowHideColumns();

            calculateGebuehr();
            lblMessage.Visible = false;
            ddlUser.SelectedIndex = 0;
        }

        /// <summary>
        /// Laden der von Benutzern der gleichen Filiale um die angelegten Daten(z.B. im Krankheitsfall) zu ziehen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdLoad_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            if (ddlUser.SelectedValue != m_User.UserName)
            {
                if (objKompletterf.Vorgangsliste.Any())
                {
                    CheckGrid(GridCheckMode.CheckNone);
                }

                if (objKompletterf.CheckBenutzerOnline(ddlUser.SelectedValue) == "False")
                {
                    objKompletterf.LoadVorgaengeFromSql(objCommon.KundenStamm, ddlUser.SelectedValue);

                    Session["objKompletterf"] = objKompletterf;
                    Fillgrid();
                    calculateGebuehr();
                    ShowButtons();

                    ShowHideColumns();

                    if (objKompletterf.ErrorOccured)
                    {
                        lblError.Text = objKompletterf.Message;
                    }
                }
                else
                {
                    lblError.Text = "Benutzer ist angemeldet. Daten können nicht gezogen werden!";
                }
            }
            else
            {
                cmdUnload_Click(sender, e);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Laden der von Benutzern der gleichen Filiale um die angelegten Daten(z.B. im Krankheitsfall) zu ziehen.
        /// </summary>
        private void LadeBenutzer()
        {
            lblError.Text = "";
            try
            {
                objKompletterf.LadeBenutzer(m_User.UserName, m_User.Reference);
                ddlUser.Items.Clear();
                ddlUser.Items.Add(new ListItem("Eigene", m_User.UserName));
                if (objKompletterf.tblUser.Rows.Count > 0)
                {
                    foreach (DataRow item in objKompletterf.tblUser.Rows)
                    {
                        ddlUser.Items.Add(new ListItem(item["UserName"].ToString(), item["UserName"].ToString()));
                    }
                }
                else
                {
                    cmdLoad.Enabled = false;
                    cmdLoad.ToolTip = "Sie können zur Zeit keine Daten von anderen Filialbenutzern laden!";
                }
            }
            catch (Exception)
            {
                lblError.Text = "Fehler bei der Ermittlung der Filialbenutzer!";
            }
        }

        /// <summary>
        /// Bindet die Vorgangsdaten(tblEingabeListe) an das Gridview(GridView1). 
        /// Binden der Kalkulationfunktion(Jscript) an Controls. Zeilen farbig markieren für Barkunden.
        /// </summary>
        /// <param name="intPageIndex">Index der Gridviewseite</param>
        /// <param name="strSort">Sortierung nach</param>
        /// <param name="filterMode"></param>
        private void Fillgrid(Int32 intPageIndex = -1, String strSort = "", GridFilterMode filterMode = GridFilterMode.Default)
        {
            List<ZLDVorgangUIKompletterfassung> srcList;

            switch (filterMode)
            {
                case GridFilterMode.ShowOnlyOandL:
                    if (objKompletterf.DataFilterActive)
                    {
                        srcList = objKompletterf.Vorgangsliste.Where(vg =>
                            ZLDCommon.FilterData(vg, objKompletterf.DataFilterProperty, objKompletterf.DataFilterValue, true) && (vg.WebBearbeitungsStatus == "O" || vg.WebBearbeitungsStatus == "L")).ToList();
                    }
                    else
                    {
                        srcList = objKompletterf.Vorgangsliste.Where(vg => vg.WebBearbeitungsStatus == "O" || vg.WebBearbeitungsStatus == "L").ToList();
                    }
                    break;

                default:
                    if (objKompletterf.DataFilterActive)
                    {
                        srcList = objKompletterf.Vorgangsliste.Where(vg =>
                            ZLDCommon.FilterData(vg, objKompletterf.DataFilterProperty, objKompletterf.DataFilterValue, true)).ToList();
                    }
                    else
                    {
                        srcList = objKompletterf.Vorgangsliste;
                    }
                    break;
            }

            if (srcList.Count == 0)
            {
                GridView1.Visible = false;
                Result.Visible = false;
                trSuche.Visible = false;

                if (objKompletterf.DataFilterActive)
                {
                    Result.Visible = false;
                    cmdSend.Enabled = false;
                    cmdSave.Enabled = false;
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
                trSuche.Visible = true;
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
                    System.Reflection.PropertyInfo prop = typeof(ZLDVorgangUIKompletterfassung).GetProperty(strTempSort);

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
                            if (objKompletterf.Vorgangsliste.Any(v => v.SapId == myId && v.BarzahlungKunde.IsTrue()))
                            {
                                row.CssClass = "GridTableBarkunde";
                            }
                            TextBox txtGebPreis = (TextBox)row.FindControl("txtGebPreis");
                            HiddenField txtGebPreisOld = (HiddenField)row.FindControl("txtGebPreisOld");
                            RadioButton rbEC = (RadioButton)row.FindControl("rbEC");
                            RadioButton rbBar = (RadioButton)row.FindControl("rbBar");
                            RadioButton rbRE = (RadioButton)row.FindControl("rbRE");
                            Label lblLoeschKZ = (Label)row.FindControl("lblPosLoesch");
                            HiddenField hfMenge = (HiddenField)row.FindControl("hfMenge");
                            Int32 iMenge = 1;
                            if (hfMenge.Value.IsNumeric()) Int32.TryParse(hfMenge.Value, out iMenge);
                            if (m_User.Groups[0].Authorizationright == 0)
                            {
                                txtGebPreis.Attributes.Add("onchange", "CalculateGebAmt('" + txtGebPreis.ClientID + "','" + txtGebPreisOld.ClientID +
                                                        "','" + lblGesamtGeb.ClientID + "','" + lblLoeschKZ.ClientID + "'," + iMenge + ")");


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

                            Label lblid_pos = (Label)row.FindControl("lblid_pos");
                            if (lblid_pos.Text != "10")
                            {
                                rbEC.Attributes.Add("style", "display:none");
                                rbBar.Attributes.Add("style", "display:none");
                                rbRE.Attributes.Add("style", "display:none");
                            }
                        }
                    }
                    // Preis Amt bei einigen Filialen ausblenden
                    if (m_User.Groups[0].Authorizationright == 1)
                    {
                        // GebuehrAmt
                        GridView1.Columns[10].Visible = false;
                        lblGesamtGebAmt.Visible = false;
                        Label2.Visible = false;
                    }
                }
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
            if (einzelsatzPruefung)
                ClearGridRowErrors(gvRow);

            try
            {
                Label lblID = (Label)gvRow.FindControl("lblsapID");
                Label posID = (Label)gvRow.FindControl("lblid_pos");
                Label ZulDate = (Label)gvRow.FindControl("lblZulassungsdatum");
                RadioButton rb = (RadioButton)gvRow.FindControl("rbBar");
                RadioButton rbEC = (RadioButton)gvRow.FindControl("rbEC");
                RadioButton rbRE = (RadioButton)gvRow.FindControl("rbRE");

                Boolean bBar = rb.Checked;
                Boolean bEC = rbEC.Checked;
                Boolean bRE = rbRE.Checked;

                var pruefungsrelevant = (checkMode == GridCheckMode.CheckAll);

                var pos = objKompletterf.Vorgangsliste.FirstOrDefault(vg => vg.SapId == lblID.Text && vg.PositionsNr == posID.Text);

                if (posID.Text == "10" && pruefungsrelevant && ZulDate.Visible && String.IsNullOrEmpty(ZulDate.Text))
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

                if (pruefungsrelevant && decPreis == 0)
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
                if (pruefungsrelevant && m_User.Groups[0].Authorizationright != 1 && pos != null)
                {
                    var kunde = objCommon.KundenStamm.FirstOrDefault(k => k.KundenNr == pos.KundenNr);
                    var mat = objCommon.MaterialStamm.FirstOrDefault(m => m.MaterialNr == pos.MaterialNr);

                    if (kunde != null && kunde.Pauschal && decGeb != decGebAmt && mat != null)
                    {
                        var gebMatNr = (kunde.OhneUst ? mat.GebuehrenMaterialNr : mat.GebuehrenMitUstMaterialNr);

                        bool SDRelGeb = objKompletterf.GetSDRelevantsGeb(lblID.Text, posID.Text, gebMatNr);
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

                TextBox txtKennzAbc = (TextBox)gvRow.FindControl("txtKennzAbc");

                if (posID.Text == "10" && pruefungsrelevant && txtKennzAbc.Visible && String.IsNullOrEmpty(txtKennzAbc.Text))
                {
                    txtKennzAbc.BorderColor = ZLDCommon.BorderColorError;
                    lblError.Text = "Bitte geben Sie das vollständige Kennzeichen ein!";
                    return true;
                }

                // Daten aktualisieren
                if (pos != null)
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

                    if (txtKennzAbc.Visible)
                        pos.KennzeichenTeil2 = txtKennzAbc.Text.NotNullOrEmpty().ToUpper();

                    if (rbEC.Visible)
                    {
                        pos.Zahlart_Bar = bBar;
                        pos.Zahlart_EC = bEC;
                        pos.Zahlart_Rechnung = bRE;
                    }

                    if (pos.PositionsNr == "10")
                    {
                        foreach (var item in objKompletterf.Vorgangsliste.Where(vg => vg.SapId == pos.SapId))
                        {
                            if (txtKennzAbc.Visible)
                                item.KennzeichenTeil2 = txtKennzAbc.Text.NotNullOrEmpty().ToUpper();
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
        /// Markieren ob eine Vorgang bearbeitet wurde(Font=Bold).
        /// </summary>
        /// <param name="gvRow">GridviewRow</param>
        /// <param name="Edited">true/false</param>
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
        /// <param name="gvRow">GridViewRow</param>
        private void ClearGridRowErrors(GridViewRow gvRow)
        {
            Label ZulDate = (Label)gvRow.FindControl("lblZulassungsdatum");
            ZulDate.BackColor = System.Drawing.Color.Empty;
            TextBox txtBox = (TextBox)gvRow.FindControl("txtPreis");
            txtBox.BorderColor = ZLDCommon.BorderColorDefault;
            txtBox = (TextBox)gvRow.FindControl("txtGebPreis");
            txtBox.BorderColor = ZLDCommon.BorderColorDefault;
            txtBox = (TextBox)gvRow.FindControl("txtPreis_Amt");
            txtBox.BorderColor = ZLDCommon.BorderColorDefault;
            txtBox = (TextBox)gvRow.FindControl("txtSteuer");
            txtBox.BorderColor = ZLDCommon.BorderColorDefault;
            txtBox = (TextBox)gvRow.FindControl("txtPreisKZ");
            txtBox.BorderColor = ZLDCommon.BorderColorDefault;
            txtBox = (TextBox)gvRow.FindControl("txtKennzAbc");
            txtBox.BorderColor = ZLDCommon.BorderColorDefault;
            lblError.Text = "";
        }

        private void ChangeZahlart(string sapId, string posNr, Zahlart zArt)
        {
            var pos = objKompletterf.Vorgangsliste.FirstOrDefault(vg => vg.SapId == sapId && vg.PositionsNr == posNr);
            if (pos != null)
            {
                pos.Zahlart_EC = (zArt == Zahlart.EC);
                pos.Zahlart_Bar = (zArt == Zahlart.Bar);
                pos.Zahlart_Rechnung = (zArt == Zahlart.Rechnung);

                // Preise aktualisieren
                foreach (GridViewRow row in GridView1.Rows)
                {
                    if (GridView1.DataKeys[row.RowIndex] != null)
                    {
                        if (GridView1.DataKeys[row.RowIndex]["SapId"].ToString() == pos.SapId && GridView1.DataKeys[row.RowIndex]["PositionsNr"].ToString() == pos.PositionsNr)
                        {
                            TextBox txtGebPreis = (TextBox)row.FindControl("txtGebPreis");
                            TextBox txtPreis_Amt = (TextBox)row.FindControl("txtPreis_Amt");

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
                List<ZLDVorgangUIKompletterfassung> liste;

                if (objKompletterf.DataFilterActive)
                {
                    liste = objKompletterf.Vorgangsliste.Where(vg =>
                        ZLDCommon.FilterData(vg, objKompletterf.DataFilterProperty, objKompletterf.DataFilterValue, true)).ToList();
                }
                else
                {
                    liste = objKompletterf.Vorgangsliste;
                }

                foreach (var item in liste)
                {
                    item.Zahlart_EC = (zArt == Zahlart.EC);
                    item.Zahlart_Bar = (zArt == Zahlart.Bar);
                    item.Zahlart_Rechnung = (zArt == Zahlart.Rechnung);
                }

                Session["objKompletterf"] = objKompletterf;
                Fillgrid();
            }
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

            List<ZLDVorgangUIKompletterfassung> liste;

            if (objKompletterf.DataFilterActive)
            {
                liste = objKompletterf.Vorgangsliste.Where(vg =>
                    ZLDCommon.FilterData(vg, objKompletterf.DataFilterProperty, objKompletterf.DataFilterValue, true)).ToList();
            }
            else
            {
                liste = objKompletterf.Vorgangsliste;
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

        /// <summary>
        /// Exceltabelle generieren.
        /// </summary>
        /// <returns>Exceltabelle</returns>
        private DataTable CreateExcelTable()
        {
            DataTable tblTemp = new DataTable();

            tblTemp.Columns.Add("ID", typeof(String));
            tblTemp.Columns.Add("L/OK", typeof(String));
            tblTemp.Columns.Add("Kundennr", typeof(String));
            tblTemp.Columns.Add("Kundenname", typeof(String));
            tblTemp.Columns.Add("Dienstleistung", typeof(String));
            tblTemp.Columns.Add("Preis", typeof(String));
            tblTemp.Columns.Add("Gebühr", typeof(String));
            tblTemp.Columns.Add("Steuern", typeof(String));
            tblTemp.Columns.Add("Preis KZ", typeof(String));
            tblTemp.Columns.Add("Zul.-Datum", typeof(String));
            tblTemp.Columns.Add("Referenz1", typeof(String));
            tblTemp.Columns.Add("Kennz.", typeof(String));
            tblTemp.Columns.Add("R/W", typeof(String));
            tblTemp.Columns.Add("F", typeof(String));
            tblTemp.Columns.Add("EC", typeof(String));
            tblTemp.Columns.Add("Bar", typeof(String));
            tblTemp.Columns.Add("RE", typeof(String));

            List<ZLDVorgangUIKompletterfassung> liste;

            if (objKompletterf.DataFilterActive)
            {
                liste = objKompletterf.Vorgangsliste.Where(vg =>
                    ZLDCommon.FilterData(vg, objKompletterf.DataFilterProperty, objKompletterf.DataFilterValue, true)).ToList();
            }
            else
            {
                liste = objKompletterf.Vorgangsliste;
            }

            foreach (var item in liste)
            {
                DataRow NewRow = tblTemp.NewRow();

                NewRow["ID"] = item.SapId;
                NewRow["L/OK"] = item.WebBearbeitungsStatus;
                NewRow["Kundennr"] = item.KundenNr;
                NewRow["Kundenname"] = item.KundenName;
                NewRow["Dienstleistung"] = item.MaterialName;
                NewRow["Preis"] = item.Preis.ToString();
                NewRow["Gebühr"] = item.Gebuehr.ToString();
                NewRow["Steuern"] = item.Steuer.ToString();
                NewRow["Preis KZ"] = item.PreisKennzeichen.ToString();
                NewRow["Zul.-Datum"] = item.Zulassungsdatum.ToString("dd.MM.yyyy");
                NewRow["Referenz1"] = item.Referenz1;
                NewRow["Kennz."] = item.KennzeichenTeil1 + "-" + item.KennzeichenTeil2;
                NewRow["R/W"] = "";

                if (item.KennzeichenReservieren.IsTrue())
                    NewRow["R/W"] = "R";

                if (item.Wunschkennzeichen.IsTrue())
                    NewRow["R/W"] = "W";

                NewRow["EC"] = item.Zahlart_EC.BoolToX();
                NewRow["Bar"] = item.Zahlart_Bar.BoolToX();
                NewRow["RE"] = item.Zahlart_Rechnung.BoolToX();

                tblTemp.Rows.Add(NewRow);
            }

            DataRow GesRow = tblTemp.NewRow();
            calculateGebuehr();
            GesRow["Gebühr"] = lblGesamtGeb.Text;
            tblTemp.Rows.Add(GesRow);

            return tblTemp;
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
            List<ZLDVorgangUIKompletterfassung> liste;

            if (objKompletterf.DataFilterActive)
            {
                liste = objKompletterf.Vorgangsliste.Where(vg =>
                    ZLDCommon.FilterData(vg, objKompletterf.DataFilterProperty, objKompletterf.DataFilterValue, true)).ToList();
            }
            else
            {
                liste = objKompletterf.Vorgangsliste;
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
                cmdContinue.Visible = false;
                trSuche.Visible = true;
                tblGebuehr.Visible = false;
                ibtnNoFilter.Visible = false;
                lblError.Text = "Keine Daten zur bestehenden Selektion vorhanden!";
            }
            else
            {
                Result.Visible = true;
                cmdSend.Enabled = true;
                cmdSave.Enabled = true;
                cmdOK.Enabled = true;
                cmdalleEC.Enabled = true;
                cmdalleBar.Enabled = true;
                cmdalleRE.Enabled = true;
                trSuche.Visible = true;
                tblGebuehr.Visible = true;
                cmdContinue.Visible = false;
                tab1.Visible = true;
            }
        }

        private void ShowHideColumns(bool modusSenden = false)
        {
            // Fehlertext
            GridView1.Columns[0].Visible = modusSenden;
            // SapId
            GridView1.Columns[1].Visible = !modusSenden;
            // WebBearbeitungsStatus
            GridView1.Columns[2].Visible = !modusSenden;

            // Gebuehr
            GridView1.Columns[9].Visible = !modusSenden;
            // GebuehrAmt
            GridView1.Columns[10].Visible = (!modusSenden && m_User.Groups[0].Authorizationright != 1);
            // Steuer
            GridView1.Columns[11].Visible = !modusSenden;

            // Aktions-Buttons
            GridView1.Columns[17].Visible = !modusSenden;
        }

        #endregion
    }
}
