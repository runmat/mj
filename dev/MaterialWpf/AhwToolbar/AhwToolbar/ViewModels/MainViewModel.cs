using System;
using System.Collections.ObjectModel;
using System.Windows.Navigation;
using AhwToolbar.UserControls;
using Dragablz;
using GalaSoft.MvvmLight;

namespace AhwToolbar.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<HeaderedItemViewModel> Tabs { get; }

        public IInterTabClient InterTabClient { get; } = new MainViewModelInterTabClient();

        public string Name => "Walter Zabel";


        public MainViewModel()
        {
            Tabs = new ObservableCollection<HeaderedItemViewModel>
            {
                new HeaderedItemViewModel("Walter", new UcContent1()),
                new HeaderedItemViewModel("Zabel", new UcContent2()),
            };
        }
    }
}