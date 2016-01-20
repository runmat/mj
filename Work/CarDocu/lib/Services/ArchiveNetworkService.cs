using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CarDocu.Models;
using GeneralTools.Models;
using GeneralTools.Services;

namespace CarDocu.Services
{
    public class ArchiveNetworkService
    {
        public bool IsOnline()
        {
            bool isOnline;

            try
            {
                if (DomainService.Repository.AppSettings.OnlineStatusAutoCheckDisabled)
                    isOnline = true;
                else
                    isOnline = DomainService.CheckOnlineState();
            }
            catch { isOnline = false; }

            return isOnline;
        }

        public bool ProcessArchivMeldung(ref CardocuQueueEntity baseLogItem)
        {
            var logItem = (ArchiveLogItem)baseLogItem;

            var logItemID = logItem.DocumentID;
            var scanDocument = DomainService.Repository.ScanDocumentRepository.ScanDocuments.FirstOrDefault(sd => sd.DocumentID == logItemID);
            if (scanDocument == null)
                return false;
            if (scanDocument.ScanImages.Count == 0)
                scanDocument.XmlLoadScanImages();

            int mailItemCount;
            if (!NetworkDeliveryToArchive(ref scanDocument, out mailItemCount))
                return false;

            //
            // delivery successfull => let's mark all corresponding entries with an apropiate delivery date right here:
            //
            var deliveryDate = DateTime.Now;
            logItem.DeliveryDate = deliveryDate;
            SetDeliveryDate(scanDocument, deliveryDate);

            logItem.FinNummer = scanDocument.FinNumber;
            logItem.KundenNr = scanDocument.KundenNr;
            logItem.StandortCode = scanDocument.StandortCode;

            logItem.MailDeliveryNeeded = scanDocument.ArchiveMailDeliveryNeeded;
            if (logItem.MailDeliveryNeeded)
                logItem.MailItemCount = mailItemCount;

            return true;
        }

        public static void SetDeliveryDate(ScanDocument scanDocument, DateTime deliveryDate)
        {
            scanDocument.ArchiveDeliveryDate = deliveryDate;
        }

        static bool NetworkDeliveryToArchive(ref ScanDocument scanDocument, out int mailItemCount)
        {
            mailItemCount = 0;
            var archiveMailDeliveryNeeded = scanDocument.ArchiveMailDeliveryNeeded;
            scanDocument.EnsureDocumentType();
            var docType = scanDocument.SelectedDocumentType;

            try
            {
                DomainService.Logger.LogMessage("Info: Funktion NetworkDeliveryToArchive, Start ...");

                var archive = scanDocument.GetArchive();

                if (archive == null)
                {
                    DomainService.Logger.LogMessage("Fehler: Kein Archiv vorhanden (Funktion NetworkDeliveryToArchive)");
                    return false;
                }

                var archiveFolder = archive.Path;

                var pdfFileNames = scanDocument.GetPdfFileNames();
                if (!pdfFileNames.Any())
                {
                    DomainService.Logger.LogMessage("Fehler: Keine PDF-Dateien vorhanden (Funktion NetworkDeliveryToArchive)");
                    return false;
                }

                if (archiveMailDeliveryNeeded)
                {
                    // mail delivery occurs right here:
                    if (!MailDeliveryToUser(scanDocument, pdfFileNames, docType, out mailItemCount))
                    {
                        DomainService.Logger.LogMessage("Fehler: NetworkDelivery abgebrochen (Funktion NetworkDeliveryToArchive)");
                        return false;
                    }
                }


                pdfFileNames.ForEach(srcFileName =>
                {
                    if (!File.Exists(srcFileName))
                        // no source file available anymore, 
                        // => so it's ok for us, because the file has already been copied actually
                        return;

                    var srcFileInfo = new FileInfo(srcFileName);
                    var dstFileName = Path.Combine(archiveFolder, srcFileInfo.Name);
                    var dstFileName2 = docType.InlineNetworkDeliveryArchiveFolder.IsNullOrEmpty() ? "" : Path.Combine(docType.InlineNetworkDeliveryArchiveFolder, srcFileInfo.Name);

                    if (docType.UseExternalCommandline)
                    {
                        var externalCommandProgram = docType.ExternalCommandlineProgramPath;
                        var externalCommandArgs = docType.ExternalCommandlineArguments;

                        var args = externalCommandArgs.Replace("%1", string.Format("\"{0}\"", srcFileName));

                        var pi = new ProcessStartInfo
                        {
                            FileName = externalCommandProgram,
                            Arguments = args,
                            WindowStyle = ProcessWindowStyle.Hidden
                        };
                        var p = Process.Start(pi);
                        if (p == null)
                            throw new Exception(string.Format("Fehler beim Aufruf des externen Processes \"{0}\" mit Parameter {1}", externalCommandProgram, args));

                        const int timeoutMinutes = 10;
                        p.WaitForExit(timeoutMinutes*60*1000);

                        if (p.ExitCode != 0)
                            throw new Exception(string.Format("Externer Process meldet Fehler-Code {2}, Process = \"{0}\", Parameter = {1}", externalCommandProgram, args, p.ExitCode)); 
                    }
                    else
                    {
                        FileService.TryFileDelete(dstFileName);
                        File.Copy(srcFileName, dstFileName, true);

                        if (dstFileName2.IsNotNullOrEmpty())
                        {
                            FileService.TryFileDelete(dstFileName2);
                            File.Copy(srcFileName, dstFileName2, true);
                        }
                    }

                    TryDeleteAndBackupFileAfterDelivery(docType, srcFileName);

                });
            }
            catch (Exception e)
            {
                DomainService.Logger.LogMessage("Fehler: Kopieren der PDF-Datei fehlgeschlagen (Funktion NetworkDeliveryToArchive)");
                DomainService.Logger.LogMessage(e);
                return false;
            }

            DomainService.Logger.LogMessage("Info: Funktion NetworkDeliveryToArchive, End ...");

            return true;
        }

