using System;
using System.Collections.Generic;
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

            try { isOnline = DomainService.SendTestMail(); }
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

            try
            {
                DomainService.Logger.LogMessage("Info: Funktion NetworkDeliveryToArchive, Start ...");

                var archive = scanDocument.Archive;

                if (archive == null)
                {
                    DomainService.Logger.LogMessage("Fehler: Kein Archiv vorhanden (Funktion NetworkDeliveryToArchive)");
                    return false;
                }

                DomainService.Logger.LogMessage("Info: Funktion NetworkDeliveryToArchive, Archiv: " + archive.Name + ", Pfad: " + archive.Path);

                var archiveFolder = archive.Path;

                DomainService.Logger.LogMessage("Info: Funktion NetworkDeliveryToArchive, ScanDokument.ID: " + scanDocument.DocumentID.NotNullOrEmpty());

                var pdfFileNames = scanDocument.GetPdfFileNames();
                if (!pdfFileNames.Any())
                {
                    DomainService.Logger.LogMessage("Fehler: Keine PDF-Dateien vorhanden (Funktion NetworkDeliveryToArchive)");
                    return false;
                }

                DomainService.Logger.LogMessage("Info: Funktion NetworkDeliveryToArchive, ScanDocument.ArchiveMailDeliveryNeeded:" + scanDocument.ArchiveMailDeliveryNeeded);

                if (archiveMailDeliveryNeeded)
                {
                    // mail delivery occurs right here:
                    if (!MailDeliveryToUser(scanDocument, pdfFileNames, out mailItemCount))
                    {
                        DomainService.Logger.LogMessage("Fehler: NetworkDelivery abgebrochen (Funktion NetworkDeliveryToArchive)");
                        return false;
                    }
                }

                //throw new Exception("TEST-ERROR");

                DomainService.Logger.LogMessage("Info: Funktion NetworkDeliveryToArchive, Before Copy  ...");

                pdfFileNames.ForEach(srcFileName =>
                {
                    var srcFileInfo = new FileInfo(srcFileName);
                    var dstFileName = Path.Combine(archiveFolder, srcFileInfo.Name);

                    DomainService.Logger.LogMessage("Info: Copy PDF-Datei, Quell-Datei: " + srcFileName + " (Funktion NetworkDeliveryToArchive)");
                    DomainService.Logger.LogMessage("Info: Copy PDF-Datei, Ziel-Datei: " + dstFileName + " (Funktion NetworkDeliveryToArchive)");

                    FileService.TryFileDelete(dstFileName);
                    File.Copy(srcFileName, dstFileName, true);
                });

                DomainService.Logger.LogMessage("Info: Funktion NetworkDeliveryToArchive, After Copy  ...");
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

        static bool MailDeliveryToUser(ScanDocument scanDocument, IEnumerable<string> pdfFileNames, out int mailItemCount)
        {
            mailItemCount = 0;

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
                    return false;
            }

            mailItemCount = allList.Count;

            return true;
        }
    }
}
