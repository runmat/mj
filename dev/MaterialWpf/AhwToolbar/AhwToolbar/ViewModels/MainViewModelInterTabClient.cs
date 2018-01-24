using System.Diagnostics.CodeAnalysis;
using System.Windows;
using Dragablz;

namespace AhwToolbar.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class MainViewModelInterTabClient : IInterTabClient
    {
        public INewTabHost<Window> GetNewHost(IInterTabClient interTabClient, object partition, TabablzControl source)
        {
            return new NewTabHost<Window>(Application.Current.MainWindow, source);
        }

        public TabEmptiedResponse TabEmptiedHandler(TabablzControl tabControl, Window window)
        {
            return TabEmptiedResponse.CloseWindowOrLayoutBranch;
        }
    }
}
