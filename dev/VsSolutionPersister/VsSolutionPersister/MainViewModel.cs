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

        public string StartPageUrl
        {
            get { return PersisterService.GetSolutionStartpageUrl(); }
            set
            {
                PersisterService.SetSolutionStartpageUrl(value);
                SaveSolutionItems();
                SendPropertyChanged("StartPageUrl");
            }
        }

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

            SolutionItems = new ObservableCollection<SolutionItem>(PersisterService.LoadSolutionItems());

            SelectedSolutionItem = SolutionItems.FirstOrDefault(item => item.Name == CreateCurrentSolutionItem().Name);
        }

        void SaveSolutionItems()
        {
            PersisterService.SaveSolutionItems(SolutionItems.ToList());
        }

        void SolutionItemAdd()
        {
            var newItem = CreateCurrentSolutionItem();

            if (SolutionItems.None(item => item.Name == newItem.Name))
                SolutionItems.Add(newItem);

            SaveSolutionItems();
        }

        SolutionItem CreateCurrentSolutionItem()
        {
            return new SolutionItem
            {
                GitBranchName = PersisterService.GetCurrentGitBranchName(),
                RemoteSolutionStartPage = PersisterService.GetSolutionStartpageUrl(),
            };
        }

        void SolutionItemDelete(string solutionName)
        {
            if (!Tools.Confirm(string.Format("Delete item '{0}'?", solutionName.Replace("___","   "))))
                return;

            SolutionItems.Remove(SolutionItems.First(i => i.Name == solutionName));

            SaveSolutionItems();
        }
    }
}
