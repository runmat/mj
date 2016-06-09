using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Windows.Input;
using System.Xml.Serialization;
using System.Linq;
using CarDocu.Services;
using DocumentTools.Services;
using GeneralTools.Models;
using GeneralTools.Services;
using WpfTools4.Commands;

namespace CarDocu.Models
{
    public class ScanDocument : ModelBase
    {
        #region Properties

        [Key]
        public string DocumentID { get; set; }

        public string FinNumber { get; set; }

        public string PdfErrorGuid { get; set; }

        public string StandortCode { get; set; }

        public string KundenNr { get; set; }

        [XmlIgnore]
        public List<ScanImage> ScanImages { get; set; } = new List<ScanImage>();

        public int ScanImagesCount { get; set; }

        public string CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        private bool _pdfIsSynchronized; 
        public bool PdfIsSynchronized 
        { 
            get { return _pdfIsSynchronized; }
            set { _pdfIsSynchronized = value; SendPropertyChanged("PdfIsSynchronized"); }
        }

        public bool FinNumberUppercase => SelectedDocumentType?.InputRuleObject.AllowedLengths.Any() ?? false;

        [XmlIgnore]
        public bool ValidFinNumber
        {
            get
            {
                if (!string.IsNullOrEmpty(FinNumber) && ValidDocumentType)
                {
                    if (SelectedDocumentType.InputRuleObject.AllowedLengths.None()) 
                        return true;

                    if (SelectedDocumentType.InputRuleObject.AllowedLengths.Contains(FinNumber.Length))
                    {
                        // Range nur prüfen, wenn BatchScan aktiv ist.
                        if (SelectedDocumentType.IsBatchScanAllowed && !SelectedDocumentType.BarcodeAlphanumericAllowed)
                            return SelectedDocumentType.IsBarcodeInRange(FinNumber);

                        return true;
                    }
                    return false;
                }
                return false;
            }
        }

        private DocumentType _selectedDocumentType;
        [XmlIgnore]
        public DocumentType SelectedDocumentType
        {
            get { return _selectedDocumentType; }
            set
            {
                _selectedDocumentType = value;
                SendPropertyChanged("SelectedDocumentType");
                SendPropertyChanged("ValidFinNumber");
                SendPropertyChanged("ValidDocumentType");
            }
        }

        [XmlIgnore]
        public bool ValidDocumentType => (SelectedDocumentType != null);

        [XmlIgnore]
        public List<string> ScanDocumentTypeCodes { get { return ScanImages.GroupBy(scanImage => scanImage.ImageDocumentTypeCode).Select(g => g.Key).ToList(); } }

        [XmlIgnore]
        public List<string> ScanDocumentTypeCodesSAP { get { return ScanImages.GroupBy(scanImage => scanImage.ImageDocumentTypeCodeSAP).Select(g => g.Key).ToList(); } }

        public bool IsTemplate { get; set; }

        [XmlIgnore]
        public ICommand EmailSendoutAgainCommand { get; private set; }

        public bool BatchScanned { get; set; }


        #region Delivery

        private DateTime? _sapDeliveryDate;
        public DateTime? SapDeliveryDate
        {
            get { return _sapDeliveryDate; }
            set { _sapDeliveryDate = value; SendPropertyChanged("SapDeliveryDate"); }
        }

        private DateTime? _archiveDeliveryDate;
        public DateTime? ArchiveDeliveryDate
        {
            get { return _archiveDeliveryDate; }
            set { _archiveDeliveryDate = value; SendPropertyChanged("ArchiveDeliveryDate"); }
        }

        public bool ArchiveMailDeliveryNeeded { get; set; }

        #endregion

        #region Archive

        public Archive GetDefaultArchive()
        {
            return DomainService.Repository.GlobalSettings.Archives.FirstOrDefault(a => a.ID == (ArchiveMailDeliveryNeeded ? "ELO" : "EASY"));
        }

        public Archive GetArchive()
        {
            if (ScanImages == null)
                return GetDefaultArchive();

            if (ScanImages.None())
                XmlLoadScanImages();

            var firstScanImage = ScanImages.FirstOrDefault(); 
            return firstScanImage?.ImageDocumentType != null ? firstScanImage.ImageDocumentType.Archive : GetDefaultArchive();
        }

        public void EnsureDocumentType()
        {
            if (ValidDocumentType || ScanImages == null)
                return;

            if (ScanImages.None())
                XmlLoadScanImages();

            if (ScanImages.None())
                return;

            SelectedDocumentType = ScanImages.First().ImageDocumentType;
        }

        [XmlIgnore]
        public bool BackgroundDeliveryDisabled
        {
            get
            {
                var archive = GetArchive();
                if (archive == null)
                    return true;

                return archive.BackgroundDeliveryDisabled;
            }
        }

