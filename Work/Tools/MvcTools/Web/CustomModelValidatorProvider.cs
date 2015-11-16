// ReSharper disable RedundantUsingDirective
 using System;
using System.Collections.Generic;
using System.Linq;
 using System.Reflection;
 using System.Text;
using System.Web.Mvc;
 using GeneralTools.Models;

namespace MvcTools.Web
{
    public class CustomModelValidatorProvider : ModelValidatorProvider
    {
        public override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, ControllerContext context)
        {
            yield return new CustomModelValidator(metadata, context);
        }
    }

    public class CustomModelValidator : ModelValidator
    {
        public CustomModelValidator(ModelMetadata metadata, ControllerContext controllerContext) : base(metadata, controllerContext)
        {
        }

        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            var memberName = Metadata.PropertyName ?? Metadata.ModelType.Name;

            if (memberName.NotNullOrEmpty() == "Kennzeichen")
            {
                var pi = container.GetType().GetProperty(memberName);
                var piValue = pi == null ? null : pi.GetValue(container, null);
                if (piValue == null || piValue.ToString().IsNullOrEmpty())
                    yield return new ModelValidationResult
                    {
                        MemberName = null,
                        Message = "Das geht aber nicht!!"
                    };
            }
        }
    }
}