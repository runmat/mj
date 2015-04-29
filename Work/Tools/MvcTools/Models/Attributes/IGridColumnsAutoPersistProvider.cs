using MvcTools.Models;

namespace MvcTools.Contracts
{
    public interface IGridColumnsAutoPersistProvider
    {
        GridSettings GridCurrentSettingsAutoPersist { get; }

        void ResetGridCurrentModelTypeAutoPersist();
    }
}