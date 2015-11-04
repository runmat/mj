using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;

namespace CkgDomainLogic.General.Services
{
    public class TelerikLocalizationAdapterService : Telerik.Web.Mvc.Infrastructure.ILocalizationService
    {
        public string One(string key)
        {
            var defaultLocalizationService = DependencyResolver.Current.GetService(typeof (GeneralTools.Contracts.ILocalizationService)) as GeneralTools.Contracts.ILocalizationService;
            if (defaultLocalizationService == null)
                return key;

            return defaultLocalizationService.TranslateResourceKey("Telerik_" + key);
        }

        public IDictionary<string, string> All()
        {
            var defaultLocalizationService = DependencyResolver.Current.GetService(typeof(GeneralTools.Contracts.ILocalizationService)) as GeneralTools.Contracts.ILocalizationService;
            if (defaultLocalizationService == null)
                return new Dictionary<string, string>();

            var dict = defaultLocalizationService.GetTranslationsStartsWidthPrefixAndRemovePrefixFromKey("Telerik_");
            return dict;
        }

        public bool IsDefault { get { return false; } }
    }

    public class TelerikLocalizationAdapterServiceFactory : Telerik.Web.Mvc.Infrastructure.ILocalizationServiceFactory
    {
        public Telerik.Web.Mvc.Infrastructure.ILocalizationService Create(string resourceName, CultureInfo culture)
        {
            return new TelerikLocalizationAdapterService();
        }
    }
}
