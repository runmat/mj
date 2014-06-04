using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using GeneralTools.Models;
using MvcTools.Models;
using MvcTools.Web;
using PortalMvcTools.Models;
using Telerik.Web.Mvc.UI;
using Telerik.Web.Mvc.UI.Fluent;

namespace PortalMvcTools.Web
{
    public static class PortalHtmlHelperExtensions
    {
        private static string CR { get { return MvcTag.CR; } }

        #region Divs

        public static MvcHtmlString DivLinie(this HtmlHelper html, int? marginLeft = null, int? marginTop = null, int? marginBottom = null)
        {
            return html.EmptyDiv("linie", marginLeft: marginLeft, marginTop: marginTop, marginBottom: marginBottom);
        }

        public static MvcHtmlString FormDivLinie(this HtmlHelper html, int? marginLeft = null, int? marginTop = null, int? marginBottom = null)
        {
            return html.EmptyDiv("formLinie", marginLeft: marginLeft, marginTop: marginTop, marginBottom: marginBottom);
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

        public static MvcHtmlString FormClear(this HtmlHelper html, int formID, int marginLeft=0)
        {
            var outerTag = new MvcTag(null, "div", cssClass: "recyclebutton", marginLeft: marginLeft, onClick: string.Format("ClearModelForm({0});", formID));

            var helpLayer = new TagBuilder("div");
            helpLayer.AddCssClass("helplayer");

            var p = new TagBuilder("p") { InnerHtml = "Leert dieses Formular." };
            helpLayer.InnerHtml = p.ToString();

            return MvcHtmlString.Create(outerTag.Begin() + helpLayer + outerTag.End());
        }

        public static MvcHtmlString FormMandatoriesHint(this HtmlHelper html)
        {
            var outerTag = new MvcTag(null, "div", cssClass: "mandatoriesHint");

            return MvcHtmlString.Create(outerTag.Begin() + "* = Pflichtfelder" + outerTag.End());
        }

        public static MvcHtmlString FormAction(this HtmlHelper html, string javascriptAction, string cssClass, string toolTip=null, string id=null, bool hidden = false)
        {
            var outerTag = new TagBuilder("div");
            outerTag.Attributes.Add("class", cssClass);
            if (javascriptAction.IsNotNullOrEmpty())
                outerTag.Attributes.Add("onclick", javascriptAction);
            if (!string.IsNullOrEmpty(id))
                outerTag.Attributes.Add("id", id);
            if (hidden)
                outerTag.Attributes.Add("style", "display:none;");

            if (!string.IsNullOrEmpty(toolTip))
            {
                var helpLayer = new TagBuilder("div");
                helpLayer.AddCssClass("helplayer");

                var p = new TagBuilder("p") {InnerHtml = toolTip};
                helpLayer.InnerHtml = p.ToString();

                outerTag.InnerHtml = helpLayer.ToString();
            }

            return MvcHtmlString.Create(outerTag.ToString());
        }

        //public static MvcHtmlString HelpHint(this HtmlHelper html, string hintText)
        //{
        //    var outerTag = new TagBuilder("div");
        //    outerTag.Attributes.Add("class", "helpbutton");

        //    var helpLayer = new TagBuilder("div");
        //    helpLayer.AddCssClass("helplayer");

        //    var p = new TagBuilder("p") { InnerHtml = hintText };
        //    helpLayer.InnerHtml = p.ToString();

        //    var img = new TagBuilder("img");
        //    img.Attributes.Add("class", "helpicon");
        //    img.Attributes.Add("src", new UrlHelper(html.ViewContext.RequestContext).Content("~/images/button_help.gif"));
        //    img.Attributes.Add("width", "27");
        //    img.Attributes.Add("height", "26");
        //    img.Attributes.Add("alt", "Hilfe");

        //    outerTag.InnerHtml = helpLayer + img.ToString(TagRenderMode.SelfClosing);

        //    return MvcHtmlString.Create(outerTag.ToString());
        //}

        public static MvcHtmlString ImgTransparent(this HtmlHelper html, int? width = null, int? height = null)
        {
            var img = new TagBuilder("img");
            img.Attributes.Add("src", new UrlHelper(html.ViewContext.RequestContext).Content(@"~/images/transp.gif"));
            img.Attributes.Add("width", width.GetValueOrDefault().ToString());
            img.Attributes.Add("height", height.GetValueOrDefault().ToString());
            img.Attributes.Add("alt", "");

            return MvcHtmlString.Create(img.ToString(TagRenderMode.SelfClosing));
        }

        #endregion
    

        #region Form


        public static MvcHtmlString FormRadioButtonFor<TModel, TValue>(this HtmlHelper<TModel> html,
                                                                    Expression<Func<TModel, TValue>> expression,
                                                                    object value, string text,
                                                                    string cssClass = "FormRadio", string labelCssClass = "FormLabelNormal")
        {
            var htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            var htmlFieldId = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName);
            var htmlFieldIdNew = string.Format("{0}_{1}", htmlFieldId, value);

            var radioFor = html.RadioButtonFor(expression, value);
            radioFor = new MvcHtmlString(radioFor.ToString().Replace("id=\"" + htmlFieldId + "\"", "id=\"" + htmlFieldIdNew + "\""));

            var outerDiv = new MvcTag(null, "div", cssClass: "formselects " + cssClass);
            var labelSpan = new MvcTag(null, "span", cssClass: "radiolabel");
            var labelDiv = new MvcTag(null, "label", forAttribute: htmlFieldIdNew);

            return new MvcHtmlString(
                                        (labelCssClass != null ? html.FormLabelFor(expression, labelCssClass) : MvcHtmlString.Empty) +
                                        outerDiv.Begin() +
                                            radioFor +
                                            labelSpan.Begin() +
                                                labelDiv.Begin() +
                                                    // MJE, 15.03.2013, need these spaces for proper left mouse click notification beween radio button and his radio label string
                                                    new HtmlString("&nbsp;&nbsp;&nbsp;" + text + "&nbsp;&nbsp;&nbsp;") +
                                                labelDiv.End() +
                                            labelSpan.End() +
                                            CR +
                                        outerDiv.End());
        }

