using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace CKG.Components.Controls
{
    public class CollapsibleWizardStepContainer : Control, INamingContainer
    {
        private string navigateForwardScript;
        private string navigateBackwardScript;
        private string selectionScript;
        private string title;
        private int index;
        private bool isRequired;

        public bool IsRequired
        {
            get { return isRequired; }
            internal set { isRequired = value; }
        }

        public string NavigateForwardScript
        {
            get { return navigateForwardScript; }
            internal set { navigateForwardScript = value; }
        }        

        public string NavigateBackwardScript
        {
            get { return navigateBackwardScript; }
            internal set { navigateBackwardScript = value; }
        }

        public string SelectionScript
        {
            get { return selectionScript; }
            internal set { selectionScript = value; }
        }
        
        public string Title
        {
            get { return title; }
            internal set { title = value; }
        }
        
        public int Index
        {
            get { return index; }
            internal set { index = value; }
        }
    }
}
