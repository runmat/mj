using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using CKG.Components.Zulassung.UserControls;
using CKG.Components.Zulassung.DAL;
using System.Text;
using System.Data;

namespace CKG.Components.Zulassung
{
    public partial class _Default : System.Web.UI.Page, IWizardPage
    {
        private const string ZulassungDalSessionKey = "ZulassungDal";
        internal const string InitMultiSessionKey = "ISINITMULTI";
        private const string SubmitIDSessionKey = "SubmitID";

        private User user;
        private App app;
        private const string ToggleScript = @"function OnStepChanged(index) {{
                                                            $(""img[id^='toggleheader_']"").attr('src', '{0}');
                                                            $(""#toggleheader_"" + index).attr('src', '{1}');
                                                        }}";

        public new User User
        {
            get { return user; }
        }

        public App App
        {
            get { return app; }
        }

        public ZulassungDal DAL 
        {
             get
             {
                 var dal = Session[ZulassungDalSessionKey] as ZulassungDal;
                 if(dal == null)
                 {
                     if (System.Diagnostics.Debugger.IsAttached)
                     {
                         System.Diagnostics.Debugger.Break();
                     }

                     throw new InvalidOperationException();

                 }
                 return dal;
            }
            private set
            {
                Session[ZulassungDalSessionKey] = value;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                Session.Remove(ZulassungDalSessionKey);
                Session.Remove(InitMultiSessionKey);
                Session.Remove(SubmitIDSessionKey);
            }

            // Get current user
            this.user = Common.GetUser(this);

            // Apply forms authentication
            Common.FormAuth(this, this.user);

            // Get current application instance
            this.app = new App(this.user);

            if (!this.IsPostBack)
            {
                // Save app id to session
                Common.GetAppIDFromQueryString(this);
                // Initialise DAL
                this.DAL = new ZulassungDal(ref this.user, this.app, string.Empty, this);
            }

            base.OnInit(e);

            // Initialize all wizard steps
            InitWizardSteps();

            // Remove old update progress
            var updateProgress = Page.Master.FindControl("UpdateProgress1");
            if (updateProgress != null)
            {
                updateProgress.Parent.Controls.Remove(updateProgress);
            }

            // Register client scripts 
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "ClientScripts", string.Format(ToggleScript, Page.ResolveClientUrl("~/Images/Zulassung/toggleDown.png"), Page.ResolveClientUrl("~/Images/Zulassung/toggleUp.png")), true);
        }

        private void InitWizardSteps()
        {
            foreach (var step in TabControl1.Steps)
            {
                var wiz = step.Content as IWizardStep;

                if (wiz != null)
                {
                    wiz.Completed += OnWizStepCompleted;
                    wiz.NavigateBack += OnWizNavigateBack;
                }
            }

            if (Session[SubmitIDSessionKey] != null)
            {
                labelError.Text = string.Format("Der Auftrag wurde bereits mit der Nummer {0} angelegt.", Session[SubmitIDSessionKey] as string);
                buttonChangeData.Visible = false;
                ErrorDisplay.Visible = true;
                TabControl1.Visible = false;
                SuccessDisplay.Visible = false;
                TabControl1.SelectedIndex = TabControl1.StepCount - 1;
            }
        }

        void OnWizNavigateBack(object sender, EventArgs e)
        {
            // Step backward
            TabControl1.SelectedIndex--;
        }

        protected override void OnPreRenderComplete(EventArgs e)
        {
            Common.SetEndASPXAccess(Page);
            base.OnPreRenderComplete(e);
        }

        protected void buttonRestart_Click(object sender, EventArgs e)
        {
            Session.Remove(ZulassungDalSessionKey);
            Session.Remove(InitMultiSessionKey);
            Session.Remove(SubmitIDSessionKey);

            this.DAL = new ZulassungDal(ref this.user, this.app, string.Empty, this);
            
            Response.Redirect(Page.Request.Url.AbsoluteUri);
        }

        protected void buttonChange_Click(object sender, EventArgs e)
        {
            ErrorDisplay.Visible = false;
            TabControl1.Visible = true;
            SuccessDisplay.Visible = false;
            TabControl1.SelectedIndex = 0;
            foreach (var step in TabControl1.Steps)
            {
                var wiz = step.Content as IWizardStep;

                if (wiz != null)
                {
                    wiz.ResetNavigation();
                }
            }
        }

        private string GenerateFileSuffix()
        {
            string suffix = "";

            string vertragsnummer = this.DAL.SelectedVehicles[0]["LIZNR"].ToString();
            string fin = this.DAL.SelectedVehicles[0]["CHASSIS_NUM"].ToString();
            string zb2 = this.DAL.SelectedVehicles[0]["TIDNR"].ToString();

            if (!String.IsNullOrEmpty(vertragsnummer))
            {
                suffix = vertragsnummer;
            }
            else if (!String.IsNullOrEmpty(fin))
            {
                suffix = fin;
            }
            else if (!String.IsNullOrEmpty(zb2))
            {
                suffix = zb2;
            }
            else
            {
                suffix = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            }

            return suffix;
        }

        protected void buttonPrint_Click(object sender, EventArgs e)
        {
            var dt = new System.Data.DataTable("dt");
            var wdf = new Base.Kernel.DocumentGeneration.WordDocumentFactory(dt, null);

            System.Data.DataTable kopf;
            var ds = this.DAL.GetPrintData(out kopf);

            string dateiSuffix = GenerateFileSuffix();

            wdf.CreateDocumentDataset("Order_" + dateiSuffix, this, @"\Components\Zulassung\Dokumente\Übersicht.doc", kopf, ds);
        }

        /// <summary>
        /// Mail mit PDF im Anhang an Kunden schicken
        /// </summary>
        /// <param name="empfaenger"></param>
        private void BestaetigungsmailVersenden(string empfaenger)
        {
            var dt = new System.Data.DataTable("dt");
            var wdf = new Base.Kernel.DocumentGeneration.WordDocumentFactory(dt, null);

            System.Data.DataTable kopf;
            var ds = this.DAL.GetPrintData(out kopf);

            string dateiSuffix = GenerateFileSuffix();

            string dateiname = wdf.CreateDocumentDatasetandSaveFile("Order_" + dateiSuffix, this, @"\Components\Zulassung\Dokumente\Übersicht.doc", kopf, ds);

            string smtpMailServer = System.Configuration.ConfigurationManager.AppSettings["SmtpMailServer"];
            string smtpMailSender = System.Configuration.ConfigurationManager.AppSettings["SmtpMailSender"];
            System.Net.Mail.SmtpClient client = new SmtpClient(smtpMailServer);
            string subject = "Zulassungsbeauftragung " + dateiSuffix + " , " + this.DAL.ZulassungsDate.ToShortDateString();

            StringBuilder textBuilder = new StringBuilder();
            textBuilder.AppendLine("Guten Tag,");
            textBuilder.Append(Environment.NewLine);
            textBuilder.AppendLine("anbei erhalten Sie die Bestätigung der Zulassungsbeauftragung (Zulassungsdatum: " 
                + this.DAL.ZulassungsDate.ToShortDateString() + ") für folgende Fahrzeuge:");
            textBuilder.Append(Environment.NewLine);
            for (int i = 0; i < this.DAL.SelectedVehicles.Count; i++)
            {
                DataRowView dRow = this.DAL.SelectedVehicles[i];
                textBuilder.AppendLine((i + 1).ToString() + ".  " + dRow["LIZNR"] + " / " + dRow["CHASSIS_NUM"] + " / " + dRow["TIDNR"]);
            }
            textBuilder.Append(Environment.NewLine);
            textBuilder.AppendLine("Mit freundlichen Grüßen");
            textBuilder.AppendLine("Christoph Kroschke GmbH /");
            textBuilder.AppendLine("Deutscher Auto Dienst GmbH");

            MailMessage eMail = new MailMessage(smtpMailSender, empfaenger, subject, textBuilder.ToString());
            eMail.Attachments.Add(new Attachment(dateiname));
            client.Send(eMail);
        }

        void OnWizStepCompleted(object sender, EventArgs e)
        {
            if (Session[SubmitIDSessionKey] != null)
                return;

            if (TabControl1.SelectedIndex == TabControl1.StepCount - 1)
            {
                var id = DAL.Submit();
                if (!string.IsNullOrEmpty(id))
                {
                    Session[SubmitIDSessionKey] = id;
                    labelOrderNo.Text = "Ihre Auftragsnummer lautet: " + id;
                    ErrorDisplay.Visible = false;
                    TabControl1.Visible = false;
                    SuccessDisplay.Visible = true;
                    Session.Remove(InitMultiSessionKey);
                }
                else
                {
                    if ((DAL.HalterAddress.Country.ToUpper() != "DE") && (DAL.Status == 0))
                    {
                        // Auslandszulassung ohne Fehler
                        Session[SubmitIDSessionKey] = null;
                        labelOrderNo.Text = "";
                        ErrorDisplay.Visible = false;
                        TabControl1.Visible = false;
                        SuccessDisplay.Visible = true;
                        Session.Remove(InitMultiSessionKey);
                    }
                    else
                    {
                        // Fehler bzw. deutsche Zulassung ohne Auftragsnummer (implizit auch ein Fehler)
                        labelError.Text = DAL.Message;
                        labelError.Text += DAL.ErrMsg;
                        ErrorDisplay.Visible = true;
                        TabControl1.Visible = false;
                        SuccessDisplay.Visible = false;
                    }
                }
                if (SuccessDisplay.Visible)
                {
                    // Bei Auslandszulassungen automatisch EMail an Kunden senden
                    if (DAL.HalterAddress.Country.ToUpper() != "DE")
                    {
                        string mailEmpfaenger = DAL.GetAuslZulMailadresse();
                        if (DAL.Status != 0)
                        {
                            labelError.Text = "Mailversand der Auftragsbestätigung fehlgeschlagen. " + DAL.Message;
                            ErrorDisplay.Visible = true;
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(mailEmpfaenger))
                            {
                                BestaetigungsmailVersenden(mailEmpfaenger);
                            }
                        }
                    }
                }
            }
            else
            {
                ErrorDisplay.Visible = false;
                TabControl1.Visible = true;
                SuccessDisplay.Visible = false;

                // Step forward
                TabControl1.SelectedIndex++;
            }
        }
    }
}