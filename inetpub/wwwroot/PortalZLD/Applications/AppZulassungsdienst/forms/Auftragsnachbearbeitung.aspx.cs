using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using GeneralTools.Models;
using Telerik.Web.UI;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Auftragsnachbearbeitung
    /// </summary>
    public partial class Auftragsnachbearbeitung : Page
    {
        private User m_User;
        private NachbearbeitungAuftrag objNachbearbeitung;
        private ZLDCommon objCommon;

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            lblError.Text = "";

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

            lbtnGestern.Attributes.Add("onclick", "SetDate( -1,'" + txtStornoZulassungsdatum.ClientID + "'); return false;");
            lbtnHeute.Attributes.Add("onclick", "SetDate( 0,'" + txtStornoZulassungsdatum.ClientID + "'); return false;");
            lbtnMorgen.Attributes.Add("onclick", "SetDate( +1,'" + txtStornoZulassungsdatum.ClientID + "'); return false;");

            if (!IsPostBack)
            {
                objNachbearbeitung = new NachbearbeitungAuftrag(m_User.Reference);
                objNachbearbeitung.StornogruendeLaden();
                Session["objNachbearbeitung"] = objNachbearbeitung;

                Title = lblHead.Text;

                FillDropdowns();

                txtSucheAuftragsnummer.Focus();
            }
            else if (Session["objNachbearbeitung"] != null)
            {
                objNachbearbeitung = (NachbearbeitungAuftrag)Session["objNachbearbeitung"];
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

        protected void cmdCreate_Click(object sender, EventArgs e)
        {
            CheckAndLoadStorno();
        }

        protected void rgPositionenDisplay_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (objNachbearbeitung.AktuellerVorgang.Positionen.Any())
            {
                rgPositionenDisplay.DataSource = objNachbearbeitung.AktuellerVorgang.Positionen.OrderBy(p => p.PositionsNr.ToInt(0)).ToList();
            }
            else
            {
                rgPositionenDisplay.DataSource = null;
            }
        }

        protected void ddlStornogrund_SelectedIndexChanged(object sender, EventArgs e)
        {
            var strGrund = ddlStornogrund.SelectedValue;

            if (strGrund == "0")
            {
                trStornoKundennummer.Visible = false;
                trStornoBegruendung.Visible = false;
                trStornoStva.Visible = false;
                trStornoKennzeichen.Visible = false;
                cmdStorno.Text = "» Stornieren ";
            }
            else
            {
                var grundRows = objNachbearbeitung.tblStornogruende.Select("STORNOGRUND = '" + strGrund + "'");

                if (grundRows.Length > 0)
                {
                    trStornoKundennummer.Visible = (grundRows[0]["KUNDE_CHG"].ToString() == "X");
                    trStornoBegruendung.Visible = (grundRows[0]["GRUND_PFLICHT"].ToString() == "X");
                    trStornoStva.Visible = (grundRows[0]["AMT_CHG"].ToString() == "X");
                    trStornoKennzeichen.Visible = (grundRows[0]["KENNZ_CHG"].ToString() == "X");
                    trStornoZulassungsdatum.Visible = (grundRows[0]["DATUM_CHG"].ToString() == "X");

                    if (grundRows[0]["PREISE_CHG"].ToString() == "X")
                    {
                        cmdStorno.Text = "» Speichern ";
                    }
                    else
                    {
                        cmdStorno.Text = "» Stornieren ";
                    }
                }
                else
                {
                    trStornoKundennummer.Visible = false;
                    trStornoBegruendung.Visible = false;
                    trStornoStva.Visible = false;
                    trStornoKennzeichen.Visible = false;
                    cmdStorno.Text = "» Stornieren ";
                }
            }
        }

        protected void ddlStornoKunde_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtStornoKundennummer.Text = ddlStornoKunde.SelectedValue;
        }

        protected void cmdAbbrechen_Click(object sender, EventArgs e)
        {
            ResetNachbearbeitung(false);
        }

        protected void cmdStorno_Click(object sender, EventArgs e)
        {
            Storno();
        }

        protected void rgPositionenEdit_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (objNachbearbeitung.AktuellerVorgang.Positionen.Any())
            {
                rgPositionenEdit.DataSource = objNachbearbeitung.AktuellerVorgang.Positionen.OrderBy(p => p.PositionsNr.ToInt(0)).ToList();
            }
            else
            {
                rgPositionenEdit.DataSource = null;
            }
        }

        protected void cmdAbsenden_Click(object sender, EventArgs e)
        {
            CheckAbsenden();
        }

        protected void btnPanelConfirmPreisminderungCancel_Click(object sender, EventArgs e)
        {
            mpeConfirmPreisminderung.Hide();
        }

        protected void btnPanelConfirmPreisminderungOK_Click(object sender, EventArgs e)
        {
            mpeConfirmPreisminderung.Hide();
            Absenden();
        }

        protected void cmdOffeneStornos_Click(object sender, EventArgs e)
        {
            Panel1.Visible = false;
            cmdCreate.Visible = false;
            cmdOffeneStornos.Visible = false;

            OffeneStornos.Visible = true;
            cmdZurSuche.Visible = true;

            objNachbearbeitung.OffeneStornosLaden(objCommon.KundenStamm);

            FillGridOffeneStornos();
        }

        protected void rgOffeneStornos_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (objNachbearbeitung.tblOffeneStornos != null)
            {
                rgOffeneStornos.DataSource = objNachbearbeitung.tblOffeneStornos;
            }
            else
            {
                rgOffeneStornos.DataSource = null;
            }
        }

        protected void rgOffeneStornos_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem gridRow = e.Item as GridDataItem;

                if (e.CommandName == "nachbearbeiten")
                {
                    objNachbearbeitung.AktuellerVorgang.Kopfdaten.SapId = gridRow["ZULBELN"].Text;
                    objNachbearbeitung.VorgangLaden();

                    Session["objNachbearbeitung"] = objNachbearbeitung;

                    if (objNachbearbeitung.ErrorOccured)
                    {
                        lblError.Text = objNachbearbeitung.Message;
                    }
                    else
                    {
                        OffeneStornos.Visible = false;
                        cmdZurSuche.Visible = false;

                        ShowVorgangInfo();

                        trVorgangPositionenDisplay.Visible = false;

                        FillGridPreiseEdit();

                        EditPreise.Visible = true;
                        cmdAbsenden.Visible = true;
                    }
                }
            }
        }

        protected void cmdZurSuche_Click(object sender, EventArgs e)
        {
            OffeneStornos.Visible = false;
            cmdZurSuche.Visible = false;

            Panel1.Visible = true;
            cmdCreate.Visible = true;
            cmdOffeneStornos.Visible = true;
        }

        protected void cmdClose_Click(object sender, EventArgs e)
        {
            MPEBarquittungen.Hide();
        }

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

        #endregion

        #region Methods

        /// <summary>
        /// Auswahl für Stornogrund und Kunde füllen
        /// </summary>
        private void FillDropdowns()
        {
            ddlStornogrund.Items.Clear();
            ddlStornogrund.Items.Add(new ListItem("", "0"));
            foreach (DataRow row in objNachbearbeitung.tblStornogruende.Rows)
            {
                ddlStornogrund.Items.Add(new ListItem(row["TXT40"].ToString(), row["STORNOGRUND"].ToString()));
            }
            ddlStornogrund.SelectedIndex = 0;

            ddlStornoKunde.DataSource = objCommon.KundenStamm.Where(k => !k.Inaktiv && !k.Cpd).ToList();
            ddlStornoKunde.DataValueField = "KundenNr";
            ddlStornoKunde.DataTextField = "Name";
            ddlStornoKunde.DataBind();
            ddlStornoKunde.SelectedIndex = 0;

            txtStornoKundennummer.Text = ddlStornoKunde.SelectedValue;
            txtStornoKundennummer.Attributes.Add("onkeyup", "FilterItems(this.value," + ddlStornoKunde.ClientID + ")");
            txtStornoKundennummer.Attributes.Add("onblur", "SetDDLValue(this," + ddlStornoKunde.ClientID + ")");
        }

        /// <summary>
        /// Prüfen, ob Vorgang stornierbar, und ggf. Vorgang laden/anzeigen
        /// </summary>
        private void CheckAndLoadStorno()
        {
            if (String.IsNullOrEmpty(txtSucheAuftragsnummer.Text) && String.IsNullOrEmpty(txtSucheId.Text))
            {
                lblError.Text = "Bitte geben Sie eine Auftragsnummer oder eine ID an!";
                return;
            }

            objNachbearbeitung.SucheAuftragsnummer = txtSucheAuftragsnummer.Text.Trim();
            objNachbearbeitung.SucheId = txtSucheId.Text.Trim();

            objNachbearbeitung.VorgangPruefen();

            if (objNachbearbeitung.ErrorOccured)
            {
                lblError.Text = objNachbearbeitung.Message;
            }
            else
            {
                objNachbearbeitung.VorgangLaden();
            }

            Session["objNachbearbeitung"] = objNachbearbeitung;

            if (objNachbearbeitung.ErrorOccured)
            {
                lblError.Text = objNachbearbeitung.Message;
            }
            else
            {
                Panel1.Visible = false;
                cmdCreate.Visible = false;
                cmdOffeneStornos.Visible = false;

                ShowVorgangInfo();

                StornoDetails.Visible = true;
                cmdAbbrechen.Visible = true;
                cmdStorno.Visible = true;
            }
        }

        /// <summary>
        /// Anzeige Daten des aktuellen Vorgangs
        /// </summary>
        private void ShowVorgangInfo()
        {
            var kopfdaten = objNachbearbeitung.AktuellerVorgang.Kopfdaten;

            var kunde = objCommon.KundenStamm.FirstOrDefault(k => k.KundenNr == kopfdaten.KundenNr);

            lblIDDisplay.Text = kopfdaten.SapId;
            lblAuftragsnummerDisplay.Text = kopfdaten.AuftragsNr;
            lblKundennummerDisplay.Text = kopfdaten.KundenNr;
            lblKundeDisplay.Text = (kunde != null ? kunde.Name1 : "");
            lblReferenz1Display.Text = kopfdaten.Referenz1;
            lblZulassungsdatumDisplay.Text = kopfdaten.Zulassungsdatum.ToString("dd.MM.yyyy");
            lblKennzeichenDisplay.Text = kopfdaten.Kennzeichen;
            lblStatusDisplay.Text = kopfdaten.Kopfstatus;

            FillGridPositionenDisplay();

            ddlStornogrund.SelectedIndex = 0;
            ddlStornoKunde.SelectedIndex = 0;

            VorgangInfo.Visible = true;
            trVorgangPositionenDisplay.Visible = true;
        }

        /// <summary>
        /// Positions-Grid füllen
        /// </summary>
        private void FillGridPositionenDisplay()
        {
            if (objNachbearbeitung.AktuellerVorgang.Positionen.Any())
            {
                rgPositionenDisplay.Visible = true;
                rgPositionenDisplay.Rebind();
                // Setzen der DataSource geschieht durch das NeedDataSource-Event
            }
            else
            {
                rgPositionenDisplay.Visible = false;
            }
        }

        /// <summary>
        /// Storno durchführen und anschließend ggf. einen Nachfolgevorgang weiterbearbeiten
        /// </summary>
        private void Storno()
        {
            try
            {
                if (ddlStornogrund.SelectedValue == "0")
                {
                    lblError.Text = "Bitte wählen Sie einen Storno-Grund aus!";
                    return;
                }
                objNachbearbeitung.Stornogrund = ddlStornogrund.SelectedValue;

                if (trStornoKundennummer.Visible)
                {
                    if (ddlStornoKunde.SelectedValue == "0")
                    {
                        lblError.Text = "Bitte wählen Sie einen Kunden aus!";
                        return;
                    }
                    objNachbearbeitung.StornoKundennummer = ddlStornoKunde.SelectedValue;
                }

                if (trStornoBegruendung.Visible)
                {
                    if (String.IsNullOrEmpty(txtStornoBegruendung.Text))
                    {
                        lblError.Text = "Bitte geben Sie eine Begründung an!";
                        return;
                    }
                    objNachbearbeitung.StornoBegruendung = txtStornoBegruendung.Text;
                }

                if (trStornoStva.Visible)
                {
                    if (String.IsNullOrEmpty(txtStornoAmt.Text))
                    {
                        lblError.Text = "Bitte geben Sie ein Amt an!";
                        return;
                    }
                    objNachbearbeitung.StornoStva = txtStornoAmt.Text.ToUpper();
                }

                if (trStornoKennzeichen.Visible)
                {
                    if (String.IsNullOrEmpty(txtStornoKennz1.Text) || String.IsNullOrEmpty(txtStornoKennz2.Text))
                    {
                        lblError.Text = "Bitte geben Sie ein vollständiges Kennzeichen an!";
                        return;
                    }
                    objNachbearbeitung.StornoKennzeichen = txtStornoKennz1.Text.ToUpper() + "-" + txtStornoKennz2.Text.ToUpper();
                }

                if (trStornoZulassungsdatum.Visible)
                {
                    if (!checkDate())
                        return;

                    objNachbearbeitung.StornoZulassungsdatum = txtStornoZulassungsdatum.Text.ToNullableDateTime("ddMMyy");
                }

                objNachbearbeitung.VorgangStornieren(m_User.UserName);

                Session["objNachbearbeitung"] = objNachbearbeitung;

                if (objNachbearbeitung.ErrorOccured)
                {
                    lblError.Text = "Fehler beim Stornieren: " + objNachbearbeitung.Message;
                    return;
                }

                VorgangInfo.Visible = false;
                StornoDetails.Visible = false;
                cmdAbbrechen.Visible = false;
                cmdStorno.Visible = false;

                var grundRows = objNachbearbeitung.tblStornogruende.Select("STORNOGRUND = '" + objNachbearbeitung.Stornogrund + "'");

                // Ggf. noch neuen Vorgang laden/anzeigen und zur Preisbearbeitung wechseln
                if (grundRows.Length > 0 && grundRows[0]["PREISE_CHG"].ToString() == "X")
                {
                    lblError.Text = "Vorgang erfolgreich angelegt";

                    if (!String.IsNullOrEmpty(objNachbearbeitung.AktuellerVorgang.Kopfdaten.SapId))
                    {
                        objNachbearbeitung.VorgangLaden();

                        Session["objNachbearbeitung"] = objNachbearbeitung;

                        if (objNachbearbeitung.ErrorOccured)
                        {
                            lblError.Text = objNachbearbeitung.Message;
                        }
                        else
                        {
                            ShowVorgangInfo();

                            trVorgangPositionenDisplay.Visible = false;

                            FillGridPreiseEdit();

                            EditPreise.Visible = true;
                            cmdAbsenden.Visible = true;
                        }
                    }
                }
                else
                {
                    lblError.Text = "Vorgang erfolgreich storniert";

                    ResetNachbearbeitung(true);
                }

                if (objNachbearbeitung.tblBarquittungen.Rows.Count > 0)
                {
                    if (!objNachbearbeitung.tblBarquittungen.Columns.Contains("Filename"))
                    {
                        objNachbearbeitung.tblBarquittungen.Columns.Add("Filename", typeof(String));
                        objNachbearbeitung.tblBarquittungen.Columns.Add("Path", typeof(String));

                        foreach (DataRow BarRow in objNachbearbeitung.tblBarquittungen.Rows)
                        {
                            BarRow["Filename"] = BarRow["BARQ_NR"].ToString() + ".pdf";
                            BarRow["Path"] = ZLDCommon.GetDocRootPath(m_User.IsTestUser) + "barquittung\\" + BarRow["BARQ_NR"].ToString() + ".pdf";
                        }
                    }
                    GridView2.DataSource = objNachbearbeitung.tblBarquittungen;
                    GridView2.DataBind();
                    MPEBarquittungen.Show();
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Fehler: " + ex.Message;
            }
        }

        /// <summary>
        /// Grid für Preisbearbeitung füllen
        /// </summary>
        private void FillGridPreiseEdit()
        {
            if (objNachbearbeitung.AktuellerVorgang.Positionen.Any())
            {
                rgPositionenEdit.Visible = true;
                rgPositionenEdit.Rebind();
                // Setzen der DataSource geschieht durch das NeedDataSource-Event
            }
            else
            {
                rgPositionenEdit.Visible = false;
            }
        }

        /// <summary>
        /// Vor dem Absenden prüfen, ob Preise verringert wurden, und ggf. Warnhinweis anzeigen
        /// </summary>
        private void CheckAbsenden()
        {
            var blnPreisminderung = false;

            foreach (GridDataItem item in rgPositionenEdit.Items)
            {
                if ((item.ItemType == GridItemType.Item) || (item.ItemType == GridItemType.AlternatingItem))
                {
                    var txtPreis = (TextBox)item.FindControl("txtPreis");
                    var txtGebuehr = (TextBox)item.FindControl("txtGebuehr");
                    var txtGebuehrAmt = (TextBox)item.FindControl("txtGebuehrAmt");
                    var txtSteuer = (TextBox)item.FindControl("txtSteuer");
                    var txtPreisKennz = (TextBox)item.FindControl("txtPreisKennz");

                    var pos = objNachbearbeitung.AktuellerVorgang.Positionen.FirstOrDefault(p => p.PositionsNr == item["PositionsNr"].Text);
                    if (pos != null)
                    {
                        decimal preisNeu;
                        decimal gebAmtNeu;

                        switch (pos.WebMaterialart)
                        {
                            case "D":
                                if (Decimal.TryParse(txtPreis.Text, out preisNeu))
                                {
                                    if (pos.Preis > preisNeu)
                                        blnPreisminderung = true;
                                }
                                break;

                            case "G":
                                if (Decimal.TryParse(txtGebuehr.Text, out preisNeu))
                                {
                                    if (pos.Preis > preisNeu)
                                        blnPreisminderung = true;
                                }
                                break;

                            case "S":
                                if (Decimal.TryParse(txtSteuer.Text, out preisNeu))
                                {
                                    if (pos.Preis > preisNeu)
                                        blnPreisminderung = true;
                                }
                                break;

                            case "K":
                                if (Decimal.TryParse(txtPreisKennz.Text, out preisNeu))
                                {
                                    if (pos.Preis > preisNeu)
                                        blnPreisminderung = true;
                                }
                                break;
                        }

                        if (Decimal.TryParse(txtGebuehrAmt.Text, out gebAmtNeu))
                        {
                            if (pos.GebuehrAmt > gebAmtNeu)
                                blnPreisminderung = true;
                        }
                    }
                }
            }

            if (blnPreisminderung)
            {
                mpeConfirmPreisminderung.Show();
            }
            else
            {
                Absenden();
            }
        }

        /// <summary>
        /// Preisänderungen aus Grid übernehmen und Vorgang in SAP speichern
        /// </summary>
        private void Absenden()
        {
            try
            {
                foreach (GridDataItem item in rgPositionenEdit.Items)
                {
                    if ((item.ItemType == GridItemType.Item) || (item.ItemType == GridItemType.AlternatingItem))
                    {
                        var txtPreis = (TextBox)item.FindControl("txtPreis");
                        var txtGebuehr = (TextBox)item.FindControl("txtGebuehr");
                        var txtGebuehrAmt = (TextBox)item.FindControl("txtGebuehrAmt");
                        var txtSteuer = (TextBox)item.FindControl("txtSteuer");
                        var txtPreisKennz = (TextBox)item.FindControl("txtPreisKennz");

                        var pos = objNachbearbeitung.AktuellerVorgang.Positionen.FirstOrDefault(p => p.PositionsNr == item["PositionsNr"].Text);
                        if (pos != null)
                        {
                            decimal preisNeu;
                            decimal gebAmtNeu;

                            switch (pos.WebMaterialart)
                            {
                                case "D":
                                    if (Decimal.TryParse(txtPreis.Text, out preisNeu))
                                    {
                                        pos.Preis = preisNeu;
                                    }
                                    break;

                                case "G":
                                    if (Decimal.TryParse(txtGebuehr.Text, out preisNeu))
                                    {
                                        pos.Preis = preisNeu;
                                    }
                                    break;

                                case "S":
                                    if (Decimal.TryParse(txtSteuer.Text, out preisNeu))
                                    {
                                        pos.Preis = preisNeu;
                                    }
                                    break;

                                case "K":
                                    if (Decimal.TryParse(txtPreisKennz.Text, out preisNeu))
                                    {
                                        pos.Preis = preisNeu;
                                    }
                                    break;
                            }

                            if (Decimal.TryParse(txtGebuehrAmt.Text, out gebAmtNeu))
                            {
                                pos.GebuehrAmt = gebAmtNeu;
                            }
                        }
                    }
                }

                objNachbearbeitung.VorgangAbsenden();

                Session["objNachbearbeitung"] = objNachbearbeitung;

                if (objNachbearbeitung.ErrorOccured)
                {
                    lblError.Text = objNachbearbeitung.Message;
                }
                else
                {
                    lblError.Text = "Vorgang erfolgreich storniert";

                    ResetNachbearbeitung(true);

                    if (objNachbearbeitung.tblBarquittungen.Rows.Count > 0)
                    {
                        if (!objNachbearbeitung.tblBarquittungen.Columns.Contains("Filename"))
                        {
                            objNachbearbeitung.tblBarquittungen.Columns.Add("Filename", typeof(String));
                            objNachbearbeitung.tblBarquittungen.Columns.Add("Path", typeof(String));

                            foreach (DataRow BarRow in objNachbearbeitung.tblBarquittungen.Rows)
                            {
                                BarRow["Filename"] = BarRow["BARQ_NR"].ToString() + ".pdf";
                                BarRow["Path"] = ZLDCommon.GetDocRootPath(m_User.IsTestUser) + "barquittung\\" + BarRow["BARQ_NR"].ToString() + ".pdf";
                            }
                        }
                        GridView2.DataSource = objNachbearbeitung.tblBarquittungen;
                        GridView2.DataBind();
                        MPEBarquittungen.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Fehler: " + ex.Message;
            }
        }

        /// <summary>
        /// Grid mit offenen Stornos (stornierte und noch nicht nachbearbeitete Vorgänge) füllen
        /// </summary>
        private void FillGridOffeneStornos()
        {
            if ((objNachbearbeitung.tblOffeneStornos != null) && (objNachbearbeitung.tblOffeneStornos.Rows.Count > 0))
            {
                rgOffeneStornos.Visible = true;
                rgOffeneStornos.Rebind();
                // Setzen der DataSource geschieht durch das NeedDataSource-Event
            }
            else
            {
                rgOffeneStornos.Visible = false;
            }
        }

        private void ResetNachbearbeitung(bool resetSuchparameter)
        {
            objNachbearbeitung.ResetData(resetSuchparameter);

            Session["objNachbearbeitung"] = objNachbearbeitung;

            if (resetSuchparameter)
            {
                txtSucheId.Text = "";
                txtSucheAuftragsnummer.Text = "";
            }

            ddlStornogrund.SelectedValue = "0";
            trStornoKundennummer.Visible = false;
            trStornoBegruendung.Visible = false;
            trStornoStva.Visible = false;
            trStornoKennzeichen.Visible = false;
            cmdStorno.Text = "» Stornieren ";

            ddlStornoKunde.SelectedIndex = 0;
            txtStornoKundennummer.Text = ddlStornoKunde.SelectedValue;
            txtStornoBegruendung.Text = "";
            txtStornoAmt.Text = "";
            txtStornoKennz1.Text = "";
            txtStornoKennz2.Text = "";
            txtStornoZulassungsdatum.Text = "";

            VorgangInfo.Visible = false;
            StornoDetails.Visible = false;
            EditPreise.Visible = false;
            cmdZurSuche.Visible = false;
            cmdAbbrechen.Visible = false;
            cmdStorno.Visible = false;
            cmdAbsenden.Visible = false;

            Panel1.Visible = true;
            cmdCreate.Visible = true;
            cmdOffeneStornos.Visible = true;
        }

        /// <summary>
        /// Validierung Datum
        /// </summary>
        private bool checkDate()
        {
            String ZDat = ZLDCommon.toShortDateStr(txtStornoZulassungsdatum.Text);

            if (String.IsNullOrEmpty(ZDat))
            {
                lblError.Text = "Bitte geben Sie ein Zulassungsdatum an!";
                return false;
            }

            if (!ZDat.IsDate())
            {
                lblError.Text = "Ungültiges Zulassungsdatum: Falsches Format.";
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
                return false;
            }

            tagesdatum = DateTime.Today;
            tagesdatum = tagesdatum.AddYears(1);
            if (DateNew > tagesdatum)
            {
                lblError.Text = "Das Datum darf max. 1 Jahr in der Zukunft liegen!";
                return false;
            }

            if (ihDatumIstWerktag.Value == "false")
            {
                lblError.Text = "Bitte wählen Sie einen Werktag für das Zulassungsdatum aus!";
                return false;
            }

            return true;
        }

        #endregion
    }
}
