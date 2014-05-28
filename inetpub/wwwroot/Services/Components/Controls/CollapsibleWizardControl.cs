using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CKG.Components.Controls
{
    [ToolboxData("<{0}:CollapsibleWizardControl runat=server></{0}:CollapsibleWizardControl>")]
    [ParseChildren(ChildrenAsProperties = true)]
    [ViewStateModeById()]
    public class CollapsibleWizardControl : Control, INamingContainer, ICallbackEventHandler
    {
        private CollapsibleWizardStepCollection steps = new CollapsibleWizardStepCollection();
        private int selectedIndex;
        private int previousIndex;
        private string selectedStepChangedClientCallback;
        private bool navigateRequiredStepsOnly = true;
        private ITemplate wizardPageHeaderTemplate;
        private ITemplate wizardPageFooterTemplate;
        private ITemplate wizardStepHeaderTemplate;
        private ITemplate wizardStepSelectedHeaderTemplate;
        private string collapsibleContainerCssClass = "toggle_container";
        private string jQueryScriptIncludeUrl = Constants.JQueryUrl;
        private int toggleSpeed = 500;
        public event EventHandler<EventArgs> WizardCompleted;
        public event EventHandler<EventArgs> SelectedStepChanged;
        private List<CollapsibleWizardStepContainer> bindableControls = new List<CollapsibleWizardStepContainer>();


        const string ClientScript = @"$(document).ready(function(){{
                                        var index = {1};
                                        var container = $('#{6}').find('.{3}');
                                        container.not(':eq({2})').hide(); 
                                        if(index != {2}) 
                                        {{
                                            container.eq({2}).slideUp({5});
                                            container.eq(index).slideDown({5});
                                            {4}
                                        }}
                                    }});

                                    function {6}_UpdateWizard(result, context) 
                                    {{ 
                                        {6}_SelectWizardStep(result);
                                    }}

                                    function {6}_SelectWizardStep(index) 
                                    {{ 
                                        var container = $('#{6}').find('.{3}');
                                        $('#{0}').val(index); 
                                        container.not(':eq(' + index + ')').slideUp({5});
                                        container.eq(index).slideDown({5});
                                        {4}
                                    }}";

        const string SelectedIndexFieldName = "{0}_selectedIndex";

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                this.steps[this.selectedIndex].IsSelected = false;
                this.selectedIndex = value;
                this.steps[this.selectedIndex].IsSelected = true;
            }
        }

        public string SelectedStepChangedClientCallback
        {
            get { return selectedStepChangedClientCallback; }
            set { selectedStepChangedClientCallback = value; }
        }

        public string JQueryScriptIncludeUrl
        {
            get { return jQueryScriptIncludeUrl; }
            set { jQueryScriptIncludeUrl = value; }
        }

        public string CollapsibleContainerCssClass
        {
            get { return collapsibleContainerCssClass; }
            set { collapsibleContainerCssClass = value; }
        }

        public bool NavigateRequiredStepsOnly
        {
            get { return navigateRequiredStepsOnly; }
            set { navigateRequiredStepsOnly = value; }
        }

        public int ToggleSpeed
        {
            get { return toggleSpeed; }
            set { toggleSpeed = value; }
        }

        protected void OnWizardCompleted(EventArgs e)
        {
            if (WizardCompleted != null)
            {
                WizardCompleted(this, e);
            }
        }

        protected void OnSelectedStepChanged(EventArgs e)
        {
            if (SelectedStepChanged != null)
            {
                SelectedStepChanged(this, e);
            }
        }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public CollapsibleWizardStepCollection Steps
        {
            get { return this.steps; }
            set { this.steps = value; }
        }

        [TemplateContainer(typeof(CollapsibleWizardStepContainer))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate WizardPageHeaderTemplate
        {
            get { return wizardPageHeaderTemplate; }
            set { wizardPageHeaderTemplate = value; }
        }

        [TemplateContainer(typeof(CollapsibleWizardStepContainer))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate WizardPageFooterTemplate
        {
            get { return wizardPageFooterTemplate; }
            set { wizardPageFooterTemplate = value; }
        }

        [TemplateContainer(typeof(CollapsibleWizardStepContainer))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate WizardStepHeaderTemplate
        {
            get { return wizardStepHeaderTemplate; }
            set { wizardStepHeaderTemplate = value; }
        }

        [TemplateContainer(typeof(CollapsibleWizardStepContainer))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate WizardStepSelectedHeaderTemplate
        {
            get { return wizardStepSelectedHeaderTemplate; }
            set { wizardStepSelectedHeaderTemplate = value; }
        }

        public int StepCount
        {
            get { return steps.Count; }
        }

        public Control Content
        {
            get
            {
                if (SelectedIndex < StepCount)
                {
                    return Steps[SelectedIndex].Content;
                }
                return null;
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

        protected override void OnInit(EventArgs e)
        {
            // Let the page know this control needs the control state.
            Page.RegisterRequiresControlState(this);
            base.OnInit(e);

            RestoreClientState();

            var i = 0;
            foreach (var step in Steps)
            {
                try
                {
                    var userControl = Page.LoadControl(step.UserControl) as UserControl;
                    userControl.ID = string.Format("Step{0}Content", i);
                    // Remember wizard content control
                    step.Content = userControl;
                    i++;
                }
                catch (Exception ex)
                {
                    var errorMessage = new LiteralControl() { Text = string.Format("<pre>{0}</pre>", ex) };
                    step.Content = errorMessage;
                }
            }
        }

        protected override object SaveControlState()
        {
            return this.SelectedIndex;
        }

        protected override void LoadControlState(object savedState)
        {
            this.previousIndex = this.SelectedIndex = ((int?)savedState).GetValueOrDefault();
        }

        protected override object SaveViewState()
        {
            var first = base.SaveViewState();
            var second = ((IStateManager)this.steps).SaveViewState();

            if ((first != null) || (second != null))
            {
                return new Pair(first, second);
            }

            return null;
        }

        protected override void LoadViewState(object savedState)
        {
            if (savedState != null)
            {
                var pair = (Pair)savedState;
                base.LoadViewState(pair.First);
                ((IStateManager)this.steps).LoadViewState(pair.Second);
            }
        }

        protected override void TrackViewState()
        {
            base.TrackViewState();
            ((IStateManager)this.steps).TrackViewState();
        }

        public void NavigateForward(bool requiredStepsOnly)
        {
            this.previousIndex = this.SelectedIndex;

            for (var i = this.SelectedIndex + 1; i < this.StepCount; i++)
            {
                var step = steps[i];
                if ((!requiredStepsOnly || step.IsRequired) && step.Enabled)
                {
                    this.SelectedIndex = i;
                    OnSelectedStepChanged(EventArgs.Empty);
                    break;
                }
            }

            if (this.previousIndex == this.SelectedIndex)
            {
                OnWizardCompleted(EventArgs.Empty);
            }
        }

        public bool NavigateBackward(bool requiredStepsOnly)
        {
            this.previousIndex = this.SelectedIndex;

            for (var i = this.SelectedIndex - 1; i >= 0; i--)
            {
                var step = steps[i];
                if ((!requiredStepsOnly || step.IsRequired) && steps[i].Enabled)
                {
                    this.SelectedIndex = i;
                    OnSelectedStepChanged(EventArgs.Empty);
                    break;
                }
            }

            return this.previousIndex != this.SelectedIndex;
        }

        private Control InstantiateTemplate(ControlCollection controls, ITemplate template, string title, string selectionScript, int index, bool isRequired)
        {
            if (template != null)
            {
                var container = new CollapsibleWizardStepContainer();
                container.Title = title;
                container.Index = index;
                container.IsRequired = isRequired;
                container.SelectionScript = selectionScript;
                template.InstantiateIn(container);
                controls.Add(container);
                bindableControls.Add(container);
                return container;
            }
            return null;
        }

        public void Select(int index)
        {
            this.previousIndex = this.SelectedIndex = index;
        }

        protected override void OnPreRender(EventArgs e)
        {
            // Register client scripts
            if (!string.IsNullOrEmpty(jQueryScriptIncludeUrl))
            {
                Page.ClientScript.RegisterClientScriptInclude(Page.GetType(), Constants.JQueryScriptKey, jQueryScriptIncludeUrl.Replace("http", System.Web.HttpContext.Current.Request.Url.Scheme));
            }

            var indexVar = string.Format(SelectedIndexFieldName, this.ClientID);
            var clientCallback = !string.IsNullOrEmpty(selectedStepChangedClientCallback) ? selectedStepChangedClientCallback + "(index);" : string.Empty;
            Page.ClientScript.RegisterHiddenField(indexVar, this.SelectedIndex.ToString());
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), ClientID + "NavigateScript", string.Format(ClientScript, indexVar, this.SelectedIndex, previousIndex, collapsibleContainerCssClass, clientCallback, toggleSpeed, this.ClientID), true);

            var forwardScript = Page.ClientScript.GetCallbackEventReference(this, string.Format("$('#{0}').val() + ';forward'", indexVar), string.Format("{0}_UpdateWizard", this.ClientID), "null");
            var backwardScript = Page.ClientScript.GetCallbackEventReference(this, string.Format("$('#{0}').val() + ';backward'", indexVar), string.Format("{0}_UpdateWizard", this.ClientID), "null");

            foreach (var step in Steps)
            {
                var header = step.Header as CollapsibleWizardStepContainer;
                if (header != null)
                {
                    header.NavigateForwardScript = forwardScript;
                    header.NavigateBackwardScript = backwardScript;
                    if (header.Index == this.SelectedIndex && WizardStepSelectedHeaderTemplate != null)
                    {
                        WizardStepSelectedHeaderTemplate.InstantiateIn(step.Header);
                    }
                    else if (WizardStepHeaderTemplate != null)
                    {
                        WizardStepHeaderTemplate.InstantiateIn(step.Header);
                    }
                    header.DataBind();
                }
            }

            foreach (var control in bindableControls)
            {
                control.NavigateForwardScript = forwardScript;
                control.NavigateBackwardScript = backwardScript;

                control.DataBind();
            }

            // Damit JQuery Selector in _SelectWizardStep auch nicht aktive Steps 'findet'
            for (var i = this.Controls.Count - 1; i >= 0; i--)
            {
                var control = this.Controls[i];

                if ((control is Panel) && !control.Visible)
                {
                    var literal = new LiteralControl("<div class='" + this.CollapsibleContainerCssClass + "'></div>");
                    this.Controls.AddAt(i, literal);
                }
            }

            base.OnPreRender(e);
        }

        protected override void CreateChildControls()
        {
            for (var i = 0; i < Steps.Count; i++)
            {
                var step = Steps[i];

                var headerScript = string.Format("{0}_SelectWizardStep({1})", this.ClientID, i);

                // Create header
                var header = new CollapsibleWizardStepContainer();
                header.Title = step.Title;
                header.Index = i;
                header.IsRequired = step.IsRequired;
                header.SelectionScript = headerScript;
                header.ID = "header" + i;
                step.Header = header;
                Controls.Add(header);

                try
                {
                    // Create page
                    var container = new Panel();
                    container.CssClass = collapsibleContainerCssClass;
                    InstantiateTemplate(container.Controls, WizardPageHeaderTemplate, step.Title, headerScript, i, step.IsRequired);
                    container.Controls.Add(step.Content);
                    InstantiateTemplate(container.Controls, WizardPageFooterTemplate, step.Title, headerScript, i, step.IsRequired);
                    step.ContentPanel = container;
                    Controls.Add(container);
                }
                catch (Exception ex)
                {
                    var errorMessage = new LiteralControl() { Text = string.Format("<pre>{0}</pre>", ex) };
                    step.ContentPanel = errorMessage;
                    Controls.Add(errorMessage);
                }
            }

            base.CreateChildControls();
        }

        private void RestoreClientState()
        {
            // Restore client state
            var index = Page.Request.Form[string.Format(SelectedIndexFieldName, this.ClientID)];
            if (!string.IsNullOrEmpty(index))
            {
                this.previousIndex = this.SelectedIndex = int.Parse(index);
            }
        }

        public string GetCallbackResult()
        {
            return this.SelectedIndex.ToString();
        }

        public void RaiseCallbackEvent(string eventArgument)
        {
            var args = eventArgument.Split(';');
            this.SelectedIndex = int.Parse(args[0]);
            var action = args[1];
            if (action.Equals("forward", StringComparison.OrdinalIgnoreCase))
            {
                NavigateForward(NavigateRequiredStepsOnly);
            }
            else if (action.Equals("backward", StringComparison.OrdinalIgnoreCase))
            {
                NavigateBackward(NavigateRequiredStepsOnly);
            }
        }

        protected override void RenderChildren(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            base.RenderChildren(writer);
            writer.RenderEndTag();
        }
    }
}
