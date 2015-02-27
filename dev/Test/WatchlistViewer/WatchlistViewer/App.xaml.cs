using System.Windows;
using System.Windows.Markup;

namespace WatchlistViewer
{
    public partial class App 
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage("de-DE"))); 
            
            ProcessHelper.KillAllProcessesOf("WatchlistViewer");

            base.OnStartup(e);
        }
    }
}