        #endregion

        #endregion

        public ScanDocument()
        {
            EmailSendoutAgainCommand = new DelegateCommand(e => EmailSendoutAgain(), e => EmailCanSendoutAgain());
        }

        public bool EmailCanSendoutAgain()
        {
            var existingLogItem = DomainService.Threads.ArchiveBackgroundTask.LogItems.FirstOrDefault(logItem => logItem.DocumentID == DocumentID);
            if (existingLogItem == null)
                return false;

            return existingLogItem.MailDeliveryNeeded;
        }

        void EmailSendoutAgain()
        {
            ArchiveDeliveryDate = null;
            DomainService.Threads.ArchiveBackgroundTask.Enqueue(new ArchiveLogItem { DocumentID = DocumentID });
        }

        public static string GetDocumentPdfDirectoryName(string documentID)
        {
            var documentDirectoryName = DomainService.Repository.PdfDirectoryName;
            if (!Directory.Exists(documentDirectoryName))
                Directory.CreateDirectory(documentDirectoryName);

            return documentDirectoryName;
        }

        public string GetDocumentPdfDirectoryName()
        {
            return GetDocumentPdfDirectoryName(DocumentID);
        }

        public string GetDocumentPrivateDirectoryName(string documentID)
        {
            var directoryName = IsTemplate ? DomainService.Repository.TemplateSettingsDirectoryName : DomainService.Repository.UserSettingsDirectoryName;
            var documentDirectoryName = Path.Combine(directoryName, documentID);
            if (!Directory.Exists(documentDirectoryName))
                Directory.CreateDirectory(documentDirectoryName);

            return documentDirectoryName;
        }

        public string GetDocumentPrivateDirectoryName()
        {
            return GetDocumentPrivateDirectoryName(DocumentID);
        }

        public void ScanImageAdd(ScanImage scanImage, Bitmap image)
        {
            ScanImages.Add(scanImage);

            ScanImagesCount = ScanImages.Count;
            scanImage.ParentDocument = this;
            scanImage.Sort = ScanImages.Max(docs => docs.Sort) + 1;

            if (image != null)
                scanImage.SetImageAndStoreToFileCache(image);

            XmlSaveScanImages();

            PdfIsSynchronized = false;
        }

        public void ScanImageRemove(ScanImage scanImage)
        {
            ScanImages.Remove(scanImage);
            
            XmlSaveScanImages();

            PdfIsSynchronized = false;
        }

        void ResortScanImages()
        {
            var sortID = 0;
            ScanImages.ToList().OrderBy(i => i.Sort).ToList().ForEach(si => si.Sort = ++sortID);
        }

        public void XmlSaveScanImages()
        {
            if (ScanImages.Count == 0) 
                return;

            ResortScanImages();

            XmlService.XmlSerializeToPath(ScanImages, GetDocumentPrivateDirectoryName(), "ScanImages");
            CreateDebugHintTextFile();
        }

        void CreateDebugHintTextFile()
        {
            try { File.CreateText(Path.Combine(GetDocumentPrivateDirectoryName(), $"{FinNumber}.txt")); }
            catch
            {
                // ignored
            }
        }

        public void XmlLoadScanImages()
        {
            ScanImages = XmlService.XmlDeserializeFromPath<List<ScanImage>>(GetDocumentPrivateDirectoryName(), "ScanImages");

            ScanImages.ForEach(scanImage => scanImage.ParentDocument = this);
        }

        public string PdfDirectoryName => GetDocumentPdfDirectoryName();

        [XmlIgnore]
        public bool PdfPageCountIsValid { get; private set; }

        public bool CheckPdfPageCountIsValid(string documentTypeCode, int scanImagesCount)
        {
            if (documentTypeCode == null)
                return false;

            var documentType = DomainService.Repository.GetImageDocumentType(documentTypeCode);

            if (!BatchScanned)
            {
                PdfPageCountIsValid = true;
                PdfErrorGuid = (ValidFinNumber ? "" : FileService.CreateFriendlyGuid());
            }
            else
            {
                PdfPageCountIsValid = (documentType.EnforceExactPageCount == 0 || scanImagesCount == 0 || documentType.EnforceExactPageCount == scanImagesCount);
                PdfErrorGuid = (PdfPageCountIsValid && ValidFinNumber ? "" : ValidFinNumber ? FinNumber : FileService.CreateFriendlyGuid());
            }

            return PdfPageCountIsValid;
        }

