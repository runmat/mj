using System;
using System.Globalization;
using System.Linq.Expressions;
using GeneralTools.Models;
using System.Web.Mvc;

namespace MvcTools.Web
{
    public static class ModelStateExtensions
    {
        public static void SetModelValue(this ModelStateDictionary modelState, string key, object rawValue)
        {
            modelState.SetModelValue(key, new ValueProviderResult(rawValue, String.Empty, CultureInfo.InvariantCulture));
        }

        public static void SetModelValue<TModel, TValue>(this ModelStateDictionary modelState, TModel model, Expression<Func<TModel, TValue>> expression, object rawValue)
        {
            modelState.SetModelValue(expression.GetPropertyName(), rawValue);
        }

        public static void AddModelError<T>(this ModelStateDictionary modelState, Expression<Func<T, object>> expression, string errorMessage)
        {
            modelState.AddModelError(expression.GetPropertyName(), errorMessage);
        }

        public static void AddModelError<T>(this ModelStateDictionary modelState, Expression<Func<T, object>> expression)
        {
            modelState.AddModelError(expression, "Error");
        }
    }
}
