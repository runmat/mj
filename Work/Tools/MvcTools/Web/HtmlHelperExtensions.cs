using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.WebPages;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Resources;
using MvcTools.Contracts;
using MvcTools.Models;

namespace MvcTools.Web
{
    public static partial class HtmlHelperExtensions
    {
        #region Render Once

        public static HelperResult Once(this HtmlHelper html, string key, Func<object, HelperResult> template)
        {
            var httpContextItems = html.ViewContext.HttpContext.Items;
            var contextKey = "MvcTools.Once." + key;
            if (!httpContextItems.Contains(contextKey))
            {
                // Render and record the fact in HttpContext.Items
                httpContextItems.Add(contextKey, null);
                return template(null);
            }

            // Do nothing, already rendered something with that key
            return new HelperResult(writer => { /*no-op*/ });
        }

        #endregion


        #region LabelHelper

        public static MvcHtmlString DisplayName(this HtmlHelper html, string htmlFieldName, string labelText = null)
        {
            var metadata = ModelMetadata.FromStringExpression(htmlFieldName, html.ViewData);

            if (labelText.IsNullOrEmpty())
                labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();

            if (labelText.IsNullOrEmpty()) return MvcHtmlString.Empty;

            if (metadata.PropertyName == null) return MvcHtmlString.Empty;

            return MvcHtmlString.Create(labelText);
        }

        public static MvcHtmlString DisplayNameFor<TModel, TValue>(this HtmlHelper<TModel> html,
                                                                            Expression<Func<TModel, TValue>> expression,
                                                                            string labelText = null)
        {
            return html.DisplayName(html.GetPropertyName(expression), labelText);
        }

        public static MvcHtmlString RequiredIndicator(this HtmlHelper html, string htmlFieldName, bool hideAsteriskTag = false)
        {
            var metadata = ModelMetadata.FromStringExpression(htmlFieldName, html.ViewData);

            return RequiredIndicatorInner(hideAsteriskTag, metadata);
        }

