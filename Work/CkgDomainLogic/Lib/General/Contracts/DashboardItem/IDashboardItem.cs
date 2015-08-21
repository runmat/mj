namespace CkgDomainLogic.General.Contracts
{
    public interface IDashboardItem
    {
        int ID { get; set; }

        string Title { get; set; }

        string RelatedAppUrl { get; set; }

        string RelatedSelectorModel { get; set; }

        string ChartJsonOptions { get; set; }

        string ChartJsonDataCustomizingScriptFunction { get; set; }

        int? InitialSort { get; set; }

        int UserSort { get; }

        bool IsUserVisible { get; }

        IDashboardItemAnnotator ItemAnnotator { get; set; }
    }
}
