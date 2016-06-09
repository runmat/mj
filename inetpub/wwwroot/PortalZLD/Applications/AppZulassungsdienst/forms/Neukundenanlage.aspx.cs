using System;
using System.Web.UI.WebControls;
using AppZulassungsdienst.lib;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using System.Data;
using GeneralTools.Models;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Neukundenstammdaten. Anlegen eines neuen Kunden.
    /// </summary>
    public partial class Neukundenanlage : System.Web.UI.Page
    {
        private User m_User;
        private Neukunde objNeukunde;
        private ZLDCommon objCommon;

        #region Events

        /// <summary>
        /// Page_Load Ereignis. Prüfen ob die Anwendung dem Benutzer zugeordnet ist. Evtl. Stammdaten laden.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            Common.GetAppIDFromQueryString(this);
            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            lblError.Text = "";

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
                objCommon.GetGruppen_Touren("T");
                Session["objCommon"] = objCommon;
            }
            else
            {
                objCommon = (ZLDCommon)Session["objCommon"];
                if (objCommon.Touren == null)
                {
                    objCommon.GetGruppen_Touren("T");
                    if (objCommon.ErrorOccured)
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
                objNeukunde = new Neukunde(m_User.Reference);
                objNeukunde.Fill();
                fillDropDowns();
                Session["objNeukunde"] = objNeukunde;
            }
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
            txtIBAN.Text = "";
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
                    ddLand.BorderColor = ZLDCommon.BorderColorError;
                    rbLieferscheinKunde.BorderColor = ZLDCommon.BorderColorError;
                    rbLieferscheinKunde.BorderStyle = BorderStyle.Solid;
                    rbLieferscheinKunde.BorderWidth = 1;
                    lblError.Text = "Achtung! Bitte die markierten Positionen bearbeiten.";
                    rbLieferscheinKunde.BorderColor = ZLDCommon.BorderColorError;
                    lblNeukundeResultatMeldung.ForeColor = ZLDCommon.BorderColorError;
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
        /// Markieren der Einzugsermächtigung(Ja) im Kopfbereich. Bei Lieferscheinkunde 
        ///  wird ein Vorlage für eine Einzugsermächtigung geöffnet.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void rbEinzugJa_CheckedChanged(object sender, EventArgs e)
        {
            rbJa.Checked = rbEinzugJa.Checked;
            rbNein.Checked = !rbEinzugJa.Checked;
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
            rbEinzugJa.Checked = rbJa.Checked;
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

        #endregion

        #region Methods

        /// <summary>
        /// Füllen der Auswahldropdowns. Branche, Funktion des Ansprechpartners und Touren.
        /// </summary>
        private void fillDropDowns()
        {
            ListItem tmpItem;
            Int32 i = 0;
            ddlBranche.Items.Clear();
            do
            {
                tmpItem = new ListItem(objNeukunde.tblBranchen.Rows[i]["BRTXT"].ToString(), objNeukunde.tblBranchen.Rows[i]["BRSCH"].ToString());
                ddlBranche.Items.Add(tmpItem);
                i += 1;
            } while (i < objNeukunde.tblBranchen.Rows.Count);

            ddLand.Items.Clear();
            DataView tmpDataview = new DataView(objNeukunde.tblLaender);
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

            ddlTour.DataSource = objCommon.Touren;
            ddlTour.DataValueField = "Gruppe";
            ddlTour.DataTextField = "GruppenName";
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

            if (!rbBarkunde.Checked && !rbLieferscheinKunde.Checked)
            {
                rbBarkunde.BorderStyle = BorderStyle.Solid;
                rbBarkunde.BorderWidth = 1;
                rbBarkunde.BorderColor = ZLDCommon.BorderColorError;
                rbLieferscheinKunde.BorderStyle = BorderStyle.Solid;
                rbLieferscheinKunde.BorderWidth = 1;
                rbLieferscheinKunde.BorderColor = ZLDCommon.BorderColorError;
                bReturn = true;
            }
            if (rbLieferscheinKunde.Checked)
            {
                if (!rbEinzugJa.Checked && !rbEinzugNein.Checked)
                {
                    rbEinzugJa.BorderStyle = BorderStyle.Solid;
                    rbEinzugJa.BorderWidth = 1;
                    rbEinzugJa.BorderColor = ZLDCommon.BorderColorError;
                    rbEinzugNein.BorderStyle = BorderStyle.Solid;
                    rbEinzugNein.BorderWidth = 1;
                    rbEinzugNein.BorderColor = ZLDCommon.BorderColorError;
                    bReturn = true;
                }
            }
            if (!rbFirma.Checked && !rbHerr.Checked && !rbFrau.Checked)
            {
                rbFirma.BorderStyle = BorderStyle.Solid;
                rbFirma.BorderWidth = 1;
                rbFirma.BorderColor = ZLDCommon.BorderColorError;
                rbHerr.BorderStyle = BorderStyle.Solid;
                rbHerr.BorderWidth = 1;
                rbHerr.BorderColor = ZLDCommon.BorderColorError;
                rbFrau.BorderStyle = BorderStyle.Solid;
                rbFrau.BorderWidth = 1;
                rbFrau.BorderColor = ZLDCommon.BorderColorError;
                bReturn = true;
            }
            if (ddlBranche.SelectedIndex == -1)
            {
                ddlBranche.BorderStyle = BorderStyle.Solid;
                ddlBranche.BorderWidth = 1;
                ddlBranche.BorderColor = ZLDCommon.BorderColorError;
                bReturn = true;
            }
            else if (ddlBranche.SelectedValue == "0004" && String.IsNullOrEmpty(txtBrancheFrei.Text))
            {
                txtBrancheFrei.BorderColor = ZLDCommon.BorderColorError;
                bReturn = true;
            }
            if (String.IsNullOrEmpty(txtName1.Text))
            {
                txtName1.BorderColor = ZLDCommon.BorderColorError;
                bReturn = true;
            }

            if (String.IsNullOrEmpty(txtName1.Text))
            {
                txtName1.BorderColor = ZLDCommon.BorderColorError;
                bReturn = true;
            }

            if (String.IsNullOrEmpty(txtStrasse.Text))
            {
                txtStrasse.BorderColor = ZLDCommon.BorderColorError;
                bReturn = true;
            }

            if (String.IsNullOrEmpty(txtHausnr.Text))
            {
                txtHausnr.BorderColor = ZLDCommon.BorderColorError;
                bReturn = true;
            }
            if (String.IsNullOrEmpty(txtOrt.Text))
            {
                txtOrt.BorderColor = ZLDCommon.BorderColorError;
                bReturn = true;
            }
            if (String.IsNullOrEmpty(txtPlz.Text))
            {
                txtPlz.BorderColor = ZLDCommon.BorderColorError;
                bReturn = true;
            }
            else if (ddLand.SelectedValue == "DE" && txtPlz.Text.Trim().Length != 5)
            {
                txtPlz.BorderColor = ZLDCommon.BorderColorError;
                bReturn = true;
            }
            if (ddLand.SelectedIndex == -1)
            {
                ddLand.BorderStyle = BorderStyle.Solid;
                ddLand.BorderWidth = 1;
                ddLand.BorderColor = ZLDCommon.BorderColorError;
                bReturn = true;
            }
            else if (ddLand.SelectedValue != "DE")
            {
                if (String.IsNullOrEmpty(txtUIDNummer.Text))
                {
                    txtUIDNummer.BorderColor = ZLDCommon.BorderColorError;
                    bReturn = true;
                }
            }

            if (rbJa.Checked && String.IsNullOrEmpty(txtIBAN.Text))
            {
                txtIBAN.BorderColor = ZLDCommon.BorderColorError;
                txtSWIFT.BorderColor = ZLDCommon.BorderColorError;
                bReturn = true;
            }

            if (String.IsNullOrEmpty(txtUmsatz.Text))
            {
                txtUmsatz.BorderColor = ZLDCommon.BorderColorError;
                bReturn = true;
            }
            return bReturn;
        }

        /// <summary>
        /// Bankverbindung prüfen.
        /// </summary>
        /// <returns>ok?</returns>
        private Boolean ProofBank()
        {
            if (!String.IsNullOrEmpty(txtIBAN.Text))
            {
                objNeukunde.IBAN = txtIBAN.Text.NotNullOrEmpty().Trim().ToUpper();
                objNeukunde.ProofIBAN();

                if (objNeukunde.ErrorOccured)
                {
                    lblError.Text = objNeukunde.Message;
                    lblNeukundeResultatMeldung.ForeColor = ZLDCommon.BorderColorError;
                    lblNeukundeResultatMeldung.Text = objNeukunde.Message;
                    return false;
                }

                txtSWIFT.Text = objNeukunde.SWIFT;
                txtBankname.Text = objNeukunde.Bankname;
            }
            else
            {
                objNeukunde.SWIFT = "";
                objNeukunde.Bankkey = "";
                objNeukunde.BLZ = "";
                objNeukunde.Kontonr = "";
            }

            return true;
        }

        /// <summary>
        /// Funktion Daten an SAP übermitteln
        /// </summary>
        private void DoSubmit()
        {
            objNeukunde.Change(m_User.UserName);

            if (objNeukunde.ErrorOccured)
            {
                lblError.Text = objNeukunde.Message;
                lblNeukundeResultatMeldung.ForeColor = ZLDCommon.BorderColorError;
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

        #endregion
    }
}
