using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Components.Zulassung.DAL;
using System.Data;
using System.Text;

namespace CKG.Components.Zulassung.UserControls
{
    public partial class WizardStep3 : System.Web.UI.UserControl, IWizardStep
    {
        protected const string ValidationGroup = "ZulassungStep3";

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
            Wizard.WizardCompleted += OnWizardCompleted;
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
            if (Wizard.SelectedIndex > 1)
            {
                
                Wizard.NavigateBackward(true);
            }
            else
            {
                OnNavigateBack(this, EventArgs.Empty);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            UpdatePanel2.Update();
            base.OnPreRender(e);
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
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