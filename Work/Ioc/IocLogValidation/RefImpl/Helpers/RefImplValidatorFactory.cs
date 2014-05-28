using System;
using System.Web.Mvc;
using FluentValidation;

namespace RefImpl.Helpers
{
    public class RefImplValidatorFactory : ValidatorFactoryBase
    {
        public override IValidator CreateInstance(Type validatorType)
        {
            return DependencyResolver.Current.GetService(validatorType) as IValidator;
        }
    }
}
