using System.Collections.ObjectModel;
using WpfTools4.ViewModels;
// ReSharper disable RedundantUsingDirective
using GeneralTools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsSolutionPersister
{
    public class MainViewModel : ViewModelBase 
    {
        private SolutionItem _selectedSolutionItem;
        private ObservableCollection<SolutionItem> _solutionItems;

        public string SolutionPath { get { return System.Configuration.ConfigurationManager.AppSettings["SolutionPathToPersist"]; } }

        public string SolutionName { get { return SolutionPath.NotNullOrEmpty().Split('\\').Last(); } }

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

        public MainViewModel()
        {
            SolutionItems = new ObservableCollection<SolutionItem>
            {
                new SolutionItem {Name = "AH-2015 Zulassung", GitBranchName = "ita7764", RemoteSolutionStartPage = "autohaus/fahrzeugbestand/index"},
                new SolutionItem {Name = "CSI Schadenfälle", GitBranchName = "ita7773", RemoteSolutionStartPage = "Insurance/SchadenstatusAlle"},
                new SolutionItem {Name = "Dashboard", GitBranchName = "zDashboardPreview", RemoteSolutionStartPage = "Common/Dashboard/Index"},
            };
        }
    }
}