        private static void TryDeleteAndBackupFileAfterDelivery(DocumentType docType, string srcFileName)
        {
            if (!DomainService.Repository.AppSettings.GlobalDeleteAndBackupFileAfterDelivery)
                if (!docType.DeleteAndBackupFileAfterDelivery)
                    return;

            var srcFileInfo = new FileInfo(srcFileName);

            var backupFolder = DomainService.Repository.GlobalSettings.BackupArchive.Path;
            if (backupFolder.IsNotNullOrEmpty())
            {
                var backupFileName = Path.Combine(backupFolder, srcFileInfo.Name);
                FileService.TryFileCopy(srcFileName, backupFileName);
                FileService.TryFileDelete(srcFileName);
            }
        }

        public void DeletePdfFilesFor(ScanDocument scanDocument, IEnumerable<string> pdfFileNames, bool deleteAlsoNetworkDeliveryPdfFiles)
        {
            var archive = scanDocument.GetArchive();

            if (archive == null)
                return ;

            var archiveFolder = archive.Path;
            scanDocument.EnsureDocumentType();
            var docType = scanDocument.SelectedDocumentType;

            pdfFileNames.ToList().ForEach(srcFileName =>
            {
                var srcFileInfo = new FileInfo(srcFileName);
                FileService.TryFileDelete(srcFileName);

                if (!deleteAlsoNetworkDeliveryPdfFiles)
                    return;

                var dstFileName = Path.Combine(archiveFolder, srcFileInfo.Name);
                var dstFileName2 = docType.InlineNetworkDeliveryArchiveFolder.IsNullOrEmpty() ? "" : Path.Combine(docType.InlineNetworkDeliveryArchiveFolder, srcFileInfo.Name);

                FileService.TryFileDelete(dstFileName);
                if (dstFileName2.IsNotNullOrEmpty())
                    FileService.TryFileDelete(dstFileName2);
            });
        }

        static bool MailDeliveryToUser(ScanDocument scanDocument, IEnumerable<string> pdfFileNames, DocumentType docType, out int mailItemCount)
        {
            var mailSettings = DomainService.Repository.GlobalSettings.SmtpSettings;
            var attachmentMaxSizeBytes = mailSettings.AttachmentMaxSizeMB * 1000000;

            //
            // split all attachments to fit "attachmentMaxSizeMB" settings:
            //

            // 1. order all attachments by FileSize, acending:
            var pdfFilenameSizeDict =
                pdfFileNames.Select(pdfFileName => new { FileName = pdfFileName, Size = (int)new FileInfo(pdfFileName).Length })
                    .OrderBy(p => p.Size);

            // 2. generate the split list:
            var allList = new List<List<string>>();
            var currentSplitSize = 0;
            var splitList = new List<string>();
            allList.Add(splitList);
            foreach (var pdfFilenameSize in pdfFilenameSizeDict)
            {
                currentSplitSize += pdfFilenameSize.Size;
                if (currentSplitSize > attachmentMaxSizeBytes && splitList.Count > 0)
                {
                    currentSplitSize = 0;
                    splitList = new List<string>();
                    allList.Add(splitList);
                }
                splitList.Add(pdfFilenameSize.FileName);
            }

            var success = true;

            // 3. iterate the split list and send splitted e-mails:
            for (var i = 0; i < allList.Count; i++)
            {
                splitList = allList[i];

                var postfix = "";
                if (allList.Count > 1)
                    postfix = string.Format(", Mail {0} / {1}", (i + 1), allList.Count);

                var subject = string.Format(mailSettings.EmailSubjectText, scanDocument.FinNumber, postfix);
                var bodyText = string.Format(mailSettings.EmailBodyText, scanDocument.FinNumber, postfix);

                var emailRecipient = DomainService.Repository.LogonUser.Email;
                if (!string.IsNullOrEmpty(mailSettings.EmailRecipient))
                    emailRecipient = mailSettings.EmailRecipient;
                if (mailSettings.EmailSendAlsoToUser && emailRecipient != DomainService.Repository.LogonUser.Email)
                    emailRecipient += "," + DomainService.Repository.LogonUser.Email;


                if (!DomainService.SendMail(emailRecipient,
                                                 subject,
                                                 bodyText,
                                                 splitList))
                    success = false;
            }

            if (success)
                foreach (var list in allList)
                    list.ForEach(srcFileName => TryDeleteAndBackupFileAfterDelivery(docType, srcFileName));

            mailItemCount = allList.Count;

            return success;
        }
    }
}
