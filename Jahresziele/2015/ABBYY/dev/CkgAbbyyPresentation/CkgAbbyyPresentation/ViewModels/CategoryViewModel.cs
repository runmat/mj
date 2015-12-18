using System.Linq;
using GeneralTools.Models;
using System.Collections.Generic;
using System.IO;
using WpfTools4.ViewModels;

namespace CkgAbbyyPresentation.ViewModels
{
    public class CategoryViewModel : ViewModelBase
    {
        public string RootFolder { get { return MainViewModel.RootFolder; } }

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

        private string _folderName;

        public string FolderName
        {
            get { return _folderName; }
            set
            {
                _folderName = value;
                SendPropertyChanged("FolderName");
            }
        }

        private List<CategoryItem> _items;

        public List<CategoryItem> Items
        {
            get { return (_items ?? (_items = GetItems())); }
        }

        public int ItemsCount { get { return Items.Count; } }

        private CategoryItem _selectedItem;

        public CategoryItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                SendPropertyChanged("SelectedItem");
            }
        }

        List<CategoryItem> GetItems()
        {
            var list = Directory.GetFiles(Path.Combine(RootFolder, FolderName), "*.jpg", SearchOption.TopDirectoryOnly)
                                .Select(f => new CategoryItem
                                    {
                                        ParentCategoryViewModel = this,
                                        FullFileName = f,
                                        ShortFileName = Path.GetFileName(f),
                                        Name = Path.GetFileNameWithoutExtension(f),
                                    })
                                    .OrderBy(f => f.Name)
                                    .ToListOrEmptyList();

            SelectedItem = list[0];

            list.ForEach(SetXmlInfo);

            return list;
        }

        void SetXmlInfo(CategoryItem item)
        {
            var xmlPath = Path.Combine(RootFolder, FolderName, "export");
            var xmlFileName = Path.Combine(xmlPath, string.Format("{0}.xml", item.Name));
            if (File.Exists(xmlFileName))
            {
                item.XmlFileName = xmlFileName;
                return;
            }

            xmlFileName = Path.Combine(xmlPath, string.Format("_ERROR_{0}.xml", item.Name));
            if (File.Exists(xmlFileName))
            {
                item.IsError = true;
                item.XmlFileName = xmlFileName;
                return;
            }

            xmlFileName = Path.Combine(xmlPath, string.Format("_WARNING_{0}.xml", item.Name));
            if (File.Exists(xmlFileName))
            {
                item.IsWarning = true;
                item.XmlFileName = xmlFileName;
            }
        }
    }
}
