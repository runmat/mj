using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CKG.Components.Controls
{
    [DefaultProperty("Steps")]
    [ToolboxData("<{0}:WizardControl runat=server></{0}:WizardControl>")]
    [ParseChildren(ChildrenAsProperties=true)]
    public class WizardControl : Control, INamingContainer
    {
        public event EventHandler<EventArgs> SelectedStepChanged;

        private List<WizardStep> steps = new List<WizardStep>();
        private bool isDone;
        private int selectedIndex;
        private List<Control> bindableControls = new List<Control>();

        public int StepCount { get { return this.steps.Count; } }

        public int SelectedIndex
        {
            get { return this.selectedIndex; }
            set 
            {
                // Step.IsSelected -> event for step activated / deactivated
                this.selectedIndex = Math.Min(value, this.StepCount - 1); 
            }
        }

        public bool IsDone { get { return this.isDone; } set { this.isDone = value; } }

        [PersistenceMode(PersistenceMode.Attribute)]
        public bool DisableFutureSteps { get; set; }

        [TemplateContainer(typeof(WizardStepInfoContainer))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate HeaderTemplate { get; set; }

        [TemplateContainer(typeof(WizardStepInfoContainer))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate FooterTemplate { get; set; }

        [TemplateContainer(typeof(WizardStepInfoContainer))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate WizardNavigationHeaderTemplate { get; set; }

        [TemplateContainer(typeof(WizardStepInfoContainer))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate WizardNavigationFooterTemplate { get; set; }

        [TemplateContainer(typeof(WizardStepInfoContainer))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate WizardPageHeaderTemplate { get; set; }

        [TemplateContainer(typeof(WizardStepInfoContainer))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate WizardPageFooterTemplate { get; set; }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public List<WizardStep> Steps
        {
            get { return this.steps; }
            set { this.steps = value; }
        }

        public WizardStep CurrentStep
        {
            get
            {
                if (StepCount > SelectedIndex)
                {
                    return Steps[SelectedIndex];
                }
                return null;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // Let the page know this control needs the control state.
            Page.RegisterRequiresControlState(this);

            // Preload wizard navigation and contents
            for (var i = 0; i < this.StepCount; i++)
            {
                var step = this.Steps[i];
                step.Index = i;
                step.Click += this.OnSelectedStepChanged;

                var header = new LinkButton() { Text = step.Title };
                header.CausesValidation = false;

                // Remember wizard header control
                step.Header = header;

                try
                {
                    var userControl = Page.LoadControl(step.UserControl);
                    userControl.ID = string.Format("Step{0}Content", i);
                    // Remember wizard content control
                    step.Content = userControl;
                }
                catch (Exception ex)
                {
                    var errorMessage = new LiteralControl() { Text = string.Format("<pre>{0}</pre>", ex) };
                    step.Content = errorMessage;
                }
            }
        }

        public override ControlCollection Controls
        {
            get
            {
                this.EnsureChildControls();
                return base.Controls;
            }
        }
 
        protected override object SaveControlState()
        {
            var cs = base.SaveControlState();
            
            if (cs != null || this.selectedIndex != 0 || this.isDone)
            {
                return new Triplet(cs, this.selectedIndex, this.isDone);
            }

            return null;
        }

        protected override void LoadControlState(object savedState)
        {
            var pair = savedState as Triplet;

            if (pair != null)
            {
                base.LoadControlState(pair.First);
                this.selectedIndex = ((int?)pair.Second).GetValueOrDefault();
                this.isDone = ((bool?)pair.Third).GetValueOrDefault();
            }
            else
            {
                base.LoadControlState(null);
                this.selectedIndex = 0;
                this.isDone = false;
            }
        }

        protected override void CreateChildControls()
        {
            if (HeaderTemplate != null)
            {
                var header = new WizardStepInfoContainer(this);
                HeaderTemplate.InstantiateIn(header);
                bindableControls.Add(header);
                Controls.Add(header);
            }

            if (WizardNavigationHeaderTemplate != null)
            {
                var headerHead = new WizardStepInfoContainer(this);
                WizardNavigationHeaderTemplate.InstantiateIn(headerHead);
                bindableControls.Add(headerHead);
                Controls.Add(headerHead);
            }

            // Add wizard navigation
            foreach (var step in Steps)
            {
                Controls.Add(step.Header);
            }

            if (WizardNavigationFooterTemplate != null)
            {
                var footer = new WizardStepInfoContainer(this);
                WizardNavigationFooterTemplate.InstantiateIn(footer);
                bindableControls.Add(footer);
                Controls.Add(footer);
            }

            if (WizardPageHeaderTemplate != null)
            {
                var header = new WizardStepInfoContainer(this);
                WizardPageHeaderTemplate.InstantiateIn(header);
                bindableControls.Add(header);
                Controls.Add(header);
            }

            // Add wizard contents
            foreach (var step in Steps)
            {
                Controls.Add(step.Content);
            }

            if (WizardPageFooterTemplate != null)
            {
                var footer = new WizardStepInfoContainer(this);
                WizardPageFooterTemplate.InstantiateIn(footer);
                bindableControls.Add(footer);
                Controls.Add(footer);
            }

            if (FooterTemplate != null)
            {
                var footer = new WizardStepInfoContainer(this);
                FooterTemplate.InstantiateIn(footer);
                bindableControls.Add(footer);
                Controls.Add(footer);
            }

            base.CreateChildControls();
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (steps.Count > selectedIndex)
            {
                steps[selectedIndex].IsSelected = true;
            }

            foreach (var control in bindableControls)
            {
                control.DataBind();
            }

            for (var i = 0; i < StepCount; i++)
            {
                var step = Steps[i];
                if (step.IsSelected)
                {
                    step.Header.CssClass = step.SelectedHeaderCssClass;
                    step.Content.Visible = true;
                }
                else
                {
                    if (!string.IsNullOrEmpty(step.VisitedHeaderCssClass) && i < SelectedIndex)
                    {
                        step.Header.CssClass = step.VisitedHeaderCssClass;
                    }
                    else
                    {
                        step.Header.CssClass = step.HeaderCssClass;
                    }
                    step.Content.Visible = false;
                }

                step.Header.Enabled = !isDone && (i <= selectedIndex || !DisableFutureSteps);

            }
            base.OnPreRender(e);
        }

        protected void OnSelectedStepChanged(object sender, WizardStepSelectedEventArgs e)
        {
            this.SelectedIndex = e.Index;

            var eh = this.SelectedStepChanged;

            if (eh != null)
            {
                eh(this, e);
            }
        }

        protected override void OnDataBinding(EventArgs e)
        {
            EnsureChildControls();
            base.OnDataBinding(e);
        }
    }
}
