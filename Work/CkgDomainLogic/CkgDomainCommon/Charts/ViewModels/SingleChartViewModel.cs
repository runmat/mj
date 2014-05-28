using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.Charts.Contracts;
using CkgDomainLogic.Charts.Models;
using GeneralTools.Models;
using GeneralTools.Services;
using CkgDomainLogic.General.Services;

namespace CkgDomainLogic.Charts.ViewModels
{
    public class SingleChartViewModel : Store 
    {
        public string ChartID { get; set; }

        public string ChartGroup { get { return ChartsSqlDataDescriptor == null ? "" : ChartsSqlDataDescriptor.Template.Group; } }
        
        [XmlIgnore]
        public IChartsDataService DataService { get { return ParentViewModel.DataService; } }


        //public KgsSelector KbaPlzKgsSelector { get { return PropertyCacheGet(() => new KgsSelector()); } set { PropertyCacheSet(value); } }
        //public KgsStatistic GroupKgsStats { get { return PropertyCacheGet(() => new KgsStatistic()); } set { PropertyCacheSet(value); } }
                                                                                                                                    // ReSharper disable ConvertClosureToMethodGroup
        //public List<KbaPlzKgs> GroupKgsItems { get { return PropertyCacheGet(() => GetGroupKgsItems()); } }


        public List<ChartEntity> GroupChartInitialItems { get { return PropertyCacheGet(() => GetGroupChartInitialItems()); } }
        public List<ChartEntity> GroupChartItems { get { return PropertyCacheGet(() => GetGroupChartItems()); } }

        public List<int> GroupChartJahrItems { get { return GroupChartItems.GroupBy(g => g.Jahr).Select(g => g.Key).ToList(); } }
        public List<int> GroupChartJahrInitialItems { get { return GroupChartInitialItems.GroupBy(g => g.Jahr).Select(g => g.Key).ToList(); } }

        public List<string> GroupChartKey1Items { get { return GroupChartItems.GroupBy(g => g.Key1).Select(g => g.Key).ToList(); } }
        public List<string> GroupChartKey1InitialItems { get { return GroupChartInitialItems.GroupBy(g => g.Key1).Select(g => g.Key).ToList(); } }


        public List<List<ChartEntity>> AdditionalChartItemLists { get { return GetAdditionalChartItemLists(); } }


        public Type DetailsDataCurrentType { get; set; }

        [XmlIgnore]
        public List<dynamic> DetailsData
        {
            get { return PropertyCacheGet(() => new List<dynamic>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<dynamic> DetailsDataFiltered
        {
            get { return PropertyCacheGet(() => DetailsData); }
            private set { PropertyCacheSet(value); }
        }

        public void LoadDetailsData(string group, string subGroup)
        {
            Type dynamicType;
            IEnumerable<dynamic> dynamicObjects;

            var detailsFilterValues = new List<string>().AddIfNotNull(group).AddIfNotNull(subGroup);

            DataService.GetDetailsChartItems(ChartsSqlDataDescriptor, ChartDataSelector, out dynamicType, out dynamicObjects, detailsFilterValues.ToArray());
            if (dynamicType == null || dynamicObjects == null)
                return;

            DetailsData = dynamicObjects.ToList();
            DetailsDataCurrentType = dynamicType;

            PropertyCacheClear(this, m => m.DetailsDataFiltered);
        }


        [XmlIgnore]
        public string CssClass { get; set; }


        public ChartDataSelector ChartDataSelector
        {
            get
            {
                return PropertyCacheGet(() => new ChartDataSelector
                {
                    ChartID = ChartID,
                    ChartGroup = ChartGroup,
                    Key1Items = GroupChartKey1InitialItems.ToArray(),
                    JahrItems = GroupChartJahrInitialItems.ToArray(),
                });
            }
        }

        public ChartsSqlDataDescriptor ChartsSqlDataDescriptor { get; set; }

        public ChartsViewModel ParentViewModel { get; set; }


        public void Init(ChartsViewModel parentViewModel)
        {
            ParentViewModel = parentViewModel;
        }

        public void DataInit()
        {
            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.ChartsSqlDataDescriptor);

            //PropertyCacheClear(this, m => m.GroupKgsItems);
            PropertyCacheClear(this, m => m.GroupChartItems);

            PropertyCacheClear(this, m => m.DetailsData);
            
            ChartsSqlDataDescriptor.DataMarkForRefresh();
        }

        //List<KbaPlzKgs> GetGroupKgsItems()
        //{
        //    var list = DataService.GetGroupKgsItems(KbaPlzKgsSelector).ToList();
        //    GroupKgsStats.SetData(list);

        //    return list;
        //}

        List<ChartEntity> GetGroupChartInitialItems()
        {
            return DataService.GetGroupChartItems(ChartsSqlDataDescriptor, null).ToList();
        }

        List<ChartEntity> GetGroupChartItems()
        {
            return DataService.GetGroupChartItems(ChartsSqlDataDescriptor, ChartDataSelector).ToList();
        }

        List<List<ChartEntity>> GetAdditionalChartItemLists()
        {
            return DataService.GetAdditionalChartItemLists(ChartsSqlDataDescriptor, ChartDataSelector).Select(items => items.ToListOrEmptyList()).ToListOrEmptyList();
        }

        public void Validate(Action<string, string> addModelError)
        {
        }

        public void FilterDetailsData(string filterValue, string filterProperties)
        {
            DetailsDataFiltered = DetailsData.SearchPropertiesWithOrCondition(filterValue, filterProperties, DetailsDataCurrentType);
        }
    }
}
