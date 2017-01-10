using System;
using System.Linq;
using System.Management;

namespace CkgNetworkSwitch
{
    public class NetworkService
    {
        private static void EnumNetworkAdapters(Action<ManagementObject, string> toDoAction)
        {
            var wmiQuery = new SelectQuery("SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionId != NULL");
            var searchProcedure = new ManagementObjectSearcher(wmiQuery);
            foreach (var item in searchProcedure.Get().Cast<ManagementObject>().Where(item => new[] { "Ethernet", "WLAN" }.Contains((string)item["NetConnectionId"])))
            {
                var id = item["NetConnectionId"].ToString();
                toDoAction(item,id);
            }
        }

        public static void EnableWlan(bool enableWlan)
        {
            EnumNetworkAdapters((item, id) =>
            {
                if (id == "WLAN")
                    item.InvokeMethod(enableWlan ? "Enable" : "Disable", null);

                if (id == "Ethernet")
                    item.InvokeMethod(enableWlan ? "Disable" : "Enable", null);
            });
        }

        public static bool IsWlanEnabled()
        {
            var wlanAvailability = false;

            EnumNetworkAdapters((item, id) =>
            {
                var availability = (bool)item["NetEnabled"];

                if (id == "WLAN")
                    wlanAvailability = availability;

            });

            return wlanAvailability;
        }
    }
}