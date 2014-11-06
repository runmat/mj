using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using MvcTools.Models;

namespace MvcTools.Web
{
    public static partial class HtmlHelperExtensions
    {
        public static MvcHtmlString EditableHelpHint(this HtmlHelper html, string helpKey)
        {
            var model = new ContentEntity { ID = helpKey };
            model.DbLoad();

            return html.Partial("EditableHelpHintControl", model);
        }

        public static MvcHtmlString EditableHtml(this HtmlHelper html, string helpKey)
        {
            var model = new ContentEntity { ID = helpKey };
            model.DbLoad();

            return html.Partial("EditableHtmlControl", model);
        }

        public static MvcHtmlString EditorForWithValidation<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            var validationExpr = html.ValidationMessageFor(expression);

            if (validationExpr == null)
                return html.EditorFor(expression);

            return MvcHtmlString.Create(html.EditorFor(expression) + html.Raw("<br />").ToHtmlString() + html.ValidationMessageFor(expression));
        }

        public static object GetValue<TModel, TProperty>(this HtmlHelper html, Expression<Func<TModel, TProperty>> expression)
        {
            var modelMetaData = ModelMetadata.FromLambdaExpression(expression, (ViewDataDictionary<TModel>)html.ViewData);
            if (modelMetaData == null)
                return null;

            return modelMetaData.Model ?? "";
        }

        public static object GetValueInDisplayFormat<TModel, TProperty>(this HtmlHelper html, Expression<Func<TModel, TProperty>> expression)
        {
            var val = html.GetValue(expression);
            var formatString = html.GetDisplayFormatString(expression);
            if (formatString == null)
                return val;

            return string.Format(formatString, val);
        }

        public static string GetPropertyName<TModel, TProperty>(this HtmlHelper html, Expression<Func<TModel, TProperty>> expression)
        {
            var modelMetaData = ModelMetadata.FromLambdaExpression(expression, (ViewDataDictionary<TModel>)html.ViewData);
            if (modelMetaData == null)
                return null;

            return modelMetaData.PropertyName;
        }

        public static string GetDisplayFormatString<TModel, TProperty>(this HtmlHelper html, Expression<Func<TModel, TProperty>> expression)
        {
            var modelMetaData = ModelMetadata.FromLambdaExpression(expression, (ViewDataDictionary<TModel>)html.ViewData);
            if (modelMetaData == null)
                return null;

            return modelMetaData.DisplayFormatString;
        }

        public static string GetDisplayName<TModel, TProperty>(this HtmlHelper html, Expression<Func<TModel, TProperty>> expression)
        {
            var modelMetaData = ModelMetadata.FromLambdaExpression(expression, (ViewDataDictionary<TModel>)html.ViewData);
            if (modelMetaData == null)
                return null;

            return modelMetaData.DisplayName;
        }
    }
}
