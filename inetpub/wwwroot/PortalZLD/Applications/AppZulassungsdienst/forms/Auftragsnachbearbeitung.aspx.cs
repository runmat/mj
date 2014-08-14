using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using Telerik.Web.UI;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Auftragsnachbearbeitung
    /// </summary>
    public partial class Auftragsnachbearbeitung : Page
    {
        private User m_User;
        private App m_App;
        private NachbearbeitungAuftrag objNachbearbeitung;
        private ZLDCommon objCommon;

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);

            m_App = new App(m_User); //erzeugt ein App_objekt 
            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            lblError.Text = "";

            if (Session["objCommon"] == null)
            {
                objCommon = new ZLDCommon(ref m_User, m_App);
                objCommon.VKBUR = m_User.Reference.Substring(4, 4);
                objCommon.VKORG = m_User.Reference.Substring(0, 4);
                objCommon.getSAPDatenStamm(Session["AppID"].ToString(), Session.SessionID, this);
                objCommon.getSAPZulStellen(Session["AppID"].ToString(), Session.SessionID, this);
                objCommon.LadeKennzeichenGroesse();
                Session["objCommon"] = objCommon;
            }
            else
            {
                objCommon = (ZLDCommon)Session["objCommon"];
            }

            if (!IsPostBack)
            {
                objNachbearbeitung = new NachbearbeitungAuftrag(ref m_User, m_App);
                objNachbearbeitung.StornogruendeLaden(Session["AppID"].ToString(), Session.SessionID, this);
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
            if (objNachbearbeitung.tblPositionsdaten != null)
            {
                rgPositionenDisplay.DataSource = objNachbearbeitung.tblPositionsdaten.DefaultView;
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
                }
                else
                {
                    trStornoKundennummer.Visible = false;
                    trStornoBegruendung.Visible = false;
                    trStornoStva.Visible = false;
                    trStornoKennzeichen.Visible = false;
                }
            }
        }

        protected void ddlStornoKunde_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtStornoKundennummer.Text = ddlStornoKunde.SelectedValue;
        }

        protected void cmdAbbrechen_Click(object sender, EventArgs e)
        {
            objNachbearbeitung.StornoKundennummer = "";
            objNachbearbeitung.StornoBegruendung = "";
            objNachbearbeitung.StornoStva = "";
            objNachbearbeitung.StornoKennzeichen = "";

            Session["objNachbearbeitung"] = objNachbearbeitung;

            VorgangInfo.Visible = false;
            StornoDetails.Visible = false;
            cmdAbbrechen.Visible = false;
            cmdStorno.Visible = false;

            Panel1.Visible = true;
            cmdCreate.Visible = true;
            cmdOffeneStornos.Visible = true;
        }

        protected void cmdStorno_Click(object sender, EventArgs e)
        {
            Storno();
        }

        protected void rgPositionenEdit_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (objNachbearbeitung.tblPositionsdaten != null)
            {
                rgPositionenEdit.DataSource = objNachbearbeitung.tblPositionsdaten.DefaultView;
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

            objNachbearbeitung.OffeneStornosLaden(Session["AppID"].ToString(), Session.SessionID, this, objCommon.tblKundenStamm);

            FillGridOffeneStornos();
        }

        protected void rgOffeneStornos_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (objNachbearbeitung.tblOffeneStornos != null)
            {
                rgOffeneStornos.DataSource = objNachbearbeitung.tblOffeneStornos.DefaultView;
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
                    objNachbearbeitung.VorgangId = gridRow["ZULBELN"].Text;
                    objNachbearbeitung.VorgangLaden(Session["AppID"].ToString(), Session.SessionID, this);

                    Session["objNachbearbeitung"] = objNachbearbeitung;

                    if (objNachbearbeitung.Status != 0)
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

        #endregion

        #region Funktionen

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

            DataView tmpDView = new DataView(objCommon.tblKundenStamm);
            tmpDView.RowFilter = "XCPDK <> 'X'";
            tmpDView.Sort = "NAME1";
            ddlStornoKunde.DataSource = tmpDView;
            ddlStornoKunde.DataValueField = "KUNNR";
            ddlStornoKunde.DataTextField = "NAME1";
            ddlStornoKunde.DataBind();
            ddlStornoKunde.SelectedIndex = 0;
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

            objNachbearbeitung.VorgangPruefen(Session["AppID"].ToString(), Session.SessionID, this);

            if (objNachbearbeitung.Status != 0)
            {
                lblError.Text = objNachbearbeitung.Message;
            }
            else
            {
                objNachbearbeitung.VorgangLaden(Session["AppID"].ToString(), Session.SessionID, this);
            }

            Session["objNachbearbeitung"] = objNachbearbeitung;

            if (objNachbearbeitung.Status != 0)
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
            var kopfdaten = objNachbearbeitung.tblKopfdaten.Rows[0];

            lblIDDisplay.Text = kopfdaten["ZULBELN"].ToString();

            lblAuftragsnummerDisplay.Text = kopfdaten["VBELN"].ToString();

            lblKundennummerDisplay.Text = kopfdaten["KUNNR"].ToString().TrimStart('0');

            DataRow[] drow = objCommon.tblKundenStamm.Select("KUNNR = '" + kopfdaten["KUNNR"].ToString().TrimStart('0') + "'");
            if (drow.Length > 0)
            {
                lblKundeDisplay.Text = drow[0]["NAME1"].ToString();
            }
            else
            {
                lblKundeDisplay.Text = "";
            }

            lblReferenz1Display.Text = kopfdaten["ZZREFNR1"].ToString();

            DateTime tmpDatum;
            if (DateTime.TryParse(kopfdaten["ZZZLDAT"].ToString(), out tmpDatum))
            {
                lblZulassungsdatumDisplay.Text = tmpDatum.ToShortDateString();
            }
            else
            {
                lblZulassungsdatumDisplay.Text = "";
            }

            lblKennzeichenDisplay.Text = kopfdaten["ZZKENN"].ToString();

            lblStatusDisplay.Text = kopfdaten["KSTATUS"].ToString();

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
            if ((objNachbearbeitung.tblPositionsdaten != null) && (objNachbearbeitung.tblPositionsdaten.Rows.Count > 0))
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

                objNachbearbeitung.VorgangStornieren(Session["AppID"].ToString(), Session.SessionID, this);

                Session["objNachbearbeitung"] = objNachbearbeitung;

                if (objNachbearbeitung.Status != 0)
                {
                    lblError.Text = "Fehler beim Stornieren: " + objNachbearbeitung.Message;
                }
                else
                {
                    lblError.Text = "Der Vorgang wurde erfolgreich storniert!";

                    VorgangInfo.Visible = false;
                    StornoDetails.Visible = false;
                    cmdAbbrechen.Visible = false;
                    cmdStorno.Visible = false;

                    if (!String.IsNullOrEmpty(objNachbearbeitung.VorgangId))
                    {
                        var grundRows = objNachbearbeitung.tblStornogruende.Select("STORNOGRUND = '" + objNachbearbeitung.Stornogrund + "'");

                        // Ggf. noch neuen Vorgang laden/anzeigen und zur Preisbearbeitung wechseln
                        if (grundRows.Length > 0 && grundRows[0]["PREISE_CHG"].ToString() == "X")
                        {
                            objNachbearbeitung.VorgangLaden(Session["AppID"].ToString(), Session.SessionID, this);

                            Session["objNachbearbeitung"] = objNachbearbeitung;

                            if (objNachbearbeitung.Status != 0)
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
            if ((objNachbearbeitung.tblPositionsdaten != null) && (objNachbearbeitung.tblPositionsdaten.Rows.Count > 0))
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
                    var pos = item["ZULPOSNR"].Text;

                    var txtPreis = (TextBox)item.FindControl("txtPreis");
                    var txtGebuehr = (TextBox)item.FindControl("txtGebuehr");
                    var txtGebuehrAmt = (TextBox)item.FindControl("txtGebuehrAmt");
                    var txtSteuer = (TextBox)item.FindControl("txtSteuer");
                    var txtPreisKennz = (TextBox)item.FindControl("txtPreisKennz");

                    var posRows = objNachbearbeitung.tblPositionsdaten.Select("ZULPOSNR = '" + pos + "'");
                    if (posRows.Length > 0)
                    {
                        var posRow = posRows[0];

                        double preisAlt;
                        double preisNeu;
                        double gebAmtAlt;
                        double gebAmtNeu;

                        Double.TryParse(posRow["PREIS_C"].ToString(), out preisAlt);
                        Double.TryParse(posRow["GEB_AMT_C"].ToString(), out gebAmtAlt);

                        var mat = posRow["WEBMTART"].ToString();
                        switch (mat)
                        {
                            case "D":
                                if (Double.TryParse(txtPreis.Text, out preisNeu))
                                {
                                    if (preisAlt > preisNeu)
                                        blnPreisminderung = true;
                                }
                                break;

                            case "G":
                                if (Double.TryParse(txtGebuehr.Text, out preisNeu))
                                {
                                    if (preisAlt > preisNeu)
                                        blnPreisminderung = true;
                                }
                                break;

                            case "S":
                                if (Double.TryParse(txtSteuer.Text, out preisNeu))
                                {
                                    if (preisAlt > preisNeu)
                                        blnPreisminderung = true;
                                }
                                break;

                            case "K":
                                if (Double.TryParse(txtPreisKennz.Text, out preisNeu))
                                {
                                    if (preisAlt > preisNeu)
                                        blnPreisminderung = true;
                                }
                                break;
                        }

                        if (Double.TryParse(txtGebuehrAmt.Text, out gebAmtNeu))
                        {
                            if (gebAmtAlt > gebAmtNeu)
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
                        var pos = item["ZULPOSNR"].Text;

                        var txtPreis = (TextBox)item.FindControl("txtPreis");
                        var txtGebuehr = (TextBox)item.FindControl("txtGebuehr");
                        var txtGebuehrAmt = (TextBox)item.FindControl("txtGebuehrAmt");
                        var txtSteuer = (TextBox)item.FindControl("txtSteuer");
                        var txtPreisKennz = (TextBox)item.FindControl("txtPreisKennz");

                        var posRows = objNachbearbeitung.tblPositionsdaten.Select("ZULPOSNR = '" + pos + "'");
                        if (posRows.Length > 0)
                        {
                            var posRow = posRows[0];

                            double preisNeu;
                            double gebAmtNeu;

                            var mat = posRow["WEBMTART"].ToString();
                            switch (mat)
                            {
                                case "D":
                                    if (Double.TryParse(txtPreis.Text, out preisNeu))
                                    {
                                        posRow["PREIS_C"] = preisNeu;
                                    }
                                    break;

                                case "G":
                                    if (Double.TryParse(txtGebuehr.Text, out preisNeu))
                                    {
                                        posRow["PREIS_C"] = preisNeu;
                                    }
                                    break;

                                case "S":
                                    if (Double.TryParse(txtSteuer.Text, out preisNeu))
                                    {
                                        posRow["PREIS_C"] = preisNeu;
                                    }
                                    break;

                                case "K":
                                    if (Double.TryParse(txtPreisKennz.Text, out preisNeu))
                                    {
                                        posRow["PREIS_C"] = preisNeu;
                                    }
                                    break;
                            }

                            if (Double.TryParse(txtGebuehrAmt.Text, out gebAmtNeu))
                            {
                                posRow["GEB_AMT_C"] = gebAmtNeu;
                            }
                        }
                    }
                }

                objNachbearbeitung.tblPositionsdaten.AcceptChanges();

                objNachbearbeitung.VorgangAbsenden(Session["AppID"].ToString(), Session.SessionID, this);

                Session["objNachbearbeitung"] = objNachbearbeitung;

                if (objNachbearbeitung.Status != 0)
                {
                    lblError.Text = objNachbearbeitung.Message;
                }
                else
                {
                    lblError.Text = "Der Vorgang wurde erfolgreich in SAP gespeichert!";

                    VorgangInfo.Visible = false;
                    EditPreise.Visible = false;
                    cmdAbsenden.Visible = false;
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

        #endregion
    }
}
