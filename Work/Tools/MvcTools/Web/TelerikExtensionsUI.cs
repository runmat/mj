using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using GeneralTools.Contracts;
using GeneralTools.Models;
using MvcTools.Web;
using Telerik.Web.Mvc.UI.Fluent;

namespace Telerik.Web.Mvc.UI
{
    public static class TelerikExtensions
    {
        #region Filtered Data

        //
        // (depends on "/Scripts/TelerikExtensions.js")
        // (depends on "/Styles/TelerikExtensions.css")
        //

        /// <summary>
        /// usage, controller action: 
        /// public ActionResult FilteredDataAction(int page, string orderBy, string filterBy, string jsonColumns)
        /// </summary>
        /// <param name="html"></param>
        /// <returns>EmptyResult()</returns>
        public static MvcHtmlString FilteredDataTelerikScriptsAndStyles(this HtmlHelper html)
        {
            return html.Style("~/Shared/Styles/TelerikExtensions.css")
              .Concat(html.Script("~/Shared/Scripts/TelerikExtensions.js"));
        }

        public static GridToolBarCustomCommandBuilder<T> FilteredDataExcelCommand<T>(this GridToolBarCommandFactory<T> builder, string commandTitle, string action, string controller) where T : class
        {
            return builder.FilteredDataImageButtonCommand(commandTitle, action, controller, "excel");
        }

        public static GridToolBarCustomCommandBuilder<T> FilteredDataPdfCommand<T>(this GridToolBarCommandFactory<T> builder, string commandTitle, string action, string controller) where T : class
        {
            return builder.FilteredDataImageButtonCommand(commandTitle, action, controller, "pdf");
        }

        public static GridToolBarCustomCommandBuilder<T> FilteredDataImageButtonCommand<T>(this GridToolBarCommandFactory<T> builder, string commandTitle, string action, string controller, string imageButtonClass) where T : class
        {
            return builder.FilteredDataCommand(action, controller, commandTitle)
                .ButtonType(GridButtonType.Image)
                .HtmlAttributes(new { @class = "tgrid-command-" + imageButtonClass, title = commandTitle });
        }

        public static GridToolBarCustomCommandBuilder<T> FilteredDataCommand<T>(this GridToolBarCommandFactory<T> builder, string action, string controller, string commandTitle = "") where T : class
        {
            return builder.Custom()
                .HtmlAttributes(new { id = string.Format("{0}_FilterCommand", action), @class = "hide" })
                .Action(action, controller, new { page = 1, orderBy = "~", filterBy = "~" })
                .Text(commandTitle);
        }

        public static GridClientEventsBuilder FilteredDataOnDataBound(this GridClientEventsBuilder builder)
        {
            return builder.OnDataBound("FilteredData_Grid_OnDataBound");
        }

        public static GridClientEventsBuilder FilteredDataOnColumnReorder(this GridClientEventsBuilder builder)
        {
            return builder.OnColumnReorder("FilteredData_Grid_OnColumnReorder");
        }

        #endregion

        
        public static GridBoundColumnBuilder<T> XClientTemplateCheckBox<T>(this GridBoundColumnBuilder<T> builder)
            where T : class
        {
            return
                builder.ClientTemplate(
                    string.Format(
                        "<input type='checkbox' disabled='disabled' name='{0}' <#= {0}?\"checked='checked'\" : \"\" #> />",
                        builder.Column.Member));
        }

        public static GridBoundColumnBuilder<T> XServerTemplateDropDown<T>(this GridBoundColumnBuilder<T> builder,
                                                                          IEnumerable<object> items,
                                                                          string keyAndValue = null) where T : class
        {
            const string error =
                "Die Telerik.GridBoundColumnBuilder.ServerTemplateDropDown Extension Funktion benötigt eine Liste 'items' mit mindestens einem Eintrag!";
            if (items == null)
                throw new NotSupportedException(error);
            var itemsList = items.ToList();
            if (!itemsList.Any())
                throw new NotSupportedException(error);

            if (keyAndValue == null)
            {
                // try to extract key from Property Attribute "Key"
                var firstItem = itemsList[0];
                var keyProperty =
                    firstItem.GetType().GetProperties().FirstOrDefault(
                        p => p.GetCustomAttributes(true).OfType<KeyAttribute>().Any());
                if (keyProperty != null)
                    keyAndValue = keyProperty.Name;
            }

            var dropDownList = new SelectList(itemsList, keyAndValue, keyAndValue);

            return
                builder.EditorTemplateName("ServerTemplateDropDown").EditorViewData(
                    new KeyValuePair<string, object>(null, dropDownList));
        }

