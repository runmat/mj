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
        private SolutionPersisterService _solutionService;

        private SolutionPersisterService PersisterService
        {
            get { return (_solutionService ?? (_solutionService = new SolutionPersisterService())); }
        }

        public string SolutionName { get { return PersisterService.SolutionName; } }

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
                new SolutionItem {GitBranchName = "ita7764", RemoteSolutionStartPage = "autohaus/fahrzeugbestand/index"},
                new SolutionItem {GitBranchName = "ita7773", RemoteSolutionStartPage = "Insurance/SchadenstatusAlle"},
                new SolutionItem {GitBranchName = "zDashboardPreview", RemoteSolutionStartPage = "Common/Dashboard/Index"},
            };
        }

        void SolutionItemAdd()
        {
            var newItem = new SolutionItem
            {
                GitBranchName = PersisterService.GetCurrentGitBranchName(),
                RemoteSolutionStartPage = PersisterService.GetCurrentSolutionStartpageUrl(),
            };

            if (SolutionItems.None(item => item.Name == newItem.Name))
                SolutionItems.Add(newItem);
        }

        void SolutionItemDelete(string solutionName)
        {
            if (!Tools.Confirm(string.Format("Delete item '{0}'?", solutionName.Replace("___","   "))))
                return;

            SolutionItems.Remove(SolutionItems.First(i => i.Name == solutionName));
        }
    }
}
