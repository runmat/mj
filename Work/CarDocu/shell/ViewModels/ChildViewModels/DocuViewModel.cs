using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using CarDocu.Services;
using WpfTools4.Commands;
using WpfTools4.ViewModels;
using CarDocu.Models;
using Size = System.Windows.Size;
using Media = System.Windows.Media;
using GeneralTools.Models;
using ZXing;
using Tools = CarDocu.Services.Tools;
// ReSharper disable RedundantUsingDirective
using WpfTools4.Services;
using TwainDotNet;
// ReSharper restore RedundantUsingDirective

namespace CarDocu.ViewModels
{
    public class DocuViewModel : ViewModelBase, IMainViewModelChild
    {
        public MainViewModel Parent { get; set; }

        private ScanDocument _scanDocument; 
        public ScanDocument ScanDocument 
        { 
            get { return _scanDocument; }
            set { _scanDocument = value; SendPropertyChanged("ScanDocument"); }
        }

        public ICommand ScanDocumentStartScanCommand { get; private set; }
        public ICommand ScanDocumentStartScanBatchCommand { get; private set; }
        public ICommand ScanDocumentSaveCommand { get; private set; }
        public ICommand ScanDocumentPdfShowCommand { get; private set; }
        public ICommand ScanPdfFolderOpenCommand { get; private set; }
        public ICommand ScanDocumentTemplateInsertCommand { get; private set; }

        public bool UiModeBatchScanOnly { get { return Parent.UiModeBatchScanOnly; } }
        
        const string TempRootPath = @"D:\Backup\Test";

        private string _title; 
        public string Title 
        { 
            get { return _title; }
            set { _title = value; SendPropertyChanged("Header"); }
        }

        private Media.ImageSource _scanImageSource;
        public Media.ImageSource ScanImageSource 
        { 
            get { return _scanImageSource; }
            set
            {
                _scanImageSource = value; 
                SendPropertyChanged("ScanImageSource");
            }
        }

        private ScanImage _selectedScanImage; 
        public ScanImage SelectedScanImage 
        { 
            get { return _selectedScanImage; }
            set
            {
                _selectedScanImage = value;
                SendPropertyChanged("SelectedScanImage");

                ScanImageSource = SelectedScanImage.ImageSource;

                if (SelectedScanImage != null)
                    TrySelectDocumentType(SelectedScanImage.ImageDocumentTypeCode);

                CalcSelectedScanImageSizeForUI();
            }
        }

        static private ObservableCollection<DocumentTypeViewModel> _scanDocumentTypes;
        public ObservableCollection<DocumentTypeViewModel> ScanDocumentTypes
        {
            get { return _scanDocumentTypes; }
            set { _scanDocumentTypes = value; SendPropertyChanged("ScanDocumentTypes"); }
        }

        private List<ScanDocument> _templatetems;
        public List<ScanDocument> TemplateItems
        {
            get
            {
                if (_templatetems != null)
                    return _templatetems;

                _templatetems = DomainService.Repository.ScanTemplateRepository.ScanDocuments.OrderBy(t => t.FinNumber).ToList();

                return _templatetems;
            }
        }

        static private double _selectedScanImageWidth; 
        public double SelectedScanImageWidth 
        { 
            get { return _selectedScanImageWidth; }
            set { _selectedScanImageWidth = value; SendPropertyChanged("SelectedScanImageWidth"); }
        }

        static private double _selectedScanImageHeight;
        public double SelectedScanImageHeight
        {
            get { return _selectedScanImageHeight; }
            set
            {
                _selectedScanImageHeight = value;
                DocumentTypesEditDetailsHeight = value - 235;
                SendPropertyChanged("SelectedScanImageHeight");
            }
        }

        private double _documentTypesEditDetailsHeight;

        public double DocumentTypesEditDetailsHeight
        {
            get { return _documentTypesEditDetailsHeight; }
            set
            {
                _documentTypesEditDetailsHeight = value;
                SendPropertyChanged("DocumentTypesEditDetailsHeight");
            }
        }

        static private ObservableCollection<DocumentType> _globalDocumentTypes;
        public ObservableCollection<DocumentType> GlobalDocumentTypes 
        {
            get { return _globalDocumentTypes; }
            set
            {
                _globalDocumentTypes = value; 
                SendPropertyChanged("GlobalDocumentTypes");
                SendPropertyChanged("GlobalDocumentTypesColumnCount");
            }
        }

