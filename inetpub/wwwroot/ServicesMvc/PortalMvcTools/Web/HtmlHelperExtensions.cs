using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.WebPages;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Services;
using MvcTools.Models;
using MvcTools.Web;
using System.Web.Mvc.Ajax;
using PortalMvcTools.Models;
using CkgDomainLogic.General.Contracts;
using GeneralTools.Contracts;
using MvcTools.Contracts;

namespace PortalMvcTools.Web
{
    public static class PortalHtmlHelperExtensions
    {
        #region Divs

        public static MvcHtmlString DivLinie(this HtmlHelper html, int? marginLeft = null, int? marginTop = null, int? marginBottom = null)
        {
            return html.EmptyDiv("linie", marginLeft: marginLeft, marginTop: marginTop, marginBottom: marginBottom);
        }

        public static MvcHtmlString FormNewLine(this HtmlHelper html, string additionalCssClass = "top10")
        {
            return html.EmptyDiv("trenner " + additionalCssClass);
        }

        public static MvcHtmlString DivMetaTrenner(this HtmlHelper html)
        {
            return html.EmptyDiv("metatrenner");
        }
        public static MvcHtmlString DivMetaLinks(this HtmlHelper html)
        {
            return html.EmptyDiv("metalinks");
        }
        public static MvcHtmlString DivMetaRechts(this HtmlHelper html)
        {
            return html.EmptyDiv("metarechts");
        }

        // ReSharper disable UnusedParameter.Local
        private static MvcTag EmptyDivMvcTag(this HtmlHelper html, string divClass, int? paddingLeft = null, int? paddingTop = null, int? paddingBottom = null, int? marginLeft = null, int? marginTop = null, int? marginBottom = null)
        // ReSharper restore UnusedParameter.Local
        {
            return new MvcTag(null, "div", cssClass: divClass, paddingLeft: paddingLeft, paddingTop: paddingTop, paddingBottom: paddingBottom, marginLeft: marginLeft, marginTop: marginTop, marginBottom: marginBottom);
        }

        public static MvcHtmlString EmptyDiv(this HtmlHelper html, string divClass, int? paddingLeft = null, int? paddingTop = null, int? paddingBottom = null, int? marginLeft = null, int? marginTop = null, int? marginBottom = null)
        {
            var tag = html.EmptyDivMvcTag(divClass, paddingLeft: paddingLeft, paddingTop: paddingTop, paddingBottom: paddingBottom, marginLeft: marginLeft, marginTop: marginTop, marginBottom: marginBottom);

            return MvcHtmlString.Create(tag.Begin() + "&nbsp;" + tag.End());
        }


        #endregion


        #region Misc


        public static MvcHtmlString FormClear(this HtmlHelper html, int formID, int marginLeft = 0)
        {
            var outerTag = new MvcTag(null, "div", cssClass: "recyclebutton", marginLeft: marginLeft, onClick: string.Format("ClearModelForm({0});", formID));

            var helpLayer = new TagBuilder("div");
            helpLayer.AddCssClass("helplayer");

            var p = new TagBuilder("p") { InnerHtml = "Leert dieses Formular." };
            helpLayer.InnerHtml = p.ToString();

            return MvcHtmlString.Create(outerTag.Begin() + helpLayer + outerTag.End());
        }

        public static MvcHtmlString FormAction(this HtmlHelper html, string javascriptAction, string cssClass, string toolTip = null, string id = null, bool hidden = false)
        {
            var outerTag = new TagBuilder("div");
            outerTag.Attributes.Add("class", cssClass);
            if (javascriptAction.IsNotNullOrEmpty())
                outerTag.Attributes.Add("onclick", javascriptAction);
            if (!id.IsNullOrEmpty())
                outerTag.Attributes.Add("id", id);
            if (hidden)
                outerTag.Attributes.Add("style", "display:none;");

            if (!toolTip.IsNullOrEmpty())
            {
                var helpLayer = new TagBuilder("div");
                helpLayer.AddCssClass("helplayer");

                var p = new TagBuilder("p") { InnerHtml = toolTip };
                helpLayer.InnerHtml = p.ToString();

                outerTag.InnerHtml = helpLayer.ToString();
            }

            return MvcHtmlString.Create(outerTag.ToString());
        }

        #endregion


        #region Misc Partial Views

        public static MvcWrapper Wrapper(this HtmlHelper html, string partialViewName, object model = null)
        {
            return new MvcWrapper(html.ViewContext, partialViewName, model);
        }

        public static MvcWrapper FormSearchBox(this HtmlHelper html, object model = null)
        {
            return new MvcWrapper(html.ViewContext, "FormSearchBox", model);
        }

        public static MvcWrapper FormSearchResultsGrid(this HtmlHelper html, object model = null)
        {
            return new MvcWrapper(html.ViewContext, "FormSearchResultsGrid", model);
        }

        public static MvcWrapper FormSearchResults(this HtmlHelper html, object model = null)
        {
            return new MvcWrapper(html.ViewContext, "FormSearchResults", model);
        }

        public static MvcWrapper PortletBox(this HtmlHelper html, string header, string iconCssClass, string portletCssClass = "light-grey", bool isCollapsible = true, bool isClosable = false)
        {
            return new MvcWrapper(html.ViewContext, "PortletBox", new FormOuterLayerModel
            {
                Header = header,
                IconCssClass = iconCssClass,
                PortletCssClass = portletCssClass,
                IsCollapsible = isCollapsible,
                IsClosable = isClosable
            });
        }

        public static MvcHtmlString FormValidationSummaryResponsive(this HtmlHelper html, Func<Exception, IHtmlString> responsiveErrorUrlFunction = null)
        {
            html.ViewBag.ResponsiveErrorUrlFunction = responsiveErrorUrlFunction;
            return html.Partial("Partial/FormValidationSummaryResponsive", html.ViewData.ModelState);
        }

