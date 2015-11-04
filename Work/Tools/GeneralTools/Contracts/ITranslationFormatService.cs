using System.Collections.Generic;

namespace GeneralTools.Contracts
{
    public interface ITranslationFormatService
    {
        string GetTranslation(string resource);

        IDictionary<string, string> GetTranslationsStartsWidthPrefixAndRemovePrefixFromKey(string resourcePrefix);

        string GetTranslationKurz(string resource);

        string GetFormat(string resource);
    }
}
