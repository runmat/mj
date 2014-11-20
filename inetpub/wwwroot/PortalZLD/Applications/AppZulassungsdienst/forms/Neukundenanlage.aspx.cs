using System;
using System.Web.UI.WebControls;
using AppZulassungsdienst.lib;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using System.Data;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Neukundenstammdaten. Anlegen eines neuen Kunden.
    /// </summary>
    public partial class Neukundenanlage : System.Web.UI.Page
    {
        private User m_User;
        private App m_App;
        private Neukunde objNeukunde;
        private ZLDCommon objCommon;

        /// <summary>
        /// Page_Load Ereignis. Prüfen ob die Anwendung dem Benutzer zugeordnet ist. Evtl. Stammdaten laden.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);
            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            lblError.Text = "";

            if (m_User.Reference.Trim(' ').Length == 0)
            {
                lblError.Text = "Es wurde keine Benutzerreferenz angegeben! Somit können keine Stammdaten ermittelt werden!";
                return;
            }
            if (Session["objCommon"] == null)
            {
                objCommon = new ZLDCommon(ref m_User, m_App);
                objCommon.VKBUR = m_User.Reference.Substring(4, 4);
                objCommon.VKORG = m_User.Reference.Substring(0, 4);
                objCommon.getSAPDatenStamm(Session["AppID"].ToString(), Session.SessionID, this);
                objCommon.getSAPZulStellen(Session["AppID"].ToString(), Session.SessionID, this);
                objCommon.LadeKennzeichenGroesse();
                objCommon.GetGruppen_Touren(Session["AppID"].ToString(), Session.SessionID, this, "T");
                Session["objCommon"] = objCommon;
            }
            else
            {
                objCommon = (ZLDCommon)Session["objCommon"];
                if (objCommon.tblGruppeTouren == null) 
                {
                    objCommon.GetGruppen_Touren(Session["AppID"].ToString(), Session.SessionID, this, "T");
                    if (objCommon.Message.Length > 0)
                    {
                        lblError.Text = objCommon.Message;
                        return;
                    }
                }
            }
            if (IsPostBack)
            {
                objNeukunde = (Neukunde)Session["objNeukunde"];
            }
            else
            {
                objNeukunde = new Neukunde(ref m_User, m_App, Session["AppID"].ToString(), Session.SessionID, "");
                objNeukunde.VKBUR = m_User.Reference.Substring(4, 4);
                objNeukunde.VKORG = m_User.Reference.Substring(0, 4);
                objNeukunde.Fill(Session["AppID"].ToString(), Session.SessionID, this);
                fillDropDowns();
                Session["objNeukunde"] = objNeukunde;
            }
        }

        /// <summary>
        /// Füllen der Auswahldropdowns. Branche, Funktion des Ansprechpartners und Touren.
        /// </summary>
        private void fillDropDowns()
        { 
            ListItem tmpItem;
            Int32 i  = 0;
            ddlBranche.Items.Clear();
            do 
            {
                tmpItem = new ListItem(objNeukunde.tblBranchen.Rows[i]["BRTXT"].ToString(), objNeukunde.tblBranchen.Rows[i]["BRSCH"].ToString());
                ddlBranche.Items.Add(tmpItem);
                i += 1; 
            } while (i < objNeukunde.tblBranchen.Rows.Count);

            ddLand.Items.Clear();
            DataView tmpDataview  = objNeukunde.tblLaender.DefaultView;
            tmpDataview.Sort = "LANDX";
            i = 0;
            do
            {
                tmpItem = new ListItem(tmpDataview[i]["LANDX"].ToString(), tmpDataview[i]["LAND1"].ToString());
                if (tmpItem.Value.ToString() == "DE")
                {
                    tmpItem.Selected = true;
                }
                ddLand.Items.Add(tmpItem);
                i += 1;
            } while (i < tmpDataview.Count);

            ddlFunktion.Items.Clear();
            i = 0;
            tmpItem = new ListItem("- Bitte wählen -", "00");
            ddlFunktion.Items.Add(tmpItem);
            do
            {
                tmpItem = new ListItem(objNeukunde.tblFunktion.Rows[i]["VTEXT"].ToString(), objNeukunde.tblFunktion.Rows[i]["PAFKT"].ToString());
                ddlFunktion.Items.Add(tmpItem);
                i += 1;
            } while (i < objNeukunde.tblFunktion.Rows.Count);

            ddlTour.DataSource = objCommon.tblTourenforSelection;
            ddlTour.DataValueField = "GRUPPE";
            ddlTour.DataTextField = "BEZEI";
            ddlTour.DataBind();
            ddlTour.SelectedValue = "0";

        }

        /// <summary>
        /// Sammeln der Eingabedaten und in den Klasseneigenschaften speichern.
        /// </summary>
        private void GetMaskData()
        {
            objNeukunde.BrancheFreitext = txtBrancheFrei.Text.TrimStart(' ').TrimEnd(' ');
            objNeukunde.Name1 = txtName1.Text.TrimStart(' ').TrimEnd(' ');
            objNeukunde.Name2 = txtName2.Text.TrimStart(' ').TrimEnd(' ');
            objNeukunde.Strasse = txtStrasse.Text.TrimStart(' ').TrimEnd(' ');
            objNeukunde.HausNr = txtHausnr.Text.TrimStart(' ').TrimEnd(' ');
            objNeukunde.Ort = txtOrt.Text.TrimStart(' ').TrimEnd(' ');
            objNeukunde.PLZ = txtPlz.Text.TrimStart(' ').TrimEnd(' ');
            objNeukunde.UIDNummer = txtUIDNummer.Text.TrimStart(' ').TrimEnd(' ');
            objNeukunde.Telefon = txtTelefon.Text.TrimStart(' ').TrimEnd(' ');
            objNeukunde.Mobil = txtMobil.Text.TrimStart(' ').TrimEnd(' ');
            objNeukunde.Mail = txtMail.Text.TrimStart(' ').TrimEnd(' ');
            objNeukunde.Fax = txtFax.Text.TrimStart(' ').TrimEnd(' ');
            objNeukunde.Land = ddLand.SelectedValue;
            if (rbBarkunde.Checked)
            { objNeukunde.Abruftyp = "3"; }
            else { objNeukunde.Abruftyp = "2"; }
            if (rbEinzugJa.Checked)
            { objNeukunde.EinzugEr = "X"; }
            else { objNeukunde.EinzugEr = ""; }

            if (rbFirma.Checked)
            { objNeukunde.Anrede = "0003"; }
            else if (rbHerr.Checked)
            { objNeukunde.Anrede = "0002"; }
            else if (rbFrau.Checked)
            { objNeukunde.Anrede = "0001"; }
            objNeukunde.Branche = ddlBranche.SelectedValue;
            if (ddlFunktion.SelectedValue != "00")
            { objNeukunde.Funktion = ddlFunktion.SelectedValue; }

            objNeukunde.ASPName = txtNameAnPartner.Text.TrimStart(' ').TrimEnd(' ');
            objNeukunde.ASPVorname = txtVornameAnPartner.Text.TrimStart(' ').TrimEnd(' ');
            objNeukunde.Einzug = rbJa.Checked;
            objNeukunde.Auskunft = rbAuskunftJa.Checked;
            objNeukunde.Kreditvers = rbKreditJa.Checked;
            objNeukunde.UmSteuer = rbSteuerJa.Checked;
            objNeukunde.Bemerkung = txtBemerkung.Text;
            objNeukunde.UmStErwartung = txtUmsatz.Text;
            objNeukunde.TourID = ddlTour.SelectedValue;
            if (ddlTour.SelectedValue == "0")
            {
                objNeukunde.TourID = "";
            }

        }

        /// <summary>
        /// Funktion zum Sperren und Entsperren von Controls(nach erfolgreicher Neuanlage).
        /// </summary>
        /// <param name="enabled">Boolean</param>
        private void EnableDisableControls(Boolean enabled)
        { 
            txtBrancheFrei.Enabled = enabled;
            txtName1.Enabled = enabled;
            txtName2.Enabled = enabled;
            txtStrasse.Enabled = enabled;
            txtHausnr.Enabled = enabled;
            txtOrt.Enabled = enabled;
            txtPlz.Enabled = enabled;
            txtUIDNummer.Enabled = enabled;
            ddLand.Enabled = enabled;
            rbBarkunde.Enabled = enabled;
            rbLieferscheinKunde.Enabled = enabled;
            rbEinzugJa.Enabled = enabled;
            rbEinzugNein.Enabled = enabled;
            rbHerr.Enabled = enabled;
            rbFirma.Enabled = enabled;
            rbFrau.Enabled = enabled;
            ddlBranche.Enabled = enabled;
            ddlFunktion.Enabled = enabled;
            txtNameAnPartner.Enabled = enabled;
            txtVornameAnPartner.Enabled = enabled;
            txtTelefon.Enabled = enabled;
            txtFax.Enabled = enabled;
            txtMail.Enabled = enabled;
            txtMobil.Enabled = enabled;
            lbAbsenden.Enabled = enabled;
            rbSteuerJa.Enabled = enabled;
            rbSteuerNein.Enabled = enabled;
            rbKreditJa.Enabled = enabled;
            rbKreditNein.Enabled = enabled;
            rbAuskunftJa.Enabled = enabled;
            rbAuskunftNein.Enabled = enabled;
            rbEinzugJa.Enabled = enabled;
            rbEinzugNein.Enabled = enabled;
            txtUmsatz.Enabled = enabled;
            txtIBAN.Enabled = enabled;
            txtBemerkung.Enabled = enabled;
            ddlTour.Enabled = enabled;
            lb_Neu.Visible = !enabled;
            lblNoData.ForeColor = System.Drawing.Color.Green;
            lblNoData.Text = "Klicken Sie auf \nNeuer Kunde\n um einen weiteren Kunden zu erfassen!";
        }

        /// <summary>
        /// Errorstyles der Controls entfernen.
        /// </summary>
        private void ClearErrors()
        {
            txtBrancheFrei.BorderColor = System.Drawing.ColorTranslator.FromHtml("#dfdfdf");
            txtName1.BorderColor = System.Drawing.ColorTranslator.FromHtml("#dfdfdf");
            txtStrasse.BorderColor = System.Drawing.ColorTranslator.FromHtml("#dfdfdf");
            txtHausnr.BorderColor = System.Drawing.ColorTranslator.FromHtml("#dfdfdf");
            txtOrt.BorderColor = System.Drawing.ColorTranslator.FromHtml("#dfdfdf");
            txtPlz.BorderColor = System.Drawing.ColorTranslator.FromHtml("#dfdfdf");
            txtUIDNummer.BorderColor = System.Drawing.ColorTranslator.FromHtml("#dfdfdf");
            ddLand.BorderColor = System.Drawing.ColorTranslator.FromHtml("#dfdfdf");
            rbBarkunde.BorderColor = System.Drawing.ColorTranslator.FromHtml("#dfdfdf");
            rbLieferscheinKunde.BorderColor = System.Drawing.ColorTranslator.FromHtml("#dfdfdf");
            rbEinzugJa.BorderColor = System.Drawing.ColorTranslator.FromHtml("#dfdfdf");
            rbEinzugNein.BorderColor = System.Drawing.ColorTranslator.FromHtml("#dfdfdf");
            rbHerr.BorderColor = System.Drawing.ColorTranslator.FromHtml("#dfdfdf");
            rbFirma.BorderColor = System.Drawing.ColorTranslator.FromHtml("#dfdfdf");
            rbFrau.BorderColor = System.Drawing.ColorTranslator.FromHtml("#dfdfdf");
            ddlBranche.BorderColor = System.Drawing.ColorTranslator.FromHtml("#dfdfdf");
            txtIBAN.BorderColor = System.Drawing.ColorTranslator.FromHtml("#dfdfdf");
            txtSWIFT.BorderColor = System.Drawing.ColorTranslator.FromHtml("#dfdfdf");
            txtUmsatz.BorderColor = System.Drawing.ColorTranslator.FromHtml("#dfdfdf");
            lblError.Text = "";
        }

        /// <summary>
        /// Validieren der eingebenen Daten.
        /// </summary>
        /// <returns>true bei Fehler</returns>
        private Boolean ValidateInput()
        {
            Boolean bReturn = false;

            if (rbBarkunde.Checked == false && rbLieferscheinKunde.Checked==false)
            {
                rbBarkunde.BorderStyle = BorderStyle.Solid;
                rbBarkunde.BorderWidth = 1;
                rbBarkunde.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                rbLieferscheinKunde.BorderStyle = BorderStyle.Solid;
                rbLieferscheinKunde.BorderWidth = 1;
                rbLieferscheinKunde.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                bReturn = true;
            }
            if (rbLieferscheinKunde.Checked)
            {
                if (rbEinzugJa.Checked == false && rbEinzugNein.Checked == false)
                {
                    rbEinzugJa.BorderStyle = BorderStyle.Solid;
                    rbEinzugJa.BorderWidth = 1;
                    rbEinzugJa.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    rbEinzugNein.BorderStyle = BorderStyle.Solid;
                    rbEinzugNein.BorderWidth = 1;
                    rbEinzugNein.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bReturn = true; 
                }
            }
            if (rbFirma.Checked == false && rbHerr.Checked == false && rbFrau.Checked == false)
            {
                rbFirma.BorderStyle = BorderStyle.Solid;
                rbFirma.BorderWidth = 1;
                rbFirma.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                rbHerr.BorderStyle = BorderStyle.Solid;
                rbHerr.BorderWidth = 1;
                rbHerr.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                rbFrau.BorderStyle = BorderStyle.Solid;
                rbFrau.BorderWidth = 1;
                rbFrau.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                bReturn = true;
            }
            if (ddlBranche.SelectedIndex == -1)
            {
                ddlBranche.BorderStyle = BorderStyle.Solid;
                ddlBranche.BorderWidth = 1;
                ddlBranche.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                bReturn = true;
            }
            else if (ddlBranche.SelectedValue == "0004" && txtBrancheFrei.Text.Length == 0)
            {
                txtBrancheFrei.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                bReturn = true;
            }
            if (txtName1.Text.Trim().Length == 0)
            {
                txtName1.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                bReturn = true;
            }

            if (txtName1.Text.Trim().Length == 0)
            {
                txtName1.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                bReturn = true;
            }

            if (txtStrasse.Text.Trim().Length == 0)
            {
                txtStrasse.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                bReturn = true;
            }

            if (txtHausnr.Text.Trim().Length == 0)
            {
                txtHausnr.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                bReturn = true;
            }
            if (txtOrt.Text.Trim().Length == 0)
            {
                txtOrt.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                bReturn = true;
            }
            if (txtPlz.Text.Trim().Length == 0)
            {
                txtPlz.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                bReturn = true;
            }
            else if (ddLand.SelectedValue == "DE" && txtPlz.Text.Trim().Length != 5)
            {
                txtPlz.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                bReturn = true;
            }
            if (ddLand.SelectedIndex == -1)
            {
                ddLand.BorderStyle = BorderStyle.Solid;
                ddLand.BorderWidth = 1;
                ddLand.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                bReturn = true;
            }
            else if (ddLand.SelectedValue != "DE")
            {
                if (txtUIDNummer.Text.Trim().Length == 0)
                {
                    txtUIDNummer.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bReturn = true; 
                }
            }

            if ((rbJa.Checked) && (txtIBAN.Text.Trim(' ').Length == 0))
            {
                txtIBAN.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                txtSWIFT.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                bReturn = true;                       
            }


            if (txtUmsatz.Text.Trim().Length == 0)
            {
                txtUmsatz.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                bReturn = true;
            }
            return bReturn;
        }

        /// <summary>
        /// Auswahl/Änderung Branche. Bei einigen Branchen muss der Freitext sichtbar sein.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void ddlBranche_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlBranche.SelectedValue == "0004")
            {
                txtBrancheFrei.Visible = true;
                lblBrancheFrei.Visible = true;
            }
            else
            {
                txtBrancheFrei.Visible = false;
                lblBrancheFrei.Visible = false;
            }
        }

        /// <summary>
        /// Auswahl/Änderung Branche. Bei allen Länder ausser Deutschland ist die Ust-ID Nummer Pflicht.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void ddLand_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddLand.SelectedValue != "DE")
            {
                lblStar.Visible = true;
            }
            else
            {
                lblStar.Visible = false;
            }
        }

        /// <summary>
        /// Auswahl Barkunde. Markierung Einzugsermächtigung anpassen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void rbBarkunde_CheckedChanged(object sender, EventArgs e)
        {
            if (rbBarkunde.Checked)
            { 
                rbEinzugJa.Checked = false;
                rbEinzugNein.Checked = false;
            }

        }

        /// <summary>
        /// Auswahl Lieferscheinkunde. Markierung Einzugsermächtigung anpassen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rbLieferscheinKunde_CheckedChanged(object sender, EventArgs e)
        {
            if (rbLieferscheinKunde.Checked)
            {
                rbEinzugJa.Checked = false;
                rbEinzugNein.Checked = true;
            }
        }

        /// <summary>
        /// Neuen Kunden erfassen nach erfolgreichen Anlegen eines Kunden.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lb_Neu_Click(object sender, EventArgs e)
        {
            EnableDisableControls(true);

            objNeukunde.MitarbeiterNr = "";
            objNeukunde.Abruftyp = "";
            objNeukunde.EinzugEr = "";
            objNeukunde.Anrede = "";
            objNeukunde.Branche = "";
            objNeukunde.BrancheFreitext = "";
            objNeukunde.Name1 = "";
            objNeukunde.Name2 = "";
            objNeukunde.Strasse = "";
            objNeukunde.Ort = "";
            objNeukunde.HausNr = "";
            objNeukunde.PLZ = "";
            objNeukunde.Land = "";
            objNeukunde.UIDNummer = "";
            objNeukunde.ASPVorname = "";
            objNeukunde.ASPName = "";
            objNeukunde.Funktion = "";
            objNeukunde.Telefon = "";
            objNeukunde.Mobil = "";
            objNeukunde.HausNr = "";
            objNeukunde.Fax = "";
            objNeukunde.Land = "";

            ddLand.SelectedValue = "DE";
            lblStar.Visible = false;
            ddlBranche.SelectedIndex = 0;
            ddlFunktion.SelectedIndex = 0;
            txtBrancheFrei.Visible = false;
            txtBrancheFrei.Text = "";
            lblBrancheFrei.Visible = false;

            txtName1.Text = "";
            txtName2.Text = "";
            txtStrasse.Text = "";
            txtHausnr.Text = "";
            txtOrt.Text = "";
            txtPlz.Text = "";
            txtUIDNummer.Text = "";
            rbBarkunde.Checked = false;
            rbLieferscheinKunde.Checked = false;
            rbEinzugJa.Checked = false;
            rbEinzugNein.Checked = false;
            rbHerr.Checked = false;
            rbFirma.Checked = false;
            rbFrau.Checked = false;
            txtNameAnPartner.Text = "";
            txtVornameAnPartner.Text = "";
            txtTelefon.Text = "";
            txtFax.Text = "";
            txtMail.Text = "";
            txtMobil.Text = "";
            rbJa.Checked = false;
            rbNein.Checked = true;
            txtBankname.Text = "Wird automatisch gefüllt!";
            txtSWIFT.Text = "";
            txtIBAN.Text="";
            rbSteuerNein.Checked = true;
            rbSteuerJa.Checked = false;
            ddlTour.SelectedValue = "0";
            rbKreditNein.Checked = true;
            rbKreditJa.Checked = false;
            rbAuskunftNein.Checked = true;
            rbAuskunftJa.Checked = false;
            txtUmsatz.Text = "";
            txtBemerkung.Text = "";
            lbAbsenden.Visible = true;
            lblNoData.Text = "Bitte füllen Sie alle Pflichtfelder(*) aus!";
        }

        /// <summary>
        /// Daten sammeln an SAP übermitteln.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbAbsenden_Click(object sender, EventArgs e)
        {
            ClearErrors();
            if (ValidateInput())
            {
                lblError.Text = "Achtung! Es fehlen Angaben. Bitte die markierten Positionen bearbeiten.";
            }
            else
            {
                if (ddLand.SelectedValue != "DE" && rbLieferscheinKunde.Checked)
                {
                    ddLand.BorderStyle = BorderStyle.Solid;
                    ddLand.BorderWidth = 1;
                    ddLand.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    rbLieferscheinKunde.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    rbLieferscheinKunde.BorderStyle = BorderStyle.Solid;
                    rbLieferscheinKunde.BorderWidth = 1;
                    lblError.Text = "Achtung! Bitte die markierten Positionen bearbeiten.";
                    rbLieferscheinKunde.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    lblNeukundeResultatMeldung.ForeColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    lblNeukundeResultatMeldung.Text = "Land abweichend von Deutschland! Kein Lieferschein möglich.";
                    MPENeukundeResultat.Show();
                }
                else
                {
                    
                    GetMaskData();
                    if (ProofBank())
                    {
                        DoSubmit();
                    }
                    
                }
            }
        }

        /// <summary>
        /// Bankverbindung prüfen.
        /// </summary>
        /// <returns>ok?</returns>
        private Boolean ProofBank() 
        {
            bool blnOk = true;
            if (rbJa.Checked && txtIBAN.Text.Trim(' ').Length > 0)
            {
                objNeukunde.IBAN = txtIBAN.Text.Trim(' ');
                objNeukunde.ProofIBAN(Session["AppID"].ToString(), Session.SessionID, this);
                if (objNeukunde.Message != String.Empty)
                {
                    blnOk = false;
                    lblError.Text = objNeukunde.Message;
                    lblNeukundeResultatMeldung.ForeColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    lblNeukundeResultatMeldung.Text = objNeukunde.Message;
                }
                else
                {
                    txtSWIFT.Text = objNeukunde.SWIFT;
                    txtBankname.Text = objNeukunde.Bankname;
                }
            }
            else
            {
                objNeukunde.SWIFT = "";
                objNeukunde.Bankkey = "";
                objNeukunde.BLZ = "";
                objNeukunde.Kontonr = "";
            }

            return blnOk;    
        }

        /// <summary>
        /// Funktion Daten an SAP übermitteln
        /// </summary>
        private void DoSubmit()
        {
            objNeukunde.Change(Session["AppID"].ToString(), Session.SessionID, this);
            if (objNeukunde.Message != String.Empty)
            {
                lblError.Text = objNeukunde.Message;
                lblNeukundeResultatMeldung.ForeColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                lblNeukundeResultatMeldung.Text = objNeukunde.Message;
            }
            else
            {
                lblNeukundeResultatMeldung.ForeColor = System.Drawing.Color.Green;
                lblNeukundeResultatMeldung.Text = "Neue Kundenstammdaten erfolgreich angelegt!";
                MPENeukundeResultat.Show();
                EnableDisableControls(false);
            }
        }

        /// <summary>
        /// Markieren der Einzugsermächtigung(Ja) im Kopfbereich. Bei Lieferscheinkunde 
        ///  wird ein Vorlage für eine Einzugsermächtigung geöffnet.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void rbEinzugJa_CheckedChanged(object sender, EventArgs e)
        {
            rbJa.Checked = rbEinzugJa.Checked;
            rbNein.Checked= !rbEinzugJa.Checked;
            if (rbLieferscheinKunde.Checked && rbEinzugJa.Checked) 
            {
                ResponseHelper.Redirect("/PortalZLD/Applications/AppZulassungsdienst/Documents/Einzugsermächtigung_SEPA.pdf", "Einzugsermächtigung", "left=0,top=0,resizable=YES,scrollbars=YES");
            }
            
        }

        /// <summary>
        /// Markieren der Einzugermächtigung(Nein) im Kopfbereich.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void rbEinzugNein_CheckedChanged(object sender, EventArgs e)
        {
            rbJa.Checked = !rbEinzugNein.Checked;
            rbNein.Checked = rbEinzugNein.Checked;
        }

        /// <summary>
        /// Markieren der Einzugsermächtigung(Ja). Bei Lieferscheinkunde 
        ///  wird ein Vorlage für eine Einzugsermächtigung geöffnet.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void rbJa_CheckedChanged(object sender, EventArgs e)
        {
            rbEinzugJa.Checked =rbJa.Checked;
            rbEinzugNein.Checked = !rbJa.Checked;
            if (rbLieferscheinKunde.Checked && rbEinzugJa.Checked)
            {
                ResponseHelper.Redirect("/PortalZLD/Applications/AppZulassungsdienst/Documents/Einzugsermächtigung_SEPA.pdf", "Einzugsermächtigung", "left=0,top=0,resizable=YES,scrollbars=YES");
            }
        }

        /// <summary>
        /// Markieren der Einzugermächtigung(Nein) im Kopfbereich.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void rbNein_CheckedChanged(object sender, EventArgs e)
        {
            rbEinzugJa.Checked = !rbNein.Checked;
            rbEinzugNein.Checked = rbNein.Checked;
        }

    }
}
