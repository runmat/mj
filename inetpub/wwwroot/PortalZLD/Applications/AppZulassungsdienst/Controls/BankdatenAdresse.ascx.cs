using System;
using System.Web.UI;
using AppZulassungsdienst.lib;
using AppZulassungsdienst.lib.Models;
using GeneralTools.Models;

namespace AppZulassungsdienst.Controls
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
        public string Land { get; set; }
        public bool Einzug { get { return chkEinzug.Checked; } }
        public bool Rechnung { get { return chkRechnung.Checked; } }
        public string Kontoinhaber { get { return txtKontoinhaber.Text; } }
        public string IBAN { get { return (String.IsNullOrEmpty(txtIBAN.Text) ? "" : txtIBAN.Text.ToUpper()); } }
        public string SWIFT { get { return (String.IsNullOrEmpty(txtSWIFT.Text) ? "" : txtSWIFT.Text.ToUpper()); } }
        public bool IsSWIFTInitial { get { return (String.Compare(SWIFT, _initialText, true) == 0); } }
        public string Geldinstitut { get { return txtGeldinstitut.Text; } }
        public bool IsGeldinstitutInitial { get { return (String.Compare(Geldinstitut, _initialText, true) == 0); } }
        public string Bankkey { get; set; }
        public string Kontonr { get; set; }

        #endregion

        #region Konstruktor

        public BankdatenAdresse()
        {
            Land = "DE";
        }

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

            chkEinzug.Checked = false;
            chkRechnung.Checked = false;

            txtKontoinhaber.Text = "";
            txtIBAN.Text = "";
            txtSWIFT.Text = _initialText;
            txtGeldinstitut.Text = _initialText;

            Bankkey = "";
            Kontonr = "";
            Land = "";
        }

        public void SelectValues(ZLDBankdaten bankdaten, ZLDAdressdaten adressdaten)
        {
            txtName1.Text = adressdaten.Name1;
            txtName2.Text = adressdaten.Name2;
            txtStrasse.Text = adressdaten.Strasse;
            txtPlz.Text = adressdaten.Plz;
            txtOrt.Text = adressdaten.Ort;
            Land = adressdaten.Land;

            txtSWIFT.Text = bankdaten.SWIFT;
            txtIBAN.Text = bankdaten.IBAN;
            txtGeldinstitut.Text = (String.IsNullOrEmpty(bankdaten.Geldinstitut) ? _initialText : bankdaten.Geldinstitut);
            txtKontoinhaber.Text = bankdaten.Kontoinhaber;
            chkEinzug.Checked = bankdaten.Einzug.IsTrue();
            chkRechnung.Checked = bankdaten.Rechnung.IsTrue();
            Bankkey = bankdaten.Bankleitzahl;
            Kontonr = bankdaten.KontoNr;
        }

        public bool proofBank(ref ZLDCommon objCommon, bool cpdMitEinzug)
        {
            if (!String.IsNullOrEmpty(txtIBAN.Text))
            {
                objCommon.IBAN = txtIBAN.Text.NotNullOrEmpty().Trim().ToUpper();
                objCommon.ProofIBAN();

                if (objCommon.ErrorOccured)
                {
                    txtIBAN.BorderColor = ZLDCommon.BorderColorError;
                    lblErrorBank.Text = objCommon.Message;
                    return false;
                }

                txtSWIFT.Text = objCommon.SWIFT;
                txtGeldinstitut.Text = objCommon.Bankname;
                Bankkey = objCommon.Bankschluessel;
                Kontonr = objCommon.Kontonr;
            }
            else if (cpdMitEinzug)
            {
                txtIBAN.BorderColor = ZLDCommon.BorderColorError;
                lblErrorBank.Text = "Keine IBAN angegeben!";
                return false;
            }

            return true;
        }

        public bool proofBankAndAddressData(ZLDCommon objCommon, bool cpdKunde = false, bool cpdMitEinzug = false)
        {
            bool blnOk = true;
            bool blnPlzValid = true;
            bool blnBankdatenErfasst = false;
            bool blnAdressdatenErfasst = false;

            if (!String.IsNullOrEmpty(Kontoinhaber))
            {
                blnBankdatenErfasst = true;

                if (String.IsNullOrEmpty(IBAN))
                {
                    txtIBAN.BorderColor = ZLDCommon.BorderColorError;
                    blnOk = false;
                }
                if (String.IsNullOrEmpty(SWIFT) || IsSWIFTInitial)
                {
                    txtSWIFT.BorderColor = ZLDCommon.BorderColorError;
                    blnOk = false;
                }
                if (IsGeldinstitutInitial)
                {
                    txtGeldinstitut.BorderColor = ZLDCommon.BorderColorError;
                    blnOk = false;
                }
            }

            if (!String.IsNullOrEmpty(IBAN))
            {
                blnBankdatenErfasst = true;

                if (String.IsNullOrEmpty(Kontoinhaber))
                {
                    txtKontoinhaber.BorderColor = ZLDCommon.BorderColorError;
                    blnOk = false;
                }
                if (String.IsNullOrEmpty(SWIFT) || IsSWIFTInitial)
                {
                    txtSWIFT.BorderColor = ZLDCommon.BorderColorError;
                    blnOk = false;
                }
                if (IsGeldinstitutInitial)
                {
                    txtGeldinstitut.BorderColor = ZLDCommon.BorderColorError;
                    blnOk = false;
                }
            }

            if (!String.IsNullOrEmpty(SWIFT) && !IsSWIFTInitial)
            {
                blnBankdatenErfasst = true;

                if (String.IsNullOrEmpty(Kontoinhaber))
                {
                    txtKontoinhaber.BorderColor = ZLDCommon.BorderColorError;
                    blnOk = false;
                }
                if (String.IsNullOrEmpty(IBAN))
                {
                    txtIBAN.BorderColor = ZLDCommon.BorderColorError;
                    blnOk = false;
                }
                if (IsGeldinstitutInitial)
                {
                    txtGeldinstitut.BorderColor = ZLDCommon.BorderColorError;
                    blnOk = false;
                }
            }

            if (!String.IsNullOrEmpty(Geldinstitut) && !IsGeldinstitutInitial)
            {
                blnBankdatenErfasst = true;

                if (String.IsNullOrEmpty(Kontoinhaber))
                {
                    txtKontoinhaber.BorderColor = ZLDCommon.BorderColorError;
                    blnOk = false;
                }
                if (String.IsNullOrEmpty(IBAN))
                {
                    txtIBAN.BorderColor = ZLDCommon.BorderColorError;
                    blnOk = false;
                }
                if (String.IsNullOrEmpty(SWIFT) || IsSWIFTInitial)
                {
                    txtSWIFT.BorderColor = ZLDCommon.BorderColorError;
                    blnOk = false;
                }
            }

            if (!String.IsNullOrEmpty(Name1))
            {
                blnAdressdatenErfasst = true;

                if (String.IsNullOrEmpty(Strasse))
                {
                    txtStrasse.BorderColor = ZLDCommon.BorderColorError;
                    blnOk = false;
                }
                if (String.IsNullOrEmpty(Plz))
                {
                    txtPlz.BorderColor = ZLDCommon.BorderColorError;
                    blnOk = false;
                }
                if (String.IsNullOrEmpty(Ort))
                {
                    txtOrt.BorderColor = ZLDCommon.BorderColorError;
                    blnOk = false;
                }
            }

            if (!String.IsNullOrEmpty(Strasse))
            {
                blnAdressdatenErfasst = true;

                if (String.IsNullOrEmpty(Name1))
                {
                    txtName1.BorderColor = ZLDCommon.BorderColorError;
                    blnOk = false;
                }
                if (String.IsNullOrEmpty(Plz))
                {
                    txtPlz.BorderColor = ZLDCommon.BorderColorError;
                    blnOk = false;
                }
                if (String.IsNullOrEmpty(Ort))
                {
                    txtOrt.BorderColor = ZLDCommon.BorderColorError;
                    blnOk = false;
                }
            }

            if (!String.IsNullOrEmpty(Plz))
            {
                blnAdressdatenErfasst = true;

                if (!objCommon.CheckPlzValid(Land, Plz))
                {
                    txtPlz.BorderColor = ZLDCommon.BorderColorError;
                    blnPlzValid = false;
                    blnOk = false;
                }
                if (String.IsNullOrEmpty(Name1))
                {
                    txtName1.BorderColor = ZLDCommon.BorderColorError;
                    blnOk = false;
                }
                if (String.IsNullOrEmpty(Strasse))
                {
                    txtStrasse.BorderColor = ZLDCommon.BorderColorError;
                    blnOk = false;
                }
                if (String.IsNullOrEmpty(Ort))
                {
                    txtOrt.BorderColor = ZLDCommon.BorderColorError;
                    blnOk = false;
                }
            }

            if (!String.IsNullOrEmpty(Ort))
            {
                blnAdressdatenErfasst = true;

                if (String.IsNullOrEmpty(Name1))
                {
                    txtName1.BorderColor = ZLDCommon.BorderColorError;
                    blnOk = false;
                }
                if (String.IsNullOrEmpty(Strasse))
                {
                    txtStrasse.BorderColor = ZLDCommon.BorderColorError;
                    blnOk = false;
                }
                if (String.IsNullOrEmpty(Plz))
                {
                    txtPlz.BorderColor = ZLDCommon.BorderColorError;
                    blnOk = false;
                }
            }

            if (cpdMitEinzug && !blnBankdatenErfasst)
            {
                // Wenn Einzug -> Bankdaten Pflicht
                txtKontoinhaber.BorderColor = ZLDCommon.BorderColorError;
                txtIBAN.BorderColor = ZLDCommon.BorderColorError;
                txtSWIFT.BorderColor = ZLDCommon.BorderColorError;
                txtGeldinstitut.BorderColor = ZLDCommon.BorderColorError;
                blnOk = false;
            }

            if (cpdKunde)
            {
                if (!blnAdressdatenErfasst)
                {
                    // Adressdaten bei CPD-Kunden immer Pflicht
                    txtName1.BorderColor = ZLDCommon.BorderColorError;
                    txtStrasse.BorderColor = ZLDCommon.BorderColorError;
                    txtPlz.BorderColor = ZLDCommon.BorderColorError;
                    txtOrt.BorderColor = ZLDCommon.BorderColorError;
                    blnOk = false;
                }
            }
            else
            {
                // wenn Bankdaten erfasst wurden, muss eine Adresse vorhanden sein
                if ((blnBankdatenErfasst) && (!blnAdressdatenErfasst))
                {
                    txtName1.BorderColor = ZLDCommon.BorderColorError;
                    txtStrasse.BorderColor = ZLDCommon.BorderColorError;
                    txtPlz.BorderColor = ZLDCommon.BorderColorError;
                    txtOrt.BorderColor = ZLDCommon.BorderColorError;
                    blnOk = false;
                }
            }

            if (!blnOk)
            {
                lblErrorBank.Text = "Es müssen alle Pflichtfelder ausgefüllt sein!";

                if (!blnPlzValid)
                    lblErrorBank.Text += " Die Postleitzahl hat das falsche Format!";
            }

            return blnOk;
        }

        public void ClearError()
        {
            lblErrorBank.Text = "";
            txtName1.BorderColor = ZLDCommon.BorderColorDefault;
            txtName2.BorderColor = ZLDCommon.BorderColorDefault;
            txtStrasse.BorderColor = ZLDCommon.BorderColorDefault;
            txtOrt.BorderColor = ZLDCommon.BorderColorDefault;
            txtPlz.BorderColor = ZLDCommon.BorderColorDefault;
            txtKontoinhaber.BorderColor = ZLDCommon.BorderColorDefault;
            txtIBAN.BorderColor = ZLDCommon.BorderColorDefault;
            txtSWIFT.BorderColor = ZLDCommon.BorderColorDefault;
            txtGeldinstitut.BorderColor = ZLDCommon.BorderColorDefault;
        }

        public void SetZulDat(string zulDat)
        {
            txtZulDateBank.Text = zulDat;
        }

        public void SetKunde(string kunde)
        {
            txtKundebank.Text = kunde;
        }

        public void SetKundeSuche(string kundeSuche)
        {
            txtKundeBankSuche.Text = kundeSuche;
        }

        public void SetRef1(string ref1)
        {
            txtRef1Bank.Text = ref1;
        }

        public void SetRef2(string ref2)
        {
            txtRef2Bank.Text = ref2;
        }

        public void SetEinzug(bool blnEinzug)
        {
            chkEinzug.Checked = blnEinzug;
        }

        public void SetRechnung(bool blnRechnung)
        {
            chkRechnung.Checked = blnRechnung;
        }

        public void FocusName1()
        {
            txtName1.Focus();
        }

        #endregion
    }
}