        public static MvcHtmlString FormCheckBoxFor<TModel>(this HtmlHelper<TModel> html, 
                                                                    Expression<Func<TModel, bool>> expression, 
                                                                    string cssClass = "FormCheckbox", 
                                                                    string labelCssClass = "FormLabelNormal",
                                                                    IDictionary<string, object> additionalHtmlAttributes = null)
        {
            var checkBoxFor = html.CheckBoxFor(expression, additionalHtmlAttributes);
            var labelFor = html.LabelFor(expression);

            var outerDiv = new MvcTag(null, "div", cssClass: "formselects " + cssClass);
            var labelSpan = new MvcTag(null, "span", cssClass: "checklabel");

            return new MvcHtmlString(
                                        (labelCssClass != null ? html.FormLabelFor(expression, labelCssClass) : MvcHtmlString.Empty) +
                                        outerDiv.Begin() +
                                            checkBoxFor +
                                            labelSpan.Begin() +
                                                labelFor +
                                            labelSpan.End() +
                                            CR +
                                        outerDiv.End());
        }

        public static MvcHtmlString FormDatePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression,
                                                                        string cssClass = "formtext FormDatepicker", string labelCssClass = "FormLabelNormal")
        {
            var htmlAttributes = FormTextBoxForGetAttributes(cssClass : cssClass + " jqcalendar");

            var formatString = "{0:dd.MM.yyyy}";
            var textBoxFor = html.TextBoxFor(expression, formatString, htmlAttributes).ToHtmlString();

            var datePickerDiv = new MvcTag(null, "div", cssClass: "formfeld_end_wide");
            var datePickerTag = new MvcTag(null, "img", src: html.ToAbsolutePath("~/images/icon_datepicker.gif"), cssClass: "datepicker");
            
            var datePickerMvcString = new MvcHtmlString(datePickerDiv.Begin() + datePickerTag.Render() + datePickerDiv.End());

            return (!string.IsNullOrEmpty(labelCssClass) ? html.FormLabelFor(expression, labelCssClass) : MvcHtmlString.Empty).Concat(FormTextBoxForGetSurroundingDiv(html, cssClass, textBoxFor, postTag: datePickerMvcString));
        }

