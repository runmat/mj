using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using CarDocu.Models;
using CarDocu.Services;
using WpfTools4.Commands;
using WpfTools4.ViewModels;

namespace CarDocu.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Properties

        private AdminViewModel _adminViewModel;
        public AdminViewModel AdminViewModel => (_adminViewModel ?? (_adminViewModel = new AdminViewModel { Parent = this }));

        private DocuViewModel _newDocuViewModel;
        public DocuViewModel NewDocuViewModel
        {
            get { return _newDocuViewModel; }
            set { _newDocuViewModel = value; SendPropertyChanged("NewDocuViewModel"); }
        }

        private DocuViewModel _storedDocuViewModel;
        public DocuViewModel StoredDocuViewModel
        {
            get { return _storedDocuViewModel; }
            set { _storedDocuViewModel = value; SendPropertyChanged("StoredDocuViewModel"); }
        }

        private AllDocusViewModel _allDocusViewModel; 
        public AllDocusViewModel AllDocusViewModel => (_allDocusViewModel ?? (_allDocusViewModel = new AllDocusViewModel { Parent = this }));

        private BackstageViewModel _backstageViewModel;
        public BackstageViewModel BackstageViewModel => (_backstageViewModel ?? (_backstageViewModel = new BackstageViewModel()));

        public DomainUser LogonUser => DomainService.Repository.LogonUser;

        public bool UiModeBatchScanOnly => LogonUser.BatchScanOnly;

        public ICommand LocationSelectionToggleCommand { get; private set; }
        public ICommand LocationSelectionHideCommand { get; private set; }
        private bool _locationSelectionVisible; 
        public bool LocationSelectionVisible 
        { 
            get { return _locationSelectionVisible; }
            set { _locationSelectionVisible = value; SendPropertyChanged("LocationSelectionVisible"); }
        }

        public IEnumerable<CardocuBackgroundTask> CardocuBackgroundThreads => DomainService.Threads.BackgroundThreads;

        public static IEnumerable<DomainLocation> DomainLocations => DomainService.Repository.GlobalSettings.DomainLocations;

        public static MainWindow MainWindow => ((MainWindow) Application.Current.MainWindow);

        private bool _storedDocuTabVisible; 
        public bool StoredDocuTabVisible 
        { 
            get { return _storedDocuTabVisible; }
            set
            {
                if (_storedDocuTabVisible == value) return;

                _storedDocuTabVisible = value; 
                SendPropertyChanged("StoredDocuTabVisible");

                MainWindow.StoredDocuTab.Visibility = (StoredDocuTabVisible ? Visibility.Visible : Visibility.Collapsed);
                if (StoredDocuTabVisible)
                {
                    MainWindow.Ribbon.SelectedTabItem = MainWindow.StoredDocuTab;
                    MainWindowSizeChangedRefresh();
                }
                else
                {
                    MainWindow.Ribbon.SelectedTabItem = MainWindow.AllDocusTab;

                    AllDocusViewModel.RefreshSelectedItem(StoredDocuViewModel.ScanDocument);
                }
            }
        }

        // ReSharper disable PossibleUnintendedReferenceComparison
        public bool StoredDocuTabSelected => MainWindow.Ribbon.SelectedTabItem == MainWindow.StoredDocuTab;
        // ReSharper restore PossibleUnintendedReferenceComparison

        // ReSharper disable PossibleUnintendedReferenceComparison
        public bool NewDocuTabSelected => MainWindow.Ribbon.SelectedTabItem == MainWindow.NewDocuTab;
        // ReSharper restore PossibleUnintendedReferenceComparison

        #endregion

        public static MainViewModel Instance { get; private set; }

        public MainViewModel()
        {
            Instance = this;

            LocationSelectionToggleCommand = new DelegateCommand(e => LocationSelectionVisible = !LocationSelectionVisible);
            LocationSelectionHideCommand = new DelegateCommand(e => LocationSelectionVisible = false);

            LogonUser.PropertyChanged += (s, e) =>
                                             {
                                                 if (e.PropertyName == "DomainLocation")
                                                 {
                                                     LocationSelectionVisible = false;
                                                     
                                                     LogonUser.DomainLocationCode = LogonUser.DomainLocation.SapCode;
                                                     DomainService.Repository.GlobalSettingsSave();
                                                 }
                                             };
        }

        ~MainViewModel()
        {
            UIHintService.PersistConfirmationUIHintForUser();
        }

        public void RibbonSelectedTabChanged(bool modeNewDocument, bool? modeTemplateItems=null)
        {
            if (modeNewDocument)
            {
                if (!NewDocuTabSelected)
                    MainWindow.NewDocuTab.IsSelected = true;

                EnsureNewScanDocu();

                if (modeTemplateItems != null)
                {
                    // try to "select" the first global documentType with this mode (mode <=> "Template" or "ScanDocument")
                    var firstDocTypeOfThisMode = NewDocuViewModel.GlobalDocumentTypes.FirstOrDefault(gd => gd.IsTemplate == modeTemplateItems);
                    if (firstDocTypeOfThisMode != null)
                        NewDocuViewModel.TrySelectDocumentType(firstDocTypeOfThisMode.Code);
                }
            }

            if (!StoredDocuTabSelected)
            {
                StoredDocuTabVisible = false;
                //StoredDocuViewModel = null;
            }

            NewDocuViewModel?.RibbonSelectedTabChanged();
            StoredDocuViewModel?.RibbonSelectedTabChanged();

            GC.Collect();
        }

        public void EnsureNewScanDocu(BatchSummary batchSummary = null)
        {
            NewDocuViewModel = (NewDocuViewModel ?? new DocuViewModel
            {
                Parent = this,
                Title = "Neues Dokument / Vorlage",
                IsStoredModel = false,
                ScanDocument = new ScanDocument
                                   {
                                       StandortCode = LogonUser.DomainLocationCode,
                                       KundenNr = DomainService.Repository.GlobalSettings.SapSettings.KundenNr,

                                       DocumentID = Guid.NewGuid().ToString(),
                                       CreateDate = DateTime.Now,
                                       CreateUser = DomainService.Repository.UserName,

                                       SelectedDocumentType = DomainService.Repository.GetImageDocumentType(DomainService.Repository.UserSettings.SelectedDocumentTypeCode),

                                       FinNumber = "" 
                                   }
            });
            if (batchSummary != null)
                NewDocuViewModel.BatchSummary = batchSummary;

            MainWindowSizeChangedRefresh();
        }

        public void LoadStoredScanDocu(ScanDocument scanDocument)
        {
            scanDocument.XmlLoadScanImages();

            StoredDocuViewModel = new DocuViewModel
                                                {
                                                    Parent = this,
                                                    Title = "Dokument / Vorlage bearbeiten",
                                                    IsStoredModel = true,
                                                    ScanDocument = scanDocument
                                                };

            var firstScanImage = scanDocument.ScanImages.FirstOrDefault();
            if (firstScanImage != null)
            {
                // try to "select" the documentType of the first stored scan image as our "selected document type":
                StoredDocuViewModel.TrySelectDocumentType(firstScanImage.ImageDocumentTypeCode);
            }

            StoredDocuTabVisible = true;
        }

        public void MainWindowSizeChangedRefresh()
        {
            MainWindowSizeChanged(new Size(MainWindow.ActualWidth, MainWindow.ActualHeight));
        }

        public void MainWindowSizeChanged(Size newSize)
        {
            NewDocuViewModel?.MainWindowSizeChanged(newSize);
            StoredDocuViewModel?.MainWindowSizeChanged(newSize);
        }

        public void NotifyDocuArtSelectionBigVisible(bool val)
        {
            NewDocuViewModel?.NotifyDocuArtSelectionBigVisible(val);
            StoredDocuViewModel?.NotifyDocuArtSelectionBigVisible(val);
        }

        #region UI Help HInts

        private FixedDocumentSequence _xpsDocument; 
        public FixedDocumentSequence XpsDocument 
        { 
            get { return _xpsDocument; }
            set { _xpsDocument = value; SendPropertyChanged("XpsDocument"); }
        }

        public void XpsDocumentInit()
        {
            UIHintService.TryShowNextUIHintForUser(xpsDocument =>
                                                       {
                                                           XpsDocument = xpsDocument;
                                                           XpsDocumentVisible = true;
                                                       });
        }

        protected override void XpsDocumentOnHide()
        {
            UIHintService.PersistConfirmationUIHintForUser();

            BackstageViewModel.RefreshUserUIHintEntities();
        }

        #endregion
    }
}
