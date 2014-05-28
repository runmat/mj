using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AutohausPortal.lib;

namespace AutohausPortal.Controls
{
    public partial class BankdatenAdresse : System.Web.UI.UserControl
    {
        #region Properties

        public string Name1 { get { return txtName1.Text; } }
        public string Name2 { get { return txtName2.Text; } }
        public string Strasse { get { return txtStrasse.Text; } }
        public string Plz { get { return txtPlz.Text; } }
        public string Ort { get { return txtOrt.Text; } }
        public bool Einzug { get { return rbEinzug.Checked; } }
        public bool Rechnung { get { return rbRechnung.Checked; } }
        public bool Bar { get { return rbBar.Checked; } }
        public string Kontoinhaber { get { return txtKontoinhaber.Text; } }
        public string IBAN { get { return txtIBAN.Text; } }
        public string SWIFT { get { return txtSWIFT.Text; } }
        public string Geldinstitut { get { return txtGeldinstitut.Text; } }
        public string Bankkey { get; set; }
        public string Kontonr { get; set; }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #endregion

        #region Methods

        public void ResetFields()
        {
            txtName1.Text = "";
            txtName2.Text = "";        
            txtStrasse.Text = "";
            txtPlz.Text = "";
            txtOrt.Text = "";

            rbEinzug.Checked = false;
            rbRechnung.Checked = false;
            rbBar.Checked = false;

            txtKontoinhaber.Text = "";
            txtIBAN.Text = "";
            txtSWIFT.Text = "Wird automatisch gefüllt!";
            txtGeldinstitut.Text = "Wird automatisch gefüllt!";
        }

        public void SelectValues(AHErfassung objVorerf)
        {
            txtName1.Text = objVorerf.Name1;
            txtName2.Text = objVorerf.Name2;
            txtPlz.Text = objVorerf.PLZ;
            txtOrt.Text = objVorerf.Ort;
            txtStrasse.Text = objVorerf.Strasse;

            txtSWIFT.Text = objVorerf.SWIFT;
            txtIBAN.Text = objVorerf.IBAN;
            if (objVorerf.Geldinstitut.Length > 0)
            {
                txtGeldinstitut.Text = objVorerf.Geldinstitut;
            }

            txtKontoinhaber.Text = objVorerf.Inhaber;
            rbEinzug.Checked = objVorerf.EinzugErm;
            rbRechnung.Checked = objVorerf.Rechnung;
            rbBar.Checked = objVorerf.Barzahlung;
        }

        /// <summary>
        /// Prüfung ob anhand der eingebenen IBAN die Daten im System existieren
        /// Aufruf objCommon.ProofIBAN
        /// </summary>
        /// <returns>ok?</returns>
        public bool proofBank(ref ZLDCommon objCommon)
        {
            bool blnOk = true;
            if (!String.IsNullOrEmpty(IBAN))
            {
                objCommon.IBAN = IBAN.Trim(' ');
                objCommon.ProofIBAN(Session["AppID"].ToString(), Session.SessionID, this.Page);
                if (!String.IsNullOrEmpty(objCommon.Message))
                {
                    blnOk = false;
                    divSWIFT.Attributes["class"] = "formfeld error";
                    divGeldinstitut.Attributes["class"] = "formfeld error";
                }
                else
                {
                    if (!String.IsNullOrEmpty(objCommon.SWIFT))
                    {
                        txtSWIFT.Text = objCommon.SWIFT;
                    }
                    else
                    {
                        // SWIFT manuell eingebbar machen, wenn unbekannt
                        txtSWIFT.Enabled = true;
                        if (txtSWIFT.Text == "Wird automatisch gefüllt!")
                        {
                            txtSWIFT.Text = "";
                        }
                    }
                    txtGeldinstitut.Text = objCommon.Bankname;
                    Bankkey = objCommon.Bankkey;
                    Kontonr = objCommon.Kontonr;
                }
            }

            return blnOk;
        }

