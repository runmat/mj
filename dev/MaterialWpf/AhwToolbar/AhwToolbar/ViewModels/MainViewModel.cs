// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using AhwToolbar.UserControls;
using Dragablz;
using GalaSoft.MvvmLight;
using WpfTools4.Commands;

namespace AhwToolbar.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<HeaderedItemViewModel> Tabs { get; }

        public IInterTabClient InterTabClient { get; } = new MainViewModelInterTabClient();

        public string Name => "Walter Zabel";

        public ICommand TestCommand { get; private set; }




        public MainViewModel()
        {
            Tabs = new ObservableCollection<HeaderedItemViewModel>
            {
                new HeaderedItemViewModel("Walter", new UcContent1()),
                new HeaderedItemViewModel("Zabel", new UcContent2()),
            };

            TestCommand = new DelegateCommand(e =>
            {
            });
        }

        public void OnTabsChanged(IEnumerable<string> tabHeaders)
        {
            
        }
    }
}