        private int _docuArtListBoxMaxSavedWidth;
        private int _docuArtListBoxMaxWidth = 1000;
        public int DocuArtListBoxMaxWidth
        {
            get { return _docuArtListBoxMaxWidth; }
            set { _docuArtListBoxMaxWidth = value; SendPropertyChanged("DocuArtListBoxMaxWidth"); }
        }

        private bool _docuArtSelectionBigVisible = DomainService.Repository.UserSettings.UIDocuArtBigSelection; 
        public bool DocuArtSelectionBigVisible 
        { 
            get { return _docuArtSelectionBigVisible; }
            set
            {
                if (_docuArtSelectionBigVisible == value) return;

                _docuArtSelectionBigVisible = value; 
                SendPropertyChanged("DocuArtSelectionBigVisible");
                SendPropertyChanged("DocuArtSelectionSmallOpacity");

                Parent.NotifyDocuArtSelectionBigVisible(value);

                DomainService.Repository.UserSettings.UIDocuArtBigSelection = value;
                DomainService.Repository.UserSettingsSave();
            }
        }

        public double DocuArtSelectionSmallOpacity 
        { 
            get { return DocuArtSelectionBigVisible ? 0.2 : 1.0; }
        }

        public int GlobalDocumentTypesColumnCount 
        {
            get { return GlobalDocumentTypes.Count == 0 ? 1 : ((GlobalDocumentTypes.Count+2)/3); }
        }

// ReSharper disable InconsistentNaming
        private bool _dummyNoValidDocumentType_NeededForFinTextBoxFocus;
// ReSharper restore InconsistentNaming

        private bool _finTextBoxAutoFocus; 

        private bool _isStoredModel; 
        public bool IsStoredModel 
        { 
            get { return _isStoredModel; }
            set 
            { 
                _isStoredModel = value; 
                SendPropertyChanged("IsStoredModel");
                _finTextBoxAutoFocus = !value;
            }
        }

        public bool SelectedDocumentTypeChangeInvalid { get; set; }

        public DocumentType SelectedDocumentType 
        {
            get { return ScanDocument.SelectedDocumentType; }
            set
            {
                if (value == null)
                    return;
                
                if (ScanDocument.SelectedDocumentType != null && value.IsTemplate != ScanDocument.SelectedDocumentType.IsTemplate && ScanDocument.ScanImagesCount > 0)
                {
                    Tools.AlertError("Ein Wechsel zwischen Dokument- und Vorlagen-Modus ist nicht mehr möglich, wenn bereits Seiten gescannt wurden.");
                    SendPropertyChanged("SelectedDocumentType");
                    SelectedDocumentTypeChangeInvalid = true;
                    return;
                }

                ScanDocument.SelectedDocumentType = value;
                ScanDocument.IsTemplate = SelectedDocumentType.IsTemplate;
                SendPropertyChanged("SelectedDocumentType");

                if (_finTextBoxAutoFocus)
                {
                    _dummyNoValidDocumentType_NeededForFinTextBoxFocus = true;
                    SendPropertyChanged("ValidDocumentType");
                    _dummyNoValidDocumentType_NeededForFinTextBoxFocus = false;
                }
                SendPropertyChanged("ValidDocumentType");

                DomainService.Repository.UserSettings.SelectedDocumentTypeCode = ScanDocument.SelectedDocumentType.Code;
                DomainService.Repository.UserSettingsSave();

                SendPropertyChangedFin();
                SendPropertyChanged("CharacterCasing");
            }
        }

        public bool ModeTemplate { get { return ScanDocument != null && ScanDocument.IsTemplate; } }

        public bool ScanAppendAvailable { get { return !IsStoredModel || (ScanDocument != null && ScanDocument.IsTemplate); } }

        public string FinNumber 
        {
            get { return ScanDocument.FinNumber; }
            set
            {
                ScanDocument.FinNumber = value;
                SendPropertyChangedFin();
            }
        }

        public CharacterCasing CharacterCasing
        {
            get { return ScanDocument.FinNumberUppercase ? CharacterCasing.Upper : CharacterCasing.Normal; }
        }

        public bool ValidDocumentType
        {
            get { return !_dummyNoValidDocumentType_NeededForFinTextBoxFocus && ScanDocument.ValidDocumentType; }
        }

        public bool IsTestMode
        {
            get
            {
                if (!DomainService.DebugIsAdminEnvironment)
                    return false;

                return true;
            }
        }

