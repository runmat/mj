using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using AutohausPortal.lib;
using System.Data;
using System.Data.SqlClient;

namespace AutohausPortal.Info
{
    public partial class Kontakt : System.Web.UI.Page
    {
        CKG.Base.Kernel.Security.User m_User;

        protected void Page_Load(object sender, EventArgs e)
        {
            HtmlGenericControl backlink = (HtmlGenericControl)Master.FindControl("backlink");
            backlink.Visible = true;

            //m_User = (CKG.Base.Kernel.Security.User)Session["objUser"];
            m_User = MVC.GetSessionUserObject();

            if (!IsPostBack)
            {
                ShowAnsprechpartner();
            }
        }

        private void ShowAnsprechpartner()
        {
            if ((m_User != null) && (m_User.Customer != null) && (m_User.Groups.Count > 0))
            {
                DataTable result = new DataTable();

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"]))
                {
                    conn.Open();

                    SqlDataAdapter daApp = new SqlDataAdapter("SELECT *, Name2 + ',' + Name1 + ' (' + [position] + ')' AS EmployeeName" +
                                                              " FROM Contact INNER JOIN ContactGroups ON Contact.id = ContactGroups.ContactID " +
                                                              " WHERE (ContactGroups.CustomerID = @CustomerID) AND (ContactGroups.GroupID = @GroupID)", conn);

                    daApp.SelectCommand.Parameters.AddWithValue("@CustomerID", m_User.Customer.CustomerId);
                    daApp.SelectCommand.Parameters.AddWithValue("@GroupID", m_User.GroupID);
                    daApp.Fill(result);

                    // Standard-Kontakt nur ersetzen, wenn für die Gruppe gepflegt
                    if (result.DefaultView.Count > 0)
                    {
                        DataRowView drv = result.DefaultView[0];
                        string strName = drv["Name1"] + " " + drv["Name2"];

                        if (!String.IsNullOrEmpty(strName))
                        {
                            lblAnsprechpartner.Text = strName;
                        }
                        lblTelefon.Text = drv["Telefon"].ToString();
                        lblFax.Text = drv["Fax"].ToString();
                        lnkMail.HRef = "mailto:" + drv["Mail"].ToString();
                        lnkMail.InnerText = drv["Mail"].ToString();
                    }

                    conn.Close();
                }
            }
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