namespace CkgDomainLogic.General.Contracts
{
    public interface IDashboardItemOptions
    {
        int ColumnSpan { get; set; }

        bool IsAuthorized { get; set; }
    }
}
