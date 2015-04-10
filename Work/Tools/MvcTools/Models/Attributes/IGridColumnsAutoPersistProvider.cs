using MvcTools.Models;

namespace MvcTools.Contracts
{
    public interface IGridColumnsAutoPersistProvider
    {
        GridSettings GridCurrentAutoPersistColumns { get; set; }
    }
}