        public static GridBoundColumnBuilder<T> XServerTemplateDropDown<T>(this GridBoundColumnBuilder<T> builder,
                                                                          string commaSeparatedValues = null)
            where T : class
        {
            const string error =
                "Die Telerik.GridBoundColumnBuilder.ServerTemplateDropDown Extension Funktion benötigt eine gültige komma-separierte Liste (Parameter 'commaSeparatedValues')!";
            const string keyAndValue = "Key";
            if (commaSeparatedValues == null)
                throw new NotSupportedException(error);

            var values = commaSeparatedValues.Split(',');
            for (var i = 0; i < values.Length; i++)
                values[i] = values[i].Trim();

            return builder.XServerTemplateDropDown(values.Select(v => new {Key = v}), keyAndValue);
        }

        public static GridBoundColumnBuilder<T> XFooterAndAggregate<T>(this GridBoundColumnBuilder<T> builder,
                                                                      string aggregateFunc, string formatString,
                                                                      string align = null, string fontColor = null,
                                                                      string footerPostfix = null) where T : class
        {
            var cssStyle = "text-align: " + align + ";";
            if (!string.IsNullOrEmpty(fontColor))
                cssStyle += "color:" + fontColor + ";";

            return builder
                .HtmlAttributes(new {style = cssStyle})
                .FooterHtmlAttributes(new {style = cssStyle})
                .Aggregate(a => a.GetType().GetMethod(aggregateFunc).Invoke(a, new object[] {}))
                .Format(string.Format("{0}", formatString))
                .ClientFooterTemplate(string.Format("<#= $.telerik.formatString('∑ {0}{1}', {2}) #>", formatString,
                                                    footerPostfix, aggregateFunc));
        }

        public static GridBuilder<T> XAjaxDataBinding<T>(this GridBuilder<T> builder, string actionName, string controllerName) where T : class
        {
            return builder.DataBinding(dataBinding => dataBinding.Ajax().Select(actionName, controllerName));
        }

        public static GridBuilder<T> XAjaxDataBindingWithUpdate<T>(this GridBuilder<T> builder, string selectActionName, string updateActionName, string controllerName) where T : class
        {
            return builder.DataBinding(dataBinding => dataBinding.Ajax().Select(selectActionName, controllerName).Update(updateActionName, controllerName));
        }

        public static GridBuilder<T> XAjaxDataBindingWithUpdateAndDelete<T>(this GridBuilder<T> builder, string selectActionName, string updateActionName, string deleteActionName, string controllerName) where T : class
        {
            return builder.DataBinding(dataBinding => dataBinding.Ajax().Select(selectActionName, controllerName).Update(updateActionName, controllerName).Delete(deleteActionName, controllerName));
        }

        public static GridBuilder<T> XAjaxDataBindingWithUpdateAndInsert<T>(this GridBuilder<T> builder, string selectActionName, string updateActionName, string insertActionName, string controllerName) where T : class
        {
            return builder.DataBinding(dataBinding => dataBinding.Ajax().Select(selectActionName, controllerName).Update(updateActionName, controllerName).Insert(insertActionName, controllerName));
        }

        public static GridBuilder<T> XAjaxDataBindingWithUpdateAndInsertAndDelete<T>(this GridBuilder<T> builder, string selectActionName, string updateActionName, string insertActionName, string deleteActionName, string controllerName) where T : class
        {
            return builder.DataBinding(dataBinding => dataBinding.Ajax().Select(selectActionName, controllerName).Update(updateActionName, controllerName).Insert(insertActionName, controllerName).Delete(deleteActionName, controllerName));
        }

        public static GridBuilder<T> XAutoColumnConfiguration<T>(this GridBuilder<T> builder) where T : class
        {
            return builder.Groupable(g => g.Enabled(true))
                          .Reorderable(r => r.Columns(true))
                          .Resizable(r => r.Columns(true))
                          .ColumnContextMenu()
                          .Filterable(filter => filter.Enabled(true));
        }

        public static GridBuilder<T> XPageSize<T>(this GridBuilder<T> builder, int pageSize) where T : class
        {
            return builder.Pageable(paging => paging.PageSize(pageSize));
        }

