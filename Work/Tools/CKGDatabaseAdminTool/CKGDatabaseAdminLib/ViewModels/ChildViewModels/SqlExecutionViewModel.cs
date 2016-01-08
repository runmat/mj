using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Xml.Serialization;
using CKGDatabaseAdminLib.Contracts;
using CKGDatabaseAdminLib.Models;
using CKGDatabaseAdminLib.Services;
using GeneralTools.Models;
using SapORM.Services;
using WpfTools4.Commands;
using WpfTools4.ViewModels;

namespace CKGDatabaseAdminLib.ViewModels
{
    public class SqlExecutionViewModel : ViewModelBase
    {
        #region Properties

        [XmlIgnore]
        private readonly ISqlExecutionDataService DataService;

        public MainViewModel Parent { get; set; }

        private string _sqlString;
        public string SqlString
        {
            get { return _sqlString; }
            set { _sqlString = value; SendPropertyChanged("SqlString"); }
        }

        private List<SelectableListItem> _destinationDatabases;
        public List<SelectableListItem> DestinationDatabases
        {
            get { return _destinationDatabases; }
            set { _destinationDatabases = value; SendPropertyChanged("DestinationDatabases"); }
        }

        public ICommand CommandSqlExecution { get; private set; }
        public ICommand CommandExecuteSqlString { get; private set; }

        #endregion

        public SqlExecutionViewModel(MainViewModel parentVM)
        {
            Parent = parentVM;

            DataService = new SqlExecutionDataServiceSql();

            CommandSqlExecution = new DelegateCommand(Init);
            CommandExecuteSqlString = new DelegateCommand(ExecuteSqlString);

            _destinationDatabases = new List<SelectableListItem>();
            foreach (var item in Parent.DbConnections)
                _destinationDatabases.Add(new SelectableListItem { Key = item });
        }

        #region Commands

        public void Init(object parameter)
        {
            Parent.ActiveViewModel = this;
        }

        public void ExecuteSqlString(object parameter)
        {
            if (string.IsNullOrEmpty(SqlString))
            {
                Parent.ShowMessage("Es wurden keine SQL-Befehle angegeben", MessageType.Error);
                return;
            }

            if (DestinationDatabases.None(d => d.IsChecked))
            {
                Parent.ShowMessage("Es wurde keine Datenbank ausgewählt", MessageType.Error);
                return;
            }

            foreach (var item in DestinationDatabases.Where(d => d.IsChecked))
            {
                try
                {
                    DataService.InitDestinationDataContext(item.Key);
                    DataService.ExecuteSqlNonQuery(SqlString);
                }
                catch (Exception ex)
                {
                    Parent.ShowMessage("Abbruch. Fehler beim Ausführen der SQL-Befehle auf Datenbank " + item.Key + ": " + ex.Message, MessageType.Error);
                    return;
                }
            }

            Parent.ShowMessage("Die SQL-Befehle wurden erfolgreich ausgeführt", MessageType.Success);
        }

        #endregion
    }
}
