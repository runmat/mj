// ReSharper disable InconsistentNaming

using System.Xml.Serialization;
using GeneralTools.Services;

namespace CkgDomainLogic.General.Models
{
    public class ChartItemsPackage : Store
    {
        public string ID { get; set; }

        [XmlIgnore]
        public object data { get; set; }

        public string dataAsText { get; set; }

        public string[] labels { get; set; }

        public ChartItemsTick[] ticks { get; set; }

        public string options { get; set; }

        public string dashboardItemOptions { get; set; }

        public string customscriptfunction { get; set; }
    }

    public class ChartItemsTick
    {
        public double Pos { get; set; }

        public string Label { get; set; }
    }
}
