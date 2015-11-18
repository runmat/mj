// ReSharper disable RedundantUsingDirective
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
            var partialViewUrl = SessionHelper.GetPartialViewUrlCurrent();
            if (partialViewUrl == null)
                return false;

            var key = string.Format("REQUIRED: {0} - {1} - {2}", partialViewUrl, modelTypeName, propertyName);

            var customerConfigurationProvider = DependencyResolver.Current.GetService<ICustomerConfigurationProvider>();
            if (customerConfigurationProvider == null)
                return false;

            var fieldConfigValue = customerConfigurationProvider.GetCurrentCustomerConfigVal(key).NotNullOrEmpty().ToLower();
            var fieldIsRequired = (fieldConfigValue == "true");

            return fieldIsRequired;
        }
    }

    public class FormAdminModeWysiwygFakeValidator : ModelValidator
    {
        public FormAdminModeWysiwygFakeValidator(ModelMetadata metadata, ControllerContext controllerContext) : base(metadata, controllerContext)
        {
        }

        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            SessionHelper.SetPartialViewUrlCurrent();

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
            SessionHelper.SetPartialViewUrlCurrent();

            if (container == null)
                yield break;

            var translationService = DependencyResolver.Current.GetService<ITranslationService>();
            if (translationService == null)
                yield break; 

            var modelType = container.GetType();
            var propertyName = Metadata.PropertyName ?? Metadata.ModelType.Name;

            var fieldIsRequired = CustomModelValidatorsProvider.IsPropertyRequired(modelType.Name, propertyName);

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