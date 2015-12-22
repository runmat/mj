using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using CKGDatabaseAdminLib.Contracts;
using CkgDomainLogic.General.Services;
using CKGDatabaseAdminLib.Models;

namespace CKGDatabaseAdminLib.Services
{
    public class GitBranchInfoDataServiceSql : CkgGeneralDataService, IGitBranchInfoDataService
    {
        public GitBranchViewFilter AnzeigeFilter { get; set; }

        private ObservableCollection<GitBranchInfo> GitBranchesAll { get { return _dataContext.GitBranchInfos.Local; } }

        public ObservableCollection<GitBranchInfo> GitBranches { get; private set; }

        public ObservableCollection<CkgEntwickler> CkgEntwickler { get { return _dataContext.CkgEntwickler.Local; } }

        private DatabaseContext _dataContext;

        public GitBranchInfoDataServiceSql(string connectionName)
        {
            AnzeigeFilter = GitBranchViewFilter.aktive;
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
            
            // ReSharper disable once EmptyGeneralCatchClause
            try { _dataContext.CkgEntwickler.Load(); } catch { }

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
                case GitBranchViewFilter.aktive:
                    listeTemp = GitBranchesAll.Where(g => !g.Erledigt);
                    break;

                case GitBranchViewFilter.aktiveMitFreigabe:
                    listeTemp = GitBranchesAll.Where(g => !g.Erledigt && !String.IsNullOrEmpty(g.FreigegebenDurch));
                    break;

                case GitBranchViewFilter.abgeschlossene:
                    listeTemp = GitBranchesAll.Where(g => g.Erledigt);
                    break;

                default: //alle
                    listeTemp = GitBranchesAll;
                    break;
            }

            if (GitBranches != null)
                GitBranches.CollectionChanged -= GitBranchesFilteredOnCollectionChanged;
   
            GitBranches = new ObservableCollection<GitBranchInfo>(listeTemp.OrderBy(g => g.ID));

            GitBranches.CollectionChanged += GitBranchesFilteredOnCollectionChanged;
        }

        public List<GitBranchInfo> GetBranchesForTransportMail()
        {
            return GitBranchesAll.Where(g => !g.Erledigt && !String.IsNullOrEmpty(g.FreigegebenDurch)).OrderBy(g => g.Name).ToList();
        } 

        private void GitBranchesFilteredOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var newItem in e.NewItems)
                    {
                        // temporäre Id vergeben, um neue Entities auch vor dem Speichern in SQL anhand der ID unterscheiden zu können
                        if (GitBranchesAll.Count > 0)
                        {
                            var minId = GitBranchesAll.Min(g => g.ID);
                            (newItem as GitBranchInfo).ID = (minId < 0 ? minId - 1 : -1);
                        }
                        else
                        {
                            (newItem as GitBranchInfo).ID = -1;
                        }
                        GitBranchesAll.Add(newItem as GitBranchInfo);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var oldItem in e.OldItems)
                    {
                        GitBranchesAll.Remove(oldItem as GitBranchInfo);
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
            if (GitBranches != null)
            {
                foreach (var gitBranch in GitBranches)
                {
                    var item = GitBranchesAll.FirstOrDefault(g => g.ID == gitBranch.ID);

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
                        item.Deaktiviert = gitBranch.Deaktiviert;
                    }
                }
            }
        }
    }
}