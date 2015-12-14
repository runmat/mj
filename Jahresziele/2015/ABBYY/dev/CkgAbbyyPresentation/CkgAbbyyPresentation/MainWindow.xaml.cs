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
using Microsoft.Office.Core;
using Powerpoint = Microsoft.Office.Interop.PowerPoint;

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
            var powerpnt = new Powerpoint.Application();
            var presentation = powerpnt.Presentations.Open(@"C:\Users\JenzenM\Documents\Test.pptx", MsoTriState.msoCTrue, MsoTriState.msoCTrue, MsoTriState.msoFalse);

            //presentation.SlideShowSettings.SlideShowName = Title;
            var window = presentation.SlideShowSettings.Run();

            SetParent((IntPtr)window.HWND, Process.GetCurrentProcess().MainWindowHandle);
            SetWindowPos((IntPtr)window.HWND, (IntPtr)0, 0, 0, (int)WindowsFormsHost.Width, (int)WindowsFormsHost.Height, 0);

            while (presentation.SlideShowWindow.View.State != Powerpoint.PpSlideShowState.ppSlideShowDone)
                Thread.Sleep(50);

            presentation.Close();
            powerpnt.Quit();

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            MessageBox.Show("OK");
        }

        private void MainWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PreparePresentation();
        }
    }
}
