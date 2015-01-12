using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using AutohausPortal.lib;

namespace AutohausPortal.Controls
{
    public partial class BankdatenAdresse : UserControl
    {
        private const string _initialText = "Wird automatisch gefüllt!";

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
        public string IBAN { get { return (String.IsNullOrEmpty(txtIBAN.Text) ? "" : txtIBAN.Text.ToUpper()); } }
        public string SWIFT { get { return (String.IsNullOrEmpty(txtSWIFT.Text) ? "" : txtSWIFT.Text.ToUpper()); } }
        public bool IsSWIFTInitial { get { return (String.Compare(SWIFT, _initialText, true) == 0); } }
        public string Geldinstitut { get { return txtGeldinstitut.Text; } }
        public bool IsGeldinstitutInitial { get { return (String.Compare(Geldinstitut, _initialText, true) == 0); } }
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
            txtSWIFT.Text = _initialText;
            txtGeldinstitut.Text = _initialText;
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
                        if (IsSWIFTInitial)
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

            if (!String.IsNullOrEmpty(Kontoinhaber))
            {
                blnBankdatenErfasst = true;

                if (String.IsNullOrEmpty(IBAN))
                {
                    divIBAN.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
                if ((String.IsNullOrEmpty(SWIFT)) || (IsSWIFTInitial))
                {
                    divSWIFT.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
                if (IsGeldinstitutInitial)
                {
                    divGeldinstitut.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
            }

            if (!String.IsNullOrEmpty(IBAN))
            {
                blnBankdatenErfasst = true;

                if (String.IsNullOrEmpty(Kontoinhaber))
                {
                    divKontoinhaber.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
                if ((String.IsNullOrEmpty(SWIFT)) || (IsSWIFTInitial))
                {
                    divSWIFT.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
                if (IsGeldinstitutInitial)
                {
                    divGeldinstitut.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
            }

            if ((!String.IsNullOrEmpty(SWIFT)) && (!IsSWIFTInitial))
            {
                blnBankdatenErfasst = true;

                if (String.IsNullOrEmpty(Kontoinhaber))
                {
                    divKontoinhaber.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
                if (String.IsNullOrEmpty(IBAN))
                {
                    divIBAN.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
                if (IsGeldinstitutInitial)
                {
                    txtGeldinstitut.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
            }

            if ((!String.IsNullOrEmpty(Geldinstitut)) && (!IsGeldinstitutInitial))
            {
                blnBankdatenErfasst = true;

                if (String.IsNullOrEmpty(Kontoinhaber))
                {
                    divKontoinhaber.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
                if (String.IsNullOrEmpty(IBAN))
                {
                    divIBAN.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
                if ((String.IsNullOrEmpty(SWIFT)) || (IsSWIFTInitial))
                {
                    divSWIFT.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
            }

            if (!String.IsNullOrEmpty(Name1))
            {
                blnAdressdatenErfasst = true;

                if (String.IsNullOrEmpty(Strasse))
                {
                    divStrasse.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
                if ((String.IsNullOrEmpty(Plz)) || (Plz.Length < 5))
                {
                    divPLZ.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
                if (String.IsNullOrEmpty(Ort))
                {
                    divOrt.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
            }

            if (!String.IsNullOrEmpty(Strasse))
            {
                blnAdressdatenErfasst = true;

                if (String.IsNullOrEmpty(Name1))
                {
                    divName1.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
                if ((String.IsNullOrEmpty(Plz)) || (Plz.Length < 5))
                {
                    divPLZ.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
                if (String.IsNullOrEmpty(Ort))
                {
                    divOrt.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
            }

            if (!String.IsNullOrEmpty(Plz))
            {
                blnAdressdatenErfasst = true;

                if (String.IsNullOrEmpty(Name1))
                {
                    divName1.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
                if (String.IsNullOrEmpty(Strasse))
                {
                    divStrasse.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
                if (String.IsNullOrEmpty(Ort))
                {
                    divOrt.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
            }

            if (!String.IsNullOrEmpty(Ort))
            {
                blnAdressdatenErfasst = true;

                if (String.IsNullOrEmpty(Name1))
                {
                    divName1.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
                if (String.IsNullOrEmpty(Strasse))
                {
                    divStrasse.Attributes["class"] = "formfeld error";
                    blnOk = false;
                }
                if ((String.IsNullOrEmpty(Plz)) || (Plz.Length < 5))
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
            if (Name1 != "") { disableDefaultValue(txtName1); }
            if (Name2 != "") { disableDefaultValue(txtName2); }
            if (Strasse != "") { disableDefaultValue(txtStrasse); }
            if (Plz != "") { disableDefaultValue(txtPlz); }
            if (Ort != "") { disableDefaultValue(txtOrt); }

            if (Kontoinhaber != "") { disableDefaultValue(txtKontoinhaber); }
            if (IBAN != "") { disableDefaultValue(txtIBAN); }
            if (SWIFT != "") { disableDefaultValue(txtSWIFT); }
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