        public string PdfGetFileName(string documentTypeCode, string directoryName, string extension)
        {
            var documentType = DomainService.Repository.GetImageDocumentType(documentTypeCode);
            var pdfFinNumber = FinNumber;
            if (documentType != null && documentType.UseFileNameAbbreviationMinimumLength
                && pdfFinNumber.IsNotNullOrEmpty() && pdfFinNumber.Length == 17)
            {
                // FIN = ursprünglich 17 Stellen
                // FIN = letzte 11 Stellen (von ursprünglich 17) und von diesen 11 Stellen dann die 9. Stelle löschen:
                pdfFinNumber = pdfFinNumber.Substring(6);
                pdfFinNumber = $"{pdfFinNumber.Substring(0, 8)}{pdfFinNumber.Substring(9)}";
            }

            if (!PdfPageCountIsValid || !ValidFinNumber)
                pdfFinNumber = $"FEHLER_{PdfErrorGuid}";

            return Path.Combine(directoryName, $"{pdfFinNumber}{documentTypeCode}.{extension}");
        }

        public string PdfGetFileName(string documentTypeCode)
        {
            return PdfGetFileName(documentTypeCode, PdfDirectoryName, "pdf");
        }

        public List<string> GetPdfFileNames()
        {
            if (ScanImages.Count == 0)
                XmlLoadScanImages();

            return ScanDocumentTypeCodes.Select(PdfGetFileName).ToList();
        }

        public void PdfSaveScanImages()
        {
            const string extension = "pdf";
            var directoryName = PdfDirectoryName;
            var oldFiles = Directory.GetFiles(directoryName, $"{FinNumber}*.{extension}").ToList();
            if (FinNumber.IsNotNullOrEmpty())
                foreach (var oldFile in oldFiles)
                {
                    var saveAgain = true;
                    while (saveAgain)
                    {
                        try
                        {
                            saveAgain = false;
                            if (!File.Exists(oldFile))
                                continue;

                            var tempFileName = oldFile.ToUpper().Replace("." + extension.ToUpper(), "~1." + extension.ToUpper());
                            File.Move(oldFile, tempFileName);
                            File.Move(tempFileName, oldFile);
                        }
                        catch
                        {
                            saveAgain = Tools.Confirm(
                                $"Achtung:\r\n\r\nDas PDF speichern dieses Dokuments ist aktuell nicht möglich, weil die PDF-Datei '{Path.GetFileName(oldFile)}' schreibgeschützt ist!\r\n\r\nBitte schließen Sie alle Anwendungen, die auf dieses Dokument zugreifen!\r\n\r\nJetzt erneut versuchen?");
                            if (!saveAgain)
                                return;
                        }
                    }
                }

            ScanDocumentTypeCodes.ForEach(
                docTypeCode =>
                {
                    var scanImagesOfThisCode = ScanImages
                                                .Where(image => image.ImageDocumentTypeCode == docTypeCode)
                                                .OrderBy(i => i.Sort)
                                                .Select(image => image.GetCachedImageFileName(false))
                                                .ToListOrEmptyList();

                    CheckPdfPageCountIsValid(docTypeCode, scanImagesOfThisCode.Count);

                    var pdfFileName = PdfGetFileName(docTypeCode, directoryName, extension);    

                    var errorMessage = "";

                    var pdfEncryptionHashedPassword = "";
                    var documentType = DomainService.Repository.GetImageDocumentType(docTypeCode);
                    if (documentType != null)
                        pdfEncryptionHashedPassword = documentType.PdfEncryptionHashedPassword;
                    if (pdfEncryptionHashedPassword.IsNotNullOrEmpty())
                        pdfEncryptionHashedPassword = CryptoMd5Service.Decrypt(pdfEncryptionHashedPassword, "ScanClient");

                    try { PdfDocumentFactory.ScanClientCreatePdfFromImages(scanImagesOfThisCode, pdfFileName, pdfEncryptionHashedPassword); }
                    catch(Exception e) { errorMessage = e.Message; }

                    if (!File.Exists(pdfFileName) || !string.IsNullOrEmpty(errorMessage))
                    {
                        Tools.AlertError(
                            $"Beim Erstellen der PDF-Datei '{Path.GetFileName(pdfFileName)}' ist ein Fehler aufgetreten:\r\n\r\nFehlermeldung:\r\n{errorMessage}");
                    }
                });

            PdfIsSynchronized = true;
        }
        
        /// <summary>
        /// Erzeugt eine Kopie des ScanDocument-Objekts mit seinen Grunddaten, 
        /// Alle Listen sind initial leer.
        /// </summary>        
        public ScanDocument Clone()
        {
            var newScanDoc = new ScanDocument
                {
                    KundenNr = KundenNr,
                    StandortCode = StandortCode,
                    DocumentID = Guid.NewGuid().ToString(),
                    CreateDate = DateTime.Now,
                    CreateUser = DomainService.Repository.UserName,
                    SelectedDocumentType = DomainService.Repository.GetImageDocumentType(DomainService.Repository.UserSettings.SelectedDocumentTypeCode),
                    FinNumber = ""
                };

            return newScanDoc;
        }
    }
}
