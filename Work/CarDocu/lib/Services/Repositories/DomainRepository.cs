using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using CarDocu.Models;
using GeneralTools.Services;
using Ionic.Zip;
using WpfTools4.Services;
using GeneralTools.Models;

namespace CarDocu.Services
{
    public class DomainRepository
    {
        #region Properties

        #region Settings

        public string UserErrorLogDirectoryName => EnsurePathExists(Path.Combine(DomainService.DomainPath, "ErrorLogs", LogonUser.LoginName.Replace(AdminNamePostFix, "")));

        public string PdfDirectoryName => EnsurePathExists(Path.Combine(DomainService.DomainPath, "PDF", LogonUser.LoginName.Replace(AdminNamePostFix, "")));

        public string GlobalSettingsDirectoryName => EnsurePathExists(Path.Combine(DomainService.DomainPath, GetType().Name));
        public DomainGlobalSettings GlobalSettings { get; private set; }

        public string TemplateSettingsDirectoryName => EnsurePathExists(Path.Combine(GlobalSettingsDirectoryName, "Templates"));

        public string EnterpriseSettingsDirectoryName => GlobalSettingsDirectoryName;
        public EnterpriseSettings EnterpriseSettings { get; private set; }

        public string UserSettingsDirectoryName => EnsurePathExists(Path.Combine(GlobalSettingsDirectoryName, "Users", LogonUser.LoginID));
        public DomainUserSettings UserSettings { get; set; }

