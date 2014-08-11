using GeneralTools.Contracts;
using GeneralTools.Models;

namespace CkgDomainLogic.DomainCommon.Models
{
    public class CommonUiModel : IUiView
    {
        public int UiIndex { get; set; }

        public string GroupName { get; set; }

        public string SubGroupName { get; set; }

        public string Header { get; set; }

        private string _headerShort;
        public string HeaderShort
        {
            get { return _headerShort.IsNotNullOrEmpty() ? _headerShort : Header; }
            set { _headerShort = value; }
        }

        public string ViewName { get; set; }

        public bool IsMandatory { get; set; }

        public bool IgnoreSummaryItem { get; set; }

        public bool EditFromSummaryDisabled { get; set; }

        public virtual string GetSummaryString()
        {
            return "";
        }
    }
}