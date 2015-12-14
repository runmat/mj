using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CkgAbbyyPresentation
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        [DllImport("USER32.DLL")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int uFlags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private bool _isActivated;

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            if (_isActivated)
                return;

            _isActivated = true;
            PreparePresentation();
        }

        void PreparePresentation()
        {
            var pi = new ProcessStartInfo
            {
                FileName = @"powerpnt.exe",
                WorkingDirectory = @"C:\Users\JenzenM\Documents",
                Arguments = "/S Test.pptx",
           };
            var process = Process.Start(pi);
            if (process == null)
                return;

            process.WaitForInputIdle(5000);
            var pWnd = process.MainWindowHandle;
            ShowWindow(pWnd, 0);

            SetParent(pWnd, Process.GetCurrentProcess().MainWindowHandle);
            SetWindowPos(pWnd, (IntPtr)0, 0, 0, (int)WindowsFormsHost.Width, (int)WindowsFormsHost.Height, 0);

            ShowWindow(pWnd, 1);

            Thread.Sleep(1000);
            process.WaitForInputIdle(5000);
            MessageBox.Show("OK");
            process.Kill();
        }

        private void MainWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PreparePresentation();
        }
    }
}
