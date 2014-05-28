using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Components.Zulassung.DAL;

namespace CKG.Components.Zulassung.UserControls
{
    public partial class WizardStep4 : System.Web.UI.UserControl, IWizardStep
    {
        protected const string ValidationGroup = "ZulassungStep4";
        protected Controls.CollapsibleWizardControl myWizard;

        private IWizardPage page;

        public void Validate()
        {
            Page.Validate(ValidationGroup);
        }

        public void ResetNavigation()
        {
            Wizard.SelectedIndex = 0;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Label1.Visible = false;
            page = (IWizardPage)Page;
            Wizard.WizardCompleted += OnWizardCompleted;
            Wizard.SelectedStepChanged += new EventHandler<EventArgs>(Wizard_SelectedStepChanged); 

        }

        void Wizard_SelectedStepChanged(object sender, EventArgs e)
        {

        }

        void OnWizardCompleted(object sender, EventArgs e)
        {

            // check for invalid steps.
            for (var i = 0; i < Wizard.StepCount; i++)
            {
                var step = Wizard.Steps[i].Content as IWizardStepPart;
                if (step != null)
                {
                    step.Validate();
                    if (!Page.IsValid)
                    {
                        Wizard.SelectedIndex = i;
                        return;
                    }
                    else
                    {
                        // store data in session.
                        step.Save();
                    }
                }
            }

            if (Completed != null)
            {
                Completed(this, EventArgs.Empty);
            }
        }

        public event EventHandler<EventArgs> Completed;

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            if (Wizard.SelectedIndex > 0)
            {
                Wizard.NavigateBackward(true);
            }
            else
            {
                OnNavigateBack(this, EventArgs.Empty);
            }
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            Label1.Visible = false; 
            //Prüfen ob Regulierer angegeben wurde (Wenn step2 oder step3 aktiv)
            if ((Wizard.SelectedIndex == 1 || Wizard.SelectedIndex == 2) && (string.IsNullOrEmpty(page.DAL.Regulierer)))
            {
                Label1.Text = "Bitte erst einen Rechnungszahler auswählen.";
                Label1.Visible = true;
                return;
            }

            //Prüfen ob Empfänger angegeben wurde (Wenn step3 aktiv)
            if ((Wizard.SelectedIndex == 2) && (string.IsNullOrEmpty(page.DAL.Empfänger)))
            {
                Label1.Text = "Bitte erst eine abweichende Rechnungsadresse auswählen.";
                Label1.Visible = true;
                return;
            }

            var step = Wizard.Content as IWizardStepPart;

            if (step != null)
            {
                step.Validate();
                if (Page.IsValid)
                {
                    Wizard.NavigateForward(true);
                }
            }


        }

        protected override void OnPreRender(EventArgs e)
        {

            UpdatePanel2.Update();
            base.OnPreRender(e);
        }

        public event EventHandler<EventArgs> NavigateBack;

        void OnNavigateBack(object sender, EventArgs e)
        {
            if (NavigateBack != null)
            {
                NavigateBack(this, EventArgs.Empty);
            }
        }
    }
}