        public string AppSettingsDirectoryName => EnsurePathExists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), AppSettings.AppSettingsDirectoryName));
        public AppSettings AppSettings { get; set; }

        #endregion


        public string UserName => LogonUser.FullName;

        public ScanDocumentRepository ScanDocumentRepository { get; set; }

        public ScanDocumentRepository ScanTemplateRepository { get; set; }

        public List<CardocuQueueEntity> PausedItemsForQueue { get; set; } = new List<CardocuQueueEntity>();

        public string AdminNamePostFix => "!17";
        private DomainUser _adminUser;
        public DomainUser AdminUser => (_adminUser ?? (_adminUser = new DomainUser
        {
            IsAdmin = true,
            IsMaster = true,
            LoginID = "405ce46f-2280-4fc2-82fc-d1cfd47fe9a6_sys",
            LoginName = "Admin" + AdminNamePostFix,
            VorName = "",
            NachName = "Admin",
            Email = "matthias.jenzen@kroschke.de",
            DomainLocation = (GlobalSettings.DomainLocations.Any() ? GlobalSettings.DomainLocations.First() : null),
        }));

        private DocumentType _scanTemplateDocType;
        public DocumentType ScanTemplateDocType => (_scanTemplateDocType ?? (_scanTemplateDocType = new DocumentType
        {
            Code = "SCAN_TEMPLATE",
            SapCode = "",
            ArchiveCode = "",
            Name = "Vorlage",
            Sort = 0,
            InputRule = "TP",
            IsSystemInternal = true,
            IsTemplate = true,
            Archive = new Archive(),
        }));

        public DomainUser LogonUser { get; set; }

        #endregion


        public DomainRepository()
        {
            AppSettingsLoad();
        }

        public void InitGlobalSettings()
        {
            GlobalSettings = new DomainGlobalSettings();
        }

        public void LoadGlobalSettings()
        {
            GlobalSettingsLoad();
            EnterpriseSettingsLoad();
        }

        public void LoadRemainingSettings()
        {
            ScanDocumentRepositoryLoad();

            UserSettingsLoad();
        }


        #region App Settings

        public void AppSettingsLoad()
        {
            AppSettings = XmlService.XmlDeserializeFromPath<AppSettings>(AppSettingsDirectoryName);
            AppSettings.Init();
        }

        public void AppSettingsSave()
        {
            XmlService.XmlSerializeToPath(AppSettings, AppSettingsDirectoryName);
        }

        #endregion


        #region Enterprise Settings

        public void EnterpriseSettingsLoad()
        {
            EnterpriseSettingsLoad(EnterpriseSettingsDirectoryName);
        }

        private void EnterpriseSettingsLoad(string directoryName)
        {
            DocumentType.IsCurrentylLoadingFromRepository = true;
            EnterpriseSettings = XmlService.XmlDeserializeFromPath<EnterpriseSettings>(directoryName);
            EnterpriseSettings.DocumentTypes.Insert(0, ScanTemplateDocType);
            DocumentType.IsCurrentylLoadingFromRepository = false;
        }

        public void EnterpriseSettingsSave()
        {
            EnterpriseSettingsSave(EnterpriseSettingsDirectoryName);
        }

        private void EnterpriseSettingsSave(string directoryName)
        {
            EnterpriseSettings.DocumentTypes.Remove(ScanTemplateDocType);
            XmlService.XmlSerializeToPath(EnterpriseSettings, directoryName);
            EnterpriseSettings.DocumentTypes.Insert(0, ScanTemplateDocType);
        }

        public void EnterpriseSettingsImport(string importDirectoryName)
        {
            EnterpriseSettingsLoad(importDirectoryName);
            EnterpriseSettingsSave();
        }

        public void EnterpriseSettingsExport(string exportDirectoryName)
        {
            EnterpriseSettingsSave(exportDirectoryName);
        }

        #endregion


        #region Global Settings

        public void GlobalSettingsLoad()
        {
            GlobalSettingsLoad(GlobalSettingsDirectoryName);
        }

        private void GlobalSettingsLoad(string directoryName)
        {
            GlobalSettings = XmlService.XmlDeserializeFromPath<DomainGlobalSettings>(directoryName);
            GlobalSettings.DomainUsers.Add(AdminUser);

            // Archives
            //if (!GlobalSettings.Archives.Any())
            //{
            //    GlobalSettings.Archives = Archive.FixedAvailableArchives;
            //    GlobalSettingsSave();
            //}
            if (GlobalSettings.MergeAvailableAndFixedArchives())
                GlobalSettingsSave();

            GlobalSettings.PatchArchives();
            GlobalSettingsSave();

            // Domain Locations (Standorte)
            if (!GlobalSettings.DomainLocations.Any())
            {
                GlobalSettings.DomainLocations = DomainLocation.FixedStartupDomainLocations;
                GlobalSettingsSave();
            }
        }

        public void GlobalSettingsSave()
        {
            GlobalSettingsSave(GlobalSettingsDirectoryName);
        }

        private void GlobalSettingsSave(string directoryName)
        {
            GlobalSettings.DomainUsers.Remove(AdminUser);
            XmlService.XmlSerializeToPath(GlobalSettings, directoryName);
            GlobalSettings.DomainUsers.Add(AdminUser);
        }

        #endregion


        #region User Settings

        public void UserSettingsLoad()
        {
            UserSettings = XmlService.XmlDeserializeFromPath<DomainUserSettings>(UserSettingsDirectoryName);
            UserSettings.SaveSettings = UserSettingsSave;

            //
            // Load logs:
            DomainService.Threads.BackgroundThreads
                .ForEach(thread =>thread.LogItemsLoad(UserSettingsDirectoryName));

            ResyncLogs();

            QueueUnprocessedItems();

            //DomainService.SendMail("matthias.jenzen@kroschke.de", "test", "Dies ist ein Test.");
        }

        public void ResyncLogs()
        {
            //
            // Merge back logs (merge back processed items to master list)
            // 1. SAP items
            var sapUnprocessedEntities = GetUnprocessedSapItems().Select(s => new SapLogItem { DocumentID = s.DocumentID }).ToList();
            if (DomainService.Threads.SapBackgroundTask != null)
                DomainService.Threads.SapBackgroundTask.DeliveryReSyncToMasterItems(sapUnprocessedEntities)
                    .ForEach(resyncedItem =>
                    {
                        var masterItem = ScanDocumentRepository.ScanDocuments.FirstOrDefault(sd => sd.DocumentID == resyncedItem.DocumentID);
                        if (masterItem != null)
                            masterItem.SapDeliveryDate = resyncedItem.DeliveryDate;
                    });

            // 2. SMTP items
            var smtpUnprocessedEntities = GetUnprocessedSmtpItems().Select(s => new ArchiveLogItem { DocumentID = s.DocumentID }).ToList();
            if (DomainService.Threads.ArchiveBackgroundTask != null)
                DomainService.Threads.ArchiveBackgroundTask.DeliveryReSyncToMasterItems(smtpUnprocessedEntities)
                    .ForEach(resyncedItem =>
                    {
                        var masterItem = ScanDocumentRepository.ScanDocuments.FirstOrDefault(sd => sd.DocumentID == resyncedItem.DocumentID);
                        if (masterItem != null)
                            masterItem.ArchiveDeliveryDate = resyncedItem.DeliveryDate;
                    });

            ScanDocumentRepositorySave();
        }

        private void QueueUnprocessedItems()
        {
            ScanDocumentsEnsureDocumentType();

            var sapUnprocessedEntities = GetUnprocessedSapItems().Select(s => new SapLogItem { DocumentID = s.DocumentID }).ToList();
            sapUnprocessedEntities.ForEach(item => DomainService.Threads.SapBackgroundTask.Enqueue(item));

            var smtpUnprocessedEntities = GetUnprocessedSmtpItems().Select(s => new ArchiveLogItem { DocumentID = s.DocumentID }).ToList();
            smtpUnprocessedEntities.ForEach(item => DomainService.Threads.ArchiveBackgroundTask.Enqueue(item));
        }

        public void TryQueueNewItem(ScanDocument scanDocument)
        {
            if (scanDocument.IsTemplate)
                return;

            if (scanDocument.BackgroundDeliveryDisabled)
                return;

            var sapLogItem = new SapLogItem {DocumentID = scanDocument.DocumentID};
            var archiveLogItem = new ArchiveLogItem {DocumentID = scanDocument.DocumentID};
            if (scanDocument.BatchScanned)
            {
                PausedItemsForQueue.Add(sapLogItem);
                PausedItemsForQueue.Add(archiveLogItem);
                return;
            }

            EnqueueItemForBackgroundTask(sapLogItem);
            EnqueueItemForBackgroundTask(archiveLogItem);
        }

        public void EnqueueAllPausedItemsForBackgroundTasks()
        {
            DomainService.Repository.ScanDocumentRepositorySave();
            PausedItemsForQueue.ForEach(item => EnqueueItemForBackgroundTask(item));

            DeleteAllPausedItemsForBackgroundTasks();
        }

        public void DeleteAllPausedItemsForBackgroundTasks()
        {
            PausedItemsForQueue.Clear();

            DomainService.Repository.ScanDocumentRepositoryLoad();
        }

        private static void EnqueueItemForBackgroundTask(CardocuQueueEntity item)
        { 
            if (item is SapLogItem)
                DomainService.Threads.SapBackgroundTask.Enqueue(item);

            if (item is ArchiveLogItem)
                DomainService.Threads.ArchiveBackgroundTask.Enqueue(item);
        }

        public void UserSettingsSave()
        {
            XmlService.XmlTrySerializeToPath(UserSettings, UserSettingsDirectoryName);

            DomainService.Threads.BackgroundThreads.ForEach(thread => thread.LogItemsSave());
        }

        public void LogItemFilesDelete()
        {
            DomainService.Threads.BackgroundThreads
                .ForEach(thread => thread.LogItemFileDelete(UserSettingsDirectoryName));
        }

        #endregion


        #region ScanDocumentRepository

        public void ScanDocumentRepositoryLoad()
        {
            ScanDocumentRepository = XmlService.XmlDeserializeFromPath<ScanDocumentRepository>(UserSettingsDirectoryName);
            ScanTemplateRepositoryLoad();
        }

        public void ScanDocumentRepositorySave()
        {
            ScanDocumentRepository.Save(UserSettingsDirectoryName);
            ScanTemplateRepositorySave();
        }

        public bool ScanDocumentRepositoryTryAddScanDocument(ScanDocument scanDocument)
        {
            if (scanDocument.IsTemplate)
                return ScanTemplateRepository.TryAddScanDocument(scanDocument);

            return ScanDocumentRepository.TryAddScanDocument(scanDocument);
        }

        public bool ScanDocumentRepositoryTryDeleteScanDocument(ScanDocument scanDocument, bool deleteAlsoNetworkDeliveryPdfFiles)
        {
            if (scanDocument.IsTemplate)
                return ScanTemplateRepository.TryDeleteScanDocument(scanDocument, deleteAlsoNetworkDeliveryPdfFiles);

            return ScanDocumentRepository.TryDeleteScanDocument(scanDocument, deleteAlsoNetworkDeliveryPdfFiles);
        }

        #endregion


        #region ScanTemplateRepository

        private void ScanTemplateRepositoryLoad()
        {
            ScanTemplateRepository = XmlService.XmlDeserializeFromPath<ScanDocumentRepository>(TemplateSettingsDirectoryName, "ScanTemplateRepository");
        }

        private void ScanTemplateRepositorySave()
        {
            ScanTemplateRepository.Save(TemplateSettingsDirectoryName, "ScanTemplateRepository");
        }

        public bool ScanTemplateRepositoryTryAddScanTemplate(ScanDocument scanTemplate)
        {
            return ScanTemplateRepository.TryAddScanDocument(scanTemplate);
        }

        #endregion


        public void ScanPdfFolderOpen()
        {
            Process.Start(PdfDirectoryName);
        }


        public void GlobalSettingsMock()
        {
            GlobalSettings = new DomainGlobalSettings
                                 {
                                     ScanDataUrl = "www.dad.de",
                                     DomainUsers = new List<DomainUser>
                                                       {
                                                           new DomainUser
                                                               {
                                                                   LoginName = "matze",
                                                                   VorName = "Matthias",
                                                                   NachName = "Jenzen",
                                                                   Email = "matthias@jenzen.de"
                                                               },
                                                           new DomainUser
                                                               {
                                                                   LoginName = "gundi",
                                                                   VorName = "Gundulbert",
                                                                   NachName = "Halmacken",
                                                                   Email = "gdb@gmx.de"
                                                               },
                                                           new DomainUser
                                                               {
                                                                   LoginName = "wz",
                                                                   VorName = "Walter",
                                                                   NachName = "Zabel",
                                                                   Email = "wz@yahoo.de"
                                                               }
                                                       }
                                 };
        }

        static string EnsurePathExists(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }

        private void ScanDocumentsEnsureDocumentType()
        {
            ScanDocumentRepository.ScanDocuments.ForEach(sd => sd.EnsureDocumentType());
        }

        private IEnumerable<ScanDocument> GetUnprocessedSapItems()
        {
            return ScanDocumentRepository.ScanDocuments.Where(sd => sd.SapDeliveryDate == null && !sd.BackgroundDeliveryDisabled);
        }

        private IEnumerable<ScanDocument> GetUnprocessedSmtpItems()
        {
            return ScanDocumentRepository.ScanDocuments.Where(sd => sd.ArchiveDeliveryDate == null && !sd.BackgroundDeliveryDisabled); // && sd.ArchiveMailDeliveryNeeded);
        }

        public bool ZipArchiveUserLogsAndScanDocuments(ProgressBarOperation progressBarOperation)
        {
            var directoryToZip = PdfDirectoryName;

            var backupArchivePath = GlobalSettings.BackupArchive.Path;
            var zipArchivePath = GlobalSettings.ZipArchive.Path;
            var tempPath = GlobalSettings.TempPath;

            var tempRawFileName = $"{"CkgScanClient_PDF___"}__{DateTime.Now.ToString("yyyy_MM_dd___HH_mm_ss")}.zip";
            var tempFileName = Path.Combine(tempPath, tempRawFileName);

            var fileList = Directory.EnumerateFiles(directoryToZip, "*.*", SearchOption.AllDirectories).ToList();

            progressBarOperation.Current = 1;
            progressBarOperation.Total = fileList.Count;
            progressBarOperation.Header = "ZIP-Archivierung";
            progressBarOperation.Details = "Archiviere Datei";
            progressBarOperation.ProgressInfoVisible = true;

            try
            {
                var zip = new ZipFile();
                foreach (var filename in fileList)
                {
                    if (progressBarOperation.IsCancellationPending)
                        return false;

                    var e = zip.AddFile(filename, "PDF");
                    e.Comment = $"Datei hinzugefügt von '{DomainService.AppName}'";

                    progressBarOperation.Current++;
                    Thread.Sleep(1);
                }

                progressBarOperation.ProgressInfoVisible = false;
                progressBarOperation.BusyCircleVisible = true;
                zip.Comment = $"ZIP-Datei erstellt von '{DomainService.AppName}' auf Rechner '{System.Net.Dns.GetHostName()}'";

                // lengthly atomar operation here...
                progressBarOperation.Details = "Erstelle ZIP Datei ... bitte warten ...";
                zip.Save(tempFileName);

                if (progressBarOperation.IsCancellationPending)
                {
                    FileService.TryFileDelete(tempFileName);
                    return false;
                }


                // Copy ZIP file to ZIP folder:
                var zipFileName = Path.Combine(zipArchivePath, tempRawFileName);
                progressBarOperation.Details = "Kopiere ZIP Datei  ...";
                FileService.TryFileCopy(tempFileName, zipFileName);
                Thread.Sleep(1000);


                if (backupArchivePath.IsNotNullOrEmpty() && FileService.PathExistsAndWriteEnabled(backupArchivePath))
                {
                    // Additionally create a backup of the ZIP file:
                    var backupFileName = Path.Combine(backupArchivePath, tempRawFileName);
                    progressBarOperation.Details = "Erstelle zusätzliches Backup von ZIP Datei  ...";
                    FileService.TryFileCopy(tempFileName, backupFileName);
                    Thread.Sleep(1000);
                }


                // another lengthly atomar operation here...
                progressBarOperation.Details = "Datenbereinigung ... bitte warten ...";
                FileService.TryFileDelete(tempFileName);
                FileService.TryDirectoryDelete(directoryToZip);
                FileService.TryDirectoryDelete(UserSettingsDirectoryName);
                Thread.Sleep(1000);

                if (progressBarOperation.IsCancellationPending)
                    return false;
            }
            catch (Exception e)
            {
                Tools.AlertError("Fehler bei der ZIP-Archivierung, Details:\r\n\r\n" + e.Message);
                return false;
            }

            return true;
        }

        static bool AutoRecycleIsDateObsolete(DateTime date)
        {
            const int deleteFilesOlderThanDays = 4;

            return (date.AddDays(deleteFilesOlderThanDays) < DateTime.Now);
        }

        public bool AutoRecycleUserLogsAndScanDocuments(ProgressBarOperation progressBarOperation)
        {
            var pdfFileList = Directory.EnumerateFiles(PdfDirectoryName, "*.*", SearchOption.AllDirectories)
                .Where(pdf =>
                {
                    var fileInfo = new FileInfo(pdf);
                    return AutoRecycleIsDateObsolete(fileInfo.LastWriteTime);

                }).ToList();

            var validScanDocuments = ScanDocumentRepository.ScanDocuments
                .Where(scanDoc =>
                {
                    if (scanDoc.ArchiveDeliveryDate == null)
                        return false;

                    return AutoRecycleIsDateObsolete(scanDoc.ArchiveDeliveryDate.GetValueOrDefault());

                }).ToList().Copy();

            var totalCount = pdfFileList.Count + validScanDocuments.Count;

            if (totalCount == 0)
                return true;

            progressBarOperation.Current = 1;
            progressBarOperation.Total = totalCount;
            progressBarOperation.Header = "Automatische Datenbereinigung ... bitte warten ...";
            progressBarOperation.Details = "Lösche Dateien";
            progressBarOperation.ProgressInfoVisible = true;
            Thread.Sleep(500);

            try
            {
                foreach (var filename in pdfFileList)
                {
                    if (progressBarOperation.IsCancellationPending)
                        return false;

                    progressBarOperation.Details = $"Lösche Datei '{filename}'";
                    var pdfFileName = Path.Combine(PdfDirectoryName, filename);

                    // PDF Datei löschen
                    FileService.TryFileDelete(pdfFileName);

                    progressBarOperation.Current++;
                    Thread.Sleep(50);
                }

                if (progressBarOperation.IsCancellationPending)
                    return false;

                foreach (var scanDocument in validScanDocuments)
                {
                    if (progressBarOperation.IsCancellationPending)
                        return false;

                    var directoryInfo = new DirectoryInfo(scanDocument.GetDocumentPrivateDirectoryName());
                    var directoryName = directoryInfo.Name;
                    progressBarOperation.Details = $"Lösche temporäres Verzeichnis '{directoryName}'";

                    // ScanDocument aus Repository löschen  +  ScanDocument temp. Verzeichnis löschen
                    var task = TaskService.StartLongRunningTask(() =>
                    {
                        var sdOrg = ScanDocumentRepository.ScanDocuments.FirstOrDefault(s => s.DocumentID == scanDocument.DocumentID);
                        if (sdOrg != null)
                            ScanDocumentRepository.TryDeleteScanDocument(sdOrg, false);
                    });
                    if (!task.Wait(10000))
                        throw new Exception($"Timeout beim Löschen des temporären Verzeichnisses '{directoryName}'");

                    progressBarOperation.Current++;
                    Thread.Sleep(50);
                }

                ScanDocumentRepositorySave();

                if (progressBarOperation.IsCancellationPending)
                    return false;

                progressBarOperation.Header = "Erfolg";
                progressBarOperation.Details = "Die automatische Datenbereinigung wurde erfolgreich durchgeführt!";
                Thread.Sleep(1500);
            }
            catch (Exception e)
            {
                Tools.AlertError("Fehler bei der automatischen Datenbereinigung, Details:\r\n\r\n" + e.Message);
                return false;
            }

            return true;
        }

        public bool ZipArchiveRecycle(ProgressBarOperation progressBarOperation)
        {
            progressBarOperation.Header = "Backup Ordner Bereinigung";
            progressBarOperation.Details = "Backup Ordner wird bereinigt ... einen Moment bitte ...";
            progressBarOperation.ProgressInfoVisible = false;
            progressBarOperation.BusyCircleVisible = true;

            var success = FileService.TryDirectoryDelete(GlobalSettings.BackupArchive.Path);
            FileService.TryDirectoryCreate(GlobalSettings.BackupArchive.Path);
            Thread.Sleep(1000);

            return success;
        }

        public DocumentType GetImageDocumentType(string documentTypeCode)
        {
            if (documentTypeCode == null)
                return null;

            return EnterpriseSettings.DocumentTypes.FirstOrDefault(docTypes => docTypes.Code == documentTypeCode);
        }
    }
}
