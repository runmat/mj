using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using GeneralTools.Services;

namespace CkgDomainLogic.General.Services
{
    public class TelerikLocalizationAdapterService : Telerik.Web.Mvc.Infrastructure.ILocalizationService
    {
        public string One(string key)
        {
            var defaultLocalizationService = (GeneralTools.Contracts.ILocalizationService)DependencyResolver.Current.GetService(typeof (GeneralTools.Contracts.ILocalizationService));
            return defaultLocalizationService.TranslateResourceKey("Telerik_" + key);
        }

        public IDictionary<string, string> All()
        {
            return new Dictionary<string, string>();
        }

        public bool IsDefault { get { return true; } }
    }

    public class TelerikLocalizationAdapterServiceFactory : Telerik.Web.Mvc.Infrastructure.ILocalizationServiceFactory
    {
        public Telerik.Web.Mvc.Infrastructure.ILocalizationService Create(string resourceName, CultureInfo culture)
        {
            return new TelerikLocalizationAdapterService();
        }
    }
}
