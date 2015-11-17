using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Resources;
using MvcTools.Web;

namespace CkgDomainLogic.General.Services
{
    public partial class Localize
    {
        public static string TranslateResourceKey(string resourceKey)
        {
            return TranslationService == null ? "" : TranslationService.GetTranslation(resourceKey) ?? "";
        }

        private static ITranslationService TranslationService
        {
            get
            {
                return TranslationServiceExplicit ??
                       DependencyResolver.Current.GetService<ITranslationService>();
            }
        }

        public static ITranslationService TranslationServiceExplicit { get; set; }


        public static SelectList DefaultOptionConcat<T>(IEnumerable<T> listToConcat) where T : class, new()
        {
            var properties = typeof(T).GetProperties();
            var keyProperty = properties.FirstOrDefault(property => property.GetCustomAttributes(typeof(SelectListKeyAttribute), true).Any());
            var valueProperty = properties.FirstOrDefault(property => property.GetCustomAttributes(typeof(SelectListTextAttribute), true).Any());
            if (keyProperty == null || valueProperty == null)
                return new SelectList("", "");

            var defaultOption = new T();
            if (keyProperty.PropertyType == typeof(string))
                keyProperty.SetValue(defaultOption, "", null);
            else
                keyProperty.SetValue(defaultOption, 0, null);
            valueProperty.SetValue(defaultOption, TranslateResourceKey(LocalizeConstants.DropdownDefaultOptionPleaseChoose), null);

            return new List<T> { defaultOption }.Concat(listToConcat).ToSelectList();
        }
    }
}

