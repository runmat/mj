using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace CkgDomainLogic.Ueberfuehrung.ViewModels
{
    public partial class UeberfuehrungViewModel
    {
        #region Step Functionality

        [XmlIgnore]
        public int StepMax { get { return 4; } }

        private int _step = 1;
        [XmlIgnore]
        public int Step
        {
            get { return _step; }
            set { _step = value; }
        }

        private int _subStep = 1;
        [XmlIgnore]
        public int SubStep
        {
            get { return _subStep; }
            set { _subStep = value; }
        }

        [XmlIgnore]
        public int GlobalSubStep
        {
            get { return Step + (SubStep - 1); }
        }

        void StepMoveWithSubStep(int direction)
        {
            if (direction < 0)
            {
                if (Step == 1 && SubStep == 1)
                    return;
                if (SubStep > 1)
                    SubStep--;
                else
                {
                    Step--;
                    SubStep = Steps[Step-1].SubStepMax;
                }
            }
            else
            {
                if (Step == StepMax && SubStep == Steps[Step-1].SubStepMax)
                    return;
                if (SubStep < Steps[Step-1].SubStepMax)
                    SubStep++;
                else
                {
                    Step++;
                    SubStep = 1;
                }
            }

            Steps.ToList().ForEach(s => s.CurrentSubStep = 1);
            Steps[Step-1].CurrentSubStep = SubStep;
        }

        public bool MoveToStep(int increment)
        {
            StepMoveWithSubStep(increment);

            return NavigateToStep(Step, true, false);
        }

        public bool NavigateToStep(int stepWanted, bool enableStepPreparing = true, bool resetSubStep = true)
        {
            if (stepWanted < 1 || stepWanted > StepMax)
                return false;

            // prepare step we want to navigate to: 
            if (enableStepPreparing)
                StepFormsCollectionPrepareStep(stepWanted);
            
            Step = stepWanted;
            if (resetSubStep)
                SubStep = 1;

            return true;
        }

        public bool HandleStepIncrement(int increment)
        {
            if (FromSummaryChangeStep == 0)
            {
                if (!MoveToStep(increment))
                    return false;

                return true;
            }

            // "Change From Summary" handling here:
            //
            // we come directly from summary ... only with the purpose to change just one step
            // so let's check here if the changed data inside this step also affects other steps...
            // => in that case we must prevent navigating directly back to the summary from this step.

            // once again, re-prepare all steps starting from the current step:  (Property "Step")
            var stepForcedToNavigate = 0;

            if (!FromSummaryMajorChangeOccured)
            {
                // validate all steps, except the summary itself!
                for (var i = Step; i <= StepMax - 1; i++)  
                {
                    StepFormsCollectionPrepareStep(i);
                    if (FromSummaryMajorChangeOccured)
                    {
                        // step "i" has problems, so let's move to him:
                        stepForcedToNavigate = i;
                        break;
                    }
                }
            }

            FromSummaryChangeStep = 0;

            var majorStepChanges = FromSummaryMajorChangeOccured;
            FromSummaryMajorChangeOccured = false;
            if (majorStepChanges)
            {
                if (!NavigateToStep(stepForcedToNavigate, false))
                    return false;

                return true;
            }

            // no major changes detected inside this step!
            // ==> Let's go directly back to the summary!  (Zurück zur Übersicht)
            if (!NavigateToStep(StepMax))
                return false;
        
            return true;
        }


        #region Misc

        [XmlIgnore]
        public string StepBarID { get { return string.Format("stepbar{0}", Step); } }

        /// a custom error message to prevent coming back to summary meanwhile major changes occured in step "FromSummaryChangeStep"
        /// i. e. if the user comes from summary and changes data of one step that also affects other steps, then ...
        ///       => a direct return to the summary should be forbidden, because the affected other steps then should be edited at first
        [XmlIgnore]
        public bool FromSummaryMajorChangeOccured { get; set; }

        /// the step that is directly navigated to, coming from summary
        [XmlIgnore]
        public int FromSummaryChangeStep { get; set; }

        [XmlIgnore]
        public string StepPrevButtonText
        {
            get
            {
                if (Step == 1) return "Abbrechen";

                return "Zurück";
            }
        }

        [XmlIgnore]
        public string StepNextButtonText
        {
            get
            {
                if (Step == StepMax) return "Auftrag senden";

                return FromSummaryChangeStep > 0 ? "Zur Übersicht" : StepExplicitNextButtonText;
            }
        }

        [XmlIgnore]
        public string StepExplicitNextButtonText
        {
            get { return "Weiter"; }
        }

        [XmlIgnore]
        public bool StepExplicitNextButtonVisible
        {
            get { return FromSummaryChangeStep > 0 && Step == 3 && StepModel.SubSteps.Count > 1; }
        }

        private static readonly string[] StepBarHeadersAbbreviates = {
                                                        "Stammdaten", 
                                                        "Fahrten", 
                                                        "Dienstleistungen", 
                                                        "Übersicht"
                                                    };
        public string StepBarHeaderAbbreviate { get { return StepBarHeadersAbbreviates[Step - 1]; } }

        private static readonly string[] StepBarHeaders = {
                                                        "Fahrzeugstammdaten eingeben", 
                                                        "Festlegung der Fahrten", 
                                                        "Leistungen für {0}", 
                                                        "Übersicht & Absenden"
                                                    };
        
        //public string StepBarHeader { get { return StepBarHeaders[Step - 1]; } }
        [XmlIgnore]
        public string StepBarHeader
        {
            get
            {
                var header = StepBarHeaders[Step - 1];

                if (Step == 3)
                    header = string.Format(header, StepModel.CurrentSubStepModel.Title);

                return header;
            }
        }

        public IHtmlString GetStepBarHeaderForStep(int stepToDisplay)
        {
            var header = StepBarHeadersAbbreviates[stepToDisplay - 1];
            var outerHeader = string.Format("{0}. {1}", stepToDisplay, header);
            string html;
            if (stepToDisplay < Step)
                html = string.Format("<a href=\"{1}\">{0}</a>", outerHeader, GetStepBarHeaderLinkForStep(stepToDisplay));
            else
                html = string.Format("{0}", outerHeader);

            // use "HtmlString" here to ensure proper raw html decoding:
            return new HtmlString(html);
        }

        private static string GetStepBarHeaderLinkForStep(int stepToDisplay)
        {
            return string.Format("javascript:NavigateToStep({0});", stepToDisplay);
        }

        [XmlIgnore]
        public string StepPartialViewName { get { return string.Format("Partial/ViewStep"); } }

        #endregion

        #endregion
    }
}