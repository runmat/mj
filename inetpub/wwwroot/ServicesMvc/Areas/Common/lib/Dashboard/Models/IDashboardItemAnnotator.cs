using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.General.Database.Models
{
    public class DashboardItemAnnotator : IDashboardItemAnnotator
    {
        public IDashboardItem Parent { get; set; }

        public int ItemID { get; set; }

        public bool IsUserVisible { get; set; }

        public int UserSort { get; set; }
    }
}
