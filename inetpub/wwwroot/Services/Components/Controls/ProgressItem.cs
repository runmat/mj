using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace CKG.Components.Controls
{
    public class ProgressItem : Control, INamingContainer
    {
        private int currentStep;
        private int index = -1;
        private int stepCount;

        public int CurrentStep
        {
            get { return currentStep; }
            set { currentStep = value; }
        }        

        public int StepCount
        {
            get { return stepCount; }
            set { stepCount = value; }
        }        

        public int Index
        {
            get { return index; }
            set { index = value; }
        }
    }
}
