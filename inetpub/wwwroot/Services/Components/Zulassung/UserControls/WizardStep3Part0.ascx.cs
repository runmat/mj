using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CKG.Components.Zulassung.UserControls
{
    public partial class WizardStep3Part0 : System.Web.UI.UserControl, IWizardStepPart
    {
        private Zulassung.DAL.ZulassungDal dal;
        private Controls.CollapsibleWizardControl myWizard;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.dal = ((IWizardPage)this.Page).DAL;
            this.myWizard = (Controls.CollapsibleWizardControl)this.Parent.Parent;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.SetBuchungscodeVisibility();
        }

        public void Validate()
        {
            Page.Validate("ZulassungStep3Part0");
        }

        public void Save()
        {
            this.dal.Buchungscode = this.txtBuchungscode.Text;
        }

        private void SetBuchungscodeVisibility()
        {
            {
                // Wie im Base.Kernel.Common.Common.SetEndASPXAccess
                // Feldübersetzungen für Buchungscode erzwingen
                var appKey = System.Configuration.ConfigurationManager.AppSettings["ApplicationKey"];
                var appUrl = Page.Request.Url.LocalPath.Replace("/" + appKey + "", "..");
                var translations = (System.Data.DataTable)this.Session[appUrl];

                CKG.Base.Kernel.Common.Common.CheckAllControl(this, translations);
            }

            // Wenn Buchungscode nicht sichtbar ist, Wizardstep ausblenden
            var vis = this.lbl_Buchungscode.Visible;
            this.myWizard.Steps[0].Enabled = vis;
        }
    }
}