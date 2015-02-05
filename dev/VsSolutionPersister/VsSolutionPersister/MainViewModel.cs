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
        private string _startPageUrl;
        private SolutionItem _prevSelectedSolutionItem;
        private SolutionItem _selectedSolutionItem;
        private ObservableCollection<SolutionItem> _solutionItems;
        private SolutionPersisterService _solutionService;

        private SolutionPersisterService PersisterService
        {
            get { return (_solutionService ?? (_solutionService = new SolutionPersisterService())); }
        }

        public string SolutionName { get { return PersisterService.SolutionName; } }

        public string GitBranchName { get { return PersisterService.GetCurrentGitBranchName(); } }

        public string StartPageUrl
        {
            get { return _startPageUrl; }
            set
            {
                _startPageUrl = value;
                PersisterService.SaveSolutionStartpageUrl(value);
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
                if (SelectedSolutionItem != null)
                {
                    if (_prevSelectedSolutionItem != null && _prevSelectedSolutionItem != SelectedSolutionItem)
                    {
                        PersisterService.SaveSolutionItemFiles(_prevSelectedSolutionItem);
                        PersisterService.LoadSolutionItemFiles(SelectedSolutionItem);
                    }

                    if (StartPageUrl.NotNullOrEmpty() != SelectedSolutionItem.RemoteSolutionStartPage.NotNullOrEmpty())
                        StartPageUrl = SelectedSolutionItem.RemoteSolutionStartPage;
                }
                _prevSelectedSolutionItem = SelectedSolutionItem;
            }
        }

        public ICommand SolutionItemSaveCommand { get; private set; }
        public ICommand SolutionItemDeleteCommand { get; private set; }


        public MainViewModel()
        {
            SolutionItemSaveCommand = new DelegateCommand(e => SolutionItemSave(), e => true);
            SolutionItemDeleteCommand = new DelegateCommand(e => SolutionItemDelete((string) e), e => true);

            SolutionItems = new ObservableCollection<SolutionItem>(PersisterService.LoadSolutionItems());

            StartPageUrl = PersisterService.GetSolutionStartpageUrl();
            SelectCurrentSolutionItem();
        }
        
        void SelectCurrentSolutionItem()
        {
            SelectedSolutionItem = SolutionItems.FirstOrDefault(item => item.Name == CreateCurrentSolutionItem().Name);
        }


        SolutionItem CreateCurrentSolutionItem()
        {
            return new SolutionItem
            {
                GitBranchName = PersisterService.GetCurrentGitBranchName(),
                RemoteSolutionStartPage = PersisterService.GetSolutionStartpageUrl(),
            };
        }

        void SolutionItemSave()
        {
            PersisterService.SaveSolutionStartpageUrl(StartPageUrl);

            var newItem = CreateCurrentSolutionItem();

            if (SolutionItems.None(item => item.Name == newItem.Name))
                SolutionItems.Add(newItem);

            SelectCurrentSolutionItem();

            PersisterService.SaveSolutionItemFiles(newItem);
            SaveSolutionXmlItems();
        }

        void SolutionItemDelete(string solutionName)
        {
            if (!Tools.Confirm(string.Format("Delete item '{0}'?", solutionName.Replace("___"," / "))))
                return;

            var itemToDelete = SolutionItems.First(i => i.Name == solutionName);
            PersisterService.DeleteSolutionItemFiles(itemToDelete);
            SolutionItems.Remove(itemToDelete);

            SaveSolutionXmlItems();
        }

        void SaveSolutionXmlItems()
        {
            PersisterService.SaveSolutionXmlItems(SolutionItems.ToList());
        }
    }
}
