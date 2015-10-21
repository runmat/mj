using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using CarDocu.Services;
using CarDocu.ViewModels;
using Tools = WpfTools4.Services.Tools;

namespace CarDocu.UserControls.DocuGroup
{
    public partial class ucScanDocuRibbonSectionDocuArt 
    {
        public bool BigSelectionMode { get; set; }

        public double ItemsFontSize { get { return BigSelectionMode ? 16 : 12; } }

        public ucScanDocuRibbonSectionDocuArt()
        {
            InitializeComponent();

            TaskService.StartDelayedUiTask(500, ModifyProperties);

            SizeChanged += ScanDocuRibbonSectionDocuArtSizeChanged;
        }

        void ScanDocuRibbonSectionDocuArtSizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            ModifyProperties();
        }

        void ModifyProperties()
        {
            if (BigSelectionMode)
            {
                Lb.MaxWidth = this.ActualWidth;
                
                var uniformGrid = Tools.GetVisualChild<UniformGrid>(Lb);
                if (uniformGrid != null)
                    uniformGrid.Columns = 5;
            }
        }

        private int _oldSelectedIndex = -1;
        private void LbOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             var viewModel = (DataContext as DocuViewModel);
            if (viewModel == null)
                return;

            if (!viewModel.SelectedDocumentTypeChangeInvalid)
                _oldSelectedIndex = Lb.SelectedIndex;
            else
                Lb.SelectedIndex = _oldSelectedIndex;

            viewModel.SelectedDocumentTypeChangeInvalid = false;
        }
    }
}