        /// <summary>
        /// Validation Bank- und Adressdaten
        /// </summary>
        /// <param name="cpdKunde"></param>
        /// <returns>ok?</returns>
        public bool proofBankAndAddressData(bool cpdKunde = false)
        {
            bool blnOk = true;
            bool blnBankdatenErfasst = false;
            bool blnAdressdatenErfasst = false;

            if (!String.IsNullOrEmpty(txtKontoinhaber.Text))
            {
                blnBankdatenErfasst = true;

                if (String.IsNullOrEmpty(txtIBAN.Text))
                {
                    divIBAN.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
                if ((String.IsNullOrEmpty(txtSWIFT.Text)) || (txtSWIFT.Text == "Wird automatisch gefüllt!"))
                {
                    divSWIFT.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
                if (txtGeldinstitut.Text == "Wird automatisch gefüllt!")
                {
                    divGeldinstitut.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
            }

            if (!String.IsNullOrEmpty(txtIBAN.Text))
            {
                blnBankdatenErfasst = true;

                if (String.IsNullOrEmpty(txtKontoinhaber.Text))
                {
                    divKontoinhaber.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
                if ((String.IsNullOrEmpty(txtSWIFT.Text)) || (txtSWIFT.Text == "Wird automatisch gefüllt!"))
                {
                    divSWIFT.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
                if (txtGeldinstitut.Text == "Wird automatisch gefüllt!")
                {
                    divGeldinstitut.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
            }

            if ((!String.IsNullOrEmpty(txtSWIFT.Text)) && (txtSWIFT.Text != "Wird automatisch gefüllt!"))
            {
                blnBankdatenErfasst = true;

                if (String.IsNullOrEmpty(txtKontoinhaber.Text))
                {
                    divKontoinhaber.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
                if (String.IsNullOrEmpty(txtIBAN.Text))
                {
                    divIBAN.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
                if (txtGeldinstitut.Text == "Wird automatisch gefüllt!")
                {
                    txtGeldinstitut.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
            }

            if ((!String.IsNullOrEmpty(txtGeldinstitut.Text)) && (txtGeldinstitut.Text != "Wird automatisch gefüllt!"))
            {
                blnBankdatenErfasst = true;

                if (String.IsNullOrEmpty(txtKontoinhaber.Text))
                {
                    divKontoinhaber.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
                if (String.IsNullOrEmpty(txtIBAN.Text))
                {
                    divIBAN.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
                if ((String.IsNullOrEmpty(txtSWIFT.Text)) || (txtSWIFT.Text == "Wird automatisch gefüllt!"))
                {
                    divSWIFT.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
            }

            if (!String.IsNullOrEmpty(txtName1.Text))
            {
                blnAdressdatenErfasst = true;

                if (String.IsNullOrEmpty(txtStrasse.Text))
                {
                    divStrasse.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
                if ((String.IsNullOrEmpty(txtPlz.Text)) || (txtPlz.Text.Length < 5))
                {
                    divPLZ.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
                if (String.IsNullOrEmpty(txtOrt.Text))
                {
                    divOrt.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
            }

            if (!String.IsNullOrEmpty(txtStrasse.Text))
            {
                blnAdressdatenErfasst = true;

                if (String.IsNullOrEmpty(txtName1.Text))
                {
                    divName1.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
                if ((String.IsNullOrEmpty(txtPlz.Text)) || (txtPlz.Text.Length < 5))
                {
                    divPLZ.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
                if (String.IsNullOrEmpty(txtOrt.Text))
                {
                    divOrt.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
            }

            if (!String.IsNullOrEmpty(txtPlz.Text))
            {
                blnAdressdatenErfasst = true;

                if (String.IsNullOrEmpty(txtName1.Text))
                {
                    divName1.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
                if (String.IsNullOrEmpty(txtStrasse.Text))
                {
                    divStrasse.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
                if (String.IsNullOrEmpty(txtOrt.Text))
                {
                    divOrt.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
            }

            if (!String.IsNullOrEmpty(txtOrt.Text))
            {
                blnAdressdatenErfasst = true;

                if (String.IsNullOrEmpty(txtName1.Text))
                {
                    divName1.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
                if (String.IsNullOrEmpty(txtStrasse.Text))
                {
                    divStrasse.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
                if ((String.IsNullOrEmpty(txtPlz.Text)) || (txtPlz.Text.Length < 5))
                {
                    divPLZ.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
            }

            if ((Einzug) && (!blnBankdatenErfasst))
            {
                // Wenn Einzug -> Bankdaten Pflicht
                divKontoinhaber.Attributes["class"] = "formfeld error";
                divIBAN.Attributes["class"] = "formfeld error";
                divSWIFT.Attributes["class"] = "formfeld error";
                divGeldinstitut.Attributes["class"] = "formfeld error";
                blnOk = false;
            }

            if (cpdKunde)
            {
                if (!blnAdressdatenErfasst)
                {
                    // Adressdaten bei CPD-Kunden immer Pflicht
                    divName1.Attributes["class"] = "formfeld error";
                    divStrasse.Attributes["class"] = "formfeld error";
                    divStrasse.Attributes["class"] = "formfeld error";
                    divPLZ.Attributes["class"] = "formfeld error";
                    divOrt.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
            }
            else
            {
                // wenn Bankdaten erfasst wurden, muss eine Adresse vorhanden sein
                if ((blnBankdatenErfasst) && (!blnAdressdatenErfasst))
                {
                    divName1.Attributes["class"] = "formfeld error";
                    divStrasse.Attributes["class"] = "formfeld error";
                    divStrasse.Attributes["class"] = "formfeld error";
                    divPLZ.Attributes["class"] = "formfeld error";
                    divOrt.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
            }

            if ((blnAdressdatenErfasst) && (!rbEinzug.Checked) && (!rbRechnung.Checked) && (!rbBar.Checked))
            {
                // wenn Adressdaten erfasst wurden, muss eine Zahlart ausgewählt sein
                divZahlungsart.Attributes["class"] = "formselects error";
                blnOk = false;
            }

            return blnOk;
        }

        public void proofInserted()
        {
            if (txtName1.Text != "") { disableDefaultValue(txtName1); }
            if (txtName2.Text != "") { disableDefaultValue(txtName2); }
            if (txtStrasse.Text != "") { disableDefaultValue(txtStrasse); }
            if (txtPlz.Text != "") { disableDefaultValue(txtPlz); }
            if (txtOrt.Text != "") { disableDefaultValue(txtOrt); }

            if (txtKontoinhaber.Text != "") { disableDefaultValue(txtKontoinhaber); }
            if (txtIBAN.Text != "") { disableDefaultValue(txtIBAN); }
            if (txtSWIFT.Text != "") { disableDefaultValue(txtSWIFT); }
        }

        /// <summary>
        /// entfernt den Vorschlagswert der Textbox, wenn Eingabe erfolgte
        /// </summary>
        /// <param name="txtBox">Control</param>
        private void disableDefaultValue(TextBox txtBox)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), txtBox.ClientID,
                "<script type='text/javascript'>disableDefaultValue('" + txtBox.ClientID + "');</script>", false);
        }

        public void SetErrorKontoinhaber()
        {
            divKontoinhaber.Attributes["class"] = "formfeld error";
        }

        public void SetErrorIBAN()
        {
            divIBAN.Attributes["class"] = "formfeld error";
        }

        public void SetErrorSWIFT()
        {
            divSWIFT.Attributes["class"] = "formfeld error";
        }

        /// <summary>
        /// entfernt das Errorstyle der Controls
        /// </summary>
        public void ClearError()
        {
            divName1.Attributes["class"] = "formfeld";
            divName2.Attributes["class"] = "formfeld";
            divStrasse.Attributes["class"] = "formfeld";
            divOrt.Attributes["class"] = "formfeld";
            divPLZ.Attributes["class"] = "formfeld";
            divKontoinhaber.Attributes["class"] = "formfeld";
            divIBAN.Attributes["class"] = "formfeld";
            divSWIFT.Attributes["class"] = "formfeld";
            divGeldinstitut.Attributes["class"] = "formfeld";
            divZahlungsart.Attributes["class"] = "formselects";
        }

        #endregion
    }
}