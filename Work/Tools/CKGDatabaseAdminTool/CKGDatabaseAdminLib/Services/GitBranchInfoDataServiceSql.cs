using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using CKGDatabaseAdminLib.Contracts;
using CkgDomainLogic.General.Services;
using CKGDatabaseAdminLib.Models.DbModels;

namespace CKGDatabaseAdminLib.Services
{
    public class GitBranchInfoDataServiceSql : CkgGeneralDataService, IGitBranchInfoDataService
    {
        public ObservableCollection<GitBranchInfo> GitBranches { get { return _dataContext.GitBranchInfos.Local; } }

        private DatabaseContext _dataContext;

        public GitBranchInfoDataServiceSql(string connectionName)
        {
            InitDataContext(connectionName);
        }

        public void InitDataContext(string connectionName)
        {
            var sectionData = (NameValueCollection)ConfigurationManager.GetSection("dbConnections");
            _dataContext = new DatabaseContext(sectionData.Get(connectionName));

            _dataContext.GitBranchInfos.Load();
        }

        public string SaveChanges()
        {
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
    }
}