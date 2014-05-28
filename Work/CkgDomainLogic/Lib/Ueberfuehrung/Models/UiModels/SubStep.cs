using System.Collections.Generic;
using CkgDomainLogic.General.Models;

namespace CkgDomainLogic.Ueberfuehrung.Models
{
    public class SubStep
    {
        public string Title { get; set; }

        public string GroupName { get; set; }

        private List<UiModel> _stepForms = new List<UiModel>();
        public List<UiModel> StepForms
        {
            get { return _stepForms; }
            set { _stepForms = value; }
        }
    }
}
