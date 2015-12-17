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
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();
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
