using System;

namespace GeneralTools.Contracts
{
    public interface IMaintenanceSecurityRuleDataProvider
    {
        string MaintenanceTitle { get; }

        string MaintenanceText { get; }

        DateTime MaintenanceStartDateTime { get; }

        DateTime MaintenanceEndDateTime { get; }

        bool MaintenanceLoginDisabled { get; }

        bool MaintenanceOnTestSystem { get; }

        bool MaintenanceOnProdSystem { get; }
    }
}
