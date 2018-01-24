using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Input;
using AhwToolbar.ViewModels;
using Dragablz;

namespace AhwToolbar
{
    [ExcludeFromCodeCoverage]
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            var selectedItem = InitialTabablzControl.Items.Cast<HeaderedItemViewModel>().FirstOrDefault(i => i.IsSelected);
            if (selectedItem != null)
                InitialTabablzControl.SelectedItem = selectedItem;
        }

        private void InitialTabablzControl_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var model = DataContext as MainViewModel;
            model?.OnTabsChanged(InitialTabablzControl.GetOrderedHeaders().Select(t => t.Content.ToString()), ((HeaderedItemViewModel)InitialTabablzControl.SelectedItem).Header.ToString());
        }
    }
}