        public static GridBuilder<T> XSort<T>(this GridBuilder<T> builder, Action<GridSortDescriptorFactory<T>> sortConfigurator) where T : class
        {
            return builder.Sortable(s => s.Enabled(true).OrderBy(sortConfigurator));
        }

        public static GridClientEventsBuilder XAutoClientEvents(this GridClientEventsBuilder builder, string gridName)
        {
            return builder.OnDataBound("OnDataBound_" + gridName)
                          .OnColumnReorder("OnColumnReorder_" + gridName)
                          .OnColumnShow("OnColumnShowHide_" + gridName)
                          .OnColumnHide("OnColumnShowHide_" + gridName);
        }


        #region Auto Grid Column Translation Configuration

        private static string GridGroup
        {
            // ReSharper disable UnusedMember.Local
            get { return SessionHelper.GetSessionString("GridGroup"); }
            // ReSharper restore UnusedMember.Local
            set { SessionHelper.SetSessionValue("GridGroup", value); }
        }

        public static TBuilder XGroup<TBuilder>(this TBuilder builder, string gridGroup)
        {
            GridGroup = gridGroup;

            return builder;
        }

        public static GridBoundColumnBuilder<TModel> XBound<TModel>(this GridColumnFactory<TModel> builder, string propertyName, bool columnVisibleOnStart = true)
            where TModel : class
        {
            var column = builder.Bound(propertyName);
            var propertyInfo = typeof(TModel).GetProperty(propertyName);

            if (propertyInfo == null)
                return column;

            var propertyType = propertyInfo.PropertyType;
           
            if (propertyType == typeof(bool) || propertyType == typeof(bool?))
               column.XClientTemplateCheckBox();

            // Wir sind im Grid, wenn vorhanden dann soll die kurze Bezeichnung verwendet werden
            var columnFormat = "";

            if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
                columnFormat = "{0:d}";

            var localizedDisplayAttribute = typeof(TModel).GetAttributeFrom<ILocalizedDisplayAttribute>(propertyName);
            if (localizedDisplayAttribute != null)
            {
                if (localizedDisplayAttribute.Suffix == null)
                {
                    column.Title(TranslationFormatService.GetTranslationKurz(localizedDisplayAttribute.ResourceID));    
                }
                else
                {
                    var title =
                        string.Concat(TranslationFormatService.GetTranslationKurz(localizedDisplayAttribute.ResourceID), " ", localizedDisplayAttribute.Suffix);
                    column.Title(title);    
                }
                
                // Format nur dann übernehmen wenn eines auch angegeben ist
                var formatAusDb = TranslationFormatService.GetFormat(localizedDisplayAttribute.ResourceID);

                if (string.IsNullOrEmpty(formatAusDb) == false)
                {
                    columnFormat = formatAusDb;    
                }
            }

            if (!columnVisibleOnStart)
                columnFormat = "X~" + columnFormat;

            var gridResponsiveVisibleAttribute = typeof(TModel).GetAttributeFrom<GridResponsiveVisibleAttribute>(propertyName);
            if (gridResponsiveVisibleAttribute != null)
            {
                var responsiveCssClass = gridResponsiveVisibleAttribute.CssClassName;

                column.HtmlAttributes(new { @class = responsiveCssClass }).HeaderHtmlAttributes(new { @class = responsiveCssClass });
            }

            if (typeof (TModel).GetAttributeFrom<GridRawHtmlAttribute>(propertyName) != null)
                column.Encoded(false);

            return column.Format(columnFormat);
        }

        public static GridBoundColumnBuilder<TModel> XBound<TModel, TValue>(this GridColumnFactory<TModel> builder, Expression<Func<TModel, TValue>> expression)
            where TModel : class
        {
            return builder.XBound(expression.GetPropertyName());
        }

        public static GridBuilder<TModel> XToolBar<TModel>(this GridBuilder<TModel> builder, string controller)
            where TModel : class
        {
            return builder.ToolBar(commands =>
                {
                    commands.FilteredDataCommand("GridDataExportFilteredExcel", controller);
                    commands.FilteredDataCommand("GridDataExportFilteredPDF", controller);
                });
        }

        /// <summary>
        /// Ermittle die Instanz für die aktuelle http Request
        /// </summary>
        public static ITranslationFormatService TranslationFormatService
        {
            get { return DependencyResolver.Current.GetService<ITranslationFormatService>(); }
        }

        #endregion
    }
}
