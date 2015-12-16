using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using CkgAbbyyPresentation.ViewModels;
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


        const string RootFolder = @"C:\Backup\ABBYY\ZBII";


        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();
        }

        void PreparePresentation()
        {
            var powerpnt = new Powerpoint.Application();
            var presentation = powerpnt.Presentations.Open(@"C:\Users\JenzenM\Documents\Test.pptx", MsoTriState.msoCTrue, MsoTriState.msoCTrue, MsoTriState.msoFalse);

            var powerPointProcess = Process.GetProcessesByName("powerpnt")[0];

            //presentation.SlideShowSettings.SlideShowName = Title;
            presentation.SlideShowSettings.ShowType = Powerpoint.PpSlideShowType.ppShowTypeWindow;
            presentation.SlideShowSettings.ShowScrollbar = MsoTriState.msoFalse;
            var window = presentation.SlideShowSettings.Run();
            ShowWindow(powerPointProcess.MainWindowHandle, 6);

            var hostWindow = Process.GetCurrentProcess().MainWindowHandle;
            

            SetParent((IntPtr)window.HWND, hostWindow);
            SetWindowPos((IntPtr)window.HWND, (IntPtr)0, 0, 0, (int)600, (int)400, 0);
            ShowWindow((IntPtr)window.HWND, 3);

            //SetWindowPos(hostWindow, (IntPtr)0, (int)Left, (int)Top, (int)1000, (int)800, 0);

            while (presentation.SlideShowWindow.View.State != Powerpoint.PpSlideShowState.ppSlideShowDone)
                Thread.Sleep(50);

            presentation.Close();
            powerpnt.Quit();
            powerPointProcess.Kill();

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            MessageBox.Show("OK");
        }

        private void MainWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MediaElement.Visibility = Visibility.Visible;
            MediaElement.Position = new TimeSpan();
            MediaElement.Play();
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            MediaElement.Visibility = Visibility.Collapsed;
        }
    }
}
