using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
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

        public string UserErrorLogDirectoryName
        {
            get
            {
                return EnsurePathExists(Path.Combine(DomainService.DomainPath, "ErrorLogs", LogonUser.LoginName.Replace(AdminNamePostFix, "")));
            }
        }

        public string PdfDirectoryName { get { return EnsurePathExists(Path.Combine(DomainService.DomainPath, "PDF", LogonUser.LoginName.Replace(AdminNamePostFix, ""))); } }

        public string GlobalSettingsDirectoryName { get { return EnsurePathExists(Path.Combine(DomainService.DomainPath, this.GetType().Name)); } }
        public DomainGlobalSettings GlobalSettings { get; private set; }

        public string TemplateSettingsDirectoryName { get { return EnsurePathExists(Path.Combine(GlobalSettingsDirectoryName, "Templates")); } }

        public string EnterpriseSettingsDirectoryName { get { return GlobalSettingsDirectoryName; } }
        public EnterpriseSettings EnterpriseSettings { get; private set; }

        public string UserSettingsDirectoryName { get { return EnsurePathExists(Path.Combine(GlobalSettingsDirectoryName, "Users", LogonUser.LoginID)); } }
        public DomainUserSettings UserSettings { get; set; }

        public string AppSettingsDirectoryName { get { return EnsurePathExists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), AppSettings.AppSettingsDirectoryName)); } }
        public AppSettings AppSettings { get; set; }

        #endregion


        public string UserName { get { return LogonUser.FullName; } }

        public ScanDocumentRepository ScanDocumentRepository { get; set; }

        public ScanDocumentRepository ScanTemplateRepository { get; set; }

        public string AdminNamePostFix { get { return "!17"; } }
        private DomainUser _adminUser;
        public DomainUser AdminUser
        {
            get
            {
                return (_adminUser ?? (_adminUser = new DomainUser
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
            }
        }

        private DocumentType _scanTemplateDocType;
        public DocumentType ScanTemplateDocType
        {
            get
            {
                return (_scanTemplateDocType ?? (_scanTemplateDocType = new DocumentType
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
            }
        }

        public DomainUser LogonUser { get; set; }

        #endregion


        public DomainRepository()
        {
            AppSettingsLoad();
        }

        public void InitGlobalSettings()
        {
            GlobalSettingsLoad();
            EnterpriseSettingsLoad();
        }

        public void InitRemainingSettings()
        {
            ScanDocumentRepositoryLoad();

            UserSettingsLoad();
        }


        #region App Settings

        public void AppSettingsLoad()
        {
            AppSettings = XmlService.XmlDeserializeFromPath<AppSettings>(AppSettingsDirectoryName);
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
            EnterpriseSettings = XmlService.XmlDeserializeFromPath<EnterpriseSettings>(directoryName);
            EnterpriseSettings.DocumentTypes.Insert(0, ScanTemplateDocType);
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

        public void QueueUnprocessedItems()
        {
            var sapUnprocessedEntities = GetUnprocessedSapItems().Select(s => new SapLogItem { DocumentID = s.DocumentID }).ToList();
            sapUnprocessedEntities.ForEach(item => DomainService.Threads.SapBackgroundTask.Enqueue(item));

            var smtpUnprocessedEntities = GetUnprocessedSmtpItems().Select(s => new ArchiveLogItem { DocumentID = s.DocumentID }).ToList();
            smtpUnprocessedEntities.ForEach(item => DomainService.Threads.ArchiveBackgroundTask.Enqueue(item));
        }

        public void TryQueueNewItem(ScanDocument scanDocument)
        {
            if (scanDocument.IsTemplate)
                return;

            DomainService.Threads.SapBackgroundTask.Enqueue(new SapLogItem { DocumentID = scanDocument.DocumentID });
            DomainService.Threads.ArchiveBackgroundTask.Enqueue(new ArchiveLogItem { DocumentID = scanDocument.DocumentID });
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

        public bool ScanDocumentRepositoryTryDeleteScanDocument(ScanDocument scanDocument)
        {
            if (scanDocument.IsTemplate)
                return ScanTemplateRepository.TryDeleteScanDocument(scanDocument);

            return ScanDocumentRepository.TryDeleteScanDocument(scanDocument);
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

        public bool ScanTemplateRepositoryTryDeleteScanTemplate(ScanDocument scanTemplate)
        {
            return ScanTemplateRepository.TryDeleteScanDocument(scanTemplate);
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

        public IEnumerable<ScanDocument> GetUnprocessedSapItems()
        {
            return ScanDocumentRepository.ScanDocuments.Where(sd => sd.SapDeliveryDate == null);
        }

        public IEnumerable<ScanDocument> GetUnprocessedSmtpItems()
        {
            return ScanDocumentRepository.ScanDocuments.Where(sd => sd.ArchiveDeliveryDate == null); // && sd.ArchiveMailDeliveryNeeded);
        }

        public bool ZipArchiveUserLogsAndScanDocuments(ProgressBarOperation progressBarOperation)
        {
            var directoryToZip = UserSettingsDirectoryName;
            var zipArchivePath = GlobalSettings.ZipArchive.Path;
            var zipFileName = Path.Combine(zipArchivePath, string.Format("{0}__{1}.zip", LogonUser.LoginID, DateTime.Now.ToString("yyyy_MM_dd___HH_mm_ss")));

            var fileList = Directory.EnumerateFiles(directoryToZip, "*.*", SearchOption.AllDirectories).ToList();

            progressBarOperation.Current = 0;
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

                    var e = zip.AddFile(filename);
                    e.Comment = string.Format("Datei hinzugefügt von '{0}'", DomainService.AppName);

                    progressBarOperation.Current++;
                    Thread.Sleep(1);
                }

                progressBarOperation.ProgressInfoVisible = false;
                progressBarOperation.BusyCircleVisible = true;
                zip.Comment = string.Format("ZIP-Datei erstellt von '{0}' auf Rechner '{1}'", DomainService.AppName, System.Net.Dns.GetHostName());

                // lengthly atomar operation here...
                progressBarOperation.Details = "Speichern ... bitte warten Sie einen Moment ...";
                zip.Save(zipFileName);

                if (progressBarOperation.IsCancellationPending)
                {
                    FileService.TryFileDelete(zipFileName);
                    return false;
                }

                // another lengthly atomar operation here...
                progressBarOperation.Details = "Datenbereinigung ... bitte warten Sie einen Moment ...";
                FileService.TryDirectoryDelete(directoryToZip);
                Thread.Sleep(2000);

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

        public DocumentType GetImageDocumentType(string documentTypeCode)
        {
            if (documentTypeCode.IsNullOrEmpty())
                return null;

            return EnterpriseSettings.DocumentTypes.FirstOrDefault(docTypes => docTypes.Code == documentTypeCode);
        }
    }
}
