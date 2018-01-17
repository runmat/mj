// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using AhwToolbar.Models;
using AhwToolbar.UserControls;
using Dragablz;
using GalaSoft.MvvmLight;
using WpfTools4.Commands;

namespace AhwToolbar.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ViewModelData _viewModeldata = new ViewModelData();

        public ObservableCollection<HeaderedItemViewModel> Tabs { get; }

        public IInterTabClient InterTabClient { get; } = new MainViewModelInterTabClient();

        public string Name => "Walter Zabel";

        public ICommand TestCommand { get; private set; }


        public MainViewModel()
        {
            Tabs = new ObservableCollection<HeaderedItemViewModel>();
            _viewModeldata.Tabs.ForEach(tab => Tabs.Add(new HeaderedItemViewModel(tab.Header, Activator.CreateInstance(Type.GetType(tab.UserControlType) ?? typeof(object)), tab.IsSelected)));

            TestCommand = new DelegateCommand(e => { });
        }

        public void OnTabsChanged(IEnumerable<string> tabHeaders)
        {
            
        }
    }
}