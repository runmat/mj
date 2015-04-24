using System.Collections.Generic;

namespace GeneralTools.Contracts
{
    public interface IPersistableSelectorProvider
    {
        List<IPersistableObject> PersistableSelectors { get; }

        void PersistableSelectorsLoad<T>(string groupKey = null) where T : class, new();

        void PersistableSelectorsLoad();

        void PersistableSelectorResetCurrent();
    }
}