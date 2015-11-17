using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GeneralTools.Contracts;
using GeneralTools.Models;

namespace MvcTools.Web
{
    public class AnnotationsAndConventionsBasedModelMetaDataProvider : DataAnnotationsModelMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
        {
            var result = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);

            if (containerType == null)
                return result;

            if (string.IsNullOrEmpty(propertyName))
                return result;

            // Via Reflection das LocalizedDisplayAttribute finden, Property ResourceID einlesen
            var localizedDisplayAttribute = containerType.GetAttributeFrom<LocalizedDisplayAttribute>(propertyName);

            // Property bestitzt keine relevante Annotation, kann keine Übersetzung ermitteln
            if (localizedDisplayAttribute == null)
                return result;

            // Auf die gecachten Resourcen zugreifen und die Übersetzung ermitteln
            if (localizedDisplayAttribute.Suffix == null)
                result.DisplayName = TranslationService.GetTranslation(localizedDisplayAttribute.DisplayName);    
            else
                result.DisplayName = string.Concat(TranslationService.GetTranslation(localizedDisplayAttribute.DisplayName), " ", localizedDisplayAttribute.Suffix.ToString());    

            if (string.IsNullOrEmpty(result.DisplayFormatString))
                result.DisplayFormatString = TranslationService.GetFormat(localizedDisplayAttribute.DisplayName);

            // Auf die gecachten Resourcen zugreifen und die Übersetzung und Format ermitteln
            result.DisplayFormatString = TranslationService.GetFormat(localizedDisplayAttribute.DisplayName);

            return result;
        }

        private static ITranslationService TranslationService
        {
            get { return DependencyResolver.Current.GetService<ITranslationService>(); }
        }
    }
}
