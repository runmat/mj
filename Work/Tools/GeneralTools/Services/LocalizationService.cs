using System.Collections.Generic;
using GeneralTools.Contracts;

namespace GeneralTools.Services
{
    public class LocalizationService : ILocalizationService
    {
        #region Privates and constructor

        private readonly ITranslationService _translationService;

        public LocalizationService(ITranslationService translationService)
        {
            _translationService = translationService;
        }

        #endregion

        #region ILocalizationService Implementierung

        public string TranslateResourceKey(string resource)
        {
            return _translationService.GetTranslation(resource);
        }

        public IDictionary<string, string> GetTranslationsStartsWidthPrefixAndRemovePrefixFromKey(string resourcePrefix)
        {
            return _translationService.GetTranslationsStartsWidthPrefixAndRemovePrefixFromKey(resourcePrefix);
        }

        #endregion
    }
}
