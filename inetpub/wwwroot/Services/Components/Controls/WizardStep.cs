using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CKG.Components.Controls
{
    public sealed class WizardStepSelectedEventArgs : EventArgs
    {
        public WizardStepSelectedEventArgs(int index)
        {
            this.Index = index;
        }

        public int Index { get; private set; }
    }

    public class WizardStep
    {
        private LinkButton header;

        internal event EventHandler<WizardStepSelectedEventArgs> Click;

        public bool IsSelected { get; internal set; }

        public int Index { get; internal set; }

        public Control Content { get; internal set; }

        public LinkButton Header
        {
            get { return this.header; }

            internal set 
            {
                if (this.header != value)
                {
                    if (this.header != null)
                    {
                        this.header.Click -= this.OnHeaderClick;
                    }

                    if (value != null)
                    {
                        value.Click += this.OnHeaderClick;
                    }

                    this.header = value;
                }
            }
        }

        private void OnHeaderClick(object sender, EventArgs e)
        {
            var eh = this.Click;

            if (eh != null)
            {
                eh(this, new WizardStepSelectedEventArgs(this.Index));
            }
        }

        public string HeaderCssClass { get; set; }

        public string SelectedHeaderCssClass { get; set; }

        public string VisitedHeaderCssClass { get; set; }

        public string Title { get; set; }

        public string UserControl { get; set; }
    }
}
