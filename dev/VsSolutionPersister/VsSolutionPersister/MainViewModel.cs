using System.ComponentModel;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WpfTools4.Commands;
using WpfTools4.Services;
using WpfTools4.ViewModels;
using GeneralTools.Models;
using System;
using System.Linq;

namespace VsSolutionPersister
{
    public class MainViewModel : ViewModelBase
    {
        private string _startPageUrl;
        private string _lastCommitMessage;
        private string _newCommitMessage;
        private bool _branchIsDirty;
        private readonly bool _ctorCreated;
        private SolutionItem _prevSelectedSolutionItem;
        private SolutionItem _selectedSolutionItem;
        private ObservableCollection<SolutionItem> _solutionItems;
        private SolutionPersisterService _solutionService;

        public bool IsClosable { get; private set; }

        private SolutionPersisterService PersisterService
        {
            get { return (_solutionService ?? (_solutionService = new SolutionPersisterService())); }
        }

        public string SolutionName { get { return PersisterService.SolutionName; } }

        public string GitBranchName { get { return PersisterService.GetCurrentGitBranchName(); } }

        public string GitRootFolder { get { return PersisterService.GetCurrentRootGitFolder(); } }

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

        public string LastCommitMessage
        {
            get { return _lastCommitMessage; }
            set
            {
                _lastCommitMessage = value;
                SendPropertyChanged("LastCommitMessage");
            }
        }

        public bool BranchIsDirty
        {
            get { return _branchIsDirty; }
            set
            {
                _branchIsDirty = value;
                SendPropertyChanged("BranchIsDirty");
            }
        }

        public string NewCommitMessage
        {
            get { return _newCommitMessage; }
            set
            {
                _newCommitMessage = value;
                SendPropertyChanged("NewCommitMessage");
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
                SendPropertyChanged("GitBranchName");

                if (SelectedSolutionItem == null)
                    return;

                using (new WaitCursor())
                {
                    GitService.CheckoutBranch(GitRootFolder, SelectedSolutionItem.GitBranchName);
                    LastCommitMessage = GitService.GetLastCommitMessage(GitRootFolder);
                    SendPropertyChanged("GitBranchName");
                }

                SolutionItems.ToList().ForEach(item => item.IsSelected = false);
                SelectedSolutionItem.IsSelected = true;
                SaveSolutionXmlItems();

                if (_prevSelectedSolutionItem != SelectedSolutionItem)
                {
                    if (_prevSelectedSolutionItem != null && SolutionItems.Any(item => item.Name == _prevSelectedSolutionItem.Name))
                        PersisterService.SaveSolutionItemFiles(_prevSelectedSolutionItem);

                    if (_ctorCreated)
                        PersisterService.LoadSolutionItemFiles(SelectedSolutionItem);

                    _prevSelectedSolutionItem = SelectedSolutionItem;
                }

                if (StartPageUrl.NotNullOrEmpty() != SelectedSolutionItem.RemoteSolutionStartPage.NotNullOrEmpty())
                    StartPageUrl = SelectedSolutionItem.RemoteSolutionStartPage;
            }
        }

        public ICommand SolutionItemSaveCommand { get; private set; }
        public ICommand SolutionItemDeleteCommand { get; private set; }
        public ICommand GitCommitAmendCommand { get; private set; }


        public MainViewModel()
        {
            IsClosable = true;

            if (Tools.IsWindowOpenForProcessNamePartAndTitlePart("devenv", SolutionName, 
                        () => MessageBox.Show( string.Format("Bitte schließen Sie am Visual Studio die Solution '{0}'", SolutionName), 
                                                Assembly.GetExecutingAssembly().FullName.Split(',')[0] + " - Info", MessageBoxButton.OK, MessageBoxImage.Hand)))
            {
                Application.Current.Shutdown();
                return;
            }

            SolutionItemSaveCommand = new DelegateCommand(e => SolutionItemSave(), e => true);
            SolutionItemDeleteCommand = new DelegateCommand(e => SolutionItemDelete((string) e), e => true);
            GitCommitAmendCommand = new DelegateCommand(e => GitAmendCommitMessage(), e => LastCommitMessage.IsNotNullOrEmpty());

            SolutionItems = new ObservableCollection<SolutionItem>(PersisterService.LoadSolutionItems());
            var defaultView = CollectionViewSource.GetDefaultView(SolutionItems);
            defaultView.SortDescriptions.Add(new SortDescription("Datum", ListSortDirection.Descending ));

            StartPageUrl = PersisterService.GetSolutionStartpageUrl();

            var formerSelectedSolutionItem = SolutionItems.FirstOrDefault(item => item.IsSelected);
            if (formerSelectedSolutionItem != null)
                PersisterService.SaveSolutionItemFiles(formerSelectedSolutionItem);

            SelectCurrentSolutionItem();
            ShowNewCommitMessageIfIsDirty();
            _ctorCreated = true;
        }

        public bool AllowSelectionChange()
        {
            using (new WaitCursor())
            {
                if (GitService.GetLastCommitMessage(GitRootFolder).NotNullOrEmpty().Trim() == GitService.DefaultCommitMessage.NotNullOrEmpty().Trim())
                {
                    Alert("Please change the default commit message first!");
                    return false;
                }

                if (BranchIsDirty && !GitService.StageAndCommitWorkingDirectory(GitRootFolder, NewCommitMessage))
                    return false;
            }

            BranchIsDirty = false;
            NewCommitMessage = "";

            return true;
        }
        
        void SelectCurrentSolutionItem()
        {
            SelectedSolutionItem = SolutionItems.FirstOrDefault(item => item.Name == CreateCurrentSolutionItem().Name);
        }

        void ShowNewCommitMessageIfIsDirty()
        {
            Task.Factory.StartNew(() => Thread.Sleep(100))
                .ContinueWith(t =>
                {
                    using (new WaitCursor())
                    {
                        if (!GitService.IsDirty(GitRootFolder))
                            return;

                        BranchIsDirty = true;
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        SolutionItem CreateCurrentSolutionItem()
        {
            return new SolutionItem
            {
                GitBranchName = PersisterService.GetCurrentGitBranchName(),
                RemoteSolutionStartPage = PersisterService.GetSolutionStartpageUrl(),
                Datum = DateTime.Now,
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

        void GitAmendCommitMessage()
        {
            using (new WaitCursor())
            {
                GitService.AmendLastCommit(GitRootFolder, LastCommitMessage);
            }
        }

        void SolutionItemDelete(string solutionName)
        {
            if (!Confirm(string.Format("Delete item '{0}'?", solutionName.Replace("___"," / "))))
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

        void Alert(string message)
        {
            IsClosable = false;
            MessageBox.Show(message, "Alert", MessageBoxButton.OK, MessageBoxImage.Stop);
            IsClosable = true;
        }

        bool Confirm(string question)
        {
            IsClosable = false;
            var retVal = Tools.Confirm(question);
            IsClosable = true;

            return retVal;
        }
    }
}
