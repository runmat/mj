namespace GeneralTools.Contracts
{
    public interface IMaintenanceSecurityRuleDataProvider
    {
        string MaintenanceTitle { get; }

        string MaintenanceText { get; }

        bool MaintenanceOnTestSystem { get; }

        bool MaintenanceOnProdSystem { get; }

        bool MaintenanceLoginDisabled { get; }

        bool MaintenanceShow { get; }

        bool MaintenanceShowAndLetConfirmMessageAfterLogin { get; }
    }
}