        public static MvcHtmlString IeWarning(this HtmlHelper html, int ieVersion, bool ieExplicitVersion = false)
        {
            html.ViewBag.IeVersion = ieVersion;
            html.ViewBag.IeExplicitVersion = ieExplicitVersion;
            html.ViewBag.IeWarningMessage =
                string.Format("Diese Seite ist nicht kompatibel mit Browsern vom Typ 'Internet Explorer', Version {0} {1}",
                                ieVersion, (ieExplicitVersion ? "" : " und ältere Versionen")
                );

            return html.Partial("Partial/BrowserWarnings/IeWarning");
        }

        public static MvcHtmlString FormBreadCrumbs(this HtmlHelper html, string firstBreadCrumbText)
        {
            html.ViewBag.FirstBreadCrumbText = firstBreadCrumbText;
            return html.Partial("Partial/BreadCrumbs");
        }

        public static MvcHtmlString FormValidationSummary(this HtmlHelper html, bool excludePropertyErrors = true)
        {
            var persistenceModeErros = html.ViewData.ModelState.FirstOrDefault(ms => ms.Value.Errors != null && ms.Value.Errors.Any(error => error.ErrorMessage.ToLower().StartsWith(MvcTag.FormPersistenceModeErrorPrefix.ToLower())));
            var validationSummaryHeader = Localize.PleaseCheckYourInputs;
            if (persistenceModeErros.Key != null && persistenceModeErros.Value != null)
                validationSummaryHeader = MvcTag.FormPersistenceModeErrorPrefix;

            return html.ValidationSummary(excludePropertyErrors, validationSummaryHeader);
        }

        public static MvcHtmlString FormWizard(this HtmlHelper html, string headerIconCssClass, string header, IEnumerable<string> stepTitles, IEnumerable<string> stepKeys = null, bool stepTitlesInNewLine = false)
        {
            var model = new FormWizardModel
            {
                Header = header,
                HeaderIconCssClass = headerIconCssClass,
                StepTitles = stepTitles.Select(t => new HtmlString(t)).ToArray(),
                StepKeys = stepKeys.ToArrayOrEmptyArray(),
                StepTitlesInNewLine = stepTitlesInNewLine,
            };

            return html.Partial("Partial/FormWizard", model);
        }

        public static MvcHtmlString FormRequiredFieldsSummary(this HtmlHelper html, Func<Exception, IHtmlString> responsiveErrorUrlFunction = null)
        {
            html.ViewBag.ResponsiveErrorUrlFunction = responsiveErrorUrlFunction;
            return html.Partial("Partial/FormRequiredFieldsSummary", html.ViewData.ModelState);
        }

        public static MvcForm AutoForm<T>(this AjaxHelper ajax, T model, string controllerName, int id) where T : class
        {
            return ajax.BeginForm(typeof(T).Name + "Form", controllerName, null,
                                  new MvcAjaxOptions { UpdateTargetId = ajax.AutoFormWrapperDivID(id), OnComplete = "AjaxFormComplete(" + id + ");" },
                                  htmlAttributes: new { @class = "form-horizontal", id = "AjaxForm" + id });
        }

        public static string AutoFormWrapperDivID(this AjaxHelper ajax, int id)
        {
            return string.Format("Div_{0}", id);
        }

        #endregion


        #region Default Form Controls, Twitter Bootstrap

        private const string PartialViewNameFormLeftLabelControl = "Partial/FormControls/Form/LeftLabelControl";

        # region Label

        public static MvcHtmlString FormLabel(this HtmlHelper html, string propertyName, string cssClass = null)
        {
            return html.Label(propertyName, new { @class = cssClass });
        }

        public static MvcHtmlString FormLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string cssClass = null, string labelText = null)
        {
            var model = new FormControlModel
            {
                DisplayNameHtml = html.DisplayNameFor(expression),
                RequiredIndicatorHtml = html.RequiredIndicatorFor(expression),
                PerstistenceIndicatorHtml = html.PersistenceIndicatorFor(expression),
                ModelTypeName = typeof(TModel).GetFullTypeName(),
                PropertyName = expression.GetPropertyName(),
            };

            return html.Partial("Partial/FormControls/Form/LeftLabel", model);
        }

        private static string GetKnockoutDataBindAttributeValue(string propertyName, string controlType = "")
        {
            var dataBindPropertyValue = string.Format("value: {0}", propertyName);
            if (controlType == "textbox")
                dataBindPropertyValue = dataBindPropertyValue + ", valueUpdate:'afterkeydown'";

            if (controlType.IsNotNullOrEmpty())
            {
                if (controlType == "checkbox")
                    dataBindPropertyValue = string.Format("checkedUniform: {0}", propertyName);

                //if (controlType == "datepicker")
                //    dataBindPropertyValue = string.Format("value: eval('new ' + {0}.slice(1,-1)).toString('dd.MM.yyyy')", propertyName);
            }

            return dataBindPropertyValue;
        }

        private static IDictionary<string, object> MergeKnockoutDataBindAttributes(object controlHtmlAttributes, string propertyName, string controlType = "")
        {
            var dict = controlHtmlAttributes.ToHtmlDictionary();

            try
            {
                if (!dict.ContainsKey("data-bind"))
                    return dict;

                object existingDataBindPropertyValue;
                dict.TryGetValue("data-bind", out existingDataBindPropertyValue);
                if (existingDataBindPropertyValue == null)
                    return dict;

                var knockoutDataBindAttributeValue = GetKnockoutDataBindAttributeValue(propertyName, controlType);
                var s = existingDataBindPropertyValue.ToString().Trim();
                if (s.StartsWith("xauto"))
                    dict["data-bind"] = knockoutDataBindAttributeValue + s.Replace("xauto", "");
            }
            catch { }

            return dict;
        }

