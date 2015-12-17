using System;
using System.Collections.ObjectModel;
using WpfTools4.ViewModels;
using System.IO;
using System.Windows.Input;
using WpfTools4.Commands;

namespace CkgAbbyyPresentation.ViewModels
{
    public class MainViewModel : ViewModelBase 
    {
        public static string RootFolder { get { return @"C:\Backup\ABBYY"; } }

        private ObservableCollection<CategoryViewModel> _categoryViewModels;

        public Action<string> StartPresentation { get; set; }

        public ObservableCollection<CategoryViewModel> CategoryViewModels
        {
            get { return _categoryViewModels; }
            set
            {
                _categoryViewModels = value;
                SendPropertyChanged("CategoryViewModels");
            }
        }

        private CategoryViewModel _selectedCategoryViewModel;

        public CategoryViewModel SelectedCategoryViewModel
        {
            get { return _selectedCategoryViewModel; }
            set
            {
                _selectedCategoryViewModel = value;
                SendPropertyChanged("SelectedCategoryViewModel");
                SendPropertyChanged("SelectedCategoryPresentationLabel");
                SendPropertyChanged("SelectedCategoryPresentationTooltip");
            }
        }

        public string SelectedCategoryPresentationLabel { get { return string.Format("Fazit '{0}':", SelectedCategoryViewModel.Name); } }

        public string SelectedCategoryPresentationTooltip { get { return string.Format("Präsentation starten für Fazit '{0}'", SelectedCategoryViewModel.Name); } }

        public ICommand SelectedCategoryPresentationStartCommand { get; private set; }
        public ICommand TotalPresentationStartCommand { get; private set; }
        public ICommand IntroPresentationStartCommand { get; private set; }


        public MainViewModel(Action<string> startPresentation)
        {
            StartPresentation = startPresentation;

            CategoryViewModels = new ObservableCollection<CategoryViewModel>
            {
                new CategoryViewModel
                {
                    Name = "ZBI / ZBII",
                    FolderName = "ZBII",
                },
                new CategoryViewModel
                {
                    Name = "Strafzettel",
                    FolderName = "Strafzettel",
                },
                new CategoryViewModel
                {
                    Name = "Lieferantenrechnung",
                    FolderName = "Lieferantenrechnung",
                },
            };

            _selectedCategoryViewModel = CategoryViewModels[0];

            TryStartPresentation("Intro.mp4");

            SelectedCategoryPresentationStartCommand = new DelegateCommand(e => TryStartPresentation(string.Format("Fazit-{0}.mp4", _selectedCategoryViewModel.FolderName)));
            TotalPresentationStartCommand = new DelegateCommand(e => TryStartPresentation("Fazit-Gesamt.mp4"));
            IntroPresentationStartCommand = new DelegateCommand(e => TryStartPresentation("Intro.mp4"));
        }

        void TryStartPresentation(string presentationName)
        {
            if (StartPresentation != null)
                StartPresentation(Path.Combine(RootFolder, presentationName));
        }
    }
}
