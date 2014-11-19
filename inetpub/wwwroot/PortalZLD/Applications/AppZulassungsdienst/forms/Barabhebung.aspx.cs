using System;
using AppZulassungsdienst.lib;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using System.Configuration;

namespace AppZulassungsdienst.forms
{   
    /// <summary>
    /// Erfassung einer Barabhebung und senden einer Mail an den Innendienst.
    /// </summary>
    public partial class Barabhebung : System.Web.UI.Page
    {
        private User m_User;
        private App m_App;
        private ZLD_Suche objZLDSuche;
        private clsBarabhebung objBarabhebung;     

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

            if (Session["objBarabhebung"] != null)
            {
                objBarabhebung = (clsBarabhebung)Session["objBarabhebung"];
            }
            else
            {
                objBarabhebung = new clsBarabhebung(ref m_User, m_App, Session["AppID"].ToString(), Session.SessionID, "");
                Session["objBarabhebung"] = objBarabhebung;
            }

            if (!IsPostBack) 
            {
                txtKst.Text = m_User.Reference.Substring(4, 4);             
            }
        }

        /// <summary>
        /// Daten validieren, sammeln, Mail versenden und Daten in SAP speichern
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdCreate_Click(object sender, EventArgs e)
        {
            Boolean NotValid = false;
            if (txtName.Text.Trim().Length == 0)
            {
                txtName.BorderColor = System.Drawing.Color.Red;
                NotValid = true;
            }
            if (txtKst.Text.Trim().Length == 0)
            {
                txtKst.BorderColor = System.Drawing.Color.Red;
                NotValid = true;
            }
            if (txtNummerEC.Text.Trim().Length == 0)
            {
                txtNummerEC.BorderColor = System.Drawing.Color.Red;
                NotValid = true;
            }
            if (txtDatum.Text.Trim().Length == 0)
            {
                txtDatum.BorderColor = System.Drawing.Color.Red;
                NotValid = true;
            }
            if (txtUhrzeit.Text.Trim().Length == 0)
            {
                txtUhrzeit.BorderColor = System.Drawing.Color.Red;
                NotValid = true;
            }
            if (txtOrt.Text.Trim().Length == 0)
            {
                txtOrt.BorderColor = System.Drawing.Color.Red;
                NotValid = true;
            }
            if (txtBetrag.Text.Trim().Length == 0)
            {
                txtBetrag.BorderColor = System.Drawing.Color.Red;
                NotValid = true;
            }
            if (!NotValid)
            {
                FillBarabhebung();

                if (Sendmail())
                {
                    objBarabhebung.Change(Session["AppID"].ToString(), Session.SessionID, this);

                    if (objBarabhebung.Status != 0)
                    {
                        lblError.Text = "Fehler: " + objBarabhebung.Message;
                    }
                    else
                    {
                        lblError.Text = "Daten erfolgreich gespeichert!";
                        ClearForm();
                        if ((objBarabhebung.PDFXString != null) && (objBarabhebung.PDFXString.Length > 0))
                        {
                            Session["PDFXString"] = objBarabhebung.PDFXString;
                            ResponseHelper.Redirect("Printpdf.aspx", "_blank", "left=0,top=0,resizable=YES,scrollbars=YES");
                        }
                        else
                        {
                            lblError.Text += "PDF-Generierung fehlgeschlagen.";
                        }
                    }
                }
            }
            else
            {
                lblError.Text = "Prüfen Sie die rot markierten Felder!";
            }
        }

        /// <summary>
        /// Speichert die eingegebenen Daten im Barabhebungs-Objekt
        /// </summary>
        private void FillBarabhebung()
        {
            objBarabhebung.Name = txtName.Text;
            objBarabhebung.Kostenstelle = txtKst.Text;
            objBarabhebung.ECNr = txtNummerEC.Text;
            objBarabhebung.Datum = txtDatum.Text;
            objBarabhebung.Uhrzeit = txtUhrzeit.Text;
            objBarabhebung.Ort = txtOrt.Text;
            objBarabhebung.Betrag = txtBetrag.Text;
            Session["objBarabhebung"] = objBarabhebung;
        }

        /// <summary>
        /// Eingabefelder leeren/zurücksetzen
        /// </summary>
        private void ClearForm()
        {
            txtName.Text = "";
            txtKst.Text = "";
            txtNummerEC.Text = "";
            txtDatum.Text = "";
            txtUhrzeit.Text = "";
            txtOrt.Text = "";
            txtBetrag.Text = "";
            txtName.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf ");
            txtKst.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtNummerEC.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtDatum.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtUhrzeit.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtOrt.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtBetrag.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
        }

        /// <summary>
        ///  Mail versenden.
        /// </summary>
        /// <returns>false im Fehlerfall</returns>
        private Boolean Sendmail() 
        {
            try
            {
                System.Net.Mail.MailMessage Mail;
                objZLDSuche = new ZLD_Suche(ref m_User, m_App, "");

                objZLDSuche.LeseMailTexte("1");

                String smtpMailSender = ConfigurationManager.AppSettings["SmtpMailSender"];
                String smtpMailServer = ConfigurationManager.AppSettings["SmtpMailServer"];

                String MailText = "Name: " + objBarabhebung.Name + "<br />";
                MailText += "KSt: " + objBarabhebung.Kostenstelle + "<br />";
                MailText += "Nr. der EC-Karte: " + objBarabhebung.ECNr + "<br /><br />";
                MailText += "Datum: " + objBarabhebung.Datum + "<br />";
                MailText += "Uhrzeit: " + objBarabhebung.Uhrzeit + "<br />";
                MailText += "Ort der Barabhebung: " + objBarabhebung.Ort + "<br />";
                MailText += "Betrag: " + objBarabhebung.Betrag + "€";

                String[] Adressen;
                if (objZLDSuche.MailAdress.Trim().Split(';').Length > 1)
                {
                    Mail = new System.Net.Mail.MailMessage();
                    System.Net.Mail.MailAddress Mailsender = new System.Net.Mail.MailAddress(smtpMailSender);
                    Mail.Sender = Mailsender;
                    Mail.From = Mailsender;
                    Mail.Body = MailText;
                    Mail.Subject = "Barabhebung Filiale: " + m_User.Reference.Substring(4, 4);
                    Adressen = objZLDSuche.MailAdress.Trim().Split(';');
                    foreach (String tmpStr in Adressen)
                    {
                        Mail.To.Add(tmpStr);
                    }
                }
                else
                {
                    Mail = new System.Net.Mail.MailMessage(smtpMailSender, objZLDSuche.MailAdress.Trim(), "Barabhebung Filiale: " + m_User.Reference.Substring(4, 4), MailText);
                }
                if (objZLDSuche.MailAdressCC.Trim().Split(';').Length > 1)
                {
                    Adressen = objZLDSuche.MailAdressCC.Trim().Split(';');
                    foreach (String tmpStr in Adressen)
                    {
                        Mail.CC.Add(tmpStr);
                    }
                }
                else if (objZLDSuche.MailAdressCC.Length > 0)
                {
                    Mail.CC.Add(objZLDSuche.MailAdressCC);
                }

                Mail.IsBodyHtml = true;
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(smtpMailServer);
                client.Send(Mail);
                Mail.Attachments.Dispose();
                Mail.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                lblError.Text = "Fehler beim Senden! " + ex.Message;
                return false;
            }
        }
    }
}