        #endregion

        #region TextBox, TextArea

        static object GetAutoPostcodeCityMapping<TModel, TValue>(Expression<Func<TModel, TValue>> expression, object controlHtmlAttributes)
        {
            var modelType = typeof (TModel);

            var mExpr = expression.Body as MemberExpression;
            if (mExpr != null)
                modelType = mExpr.Expression.Type;
            
            var cityPropertyName = expression.GetPropertyName();
            var cityProperty = modelType.GetProperty(cityPropertyName);
            if (cityProperty == null)
                return controlHtmlAttributes;

            var postcodeCityMappingAttribute = cityProperty.GetCustomAttributes(typeof(AddressPostcodeCityMappingAttribute), true).OfType<AddressPostcodeCityMappingAttribute>().FirstOrDefault();
            if (postcodeCityMappingAttribute == null)
                return controlHtmlAttributes;

            var postCodePropertyName = postcodeCityMappingAttribute.PostCodePropertyName;
            var countryPropertyName = postcodeCityMappingAttribute.CountryPropertyName.NotNullOrEmpty();

            return TypeMerger.MergeTypes(controlHtmlAttributes, new
            {
                onfocus = string.Format("AutoPostcodeCityMapping_OnFocus($(this), '{0}', '{1}')", postCodePropertyName, countryPropertyName)
            });
        }

        static object GetMaxLengthAttribute<TModel, TValue>(Expression<Func<TModel, TValue>> expression, object controlHtmlAttributes)
        {
            var propertyName = expression.GetPropertyName();
            var property = typeof(TModel).GetProperty(propertyName);
            if (property == null)
                return controlHtmlAttributes;

            var attribute = property.GetCustomAttributes(typeof(LengthAttribute), true).OfType<LengthAttribute>().FirstOrDefault();
            if (attribute == null)
                return controlHtmlAttributes;

            return TypeMerger.MergeTypes(controlHtmlAttributes, new { maxlength = attribute.Length });
        }

        static IDictionary<string, object> MergeKennzeichenAttributes<TModel, TValue>(Expression<Func<TModel, TValue>> expression, IDictionary<string, object> controlHtmlAttributesDict)
        {
            var propertyName = expression.GetPropertyName();
            var property = typeof(TModel).GetProperty(propertyName);
            if (property == null)
                return controlHtmlAttributesDict;

            var attribute = property.GetCustomAttributes(typeof(IKennzeichenAttribute), true).OfType<IKennzeichenAttribute>().FirstOrDefault();
            if (attribute == null)
                return controlHtmlAttributesDict;

            var additionalClassName = "kennzeichen";
            var classPropertyValue = "";
            if (controlHtmlAttributesDict.ContainsKey("class"))
                classPropertyValue = (string)controlHtmlAttributesDict["class"];

            if (!classPropertyValue.ToLower().Split(' ').Contains(additionalClassName))
            {
                classPropertyValue += (classPropertyValue.IsNotNullOrEmpty() ? " " : "") + additionalClassName;
                controlHtmlAttributesDict["class"] = classPropertyValue;
            }

            controlHtmlAttributesDict["onkeypress"] = "return KennzeichenEnforceCleanUp();";

            return controlHtmlAttributesDict;
        }

