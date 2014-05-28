using System.Web.Mvc;
using GeneralTools.Contracts;

namespace CkgDomainLogic.General.Services
{
    public partial class Localize
    {
        public static string TranslateResourceKey(string resourceKey)
        {
            return TranslationFormatService == null ? "" : TranslationFormatService.GetTranslation(resourceKey) ?? "";
        }

        private static ITranslationFormatService TranslationFormatService
        {
            get { return TranslationFormatServiceExplicit ?? DependencyResolver.Current.GetService<ITranslationFormatService>(); }
        }

        public static ITranslationFormatService TranslationFormatServiceExplicit { get; set; }
    }
}
