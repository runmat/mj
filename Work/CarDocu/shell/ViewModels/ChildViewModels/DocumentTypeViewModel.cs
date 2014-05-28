using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CarDocu.Models;
using CarDocu.Services;
using WpfTools4.Commands;
using WpfTools4.ViewModels;

namespace CarDocu.ViewModels
{
    public class DocumentTypeViewModel : ViewModelBase
    {
        #region Properties

        public DocuViewModel Parent { get; set; }

        public DocumentType DocumentType { get; set; }

        public string DocumentTypeName { get { return DocumentType.Name; } }

        private ObservableCollection<ScanImage> _scanImages;
        public ObservableCollection<ScanImage> ScanImages
        {
            get { return _scanImages; }
            set { _scanImages = value; SendPropertyChanged("ScanImages"); }
        }

        private ScanImage _selectedScanImage;
        public ScanImage SelectedScanImage
        {
            get { return _selectedScanImage; }
            set
            {
                _selectedScanImage = value;
                SendPropertyChanged("SelectedScanImage");

                if (value != null)
                    Parent.SelectedScanImage = value;

                SendPropertyChanged("SelectedScanImageEditable");
            }
        }

        public bool SelectedScanImageEditable
        {
            get { return SelectedScanImage != null && Parent.ScanAppendAvailable; }
        }

        public ICommand ScanPageMoveLeftCommand { get; private set; }
        public ICommand ScanPageMoveRightCommand { get; private set; }
        public ICommand ScanPageDeleteCommand { get; private set; }
        public ICommand ScanPageRotateCommand { get; private set; }

        #endregion


        public DocumentTypeViewModel()
        {
            ScanPageMoveLeftCommand = new DelegateCommand(e => ScanPageMove(-1), e => CanScanPageMove(-1));
            ScanPageMoveRightCommand = new DelegateCommand(e => ScanPageMove(1), e => CanScanPageMove(1));
            ScanPageDeleteCommand = new DelegateCommand(e => ScanPageDelete());
            ScanPageRotateCommand = new DelegateCommand(e => ScanPageRotate());
        }

        int SelectedScanImageIndex { get { return SelectedScanImage == null ? -1 : ScanImages.IndexOf(SelectedScanImage); } }

        void ScanPageMove(int step)
        {
            ScanImages.Move(SelectedScanImageIndex, SelectedScanImageIndex + step);

            var sort = 0;
            ScanImages.ToList().ForEach(scanImage => scanImage.Sort = ++sort);

            Parent.ScanDocument.PdfIsSynchronized = false;
        }

        bool CanScanPageMove(int step)
        {
            if (step > 0)
                return SelectedScanImageIndex < ScanImages.Count - 1;

            return SelectedScanImageIndex > 0;
        }

        void ScanPageDelete()
        {
            if (!Tools.Confirm("Markierte Seite wirklich löschen?"))
                return;

            Parent.ScanDocument.ScanImageRemove(SelectedScanImage);
            ScanImages.Remove(SelectedScanImage);
        }

        void ScanPageRotate()
        {
            SelectedScanImage.Rotate();

            Parent.ScanDocument.XmlSaveScanImages();
            Parent.ScanDocument.PdfIsSynchronized = false;

            SendPropertyChanged("ScanImages");
            SelectedScanImage = SelectedScanImage;
            SelectedScanImage.SendPropertyChangedImageSource();
        }

        public void RefreshScanImages()
        {
            ScanImages = new ObservableCollection<ScanImage>(Parent.ScanDocument.ScanImages.OrderBy(i => i.Sort).Where(scanImage => scanImage.ImageDocumentTypeCode == DocumentType.Code));
        }
    }
}
