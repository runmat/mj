using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Resources;

namespace ServicesMvc
{
    public class LocalizedRequiredAttributeAdapter : RequiredAttributeAdapter
    {
        public LocalizedRequiredAttributeAdapter(
            ModelMetadata metadata,
            ControllerContext context,
            RequiredAttribute attribute
        ) : base(metadata, context, attribute)
        {
            attribute.ErrorMessageResourceType = typeof(ValidationMessages);
            attribute.ErrorMessageResourceName = "PropertyValueRequired";
        }
    }
}