        public static MvcHtmlString FormTextBlockFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object controlHtmlAttributes = null, string iconCssClass = null, string labelText = null, bool labelHidden = false)
        {
            html.FormLeftLabelControlConditionalInit();

            var controlHtmlAttributesDict = MergeKnockoutDataBindAttributes(controlHtmlAttributes, expression.GetPropertyName(), "textblock");
            controlHtmlAttributesDict = MergeKennzeichenAttributes(expression, controlHtmlAttributesDict);
            CheckSetPartialViewContextIsFormControlHidingAvailable(controlHtmlAttributesDict, typeof(TModel));

            var model = new FormControlModel
            {
                DisplayNameHtml = (labelText.IsNullOrEmpty() ? html.DisplayNameFor(expression) : new MvcHtmlString(labelText)),
                LabelHidden = labelHidden,
                RequiredIndicatorHtml = html.RequiredIndicatorFor(expression, hideAsteriskTag: true),
                PerstistenceIndicatorHtml = MvcHtmlString.Empty,
                ControlHtml = html.TextBlockFor(expression, controlHtmlAttributesDict),
                IconCssClass = iconCssClass,
                ControlHtmlAttributes = controlHtmlAttributesDict,
                ModelTypeName = typeof(TModel).GetFullTypeName(),
                PropertyName = expression.GetPropertyName(),
            };

            return html.FormLeftLabelControlConditional(expression, model);
        }

        public static bool FormIsInvisible<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return (html.DisplayNameFor(expression).ToString().NotNullOrEmpty().ToUpper() == "$HIDDEN$");
        }

        public static MvcHtmlString FormTextBox(this HtmlHelper html, string propertyName, object controlHtmlAttributes = null, string labelHtml = null)
        {
            var model = new FormControlModel
            {
                DisplayNameHtml = labelHtml.IsNotNullOrEmpty() ? new MvcHtmlString(labelHtml) : html.DisplayName(propertyName),
                RequiredIndicatorHtml = labelHtml.IsNotNullOrEmpty() ? MvcHtmlString.Empty : html.RequiredIndicator(propertyName),
                PerstistenceIndicatorHtml = html.PersistenceIndicator(propertyName),
                ControlHtml = html.TextBox(propertyName, null, controlHtmlAttributes),
                ValidationMessageHtml = html.ValidationMessage(propertyName),
                IconCssClass = null,
                ControlHtmlAttributes = controlHtmlAttributes.ToHtmlDictionary(),
            };

            return html.FormLeftLabelControl(model);
        }

        public static MvcHtmlString FormPlaceholderTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object controlHtmlAttributes = null, string iconCssClass = null, string labelText = null)
        {
            controlHtmlAttributes = GetAutoPostcodeCityMapping(expression, controlHtmlAttributes);
            controlHtmlAttributes = TypeMerger.MergeTypes(controlHtmlAttributes, new { placeholder = html.DisplayNameFor(expression).ToString() });
            var controlHtmlAttributesDict = MergeKnockoutDataBindAttributes(controlHtmlAttributes, expression.GetPropertyName(), "textbox");
            controlHtmlAttributesDict = MergeKennzeichenAttributes(expression, controlHtmlAttributesDict);
            CheckSetPartialViewContextIsFormControlHidingAvailable(controlHtmlAttributesDict, typeof(TModel));

            var model = new FormControlModel
            {
                DisplayNameHtml = (labelText.IsNullOrEmpty() ? html.DisplayNameFor(expression) : new MvcHtmlString(labelText)),
                RequiredIndicatorHtml = html.RequiredIndicatorFor(expression),
                PerstistenceIndicatorHtml = html.PersistenceIndicatorFor(expression),
                ControlHtml = html.TextBoxFor(expression, controlHtmlAttributesDict),
                ValidationMessageHtml = html.ValidationMessageFor(expression),
                IconCssClass = iconCssClass,
                ControlHtmlAttributes = controlHtmlAttributesDict,
                ModelTypeName = typeof(TModel).GetFullTypeName(),
                PropertyName = expression.GetPropertyName(),
            };

            return html.Partial("Partial/FormControls/Form/ControlWithPlaceholder", model);
        }

        public static MvcHtmlString FormTextAreaFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object controlHtmlAttributes = null, string iconCssClass = null, int columns = 40, int rows = 4, string labelText = null)
        {
            html.FormLeftLabelControlConditionalInit();

            var controlHtmlAttributesDict = MergeKnockoutDataBindAttributes(controlHtmlAttributes, expression.GetPropertyName(), "textbox");
            CheckSetPartialViewContextIsFormControlHidingAvailable(controlHtmlAttributesDict, typeof(TModel));

            var model = new FormControlModel
            {
                DisplayNameHtml = (labelText.IsNullOrEmpty() ? html.DisplayNameFor(expression) : new MvcHtmlString(labelText)),
                RequiredIndicatorHtml = html.RequiredIndicatorFor(expression),
                PerstistenceIndicatorHtml = html.PersistenceIndicatorFor(expression),
                ControlHtml = html.TextAreaFor(expression, rows, columns, controlHtmlAttributesDict),
                ValidationMessageHtml = html.ValidationMessageFor(expression),
                IconCssClass = iconCssClass,
                ControlHtmlAttributes = controlHtmlAttributesDict,
                ModelTypeName = typeof(TModel).GetFullTypeName(),
                PropertyName = expression.GetPropertyName(),
            };

            return html.FormLeftLabelControlConditional(expression, model);
        }

        public static MvcHtmlString FormTemplateControl(this HtmlHelper html, string label, Func<object, HelperResult> templateControlHtml, object controlHtmlAttributes = null)
        {
            var model = new FormControlModel
            {
                DisplayNameHtml = new MvcHtmlString(label),
                RequiredIndicatorHtml = MvcHtmlString.Empty,
                PerstistenceIndicatorHtml = MvcHtmlString.Empty,
                ControlHtml = new MvcHtmlString(templateControlHtml.Invoke(null).ToString()),
                ValidationMessageHtml = null,
                IconCssClass = null,
                ControlHtmlAttributes = controlHtmlAttributes.ToHtmlDictionary(),
            };

            return html.FormLeftLabelControl(model);
        }
        #endregion

        #region DatePicker

        public static MvcHtmlString FormDatePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object controlHtmlAttributes = null, string iconCssClass = null, Func<object, HelperResult> postControlHtml = null)
        {
            var controlHtmlAttributesDict = MergeKnockoutDataBindAttributes(controlHtmlAttributes, expression.GetPropertyName(), "datepicker");
            CheckSetPartialViewContextIsFormControlHidingAvailable(controlHtmlAttributesDict, typeof(TModel));

            return FormDatePickerForInner(html, expression, controlHtmlAttributesDict, iconCssClass, postControlHtml: postControlHtml);
        }

        public static MvcHtmlString FormDatePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string cssClass = "m-wrap medium", string labelCssClass = "control-label", Func<object, HelperResult> postControlHtml = null)
        {
            var htmlAttributes = FormTextBoxForGetAttributes(cssClass);

            return FormDatePickerForInner(html, expression, htmlAttributes, postControlHtml: postControlHtml);
        }

        private static MvcHtmlString FormDatePickerForInner<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> controlHtmlAttributes = null, string iconCssClass = null, string labelText = null, Func<object, HelperResult> postControlHtml = null)
        {
            html.FormLeftLabelControlConditionalInit();

            var formatString = "{0:d}";
            var datePickerFor = html.TextBoxFor(expression, formatString, controlHtmlAttributes)
                                    .Concat(new MvcHtmlString("<span class=\"add-on\"><i class=\"icon-calendar\"></i></span>"));
            datePickerFor = FormControlForGetSurroundingDiv(datePickerFor, "input-append datepicker");

            var model = new FormControlModel
            {
                DisplayNameHtml = (labelText.IsNullOrEmpty() ? html.DisplayNameFor(expression) : new MvcHtmlString(labelText)),
                RequiredIndicatorHtml = html.RequiredIndicatorFor(expression),
                PerstistenceIndicatorHtml = html.PersistenceIndicatorFor(expression),
                ControlHtml = datePickerFor,
                ValidationMessageHtml = html.ValidationMessageFor(expression),
                IconCssClass = iconCssClass,
                ControlHtmlAttributes = controlHtmlAttributes,
                PostControlHtml = postControlHtml == null ? null : postControlHtml.Invoke(null),
                ModelTypeName = typeof(TModel).GetFullTypeName(),
                PropertyName = expression.GetPropertyName(),
            };

            return html.FormLeftLabelControlConditional(expression, model);
        }

        #endregion

        #region DropDownList

        private static FormControlModel GetFormPlaceHolderModel(Func<object, HelperResult> controlHtml, object controlHtmlAttributes)
        {
            var model = new FormControlModel
            {
                PostControlHtml = controlHtml == null ? null : controlHtml.Invoke(null),
                ControlHtmlAttributes = controlHtmlAttributes == null ? null : controlHtmlAttributes.ToHtmlDictionary(),
            };

            return model;
        }

        public static MvcHtmlString FormPlaceHolder(this HtmlHelper html, Func<object, HelperResult> controlHtml = null, object controlHtmlAttributes = null)
        {
            var model = GetFormPlaceHolderModel(controlHtml, controlHtmlAttributes);

            return html.FormLeftLabelControl(model);
        }

        public static MvcHtmlString FormDropDownListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList, object controlHtmlAttributes = null, Func<object, HelperResult> preControlHtml = null, Func<object, HelperResult> postControlHtml = null)
        {
            return html.FormDropDownListForInner(expression, selectList, controlHtmlAttributes, preControlHtml, postControlHtml);
        }

        public static MvcHtmlString FormDropDownListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectItem> selectList, object controlHtmlAttributes = null, Func<object, HelperResult> preControlHtml = null, Func<object, HelperResult> postControlHtml = null)
        {
            return html.FormDropDownListFor(expression, selectList.ToSelectList(), controlHtmlAttributes, preControlHtml, postControlHtml);
        }

        public static MvcHtmlString FormDropDownListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IEnumerable<string> selectList,
                                                                object controlHtmlAttributes = null, Func<object, HelperResult> preControlHtml = null, Func<object, HelperResult> postControlHtml = null)
        {
            return html.FormDropDownListFor(expression, selectList.ToSelectList(), controlHtmlAttributes, preControlHtml, postControlHtml);
        }

        private static MvcHtmlString FormDropDownListForInner<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList, object controlHtmlAttributes = null, Func<object, HelperResult> preControlHtml = null, Func<object, HelperResult> postControlHtml = null, string labelText = null)
        {
            html.FormLeftLabelControlConditionalInit();

            var controlHtmlAttributesDict = MergeKnockoutDataBindAttributes(controlHtmlAttributes, expression.GetPropertyName(), "dropdown");
            CheckSetPartialViewContextIsFormControlHidingAvailable(controlHtmlAttributesDict, typeof(TModel));

            var model = new FormControlModel
            {
                DisplayNameHtml = (labelText.IsNullOrEmpty() ? html.DisplayNameFor(expression) : new MvcHtmlString(labelText)),
                RequiredIndicatorHtml = html.RequiredIndicatorFor(expression),
                PerstistenceIndicatorHtml = html.PersistenceIndicatorFor(expression),
                ControlHtml = html.DropDownListFor(expression, selectList, controlHtmlAttributesDict),
                ValidationMessageHtml = html.ValidationMessageFor(expression),
                IconCssClass = "",
                ControlHtmlAttributes = controlHtmlAttributesDict,
                PreControlHtml = preControlHtml == null ? null : preControlHtml.Invoke(null),
                PostControlHtml = postControlHtml == null ? null : postControlHtml.Invoke(null),
                ModelTypeName = typeof(TModel).GetFullTypeName(),
                PropertyName = expression.GetPropertyName(),
            };

            return html.FormLeftLabelControlConditional(expression, model);
        }

        public static MvcHtmlString FormDropDownList<TModel>(this HtmlHelper<TModel> html, string propertyName, IEnumerable<SelectListItem> selectList, object controlHtmlAttributes = null, Func<object, HelperResult> preControlHtml = null, Func<object, HelperResult> postControlHtml = null, string labelText = null)
        {
            var controlHtmlAttributesDict = MergeKnockoutDataBindAttributes(controlHtmlAttributes, propertyName, "dropdown");

            var model = new FormControlModel
            {
                DisplayNameHtml = (String.IsNullOrEmpty(labelText) ? html.DisplayName(propertyName) : new MvcHtmlString(labelText)),
                RequiredIndicatorHtml = html.RequiredIndicator(propertyName),
                PerstistenceIndicatorHtml = html.PersistenceIndicator(propertyName),
                ControlHtml = html.DropDownList(propertyName, selectList, controlHtmlAttributesDict),
                ValidationMessageHtml = html.ValidationMessage(propertyName),
                IconCssClass = "",
                ControlHtmlAttributes = controlHtmlAttributesDict,
                PreControlHtml = preControlHtml == null ? null : preControlHtml.Invoke(null),
                PostControlHtml = postControlHtml == null ? null : postControlHtml.Invoke(null),
            };

            return html.Partial("Partial/FormControls/Form/LeftLabelControl", model);
        }

        public static MvcHtmlString FormMultiSelectListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList, 
                                                                object controlHtmlAttributes = null, Func<object, HelperResult> preControlHtml = null, Func<object, HelperResult> postControlHtml = null)
        {
            return html.FormMultiSelectListForInner(expression, selectList, controlHtmlAttributes, null, preControlHtml, postControlHtml);
        }

        public static MvcHtmlString FormMultiSelectListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IEnumerable<string> selectList,
                                                                object controlHtmlAttributes = null, Func<object, HelperResult> preControlHtml = null, Func<object, HelperResult> postControlHtml = null)
        {
            return html.FormMultiSelectListForInner(expression, selectList.ToMultiSelectList(), controlHtmlAttributes, null, preControlHtml, postControlHtml);
        }

        private static MvcHtmlString FormMultiSelectListForInner<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList, 
            object controlHtmlAttributes = null, string labelText = null, 
            Func<object, HelperResult> preControlHtml = null, Func<object, HelperResult> postControlHtml = null)
        {
            html.FormLeftLabelControlConditionalInit();

            var controlHtmlAttributesDict = controlHtmlAttributes.MergePropertiesStrictly(new { multiple = "multiple", @class = "hide" });
            controlHtmlAttributesDict.Add("data-placeholder", "..."); // because of the hyphen it is necessary to add this attribute here and not right above
            controlHtmlAttributesDict = MergeKnockoutDataBindAttributes(controlHtmlAttributesDict, expression.GetPropertyName(), "multiselect");
            CheckSetPartialViewContextIsFormControlHidingAvailable(controlHtmlAttributesDict, typeof(TModel));

            var model = new FormControlModel
            {
                DisplayNameHtml = (labelText.IsNullOrEmpty() ? html.DisplayNameFor(expression) : new MvcHtmlString(labelText)),
                RequiredIndicatorHtml = html.RequiredIndicatorFor(expression),
                PerstistenceIndicatorHtml = html.PersistenceIndicatorFor(expression),
                ControlHtml = html.ListBoxFor(expression, selectList, controlHtmlAttributesDict),
                ValidationMessageHtml = html.ValidationMessageFor(expression),
                IconCssClass = "",
                ControlHtmlAttributes = controlHtmlAttributesDict,
                PreControlHtml = preControlHtml == null ? null : preControlHtml.Invoke(null),
                PostControlHtml = postControlHtml == null ? null : postControlHtml.Invoke(null),
                ModelTypeName = typeof(TModel).GetFullTypeName(),
                PropertyName = expression.GetPropertyName(),
            };

            return html.FormLeftLabelControlConditional(expression, model);
        }

        #endregion

        #region RadioButtonList

        public static MvcHtmlString FormRadioButtonListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string selectOptions, object controlHtmlAttributes = null)
        {
            return html.FormRadioButtonListFor(expression, selectOptions.ToSelectList(), controlHtmlAttributes);
        }

        public static MvcHtmlString FormRadioButtonListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectItem> selectList, object controlHtmlAttributes = null)
        {
            return html.FormRadioButtonListFor(expression, new SelectList(selectList, "Key", "Text"), controlHtmlAttributes);
        }

        public static MvcHtmlString FormRadioButtonListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList, object controlHtmlAttributes = null, string iconCssClass = null, string labelText = null)
        {
            html.FormLeftLabelControlConditionalInit();

            var radioButtonsFor = MvcHtmlString.Empty.Concat(selectList.Select(item => html.FormRadioButtonForInner(expression, item)).ToArray());
            CheckSetPartialViewContextIsFormControlHidingAvailable(controlHtmlAttributes.ToHtmlDictionary(), typeof(TModel));

            var model = new FormControlModel
            {
                DisplayNameHtml = (labelText.IsNullOrEmpty() ? html.DisplayNameFor(expression) : new MvcHtmlString(labelText)),
                RequiredIndicatorHtml = html.RequiredIndicatorFor(expression),
                PerstistenceIndicatorHtml = html.PersistenceIndicatorFor(expression),
                ControlHtml = radioButtonsFor,
                ValidationMessageHtml = html.ValidationMessageFor(expression),
                IconCssClass = iconCssClass,
                ControlHtmlAttributes = controlHtmlAttributes.ToHtmlDictionary(),
                ModelTypeName = typeof(TModel).GetFullTypeName(),
                PropertyName = expression.GetPropertyName(),
            };

            return html.FormLeftLabelControlConditional(expression, model);
        }

        private static MvcHtmlString FormRadioButtonForInner<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, SelectListItem item, string radioLabelCssClass = "radio")
        {
            var radio = html.RadioButtonFor(expression, item.Value, new { style = "opacity:0;" })
                            .Concat(new MvcHtmlString(" " + item.Text));  // leading html space is necessary in bootstrap for checkboxes and radiobuttons

            return FormControlForGetSurroundingDiv(radio, radioLabelCssClass, "label");
        }

        #endregion

        #region CheckBox, CheckBoxList

        public static MvcHtmlString FormCheckBoxFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, bool>> expression, object controlHtmlAttributes = null, string iconCssClass = null, Func<object, HelperResult> preControlHtml = null, Func<object, HelperResult> postControlHtml = null, bool labelHidden = false, string labelText = null)
        {
            html.FormLeftLabelControlConditionalInit();

            var controlHtmlAttributesDict = MergeKnockoutDataBindAttributes(controlHtmlAttributes, expression.GetPropertyName(), "checkbox");
            CheckSetPartialViewContextIsFormControlHidingAvailable(controlHtmlAttributesDict, typeof(TModel));

            var model = new FormControlModel
            {
                IsCheckBox = true,
                LabelHidden = labelHidden,
                DisplayNameHtml = (labelText.IsNullOrEmpty() ? html.DisplayNameFor(expression) : new MvcHtmlString(labelText)),
                RequiredIndicatorHtml = html.RequiredIndicatorFor(expression),
                PerstistenceIndicatorHtml = html.PersistenceIndicatorFor(expression),
                ControlHtml = html.CheckBoxFor(expression, controlHtmlAttributesDict), // MJE, deactivated this explicitely for knockout bindings:  .MergePropertiesStrictly(new { @class = "hide" })), 
                ValidationMessageHtml = html.ValidationMessageFor(expression),
                IconCssClass = iconCssClass,
                ControlHtmlAttributes = controlHtmlAttributesDict,
                PreControlHtml = preControlHtml == null ? null : preControlHtml.Invoke(null),
                PostControlHtml = postControlHtml == null ? null : postControlHtml.Invoke(null),
                ModelTypeName = typeof(TModel).GetFullTypeName(),
                PropertyName = expression.GetPropertyName(),
            };

            return html.FormLeftLabelControlConditional(expression, model);
        }

        public static MvcHtmlString FormCheckBoxListFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, object>> labelExpression, params Expression<Func<TModel, bool>>[] expressionArray)
        {
            return html.FormCheckBoxListForInner(expressionArray.ToList(), html.DisplayNameFor(labelExpression));
        }

        public static MvcHtmlString FormCheckBoxListFor<TModel>(this HtmlHelper<TModel> html, string labelText, params Expression<Func<TModel, bool>>[] expressionArray)
        {
            return html.FormCheckBoxListForInner(expressionArray.ToList(), new MvcHtmlString(labelText));
        }

        public static MvcHtmlString FormCheckBoxListFor<TModel>(this HtmlHelper<TModel> html, string labelText, object controlHtmlAttributes, params Expression<Func<TModel, bool>>[] expressionArray)
        {
            return html.FormCheckBoxListForInner(expressionArray.ToList(), new MvcHtmlString(labelText), controlHtmlAttributes);
        }

        private static MvcHtmlString FormCheckBoxListForInner<TModel>(this HtmlHelper<TModel> html, List<Expression<Func<TModel, bool>>> expressionList, MvcHtmlString labelText, object controlHtmlAttributes = null)
        {
            var checkBoxesFor = MvcHtmlString.Empty.Concat(expressionList.Select(expression => html.FormCheckBoxForInner(expression)).ToArray());

            var firstExpression = expressionList[0];
            var validationMessageHtml = html.ValidationMessageFor(firstExpression);
            var model = new FormControlModel
            {
                IsCheckBox = true,
                DisplayNameHtml = labelText,
                RequiredIndicatorHtml = html.RequiredIndicatorFor(firstExpression),
                PerstistenceIndicatorHtml = html.PersistenceIndicatorFor(firstExpression),
                ControlHtml = checkBoxesFor,
                ValidationMessageHtml = validationMessageHtml,
                IconCssClass = null,
                ControlHtmlAttributes = controlHtmlAttributes == null ? null : controlHtmlAttributes.ToHtmlDictionary(),
            };

            return html.FormLeftLabelControl(model);
        }

        private static MvcHtmlString FormCheckBoxForInner<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, bool>> expression, string checkBoxCssClass = "checkbox")
        {
            var radio = html.CheckBoxFor(expression, new { style = "opacity:0;" })
                            .Concat(new MvcHtmlString(" " + html.GetDisplayName(expression))); // leading html space is necessary in bootstrap for checkboxes and radiobuttons

            return FormControlForGetSurroundingDiv(radio, checkBoxCssClass, "label");
        }

        public static MvcHtmlString FormDateRangePickerFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, DateRange>> dateRangeExpression, object controlHtmlAttributes = null, string[] dateRangeGroupsToExclude = null, string labelText = null)
        {
            html.FormLeftLabelControlConditionalInit();

            var dateRangePropertyName = dateRangeExpression.GetPropertyName();
            CheckSetPartialViewContextIsFormControlHidingAvailable(controlHtmlAttributes.ToHtmlDictionary(), typeof(TModel));

            var innerModel = new FormDateRangePickerModel
            {
                InlineDisplayNameHtml = new MvcHtmlString("Datumsbereich wählen..."),
                ControlHtml = html.CheckBox(dateRangePropertyName + ".IsSelected", controlHtmlAttributes.MergePropertiesStrictly(new { @class = "hide" })),
                ControlHtmlStartDate = html.Hidden(dateRangePropertyName + ".StartDate", null, controlHtmlAttributes),
                ControlHtmlEndDate = html.Hidden(dateRangePropertyName + ".EndDate", null, controlHtmlAttributes),
                IconCssClass = null,

                DateRangeProperty = dateRangePropertyName,
                DateRangeGroupsToExclude = dateRangeGroupsToExclude,
            };

            var innerHtml = html.Partial("Partial/FormControls/Form/DateRangePicker", innerModel);

            var model = new FormControlModel
            {
                DisplayNameHtml = (labelText.IsNullOrEmpty() ? html.DisplayNameFor(dateRangeExpression) : new MvcHtmlString(labelText)),
                RequiredIndicatorHtml = html.RequiredIndicatorFor(dateRangeExpression),
                PerstistenceIndicatorHtml = html.PersistenceIndicatorFor(dateRangeExpression),
                ControlHtml = innerHtml,
                ValidationMessageHtml = html.ValidationMessageFor(dateRangeExpression),
                IconCssClass = null,
                ControlHtmlAttributes = controlHtmlAttributes.ToHtmlDictionary(),
                ModelTypeName = typeof(TModel).GetFullTypeName(),
                PropertyName = dateRangeExpression.GetPropertyName(),
            };

            return html.FormLeftLabelControlConditional(dateRangeExpression, model);
        }

        public static MvcHtmlString FormTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object controlHtmlAttributes = null, string iconCssClass = null, Func<object, HelperResult> preControlHtml = null, Func<object, HelperResult> postControlHtml = null, string labelText = null, bool labelHidden = false)
        {
            html.FormLeftLabelControlConditionalInit();

            controlHtmlAttributes = GetAutoPostcodeCityMapping(expression, controlHtmlAttributes);
            controlHtmlAttributes = GetMaxLengthAttribute(expression, controlHtmlAttributes);
            var controlHtmlAttributesDict = MergeKnockoutDataBindAttributes(controlHtmlAttributes, expression.GetPropertyName(), "textbox");
            controlHtmlAttributesDict = MergeKennzeichenAttributes(expression, controlHtmlAttributesDict);
            CheckSetPartialViewContextIsFormControlHidingAvailable(controlHtmlAttributesDict, typeof(TModel));

            var model = new FormControlModel
            {
                DisplayNameHtml = (labelText.IsNullOrEmpty() ? html.DisplayNameFor(expression) : new MvcHtmlString(labelText)),
                RequiredIndicatorHtml = html.RequiredIndicatorFor(expression),
                PerstistenceIndicatorHtml = html.PersistenceIndicatorFor(expression),
                ControlHtml = html.TextBoxFor(expression, controlHtmlAttributesDict),
                ValidationMessageHtml = html.ValidationMessageFor(expression),
                IconCssClass = iconCssClass,
                ControlHtmlAttributes = controlHtmlAttributesDict,
                PreControlHtml = preControlHtml == null ? null : preControlHtml.Invoke(null),
                PostControlHtml = postControlHtml == null ? null : postControlHtml.Invoke(null),
                LabelHidden = labelHidden,
                ModelTypeName = typeof(TModel).GetFullTypeName(),
                PropertyName = expression.GetPropertyName(),
            };

            return html.FormLeftLabelControlConditional(expression, model);
        }

        private static void FormLeftLabelControlConditionalInit(this HtmlHelper html)
        {
            SessionHelper.SetPartialViewContextCurrent(html);
        }

        private static MvcHtmlString FormLeftLabelControlConditional<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, FormControlModel model)
        {
            var modelType = typeof(TModel);

            var mExpr = expression.Body as MemberExpression;
            if (mExpr != null)
                modelType = mExpr.Expression.Type;

            var propertyName = expression.GetPropertyName();

            var partialViewUrl = SessionHelper.GetPartialViewContextCurrent();
            if (partialViewUrl == null)
                return html.HiddenFor(expression);

            var formSettingsAdminMode = html.ViewContext.Controller.GetPropertyValueIfIs<IGridSettingsAdministrationProvider, bool>(o => o.GridSettingsAdminMode);
            var fieldIsHidden = CustomModelValidatorsProvider.IsPropertyHidden(modelType.GetFullTypeName(), propertyName);

            if (fieldIsHidden)
            {
                if (formSettingsAdminMode)
                    model.IsGrayed = true;
                else
                    model.IsCollapsed = true;
            }

            return html.Partial(PartialViewNameFormLeftLabelControl, model);
        }

        private static MvcHtmlString FormLeftLabelControl(this HtmlHelper html, FormControlModel model)
        {
            return html.Partial(PartialViewNameFormLeftLabelControl, model);
        }

        public static MvcHtmlString FormMultiColumnModeStart(this HtmlHelper html, string autoKey)
        {
            new FormControlModel().AutoMultiColumnModeStart(autoKey);

            return MvcHtmlString.Empty;
        }

        public static MvcHtmlString FormMultiColumnModeEnd(this HtmlHelper html, string autoKey)
        {
            while (!new FormControlModel().AutoMultiColumnModeEndReached(autoKey))
                html.RenderPartial(PartialViewNameFormLeftLabelControl, GetFormPlaceHolderModel(null, null));

            // explizite Freigabe für Form Control Ausblendbarkeit
            SessionHelper.SetPartialViewContextIsFormControlHidingNotAvailable(false);

            return MvcHtmlString.Empty;
        }

        static void CheckSetPartialViewContextIsFormControlHidingAvailable(IDictionary<string, object> dict, Type modelType)
        {
            if (modelType.Name.ToLower().Contains("gridadminviewmodel"))
                return;

            object propertyValue;
            dict.TryGetValue("col", out propertyValue);
            if (propertyValue == null)
                return;

            if (propertyValue.ToString().ToLower() == "left")
                // explizite Sperre für Form Control Ausblendbarkeit
                SessionHelper.SetPartialViewContextIsFormControlHidingNotAvailable(true);
        }


        #endregion

        private static MvcHtmlString FormControlForGetSurroundingDiv(MvcHtmlString controlHtml, string cssClass = "controls", string tagName = "div")
        {
            var outerDiv = new MvcTag(null, tagName, cssClass);
            return new MvcHtmlString(
                outerDiv.Begin() +
                controlHtml +
                outerDiv.End()
                );
        }

        private static IDictionary<string, object> FormTextBoxForGetAttributes(string cssClass, string placeHolder = "")
        {
            IDictionary<string, object> htmlAttributes = new Dictionary<string, object>();
            htmlAttributes.Add("placeholder", placeHolder);

            return htmlAttributes.MergeHtmlAttributes(FormControlForGetAttributes(cssClass));
        }

        private static IEnumerable<KeyValuePair<string, object>> FormControlForGetAttributes(string cssClass)
        {
            IDictionary<string, object> htmlAttributes = new Dictionary<string, object>();
            htmlAttributes.Add("class", cssClass);

            return htmlAttributes;
        }

        public static MvcHtmlString TileSimple(this HtmlHelper html, string title, string cssClass = "", string iconCssClass = "", string onclickAction = "", string bodyCssClass = "", string titleCssClass = "")
        {
            var model = new TileSimpleModel
            {
                Title = title,
                CssClass = cssClass,
                IconCssClass = iconCssClass,
                OnClickAction = onclickAction,
                BodyCssClass = bodyCssClass,
                TitleCssClass = titleCssClass
            };
            
            return html.Partial("Partial/TileSimple", model);
        }

        #endregion
    }
}
