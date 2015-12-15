using System;
using System.Collections.Generic;
using System.Linq;
using GeneralTools.Models;
using System.Collections.ObjectModel;
using WpfTools4.ViewModels;

namespace CkgAbbyyPresentation.ViewModels
{
    public class MainViewModel : ViewModelBase 
    {
        public static string RootFolder { get { return @"C:\Backup\ABBYY"; } }

        private ObservableCollection<CategoryViewModel> _categoryViewModels;

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
            }
        }

        public MainViewModel()
        {
            CategoryViewModels = new ObservableCollection<CategoryViewModel>
            {
                new CategoryViewModel
                {
                    Name = "ZBII",
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

            SelectedCategoryViewModel = CategoryViewModels[0];
        }
    }
}