        public static MvcHtmlString FormTextareaFor<TModel, TValue>(this HtmlHelper<TModel> html,
                                                                    Expression<Func<TModel, TValue>> expression,
                                                                    string cssClass = "FormTextareaNormal",
                                                                    string labelCssClass = "FormLabelNormal",
                                                                    IDictionary<string, object> additionalHtmlAttributes = null)
        {
            IDictionary<string, object> htmlAttributes = new Dictionary<string, object>();
            htmlAttributes.Add("class", cssClass);

            var textareaFor = html.TextAreaFor(expression, htmlAttributes);

            return (!string.IsNullOrEmpty(labelCssClass) ? html.FormLabelFor(expression, labelCssClass) : MvcHtmlString.Empty).Concat(textareaFor);
        }

        public static MvcHtmlString FormTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html,
                                                                    Expression<Func<TModel, TValue>> expression,
                                                                    string cssClass = "FormTextboxNormal",
                                                                    string labelCssClass = "FormLabelNormal",
                                                                    IDictionary<string, object> additionalHtmlAttributes = null)
        {
            var htmlAttributes = FormTextBoxForGetAttributes("formtext " + cssClass);
            MergeHtmlAttributes(htmlAttributes, additionalHtmlAttributes);

            var textBoxFor = html.TextBoxFor(expression, htmlAttributes).ToHtmlString();

            return (!string.IsNullOrEmpty(labelCssClass) ? html.FormLabelFor(expression, labelCssClass) : MvcHtmlString.Empty).Concat(FormTextBoxForGetSurroundingDiv(html, cssClass, textBoxFor));
        }

        public static MvcHtmlString FormAutoCompleteFor<TModel, TValue>(this HtmlHelper<TModel> html,
                                                                    Expression<Func<TModel, TValue>> expression,
                                                                    string autocompleteSelectAction,
                                                                    string autocompleteSelectController,    
                                                                    object autocompleteSelectRouteValues,
                                                                    string clientEventOnChange = "",
                                                                    string cssClass = "FormTextboxNormal",
                                                                    string labelCssClass = "FormLabelNormal",
                                                                    IDictionary<string, object> additionalHtmlAttributes = null)
        {
            var htmlAttributes = FormTextBoxForGetAttributes("formtext " + cssClass);
            MergeHtmlAttributes(htmlAttributes, additionalHtmlAttributes);

            //var autocompleteFor = html.TextBoxFor(expression, htmlAttributes).ToHtmlString();
            var autocompleteFor = html.Telerik().AutoCompleteFor(expression).AutoFill(false)
                .DataBinding(databinding => databinding.Ajax()
                                   .Select(autocompleteSelectAction, autocompleteSelectController,
                                           autocompleteSelectRouteValues))
                .Filterable(filtering =>{
                                            filtering.FilterMode(AutoCompleteFilterMode.StartsWith);
                                            filtering.MinimumChars(1);
                                        })
                .ClientEvents(c => c.OnChange(clientEventOnChange))
                .HighlightFirstMatch(true).Multiple(multi => multi.Enabled(false)).Encode(false)
                .HtmlAttributes(htmlAttributes).ToHtmlString();

            return (!string.IsNullOrEmpty(labelCssClass) ? html.FormLabelFor(expression, labelCssClass) : MvcHtmlString.Empty).Concat(FormTextBoxForGetSurroundingDiv(html, cssClass, autocompleteFor));
        }

        public static MvcHtmlString TextBlockFor<TModel, TValue>(this HtmlHelper<TModel> html,
                                                                    Expression<Func<TModel, TValue>> expression,
                                                                    string cssClass = "FormTextblockNormal",
                                                                    string labelCssClass = "FormLabelNormal")
        {
            var sValue = (string)html.GetValueInDisplayFormat(expression);

            var readOnlyTextBoxFor = html.FormLabel(sValue, cssClass, prefixCssClass:"").Concat(html.HiddenFor(expression));

            return (!string.IsNullOrEmpty(labelCssClass) ? html.FormLabelFor(expression, labelCssClass) : MvcHtmlString.Empty).Concat(readOnlyTextBoxFor);
        }

        public static MvcHtmlString FormTextBlockFor<TModel, TValue>(this HtmlHelper<TModel> html,
                                                                    Expression<Func<TModel, TValue>> expression,
                                                                    string cssClass = "FormTextblockNormal",
                                                                    string labelCssClass = "FormLabelNormal", 
                                                                    string id = null, bool hidden = false)
        {
            var sValue = (string)html.GetValueInDisplayFormat(expression);

            var readOnlyTextBoxFor = html.FormLabel(sValue, "formtext " + cssClass + " formtextblock", id: id, hidden : hidden).Concat(html.HiddenFor(expression));

            return (!string.IsNullOrEmpty(labelCssClass) ? html.FormLabelFor(expression, labelCssClass) : MvcHtmlString.Empty).Concat(readOnlyTextBoxFor);
        }

        public static MvcHtmlString FormIntegerTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html,
                                                                    Expression<Func<TModel, TValue>> expression,
                                                                    string cssClass = "FormTextboxInteger",
                                                                    string labelCssClass = "FormLabelNormal")
        {
            return html.FormNumericTextBoxFor(expression, "formtext " + cssClass, labelCssClass);
        }

        public static MvcHtmlString FormDecimalTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html,
                                                                    Expression<Func<TModel, TValue>> expression,
                                                                    string cssClass = "FormTextboxDecimal",
                                                                    string labelCssClass = "FormLabelNormal")
        {
            return html.FormNumericTextBoxFor(expression, "formtext " + cssClass, labelCssClass);
        }

        private static MvcHtmlString FormNumericTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html,
                                                                    Expression<Func<TModel, TValue>> expression,
                                                                    string cssClass = null,
                                                                    string labelCssClass = null,
                                                                    bool useDecimal = true)
        {
            var htmlAttributes = FormTextBoxForGetAttributes(cssClass); //  width: width, textAlign: "right");

            htmlAttributes.Add("onkeypress", string.Format("return NumbersOnly(event, {0})", useDecimal.ToString().ToLower()));

            var formatString = (useDecimal ? "{0:0.00}" : "{0:0}");
            var textBoxForHtml = html.TextBoxFor(expression, formatString, htmlAttributes).ToHtmlString();

            return (!string.IsNullOrEmpty(labelCssClass) ? html.FormLabelFor(expression, labelCssClass) : MvcHtmlString.Empty).Concat(FormTextBoxForGetSurroundingDiv(html, cssClass, textBoxForHtml));
        }

        private static MvcHtmlString FormTextBoxForGetSurroundingDiv<TModel>(HtmlHelper<TModel> html, 
                                                                             string cssClass, 
                                                                             string textBoxFor,
                                                                             MvcHtmlString postTag = null)
        {
            string outerCssClass;
            // wenn mehrere Klassen, den "_Outer"-Klassennamen für das umgebende DIV nur aus dem erstgenanten herleiten
            if (cssClass.Contains(' '))
            {
                outerCssClass = cssClass.Split(' ')[0] + "_Outer";
            }
            else
            {
                outerCssClass = cssClass + "_Outer";
            }
            var outerDiv = new MvcTag(null, "div", "formfeld " + outerCssClass);
            return new MvcHtmlString(
                outerDiv.Begin() + 
                html.EmptyDiv("formfeld_start") +
                textBoxFor + CR + 
                (postTag ?? html.EmptyDiv("formfeld_end")) + CR + 
                outerDiv.End());
        }

        private static IDictionary<string, object> FormTextBoxForGetAttributes(string cssClass)
        {
            IDictionary<string, object> htmlAttributes = new Dictionary<string, object>();
            htmlAttributes.Add("class", cssClass);
            
            return htmlAttributes;
        }

        public static MvcHtmlString FormLabel(this HtmlHelper html, string labelText, string cssClass, string prefixCssClass = "formname", string id = null, bool hidden = false) 
        {
            var cssArray = new List<string> {prefixCssClass, cssClass};
            cssArray.RemoveAll(string.IsNullOrEmpty);

            var hiddenStyle = (hidden ? "display:none;" : null);
            var outerDiv = new MvcTag(null, "div", cssClass: string.Join(" ", cssArray), id:id, style:hiddenStyle); 

            return new MvcHtmlString(
                    outerDiv.Begin() +
                        labelText +
                    outerDiv.End()
                );
        }

        public static MvcHtmlString FormLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string cssClass=null) //, int width = 80, int paddingLeft = 10, int additionalCssClass = 10)
        {
            return html.FormLabel(html.LabelFor(expression).ToHtmlString(), cssClass );// , width: width, paddingLeft: paddingLeft, additionalCssClass:additionalCssClass);
        }


        #region Telerik Controls

        public static MvcHtmlString FormSlider<TModel, TValue>(this HtmlHelper<TModel> html, SliderBuilder<TValue> telerikSliderFor, bool useLabel = true, int? width = null)
            where TValue : struct, IComparable
        {
            return new MvcHtmlString(telerikSliderFor.ToHtmlString());
        }

        public static MvcHtmlString FormRangeSlider<TModel, TValue>(this HtmlHelper<TModel> html, RangeSliderBuilder<TValue> telerikRangeSliderFor, bool useLabel = true, int? width = null)
            where TValue : struct, IComparable
        {
            return new MvcHtmlString(telerikRangeSliderFor.ToHtmlString());
        }

        public static MvcHtmlString FormDropDownListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string selectOptions, string cssClass = "FormDropdownWide", string labelCssClass = "FormLabelNormal")
        {
            return html.FormDropDownListFor(expression, selectOptions.ToSelectList(), cssClass, labelCssClass);
        }

        public static MvcHtmlString FormDropDownListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList, string cssClass = "FormDropdownWide", string labelCssClass = "FormLabelNormal")
        {
            return html.FormDropDownListForInner(expression, selectList, cssClass, labelCssClass);
        }

        public static MvcHtmlString FormDropDownListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectItem> selectList, string cssClass = "FormDropdownWide", string labelCssClass = "FormLabelNormal")
        {
            return html.FormDropDownListForInner(expression, new SelectList(selectList, "Key", "Text"), cssClass, labelCssClass);
        }

        public static MvcHtmlString FormDropDownListSmallFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string selectOptions, string cssClass = "FormDropdownSmall", string labelCssClass = "FormLabelNormal")
        {
            return html.FormDropDownListSmallFor(expression, selectOptions.ToSelectList(), cssClass, labelCssClass);
        }

        public static MvcHtmlString FormDropDownListSmallFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList, string cssClass = "FormDropdownSmall", string labelCssClass = "FormLabelNormal")
        {
            return html.FormDropDownListForInner(expression, selectList, cssClass, labelCssClass);
        }

        public static MvcHtmlString FormDropDownListSmallFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectItem> selectList, string cssClass = "FormDropdownSmall", string labelCssClass = "FormLabelNormal")
        {
            return html.FormDropDownListForInner(expression, new SelectList(selectList, "Key", "Text"), cssClass, labelCssClass);
        }

        private static MvcHtmlString FormDropDownListForInner<TModel, TValue>(this HtmlHelper<TModel> html,
                                                                 Expression<Func<TModel, TValue>> expression,
                                                                 IEnumerable<SelectListItem> selectList,
                                                                 string cssClass,
                                                                 string labelCssClass)
        {
            var telerikDropDownListFor = html.Telerik().DropDownListFor(expression).BindTo(selectList);

            //telerikDropDownListFor = telerikDropDownListFor.HtmlAttributes(new { style = string.Format("width:{0}px",(width-20)) });
            telerikDropDownListFor = telerikDropDownListFor.HtmlAttributes(new { @class = string.Format("{0}_Telerik",cssClass) });

            //var validationMessageFor = html.ValidationMessageFor(expression);
            var outerDiv = new MvcTag(null, "div", "formfeld-div-" + cssClass);
            return new MvcHtmlString(
                (!string.IsNullOrEmpty(labelCssClass) ? html.FormLabelFor(expression, cssClass: labelCssClass) : MvcHtmlString.Empty) +
                outerDiv.Begin() +
                telerikDropDownListFor + CR +
                //validationMessageFor +
                outerDiv.End());
        }

        #endregion

        private static void MergeHtmlAttributes(IDictionary<string, object> htmlAttributes, IEnumerable<KeyValuePair<string, object>> additionalHtmlAttributes)
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
        }

        #endregion


        #region Partial Views

        public static MvcWrapper Wrapper(this HtmlHelper html, string partialViewName, object model = null)
        {
            return new MvcWrapper(html.ViewContext, partialViewName, model);
        }

        public static MvcWrapper FormOuterLayer(this HtmlHelper html)
        {
            //var model = new FormOuterLayerModel { ID = id, Header = header, HeaderCssClass = headerCssClass };
            return html.Wrapper("FormOuterLayer");
        }

        public static MvcWrapper FormInnerLayer(this HtmlHelper html, int id, string header, string headerCssClass = null, string formOpenerCssClass = null, string formLayerAdditionalCssClass = null)
        {
            var model = new FormInnerLayerModel { ID = id, Header = header, HeaderCssClass = headerCssClass, FormOpenerCssClass = formOpenerCssClass, FormLayerAdditionalCssClass = formLayerAdditionalCssClass };
            return html.Wrapper("FormInnerLayer", model);
        }

        public static MvcHtmlString StepBar(this HtmlHelper html)
        {
            return html.Partial("Partial/StepBar");
        }

        public static MvcHtmlString BusyArrows(this HtmlHelper html, int arrowCount)
        {
            html.ViewBag.ArrowCount = arrowCount;
            return html.Partial("Partial/BusyArrows");
        }

        public static MvcHtmlString StepButtons(this HtmlHelper html, bool showHeader)
        {
            html.ViewData["showHeader"] = showHeader;

            return html.Partial("Partial/StepButtons");
        }

        public static MvcHtmlString DateRangePickerLinks(this HtmlHelper html, string datePickerProperty)
        {
            html.ViewData["DatePickerProperty"] = datePickerProperty;

            return html.Partial("Partial/DateRangePickerLinks");
        }

        #endregion


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
            var value = html.GetValueInDisplayFormat(expression);
            if (value == null || value.ToString() == "")
                value = html.GetValue(expression);

            var model = new FormControlModel
            {
                DisplayNameHtml = labelHtml.IsNotNullOrEmpty() ? new MvcHtmlString(labelHtml) : html.DisplayName(html.GetPropertyName(expression)),
                ControlHtml = html.Partial("Partial/FormControls/SimpleForm/TextBlockInner", value),
                ValidationMessageHtml = html.ValidationMessageFor(expression),
                IconCssClass = iconCssClass,
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
            };

            return html.Partial("Partial/FormControls/SimpleForm/DateRangePicker", model);
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
