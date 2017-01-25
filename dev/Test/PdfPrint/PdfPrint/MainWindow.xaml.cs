using WpfTools4.Services;

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
                Tools.Alert("Hinweis:\r\n\r\nAktive Hintergrund-Druckprozesse müssen noch abgeschlossen werden!\r\n\r\nBitte versuchen Sie in ein paar Augenblicken noch einmal die Anwendung zu schließen.");
                e.Cancel = true;
                return;
            }

            base.OnClosing(e);
        }
    }
}
