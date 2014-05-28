using CKGDatabaseAdminLib.ViewModels;

namespace CKGDatabaseAdminTool.UIServices
{
    public enum DialogTypes
    {
        DbSelection
    }

    public static class DialogService
    {
        public static bool? ShowDialog(ref MainViewModel vm, DialogTypes dialogType, object[] parameters = null)
        {
            bool? erg = false;

            switch (dialogType)
            {
                case DialogTypes.DbSelection:
                    var selectDbWindow = new SelectDbWindow {DataContext = vm };
                    erg = selectDbWindow.ShowDialog();
                    break;
            }

            return erg;
        }

    }
}
