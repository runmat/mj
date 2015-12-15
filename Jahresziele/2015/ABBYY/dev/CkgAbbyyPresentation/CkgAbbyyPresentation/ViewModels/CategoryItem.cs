using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeneralTools.Models;
using GeneralTools.Services;
using Size = System.Windows.Size;
using Media = System.Windows.Media;
using WpfTools4.ViewModels;

namespace CkgAbbyyPresentation.ViewModels
{
    public class CategoryItem : ViewModelBase
    {
        private CategoryViewModel _parentCategoryViewModel;

        public CategoryViewModel ParentCategoryViewModel
        {
            get { return _parentCategoryViewModel; }
            set
            {
                _parentCategoryViewModel = value;
                SendPropertyChanged("ParentCategoryViewModel");
            }
        }

        private string _fullFileName;

        public string FullFileName
        {
            get { return _fullFileName; }
            set
            {
                _fullFileName = value;
                SendPropertyChanged("FullFileName");
            }
        }

        private string _shortFileName;

        public string ShortFileName
        {
            get { return _shortFileName; }
            set
            {
                _shortFileName = value;
                SendPropertyChanged("ShortFileName");
            }
        }

        private string _xmlFileName;

        public string XmlFileName
        {
            get { return _xmlFileName; }
            set
            {
                _xmlFileName = value;
                SendPropertyChanged("XmlFileName");
            }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                SendPropertyChanged("Name");
            }
        }

        private bool _isError;

        public bool IsError
        {
            get { return _isError; }
            set
            {
                _isError = value;
                SendPropertyChanged("IsError");
            }
        }

        public Media.ImageSource SelectedImageSource
        {
            get { return GetCachedImageSource(true); }
        }

        public Media.ImageSource GetCachedImageSource(bool useThumbnail)
        {
            return ImagingService.ImageSourceFromUri(new Uri(FullFileName));
        }
    }
}
