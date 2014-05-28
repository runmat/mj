using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace CKG.Components.Controls
{
    public class WizardStepInfoContainer : Control, INamingContainer 
    {
        public WizardStepInfoContainer(WizardControl wizardControl)
        {
            this.wizardControl = wizardControl;
        }

        private WizardControl wizardControl;

        public int SelectedIndex
        {
            get { return wizardControl.SelectedIndex; }
        }

        public int StepCount
        {
            get { return wizardControl.StepCount; }
        }
    }
}
