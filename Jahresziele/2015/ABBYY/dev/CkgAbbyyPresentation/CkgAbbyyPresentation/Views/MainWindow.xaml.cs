using System;
using System.Windows;
using CkgAbbyyPresentation.ViewModels;

namespace CkgAbbyyPresentation
{
    public partial class MainWindow 
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel(StartPresentation);
        }

        void StartPresentation(string presentationName)
        {
            MediaElement.Source = new Uri(presentationName);
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
