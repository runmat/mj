namespace CkgDomainLogic.General.Contracts
{
    public interface IDashboardItem
    {
        int ID { get; set; }

        string Title { get; set; }

        string RelatedAppUrl { get; set; }

        string RelatedSelectorModel { get; set; }

        string ChartJsonOptions { get; set; }

        bool IsUserVisible { get; set; }

        int UserSort { get; set; }
    }
}
