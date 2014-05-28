using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Components.Zulassung.DAL;

namespace CKG.Components.Zulassung.UserControls
{
    public interface IStep2
    {
        string Postcode { get; set; }
        string Town { get; set; }
    }

    public partial class WizardStep2 : System.Web.UI.UserControl, IWizardStep, IStep2
    {
        private IWizardPage page;
        protected const string ValidationGroup = "ZulassungStep2";

        public string Postcode { get; set; }
        public string Town { get; set; }

        public void Validate()
        {
            Page.Validate(ValidationGroup);
        }

        public void ResetNavigation()
        {
            Wizard.SelectedIndex = 0;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            page = (IWizardPage)Page;
            Wizard.WizardCompleted += OnWizardCompleted;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public event EventHandler<EventArgs> Completed;

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


        public event EventHandler<EventArgs> NavigateBack;

        void OnNavigateBack(object sender, EventArgs e)
        {
            if (NavigateBack != null)
            {
                NavigateBack(this, EventArgs.Empty);
            }
        }


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

        protected void ButtonSetDefaultData_Click(object sender, EventArgs e)
        {
            page.DAL.SetDefaultData();
        }
    }
}