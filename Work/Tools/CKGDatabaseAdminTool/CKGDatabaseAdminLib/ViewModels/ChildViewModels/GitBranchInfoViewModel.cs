using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Windows.Input;
using System.Xml.Serialization;
using CKGDatabaseAdminLib.Contracts;
using CKGDatabaseAdminLib.Models;
using CKGDatabaseAdminLib.Services;
using DocumentTools.Services;
using GeneralTools.Models;
using WpfTools4.Commands;
using WpfTools4.ViewModels;

namespace CKGDatabaseAdminLib.ViewModels
{
    public class GitBranchInfoViewModel : ViewModelBase
    {
        #region Properties

        public ObservableCollection<GitBranchInfo> GitBranches { get { return DataService.GitBranches; } }

        [XmlIgnore]
        private readonly IGitBranchInfoDataService DataService;

        public MainViewModel Parent { get; set; }

        public GitBranchViewFilter AnzeigeFilter
        {
            get { return DataService.AnzeigeFilter; }
            set { DataService.AnzeigeFilter = value; SendPropertyChanged("AnzeigeFilter"); FilterGitBranches(); }
        }

        public ICommand CommandManageGitBranches { get; private set; }
        public ICommand CommandSaveGitBranchInfos { get; private set; }
        public ICommand CommandCancelGitBranchInfos { get; private set; }

        #endregion

        public GitBranchInfoViewModel(MainViewModel parentVM)
        {
            Parent = parentVM;

            DataService = new GitBranchInfoDataServiceSql(Parent.ActualDatabase);
            
            CommandManageGitBranches = new DelegateCommand(ManageGitBranches);
            CommandSaveGitBranchInfos = new DelegateCommand(SaveGitBranchInfos);
            CommandCancelGitBranchInfos = new DelegateCommand(CancelGitBranchInfos);
        }

        #region Commands

        public void ManageGitBranches(object parameter)
        {
            Parent.ActiveViewModel = this;
        }

        public void SaveGitBranchInfos(object parameter)
        {
            var ergebnis = DataService.SaveChanges();
            if (String.IsNullOrEmpty(ergebnis))
            {
                Parent.ShowMessage("Änderungen wurden gespeichert", MessageType.Success);
            }
            else
            {
                Parent.ShowMessage("Speichern nicht möglich: " + ergebnis, MessageType.Error);
            }
        }

        public void CancelGitBranchInfos(object parameter)
        {
            DataService.ReloadData(Parent.ActualDatabase);
            SendPropertyChanged("GitBranches");
            Parent.ShowMessage("Änderungen verworfen", MessageType.Success);
        }

        public void ExportGitBranchInfos(object parameter)
        {
            var savePath = (parameter as string);

            new ExcelDocumentFactory().CreateExcelDocumentAndSaveAsFile(savePath, GetGitBranchesAsDataTable());
        }

        public void SendTransportMail()
        {
            try
            {
                var mailBody = GenerateMailBody();

                var sdp = Process.Start("mailto:?subject=Heutige%20Transporte&body=" + HttpUtility.UrlEncode(mailBody).NotNullOrEmpty().Replace("+", "%20"));
                if (sdp != null)
                {
                    var waitCount = 0;
                    while (!sdp.HasExited)
                    {
                        if (waitCount == 20)
                        {
                            if (sdp.Responding)
                            {
                                sdp.CloseMainWindow();
                            }
                            else
                            {
                                sdp.Kill();
                            }
                            throw new Exception("EMail-Client konnte nicht geöffnet werden!");
                        }

                        waitCount++;
                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception ex)
            {
                Parent.ShowMessage("Fehler beim Senden der Mail: " + ex.Message, MessageType.Error);
            }
        }

        private string GenerateMailBody()
        {
            var strBuilder = new StringBuilder();

            strBuilder.AppendLine("Hallo,");
            strBuilder.AppendLine();
            strBuilder.AppendLine("folgende ITAs/Tickets wurden transportiert:");
            strBuilder.AppendLine();

            var branches = DataService.GetBranchesForTransportMail();

            var portalListe = new List<string>();
            var serverListe = new List<string>();

            foreach (var branch in branches)
            {
                foreach (var item in branch.PortalBoolListe.Where(p => p.IsChecked))
                {
                    if (!portalListe.Contains(item.Key))
                        portalListe.Add(item.Key);
                }

                foreach (var item in branch.ServerBoolListe.Where(s => s.IsChecked))
                {
                    if (!serverListe.Contains(item.Key))
                        serverListe.Add(item.Key);
                }
            }

            foreach (var portal in portalListe)
            {
                strBuilder.AppendLine(portal);
                strBuilder.AppendLine("----------------");

                foreach (var item in branches.Where(b => b.PortalBoolListe.Any(p => p.Key == portal && p.IsChecked)))
                    strBuilder.AppendLine(string.Format("- {0} ({1}): {2}", item.Name, item.PM, item.Bemerkung));

                strBuilder.AppendLine();
            }

            if (branches.Any(b => b.PortalBoolListe.None(p => p.IsChecked)))
            {
                strBuilder.AppendLine("sonstige");
                strBuilder.AppendLine("----------------");

                foreach (var item in branches.Where(b => b.PortalBoolListe.None(p => p.IsChecked)))
                    strBuilder.AppendLine(string.Format("- {0} ({1}): {2}", item.Name, item.PM, item.Bemerkung));

                strBuilder.AppendLine();
            }

            if (serverListe.Any())
            {
                strBuilder.AppendLine("Folgende Server sind betroffen:");
                strBuilder.AppendLine(string.Join(", ", serverListe));
                strBuilder.AppendLine();
            }

            strBuilder.AppendLine();
            strBuilder.AppendLine("IT-Entwicklung");

            return strBuilder.ToString();
        }

        #endregion

        private void FilterGitBranches()
        {
            DataService.FilterGitBranches();
            SendPropertyChanged("GitBranches");
        }

        private DataTable GetGitBranchesAsDataTable()
        {
            return (GitBranches != null ? GitBranches.ToExcelExportDataTable() : new DataTable());
        }
    }
}
