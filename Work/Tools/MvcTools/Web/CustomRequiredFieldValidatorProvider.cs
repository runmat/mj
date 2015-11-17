// ReSharper disable RedundantUsingDirective
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace MvcTools.Web
{
    public class CustomRequiredFieldValidatorProvider : ModelValidatorProvider
    {
        public override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, ControllerContext context)
        {
            yield return new CustomRequiredFieldValidator(metadata, context);
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

            var modelType = container.GetType();
            var propertyName = Metadata.PropertyName ?? Metadata.ModelType.Name;

            var partialViewUrl = SessionHelper.GetPartialViewUrlCurrent();
            if (partialViewUrl == null)
                yield break;

            var translationService = DependencyResolver.Current.GetService<ITranslationService>();
            if (translationService == null)
                yield break;

            var key = string.Format("REQUIRED: {0} - {1} - {2}", partialViewUrl, modelType.Name, propertyName);

            var customerConfigurationProvider = DependencyResolver.Current.GetService<ICustomerConfigurationProvider>();
            if (customerConfigurationProvider == null)
                yield break;

            var fieldConfigValue = customerConfigurationProvider.GetCurrentCustomerConfigVal(key).NotNullOrEmpty().ToLower();
            var fieldIsRequired = (fieldConfigValue == "true");

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