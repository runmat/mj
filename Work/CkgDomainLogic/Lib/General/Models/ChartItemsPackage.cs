// ReSharper disable InconsistentNaming

using GeneralTools.Models;

namespace CkgDomainLogic.General.Models
{
    public class ChartItemsPackage : JsonItemsPackage
    {

        public string[] labels { get; set; }

        public ChartItemsTick[] ticks { get; set; }

        public string options { get; set; }

        public string dashboardItemOptions { get; set; }

        public string customscriptfunction { get; set; }
    }
}
