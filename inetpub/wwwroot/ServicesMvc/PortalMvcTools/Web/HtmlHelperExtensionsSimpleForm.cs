using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using GeneralTools.Models;
using MvcTools.Models;
using MvcTools.Web;

namespace PortalMvcTools.Web
{
    public static class PortalHtmlHelperExtensionsSimpleForm
    {
        #region SimpleForm Controls

        public static MvcHtmlString SimpleFormDropDownListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IEnumerable<string> selectList,
                                                                object controlHtmlAttributes = null,
                                                                string iconCssClass = null)
        {
            return html.SimpleFormDropDownListFor(expression, selectList.ToSelectList(), controlHtmlAttributes, iconCssClass);
        }

        public static MvcHtmlString SimpleFormDropDownListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string selectOptions,
                                                                object controlHtmlAttributes = null,
                                                                string iconCssClass = null)
        {
            return html.SimpleFormDropDownListFor(expression, selectOptions.ToSelectList(), controlHtmlAttributes, iconCssClass);
        }

        public static MvcHtmlString SimpleFormDropDownListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList,
                                                                object controlHtmlAttributes = null,
                                                                string iconCssClass = null)
        {
            return html.SimpleFormDropDownListForInner(expression, selectList, controlHtmlAttributes, iconCssClass);
        }

        public static MvcHtmlString SimpleFormDropDownListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectItem> selectList,
                                                                object controlHtmlAttributes = null,
                                                                string iconCssClass = null)
        {
            return html.SimpleFormDropDownListForInner(expression, new SelectList(selectList, "Key", "Text"), controlHtmlAttributes, iconCssClass);
        }

        private static MvcHtmlString SimpleFormDropDownListForInner<TModel, TValue>(this HtmlHelper<TModel> html,
                                                                Expression<Func<TModel, TValue>> expression,
                                                                IEnumerable<SelectListItem> selectList,
                                                                object controlHtmlAttributes = null,
                                                                string iconCssClass = null)
        {
            var model = new FormControlModel
            {
                DisplayNameHtml = html.DisplayNameFor(expression),
                RequiredIndicatorHtml = html.RequiredIndicatorFor(expression),
                ControlHtml = html.DropDownListFor(expression, selectList, controlHtmlAttributes),
                ValidationMessageHtml = html.ValidationMessageFor(expression),
                IconCssClass = iconCssClass,
                ControlHtmlAttributes = controlHtmlAttributes.ToHtmlDictionary(),
            };

            return html.Partial("Partial/FormControls/SimpleForm/LeftLabelControl", model);
        }

        public static MvcHtmlString SimpleFormTextBox(this HtmlHelper html,
                                                                string propertyName,
                                                                object controlHtmlAttributes = null,
                                                                string iconCssClass = null,
                                                                string labelHtml = null)
        {
            var model = new FormControlModel
            {
                DisplayNameHtml = labelHtml.IsNotNullOrEmpty() ? new MvcHtmlString(labelHtml) : html.DisplayName(propertyName),
                RequiredIndicatorHtml = labelHtml.IsNotNullOrEmpty() ? MvcHtmlString.Empty : html.RequiredIndicator(propertyName),
                ControlHtml = html.TextBox(propertyName, null, controlHtmlAttributes),
                ValidationMessageHtml = html.ValidationMessage(propertyName),
                IconCssClass = iconCssClass,
                ControlHtmlAttributes = controlHtmlAttributes.ToHtmlDictionary(),
            };

            return html.Partial("Partial/FormControls/SimpleForm/LeftLabelControl", model);
        }

        public static MvcHtmlString SimpleFormTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html,
                                                                Expression<Func<TModel, TValue>> expression,
                                                                object controlHtmlAttributes = null,
                                                                string iconCssClass = null,
                                                                string labelHtml = null)
        {
            return html.SimpleFormTextBox(html.GetPropertyName(expression), controlHtmlAttributes, iconCssClass, labelHtml);
        }

        public static MvcHtmlString SimpleFormTextBlockFor<TModel, TValue>(this HtmlHelper<TModel> html,
                                                                Expression<Func<TModel, TValue>> expression,
                                                                object controlHtmlAttributes = null,
                                                                string iconCssClass = null,
                                                                string labelHtml = null)
        {
            var model = new FormControlModel
            {
                DisplayNameHtml = labelHtml.IsNotNullOrEmpty() ? new MvcHtmlString(labelHtml) : html.DisplayName(html.GetPropertyName(expression)),
                ControlHtml = html.Partial("Partial/FormControls/SimpleForm/TextBlockInner", html.GetValueInDisplayFormat(expression)),
                ValidationMessageHtml = html.ValidationMessageFor(expression),
                IconCssClass = iconCssClass,
                ControlHtmlAttributes = controlHtmlAttributes.ToHtmlDictionary(),
            };

            return html.Partial("Partial/FormControls/SimpleForm/LeftLabelControl", model);
        }

        public static MvcHtmlString SimpleFormCheckBoxFor<TModel>(this HtmlHelper<TModel> html,
                                                                Expression<Func<TModel, bool>> expression,
                                                                object controlHtmlAttributes = null,
                                                                string iconCssClass = null)
        {
            var model = new FormControlModel
            {
                DisplayNameHtml = html.DisplayNameFor(expression),
                RequiredIndicatorHtml = html.RequiredIndicatorFor(expression),
                ControlHtml = html.CheckBoxFor(expression, controlHtmlAttributes),
                ValidationMessageHtml = html.ValidationMessageFor(expression),
                IconCssClass = iconCssClass,
                ControlHtmlAttributes = controlHtmlAttributes.ToHtmlDictionary(),
            };

            return html.Partial("Partial/FormControls/SimpleForm/CheckBox", model);
        }

        public static MvcHtmlString SimpleFormCheckBoxInlineFor<TModel>(this HtmlHelper<TModel> html,
                                                                Expression<Func<TModel, bool>> expression,
                                                                object controlHtmlAttributes = null,
                                                                string iconCssClass = null)
        {
            var model = new FormControlModel
            {
                DisplayNameHtml = html.DisplayNameFor(expression),
                ControlHtml = html.CheckBoxFor(expression, controlHtmlAttributes),
                ValidationMessageHtml = html.ValidationMessageFor(expression),
                IconCssClass = iconCssClass,
                ControlHtmlAttributes = controlHtmlAttributes.ToHtmlDictionary(),
            };

            return html.Partial("Partial/FormControls/SimpleForm/CheckBoxInline", model);
        }

        public static MvcHtmlString SimpleFormDateRangePickerFor<TModel>(this HtmlHelper<TModel> html,
                                                                Expression<Func<TModel, bool>> useDateRangeExpression,
                                                                DateTime? dateRangeStartValue,
                                                                DateTime? dateRangeEndValue,
                                                                object controlHtmlAttributes = null,
                                                                string iconCssClass = null)
        {
            var useDateRangeValue = (bool)GetPropertyValue(typeof(TModel), html.ViewData.Model, useDateRangeExpression);

            var model = new SimpleFormDateRangePickerModel
            {
                DisplayNameHtml = html.DisplayNameFor(useDateRangeExpression),
                RequiredIndicatorHtml = html.RequiredIndicatorFor(useDateRangeExpression),
                ControlHtml = html.CheckBoxFor(useDateRangeExpression, controlHtmlAttributes),
                ValidationMessageHtml = html.ValidationMessageFor(useDateRangeExpression),
                IconCssClass = iconCssClass,

                UseDateRangeValue = useDateRangeValue,
                UseDateRangeProperty = useDateRangeExpression.GetPropertyName(),

                DateRangeStart = dateRangeStartValue,
                DateRangeEnd = dateRangeEndValue,
                ControlHtmlAttributes = controlHtmlAttributes.ToHtmlDictionary(),
            };

            return html.Partial("Partial/FormControls/SimpleForm/DateRangePicker", model);
        }

        public static MvcHtmlString Win8Loader(this HtmlHelper html)
        {
            return html.Partial("Partial/Win8Loader");
        }

        private static object GetPropertyValue(Type type, object model, LambdaExpression expression)
        {
            return GetPropertyValue(type, model, expression.GetPropertyName());
        }

        private static object GetPropertyValue(Type type, object model, string propertyName)
        {
            var propertyInfo = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(p => p.Name == propertyName);
            if (propertyInfo == null)
                return null;

            return propertyInfo.GetValue(model, null);
        }

        #endregion
    }
}
