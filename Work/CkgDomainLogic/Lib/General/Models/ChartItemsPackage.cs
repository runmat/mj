// ReSharper disable InconsistentNaming

using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.General.Models
{
    public class ChartItemsPackage 
    {
        public object data { get; set; }

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
