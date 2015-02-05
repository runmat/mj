// ReSharper disable RedundantUsingDirective
using System.Collections.ObjectModel;
using System.Windows.Input;
using WpfTools4.Commands;
using WpfTools4.Services;
using WpfTools4.ViewModels;
using GeneralTools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VsSolutionPersister
{
    public class MainViewModel : ViewModelBase 
    {
        private SolutionItem _selectedSolutionItem;
        private ObservableCollection<SolutionItem> _solutionItems;
        private SolutionPersisterService _solutionPersisterService;

        private SolutionPersisterService SolutionPersisterService
        {
            get { return (_solutionPersisterService ?? (_solutionPersisterService = new SolutionPersisterService())); }
        }

        public string SolutionPath { get { return SolutionPersisterService.SolutionPath; } }

        public string SolutionName { get { return SolutionPersisterService.SolutionName; } }

        public ObservableCollection<SolutionItem> SolutionItems
        {
            get { return _solutionItems; }
            set
            {
                _solutionItems = value;
                SendPropertyChanged("SolutionItems");
            }
        }

        public SolutionItem SelectedSolutionItem
        {
            get { return _selectedSolutionItem; }
            set
            {
                _selectedSolutionItem = value;
                SendPropertyChanged("SelectedSolutionItem");
            }
        }

        public ICommand SolutionItemAddCommand { get; private set; }
        public ICommand SolutionItemDeleteCommand { get; private set; }


        public MainViewModel()
        {
            SolutionItemAddCommand = new DelegateCommand(e => SolutionItemAdd(), e => true);
            SolutionItemDeleteCommand = new DelegateCommand(e => SolutionItemDelete((string)e), e => true);

            SolutionItems = new ObservableCollection<SolutionItem>
            {
                new SolutionItem {Name = "AH-2015 Zulassung", GitBranchName = "ita7764", RemoteSolutionStartPage = "autohaus/fahrzeugbestand/index"},
                new SolutionItem {Name = "CSI Schadenfälle", GitBranchName = "ita7773", RemoteSolutionStartPage = "Insurance/SchadenstatusAlle"},
                new SolutionItem {Name = "Dashboard", GitBranchName = "zDashboardPreview", RemoteSolutionStartPage = "Common/Dashboard/Index"},
            };
        }

        void SolutionItemAdd()
        {
            var newSolutionName = Tools.Input("Please provide a name for the new item:");
            if (newSolutionName.IsNullOrEmpty())
                return;
            
            SolutionItems.Add(new SolutionItem
            {
                Name = newSolutionName,
                GitBranchName = "",
                RemoteSolutionStartPage = "",
            });
        }

        void SolutionItemDelete(string solutionName)
        {
            if (!Tools.Confirm(string.Format("Delete solution '{0}'?", solutionName)))
                return;

            SolutionItems.Remove(SolutionItems.First(i => i.Name == solutionName));
        }
    }
}
