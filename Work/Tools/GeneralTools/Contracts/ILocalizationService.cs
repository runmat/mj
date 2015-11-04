using System.Collections.Generic;

namespace GeneralTools.Contracts
{
    public interface ILocalizationService
    {
        string TranslateResourceKey(string resourceKey);

        IDictionary<string, string> GetTranslationsStartsWidthPrefixAndRemovePrefixFromKey(string resourcePrefix);
    }
}
