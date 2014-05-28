using System;
using System.Collections.Generic;
using System.Web.Mvc;
using RefImplBibl.Resources;

namespace RefImpl.Helpers
{
    public class AnnotationsAndConventionsBasedModelMetaDataProvider : DataAnnotationsModelMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
        {
            var result = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);

            if (string.IsNullOrEmpty(result.DisplayName) && containerType != null && propertyName != null)
            {
                var keyForDisplayValue = string.Format("{0}_{1}", containerType.Name, propertyName);
                var translatedValue =  Properties.ResourceManager.GetObject(keyForDisplayValue) as string;

                if (!string.IsNullOrEmpty(translatedValue))
                {
                    result.DisplayName = translatedValue;
                }

                var keyForDisplayFormatValue = string.Format("{0}_{1}Display", containerType.Name, propertyName);
                translatedValue = Properties.ResourceManager.GetObject(keyForDisplayFormatValue) as string;

                if (!string.IsNullOrEmpty(translatedValue))
                {
                    result.DisplayFormatString = translatedValue;
                }
            }

            return result;
        }
    }
}