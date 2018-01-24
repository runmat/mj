// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Windows.Input;
using AhwToolbar.Models;
using Dragablz;
using GalaSoft.MvvmLight;
using WpfTools4.Commands;

namespace AhwToolbar.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ViewModelData _viewModeldata = new ViewModelData();

        [ExcludeFromCodeCoverage]
        public ObservableCollection<HeaderedItemViewModel> Tabs { get; private set; }

        [ExcludeFromCodeCoverage]
        public IInterTabClient InterTabClient { get; } = new MainViewModelInterTabClient();

        [ExcludeFromCodeCoverage]
        public string Name => "Walter Zabel";

        public ICommand TestCommand { get; }


        public MainViewModel()
        {
            _viewModeldata = _viewModeldata.LoadViewModelData();

            CreateTabs();

            TestCommand = new DelegateCommand(e => { });
        }

        [ExcludeFromCodeCoverage]
        private void CreateTabs()
        {
            Tabs = new ObservableCollection<HeaderedItemViewModel>();
            
            if (SynchronizationContext.Current == null)
                return;

            _viewModeldata.Tabs.ForEach(tab => Tabs.Add(new HeaderedItemViewModel(tab.Header, Activator.CreateInstance(Type.GetType(tab.UserControlType) ?? typeof(object)), tab.IsSelected)));
        }

        [ExcludeFromCodeCoverage]
        public void OnTabsChanged(IEnumerable<string> tabHeaders, string selectedTabHeader)
        {
            _viewModeldata.SetTabs(tabHeaders, selectedTabHeader);

            _viewModeldata.PersistViewModelData();
        }
    }
}