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
            {
                return result;
            }

            if (string.IsNullOrEmpty(propertyName))
            {
                return result;
            }

            // Via Reflection die LocalizedDiesplayAttribute finden, Property ResourceID einlesen
            var localizedDisplayAttribute = containerType.GetAttributeFrom<LocalizedDisplayAttribute>(propertyName);

            // Property bestitz keine relevante Annotation, kann keine Übersetzung ermitteln
            if (localizedDisplayAttribute == null)
            {
                return result;
            }

            // Auf die gecachten Resourcen zugreifen und die Übersetzung ermitteln
            if (localizedDisplayAttribute.Suffix == null)
            {
                result.DisplayName = TranslationFormatService.GetTranslation(localizedDisplayAttribute.DisplayName);    
            }
            else
            {
                result.DisplayName = string.Concat(TranslationFormatService.GetTranslation(localizedDisplayAttribute.DisplayName), " ", localizedDisplayAttribute.Suffix.ToString());    
            }

            if (string.IsNullOrEmpty(result.DisplayFormatString))
            {
                result.DisplayFormatString = TranslationFormatService.GetFormat(localizedDisplayAttribute.DisplayName);
            }

            // Auf die gecachten Resourcen zugreifen und die Übersetzung und Format ermitteln
            result.DisplayFormatString = TranslationFormatService.GetFormat(localizedDisplayAttribute.DisplayName);

            return result;
        }

        /// <summary>
        /// Ermittle die Instanz für die aktuelle http Request
        /// </summary>
        public ITranslationFormatService TranslationFormatService
        {
            get { return DependencyResolver.Current.GetService<ITranslationFormatService>(); }
        }
    }
}