        private BatchSummary _batchSummary = new BatchSummary();

        public BatchSummary BatchSummary
        {
            get { return _batchSummary; }
            set
            {
                _batchSummary = value;
                SendPropertyChanged("BatchSummary");
            }
        }
        public ObservableCollection<StatusMessage> StatusMessages { get { return DomainService.StatusMessages; } }


        private void SendPropertyChangedFin()
        {
            SendPropertyChanged("FinNumber");
            SendPropertyChanged("FinNumberBackgroundOk");
            SendPropertyChanged("FinNumberBackground");
            SendPropertyChanged("FinNumberForeground");
            SendPropertyChanged("FinNumberInputHintVisible");
        }

        public bool FinNumberInputHintVisible
        {
            get { return ScanDocument.ValidFinNumber; }
        }

        public bool FinNumberBackgroundOk
        {
            get { return string.IsNullOrEmpty(FinNumber) || ScanDocument.ValidFinNumber; }
        }

        public Media.Brush FinNumberBackground
        {
            get { return FinNumberBackgroundOk ? Media.Brushes.LightGoldenrodYellow : Media.Brushes.LightPink; }
        }

        public Media.Brush FinNumberForeground
        {
            get { return FinNumberBackgroundOk ? Media.Brushes.Blue : Media.Brushes.Red; }
        }

        public string LastScannedBarcodeType { get; private set; }

        public string LastScannedBarcodeValue { get; private set; }

        public DocuViewModel()
        {
            LastScannedBarcodeType = "";
            LastScannedBarcodeValue = "";
            SetCommandBindings();

            ReloadGlobalDocumentTypes();
            ReloadScanDocumentTypes();
        }

        void SetCommandBindings()
        {
            ScanDocumentStartScanCommand = new DelegateCommand(e => ScanDocumentStartScan(), e => CanScanDocumentStartScan());
            ScanDocumentStartScanBatchCommand = new DelegateCommand(e => ScanDocumentStartBatchScan(), e => CanScanDocumentStartBatchScan());
            ScanDocumentSaveCommand = new DelegateCommand(e => ScanDocumentSave(true), e => CanScanDocumentSave());
            ScanDocumentPdfShowCommand = new DelegateCommand(e => ScanDocumentPdfShow((string)e), e => CanScanDocumentPdfShow());
            ScanPdfFolderOpenCommand = new DelegateCommand(e => DomainService.Repository.ScanPdfFolderOpen());
            ScanDocumentTemplateInsertCommand = new DelegateCommand(e => ScanDocumentTemplateInsert((string)e), e => CanScanDocumentTemplateInsert());
        }

        public void TrySelectDocumentType(string documentTypeCodeToSelect)
        {
            var docTypeObject = DomainService.Repository.EnterpriseSettings.DocumentTypes.FirstOrDefault(docType => docType.Code == documentTypeCodeToSelect);
            if (docTypeObject != null)
            {
                ScanDocumentTypes.Where(docType => docType.DocumentType != docTypeObject).ToList().ForEach(docType => docType.SelectedScanImage = null);

                SelectedDocumentType = docTypeObject;
            }
        }

        public void RibbonSelectedTabChanged()
        {
            ReloadGlobalDocumentTypes();
            ReloadScanDocumentTypes();

            Parent.MainWindowSizeChangedRefresh();
            
            Parent.AllDocusViewModel.ModeScanItems = !Parent.AllDocusViewModel.ModeScanItems;
            Parent.AllDocusViewModel.ModeScanItems = !Parent.AllDocusViewModel.ModeScanItems;
        }

        private Size _windowSize;

        private static double GetMaxHeight(Size size)
        {
             return size.Height - 210;
        }

        public void MainWindowSizeChanged(Size newSize)
        {
            _windowSize = newSize;
            SelectedScanImageHeight = GetMaxHeight(_windowSize);

            CalcSelectedScanImageSizeForUI();
        }

        void CalcSelectedScanImageSizeForUI()
        { 
            if (SelectedScanImage == null)
                return;

            var img = SelectedScanImage.ImageSource;
            double calcedHeight;
            if (img.Height < img.Width)
                calcedHeight = (GetMaxHeight(_windowSize)) * (img.Height / img.Width);
            else
                calcedHeight = GetMaxHeight(_windowSize);

            SelectedScanImageHeight = calcedHeight;
            //SelectedScanImageWidth = SelectedScanImageHeight * img.Width / img.Height;

            _docuArtListBoxMaxSavedWidth = (int)(_windowSize.Width - 630);
            DocuArtListBoxMaxWidth = _docuArtListBoxMaxSavedWidth;
        }

