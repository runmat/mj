using System.Collections.Generic;
using System.IO;
using System.Linq;
using CkgDomainLogic.Charts.Contracts;
using GeneralTools.Models;
using GeneralTools.Services;

namespace CkgDomainLogic.Charts.Models
{
    public class ChartsSqlDataDescriptor : IChartsSqlDataDescriptor
    {
        public string TemplateName { get; set; }

        public string XmlFileName { get; set; }
        
        public string SqlTableOrViewName { get { return Template.SqlTableOrViewName;  } }

        public string FilterString
        {
            get { return Template.FilterInitial; }
        }

        public string FilterStringSelector
        {
            get { return Template.FilterSelector; }
        }

        public string FilterStringSelectorDetails
        {
            get { return Template.FilterSelectorDetails; }
        }

        public ChartsSqlDataDescriptorTemplate Template { get; set; }


        public ChartsSqlDataDescriptor(string xmlFileName)
        {
            XmlFileName = xmlFileName;
            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            Template = XmlService.XmlDeserializeFromFile<ChartsSqlDataDescriptorTemplate>(XmlFileName);
            TemplateName = Template.HtmlTemplateName.IsNotNullOrEmpty() ?  Template.HtmlTemplateName.Replace(".cshtml","") : Path.GetFileNameWithoutExtension(XmlFileName).NotNullOrEmpty().Substring(3);
        }

        private string GetStaticFilterString()
        {
            if (FilterString.NotNullOrEmpty().Trim().IsNullOrEmpty())
                return "";

            return FilterString;
        }

        private string GetSelectorFilterString(ChartDataSelector selector)
        {
            if (selector == null || FilterStringSelector.NotNullOrEmpty().Trim().IsNullOrEmpty())
                return "";

            return string.Format(FilterStringSelector,
                                 string.Join(",", selector.SelectedKey1Items.Select(s => string.Format("'{0}'", s))),
                                 string.Join(",", selector.SelectedJahrItems.Select(s => string.Format("'{0}'", s)))
                );
        }

        private string GetSelectorFilterDetailsString(ChartDataSelector selector, params string[] detailsFilterValues)
        {
            if (selector == null || FilterStringSelectorDetails.NotNullOrEmpty().Trim().IsNullOrEmpty() || detailsFilterValues == null || detailsFilterValues.None())
                return "";

            return string.Format(FilterStringSelectorDetails, detailsFilterValues.Select(s => string.Format("'{0}'", s)).ToArray());
        }

        private string GetFullFilterString(ChartDataSelector selector, params string[] detailsFilterValues)
        {
            var fullFilterString = "";

            var staticFilterString = GetStaticFilterString();
            if (staticFilterString.IsNotNullOrEmpty())
                fullFilterString = (fullFilterString.IsNullOrEmpty() ? " where " : " and ") + staticFilterString;

            var selectorFilterString = GetSelectorFilterString(selector);
            if (selectorFilterString.IsNotNullOrEmpty())
                fullFilterString += (fullFilterString.IsNullOrEmpty() ? " where " : " and ") + selectorFilterString;

            var selectorFilterDetailsString = GetSelectorFilterDetailsString(selector, detailsFilterValues);
            if (selectorFilterDetailsString.IsNotNullOrEmpty())
                fullFilterString += (fullFilterString.IsNullOrEmpty() ? " where " : " and ") + selectorFilterDetailsString;

            return fullFilterString;
        }

        public string GetGroupChartItemsStatement(ChartDataSelector selector)
        {
            return GetChartItemsStatement(Template.GroupSqlStatement, selector);
        }

        public string GetDetailsChartItemsStatement(ChartDataSelector selector, params string[] detailsFilterValues)
        {
            return GetChartItemsStatement(Template.DetailsSqlStatement, selector, detailsFilterValues);
        }

        public List<string> GetAdditionalChartItemListsStatements(ChartDataSelector selector)
        {
            return Template.AdditionalSqlStatements.ToListOrEmptyList().Select(statement => GetChartItemsStatement(statement, selector)).ToList();
        }

        private string GetChartItemsStatement(string sqlStatement, ChartDataSelector selector, params string[] detailsFilterValues)
        {
            return string.Format(sqlStatement, SqlTableOrViewName, GetFullFilterString(selector, detailsFilterValues));
        }
    }
}
