using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using AutohausPortal.lib;

namespace AutohausPortal.Info
{
    public partial class KontaktLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HtmlGenericControl backlink = (HtmlGenericControl)Master.FindControl("backlink");
            backlink.Visible = true;
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            bool bError = false;
            lblError.Text = "";
            divName.Attributes["class"] = "formfeld";
            divBetreff.Attributes["class"] = "formfeld";
            divTelefon.Attributes["class"] = "formfeld";
            divDatum.Attributes["class"] = "formfeld";


            if (String.IsNullOrEmpty(txtName.Text))
            {
                divName.Attributes["class"] = "formfeld error";
                bError = true;
            }
            if (String.IsNullOrEmpty(txtBetreff.Text))
            {
                divBetreff.Attributes["class"] = "formfeld error";
                bError = true;
            }
            if (String.IsNullOrEmpty(txtTelefon.Text))
            {
                divTelefon.Attributes["class"] = "formfeld error";
                bError = true;
            }
            if (String.IsNullOrEmpty(txtDatum.Text))
            {
                divDatum.Attributes["class"] = "formfeld error";
                bError = true;
            }

            if (!bError)
            {
                string str = "";
                str += "\r\n";
                str += "Anrede         : " + ddlAnrede.SelectedItem.Text + "\r\n";
                str += "Name           : " + txtName.Text + "\r\n";
                str += "Telefon        : " + txtTelefon.Text + "\r\n";
                str += "Betreff        : " + txtBetreff.Text + "\r\n";
                str += "Rückruftermin  : " + txtDatum.Text + " " + ddlZeit.SelectedItem.Text + "\r\n";

                if (SendMail(str))
                {
                    ddlAnrede.SelectedIndex = -1;
                    txtName.Text = String.Empty;
                    txtTelefon.Text = String.Empty;
                    txtBetreff.Text = String.Empty;
                    txtDatum.Text = String.Empty;
                    ddlZeit.SelectedIndex = -1;
                    lblMessage.Visible = true;
                }
            }
            else
            {
                lblError.Text = "Füllen Sie bitte alle markierten Felder!";
            }
        }

        private bool SendMail(string message)
        {
            bool bSend = true;

            try
            {
                System.Net.Mail.MailMessage Mail;
                System.Net.Mail.MailAddress smtpMailSender = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["SmtpMailSender"]);
                String smtpMailServer = "", MailAdresses = "";

                Mail = new System.Net.Mail.MailMessage();
                Mail.Body = message;
                Mail.From = smtpMailSender;
                ZLDCommon.LeseMailEmpfaenger("3", ref MailAdresses);

                if (MailAdresses.Trim().Split(';').Length > 0)
                {
                    String[] Adressen = MailAdresses.Trim().Split(';');

                    foreach (String tmpStr in Adressen)
                    {
                        Mail.To.Add(tmpStr);
                    }
                }
                else if (MailAdresses.Length > 0)
                {
                    Mail.To.Add(MailAdresses);
                }
                else
                {
                    throw new Exception("Kein Mailempfänger gefunden");
                }
               
                Mail.Subject = "Benutzer bittet um Rückruf (Kundenportal)";
                Mail.IsBodyHtml = false;
                smtpMailServer = ConfigurationManager.AppSettings["SmtpMailServer"];
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(smtpMailServer);
                client.Send(Mail);
            }
            catch (Exception)
            {
                bSend = false;
                lblError.Text = "Fehler beim Versenden der E-Mail.";
            }

            return bSend;
        }
    }
}