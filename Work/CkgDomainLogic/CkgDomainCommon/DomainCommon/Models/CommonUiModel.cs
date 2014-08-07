using System.Xml.Serialization;
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

        [XmlIgnore]
        public virtual GeneralEntity SummaryItem
        {
            get { return new GeneralEntity(); }
        }

        public bool IgnoreSummaryItem { get; set; }
    }
}