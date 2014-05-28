using CkgDomainLogic.Charts.Contracts;
using GeneralTools.Contracts;

namespace CkgDomainLogic.Charts.Models
{
    public class ChartsSqlAutovermieterDataDescriptor : ChartsSqlBaseDataDescriptor, IChartsSqlDataDescriptor
    {
        public string TableName { get { return "ZDAD_V_REM_DATEN"; } }

        public string FilterString
        {
            get { return PropertyCacheGet(() => ChartsSqlDataDescriptorTemplate.FilterString); }
            set { PropertyCacheSet(value);}
        }


        public ChartsSqlAutovermieterDataDescriptor(IAppSettings appSettings) :base (appSettings)
        {
            PropertyCacheClear(this, m => m.FilterString);
        }

        public string GetGroupChartItemsStatement()
        {
            return string.Format(ChartsSqlDataDescriptorTemplate.GroupSqlStatement, TableName, FilterString);
        }

        public string GetDetailsChartItemsStatement()
        {
            return string.Format(ChartsSqlDataDescriptorTemplate.DetailsSqlStatement, TableName, FilterString);
        }
    }
}
