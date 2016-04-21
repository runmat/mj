using System;
using System.Data;
using AppZulassungsdienst.lib;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace AppZulassungsdienst.forms
{
    public partial class NachGekaufteKennzeichen : System.Web.UI.Page
    {
        private User m_User;
        private NacherfassungGekaufteKennzeichen NGK;

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            Common.GetAppIDFromQueryString(this);
            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            lblError.Text = "";
            lblMessage.Text = "";
            lblErrorInfotext.Text = "";

            if (Session["NGK"] == null)
            {
                NGK = new NacherfassungGekaufteKennzeichen(m_User.Reference, m_User.CustomerName.Contains("ZLD"));
            }
            else
            {
                NGK = (NacherfassungGekaufteKennzeichen)Session["NGK"];
            }

            if (!IsPostBack)
            {
                FillLieferanten();
                FillArtikel();
                RefreshBuchungen();
                CheckPreisShow();
            }
        }

        protected void ddlLiefer_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillArtikel();
            RefreshBuchungen();
        }

        protected void cmdCreate_Click(object sender, EventArgs e)
        {
            NGK.SendToSap(m_User.UserName);

            if (NGK.ErrorOccured)
            {
                lblError.Text = "Beim Speichern ist ein Fehler aufgetreten.";
            }
            else
            {
                lblMessage.Text = "Buchung erfolgreich";
            }

            txtLieferscheinnummer.Text = "";
            FillGrid();
        }

        protected void lbNewLine_Click(object sender, EventArgs e)
        {
            DateTime Date;
            int iMenge;
            double dPreis = 0;

            if(String.IsNullOrEmpty(txtDatum.Text) || !DateTime.TryParse(txtDatum.Text,out Date))
            {
                lblError.Text = "Geben Sie ein gültiges Datum an!";
                return;
            }
            // Jahr automatisch auf das aktuelle setzen, wenn zu klein
            if (Date < DateTime.Now.AddYears(-1))
            {
                txtDatum.Text = (new DateTime(DateTime.Today.Year, Date.Month, Date.Day)).ToString("dd.MM.yyyy");
            }

            if(ddlLiefer.SelectedIndex == -1 || ddlLiefer.SelectedValue == null)
            {
                lblError.Text = "Wählen Sie einen gültigen Lieferanten aus!";
                return;
            }

            if(String.IsNullOrEmpty(txtLieferscheinnummer.Text))
            {
                lblError.Text = "Geben Sie eine gültige Lieferscheinnummer an!";
                return;
            }
            
            if (String.IsNullOrEmpty(txtMenge.Text) || !int.TryParse(txtMenge.Text,out iMenge))
            {
                lblError.Text = "Geben Sie eine gültige Stückzahl an!";
                return;
            }
            
            if (iMenge == 0) 
            {
                lblError.Text = "Geben Sie eine gültige Stückzahl an!";
                return;
            }
            
            if (ddlArtikel.SelectedIndex == -1 || ddlArtikel.SelectedValue == null)
            {
                lblError.Text = "Wählen Sie einen gültigen Artikel aus!";
                return;
            }
            
            if (txtPreis.Visible)
            {
                if (String.IsNullOrEmpty(txtPreis.Text))
                {
                    lblError.Text = "Geben Sie einen gültigen Preis an!";
                    return;
                }

                if (!double.TryParse(txtPreis.Text.Trim(), out dPreis))
                {
                    lblError.Text = "Geben Sie einen gültigen Preis an!";
                    return;
                }

                if (dPreis == 0)
                {
                    lblError.Text = "Der Preis darf nicht 0.00€ sein!";
                    return;
                }
            }
            
            DataRow[] rows = NGK.tblKennzeichen.Select("ArtikelID='" + ddlArtikel.SelectedValue + "' AND Datum='"+txtDatum.Text+
                                                        "' AND Lieferscheinnummer='" + txtLieferscheinnummer.Text + "' AND LieferantID='"+ddlLiefer.SelectedValue+"'");
            DataRow[] rows2 = NGK.tblArtikel.Select("ARTLIF='" + ddlArtikel.SelectedValue + "'");
            if (rows.GetLength(0) == 0 && rows2.GetLength(0) > 0)
            {
                DataRow row = rows2[0];

                if (txtPreis.Visible)
                {
                    if (NGK.CheckLangtextNeeded(ddlArtikel.SelectedValue))
                    {
                        OpenInfotext(ddlArtikel.SelectedValue, txtDatum.Text, txtMenge.Text, "", "", row["ARTBEZ"].ToString(), ddlLiefer.SelectedValue,txtLieferscheinnummer.Text, true, txtPreis.Text);
                    }
                    else 
                    {
                        NGK.AddKennzeichen(txtDatum.Text, ddlArtikel.SelectedValue, row["ARTBEZ"].ToString(), iMenge, ddlLiefer.SelectedValue, txtLieferscheinnummer.Text, dPreis);
                    }
                }
                else
                {
                    if (NGK.CheckLangtextNeeded(ddlArtikel.SelectedValue))
                    {
                        OpenInfotext(ddlArtikel.SelectedValue, txtDatum.Text, txtMenge.Text, "", "", row["ARTBEZ"].ToString(), ddlLiefer.SelectedValue, txtLieferscheinnummer.Text, true, "");
                    }
                    else
                    {
                        NGK.AddKennzeichen(txtDatum.Text, ddlArtikel.SelectedValue, row["ARTBEZ"].ToString(), iMenge, ddlLiefer.SelectedValue, txtLieferscheinnummer.Text);
                    }
                }
            }
            else
            {
                lblError.Text = "Der Artikel ist in der aktuellen Bestellung schon enthalten! Bitte ändern Sie die Menge über die Schaltflächen in der Liste.";
                return;
            }

            ClearInput();
            FillGrid();
        }

        protected void gvArtikel_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillGrid();
        }

        protected void gvArtikel_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "entfernen":
                    NGK.tblKennzeichen.Select("ArtikelID='" + e.CommandArgument + "'")[0].Delete();
                    if (NGK.tblKennzeichen.Rows.Count == 0)
                    {
                        NGK.tblKennzeichen.Rows.Clear();                        
                    }
                    FillGrid();
                    break;

                case "bearbeiten":
                    DataRow TRow = NGK.tblKennzeichen.Select("ArtikelID='" + e.CommandArgument + "'")[0];
                   
                    string strDatum = "";
                    string strMenge = "";
                    string strArtikel = "";
                    string strLieferantID = "";
                    string strLieferscheinnummer = "";
                    string strPreis = "";
                    string strLText = "";
                    string strLTextNr = "";
                    bool bPflichtText = false;

                    if (TRow["Datum"] != null)
                    {
                        strDatum = TRow["Datum"].ToString();                                               
                    }
                    if (TRow["Menge"] != null)
                    {
                        strMenge = TRow["Menge"].ToString();
                    }
                    if (TRow["Artikel"] != null)
                    {
                        strArtikel = TRow["Artikel"].ToString();
                    }
                    if (TRow["LieferantID"] != null)
                    {
                        strLieferantID = TRow["LieferantID"].ToString();
                    }
                    if(TRow["Lieferscheinnummer"] != null)
                    {
                        strLieferscheinnummer = TRow["Lieferscheinnummer"].ToString();
                    }
                    if (TRow["Preis"] != null)
                    {
                        strPreis = TRow["Preis"].ToString();
                    }                   
                    if (TRow["Langtext"] != null)
                    {
                        strLText = TRow["Langtext"].ToString();
                    }
                    if (TRow["LangtextID"] != null)
                    {
                        strLTextNr = TRow["LangtextID"].ToString();
                    }                    
                    if (NGK.CheckLangtextNeeded(TRow["ArtikelID"].ToString()))
                    {
                        bPflichtText = true;
                    }

                    Session["LastInfoText"] = TRow;

                    OpenInfotext(e.CommandArgument.ToString(),strDatum, strMenge, strLText, strLTextNr, strArtikel, strLieferantID,strLieferscheinnummer ,bPflichtText, strPreis);
                    
                    break;

                case "minusMenge":
                    DataRow[] rows = NGK.tblKennzeichen.Select("ArtikelID=" + e.CommandArgument);
                    if (rows.GetLength(0) > 0)
                    {
                        int iMenge;
                        int.TryParse(rows[0]["Menge"].ToString(), out iMenge);
                        if (iMenge > 0)
                        {
                            rows[0]["Menge"] = iMenge - 1;
                        }
                        FillGrid();
                    }
                    break;

                case "plusMenge":
                    DataRow[] rows2 = NGK.tblKennzeichen.Select("ArtikelID=" + e.CommandArgument);
                    if (rows2.GetLength(0) > 0)
                    {
                        int iMenge;
                        int.TryParse(rows2[0]["Menge"].ToString(), out iMenge);
                        if (iMenge > 0)
                        {
                            rows2[0]["Menge"] = iMenge + 1;
                        }
                        FillGrid();
                    }
                    break;
            }
        }

        protected void lbInfotextSave_Click(object sender, EventArgs e)
        {
            lblErrorInfotext.Text = "";
            if (lblPflicht.Text == "true")
            {
                if (txtInfotext.Text.TrimStart(',') == "")
                {
                    lblErrorInfotext.Text = "Geben Sie einen Text ein!";
                    MPEInfotext.Show();
                    return;
                }
            }
            double dPreis;     
            int iMenge;
            if (!double.TryParse(lblPreisBox.Text, out dPreis))
            {
                dPreis = 0;
            }
            if (!int.TryParse(lblMenge.Text, out iMenge))
            {
                iMenge = 1;
            }

            if (Session["LastInfoText"] == null)
            {
                Session["LastInfoText"] = NGK.AddKennzeichen(lblDat.Text, lblMatNr.Text, lblArtikelbezeichnung.Text, iMenge, lblLieferantenID.Text,
                                            lblLieferscheinnummer.Text, dPreis, txtInfotext.Text.TrimStart(','), lblLTextNr.Text);
            }
            else 
            {
                DataRow row = (DataRow)Session["LastInfoText"];
                row["Langtext"] = txtInfotext.Text.TrimStart(',');                
            }

            CloseInfotext();
        }

        protected void lbInfotextClose_Click(object sender, EventArgs e)
        {
            CloseInfotext();
        }
        
        protected void ddlArtikel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckPreisShow();
        }

        protected void gvArtikel_OnRowDataBound(object sender, GridViewRowEventArgs e)
        { 

        }

        protected void lbToggleBuchungen_Click(object sender, EventArgs e)
        {
            ShowBuchungen(!ibHideBuchungen.Visible);
        }

        protected void rgGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!e.IsFromDetailTable)
            {
                if (NGK.tblErfassteKennzeichenKopf != null)
                    rgGrid1.DataSource = NGK.tblErfassteKennzeichenKopf.DefaultView;
                else
                    rgGrid1.DataSource = null;
            }
        }

        protected void rgGrid1_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
        {
            var bestellNr = e.DetailTableView.ParentItem.GetDataKeyValue("BSTNR").ToString();

            if (!string.IsNullOrEmpty(bestellNr) && NGK.tblErfassteKennzeichenPositionen != null)
            {
                var dv = new DataView(NGK.tblErfassteKennzeichenPositionen);
                dv.RowFilter = "BSTNR = '" + bestellNr + "'";
                e.DetailTableView.DataSource = dv;
            }
        }

        #endregion

        #region Methods

        private void ClearInput()
        {
            txtPreis.Text = "";
            txtMenge.Text = "";
        }

        private void OpenInfotext(string ArtikelID,
                                    string Datum,
                                    string Menge,
                                    string Text,
                                    string TextNr,
                                    string MatText,
                                    string LieferantenID,
                                    string Lieferscheinnummer,
                                    bool Pflicht,
                                    string Preis)
        {
            lblMatNr.Text = ArtikelID;
            lblDat.Text = Datum;
            lblMenge.Text = Menge;
            txtInfotext.Text = Text;
            lblLTextNr.Text = TextNr;
            lblArtikelbezeichnung.Text = MatText;
            lblLieferantenID.Text = LieferantenID;
            lblLieferscheinnummer.Text = Lieferscheinnummer;
            lblPreisBox.Text = Preis;
            
            if (Pflicht)
            {
                lblPflicht.Text = "true";
            }
            else
            {
                lblPflicht.Text = "false";
            }

            MPEInfotext.Show();
        }

        private void CloseInfotext()
        {
            txtInfotext.Text = "";
            lblLTextNr.Text = "";
            lblMatNr.Text = "";
            lblPflicht.Text = "";
            lblArtikelbezeichnung.Text = "";
            lblLieferantenID.Text = "";
            lblPreisBox.Text = "";
            lblDat.Text = "";
            lblMenge.Text = "";

            Session["LastInfoText"] = null;

            MPEInfotext.Hide();
            FillGrid();
        }

        private void FillGrid()
        {
            DataView tmpDataView = new DataView(NGK.tblKennzeichen);
            String strFilter = "";

            tmpDataView.RowFilter = strFilter;

            if (tmpDataView.Count == 0)
            {
                // Lieferantenliste freigeben
                ddlLiefer.Enabled = true;
                // Speichern Button ausblenden
                cmdCreate.Visible = false;

                gvArtikel.Visible = false;
                lblNoData.Visible = true;
            }
            else
            {
                // Lieferantenliste sperren
                ddlLiefer.Enabled = false;
                // Speichern Button einblenden
                cmdCreate.Visible = true;

                gvArtikel.Visible = true;
                lblNoData.Visible = false;

                gvArtikel.PageIndex = 0;
                gvArtikel.DataSource = tmpDataView;
                gvArtikel.DataBind();
            }
        }

        private void FillArtikel()
        {
            if (ddlLiefer.SelectedIndex >= 0 && ddlLiefer.SelectedValue != null)
            {
                NGK.GetArtikel(ddlLiefer.SelectedValue);
                Session["NGK"] = NGK;

                if (NGK.tblArtikel.Rows.Count == 0)
                {
                    lblError.Text = "Für den gewählten Lieferant konnten keine Artikel ermittelt werden!";
                    ddlArtikel.DataSource = null;
                    ddlArtikel.Items.Clear();
                    ddlArtikel.Items.Add("-- Kein Artikel --");

                    //hinzufügen unterbinden
                    lbNewLine.Enabled = false;
                }
                else
                {
                    DataView dv = new DataView(NGK.tblArtikel);
                    dv.Sort = "Pos asc";
                    ddlArtikel.DataSource = dv;
                    ddlArtikel.DataValueField = "ARTLIF";
                    ddlArtikel.DataTextField = "ARTBEZ";

                    //hinzufügen ermöglichen
                    lbNewLine.Enabled = true;
                }
                ddlArtikel.DataBind();
                ddlArtikel.SelectedIndex = 0;
                CheckPreisShow();
            }
            else { lblError.Text = "Wählen Sie einen gültigen Lieferanten aus!"; }
        }

        private void FillLieferanten()
        {
            if (NGK.tblLieferanten.Rows.Count == 0)
            {
                lblError.Text = "Für Ihre Kostenstelle konnten keine Lieferanten ermittelt werden!";
                ddlLiefer.DataSource = null;
                ddlLiefer.Items.Clear();
                ddlLiefer.Items.Add("-- Kein Lieferant --");
            }
            else 
            {
                DataView dv = new DataView(NGK.tblLieferanten);
                dv.Sort = "NAME1 asc";
                ddlLiefer.DataSource = dv;
                ddlLiefer.DataTextField = "NAME1";
                ddlLiefer.DataValueField= "LIFNR";                
            }
            ddlLiefer.DataBind();
            ddlLiefer.SelectedIndex = 0;
        }

        private void CheckPreisShow()
        {
            bool show = NGK.CheckpreisNeeded(ddlArtikel.SelectedValue);
            txtPreis.Visible = show;
            lblPreis.Visible = show;
        }

        private void RefreshBuchungen()
        {
            ShowBuchungen(ibHideBuchungen.Visible);
        }

        private void ShowBuchungen(bool show)
        {
            lbShowBuchungen.Text = (!show ? "erfasste Käufe..." : "erfasste Käufe schließen!");

            ibHideBuchungen.Visible = show;
            divKaeufe.Visible = show;

            if (show)
            {
                FillKaeufe();
                FillGridKaeufe();
            }
        }

        private void FillGridKaeufe()
        {
            if (NGK.tblErfassteKennzeichenKopf != null && NGK.tblErfassteKennzeichenKopf.Rows.Count > 0)
            {
                rgGrid1.Visible = true;
                rgGrid1.Rebind();
                // Setzen der DataSource geschieht durch das NeedDataSource-Event
                lblNoData.Visible = false;
            }
            else
            {
                rgGrid1.Visible = false;
                lblNoData.Text = "Keine Daten gefunden";
                lblNoData.Visible = true;
            }
        }

        private void FillKaeufe()
        {
            if (ddlLiefer.SelectedIndex >= 0 && ddlLiefer.SelectedValue != null)
            {
                NGK.GetKaeufe(ddlLiefer.SelectedValue);
                Session["NGK"] = NGK;

                lblGewaehlterLieferant.Text = ddlLiefer.SelectedItem.Text;
                lblAnzahlKaeufe.Text = NGK.tblErfassteKennzeichenKopf.Rows.Count.ToString();
            }
            else { lblError.Text = "Wählen Sie einen gültigen Lieferanten aus!"; }
        }

        #endregion
    }
}