        void ReloadGlobalDocumentTypes()
        {
            GlobalDocumentTypes = new ObservableCollection<DocumentType>(DomainService.Repository.EnterpriseSettings.DocumentTypes
                .Where(gd => !gd.IsTemplate || (gd.IsTemplate && !DomainService.Repository.LogonUser.BatchScanOnly))
                    .OrderByDescending(gd => gd.IsSystemInternal)
                        .ThenBy(gd => gd.Name));
        }

        void ReloadScanDocumentTypes()
        {
            if (ScanDocument == null)
            {
                ScanDocumentTypes = new ObservableCollection<DocumentTypeViewModel>();
                return;
            }

            var scannedDocTypCodes = ScanDocument.ScanImages.GroupBy(scanImage => scanImage.ImageDocumentTypeCode).Select(g => g.Key);

            var scannedDocTypes = GlobalDocumentTypes.Where(gdt => scannedDocTypCodes.Contains(gdt.Code));

            var scannedDocTypeViewModels = scannedDocTypes.Select(dt => new DocumentTypeViewModel{ Parent = this, DocumentType = dt });

            ScanDocumentTypes = new ObservableCollection<DocumentTypeViewModel>(scannedDocTypeViewModels);

            ScanDocumentTypes.ToList().ForEach(scanDocType => scanDocType.RefreshScanImages());
        }

        bool CanScanDocumentStartScan()
        {
            return ScanDocument.ValidFinNumber && SelectedDocumentType != null;
        }

        bool CanScanDocumentStartBatchScan()
        {
            return  SelectedDocumentType != null && SelectedDocumentType.IsBatchScanAllowed;
        }

        bool ScanAppendAllowed()
        {
            var existingArchive = ScanDocument.GetArchive();

            if (ScanDocument.ScanImagesCount == 0)
                return true;

            if (existingArchive != null && SelectedDocumentType.Archive != null
                    && SelectedDocumentType.Archive.ID.IsNotNullOrEmpty()
                    && existingArchive.ID != SelectedDocumentType.Archive.ID)
            {
                // ok we already have at least 1 page scanned ...
                // and the user changed meanwhile the "Archive Type"...
                // ==> change of "Archive Type" during scans is strongly forbidden!
                Tools.AlertError(string.Format(
                    "Achtung: Aktion erfolglos!\r\n\r\n" +
                    "Sie haben für dieses Dokument bereits Seiten mit einer Dokumenten-Art erfasst, \r\n" +
                    "deren Archiv-Typ vom aktuell eingestellten Archiv-Typ abweicht! \r\n\r\n" +
                    "Bitte stellen Sie sicher, dass Sie pro Dokument nur Dokumenten-Arten desselben Archiv-Typs scannen!"));
                return false;
            }

            return true;
        }

