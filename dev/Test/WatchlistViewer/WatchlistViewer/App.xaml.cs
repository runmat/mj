using System.Linq;
using System.Management;
using System.Windows;
using System.Windows.Markup;

namespace WatchlistViewer
{
    public partial class App 
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            TestNetwork(false);
            return;

            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage("de-DE"))); 
            
            ProcessHelper.KillAllProcessesOf("WatchlistViewer");

            base.OnStartup(e);
        }

        void TestNetwork(bool enableWlan)
        {
            var wmiQuery = new SelectQuery("SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionId != NULL");
            var searchProcedure = new ManagementObjectSearcher(wmiQuery);
            foreach (var item in searchProcedure.Get().Cast<ManagementObject>().Where(item => new [] {"Ethernet", "WLAN"}.Contains((string)item["NetConnectionId"])))
            {
                var ps = item.Properties;

                var id = item["NetConnectionId"].ToString();
                var availability = (bool)item["NetEnabled"];

                if (id == "WLAN")
                    item.InvokeMethod(enableWlan ? "Enable" : "Disable", null);

                if (id == "Ethernet")
                    item.InvokeMethod(enableWlan ? "Disable" : "Enable", null);
            }
        }
    }
}
