using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Web.UI;

namespace CKG.Components.Controls
{
    [DefaultProperty("Tabs")]
    [ToolboxData("<{0}:ProgressIndicator runat=server></{0}:ProgressIndicator>")]
    [ParseChildren(ChildrenAsProperties = true)]
    public class ProgressIndicator : Control, INamingContainer
    {
        private int stepCount;
        private int currentIndex;
        private ITemplate headerTemplate;
        private ITemplate footerTemplate;
        private ITemplate completeStepTemplate;
        private ITemplate incompleteStepTemplate;

        public int StepCount
        {
            get { return stepCount; }
            set { stepCount = value; }
        }

        public int CurrentIndex
        {
            get { return currentIndex; }
            set { currentIndex = value; }
        }

        [TemplateContainer(typeof(ProgressItem))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate HeaderTemplate
        {
            get { return headerTemplate; }
            set { headerTemplate = value; }
        }

        [TemplateContainer(typeof(ProgressItem))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate FooterTemplate
        {
            get { return footerTemplate; }
            set { footerTemplate = value; }
        }

        [TemplateContainer(typeof(ProgressItem))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate CompleteStepTemplate
        {
            get { return completeStepTemplate; }
            set { completeStepTemplate = value; }
        }

        [TemplateContainer(typeof(ProgressItem))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate IncompleteStepTemplate
        {
            get { return incompleteStepTemplate; }
            set { incompleteStepTemplate = value; }
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (HeaderTemplate != null)
            {
                var header = new ProgressItem();
                header.CurrentStep = currentIndex;
                header.StepCount = stepCount;
                HeaderTemplate.InstantiateIn(header);
                Controls.Add(header);
                header.DataBind();
            }

            for (int i = 0; i < stepCount; i++)
            {
                if (i <= currentIndex)
                {
                    if (CompleteStepTemplate != null)
                    {
                        var step = new ProgressItem();
                        step.CurrentStep = currentIndex;
                        step.StepCount = stepCount;
                        step.Index = i;
                        CompleteStepTemplate.InstantiateIn(step);
                        Controls.Add(step);
                        step.DataBind();
                    }
                }
                else
                {
                    if (IncompleteStepTemplate != null)
                    {
                        var step = new ProgressItem();
                        step.CurrentStep = currentIndex;
                        step.StepCount = stepCount;
                        step.Index = i;
                        IncompleteStepTemplate.InstantiateIn(step);
                        Controls.Add(step);
                        step.DataBind();
                    }
                }
            }

            if (FooterTemplate != null)
            {
                var footer = new ProgressItem();
                footer.CurrentStep = currentIndex;
                footer.StepCount = stepCount;
                FooterTemplate.InstantiateIn(footer);
                Controls.Add(footer);
                footer.DataBind();
            }
            base.OnPreRender(e);
        }

    }
}
