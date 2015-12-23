namespace CkgDomainLogic.General.Contracts
{
    public interface IDashboardItem
    {
        int ID { get; set; }

        string ItemKey { get; set; }

        string Title { get; set; }

        string RelatedAppUrl { get; set; }

        string RelatedSelectorModel { get; set; }

        string ChartJsonOptions { get; set; }

        string ChartJsonDataCustomizingScriptFunction { get; set; }

        string ItemOptions { get; set; }

        IDashboardItemOptions Options { get; }

        int? InitialSort { get; set; }

        int UserSort { get; }

        bool IsUserVisible { get; }

        bool IsChart { get; }

        bool IsPartialView { get; }

        IDashboardItemAnnotator ItemAnnotator { get; set; }
    }
}
