// ReSharper disable RedundantUsingDirective

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace MvcTools.Web
{
    public class CustomModelValidatorsProvider : ModelValidatorProvider
    {
        public override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, ControllerContext context)
        {
            yield return new FormAdminModeWysiwygFakeValidator(metadata, context);
            yield return new CustomRequiredFieldValidator(metadata, context);
        }

        public static bool IsPropertyRequired(string modelTypeName, string propertyName)
        {
            return IsPropertyTrueFor("REQUIRED", modelTypeName, propertyName);
        }

        public static bool IsPropertyHidden(string modelTypeName, string propertyName)
        {
            return IsPropertyTrueFor("HIDDEN", modelTypeName, propertyName);
        }

        private static bool IsPropertyTrueFor(string propertyBooleanName, string modelFullTypeName, string propertyName)
        {
            var modelType = Type.GetType(modelFullTypeName);
            if (modelType == null)
                return false;

            var modelTypeName = modelType.Name;

            var partialViewContextCurrent = SessionHelper.GetPartialViewContextCurrent();
            if (partialViewContextCurrent == null)
                return false;

            var customerConfigurationProvider = DependencyResolver.Current.GetService<ICustomerConfigurationProvider>();
            if (customerConfigurationProvider == null)
                return false;

            var basicKey = string.Format("{0}___{1}___{2}", partialViewContextCurrent, modelTypeName, propertyName);

            var key = propertyBooleanName + ": " + basicKey;
            var fieldConfigValue = customerConfigurationProvider.GetCurrentBusinessCustomerConfigVal(key).NotNullOrEmpty().ToLower();
            var fieldIsTrue = (fieldConfigValue == "true");

            if (!fieldIsTrue)
            {
                var localizedDisplayAttribute = modelType.GetAttributeFrom<LocalizedDisplayAttribute>(propertyName);

                if (localizedDisplayAttribute == null)
                    return false;

                key = propertyBooleanName + ": " + localizedDisplayAttribute.ResourceID;
                fieldConfigValue = customerConfigurationProvider.GetCurrentBusinessCustomerConfigVal(key).NotNullOrEmpty().ToLower();
                fieldIsTrue = (fieldConfigValue == "true");
            }

            return fieldIsTrue;
        }
    }

    public class FormAdminModeWysiwygFakeValidator : ModelValidator
    {
        public FormAdminModeWysiwygFakeValidator(ModelMetadata metadata, ControllerContext controllerContext) : base(metadata, controllerContext)
        {
        }

        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            SessionHelper.SetPartialViewContextCurrent();

            if (SessionHelper.FormSettingsAdminModeWysiwygModeGet())
                yield return new ModelValidationResult
                {
                    MemberName = null,
                    Message = @"This is only a fake validation error message, " +
                              @"to avoid a valid model state + prevent unnecessary querying of business data, if we are only in form admin wysiwyg mode"
                };
        }
    }

    public class CustomRequiredFieldValidator : ModelValidator
    {
        public CustomRequiredFieldValidator(ModelMetadata metadata, ControllerContext controllerContext) : base(metadata, controllerContext)
        {
        }

        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            SessionHelper.SetPartialViewContextCurrent();

            if (container == null)
                yield break;

            var translationService = DependencyResolver.Current.GetService<ITranslationService>();
            if (translationService == null)
                yield break; 

            var modelType = container.GetType();
            var propertyName = Metadata.PropertyName ?? Metadata.ModelType.Name;

            var fieldIsRequired = CustomModelValidatorsProvider.IsPropertyRequired(modelType.GetFullTypeName(), propertyName);

            if (!fieldIsRequired)
                yield break;

            var pi = modelType.GetProperty(propertyName);
            var piValue = pi == null ? null : pi.GetValue(container, null);
            if (piValue == null || piValue.ToString().IsNullOrEmpty())
                yield return new ModelValidationResult
                {
                    MemberName = null,
                    Message = translationService.GetTranslation(LocalizeConstants.ThisFieldIsRequired)
                };
        }
    }
}