        void ScanDocumentStartScan()
        {
            if (!ScanAppendAllowed())
                return;

            BatchSummary.Available = false;

            if (!IsTestMode)
            {
                var scanSettings = DomainService.Repository.GlobalSettings.ScanSettings;
                try
                {
                    TwainService.Instance
                        .StartScan(new ResolutionSettings
                        {
                            Dpi = scanSettings.DPI,
                            ColourSetting = scanSettings.UseColor ? ColourSetting.Colour : ColourSetting.GreyScale
                        })
                        .PageScanned(ScanPageScanned)
                        .Complete(e => ScanComplete());
                }
                catch (Exception)
                {
                    MessageBox.Show("Fehler beim Zugriff auf den Scanner ... ist er angeschlossen und eingeschaltet?",
                        "Fehler beim Scanner Zugriff", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            else
                TestLoadMockImages();
        }

        void ScanDocumentStartBatchScan()
        {
            ScanDocument.ArchiveMailDeliveryNeeded = SelectedDocumentType.Archive.MailDeliveryNeeded;

            if (!ScanAppendAllowed())
                return;

            StatusMessages.Clear();

            BatchSummary.Available = true;
            BatchSummary.Title = "Stapel Scan läuft ...";
            BatchSummary.ResultsAvailable = false;
            BatchSummary.ResultsGoodItems = BatchSummary.ResultsBadItems = BatchSummary.ResultsTotalItems = 0;
            UpdateUI();

            if (!IsTestMode)
            {
                var scanSettings = DomainService.Repository.GlobalSettings.ScanSettings;
                try
                {
                    TwainService.Instance
                        .StartScan(new ResolutionSettings
                        {
                            Dpi = scanSettings.DPI,
                            ColourSetting = scanSettings.UseColor ? ColourSetting.Colour : ColourSetting.GreyScale
                        })
                        .PageScanned(BatchScanPageScanned)
                        .Complete(e => BatchScanComplete());
                }
                catch (Exception)
                {
                    MessageBox.Show("Fehler beim Zugriff auf den Scanner ... ist er angeschlossen und eingeschaltet?",
                        "Fehler beim Scanner Zugriff", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }

            }
            else
                TestLoadMockImages(true);            
        }

        void ScanPageScanned(Bitmap image)
        {
            var guid = Guid.NewGuid().ToString();

            var scanImage = new ScanImage
            {
                ImageID = guid,
                ImageDocumentTypeCode = SelectedDocumentType.Code,
                ImageDocumentTypeCodeSAP = SelectedDocumentType.SapCode,
            };

            ScanDocument.ScanImageAdd(scanImage, image);
            image.Dispose();

            ScanDocument.ArchiveMailDeliveryNeeded = SelectedDocumentType.Archive.MailDeliveryNeeded;

            SelectedScanImage = scanImage;
        }

        void BatchScanPageScanned(Bitmap image)
        {
            var guid = Guid.NewGuid().ToString();

            var scanImage = new ScanImage
            {
                ImageID = guid,
                ImageDocumentTypeCode = SelectedDocumentType.Code,
                ImageDocumentTypeCodeSAP = SelectedDocumentType.SapCode,
            };

            if (CheckImageHasNewValidBarcode(image))
            {
                // altes Dokument abschließen
                BatchFinishScanDocument();

                ScanDocument = ScanDocument.Clone();
                ScanDocument.FinNumber = LastScannedBarcodeValue;                    
            }
            
            ScanDocument.ScanImageAdd(scanImage, image);
           
            image.Dispose();
            
            SelectedScanImage = scanImage;

            Thread.Sleep(100);
        }

        bool CheckImageHasNewValidBarcode(Bitmap barcodeBitmap)
        {
            // create a barcode reader instance
            IBarcodeReader reader = new BarcodeReader();

            //reader.TryHarder = true;
            
            // Debugausgabe des gescannten Bildes
            var key = Guid.NewGuid().ToString();
            var tmpFile = DomainService.Repository.GlobalSettings.TempPath + "FullScannImage" + key + ".jpg";
            barcodeBitmap.Save(tmpFile, ScanImage.ImageFormat);
            var savedBarcodeBitmap = new Bitmap(tmpFile);

            var startBarcodeY = SelectedDocumentType.StartBarcodeY;
            if (startBarcodeY > 200)
                startBarcodeY -= 40;

            var x = SelectedDocumentType.TranslateXmmToPixel(SelectedDocumentType.StartBarcodeX, savedBarcodeBitmap.Width);
            var y = SelectedDocumentType.TranslateYmmToPixel(startBarcodeY, savedBarcodeBitmap.Height);
            var w = SelectedDocumentType.TranslateXmmToPixel(SelectedDocumentType.BarcodeWidth, savedBarcodeBitmap.Width);
            var h = SelectedDocumentType.TranslateYmmToPixel(SelectedDocumentType.BarcodeHeight, savedBarcodeBitmap.Height);
            if (w > barcodeBitmap.Width - x) w = barcodeBitmap.Width - x - 1;
            if (h > barcodeBitmap.Height - y) w = barcodeBitmap.Width - y - 1;

            var tmpBmp = savedBarcodeBitmap.Clone(new Rectangle(x, y, w, h), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            
            // Temporäre Ausgabe des Scan-Ergebnisses
            tmpBmp.Save(DomainService.Repository.GlobalSettings.TempPath + "Crop" + key + ".jpg", ScanImage.ImageFormat);

            // detect and decode the barcode inside the bitmap
            var result = reader.Decode(tmpBmp);

            savedBarcodeBitmap .Dispose();
            tmpBmp.Dispose();

            if (result == null)
                return false;

            // do something with the result
            LastScannedBarcodeType = result.BarcodeFormat.ToString();
            LastScannedBarcodeValue = result.Text;

            if (SelectedDocumentType.BarcodeType != LastScannedBarcodeType)
            {
                DomainService.StatusMessages.Add(new StatusMessage(StatusMessage.MessageType.Warning, "Ungültiger Barcodetyp wurde übersprungen."));
                return false;
            }

            if (SelectedDocumentType.BarcodeAlphanumericAllowed)
            {
                //erfolgreich gescannt!
                DomainService.StatusMessages.Add(new StatusMessage(StatusMessage.MessageType.BarcodeOk, "Barcode " + LastScannedBarcodeValue + " erkannt.")); 
                return true;
            }

            var barcodeValue = long.Parse(LastScannedBarcodeValue);
            if (SelectedDocumentType.BarcodeRangeStart <= barcodeValue && barcodeValue <= SelectedDocumentType.BarcodeRangeEnd)
                return true;

            DomainService.StatusMessages.Add(new StatusMessage(StatusMessage.MessageType.Warning, "Barcode ist außerhalb des aktuellen Nummernkreises!"));
            return false;
        }

        bool CanScanDocumentSave()
        {
            return ScanDocument.ValidFinNumber && ScanDocument.ScanImagesCount > 0;
        }

        void ScanDocumentSave(bool archiveToHistory = false, bool savePdf = true)
        {
            if (savePdf)
                ScanDocument.PdfSaveScanImages();

            if (ScanDocument.ScanImages.Any())
            {
                DomainService.Repository.ScanDocumentRepositoryTryAddScanDocument(ScanDocument);
                DomainService.Repository.ScanDocumentRepositorySave();
            }

            if (archiveToHistory)
            {
                DomainService.Repository.TryQueueNewItem(ScanDocument);

                EnsureNewScanDocu();
            }
        }

        void EnsureNewScanDocu()
        {
            // neues Dokument aus dem "Neu" Tab entladen, es geht nun über in die "Übersicht" Liste:
            Parent.NewDocuViewModel = null;

            Parent.EnsureNewScanDocu(BatchSummary);

            Parent.AllDocusViewModel.ReloadItems();
        }

        bool CanScanDocumentPdfShow()
        {
            return CanScanDocumentSave(); 
        }

        void ScanDocumentPdfShow(string commandParameter)
        {
            if (!ScanDocument.PdfIsSynchronized)
                ScanDocumentSave();

            ReloadScanDocumentTypes();

            DocumentTypeViewModel docTypeCode;
            if (!string.IsNullOrEmpty(commandParameter))
                docTypeCode = ScanDocumentTypes.FirstOrDefault(code => code.DocumentType.Code == commandParameter);
            else
            {
                if (ScanDocumentTypes.Count == 1)
                    docTypeCode = ScanDocumentTypes.FirstOrDefault();
                else
                {
                    Tools.Alert("Bitte wählen Sie in diesem Dropdown die Dokumentenart, für die Sie das PDF öffnen wollen.");
                    return;
                }
            }

            if (docTypeCode == null)
            {
                var globalCode = DomainService.Repository.EnterpriseSettings.DocumentTypes.FirstOrDefault(dt => dt.Code == commandParameter);
                var codeName = (globalCode != null ? globalCode.Name : commandParameter);
                Tools.AlertError(string.Format("Fehler: Dieses Dokument enthält keine Dokumentenart '{0}'.", codeName));
                return;
            }

            Process.Start(ScanDocument.PdfGetFileName(docTypeCode.DocumentType.Code));
        }

        bool CanScanDocumentTemplateInsert()
        {
            return ScanDocument.ValidFinNumber && !ScanDocument.IsTemplate;
        }

        private void ScanDocumentTemplateInsert(string commandParameter)
        {
            if (commandParameter.IsNullOrEmpty())
                return;

            if (!ScanAppendAllowed())
                return;

            var template = DomainService.Repository.ScanTemplateRepository.ScanDocuments.FirstOrDefault(t => t.DocumentID == commandParameter);
            if (template == null)
                return;

            template.XmlLoadScanImages();
            template.ScanImages.ForEach(templateScanImage => ScanPageScanned(new Bitmap(templateScanImage.GetCachedImageFileName(false))));
            
            ScanComplete();
        }

        void TestLoadMockImages(bool batchTest = false)
        {
            var testDirectory = new DirectoryInfo(TempRootPath);
            var imageFiles = testDirectory.GetFiles("*.jpg", SearchOption.TopDirectoryOnly);

            if (batchTest)
            {
                // Standardaufruf im aktuellen Thread
                //for (var i = 0; i < 50; i++)
                {
                    foreach (var file in imageFiles)
                    {
                        BatchScanPageScanned(new Bitmap(file.FullName));
                        Thread.Sleep(10);
                    }
                }
                BatchScanComplete();

                App.ClearTempFolders();
            }
            else
            {
                foreach (var file in imageFiles)
                {
                    ScanPageScanned(new Bitmap(file.FullName));
                }
                ScanComplete();
            }

            DomainService.Repository.UserSettingsSave();
        }

        public bool TestScanDocuments(ProgressBarOperation progressBarOperation)
        {
            try
            {
                var repeatings = 50;
                var testDirectory = new DirectoryInfo(TempRootPath);

                var allFilesCount = 0;
                foreach (var dir in testDirectory.GetDirectories())
                {
                    allFilesCount += dir.GetFiles().Length * repeatings;
                }

                progressBarOperation.Current = 0;
                progressBarOperation.Total = allFilesCount;
                progressBarOperation.Header = "Scantest";
                progressBarOperation.Details = "Scanne Dateien";
                progressBarOperation.ProgressInfoVisible = true;

                for (var i = 0; i < repeatings; i++)
                {
                    foreach (var dir in testDirectory.GetDirectories())
                    {
                        foreach (var file in dir.GetFiles())
                        {
                            if (file.Extension.ToLower() == ".jpg" || file.Extension.ToLower() == ".jpeg")
                            {
                                BatchScanPageScanned(new Bitmap(file.FullName));
                                Thread.Sleep(10);
                                progressBarOperation.Current++;
                                progressBarOperation.Details = "Scanne Datei " + progressBarOperation.Current + " von " + allFilesCount;
                            }
                        }
                    }
                }
                BatchScanComplete();

                App.ClearTempFolders();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        static void TestScanComplete(ProgressBarOperation progressBarOperation)
        {
            if (!progressBarOperation.TaskResult)
            {
                Tools.Alert("Der Scantest wurde abgebrochen!");
                return;
            }

            Tools.Alert("Der Scantest wurde erfolgreich durchgeführt!");
        }

        bool ScanComplete()
        {
            ReloadScanDocumentTypes();

            ScanDocument.BatchScanned = false;

            ScanDocumentSave(false, false);

            SendPropertyChanged("ScanDocument");

            GC.Collect();

            DomainService.Repository.UserSettingsSave();

            //return !Tools.Confirm("Weitere Seite?");
            return true;
        }

        void BatchFinishScanDocument()
        {
            ScanDocument.BatchScanned = true;

            if (ScanDocument.ScanImagesCount == 0)
                return;

            ScanDocument.ArchiveMailDeliveryNeeded = SelectedDocumentType.Archive.MailDeliveryNeeded;

            ScanDocumentSave(true);
            SendPropertyChanged("ScanDocument");

            var scanDocumentIsValid = (ScanDocument.ValidFinNumber && ScanDocument.PdfPageCountIsValid);

            ScanDocument.ScanImages.Clear();

            if (scanDocumentIsValid)
                BatchSummary.ResultsGoodItems++;
            else
                BatchSummary.ResultsBadItems++;

            BatchSummary.Title += ".";
            BatchSummary.ResultsTotalItems = BatchSummary.ResultsBadItems + BatchSummary.ResultsGoodItems;
            UpdateUI();
        }

        bool BatchScanComplete()
        {
            ReloadScanDocumentTypes();

            // altes Dokument abschließen
            BatchFinishScanDocument();

            ScanDocument = ScanDocument.Clone();
            ScanDocument.FinNumber = "";

            GC.Collect();

            DomainService.Repository.UserSettingsSave();

            BatchSummary.ResultsAvailable = true;
            BatchSummary.Title = "Stapel Scan beendet, Ergebnis:";
            UpdateUI();

            return true;
        }

        public void ShowBigDocuArtSelection()
        {
            DocuArtSelectionBigVisible = !DocuArtSelectionBigVisible;
        }

        public void NotifyDocuArtSelectionBigVisible(bool val)
        {
            DocuArtSelectionBigVisible = val;
        }

        void UpdateUI()
        {
            Dispatcher.CurrentDispatcher.Invoke(new Action(() => { }), DispatcherPriority.ContextIdle, null);
        }
    }
}
