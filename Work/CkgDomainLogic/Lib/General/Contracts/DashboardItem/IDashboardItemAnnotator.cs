namespace CkgDomainLogic.General.Contracts
{
    public interface IDashboardItemAnnotator
    {
        int ItemID { get; set; }

        bool IsUserVisible { get; set; }

        int UserSort { get; set; }

        int RowSpanOverride { get; set; }
    }
}
