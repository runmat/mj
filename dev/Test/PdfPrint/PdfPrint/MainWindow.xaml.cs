using System.Windows;

namespace PdfPrint
{
    public partial class MainWindow
    {
        private readonly MainViewModel _vm;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = _vm = new MainViewModel();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (_vm.IsPrinting)
            {
                MainViewModel.RequestBusyHint = true;

                e.Cancel = true;
                return;
            }

            base.OnClosing(e);
        }
    }
}
