using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.General.Models;
using System.Linq;
using GeneralTools.Models;

namespace CkgDomainLogic.Ueberfuehrung.Models
{
    public class Step
    {
        public string GroupName { get; set; }

        private List<SubStep> _subSteps = new List<SubStep>();
        public List<SubStep> SubSteps
        {
            get { return _subSteps; }
            set { _subSteps = value; }
        }

        [XmlIgnore]
        public int SubStepMax { get { return SubSteps.Count; } }

        private int _currentSubStep = 1;

        [XmlIgnore]
        public int CurrentSubStep
        {
            get { return _currentSubStep; }
            set { _currentSubStep = value; }
        }

        [XmlIgnore]
        public SubStep CurrentSubStepModel
        {
            get
            {
                if (SubSteps.None())
                    SubSteps.Add(new SubStep());
                return SubSteps[CurrentSubStep - 1];
            }
        }

        [XmlIgnore]
        public List<UiModel> CurrentSubStepForms { get { return CurrentSubStepModel.StepForms; } }

        [XmlIgnore]
        public List<UiModel> AllSubStepForms { get { return SubSteps.SelectMany(subStep => subStep.StepForms).ToList(); } }
    }
}
