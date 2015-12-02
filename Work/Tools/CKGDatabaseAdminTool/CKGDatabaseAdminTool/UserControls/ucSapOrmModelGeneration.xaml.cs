using System.Windows.Controls;
using CKGDatabaseAdminLib.ViewModels;

namespace CKGDatabaseAdminTool.UserControls
{
    /// <summary>
    /// Interaktionslogik für ucSapOrmModelGeneration.xaml
    /// </summary>
    public partial class ucSapOrmModelGeneration : UserControl
    {
        public ucSapOrmModelGeneration()
        {
            InitializeComponent();
        }

        private void dgBapis_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var sapOrmModelGenerationViewModel = DataContext as SapOrmModelGenerationViewModel;
            if (sapOrmModelGenerationViewModel != null)
            {
                sapOrmModelGenerationViewModel.dgBapis_OnSelectionChanged(e);
            }
        }
    }
}
