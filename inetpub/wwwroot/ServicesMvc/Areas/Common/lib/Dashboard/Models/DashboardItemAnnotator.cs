using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.General.Models
{
    public class DashboardItemAnnotator : IDashboardItemAnnotator
    {
        public int ItemID { get; set; }

        public bool IsUserVisible { get; set; }

        public int UserSort { get; set; }

        public int RowSpanOverride { get; set; }
    }
}