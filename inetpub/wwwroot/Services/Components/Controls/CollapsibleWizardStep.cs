using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CKG.Components.Controls
{
    public class CollapsibleWizardStep : IStateManager
    {
        private string title;
        private string userControl;
        private Control contentPanel;
        private Control header;
        private bool isSelected;
        private bool isRequired;
        private Control content;
        private bool isTrackingViewState;
        private StateBag viewState;

        public Control Content
        {
            get { return content; }
            set { content = value; }
        }

        public bool IsRequired
        {
            get { return isRequired; }
            set { isRequired = value; }
        }

        public bool IsSelected
        {
            get { return isSelected; }
            internal set { isSelected = value; }
        }

        internal Control ContentPanel
        {
            get { return contentPanel; }
            set
            {
                contentPanel = value;
                contentPanel.Visible = this.Enabled;
            }
        }

        internal Control Header
        {
            get { return header; }
            set
            {
                header = value;
                header.Visible = this.Enabled;
            }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public bool Enabled
        {
            get
            {
                return ((bool?)this.ViewState["Enabled"]).GetValueOrDefault(true);
            }

            set
            {
                this.ViewState["Enabled"] = value;

                if (this.ContentPanel != null)
                {
                    this.ContentPanel.Visible = value;

                    if (!value && this.IsSelected)
                    {
                        var wiz = (CollapsibleWizardControl)this.ContentPanel.Parent;
                        if (wiz.SelectedIndex == wiz.StepCount - 1)
                        {
                            wiz.NavigateBackward(false);
                        }
                        else
                        {
                            wiz.NavigateForward(false);
                        }
                    }
                }

                if (this.Header != null)
                {
                    this.Header.Visible = value;
                }
            }
        }

        public string UserControl
        {
            get { return userControl; }
            set { userControl = value; }
        }

        protected StateBag ViewState
        {
            get
            {
                if (this.viewState == null)
                {
                    this.viewState = new StateBag(false);

                    if (this.isTrackingViewState)
                    {
                        ((IStateManager)this.viewState).TrackViewState();
                    }
                }

                return this.viewState;
            }
        }

        internal void SetDirty()
        {
            if (this.viewState != null)
            {
                this.viewState.SetDirty(true);
            }
        }

        bool IStateManager.IsTrackingViewState
        {
            get { return this.isTrackingViewState; }
        }

        void IStateManager.LoadViewState(object state)
        {
            if (state != null)
            {
                ((IStateManager)this.ViewState).LoadViewState(state);
            }
        }

        object IStateManager.SaveViewState()
        {
            if (this.viewState != null)
            {
                return ((IStateManager)this.ViewState).SaveViewState();
            }

            return null;
        }

        void IStateManager.TrackViewState()
        {
            this.isTrackingViewState = true;

            if (this.viewState != null)
            {
                ((IStateManager)this.viewState).TrackViewState();
            }
        }
    }
}
