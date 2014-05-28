using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.Charts.Contracts;
using CkgDomainLogic.Charts.Models;
using CkgDomainLogic.General.ViewModels;
using GeneralTools.Models;

namespace CkgDomainLogic.Charts.ViewModels
{
    public class ChartsViewModel : CkgBaseViewModel 
    {
        [XmlIgnore]
        public IChartsDataService DataService { get { return CacheGet<IChartsDataService>(); } }

        public List<SingleChartViewModel> ChartViewModels
        {
            get
            {
                return PropertyCacheGet(() =>
                    {
                        var viewModels = Directory.GetFiles(Path.Combine(AppSettings.DataPath, "Charts"), "*.xml")
                                 .Select(xmlFileName => new SingleChartViewModel
                                     {
                                         ChartID = Guid.NewGuid().ToString(),
                                         ParentViewModel = this,
                                         ChartsSqlDataDescriptor = new ChartsSqlDataDescriptor(xmlFileName),
                                         CssClass =
                                             (xmlFileName.ToLower().EndsWith("doubleheight.xml") ? "chart-map" : ""),
                                     });
                        
                        var filterChartGroup = ConfigurationManager.AppSettings["ChartsGroup"];
                        if (filterChartGroup.IsNotNullOrEmpty())
                            viewModels = viewModels.Where(vm => vm.ChartGroup == filterChartGroup);

                        return viewModels.ToList();
                    }
                    );
            }
        }

        public List<string> ChartViewModelGroups { get { return ChartViewModels.GroupBy(vm => vm.ChartGroup).Select(g => g.Key).ToList(); } }

        public List<SingleChartViewModel> GetChartViewModelsForGroup(string group)
        {
            return ChartViewModels.Where(vm => vm.ChartGroup == group).ToListOrEmptyList();
        }


        public void DataInit()
        {
            DataMarkForRefresh();
            ChartViewModels.ForEach(vm => vm.DataInit());
        }

        public void DataMarkForRefresh()
        {
            //PropertyCacheClear(this, m => m.ChartViewModels);
            ChartViewModels.ForEach(vm => vm.DataMarkForRefresh());
        }

        public SingleChartViewModel GetChartViewModel(string chartID)
        {
            return ChartViewModels.First(vm => vm.ChartID == chartID);
        }

        public SingleChartViewModel CurrentChartViewModel { get; set; }

        public void SetCurrentChartViewModel(string chartID)
        {
            CurrentChartViewModel = ChartViewModels.FirstOrDefault(vm => vm.ChartID == chartID);
        }

        public void Validate(Action<string, string> addModelError)
        {
        }
    }
}
