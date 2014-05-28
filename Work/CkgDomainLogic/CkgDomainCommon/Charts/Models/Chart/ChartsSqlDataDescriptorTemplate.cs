using System.Collections.Generic;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Charts.Models
{
    public class ChartsSqlDataDescriptorTemplate
    {
        private string _group;
        private string _htmlTemplateName;

        public string Group
        {
            get { return _group; }
            set { _group = value.NotNullOrEmpty().Trim(); }
        }

        public string HtmlTemplateName
        {
            get { return _htmlTemplateName; }
            set { _htmlTemplateName = value.NotNullOrEmpty().Trim(); }
        }

        public string SqlTableOrViewName { get; set; }

        public string GroupSqlStatement { get; set; }

        public string DetailsSqlStatement { get; set; }

        public string FilterInitial { get; set; }

        public string FilterSelector { get; set; }
        
        public string FilterSelectorDetails { get; set; }

        public List<string> AdditionalSqlStatements { get; set; }

        public dynamic HtmlParameterXml { get; set; }

        public dynamic HtmlParameter { get { return DynamicXml.GetDynamicObject(HtmlParameterXml); } }
    }
}