        public static MvcHtmlString RequiredIndicatorFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, bool hideAsteriskTag = false)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);

            return RequiredIndicatorInner(hideAsteriskTag, metadata);
        }

        private static MvcHtmlString RequiredIndicatorInner(bool hideAsteriskTag, ModelMetadata metadata)
        {
            var isRequired = false;
            if (metadata.PropertyName == null) return MvcHtmlString.Empty;

            if (metadata.ContainerType != null)
            {
                isRequired = metadata.ContainerType.GetProperty(metadata.PropertyName).GetCustomAttributes(typeof(RequiredAttribute), false).Any() ||
                             metadata.ContainerType.GetProperty(metadata.PropertyName).GetCustomAttributes(typeof(RequiredConditionalAttribute), false).Any();

                if (!isRequired)
                    isRequired = CustomModelValidatorsProvider.IsPropertyRequired(metadata.ContainerType.GetFullTypeName(), metadata.PropertyName);
            }

            var asteriskTag = new TagBuilder("span") { InnerHtml = "&nbsp;" };
            var requiredClass = "requiredField";
            if (isRequired && !hideAsteriskTag)
            {
                asteriskTag.SetInnerText("*");
                requiredClass += " requiredFieldMarker";
            }
            asteriskTag.Attributes.Add("class", requiredClass);
            var requiredSpan = asteriskTag.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create(requiredSpan);
        }

        #endregion


        #region Tags

        public static MvcTag BeginDiv(this HtmlHelper html, string className = null, int? width = null, int? height = null, string id = null)
        {
            return new MvcTag(html.ViewContext, "div", className, width: width, height:height, id : id);
        }

        public static MvcTag BeginSpan(this HtmlHelper html, string className = null, int? width = null, string id = null)
        {
            return new MvcTag(html.ViewContext, "span", className, width: width, id: id);
        }

        public static MvcTag BeginParagraph(this HtmlHelper html, string className = null, int? width = null, string id = null)
        {
            return new MvcTag(html.ViewContext, "p", className, width: width, id: id);
        }

        public static MvcHtmlString Script(this HtmlHelper html, string src)
        {
            if (src.Contains("~"))
                src = VirtualPathUtility.ToAbsolute(src);

            var script = new TagBuilder("script");
            script.Attributes.Add("src", src);
            script.Attributes.Add("type", "text/javascript");

            return new MvcHtmlString(script.ToString(TagRenderMode.Normal) + MvcTag.CR);
        }

        public static MvcHtmlString Style(this HtmlHelper html, string href)
        {
            if (href.Contains("~"))
                href = VirtualPathUtility.ToAbsolute(href);

            var style = new TagBuilder("link");
            style.Attributes.Add("href", href);
            style.Attributes.Add("rel", "stylesheet");

            return new MvcHtmlString(style.ToString(TagRenderMode.Normal) + MvcTag.CR);
        }

        public static MvcHtmlString Concat(this MvcHtmlString first, params MvcHtmlString[] strings)
        {
            return MvcHtmlString.Create(first + string.Concat(strings.Select(s => s.ToString())));
        }

        #endregion


        #region Misc

        public static string ToAbsolutePath(this HtmlHelper html, string path)
        {
            return VirtualPathUtility.ToAbsolute(path);
        }

        public static MvcHtmlString PartialFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string partialViewName)
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            var model = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model;
            var viewData = new ViewDataDictionary(htmlHelper.ViewData)
            {
                TemplateInfo = new TemplateInfo
                {
                    HtmlFieldPrefix = name
                }
            };

            return htmlHelper.Partial(partialViewName, model, viewData);
        }

        public static MvcHtmlString PartialConditionalFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string partialViewName, Expression<Func<TModel, bool>> conditionalExpression)
        {
            if ((bool)ModelMetadata.FromLambdaExpression(conditionalExpression, htmlHelper.ViewData).Model)
                return htmlHelper.PartialFor(expression, partialViewName);

            return MvcHtmlString.Empty;
        }

        public static MvcHtmlString PartialConditional<TModel>(this HtmlHelper<TModel> htmlHelper, string partialViewName, object model, Func<bool> conditionalFunc)
        {
            if (conditionalFunc())
                return htmlHelper.Partial(partialViewName, model);

            return MvcHtmlString.Empty;
        }

        public static IDictionary<string, object> MergeHtmlAttributes(this IDictionary<string, object> htmlAttributes, IEnumerable<KeyValuePair<string, object>> additionalHtmlAttributes)
        {
            if (additionalHtmlAttributes != null)
                additionalHtmlAttributes.ToList().ForEach(a =>
                {
                    var attr = new KeyValuePair<string, object>(a.Key, a.Value);
                    if (htmlAttributes.ContainsKey(a.Key))
                    {
                        object value;
                        if (htmlAttributes.TryGetValue(a.Key, out value))
                        {
                            var sValue = (string)value;
                            sValue += (string)a.Value;
                            attr = new KeyValuePair<string, object>(a.Key, sValue);
                        }
                    }

                    htmlAttributes.Remove(attr.Key);
                    htmlAttributes.Add(attr);
                });

            return htmlAttributes;
        }


        public static MvcHtmlString PartialOrFormErrorHint(this HtmlHelper html, string partialViewNameSuccess, string errorMessage)
        {
            if (errorMessage.IsNullOrEmpty())
                return html.Partial(partialViewNameSuccess);

            return html.FormError(errorMessage);
        }

        private static MvcHtmlString FormHint(this HtmlHelper html, FormHintModel model)
        {
            return html.Partial("Partial/FormHint", model);
        }

        public static MvcHtmlString FormHint(this HtmlHelper html, FormHintModel.Mode hintMode, string title, string body)
        {
            return html.FormHint(new FormHintModel
            {
                HintMode = hintMode, 
                Title = title, 
                Body = body,
            });
        }

        public static MvcHtmlString FormError(this HtmlHelper html, string body)
        {
            return html.FormHint(new FormHintModel
            {
                HintMode = FormHintModel.Mode.Error,
                Title = CommonResources.Error,
                Body = body,
            });
        }

        public static MvcHtmlString FormAlert(this HtmlHelper html, string body)
        {
            return html.FormHint(new FormHintModel
            {
                HintMode = FormHintModel.Mode.Error,
                Title = CommonResources.Alert,
                Body = body,
            });
        }

        public static MvcHtmlString FormError(this HtmlHelper html, string title, string body)
        {
            return html.FormHint(new FormHintModel
            {
                HintMode = FormHintModel.Mode.Error,
                Title = title,
                Body = body,
            });
        }

        public static MvcHtmlString FormSuccess(this HtmlHelper html, string title, string body)
        {
            return html.FormHint(new FormHintModel
            {
                HintMode = FormHintModel.Mode.Success,
                Title = title,
                Body = body,
            });
        }

        public static MvcHtmlString FormInfo(this HtmlHelper html, string title, string body)
        {
            return html.FormHint(new FormHintModel
            {
                HintMode = FormHintModel.Mode.Info,
                Title = title,
                Body = body,
            });
        }


        public static MvcHtmlString SpanAlert(this HtmlHelper html, string spanID, string additionalCssClass = "")
        {
            html.ViewBag.SpanID = spanID;
            html.ViewBag.AdditionalCssClass = additionalCssClass;
            return html.Partial("Partial/SpanAlert");
        }

        public static MvcHtmlString ChartToolsFlotr2(this HtmlHelper html)
        {
            return html.Partial("Partial/ChartToolsFlotr2");
        }

        #endregion


        #region TextBlock

        public static MvcHtmlString TextBlockFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.TextBlockFor(expression, format: null, htmlAttributes: htmlAttributes);
        }

        public static MvcHtmlString TextBlockFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format, IDictionary<string, object> htmlAttributes)
        {
            if (format == null)
                format = (typeof(TProperty) == typeof(DateTime) || typeof(TProperty) == typeof(DateTime?) ? "{0:d}" : "");

            if (typeof (TProperty) == typeof (bool) || typeof (TProperty) == typeof (bool?))
                return htmlHelper.CheckBox(expression.GetPropertyName(), new { @disabled = "disabled" });

            return TextBlockHelper(htmlHelper,
                                 ExpressionHelper.GetExpressionText(expression),
                                 null,
                                 true,
                                 format,
                                 htmlAttributes);
        }

        private static MvcHtmlString TextBlockHelper(HtmlHelper htmlHelper, string name, object value, bool useViewData, string format, IDictionary<string, object> htmlAttributes)
        {
            var fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);

            var tagBuilder = new TagBuilder("span");
            tagBuilder.MergeAttributes(htmlAttributes);

            var valueParameter = htmlHelper.FormatValue(value, format);

            var attemptedValue = (string)htmlHelper.GetModelStateValue(fullName, typeof(string));
            var finalValue = (attemptedValue ?? ((useViewData) ? htmlHelper.EvalString(fullName, format) : valueParameter));
            
            tagBuilder.GenerateId(fullName);

            // If there are any errors for a named field, we add the css attribute.
            ModelState modelState;
            if (htmlHelper.ViewData.ModelState.TryGetValue(fullName, out modelState))
                if (modelState.Errors.Count > 0)
                    tagBuilder.AddCssClass(HtmlHelper.ValidationInputCssClassName);

            tagBuilder.SetInnerText(finalValue);

            return MvcHtmlString.Create(tagBuilder.ToString());
        }

        internal static object GetModelStateValue(this HtmlHelper htmlHelper, string key, Type destinationType)
        {
            ModelState modelState;
            if (htmlHelper.ViewData.ModelState.TryGetValue(key, out modelState))
                if (modelState.Value != null)
                    return modelState.Value.ConvertTo(destinationType, null /* culture */);

            return null;
        }

        internal static bool EvalBoolean(this HtmlHelper htmlHelper, string key)
        {
            return Convert.ToBoolean(htmlHelper.ViewData.Eval(key), CultureInfo.InvariantCulture);
        }

        internal static string EvalString(this HtmlHelper htmlHelper, string key)
        {
            return Convert.ToString(htmlHelper.ViewData.Eval(key), CultureInfo.CurrentCulture);
        }

        internal static string EvalString(this HtmlHelper htmlHelper, string key, string format)
        {
            return Convert.ToString(htmlHelper.ViewData.Eval(key, format), CultureInfo.CurrentCulture);
        }

        #endregion


        #region UrlHelper

        public static string ContentArea(this UrlHelper url, string path)
        {
            var modulName = url.RequestContext.RouteData.DataTokens["area"];
            var modulContentLoad = "";

            if (modulName == null)
                return string.Empty;

            if (!string.IsNullOrEmpty(modulName.ToString()))
                modulContentLoad = "Areas/" + modulName;

            if (path.StartsWith("~/"))
                path = path.Remove(0, 2);

            if (path.StartsWith("/"))
                path = path.Remove(0, 1);

            path = path.Replace("../", string.Empty);

            return VirtualPathUtility.ToAbsolute("~/" + modulContentLoad + "/" + path);
        }

        #endregion

        
        #region Form Persistence / Grid Auto Persistence

        public static MvcHtmlString PersistenceIndicator(this HtmlHelper html, string htmlFieldName)
        {
            var metadata = ModelMetadata.FromStringExpression(htmlFieldName, html.ViewData);

            return html.PersistenceIndicatorInner(metadata, html.ViewData.Model);
        }

        public static MvcHtmlString PersistenceIndicatorFor<TModel, TValue>(this HtmlHelper<TModel> html,
                                                                            Expression<Func<TModel, TValue>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);

            return html.PersistenceIndicatorInner(metadata, html.ViewData.Model);
        }

        private static MvcHtmlString PersistenceIndicatorInner(this HtmlHelper html, ModelMetadata metadata,
                                                               object parentModel)
        {
            if (metadata.PropertyName == null) return MvcHtmlString.Empty;

            var isPersistable = false;
            if (metadata.ContainerType != null)
                isPersistable =
                    metadata.ContainerType.GetProperty(metadata.PropertyName)
                            .GetCustomAttributes(typeof (FormPersistableAttribute), false)
                            .Any();

            var isPersistMode = false;
            var persistableModel = (parentModel as IPersistableObject);
            if (persistableModel != null)
                isPersistMode = (persistableModel.ObjectKey.IsNotNullOrEmpty());

            if (isPersistable && isPersistMode)
                return html.Partial("Partial/FormPersistence/FieldIndicator");

            return MvcHtmlString.Empty;
        }

        public static MvcHtmlString FormPersistenceMenu<T>(this HtmlHelper html, T model) where T : class, new()
        {
            var controller = (html.ViewContext.Controller as IPersistableSelectorProvider);
            if (controller != null)
                controller.PersistableSelectorsLoad<T>();

            return html.Partial("Partial/FormPersistence/Menu", model);
        }

        public static MvcHtmlString FormPersistenceGridMenu(this HtmlHelper html)
        {
            var viewGridMenuReset = "Partial/FormPersistence/GridMenuReset";

            var controller = (html.ViewContext.Controller as IPersistableSelectorProvider);
            if (controller == null)
                return html.Partial(viewGridMenuReset);

            var persistableSelectorObjectKeyCurrent =
                SessionHelper.GetSessionString("PersistableSelectorObjectKeyCurrent");
            if (persistableSelectorObjectKeyCurrent == null)
                return html.Partial(viewGridMenuReset);

            controller.PersistableSelectorsLoad();
            var selectors = controller.PersistableSelectors;
            if (selectors == null || selectors.None())
                return html.Partial(viewGridMenuReset);

            var selectorCurrent = selectors.FirstOrDefault(s => s.ObjectKey == persistableSelectorObjectKeyCurrent);
            if (selectorCurrent == null)
                return html.Partial(viewGridMenuReset);

            return html.Partial("Partial/FormPersistence/GridMenu", selectorCurrent);
        }

        static bool FormGetGridColumnsAutoPersistProvider(this HtmlHelper html, Type type, out IGridColumnsAutoPersistProvider controller)
        {
            controller = null;

            if (type.GetCustomAttributes(true).OfType<GridColumnsAutoPersistAttribute>().None())
                return false;

            var gridCurrentGetAutoPersistColumnsKey = SessionHelper.GridCurrentGetAutoPersistColumnsKey();
            if (gridCurrentGetAutoPersistColumnsKey.IsNullOrEmpty())
                return false;

            controller = (html.ViewContext.Controller as IGridColumnsAutoPersistProvider);
            if (controller == null)
                return false;

            return true;
        }

        public static MvcHtmlString FormGridCurrentLoadAutoPersistColumns(this HtmlHelper html, Type type)
        {
            IGridColumnsAutoPersistProvider controller;

            if (!html.FormGetGridColumnsAutoPersistProvider(type, out controller))
                return MvcHtmlString.Empty;

            var gridCurrentGetAutoPersistColumns = controller.GridCurrentSettingsAutoPersist;
            if (gridCurrentGetAutoPersistColumns == null || gridCurrentGetAutoPersistColumns.Columns.IsNullOrEmpty())
                return MvcHtmlString.Empty;

            return html.Partial("Partial/FormGridCurrent/SetAutoPersistColumns", gridCurrentGetAutoPersistColumns);
        }

        public static MvcHtmlString FormReportGeneratorSettings(this HtmlHelper html)
        {
            return html.Partial("Partial/FormGridCurrent/ReportGeneratorSettings");
        }

        public static MvcHtmlString FormGridSettingsAdministration(this HtmlHelper html, Type type)
        {
            // based on "GridAutoPersistColumnsProvider" system
            IGridColumnsAutoPersistProvider controller;
            if (!html.FormGetGridColumnsAutoPersistProvider(type, out controller))
                return MvcHtmlString.Empty;


            var adminController = (html.ViewContext.Controller as IGridSettingsAdministrationProvider);
            if (adminController == null || !adminController.GridSettingsAdminMode)
                return MvcHtmlString.Empty;

            return html.Partial("Partial/FormGridCurrent/GridSettingsAdministration");
        }

        #endregion
    }
}
