using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using CKGDatabaseAdminLib.Contracts;
using CkgDomainLogic.General.Services;
using CKGDatabaseAdminLib.Models.DbModels;

namespace CKGDatabaseAdminLib.Services
{
    public class GitBranchInfoDataServiceSql : CkgGeneralDataService, IGitBranchInfoDataService
    {
        public GitBranchViewFilter AnzeigeFilter { get; set; }

        private ObservableCollection<GitBranchInfo> GitBranches { get { return _dataContext.GitBranchInfos.Local; } }

        public ObservableCollection<GitBranchInfo> GitBranchesFiltered { get; set; }

        private DatabaseContext _dataContext;

        public GitBranchInfoDataServiceSql(string connectionName)
        {
            AnzeigeFilter = GitBranchViewFilter.alle;
            InitDataContext(connectionName);
        }

        public void InitDataContext(string connectionName)
        {
            LoadData(connectionName);
        }

        public void ReloadData(string connectionName)
        {
            LoadData(connectionName);
        }

        private void LoadData(string connectionName)
        {
            var sectionData = (NameValueCollection)ConfigurationManager.GetSection("dbConnections");
            _dataContext = new DatabaseContext(sectionData.Get(connectionName));
            _dataContext.GitBranchInfos.Load();
            FilterData();
        }

        public void FilterGitBranches()
        {
            ApplyChanges();
            FilterData();
        }

        public void FilterData()
        {
            IEnumerable<GitBranchInfo> listeTemp;

            switch (AnzeigeFilter)
            {
                case GitBranchViewFilter.imMasterUndNichtProduktiv:
                    listeTemp = GitBranches.Where(g => g.ImMaster && !g.ProduktivSeit.HasValue).OrderBy(g => g.ID);
                    break;
                case GitBranchViewFilter.imMasterUndProduktiv:
                    listeTemp = GitBranches.Where(g => g.ImMaster && g.ProduktivSeit.HasValue).OrderBy(g => g.ID);
                    break;
                case GitBranchViewFilter.nichtImMaster:
                    listeTemp = GitBranches.Where(g => !g.ImMaster).OrderBy(g => g.ID);
                    break;
                case GitBranchViewFilter.nichtImMasterUndNichtProduktiv:
                    listeTemp = GitBranches.Where(g => !g.ImMaster && !g.ProduktivSeit.HasValue).OrderBy(g => g.ID);
                    break;
                case GitBranchViewFilter.nichtProduktiv:
                    listeTemp = GitBranches.Where(g => !g.ProduktivSeit.HasValue).OrderBy(g => g.ID);
                    break;
                case GitBranchViewFilter.produktivUndNichtImMaster:
                    listeTemp = GitBranches.Where(g => !g.ImMaster && g.ProduktivSeit.HasValue).OrderBy(g => g.ID);
                    break;
                default:
                    // alle
                    listeTemp = GitBranches.OrderBy(g => g.ID);
                    break;
            }

            if (GitBranchesFiltered != null)
            {
                GitBranchesFiltered.CollectionChanged -= GitBranchesFilteredOnCollectionChanged;
            }      
            GitBranchesFiltered = new ObservableCollection<GitBranchInfo>(listeTemp);
            GitBranchesFiltered.CollectionChanged += GitBranchesFilteredOnCollectionChanged;
        }

        private void GitBranchesFilteredOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var newItem in e.NewItems)
                    {
                        // temporäre Id vergeben, um neue Entities auch vor dem Speichern in SQL anhand der ID unterscheiden zu können
                        var minId = GitBranches.Min(g => g.ID);
                        (newItem as GitBranchInfo).ID = (minId < 0 ? minId - 1 : -1);
                        GitBranches.Add(newItem as GitBranchInfo);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var oldItem in e.OldItems)
                    {
                        GitBranches.Remove(oldItem as GitBranchInfo);
                    }
                    break;
            }
        }

        public string SaveChanges()
        {
            ApplyChanges();

            var validierungsFehler = _dataContext.GetValidationErrors();

            if (validierungsFehler.Any())
            {
                var erg = "";
                foreach (var fehler in validierungsFehler)
                {
                    erg += fehler.ValidationErrors.First().ErrorMessage + ", ";
                }
                return erg.TrimEnd(' ', ',');
            }

            _dataContext.SaveChanges();
            return "";
        }

        private void ApplyChanges()
        {
            if (GitBranchesFiltered != null)
            {
                foreach (var gitBranch in GitBranchesFiltered)
                {
                    var item = GitBranches.FirstOrDefault(g => g.ID == gitBranch.ID);

                    if (item != null)
                    {
                        item.Anwendung = gitBranch.Anwendung;
                        item.Bemerkung = gitBranch.Bemerkung;
                        item.Entwickler = gitBranch.Entwickler;
                        item.FreigegebenDurch = gitBranch.FreigegebenDurch;
                        item.ImMaster = gitBranch.ImMaster;
                        item.ImTestSeit = gitBranch.ImTestSeit;
                        item.Name = gitBranch.Name;
                        item.PM = gitBranch.PM;
                        item.ProduktivSeit = gitBranch.ProduktivSeit;
                    }
                }
            }
        }
    }
}