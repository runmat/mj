// ReSharper disable InconsistentNaming

namespace CkgDomainLogic.General.Models
{
    public class ChartItemsPackage 
    {
        public object data { get; set; }

        public string[] labels { get; set; }

        public ChartItemsTick[] ticks { get; set; }
    }

    public class ChartItemsTick
    {
        public double Pos { get; set; }
        public string Label { get; set; }
    }
}
